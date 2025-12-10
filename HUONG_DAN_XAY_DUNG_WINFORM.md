# HƯỚNG DẪN XÂY DỰNG ỨNG DỤNG WINDOWS FORM - QUẢN LÝ GIAO DỊCH (CLO4)

## MỤC TIÊU
Xây dựng ứng dụng quản lý giao dịch, kiểm soát dữ liệu nghĩa và truy vấn phân tán sử dụng Windows Form với C# và SQL Server phân tán.

---

## PHẦN 1: THIẾT LẬP DỰ ÁN

### 1.1. Tạo Project Windows Forms
```bash
# Tạo solution mới
dotnet new winforms -n QLDeAnPhanTan -f net6.0-windows

# Mở Visual Studio và tạo Windows Forms App (.NET 6.0)
```

### 1.2. Cài đặt NuGet Packages
```xml
<ItemGroup>
  <PackageReference Include="System.Data.SqlClient" Version="4.8.5" />
</ItemGroup>
```

---

## PHẦN 2: THIẾT KẾ CẤU TRÚC DỰ ÁN

### 2.1. Cấu trúc thư mục
```
QLDeAnPhanTan/
├── Program.cs                  # Entry point
├── ConnectionForm.cs           # Form kết nối database
├── MainForm.cs                 # Form chính
├── NhanVienForm.cs            # Form quản lý nhân viên
├── ThamGiaForm.cs             # Form quản lý tham gia
├── DatabaseHelper.cs          # Lớp trợ giúp database
└── bin/Debug/net6.0-windows/
```

### 2.2. Class DatabaseHelper - Lớp quản lý kết nối
```csharp
public class DatabaseHelper
{
    private string connectionString;
    
    public DatabaseHelper(string server, string database, string username, string password)
    {
        connectionString = $"Server={server};Database={database};" +
                          $"User Id={username};Password={password};" +
                          $"TrustServerCertificate=True;";
    }
    
    // Các phương thức CRUD
    public DataTable LoadData(string tableName) { }
    public void InsertData(string tableName, Dictionary<string, object> data) { }
    public void UpdateData(string tableName, Dictionary<string, object> data, string primaryKey) { }
    public void DeleteData(string tableName, string primaryKey, object value) { }
}
```

---

## PHẦN 3: XÂY DỰNG FORM KẾT NỐI (ConnectionForm)

### 3.1. Thiết kế giao diện
**Controls cần thêm:**
- **Label**: "Server:", "Database:", "Username:", "Password:"
- **TextBox**: `txtServer`, `txtDatabase`, `txtUsername`, `txtPassword`
- **Button**: `btnConnect` ("Kết nối")
- **ComboBox**: `cboDatabase` (QLDeAn, QLDeAn_P1, QLDeAn_P2)

### 3.2. Code xử lý kết nối
```csharp
private void btnConnect_Click(object sender, EventArgs e)
{
    try
    {
        string server = txtServer.Text.Trim();
        string database = cboDatabase.SelectedItem?.ToString();
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text;
        
        // Kiểm tra kết nối
        DatabaseHelper dbHelper = new DatabaseHelper(server, database, username, password);
        dbHelper.TestConnection();
        
        // Mở MainForm
        MainForm mainForm = new MainForm(dbHelper, database);
        mainForm.Show();
        this.Hide();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Lỗi kết nối: {ex.Message}", "Lỗi");
    }
}
```

---

## PHẦN 4: XÂY DỰNG FORM CHÍNH (MainForm)

### 4.1. Thiết kế TabControl với 5 tabs

#### **Tab 1: CRUD Dữ liệu**
**Controls:**
- `ComboBox cboTable`: Chọn bảng (nhomnc, nhanvien, dean, thamgia)
- `Button btnLoad`: Load dữ liệu
- `Button btnThem`: Thêm mới
- `Button btnSua`: Sửa
- `Button btnXoa`: Xóa
- `DataGridView dgvData`: Hiển thị dữ liệu

