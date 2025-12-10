namespace QLDeAnPhanTan
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabQuanLy = new System.Windows.Forms.TabPage();
            this.dgvQuanLy = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLoadDean = new System.Windows.Forms.Button();
            this.btnLoadThamGia = new System.Windows.Forms.Button();
            this.btnLoadNhanVien = new System.Windows.Forms.Button();
            this.btnLoadNhomNC = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabTruyVan = new System.Windows.Forms.TabPage();
            this.dgvTruyVan = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnTruyVanMuc2 = new System.Windows.Forms.Button();
            this.btnTruyVanMuc1 = new System.Windows.Forms.Button();
            this.txtMaNhom = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabCapNhat = new System.Windows.Forms.TabPage();
            this.tabXemSiteKhac = new System.Windows.Forms.TabPage();
            this.dgvSiteKhac = new System.Windows.Forms.DataGridView();
            this.contextMenuSiteKhac = new System.Windows.Forms.ContextMenuStrip();
            this.mnuSuaDuLieu = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuXoaDuLieu = new System.Windows.Forms.ToolStripMenuItem();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cboChonSite = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnLoadNhomSiteKhac = new System.Windows.Forms.Button();
            this.btnLoadNVSiteKhac = new System.Windows.Forms.Button();
            this.btnLoadDASiteKhac = new System.Windows.Forms.Button();
            this.btnLoadTGSiteKhac = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblSite = new System.Windows.Forms.Label();
            this.btnCapNhat = new System.Windows.Forms.Button();
            this.txtTenPhongMoi = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMaNhomCapNhat = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboSiteThemNhom = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnThemNhom = new System.Windows.Forms.Button();
            this.txtTenPhong = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTenNhomMoi = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMaNhomMoi = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuHeThong = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuKetNoi = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQuanLyNhanVien = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQuanLyThamGia = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabQuanLy.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuanLy)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabTruyVan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTruyVan)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabCapNhat.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabXemSiteKhac.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSiteKhac)).BeginInit();
            this.contextMenuSiteKhac.SuspendLayout();
            this.panel4.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabQuanLy);
            this.tabControl1.Controls.Add(this.tabTruyVan);
            this.tabControl1.Controls.Add(this.tabCapNhat);
            this.tabControl1.Controls.Add(this.tabXemSiteKhac);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(984, 537);
            this.tabControl1.TabIndex = 0;
            // 
            // tabQuanLy
            // 
            this.tabQuanLy.Controls.Add(this.dgvQuanLy);
            this.tabQuanLy.Controls.Add(this.panel1);
            this.tabQuanLy.Location = new System.Drawing.Point(4, 24);
            this.tabQuanLy.Name = "tabQuanLy";
            this.tabQuanLy.Padding = new System.Windows.Forms.Padding(3);
            this.tabQuanLy.Size = new System.Drawing.Size(976, 509);
            this.tabQuanLy.TabIndex = 0;
            this.tabQuanLy.Text = "Quản lý Dữ liệu";
            this.tabQuanLy.UseVisualStyleBackColor = true;
            // 
            // dgvQuanLy
            // 
            this.dgvQuanLy.AllowUserToAddRows = false;
            this.dgvQuanLy.AllowUserToDeleteRows = false;
            this.dgvQuanLy.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvQuanLy.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQuanLy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvQuanLy.Location = new System.Drawing.Point(3, 83);
            this.dgvQuanLy.Name = "dgvQuanLy";
            this.dgvQuanLy.ReadOnly = true;
            this.dgvQuanLy.RowTemplate.Height = 25;
            this.dgvQuanLy.Size = new System.Drawing.Size(970, 423);
            this.dgvQuanLy.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnLoadDean);
            this.panel1.Controls.Add(this.btnLoadThamGia);
            this.panel1.Controls.Add(this.btnLoadNhanVien);
            this.panel1.Controls.Add(this.btnLoadNhomNC);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(970, 80);
            this.panel1.TabIndex = 0;
            // 
            // btnLoadDean
            // 
            this.btnLoadDean.Location = new System.Drawing.Point(549, 40);
            this.btnLoadDean.Name = "btnLoadDean";
            this.btnLoadDean.Size = new System.Drawing.Size(120, 30);
            this.btnLoadDean.TabIndex = 4;
            this.btnLoadDean.Text = "Đề Án";
            this.btnLoadDean.UseVisualStyleBackColor = true;
            this.btnLoadDean.Click += new System.EventHandler(this.btnLoadDean_Click);
            // 
            // btnLoadThamGia
            // 
            this.btnLoadThamGia.Location = new System.Drawing.Point(387, 40);
            this.btnLoadThamGia.Name = "btnLoadThamGia";
            this.btnLoadThamGia.Size = new System.Drawing.Size(120, 30);
            this.btnLoadThamGia.TabIndex = 3;
            this.btnLoadThamGia.Text = "Tham Gia";
            this.btnLoadThamGia.UseVisualStyleBackColor = true;
            this.btnLoadThamGia.Click += new System.EventHandler(this.btnLoadThamGia_Click);
            // 
            // btnLoadNhanVien
            // 
            this.btnLoadNhanVien.Location = new System.Drawing.Point(225, 40);
            this.btnLoadNhanVien.Name = "btnLoadNhanVien";
            this.btnLoadNhanVien.Size = new System.Drawing.Size(120, 30);
            this.btnLoadNhanVien.TabIndex = 2;
            this.btnLoadNhanVien.Text = "Nhân Viên";
            this.btnLoadNhanVien.UseVisualStyleBackColor = true;
            this.btnLoadNhanVien.Click += new System.EventHandler(this.btnLoadNhanVien_Click);
            // 
            // btnLoadNhomNC
            // 
            this.btnLoadNhomNC.Location = new System.Drawing.Point(63, 40);
            this.btnLoadNhomNC.Name = "btnLoadNhomNC";
            this.btnLoadNhomNC.Size = new System.Drawing.Size(120, 30);
            this.btnLoadNhomNC.TabIndex = 1;
            this.btnLoadNhomNC.Text = "Nhóm NC";
            this.btnLoadNhomNC.UseVisualStyleBackColor = true;
            this.btnLoadNhomNC.Click += new System.EventHandler(this.btnLoadNhomNC_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(408, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hiển thị dữ liệu toàn cục (Mức 1 - Trong suốt phân mảnh)";
            // 
            // tabTruyVan
            // 
            this.tabTruyVan.Controls.Add(this.dgvTruyVan);
            this.tabTruyVan.Controls.Add(this.panel2);
            this.tabTruyVan.Location = new System.Drawing.Point(4, 24);
            this.tabTruyVan.Name = "tabTruyVan";
            this.tabTruyVan.Padding = new System.Windows.Forms.Padding(3);
            this.tabTruyVan.Size = new System.Drawing.Size(976, 509);
            this.tabTruyVan.TabIndex = 1;
            this.tabTruyVan.Text = "Truy vấn";
            this.tabTruyVan.UseVisualStyleBackColor = true;
            // 
            // dgvTruyVan
            // 
            this.dgvTruyVan.AllowUserToAddRows = false;
            this.dgvTruyVan.AllowUserToDeleteRows = false;
            this.dgvTruyVan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTruyVan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTruyVan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTruyVan.Location = new System.Drawing.Point(3, 153);
            this.dgvTruyVan.Name = "dgvTruyVan";
            this.dgvTruyVan.ReadOnly = true;
            this.dgvTruyVan.RowTemplate.Height = 25;
            this.dgvTruyVan.Size = new System.Drawing.Size(970, 353);
            this.dgvTruyVan.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnTruyVanMuc2);
            this.panel2.Controls.Add(this.btnTruyVanMuc1);
            this.panel2.Controls.Add(this.txtMaNhom);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(970, 150);
            this.panel2.TabIndex = 0;
            // 
            // btnTruyVanMuc2
            // 
            this.btnTruyVanMuc2.Location = new System.Drawing.Point(367, 100);
            this.btnTruyVanMuc2.Name = "btnTruyVanMuc2";
            this.btnTruyVanMuc2.Size = new System.Drawing.Size(200, 35);
            this.btnTruyVanMuc2.TabIndex = 4;
            this.btnTruyVanMuc2.Text = "Truy vấn Mức 2 (Trong suốt vị trí)";
            this.btnTruyVanMuc2.UseVisualStyleBackColor = true;
            this.btnTruyVanMuc2.Click += new System.EventHandler(this.btnTruyVanMuc2_Click);
            // 
            // btnTruyVanMuc1
            // 
            this.btnTruyVanMuc1.Location = new System.Drawing.Point(105, 100);
            this.btnTruyVanMuc1.Name = "btnTruyVanMuc1";
            this.btnTruyVanMuc1.Size = new System.Drawing.Size(220, 35);
            this.btnTruyVanMuc1.TabIndex = 3;
            this.btnTruyVanMuc1.Text = "Truy vấn Mức 1 (Trong suốt phân mảnh)";
            this.btnTruyVanMuc1.UseVisualStyleBackColor = true;
            this.btnTruyVanMuc1.Click += new System.EventHandler(this.btnTruyVanMuc1_Click);
            // 
            // txtMaNhom
            // 
            this.txtMaNhom.Location = new System.Drawing.Point(105, 60);
            this.txtMaNhom.Name = "txtMaNhom";
            this.txtMaNhom.Size = new System.Drawing.Size(150, 23);
            this.txtMaNhom.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Nhập mã nhóm:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(10, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(687, 38);
            this.label2.TabIndex = 0;
            this.label2.Text = "Cho biết mã đề án, tên đề án của các đề án thuộc nhóm ($manhom) \r\nmà có nhân viên của nhóm nghiên cứu khác tham gia";
            // 
            // tabCapNhat
            // 
            this.tabCapNhat.Controls.Add(this.panel3);
            this.tabCapNhat.Location = new System.Drawing.Point(4, 24);
            this.tabCapNhat.Name = "tabCapNhat";
            this.tabCapNhat.Size = new System.Drawing.Size(976, 509);
            this.tabCapNhat.TabIndex = 2;
            this.tabCapNhat.Text = "Cập nhật";
            this.tabCapNhat.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Controls.Add(this.lblSite);
            this.panel3.Controls.Add(this.btnCapNhat);
            this.panel3.Controls.Add(this.txtTenPhongMoi);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.txtMaNhomCapNhat);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(976, 270);
            this.panel3.TabIndex = 0;
            // 
            // lblSite
            // 
            this.lblSite.AutoSize = true;
            this.lblSite.ForeColor = System.Drawing.Color.Blue;
            this.lblSite.Location = new System.Drawing.Point(125, 125);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(0, 15);
            this.lblSite.TabIndex = 6;
            // 
            // btnCapNhat
            // 
            this.btnCapNhat.Location = new System.Drawing.Point(125, 150);
            this.btnCapNhat.Name = "btnCapNhat";
            this.btnCapNhat.Size = new System.Drawing.Size(150, 35);
            this.btnCapNhat.TabIndex = 5;
            this.btnCapNhat.Text = "Cập nhật";
            this.btnCapNhat.UseVisualStyleBackColor = true;
            this.btnCapNhat.Click += new System.EventHandler(this.btnCapNhat_Click);
            // 
            // txtTenPhongMoi
            // 
            this.txtTenPhongMoi.Location = new System.Drawing.Point(125, 90);
            this.txtTenPhongMoi.MaxLength = 2;
            this.txtTenPhongMoi.Name = "txtTenPhongMoi";
            this.txtTenPhongMoi.Size = new System.Drawing.Size(150, 23);
            this.txtTenPhongMoi.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 15);
            this.label6.TabIndex = 3;
            this.label6.Text = "Chuyển sang (P1/P2):";
            // 
            // txtMaNhomCapNhat
            // 
            this.txtMaNhomCapNhat.Location = new System.Drawing.Point(125, 55);
            this.txtMaNhomCapNhat.Name = "txtMaNhomCapNhat";
            this.txtMaNhomCapNhat.Size = new System.Drawing.Size(150, 23);
            this.txtMaNhomCapNhat.TabIndex = 2;
            this.txtMaNhomCapNhat.TextChanged += new System.EventHandler(this.txtMaNhomCapNhat_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Mã nhóm:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(10, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(485, 19);
            this.label4.TabIndex = 0;
            this.label4.Text = "Chuyển nhóm nghiên cứu giữa các Site (Cập nhật phân tán)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboSiteThemNhom);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.btnThemNhom);
            this.groupBox2.Controls.Add(this.txtTenPhong);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtTenNhomMoi);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtMaNhomMoi);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(500, 10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(450, 250);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Thêm nhóm nghiên cứu mới";
            // 
            // cboSiteThemNhom
            // 
            this.cboSiteThemNhom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSiteThemNhom.FormattingEnabled = true;
            this.cboSiteThemNhom.Items.AddRange(new object[] {
            "LINK_P1 (Site P1)",
            "LINK_P2 (Site P2)"});
            this.cboSiteThemNhom.Location = new System.Drawing.Point(120, 90);
            this.cboSiteThemNhom.Name = "cboSiteThemNhom";
            this.cboSiteThemNhom.Size = new System.Drawing.Size(200, 23);
            this.cboSiteThemNhom.TabIndex = 8;
            this.cboSiteThemNhom.SelectedIndexChanged += new System.EventHandler(this.cboSiteThemNhom_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 93);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 15);
            this.label7.TabIndex = 7;
            this.label7.Text = "Chọn Site:";
            // 
            // btnThemNhom
            // 
            this.btnThemNhom.Location = new System.Drawing.Point(120, 220);
            this.btnThemNhom.Name = "btnThemNhom";
            this.btnThemNhom.Size = new System.Drawing.Size(150, 35);
            this.btnThemNhom.TabIndex = 6;
            this.btnThemNhom.Text = "Thêm nhóm";
            this.btnThemNhom.UseVisualStyleBackColor = true;
            this.btnThemNhom.Click += new System.EventHandler(this.btnThemNhom_Click);
            // 
            // txtTenPhong
            // 
            this.txtTenPhong.Location = new System.Drawing.Point(120, 180);
            this.txtTenPhong.MaxLength = 3;
            this.txtTenPhong.Name = "txtTenPhong";
            this.txtTenPhong.ReadOnly = true;
            this.txtTenPhong.Size = new System.Drawing.Size(100, 23);
            this.txtTenPhong.TabIndex = 10;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(20, 183);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 15);
            this.label11.TabIndex = 9;
            this.label11.Text = "Tên phòng:";
            // 
            // txtTenNhomMoi
            // 
            this.txtTenNhomMoi.Location = new System.Drawing.Point(120, 140);
            this.txtTenNhomMoi.MaxLength = 50;
            this.txtTenNhomMoi.Name = "txtTenNhomMoi";
            this.txtTenNhomMoi.Size = new System.Drawing.Size(300, 23);
            this.txtTenNhomMoi.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 143);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 15);
            this.label8.TabIndex = 4;
            this.label8.Text = "Tên nhóm:";
            // 
            // txtMaNhomMoi
            // 
            this.txtMaNhomMoi.Location = new System.Drawing.Point(120, 50);
            this.txtMaNhomMoi.MaxLength = 4;
            this.txtMaNhomMoi.Name = "txtMaNhomMoi";
            this.txtMaNhomMoi.Size = new System.Drawing.Size(150, 23);
            this.txtMaNhomMoi.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 15);
            this.label9.TabIndex = 2;
            this.label9.Text = "Mã nhóm:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.label10.ForeColor = System.Drawing.Color.Green;
            this.label10.Location = new System.Drawing.Point(20, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(400, 30);
            this.label10.TabIndex = 1;
            this.label10.Text = "Thêm nhóm mới vào Site P1 hoặc P2\r\n(Mã nhóm: NC01-NC99, Tên nhóm: tùy ý)\r\n(Tên phòng tự động điền P1/P2 theo Site chọn)";
            this.label10.Visible = false;
            // 
            // tabXemSiteKhac
            // 
            this.tabXemSiteKhac.Controls.Add(this.dgvSiteKhac);
            this.tabXemSiteKhac.Controls.Add(this.panel4);
            this.tabXemSiteKhac.Location = new System.Drawing.Point(4, 24);
            this.tabXemSiteKhac.Name = "tabXemSiteKhac";
            this.tabXemSiteKhac.Size = new System.Drawing.Size(976, 509);
            this.tabXemSiteKhac.TabIndex = 3;
            this.tabXemSiteKhac.Text = "Xem Site khác";
            this.tabXemSiteKhac.UseVisualStyleBackColor = true;
            // 
            // dgvSiteKhac
            // 
            this.dgvSiteKhac.AllowUserToAddRows = false;
            this.dgvSiteKhac.AllowUserToDeleteRows = false;
            this.dgvSiteKhac.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSiteKhac.ContextMenuStrip = this.contextMenuSiteKhac;
            this.dgvSiteKhac.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSiteKhac.Location = new System.Drawing.Point(0, 100);
            this.dgvSiteKhac.Name = "dgvSiteKhac";
            this.dgvSiteKhac.ReadOnly = true;
            this.dgvSiteKhac.RowTemplate.Height = 25;
            this.dgvSiteKhac.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSiteKhac.Size = new System.Drawing.Size(976, 409);
            this.dgvSiteKhac.TabIndex = 1;
            // 
            // contextMenuSiteKhac
            // 
            this.contextMenuSiteKhac.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSuaDuLieu,
            this.mnuXoaDuLieu});
            this.contextMenuSiteKhac.Name = "contextMenuSiteKhac";
            this.contextMenuSiteKhac.Size = new System.Drawing.Size(181, 70);
            // 
            // mnuSuaDuLieu
            // 
            this.mnuSuaDuLieu.Name = "mnuSuaDuLieu";
            this.mnuSuaDuLieu.Size = new System.Drawing.Size(180, 22);
            this.mnuSuaDuLieu.Text = "✏️ Sửa dữ liệu";
            this.mnuSuaDuLieu.Click += new System.EventHandler(this.mnuSuaDuLieu_Click);
            // 
            // mnuXoaDuLieu
            // 
            this.mnuXoaDuLieu.Name = "mnuXoaDuLieu";
            this.mnuXoaDuLieu.Size = new System.Drawing.Size(180, 22);
            this.mnuXoaDuLieu.Text = "❌ Xóa dữ liệu";
            this.mnuXoaDuLieu.Click += new System.EventHandler(this.mnuXoaDuLieu_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.btnLoadTGSiteKhac);
            this.panel4.Controls.Add(this.btnLoadDASiteKhac);
            this.panel4.Controls.Add(this.btnLoadNVSiteKhac);
            this.panel4.Controls.Add(this.btnLoadNhomSiteKhac);
            this.panel4.Controls.Add(this.cboChonSite);
            this.panel4.Controls.Add(this.label13);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(976, 100);
            this.panel4.TabIndex = 0;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.label14.ForeColor = System.Drawing.Color.Blue;
            this.label14.Location = new System.Drawing.Point(250, 15);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(400, 15);
            this.label14.TabIndex = 6;
            this.label14.Text = "Xem và quản lý dữ liệu từ Site khác (Truy vấn phân tán xuyên Site)";
            // 
            // btnLoadTGSiteKhac
            // 
            this.btnLoadTGSiteKhac.Location = new System.Drawing.Point(600, 55);
            this.btnLoadTGSiteKhac.Name = "btnLoadTGSiteKhac";
            this.btnLoadTGSiteKhac.Size = new System.Drawing.Size(140, 30);
            this.btnLoadTGSiteKhac.TabIndex = 5;
            this.btnLoadTGSiteKhac.Text = "Load Tham Gia";
            this.btnLoadTGSiteKhac.UseVisualStyleBackColor = true;
            this.btnLoadTGSiteKhac.Click += new System.EventHandler(this.btnLoadTGSiteKhac_Click);
            // 
            // btnLoadDASiteKhac
            // 
            this.btnLoadDASiteKhac.Location = new System.Drawing.Point(450, 55);
            this.btnLoadDASiteKhac.Name = "btnLoadDASiteKhac";
            this.btnLoadDASiteKhac.Size = new System.Drawing.Size(140, 30);
            this.btnLoadDASiteKhac.TabIndex = 4;
            this.btnLoadDASiteKhac.Text = "Load Đề Án";
            this.btnLoadDASiteKhac.UseVisualStyleBackColor = true;
            this.btnLoadDASiteKhac.Click += new System.EventHandler(this.btnLoadDASiteKhac_Click);
            // 
            // btnLoadNVSiteKhac
            // 
            this.btnLoadNVSiteKhac.Location = new System.Drawing.Point(300, 55);
            this.btnLoadNVSiteKhac.Name = "btnLoadNVSiteKhac";
            this.btnLoadNVSiteKhac.Size = new System.Drawing.Size(140, 30);
            this.btnLoadNVSiteKhac.TabIndex = 3;
            this.btnLoadNVSiteKhac.Text = "Load Nhân Viên";
            this.btnLoadNVSiteKhac.UseVisualStyleBackColor = true;
            this.btnLoadNVSiteKhac.Click += new System.EventHandler(this.btnLoadNVSiteKhac_Click);
            // 
            // btnLoadNhomSiteKhac
            // 
            this.btnLoadNhomSiteKhac.Location = new System.Drawing.Point(150, 55);
            this.btnLoadNhomSiteKhac.Name = "btnLoadNhomSiteKhac";
            this.btnLoadNhomSiteKhac.Size = new System.Drawing.Size(140, 30);
            this.btnLoadNhomSiteKhac.TabIndex = 2;
            this.btnLoadNhomSiteKhac.Text = "Load Nhóm NC";
            this.btnLoadNhomSiteKhac.UseVisualStyleBackColor = true;
            this.btnLoadNhomSiteKhac.Click += new System.EventHandler(this.btnLoadNhomSiteKhac_Click);
            // 
            // cboChonSite
            // 
            this.cboChonSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChonSite.FormattingEnabled = true;
            this.cboChonSite.Items.AddRange(new object[] {
            "LINK_P1 (Site P1)",
            "LINK_P2 (Site P2)"});
            this.cboChonSite.Location = new System.Drawing.Point(100, 12);
            this.cboChonSite.Name = "cboChonSite";
            this.cboChonSite.Size = new System.Drawing.Size(140, 23);
            this.cboChonSite.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label13.Location = new System.Drawing.Point(10, 13);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(83, 19);
            this.label13.TabIndex = 0;
            this.label13.Text = "Chọn Site:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 561);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(984, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(85, 17);
            this.lblStatus.Text = "Chưa kết nối...";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHeThong});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuHeThong
            // 
            this.mnuHeThong.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuKetNoi,
            this.mnuQuanLyNhanVien,
            this.mnuQuanLyThamGia});
            this.mnuHeThong.Name = "mnuHeThong";
            this.mnuHeThong.Size = new System.Drawing.Size(69, 20);
            this.mnuHeThong.Text = "Hệ thống";
            // 
            // mnuKetNoi
            // 
            this.mnuKetNoi.Name = "mnuKetNoi";
            this.mnuKetNoi.Size = new System.Drawing.Size(200, 22);
            this.mnuKetNoi.Text = "Kết nối CSDL";
            this.mnuKetNoi.Click += new System.EventHandler(this.mnuKetNoi_Click);
            // 
            // mnuQuanLyNhanVien
            // 
            this.mnuQuanLyNhanVien.Name = "mnuQuanLyNhanVien";
            this.mnuQuanLyNhanVien.Size = new System.Drawing.Size(237, 22);
            this.mnuQuanLyNhanVien.Text = "Quản lý Nhân viên";
            this.mnuQuanLyNhanVien.Click += new System.EventHandler(this.mnuQuanLyNhanVien_Click);
            // 
            // mnuQuanLyThamGia
            // 
            this.mnuQuanLyThamGia.Name = "mnuQuanLyThamGia";
            this.mnuQuanLyThamGia.Size = new System.Drawing.Size(237, 22);
            this.mnuQuanLyThamGia.Text = "Quản lý Tham gia Đề án";
            this.mnuQuanLyThamGia.Click += new System.EventHandler(this.mnuQuanLyThamGia_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 583);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý Đề Án Phân Tán - CSDL Phân Tán (2-Tier)";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabQuanLy.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuanLy)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabTruyVan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTruyVan)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabCapNhat.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabXemSiteKhac.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSiteKhac)).EndInit();
            this.contextMenuSiteKhac.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabQuanLy;
        private System.Windows.Forms.TabPage tabTruyVan;
        private System.Windows.Forms.TabPage tabCapNhat;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.DataGridView dgvQuanLy;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoadNhomNC;
        private System.Windows.Forms.Button btnLoadDean;
        private System.Windows.Forms.Button btnLoadThamGia;
        private System.Windows.Forms.Button btnLoadNhanVien;
        private System.Windows.Forms.DataGridView dgvTruyVan;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMaNhom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnTruyVanMuc2;
        private System.Windows.Forms.Button btnTruyVanMuc1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCapNhat;
        private System.Windows.Forms.TextBox txtTenPhongMoi;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMaNhomCapNhat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblSite;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuHeThong;
        private System.Windows.Forms.ToolStripMenuItem mnuKetNoi;
        private System.Windows.Forms.ToolStripMenuItem mnuQuanLyNhanVien;
        private System.Windows.Forms.ToolStripMenuItem mnuQuanLyThamGia;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboSiteThemNhom;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnThemNhom;
        private System.Windows.Forms.TextBox txtTenPhong;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtTenNhomMoi;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMaNhomMoi;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tabXemSiteKhac;
        private System.Windows.Forms.DataGridView dgvSiteKhac;
        private System.Windows.Forms.ContextMenuStrip contextMenuSiteKhac;
        private System.Windows.Forms.ToolStripMenuItem mnuSuaDuLieu;
        private System.Windows.Forms.ToolStripMenuItem mnuXoaDuLieu;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox cboChonSite;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnLoadNhomSiteKhac;
        private System.Windows.Forms.Button btnLoadNVSiteKhac;
        private System.Windows.Forms.Button btnLoadDASiteKhac;
        private System.Windows.Forms.Button btnLoadTGSiteKhac;
        private System.Windows.Forms.Label label14;
    }
}
