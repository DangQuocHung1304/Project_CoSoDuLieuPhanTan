using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace QLDeAnPhanTan
{
    public partial class MainForm : Form
    {
        private DatabaseHelper? dbHelper;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Chưa kết nối... Vui lòng kết nối đến CSDL từ menu Hệ thống";
        }

        /// <summary>
        /// Menu kết nối đến CSDL
        /// </summary>
        private void mnuKetNoi_Click(object sender, EventArgs e)
        {
            using (ConnectionForm frm = new ConnectionForm())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    dbHelper = new DatabaseHelper(frm.ServerName, frm.Username, frm.Password, frm.DatabaseName);
                    
                    if (dbHelper.TestConnection(out string message))
                    {
                        lblStatus.Text = message + $" (Server: {frm.ServerName}, DB: {frm.DatabaseName})";
                        MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        lblStatus.Text = message;
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dbHelper = null;
                    }
                }
            }
        }

        /// <summary>
        /// Menu Quản lý Nhân viên
        /// </summary>
        private void mnuQuanLyNhanVien_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            NhanVienForm frm = new NhanVienForm(dbHelper!);
            frm.ShowDialog();
            
            // Refresh dữ liệu nếu đang ở tab Quản lý
            if (tabControl1.SelectedTab == tabQuanLy)
            {
                btnLoadNhanVien_Click(sender, e);
            }
        }

        /// <summary>
        /// Kiểm tra kết nối trước khi thực hiện thao tác
        /// </summary>
        private bool CheckConnection()
        {
            if (dbHelper == null)
            {
                MessageBox.Show("Vui lòng kết nối đến CSDL trước!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        #region Tab Quản lý Dữ liệu

        private void btnLoadNhomNC_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            try
            {
                DataTable dt = dbHelper!.LoadToanCuc("nhomnc_view");
                dgvQuanLy.DataSource = dt;
                lblStatus.Text = $"Đã load {dt.Rows.Count} nhóm nghiên cứu từ View toàn cục";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadNhanVien_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            try
            {
                DataTable dt = dbHelper!.LoadToanCuc("nhanvien_view");
                dgvQuanLy.DataSource = dt;
                lblStatus.Text = $"Đã load {dt.Rows.Count} nhân viên từ View toàn cục";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadThamGia_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            try
            {
                DataTable dt = dbHelper!.LoadToanCuc("thamgia_view");
                dgvQuanLy.DataSource = dt;
                lblStatus.Text = $"Đã load {dt.Rows.Count} bản ghi tham gia từ View toàn cục";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadDean_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            try
            {
                DataTable dt = dbHelper!.LoadToanCuc("dean_view");
                dgvQuanLy.DataSource = dt;
                lblStatus.Text = $"Đã load {dt.Rows.Count} đề án từ View toàn cục";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Tab Truy vấn

        private void btnTruyVanMuc1_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            if (string.IsNullOrWhiteSpace(txtMaNhom.Text))
            {
                MessageBox.Show("Vui lòng nhập mã nhóm!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhom.Focus();
                return;
            }

            try
            {
                DataTable dt = dbHelper!.TruyVanMuc1(txtMaNhom.Text.Trim());
                dgvTruyVan.DataSource = dt;
                lblStatus.Text = $"Truy vấn Mức 1: Tìm thấy {dt.Rows.Count} đề án (Trong suốt phân mảnh)";
                
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy đề án nào phù hợp!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTruyVanMuc2_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            if (string.IsNullOrWhiteSpace(txtMaNhom.Text))
            {
                MessageBox.Show("Vui lòng nhập mã nhóm!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhom.Focus();
                return;
            }

            try
            {
                DataTable dt = dbHelper!.TruyVanMuc2(txtMaNhom.Text.Trim());
                dgvTruyVan.DataSource = dt;
                lblStatus.Text = $"Truy vấn Mức 2: Tìm thấy {dt.Rows.Count} đề án (Trong suốt vị trí)";
                
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy đề án nào phù hợp!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Tab Cập nhật

        private void txtMaNhomCapNhat_TextChanged(object sender, EventArgs e)
        {
            // Tự động xác định Site khi người dùng nhập mã nhóm
            if (!CheckConnection()) return;

            if (string.IsNullOrWhiteSpace(txtMaNhomCapNhat.Text))
            {
                lblSite.Text = "";
                return;
            }

            try
            {
                string maNhom = txtMaNhomCapNhat.Text.Trim();
                
                // Kiểm tra nhóm có tồn tại không (tùy theo database đang kết nối)
                DataTable dt = dbHelper!.LoadToanCuc("nhomnc_view");
                var row = dt.AsEnumerable().FirstOrDefault(r => r["manhom"].ToString()?.Trim() == maNhom);
                
                if (row != null)
                {
                    lblSite.Text = $"Nhóm đang ở: {row["tenphong"]}";
                }
                else
                {
                    lblSite.Text = "Không tìm thấy nhóm";
                }
            }
            catch
            {
                lblSite.Text = "Không xác định được Site";
            }
        }

        /// <summary>
        /// Khi chọn Site, tự động điền tên phòng (P1 hoặc P2)
        /// </summary>
        private void cboSiteThemNhom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSiteThemNhom.SelectedIndex == 0)
            {
                txtTenPhong.Text = "P1";
            }
            else if (cboSiteThemNhom.SelectedIndex == 1)
            {
                txtTenPhong.Text = "P2";
            }
            else
            {
                txtTenPhong.Clear();
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            if (string.IsNullOrWhiteSpace(txtMaNhomCapNhat.Text))
            {
                MessageBox.Show("Vui lòng nhập mã nhóm!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhomCapNhat.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenPhongMoi.Text))
            {
                MessageBox.Show("Vui lòng nhập tên phòng mới (P1 hoặc P2)!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenPhongMoi.Focus();
                return;
            }

            if (txtTenPhongMoi.Text.Trim().Length > 2)
            {
                MessageBox.Show("Tên phòng chỉ được tối đa 2 ký tự (VD: P1, P2)!\n\n" +
                    "Lưu ý: Cột 'tenphong' trong database chỉ có kiểu CHAR(2).", 
                    "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenPhongMoi.Focus();
                txtTenPhongMoi.SelectAll();
                return;
            }

            string tenPhongMoi = txtTenPhongMoi.Text.Trim().ToUpper();
            if (tenPhongMoi != "P1" && tenPhongMoi != "P2")
            {
                MessageBox.Show("Tên phòng phải là P1 hoặc P2!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenPhongMoi.Focus();
                txtTenPhongMoi.SelectAll();
                return;
            }

            try
            {
                string maNhom = txtMaNhomCapNhat.Text.Trim().ToUpper();

                // Xác định Site hiện tại (dựa vào database đang kết nối hoặc từ View)
                string tenPhongHienTai = "";
                try
                {
                    DataTable dt = dbHelper!.LoadToanCuc("nhomnc_view");
                    var row = dt.AsEnumerable().FirstOrDefault(r => r["manhom"].ToString()?.Trim() == maNhom);
                    if (row != null)
                    {
                        tenPhongHienTai = row["tenphong"].ToString()?.Trim() ?? "";
                    }
                }
                catch { }

                string siteHienTai = tenPhongHienTai == "P1" ? "Site P1" : tenPhongHienTai == "P2" ? "Site P2" : "Site không xác định";
                string siteMoi = tenPhongMoi == "P1" ? "Site P1" : "Site P2";

                // Xác nhận trước khi chuyển
                string confirmMessage;
                if (tenPhongHienTai == tenPhongMoi)
                {
                    confirmMessage = $"Nhóm '{maNhom}' đang ở {siteHienTai}.\n" +
                                    $"Bạn có muốn cập nhật tên phòng thành '{tenPhongMoi}'?";
                }
                else
                {
                    confirmMessage = $"CHUYỂN NHÓM GIỮA CÁC SITE\n\n" +
                                    $"Nhóm '{maNhom}' đang ở: {siteHienTai}\n" +
                                    $"Sẽ chuyển sang: {siteMoi}\n\n" +
                                    $"Hệ thống sẽ tự động chuyển:\n" +
                                    $"✓ Nhóm nghiên cứu\n" +
                                    $"✓ Nhân viên thuộc nhóm\n" +
                                    $"✓ Đề án của nhóm\n" +
                                    $"✓ Tham gia của nhân viên\n\n" +
                                    $"Bạn có chắc muốn thực hiện?";
                }

                var result = MessageBox.Show(confirmMessage, "Xác nhận", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dbHelper!.ChuyenNhomSangSiteKhac(maNhom, tenPhongMoi);
                    
                    if (tenPhongHienTai == tenPhongMoi)
                    {
                        MessageBox.Show("Cập nhật tên phòng thành công!", "Thành công", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblStatus.Text = $"Đã cập nhật tên phòng cho nhóm {maNhom} thành {tenPhongMoi}";
                    }
                    else
                    {
                        MessageBox.Show($"Chuyển nhóm thành công!\n\nNhóm '{maNhom}' đã được chuyển từ {siteHienTai} sang {siteMoi}", 
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblStatus.Text = $"Đã chuyển nhóm {maNhom} từ {siteHienTai} sang {siteMoi}";
                    }
                    
                    // Xóa dữ liệu sau khi thành công
                    txtMaNhomCapNhat.Clear();
                    txtTenPhongMoi.Clear();
                    lblSite.Text = "";
                    txtMaNhomCapNhat.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Thêm nhóm nghiên cứu mới
        /// </summary>
        private void btnThemNhom_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            // Validate dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(txtMaNhomMoi.Text))
            {
                MessageBox.Show("Vui lòng nhập mã nhóm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhomMoi.Focus();
                return;
            }

            if (txtMaNhomMoi.Text.Trim().Length > 4)
            {
                MessageBox.Show("Mã nhóm tối đa 4 ký tự (VD: NC01, NC04)!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNhomMoi.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenNhomMoi.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhóm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNhomMoi.Focus();
                return;
            }

            if (cboSiteThemNhom.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Site để thêm nhóm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboSiteThemNhom.Focus();
                return;
            }

            try
            {
                string maNhom = txtMaNhomMoi.Text.Trim().ToUpper();
                string tenNhom = txtTenNhomMoi.Text.Trim();
                string tenPhong = txtTenPhong.Text.Trim(); // P1 hoặc P2 (tự động điền)
                string linkServer = cboSiteThemNhom.SelectedIndex == 0 ? "LINK_P1" : "LINK_P2";
                string siteName = cboSiteThemNhom.SelectedIndex == 0 ? "Site P1" : "Site P2";

                // Xác nhận
                var result = MessageBox.Show(
                    $"Bạn có chắc muốn thêm nhóm?\n" +
                    $"- Mã nhóm: {maNhom}\n" +
                    $"- Tên nhóm: {tenNhom}\n" +
                    $"- Tên phòng: {tenPhong}\n" +
                    $"- Site: {siteName}",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dbHelper!.ThemNhomNghienCuu(maNhom, tenNhom, tenPhong, linkServer);
                    MessageBox.Show("Thêm nhóm nghiên cứu thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = $"Đã thêm nhóm {maNhom} ({tenNhom}) vào {siteName}";

                    // Xóa dữ liệu sau khi thêm thành công
                    txtMaNhomMoi.Clear();
                    txtTenNhomMoi.Clear();
                    txtTenPhong.Clear();
                    cboSiteThemNhom.SelectedIndex = -1;
                    txtMaNhomMoi.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Mở form quản lý tham gia đề án
        /// </summary>
        private void mnuQuanLyThamGia_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            ThamGiaForm thamGiaForm = new ThamGiaForm(dbHelper!);
            thamGiaForm.ShowDialog();
        }

        #endregion

        #region Tab Xem Site khác

        private void btnLoadNhomSiteKhac_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            if (cboChonSite.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Site!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string linkServer = cboChonSite.SelectedIndex == 0 ? "LINK_P1" : "LINK_P2";
                string dbName = cboChonSite.SelectedIndex == 0 ? "QLDeAn_P1" : "QLDeAn_P2";
                string siteName = cboChonSite.SelectedIndex == 0 ? "Site P1" : "Site P2";

                DataTable dt = dbHelper!.LoadDataFromRemoteSite(linkServer, dbName, "nhomnc");
                dgvSiteKhac.DataSource = dt;
                lblStatus.Text = $"Đã load {dt.Rows.Count} nhóm nghiên cứu từ {siteName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadNVSiteKhac_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            if (cboChonSite.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Site!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string linkServer = cboChonSite.SelectedIndex == 0 ? "LINK_P1" : "LINK_P2";
                string dbName = cboChonSite.SelectedIndex == 0 ? "QLDeAn_P1" : "QLDeAn_P2";
                string siteName = cboChonSite.SelectedIndex == 0 ? "Site P1" : "Site P2";

                DataTable dt = dbHelper!.LoadDataFromRemoteSite(linkServer, dbName, "nhanvien");
                dgvSiteKhac.DataSource = dt;
                lblStatus.Text = $"Đã load {dt.Rows.Count} nhân viên từ {siteName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadDASiteKhac_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            if (cboChonSite.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Site!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string linkServer = cboChonSite.SelectedIndex == 0 ? "LINK_P1" : "LINK_P2";
                string dbName = cboChonSite.SelectedIndex == 0 ? "QLDeAn_P1" : "QLDeAn_P2";
                string siteName = cboChonSite.SelectedIndex == 0 ? "Site P1" : "Site P2";

                DataTable dt = dbHelper!.LoadDataFromRemoteSite(linkServer, dbName, "dean");
                dgvSiteKhac.DataSource = dt;
                lblStatus.Text = $"Đã load {dt.Rows.Count} đề án từ {siteName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadTGSiteKhac_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            if (cboChonSite.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Site!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string linkServer = cboChonSite.SelectedIndex == 0 ? "LINK_P1" : "LINK_P2";
                string dbName = cboChonSite.SelectedIndex == 0 ? "QLDeAn_P1" : "QLDeAn_P2";
                string siteName = cboChonSite.SelectedIndex == 0 ? "Site P1" : "Site P2";

                DataTable dt = dbHelper!.LoadDataFromRemoteSite(linkServer, dbName, "thamgia");
                dgvSiteKhac.DataSource = dt;
                lblStatus.Text = $"Đã load {dt.Rows.Count} bản ghi tham gia từ {siteName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện Sửa dữ liệu từ Context Menu
        /// </summary>
        private void mnuSuaDuLieu_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            if (cboChonSite.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Site!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvSiteKhac.CurrentRow == null || dgvSiteKhac.DataSource == null)
            {
                MessageBox.Show("Vui lòng chọn dòng dữ liệu cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string linkServer = cboChonSite.SelectedIndex == 0 ? "LINK_P1" : "LINK_P2";
                string dbName = cboChonSite.SelectedIndex == 0 ? "QLDeAn_P1" : "QLDeAn_P2";
                string siteName = cboChonSite.SelectedIndex == 0 ? "Site P1" : "Site P2";
                
                DataTable dt = (DataTable)dgvSiteKhac.DataSource;
                int rowIndex = dgvSiteKhac.CurrentRow.Index;
                DataRow selectedRow = dt.Rows[rowIndex];

                // Xác định bảng và khóa chính dựa vào cột trong DataTable
                string tableName = "";
                string pkColumn = "";
                object? pkValue = null;

                if (dt.Columns.Contains("manhom") && !dt.Columns.Contains("manv") && !dt.Columns.Contains("mada"))
                {
                    // Bảng nhomnc
                    tableName = "nhomnc";
                    pkColumn = "manhom";
                    pkValue = selectedRow["manhom"];
                }
                else if (dt.Columns.Contains("manv") && !dt.Columns.Contains("mada"))
                {
                    // Bảng nhanvien
                    tableName = "nhanvien";
                    pkColumn = "manv";
                    pkValue = selectedRow["manv"];
                }
                else if (dt.Columns.Contains("mada") && !dt.Columns.Contains("manv"))
                {
                    // Bảng dean
                    tableName = "dean";
                    pkColumn = "mada";
                    pkValue = selectedRow["mada"];
                }
                else if (dt.Columns.Contains("manv") && dt.Columns.Contains("mada"))
                {
                    // Bảng thamgia
                    tableName = "thamgia";
                    MessageBox.Show("Chức năng sửa bảng Tham Gia đang được phát triển...", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    MessageBox.Show("Không xác định được bảng dữ liệu!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Hiển thị form chỉnh sửa đơn giản
                using (var editForm = new Form())
                {
                    editForm.Text = $"Sửa dữ liệu - {tableName} ({siteName})";
                    editForm.Size = new System.Drawing.Size(500, 400);
                    editForm.StartPosition = FormStartPosition.CenterParent;
                    editForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    editForm.MaximizeBox = false;
                    editForm.MinimizeBox = false;

                    var panel = new Panel { Dock = DockStyle.Fill, Padding = new System.Windows.Forms.Padding(20) };
                    var flowLayout = new FlowLayoutPanel 
                    { 
                        Dock = DockStyle.Fill, 
                        FlowDirection = FlowDirection.TopDown,
                        AutoScroll = true
                    };

                    // Tạo controls cho từng cột (trừ khóa chính và các cột đặc biệt)
                    var textBoxes = new System.Collections.Generic.Dictionary<string, TextBox>();
                    var excludedColumns = new System.Collections.Generic.HashSet<string>(System.StringComparer.OrdinalIgnoreCase)
                    {
                        "rowguid",      // ROWGUIDCOL - không được UPDATE
                        "timestamp",    // Timestamp column
                        "hoten"         // Computed column (nếu có)
                    };

                    foreach (DataColumn col in dt.Columns)
                    {
                        // Bỏ qua các cột đặc biệt (computed, readonly, rowguid...)
                        if (excludedColumns.Contains(col.ColumnName))
                            continue;

                        var lblField = new Label 
                        { 
                            Text = col.ColumnName + ":",
                            AutoSize = true,
                            Margin = new System.Windows.Forms.Padding(0, 10, 0, 5)
                        };
                        flowLayout.Controls.Add(lblField);

                        var txtField = new TextBox 
                        { 
                            Width = 400,
                            Text = selectedRow[col].ToString(),
                            ReadOnly = col.ColumnName == pkColumn, // Khóa chính không cho sửa
                            Margin = new System.Windows.Forms.Padding(0, 0, 0, 5)
                        };
                        flowLayout.Controls.Add(txtField);
                        textBoxes[col.ColumnName] = txtField;
                    }

                    // Nút Lưu và Hủy
                    var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 50 };
                    var btnSave = new Button { Text = "💾 Lưu", Width = 100, Height = 35, Left = 170 };
                    var btnCancel = new Button { Text = "❌ Hủy", Width = 100, Height = 35, Left = 280 };
                    btnCancel.Click += (s, ev) => editForm.DialogResult = DialogResult.Cancel;
                    btnSave.Click += (s, ev) =>
                    {
                        // Cập nhật giá trị vào DataRow (chỉ các cột có trong textBoxes)
                        foreach (var kvp in textBoxes)
                        {
                            string colName = kvp.Key;
                            if (dt.Columns.Contains(colName) && colName != pkColumn)
                            {
                                string newValue = kvp.Value.Text.Trim();
                                selectedRow[colName] = string.IsNullOrEmpty(newValue) ? DBNull.Value : (object)newValue;
                            }
                        }
                        editForm.DialogResult = DialogResult.OK;
                    };

                    btnPanel.Controls.Add(btnSave);
                    btnPanel.Controls.Add(btnCancel);
                    panel.Controls.Add(flowLayout);
                    panel.Controls.Add(btnPanel);
                    editForm.Controls.Add(panel);

                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // Gọi DatabaseHelper để cập nhật
                        if (dbHelper!.UpdateRemoteSite(linkServer, dbName, tableName, pkColumn, pkValue, selectedRow))
                        {
                            MessageBox.Show($"Đã cập nhật dữ liệu trên {siteName}!", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            // Refresh dữ liệu
                            RefreshCurrentTableData();
                        }
                        else
                        {
                            MessageBox.Show("Không có dữ liệu nào được cập nhật!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Xử lý sự kiện Xóa dữ liệu từ Context Menu
        /// </summary>
        private void mnuXoaDuLieu_Click(object sender, EventArgs e)
        {
            if (!CheckConnection()) return;

            if (cboChonSite.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Site!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvSiteKhac.CurrentRow == null || dgvSiteKhac.DataSource == null)
            {
                MessageBox.Show("Vui lòng chọn dòng dữ liệu cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string linkServer = cboChonSite.SelectedIndex == 0 ? "LINK_P1" : "LINK_P2";
                string dbName = cboChonSite.SelectedIndex == 0 ? "QLDeAn_P1" : "QLDeAn_P2";
                string siteName = cboChonSite.SelectedIndex == 0 ? "Site P1" : "Site P2";

                DataTable dt = (DataTable)dgvSiteKhac.DataSource;
                int rowIndex = dgvSiteKhac.CurrentRow.Index;
                DataRow selectedRow = dt.Rows[rowIndex];

                // Xác định bảng và khóa chính
                string tableName = "";
                string pkColumn = "";
                object? pkValue = null;
                string recordInfo = "";

                if (dt.Columns.Contains("manhom") && !dt.Columns.Contains("manv") && !dt.Columns.Contains("mada"))
                {
                    tableName = "nhomnc";
                    pkColumn = "manhom";
                    pkValue = selectedRow["manhom"];
                    recordInfo = $"Nhóm NC: {pkValue} - {selectedRow["tennhom"]}";
                }
                else if (dt.Columns.Contains("manv") && !dt.Columns.Contains("mada"))
                {
                    tableName = "nhanvien";
                    pkColumn = "manv";
                    pkValue = selectedRow["manv"];
                    recordInfo = $"Nhân viên: {pkValue} - {selectedRow["tennv"]}";
                }
                else if (dt.Columns.Contains("mada") && !dt.Columns.Contains("manv"))
                {
                    tableName = "dean";
                    pkColumn = "mada";
                    pkValue = selectedRow["mada"];
                    recordInfo = $"Đề án: {pkValue} - {selectedRow["tenda"]}";
                }
                else if (dt.Columns.Contains("manv") && dt.Columns.Contains("mada"))
                {
                    tableName = "thamgia";
                    pkColumn = "manv"; // Composite key, cần xử lý đặc biệt
                    MessageBox.Show("Chức năng xóa bảng Tham Gia đang được phát triển...", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    MessageBox.Show("Không xác định được bảng dữ liệu!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Xác nhận xóa
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa dữ liệu này trên {siteName}?\n\n{recordInfo}\n\n" +
                    "⚠️ Hệ thống sẽ kiểm tra ràng buộc Foreign Key trước khi xóa.",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string errorMessage;
                    if (dbHelper!.DeleteFromRemoteSite(linkServer, dbName, tableName, pkColumn, pkValue, out errorMessage))
                    {
                        MessageBox.Show($"Đã xóa dữ liệu trên {siteName}!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh dữ liệu
                        RefreshCurrentTableData();
                    }
                    else
                    {
                        MessageBox.Show(errorMessage, "Không thể xóa",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Refresh lại bảng dữ liệu hiện tại trong tab Xem Site khác
        /// </summary>
        private void RefreshCurrentTableData()
        {
            if (dgvSiteKhac.DataSource == null) return;

            DataTable dt = (DataTable)dgvSiteKhac.DataSource;

            // Xác định bảng nào đang hiển thị và gọi lại button tương ứng
            if (dt.Columns.Contains("manhom") && !dt.Columns.Contains("manv") && !dt.Columns.Contains("mada"))
            {
                btnLoadNhomSiteKhac_Click(this, EventArgs.Empty);
            }
            else if (dt.Columns.Contains("manv") && !dt.Columns.Contains("mada"))
            {
                btnLoadNVSiteKhac_Click(this, EventArgs.Empty);
            }
            else if (dt.Columns.Contains("mada") && !dt.Columns.Contains("manv"))
            {
                btnLoadDASiteKhac_Click(this, EventArgs.Empty);
            }
            else if (dt.Columns.Contains("manv") && dt.Columns.Contains("mada"))
            {
                btnLoadTGSiteKhac_Click(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