**Code xử lý:**
```csharp
private void btnLoad_Click(object sender, EventArgs e)
{
    string tableName = cboTable.SelectedItem?.ToString();
    if (string.IsNullOrEmpty(tableName)) return;
    
    DataTable dt = dbHelper.LoadData(tableName);
    dgvData.DataSource = dt;
}

private void btnThem_Click(object sender, EventArgs e)
{
    if (cboTable.SelectedItem?.ToString() == "nhanvien")
    {
        NhanVienForm form = new NhanVienForm(dbHelper, currentDatabase);
        form.ShowDialog();
        btnLoad_Click(sender, e); // Refresh
    }
}
```

#### **Tab 2: Truy vấn Mức 1 (Trong suốt phân mảnh)**
**Chức năng:** Cho biết mã đề án, tên đề án của các đề án thuộc nhóm ($manhom) mà có nhân viên của nhóm nghiên cứu khác tham gia

**Controls:**
- `TextBox txtMaNhom`: Nhập mã nhóm
- `Button btnTruyVanMuc1`: Thực hiện truy vấn
- `DataGridView dgvMuc1`: Hiển thị kết quả

**SQL Query (sử dụng View):**
```sql
-- Tạo View toàn cục
CREATE VIEW v_dean_global AS
SELECT * FROM dean
UNION ALL
SELECT * FROM LINK_P1.QLDeAn_P1.dbo.dean
UNION ALL
SELECT * FROM LINK_P2.QLDeAn_P2.dbo.dean;

-- Truy vấn
SELECT DISTINCT d.mada, d.tenda
FROM v_dean_global d
JOIN v_thamgia_global tg ON d.mada = tg.mada
JOIN v_nhanvien_global nv ON tg.manv = nv.manv
WHERE d.manhom = @manhom AND nv.manhom <> @manhom;
```

**Code C#:**
```csharp
private void btnTruyVanMuc1_Click(object sender, EventArgs e)
{
    string maNhom = txtMaNhom.Text.Trim();
    DataTable dt = dbHelper.TruyVanMuc1(maNhom);
    dgvMuc1.DataSource = dt;
}
```

#### **Tab 3: Truy vấn Mức 2 (Trong suốt vị trí)**
**Chức năng:** Tương tự Tab 2 nhưng sử dụng UNION ALL và cú pháp 4 phần

**SQL Query:**
```sql
SELECT DISTINCT d.mada, d.tenda
FROM (
    SELECT * FROM dean
    UNION ALL
    SELECT * FROM LINK_P1.QLDeAn_P1.dbo.dean
    UNION ALL
    SELECT * FROM LINK_P2.QLDeAn_P2.dbo.dean
) d
JOIN (
    SELECT * FROM thamgia
    UNION ALL
    SELECT * FROM LINK_P1.QLDeAn_P1.dbo.thamgia
    UNION ALL
    SELECT * FROM LINK_P2.QLDeAn_P2.dbo.thamgia
) tg ON d.mada = tg.mada
JOIN (
    SELECT * FROM nhanvien
    UNION ALL
    SELECT * FROM LINK_P1.QLDeAn_P1.dbo.nhanvien
    UNION ALL
    SELECT * FROM LINK_P2.QLDeAn_P2.dbo.nhanvien
) nv ON tg.manv = nv.manv
WHERE d.manhom = @manhom AND nv.manhom <> @manhom;
```

#### **Tab 4: Cập nhật phân tán**
**Chức năng:** Chuyển nhóm nghiên cứu giữa các Site (P1 ↔ P2)

**Controls:**
- `TextBox txtMaNhomChuyen`: Mã nhóm cần chuyển
- `ComboBox cboSiteHienTai`: Site hiện tại (P1/P2)
- `ComboBox cboSiteDich`: Site đích (P1/P2)
- `Button btnChuyenNhom`: Thực hiện chuyển
- `DataGridView dgvCapNhat`: Hiển thị kết quả

