using System;
using System.Data;
using System.Windows.Forms;

namespace QLDeAnPhanTan
{
    public partial class NhanVienForm : Form
    {
        private DatabaseHelper dbHelper;

        public NhanVienForm(DatabaseHelper helper)
        {
            InitializeComponent();
            dbHelper = helper;
        }

        private void NhanVienForm_Load(object sender, EventArgs e)
        {
            LoadDanhSachNhanVien();
            LoadDanhSachNhom();
        }

        /// <summary>
        /// Load danh sách nhân viên từ View toàn cục
        /// </summary>
        private void LoadDanhSachNhanVien()
        {
            try
            {
                DataTable dt = dbHelper.LoadToanCuc("nhanvien_view");
                dgvNhanVien.DataSource = dt;

                // Đặt tiêu đề cột
                if (dgvNhanVien.Columns.Count > 0)
                {
                    dgvNhanVien.Columns["manv"].HeaderText = "Mã NV";
                    dgvNhanVien.Columns["hoten"].HeaderText = "Họ tên";
                    dgvNhanVien.Columns["manhom"].HeaderText = "Mã nhóm";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load danh sách nhân viên: {ex.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load danh sách nhóm vào ComboBox
        /// </summary>
        private void LoadDanhSachNhom()
        {
            try
            {
                DataTable dt = dbHelper.LoadToanCuc("nhomnc_view");
                
                cboNhom.DataSource = dt;
                cboNhom.DisplayMember = "tennhom";
                cboNhom.ValueMember = "manhom";
                cboNhom.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load danh sách nhóm: {ex.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Khi chọn nhân viên trong DataGridView
        /// </summary>
        private void dgvNhanVien_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow != null && dgvNhanVien.CurrentRow.Index >= 0)
            {
                DataGridViewRow row = dgvNhanVien.CurrentRow;
                
                txtMaNV.Text = row.Cells["manv"].Value?.ToString() ?? "";
                txtHoTen.Text = row.Cells["hoten"].Value?.ToString() ?? "";
                
                string maNhom = row.Cells["manhom"].Value?.ToString() ?? "";
                cboNhom.SelectedValue = maNhom;

                // Không cho sửa mã nhân viên khi đã chọn
                txtMaNV.ReadOnly = true;
            }
        }

        /// <summary>
        /// Nút Làm mới - Clear form
        /// </summary>
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            cboNhom.SelectedIndex = -1;
            txtMaNV.ReadOnly = false;
            txtMaNV.Focus();
            dgvNhanVien.ClearSelection();
        }

        /// <summary>
        /// Nút Thêm nhân viên
        /// </summary>
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate dữ liệu
                if (string.IsNullOrWhiteSpace(txtMaNV.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã nhân viên!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaNV.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHoTen.Focus();
                    return;
                }

                if (cboNhom.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn nhóm nghiên cứu!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboNhom.Focus();
                    return;
                }

                string maNV = txtMaNV.Text.Trim();
                string hoTen = txtHoTen.Text.Trim();
                string maNhom = cboNhom.SelectedValue.ToString()!;

                // Xác nhận
                var result = MessageBox.Show(
                    $"Thêm nhân viên:\n" +
                    $"- Mã NV: {maNV}\n" +
                    $"- Họ tên: {hoTen}\n" +
                    $"- Nhóm: {maNhom}\n\n" +
                    $"Nhân viên sẽ được thêm vào Site chứa nhóm '{maNhom}'.\n" +
                    $"Bạn có chắc chắn?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dbHelper.ThemNhanVien(maNV, hoTen, maNhom);
                    MessageBox.Show("Thêm nhân viên thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    LoadDanhSachNhanVien();
                    btnLamMoi_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Nút Sửa nhân viên
        /// </summary>
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate
                if (string.IsNullOrWhiteSpace(txtMaNV.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần sửa!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHoTen.Focus();
                    return;
                }

                if (cboNhom.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn nhóm nghiên cứu!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboNhom.Focus();
                    return;
                }

                string maNV = txtMaNV.Text.Trim();
                string hoTen = txtHoTen.Text.Trim();
                string maNhom = cboNhom.SelectedValue.ToString()!;

                // Xác nhận
                var result = MessageBox.Show(
                    $"Cập nhật thông tin nhân viên '{maNV}'?\n\n" +
                    $"Lưu ý: Không thể chuyển nhân viên sang nhóm thuộc Site khác!",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dbHelper.SuaNhanVien(maNV, hoTen, maNhom);
                    MessageBox.Show("Cập nhật nhân viên thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    LoadDanhSachNhanVien();
                    btnLamMoi_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Nút Xóa nhân viên
        /// </summary>
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaNV.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maNV = txtMaNV.Text.Trim();
                string hoTen = txtHoTen.Text.Trim();

                // Xác nhận
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa nhân viên?\n\n" +
                    $"Mã NV: {maNV}\n" +
                    $"Họ tên: {hoTen}\n\n" +
                    $"Lưu ý: Không thể xóa nếu nhân viên đang tham gia đề án!",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    dbHelper.XoaNhanVien(maNV);
                    MessageBox.Show("Xóa nhân viên thành công!", "Thành công", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    LoadDanhSachNhanVien();
                    btnLamMoi_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
