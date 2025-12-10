using System;
using System.Data;
using System.Windows.Forms;

namespace QLDeAnPhanTan
{
    public partial class ThamGiaForm : Form
    {
        private DatabaseHelper dbHelper;

        public ThamGiaForm(DatabaseHelper db)
        {
            InitializeComponent();
            this.dbHelper = db;
        }

        private void ThamGiaForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDanhSachNhanVien();
                LoadDanhSachDean();
                LoadDanhSachThamGia();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load danh sách nhân viên vào ComboBox
        /// </summary>
        private void LoadDanhSachNhanVien()
        {
            try
            {
                DataTable dt = dbHelper.LayDanhSachTatCaNhanVien();
                cboNhanVien.DataSource = dt;
                cboNhanVien.DisplayMember = "manv";
                cboNhanVien.ValueMember = "manv";
                cboNhanVien.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load danh sách nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load danh sách đề án vào ComboBox
        /// </summary>
        private void LoadDanhSachDean()
        {
            try
            {
                DataTable dt = dbHelper.LayDanhSachTatCaDean();
                cboDean.DataSource = dt;
                cboDean.DisplayMember = "mada";
                cboDean.ValueMember = "mada";
                cboDean.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load danh sách đề án: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load danh sách tham gia vào DataGridView
        /// </summary>
        private void LoadDanhSachThamGia()
        {
            try
            {
                DataTable dt = dbHelper.LoadToanCuc("thamgia_view");
                dgvThamGia.DataSource = dt;

                // Đặt tiêu đề cột
                if (dgvThamGia.Columns["manv"] != null)
                    dgvThamGia.Columns["manv"].HeaderText = "Mã nhân viên";
                if (dgvThamGia.Columns["mada"] != null)
                    dgvThamGia.Columns["mada"].HeaderText = "Mã đề án";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load danh sách tham gia: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Khi chọn nhân viên, hiển thị thông tin
        /// </summary>
        private void cboNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNhanVien.SelectedIndex == -1)
            {
                txtHoTen.Clear();
                txtNhomNV.Clear();
                return;
            }

            try
            {
                DataRowView row = (DataRowView)cboNhanVien.SelectedItem;
                txtHoTen.Text = row["hoten"].ToString();
                txtNhomNV.Text = row["manhom"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị thông tin nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Khi chọn đề án, hiển thị thông tin
        /// </summary>
        private void cboDean_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDean.SelectedIndex == -1)
            {
                txtTenDA.Clear();
                txtNhomDA.Clear();
                return;
            }

            try
            {
                DataRowView row = (DataRowView)cboDean.SelectedItem;
                txtTenDA.Text = row["tenda"].ToString();
                txtNhomDA.Text = row["manhom"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị thông tin đề án: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Thêm nhân viên vào đề án
        /// Logic: Nhân viên và đề án phải cùng Site
        /// </summary>
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (cboNhanVien.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboNhanVien.Focus();
                    return;
                }

                if (cboDean.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn đề án!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboDean.Focus();
                    return;
                }

                string maNV = cboNhanVien.SelectedValue.ToString()!;
                string maDA = cboDean.SelectedValue.ToString()!;
                string nhomNV = txtNhomNV.Text;
                string nhomDA = txtNhomDA.Text;

                // Hiển thị cảnh báo nếu nhân viên và đề án thuộc nhóm khác nhau
                if (nhomNV != nhomDA)
                {
                    DialogResult result = MessageBox.Show(
                        $"Cảnh báo: Nhân viên '{maNV}' thuộc nhóm '{nhomNV}', " +
                        $"nhưng đề án '{maDA}' thuộc nhóm '{nhomDA}'.\n\n" +
                        $"Trong hệ thống phân tán, nhân viên và đề án phải cùng Site.\n" +
                        $"Bạn có chắc chắn muốn thêm?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.No)
                        return;
                }

                // Thêm tham gia
                dbHelper.ThemThamGia(maNV, maDA);

                MessageBox.Show("Thêm nhân viên tham gia đề án thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh lại danh sách
                LoadDanhSachThamGia();
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xóa nhân viên khỏi đề án
        /// </summary>
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvThamGia.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn bản ghi cần xóa trong danh sách!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maNV = dgvThamGia.SelectedRows[0].Cells["manv"].Value.ToString()!;
                string maDA = dgvThamGia.SelectedRows[0].Cells["mada"].Value.ToString()!;

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa nhân viên '{maNV}' khỏi đề án '{maDA}'?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dbHelper.XoaThamGia(maNV, maDA);
                    MessageBox.Show("Xóa thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadDanhSachThamGia();
                    LamMoi();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Khi chọn dòng trong DataGridView, hiển thị thông tin
        /// </summary>
        private void dgvThamGia_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvThamGia.SelectedRows.Count > 0)
            {
                try
                {
                    string maNV = dgvThamGia.SelectedRows[0].Cells["manv"].Value.ToString()!;
                    string maDA = dgvThamGia.SelectedRows[0].Cells["mada"].Value.ToString()!;

                    // Tìm và chọn trong ComboBox
                    for (int i = 0; i < cboNhanVien.Items.Count; i++)
                    {
                        DataRowView row = (DataRowView)cboNhanVien.Items[i];
                        if (row["manv"].ToString() == maNV)
                        {
                            cboNhanVien.SelectedIndex = i;
                            break;
                        }
                    }

                    for (int i = 0; i < cboDean.Items.Count; i++)
                    {
                        DataRowView row = (DataRowView)cboDean.Items[i];
                        if (row["mada"].ToString() == maDA)
                        {
                            cboDean.SelectedIndex = i;
                            break;
                        }
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Làm mới form
        /// </summary>
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void LamMoi()
        {
            cboNhanVien.SelectedIndex = -1;
            cboDean.SelectedIndex = -1;
            txtHoTen.Clear();
            txtNhomNV.Clear();
            txtTenDA.Clear();
            txtNhomDA.Clear();
            cboNhanVien.Focus();
        }
    }
}