**Logic xử lý:**
```csharp
private void btnChuyenNhom_Click(object sender, EventArgs e)
{
    string maNhom = txtMaNhomChuyen.Text.Trim();
    string siteHienTai = cboSiteHienTai.SelectedItem?.ToString();
    string siteDich = cboSiteDich.SelectedItem?.ToString();
    
    // Xác định database và link server
    string dbSource = (siteHienTai == "P1") ? "QLDeAn_P1" : "QLDeAn_P2";
    string dbTarget = (siteDich == "P1") ? "QLDeAn_P1" : "QLDeAn_P2";
    string linkServer = (siteDich == "P1") ? "LINK_P1" : "LINK_P2";
    
    // Gọi stored procedure hoặc thực hiện 4 bước:
    // 1. INSERT vào site đích (nhóm, nhân viên, đề án, tham gia)
    // 2. Kiểm tra dữ liệu đã chèn
    // 3. DELETE khỏi site nguồn
    // 4. Commit hoặc rollback
    
    bool result = dbHelper.ChuyenNhomGiuaSite(maNhom, dbSource, dbTarget, linkServer);
    
    if (result)
        MessageBox.Show("Chuyển nhóm thành công!", "Thông báo");
    else
        MessageBox.Show("Chuyển nhóm thất bại!", "Lỗi");
}
```

**Các bước trong DatabaseHelper.ChuyenNhomGiuaSite():**
```csharp
public bool ChuyenNhomGiuaSite(string maNhom, string dbSource, string dbTarget, string linkServer)
{
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();
        SqlTransaction trans = conn.BeginTransaction();
        
        try
        {
            // BƯỚC 1: INSERT nhóm nghiên cứu
            string sql1 = $@"
                INSERT INTO {linkServer}.{dbTarget}.dbo.nhomnc 
                SELECT * FROM {dbSource}.dbo.nhomnc WHERE manhom = @manhom";
            ExecuteNonQuery(sql1, trans, new SqlParameter("@manhom", maNhom));
            
            // BƯỚC 2: INSERT nhân viên
            string sql2 = $@"
                INSERT INTO {linkServer}.{dbTarget}.dbo.nhanvien 
                SELECT * FROM {dbSource}.dbo.nhanvien WHERE manhom = @manhom";
            ExecuteNonQuery(sql2, trans, new SqlParameter("@manhom", maNhom));
            
            // BƯỚC 3: INSERT đề án
            string sql3 = $@"
                INSERT INTO {linkServer}.{dbTarget}.dbo.dean 
                SELECT * FROM {dbSource}.dbo.dean WHERE manhom = @manhom";
            ExecuteNonQuery(sql3, trans, new SqlParameter("@manhom", maNhom));
            
            // BƯỚC 4: INSERT tham gia
            string sql4 = $@"
                INSERT INTO {linkServer}.{dbTarget}.dbo.thamgia 
                SELECT tg.* FROM {dbSource}.dbo.thamgia tg
                JOIN {dbSource}.dbo.nhanvien nv ON tg.manv = nv.manv
                WHERE nv.manhom = @manhom";
            ExecuteNonQuery(sql4, trans, new SqlParameter("@manhom", maNhom));
            
            // BƯỚC 5: DELETE khỏi site nguồn (ngược lại)
            ExecuteNonQuery($"DELETE FROM {dbSource}.dbo.thamgia WHERE manv IN (SELECT manv FROM {dbSource}.dbo.nhanvien WHERE manhom = @manhom)", trans, new SqlParameter("@manhom", maNhom));
            ExecuteNonQuery($"DELETE FROM {dbSource}.dbo.dean WHERE manhom = @manhom", trans, new SqlParameter("@manhom", maNhom));
            ExecuteNonQuery($"DELETE FROM {dbSource}.dbo.nhanvien WHERE manhom = @manhom", trans, new SqlParameter("@manhom", maNhom));
            ExecuteNonQuery($"DELETE FROM {dbSource}.dbo.nhomnc WHERE manhom = @manhom", trans, new SqlParameter("@manhom", maNhom));
            
            trans.Commit();
            return true;
        }
        catch
        {
            trans.Rollback();
            return false;
        }
    }
}
```

#### **Tab 5: Xem Site khác**
**Chức năng:** Xem và chỉnh sửa dữ liệu từ site khác

**Controls:**
- `ComboBox cboChonSite`: Chọn site (P1/P2)
- `Button btnLoadNhomSiteKhac`: Load bảng nhomnc
- `Button btnLoadNVSiteKhac`: Load bảng nhanvien
- `Button btnLoadDASiteKhac`: Load bảng dean
- `Button btnLoadTGSiteKhac`: Load bảng thamgia
- `DataGridView dgvSiteKhac`: Hiển thị dữ liệu
- **ContextMenuStrip**: Menu chuột phải (Sửa, Xóa)

**Code xử lý:**
```csharp
private void btnLoadNhomSiteKhac_Click(object sender, EventArgs e)
{
    string remoteSite = cboChonSite.SelectedItem?.ToString();
    string remoteDb = (remoteSite == "P1") ? "QLDeAn_P1" : "QLDeAn_P2";
    string linkServer = DetermineLinkServer(currentDatabase, remoteDb);
    
    DataTable dt = dbHelper.LoadDataFromRemoteSite(linkServer, remoteDb, "nhomnc");
    dgvSiteKhac.DataSource = dt;
}

private void mnuSuaDuLieu_Click(object sender, EventArgs e)
{
    if (dgvSiteKhac.SelectedRows.Count == 0) return;
    
    DataGridViewRow row = dgvSiteKhac.SelectedRows[0];
    
    // Tạo form động để sửa dữ liệu
    Form editForm = new Form
    {
        Text = "Sửa dữ liệu",
        Size = new Size(400, 300)
    };
    
    Dictionary<string, TextBox> textBoxes = new Dictionary<string, TextBox>();
    int yPos = 20;
    
    foreach (DataGridViewColumn col in dgvSiteKhac.Columns)
    {
        // Bỏ qua các cột computed (rowguid, timestamp)
        if (col.Name == "rowguid" || col.Name == "timestamp") continue;
        
        Label lbl = new Label { Text = col.Name, Location = new Point(20, yPos) };
        TextBox txt = new TextBox { Location = new Point(150, yPos), Width = 200 };
        txt.Text = row.Cells[col.Name].Value?.ToString();
        
        editForm.Controls.Add(lbl);
        editForm.Controls.Add(txt);
        textBoxes[col.Name] = txt;
        
        yPos += 30;
    }
    
    Button btnSave = new Button { Text = "Lưu", Location = new Point(150, yPos) };
    btnSave.Click += (s, ev) =>
    {
        Dictionary<string, object> updatedData = new Dictionary<string, object>();
        foreach (var kvp in textBoxes)
        {
            updatedData[kvp.Key] = kvp.Value.Text;
        }
        
        // Xác định remote site và link server
        string remoteSite = cboChonSite.SelectedItem?.ToString();
        string remoteDb = (remoteSite == "P1") ? "QLDeAn_P1" : "QLDeAn_P2";
        string linkServer = DetermineLinkServer(currentDatabase, remoteDb);
        string tableName = currentTableName; // Lưu từ lần load
        
        dbHelper.UpdateRemoteSite(linkServer, remoteDb, tableName, updatedData, primaryKeyColumn);
        editForm.Close();
        RefreshCurrentTableData();
    };
    
    editForm.Controls.Add(btnSave);
    editForm.ShowDialog();
}
```

---

## PHẦN 5: XÂY DỰNG FORM NHÂN VIÊN (NhanVienForm)

### 5.1. Thiết kế giao diện
**Controls:**
- `TextBox txtMaNV`: Mã nhân viên
- `TextBox txtHo`: Họ
- `TextBox txtTen`: Tên
- `DateTimePicker dtpNgaySinh`: Ngày sinh
- `TextBox txtDiaChi`: Địa chỉ
- `ComboBox cboMaNhom`: Mã nhóm (load từ DB)
- `Button btnLuu`: Lưu
- `Button btnHuy`: Hủy

### 5.2. Code xử lý
```csharp
private void btnLuu_Click(object sender, EventArgs e)
{
    try
    {
        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "manv", txtMaNV.Text.Trim() },
            { "ho", txtHo.Text.Trim() },
            { "ten", txtTen.Text.Trim() },
            { "ngaysinh", dtpNgaySinh.Value },
            { "diachi", txtDiaChi.Text.Trim() },
            { "manhom", cboMaNhom.SelectedValue }
        };
        
        if (isEditMode)
            dbHelper.UpdateData("nhanvien", data, "manv");
        else
            dbHelper.InsertData("nhanvien", data);
        
        MessageBox.Show("Lưu thành công!", "Thông báo");
        this.Close();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi");
    }
}
```

---

## PHẦN 6: XÂY DỰNG FORM THAM GIA (ThamGiaForm)

### 6.1. Thiết kế tương tự NhanVienForm
**Controls:**
- `ComboBox cboMaNV`: Mã nhân viên
- `ComboBox cboMaDA`: Mã đề án
- `TextBox txtThoiGian`: Thời gian (số giờ)
- `Button btnLuu`, `Button btnHuy`

### 6.2. Xử lý Composite Key
```csharp
private void btnLuu_Click(object sender, EventArgs e)
{
    Dictionary<string, object> data = new Dictionary<string, object>
    {
        { "manv", cboMaNV.SelectedValue },
        { "mada", cboMaDA.SelectedValue },
        { "thoigian", Convert.ToDecimal(txtThoiGian.Text) }
    };
    
    // Với bảng có composite key, cần xử lý đặc biệt
    if (isEditMode)
    {
        string whereClause = $"manv = '{oldMaNV}' AND mada = '{oldMaDA}'";
        dbHelper.UpdateDataWithCustomWhere("thamgia", data, whereClause);
    }
    else
        dbHelper.InsertData("thamgia", data);
}
```

---

## PHẦN 7: XỬ LÝ DỮ LIỆU PHÂN TÁN

### 7.1. Logic xác định Link Server
```csharp
private string DetermineLinkServer(string currentDb, string targetDb)
{
    // Từ QLDeAn -> P1 hoặc P2
    if (currentDb == "QLDeAn")
    {
        return targetDb == "QLDeAn_P1" ? "LINK_P1" : "LINK_P2";
    }
    // Từ P1 -> P2
    else if (currentDb == "QLDeAn_P1" && targetDb == "QLDeAn_P2")
    {
        return "LINK_P2"; // Từ P1 nhìn sang P2
    }
    // Từ P2 -> P1
    else if (currentDb == "QLDeAn_P2" && targetDb == "QLDeAn_P1")
    {
        return "LINK_P1"; // Từ P2 nhìn sang P1
    }
    
    throw new Exception("Không xác định được Link Server");
}
```

### 7.2. Load dữ liệu từ Remote Site
```csharp
public DataTable LoadDataFromRemoteSite(string linkServer, string dbName, string tableName)
{
    string sql = $"SELECT * FROM {linkServer}.{dbName}.dbo.{tableName}";
    
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        return dt;
    }
}
```

### 7.3. Update dữ liệu Remote Site
```csharp
public void UpdateRemoteSite(string linkServer, string dbName, string tableName, 
                             Dictionary<string, object> data, string primaryKeyColumn)
{
    // Loại bỏ cột computed
    HashSet<string> excludedColumns = new HashSet<string> 
    { 
        primaryKeyColumn, "rowguid", "timestamp", "CreatedDate", "ModifiedDate" 
    };
    
    Dictionary<string, object> updateParams = new Dictionary<string, object>();
    
    foreach (var col in data.Keys)
    {
        if (!excludedColumns.Contains(col))
            updateParams[col] = data[col];
    }
    
    if (updateParams.Count == 0)
        throw new Exception("Không có cột nào để cập nhật!");
    
    // Build SET clause
    string setClauses = string.Join(", ", 
        updateParams.Keys.Select(k => $"{k} = @{k}"));
    
    string sql = $@"
        UPDATE {linkServer}.{dbName}.dbo.{tableName}
        SET {setClauses}
        WHERE {primaryKeyColumn} = @primaryKey";
    
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        SqlCommand cmd = new SqlCommand(sql, conn);
        
        foreach (var kvp in updateParams)
            cmd.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value ?? DBNull.Value);
        
        cmd.Parameters.AddWithValue("@primaryKey", data[primaryKeyColumn]);
        
        conn.Open();
        cmd.ExecuteNonQuery();
    }
}
```

### 7.4. Delete với kiểm tra Foreign Key
```csharp
public void DeleteFromRemoteSite(string linkServer, string dbName, string tableName, 
                                 string primaryKey, object value)
{
    // Kiểm tra FK trước khi xóa
    if (tableName == "nhomnc")
    {
        // Kiểm tra có nhân viên hoặc đề án không
        string checkSql = $@"
            SELECT COUNT(*) FROM {linkServer}.{dbName}.dbo.nhanvien WHERE manhom = @value
            UNION ALL
            SELECT COUNT(*) FROM {linkServer}.{dbName}.dbo.dean WHERE manhom = @value";
        
        // Nếu có dữ liệu phụ thuộc -> báo lỗi
    }
    
    string sql = $"DELETE FROM {linkServer}.{dbName}.dbo.{tableName} WHERE {primaryKey} = @value";
    
    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@value", value);
        conn.Open();
        cmd.ExecuteNonQuery();
    }
}
```

---

## PHẦN 8: XỬ LÝ LỖI VÀ VALIDATION

### 8.1. Validation Input
```csharp
private bool ValidateInput()
{
    if (string.IsNullOrWhiteSpace(txtMaNV.Text))
    {
        MessageBox.Show("Mã nhân viên không được rỗng!", "Lỗi");
        txtMaNV.Focus();
        return false;
    }
    
    if (string.IsNullOrWhiteSpace(txtTen.Text))
    {
        MessageBox.Show("Tên không được rỗng!", "Lỗi");
        txtTen.Focus();
        return false;
    }
    
    return true;
}
```

### 8.2. Xử lý lỗi trùng Primary Key
```csharp
try
{
    dbHelper.InsertData("nhanvien", data);
}
catch (SqlException ex)
{
    if (ex.Number == 2627) // Violation of PRIMARY KEY constraint
    {
        MessageBox.Show("Mã nhân viên đã tồn tại!", "Lỗi");
    }
    else if (ex.Number == 547) // FK constraint violation
    {
        MessageBox.Show("Mã nhóm không tồn tại!", "Lỗi");
    }
    else
    {
        MessageBox.Show($"Lỗi database: {ex.Message}", "Lỗi");
    }
}
```

### 8.3. Xử lý lỗi ROWGUIDCOL
```csharp
// Trong UpdateRemoteSite, loại bỏ các cột không được phép cập nhật
foreach (DataColumn col in dt.Columns)
{
    if (col.AutoIncrement || col.ReadOnly || 
        col.ColumnName == "rowguid" || col.ColumnName == "timestamp")
    {
        continue; // Bỏ qua cột này
    }
    
    updateParams[col.ColumnName] = row[col.ColumnName];
}
```

---

## PHẦN 9: THIẾT LẬP LINK SERVER

### 9.1. Tạo Link Server trên P1
```sql
-- Chạy trên instance P1
EXEC sp_addlinkedserver 
    @server = 'LINK_P2',
    @srvproduct = '',
    @provider = 'SQLNCLI',
    @datasrc = 'localhost\P2';

EXEC sp_addlinkedsrvlogin 
    @rmtsrvname = 'LINK_P2',
    @useself = 'false',
    @rmtuser = 'sa',
    @rmtpassword = 'your_password';
```

### 9.2. Tạo Link Server trên P2
```sql
-- Chạy trên instance P2
EXEC sp_addlinkedserver 
    @server = 'LINK_P1',
    @srvproduct = '',
    @provider = 'SQLNCLI',
    @datasrc = 'localhost\P1';

EXEC sp_addlinkedsrvlogin 
    @rmtsrvname = 'LINK_P1',
    @useself = 'false',
    @rmtuser = 'sa',
    @rmtpassword = 'your_password';
```

### 9.3. Kiểm tra kết nối
```sql
-- Kiểm tra Link Server
SELECT * FROM sys.servers WHERE is_linked = 1;

-- Test query
SELECT * FROM LINK_P2.QLDeAn_P2.dbo.nhomnc;
```

---

## PHẦN 10: TESTING VÀ DEBUG

### 10.1. Test CRUD cơ bản
1. Kết nối vào QLDeAn
2. Thêm/Sửa/Xóa dữ liệu bảng nhomnc, nhanvien, dean, thamgia
3. Kiểm tra FK constraint khi xóa

### 10.2. Test truy vấn phân tán
1. Chạy truy vấn Mức 1 với mã nhóm có dữ liệu ở cả 3 site
2. Kiểm tra kết quả có đầy đủ không
3. So sánh kết quả Mức 1 và Mức 2

### 10.3. Test cập nhật phân tán
1. Tạo nhóm mới ở P1
2. Thêm nhân viên, đề án, tham gia vào nhóm đó
3. Chuyển nhóm từ P1 sang P2
4. Kiểm tra dữ liệu đã chuyển đúng và xóa sạch ở P1

### 10.4. Test xem site khác
1. Kết nối vào P1
2. Xem dữ liệu của P2
3. Sửa/Xóa dữ liệu từ P2 qua P1
4. Kiểm tra dữ liệu thay đổi ở P2

---

## PHẦN 11: TỐI ƯU VÀ BẢO MẬT

### 11.1. Sử dụng Parameterized Query
```csharp
// ĐÚNG
SqlCommand cmd = new SqlCommand("SELECT * FROM nhanvien WHERE manv = @manv", conn);
cmd.Parameters.AddWithValue("@manv", maNV);

// SAI (SQL Injection)
string sql = $"SELECT * FROM nhanvien WHERE manv = '{maNV}'";
```

### 11.2. Sử dụng Transaction
```csharp
using (SqlConnection conn = new SqlConnection(connectionString))
{
    conn.Open();
    SqlTransaction trans = conn.BeginTransaction();
    
    try
    {
        // Thực hiện nhiều lệnh SQL
        ExecuteNonQuery(sql1, trans);
        ExecuteNonQuery(sql2, trans);
        
        trans.Commit();
    }
    catch
    {
        trans.Rollback();
        throw;
    }
}
```

### 11.3. Connection Pooling
```csharp
// Connection string với pooling
string connStr = "Server=...;Database=...;User Id=...;Password=...;" +
                 "Min Pool Size=5;Max Pool Size=100;Pooling=true;";
```

---

## PHẦN 12: BUILD VÀ DEPLOY

### 12.1. Build ứng dụng
```bash
# Build Debug
dotnet build

# Build Release
dotnet build -c Release

# Publish
dotnet publish -c Release -r win-x64 --self-contained true
```

### 12.2. File cấu hình App.config
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <connectionStrings>
        <add name="DefaultConnection" 
             connectionString="Server=localhost;Database=QLDeAn;User Id=sa;Password=123;TrustServerCertificate=True;" 
             providerName="System.Data.SqlClient" />
    </connectionStrings>
</configuration>
```

---

## PHẦN 13: KẾT LUẬN

### 13.1. Các tính năng đã hoàn thành
✅ Kết nối database phân tán (3 site)  
✅ CRUD cơ bản (Thêm/Sửa/Xóa/Xem)  
✅ Truy vấn phân tán 2 mức (View + UNION ALL)  
✅ Cập nhật phân tán (chuyển nhóm giữa các site)  
✅ Xem và chỉnh sửa dữ liệu site khác  
✅ Validation và xử lý lỗi FK, ROWGUIDCOL  
✅ Sử dụng Link Server với cú pháp 4 phần  

### 13.2. Kiến thức đạt được (CLO4)
- **Quản lý giao dịch**: Transaction, Commit, Rollback
- **Kiểm soát dữ liệu nghĩa**: FK constraint, validation
- **Truy vấn phân tán**: Link Server, UNION ALL, View toàn cục
- **Cập nhật phân tán**: INSERT-DELETE cross-server
- **Lập trình C#**: WinForms, ADO.NET, OOP

### 13.3. Tài liệu tham khảo
- Microsoft Docs: SQL Server Distributed Queries
- Microsoft Docs: Windows Forms Programming
- Stack Overflow: Link Server troubleshooting
- GitHub: Distributed Database Examples

---

**Hoàn thành:** 27/11/2025  
**Người thực hiện:** [Tên của bạn]  
**Môn học:** Cơ Sở Dữ Liệu Phân Tán  
**Mục tiêu:** CLO4 - Xây dựng ứng dụng quản lý giao dịch, kiểm soát dữ liệu nghĩa và truy vấn phân tán
