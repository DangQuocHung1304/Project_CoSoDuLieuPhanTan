using System;
using System.Data;
using System.Data.SqlClient;

namespace QLDeAnPhanTan
{
    /// <summary>
    /// Lớp xử lý kết nối và truy vấn CSDL Phân tán
    /// Kết nối đến Site Gốc: MSI\YLC (chứa View toàn cục và Link Server)
    /// Site P1: DESKTOP-SEERKGC\ANHTHU (QLDeAn_P1)
    /// Site P2: LAPTOP-F37K83BK\SQLEXPRESS (QLDeAn_P2)
    /// </summary>
    public class DatabaseHelper
    {
        private readonly string connectionString;
        private readonly string databaseName;

        public DatabaseHelper(string serverName, string username = "sa", string password = "123", string database = "QLDeAn")
        {
            databaseName = database;
            connectionString = $"Data Source={serverName};" +
                             $"Initial Catalog={databaseName};" +
                             $"User ID={username};" +
                             $"Password={password};" +
                             $"Integrated Security=False;" +
                             $"TrustServerCertificate=True;";
        }

        /// <summary>
        /// Kiểm tra kết nối đến CSDL
        /// </summary>
        public bool TestConnection(out string message)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    message = "Kết nối thành công đến CSDL!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                message = $"Lỗi kết nối: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// MỨC 1: TRONG SUỐT PHÂN MẢNH
        /// Hàm load dữ liệu toàn cục từ View (Site Gốc) hoặc bảng trực tiếp (Site P1/P2)
        /// </summary>
        public DataTable LoadToanCuc(string viewName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql;
                    
                    // Nếu đang kết nối vào Site P1 hoặc P2, query trực tiếp từ bảng
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        // Chuyển từ view_name sang table_name (bỏ _view)
                        string tableName = viewName.Replace("_view", "");
                        sql = $"SELECT * FROM {tableName}";
                    }
                    else
                    {
                        // Site Gốc: query từ View
                        sql = $"SELECT * FROM {viewName}";
                    }
                    
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi load dữ liệu từ {viewName}: {ex.Message}");
            }
        }

        /// <summary>
        /// TRUY VẤN XUYÊN SITE
        /// Load dữ liệu từ Site khác (P1 xem P2 hoặc P2 xem P1)
        /// Hỗ trợ khi kết nối vào bất kỳ database nào (QLDeAn, QLDeAn_P1, QLDeAn_P2)
        /// </summary>
        public DataTable LoadDataFromRemoteSite(string linkServer, string dbName, string tableName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql;
                    
                    // Xác định Link Server name dựa vào database hiện tại
                    if (databaseName == "QLDeAn_P1")
                    {
                        // Kết nối từ P1 → muốn xem P2 → dùng LINK_P2
                        sql = $"SELECT * FROM LINK_P2.{dbName}.dbo.{tableName}";
                    }
                    else if (databaseName == "QLDeAn_P2")
                    {
                        // Kết nối từ P2 → muốn xem P1 → dùng LINK_P1
                        sql = $"SELECT * FROM LINK_P1.{dbName}.dbo.{tableName}";
                    }
                    else
                    {
                        // Kết nối từ Site Gốc: dùng Link Server từ tham số
                        sql = $"SELECT * FROM {linkServer}.{dbName}.dbo.{tableName}";
                    }
                    
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                string helpMessage = "";
                if (databaseName == "QLDeAn_P1")
                {
                    helpMessage = "\n\n💡 Lưu ý: Bạn đang kết nối vào Site P1.\n" +
                        "Để xem dữ liệu Site P2, cần cấu hình Link Server 'LINK_P2' trên P1.\n" +
                        "Chạy script: TaiLieu_Buoc3_LinkServer\\TaoLinkServerSite1_P1.sql";
                }
                else if (databaseName == "QLDeAn_P2")
                {
                    helpMessage = "\n\n💡 Lưu ý: Bạn đang kết nối vào Site P2.\n" +
                        "Để xem dữ liệu Site P1, cần cấu hình Link Server 'LINK_P1' trên P2.\n" +
                        "Chạy script: TaiLieu_Buoc3_LinkServer\\TaoLinkServerSite2_P2.sql";
                }
                throw new Exception($"Lỗi khi load dữ liệu: {ex.Message}{helpMessage}");
            }
        }

        /// <summary>
        /// CẬP NHẬT DỮ LIỆU XUYÊN SITE
        /// Cập nhật một dòng dữ liệu trên Site khác
        /// </summary>
        public bool UpdateRemoteSite(string linkServer, string dbName, string tableName, 
            string primaryKeyColumn, object primaryKeyValue, DataRow updatedRow)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Danh sách các cột không được phép UPDATE
                    var excludedColumns = new System.Collections.Generic.HashSet<string>(
                        System.StringComparer.OrdinalIgnoreCase)
                    {
                        primaryKeyColumn,
                        "rowguid",      // ROWGUIDCOL
                        "timestamp",    // Timestamp column
                        "CreatedDate",  // Computed hoặc auto-generated columns
                        "ModifiedDate"
                    };
                    
                    // Build UPDATE statement - chỉ lấy các cột được phép UPDATE
                    var setClauses = new System.Text.StringBuilder();
                    var updateParams = new System.Collections.Generic.Dictionary<string, object>();
                    
                    foreach (DataColumn col in updatedRow.Table.Columns)
                    {
                        // Bỏ qua: PK, computed columns, readonly columns
                        if (!excludedColumns.Contains(col.ColumnName) && 
                            !col.AutoIncrement && 
                            !col.ReadOnly)
                        {
                            if (setClauses.Length > 0)
                                setClauses.Append(", ");
                            setClauses.Append($"{col.ColumnName} = @{col.ColumnName}");
                            updateParams[col.ColumnName] = updatedRow[col];
                        }
                    }
                    
                    // Kiểm tra có cột nào để UPDATE không
                    if (setClauses.Length == 0)
                    {
                        throw new Exception("Không có cột nào được phép cập nhật!");
                    }

                    // Xác định câu lệnh UPDATE dựa vào database hiện tại
                    string sql;
                    if (databaseName == "QLDeAn_P1")
                    {
                        // Kết nối từ P1 → dùng LINK_P2
                        sql = $"UPDATE LINK_P2.{dbName}.dbo.{tableName} " +
                              $"SET {setClauses} " +
                              $"WHERE {primaryKeyColumn} = @PK";
                    }
                    else if (databaseName == "QLDeAn_P2")
                    {
                        // Kết nối từ P2 → dùng LINK_P1
                        sql = $"UPDATE LINK_P1.{dbName}.dbo.{tableName} " +
                              $"SET {setClauses} " +
                              $"WHERE {primaryKeyColumn} = @PK";
                    }
                    else
                    {
                        // Kết nối từ Site Gốc: dùng Link Server từ tham số
                        sql = $"UPDATE {linkServer}.{dbName}.dbo.{tableName} " +
                              $"SET {setClauses} " +
                              $"WHERE {primaryKeyColumn} = @PK";
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    
                    // Add parameters - chỉ thêm các cột đã được chọn để UPDATE
                    foreach (var param in updateParams)
                    {
                        cmd.Parameters.AddWithValue($"@{param.Key}", 
                            param.Value == DBNull.Value ? (object)DBNull.Value : param.Value);
                    }
                    cmd.Parameters.AddWithValue("@PK", primaryKeyValue);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật dữ liệu trên {linkServer}.{dbName}.{tableName}: {ex.Message}");
            }
        }

        /// <summary>
        /// XÓA DỮ LIỆU XUYÊN SITE
        /// Xóa một dòng dữ liệu trên Site khác
        /// Kiểm tra ràng buộc Foreign Key trước khi xóa
        /// </summary>
        public bool DeleteFromRemoteSite(string linkServer, string dbName, string tableName, 
            string primaryKeyColumn, object primaryKeyValue, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra ràng buộc trước khi xóa
                    string checkMsg;
                    if (!CheckForeignKeyBeforeDelete(conn, linkServer, dbName, tableName, primaryKeyColumn, primaryKeyValue, out checkMsg))
                    {
                        errorMessage = checkMsg;
                        return false;
                    }

                    // Thực hiện xóa
                    string sql;
                    if (databaseName == "QLDeAn_P1")
                    {
                        // Kết nối từ P1 → dùng LINK_P2
                        sql = $"DELETE FROM LINK_P2.{dbName}.dbo.{tableName} " +
                              $"WHERE {primaryKeyColumn} = @PK";
                    }
                    else if (databaseName == "QLDeAn_P2")
                    {
                        // Kết nối từ P2 → dùng LINK_P1
                        sql = $"DELETE FROM LINK_P1.{dbName}.dbo.{tableName} " +
                              $"WHERE {primaryKeyColumn} = @PK";
                    }
                    else
                    {
                        // Kết nối từ Site Gốc: dùng Link Server từ tham số
                        sql = $"DELETE FROM {linkServer}.{dbName}.dbo.{tableName} " +
                              $"WHERE {primaryKeyColumn} = @PK";
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@PK", primaryKeyValue);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi khi xóa dữ liệu trên {linkServer}.{dbName}.{tableName}: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra ràng buộc Foreign Key trước khi xóa
        /// </summary>
        private bool CheckForeignKeyBeforeDelete(SqlConnection conn, string linkServer, string dbName, 
            string tableName, string primaryKeyColumn, object primaryKeyValue, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                // Xác định prefix cho query dựa vào database hiện tại
                string tablePrefix;
                if (databaseName == "QLDeAn_P1")
                {
                    // Kết nối từ P1 → dùng LINK_P2
                    tablePrefix = $"LINK_P2.{dbName}.dbo";
                }
                else if (databaseName == "QLDeAn_P2")
                {
                    // Kết nối từ P2 → dùng LINK_P1
                    tablePrefix = $"LINK_P1.{dbName}.dbo";
                }
                else
                {
                    // Kết nối từ Site Gốc: dùng Link Server từ tham số
                    tablePrefix = $"{linkServer}.{dbName}.dbo";
                }

                // Kiểm tra từng bảng có FK tham chiếu
                if (tableName == "nhomnc")
                {
                    // Kiểm tra nhân viên
                    string sqlCheckNV = $"SELECT COUNT(*) FROM {tablePrefix}.nhanvien WHERE manhom = @PK";
                    SqlCommand cmdNV = new SqlCommand(sqlCheckNV, conn);
                    cmdNV.Parameters.AddWithValue("@PK", primaryKeyValue);
                    int countNV = (int)cmdNV.ExecuteScalar();
                    if (countNV > 0)
                    {
                        errorMessage = $"Không thể xóa nhóm này vì còn {countNV} nhân viên thuộc nhóm. Hãy xóa nhân viên trước.";
                        return false;
                    }

                    // Kiểm tra đề án
                    string sqlCheckDA = $"SELECT COUNT(*) FROM {tablePrefix}.dean WHERE manhom = @PK";
                    SqlCommand cmdDA = new SqlCommand(sqlCheckDA, conn);
                    cmdDA.Parameters.AddWithValue("@PK", primaryKeyValue);
                    int countDA = (int)cmdDA.ExecuteScalar();
                    if (countDA > 0)
                    {
                        errorMessage = $"Không thể xóa nhóm này vì còn {countDA} đề án thuộc nhóm. Hãy xóa đề án trước.";
                        return false;
                    }
                }
                else if (tableName == "nhanvien")
                {
                    // Kiểm tra tham gia
                    string sqlCheckTG = $"SELECT COUNT(*) FROM {tablePrefix}.thamgia WHERE manv = @PK";
                    SqlCommand cmdTG = new SqlCommand(sqlCheckTG, conn);
                    cmdTG.Parameters.AddWithValue("@PK", primaryKeyValue);
                    int countTG = (int)cmdTG.ExecuteScalar();
                    if (countTG > 0)
                    {
                        errorMessage = $"Không thể xóa nhân viên này vì đang tham gia {countTG} đề án. Hãy xóa bản ghi tham gia trước.";
                        return false;
                    }
                }
                else if (tableName == "dean")
                {
                    // Kiểm tra tham gia
                    string sqlCheckTG = $"SELECT COUNT(*) FROM {tablePrefix}.thamgia WHERE mada = @PK";
                    SqlCommand cmdTG = new SqlCommand(sqlCheckTG, conn);
                    cmdTG.Parameters.AddWithValue("@PK", primaryKeyValue);
                    int countTG = (int)cmdTG.ExecuteScalar();
                    if (countTG > 0)
                    {
                        errorMessage = $"Không thể xóa đề án này vì có {countTG} nhân viên đang tham gia. Hãy xóa bản ghi tham gia trước.";
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi khi kiểm tra ràng buộc: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// MỨC 1: TRONG SUỐT PHÂN MẢNH
        /// Truy vấn đề án theo mã nhóm (sử dụng View hoặc bảng local)
        /// Cho biết mã đề án, tên đề án của các đề án thuộc nhóm ($manhom) 
        /// mà có nhân viên của nhóm nghiên cứu khác tham gia
        /// </summary>
        public DataTable TruyVanMuc1(string maNhom)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        // Kết nối trực tiếp vào Site P1/P2: query từ bảng local
                        // CHỈ trả về đề án thuộc site này
                        sql = @"
                            SELECT DISTINCT DA.mada, DA.tenda 
                            FROM dean DA 
                            JOIN thamgia TG ON DA.mada = TG.mada 
                            JOIN nhanvien NV ON TG.manv = NV.manv 
                            WHERE DA.manhom = @manhom AND NV.manhom <> @manhom";
                    }
                    else
                    {
                        // Site Gốc: query từ View (toàn cục)
                        sql = @"
                            SELECT DISTINCT DA.mada, DA.tenda 
                            FROM dean_view DA 
                            JOIN thamgia_view TG ON DA.mada = TG.mada 
                            JOIN nhanvien_view NV ON TG.manv = NV.manv 
                            WHERE DA.manhom = @manhom AND NV.manhom <> @manhom";
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@manhom", maNhom);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thực hiện truy vấn Mức 1: {ex.Message}");
            }
        }

        /// <summary>
        /// MỨC 2: TRONG SUỐT VỊ TRÍ
        /// Truy vấn đề án theo mã nhóm (sử dụng UNION ALL và cú pháp 4 phần)
        /// Cho biết mã đề án, tên đề án của các đề án thuộc nhóm ($manhom) 
        /// mà có nhân viên của nhóm nghiên cứu khác tham gia
        /// </summary>
        public DataTable TruyVanMuc2(string maNhom)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        // Kết nối trực tiếp vào Site P1/P2: query local
                        // CHỈ trả về đề án thuộc site này (không có UNION ALL)
                        sql = @"
                            SELECT DISTINCT DA.mada, DA.tenda 
                            FROM dean DA 
                            JOIN thamgia TG ON DA.mada = TG.mada 
                            JOIN nhanvien NV ON TG.manv = NV.manv 
                            WHERE DA.manhom = @manhom AND NV.manhom <> @manhom";
                    }
                    else
                    {
                        // Site Gốc: sử dụng UNION ALL và cú pháp 4 phần
                        sql = @"
                            SELECT DISTINCT A.mada, A.tenda 
                            FROM 
                            (
                                SELECT mada COLLATE Vietnamese_CI_AS AS mada, 
                                       tenda COLLATE Vietnamese_CI_AS AS tenda, 
                                       manhom COLLATE Vietnamese_CI_AS AS manhom
                                FROM LINK_P1.QLDeAn_P1.dbo.dean 
                                UNION ALL 
                                SELECT mada COLLATE Vietnamese_CI_AS, 
                                       tenda COLLATE Vietnamese_CI_AS, 
                                       manhom COLLATE Vietnamese_CI_AS
                                FROM LINK_P2.QLDeAn_P2.dbo.dean
                            ) AS A
                            JOIN 
                            (
                                SELECT manv COLLATE Vietnamese_CI_AS AS manv, 
                                       mada COLLATE Vietnamese_CI_AS AS mada
                                FROM LINK_P1.QLDeAn_P1.dbo.thamgia 
                                UNION ALL 
                                SELECT manv COLLATE Vietnamese_CI_AS, 
                                       mada COLLATE Vietnamese_CI_AS
                                FROM LINK_P2.QLDeAn_P2.dbo.thamgia
                            ) AS TG ON A.mada = TG.mada
                            JOIN 
                            (
                                SELECT manv COLLATE Vietnamese_CI_AS AS manv, 
                                       manhom COLLATE Vietnamese_CI_AS AS manhom
                                FROM LINK_P1.QLDeAn_P1.dbo.nhanvien 
                                UNION ALL 
                                SELECT manv COLLATE Vietnamese_CI_AS, 
                                       manhom COLLATE Vietnamese_CI_AS
                                FROM LINK_P2.QLDeAn_P2.dbo.nhanvien
                            ) AS NV ON TG.manv = NV.manv
                            WHERE A.manhom = @manhom AND NV.manhom <> @manhom";
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@manhom", maNhom);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thực hiện truy vấn Mức 2: {ex.Message}");
            }
        }

        /// <summary>
        /// CẬP NHẬT PHÂN TÁN
        /// Chuyển nhóm nghiên cứu từ Site này sang Site khác
        /// Khi đổi tên phòng (P1 -> P2 hoặc ngược lại), cần chuyển toàn bộ dữ liệu liên quan:
        /// - Nhóm nghiên cứu (nhomnc)
        /// - Nhân viên thuộc nhóm (nhanvien)
        /// - Đề án của nhóm (dean)
        /// - Tham gia của nhân viên trong nhóm (thamgia)
        /// 
        /// LƯU Ý: Không dùng distributed transaction để tránh lỗi RPC/MSDTC
        /// Hỗ trợ cả khi kết nối vào Site Gốc (QLDeAn) hoặc Site local (QLDeAn_P1, QLDeAn_P2)
        /// </summary>
        public void ChuyenNhomSangSiteKhac(string maNhom, string tenPhongMoi)
        {
            string linkServerNguon;
            string dbNguon;
            string linkServerDich;
            string dbDich;

            // Xác định Site nguồn và Site đích dựa vào database hiện tại
            if (databaseName == "QLDeAn_P1")
            {
                // Đang kết nối vào Site P1
                linkServerNguon = "LOCAL_P1";
                dbNguon = "QLDeAn_P1";
                linkServerDich = tenPhongMoi == "P1" ? "LOCAL_P1" : "LINK_P2";
                dbDich = tenPhongMoi == "P1" ? "QLDeAn_P1" : "QLDeAn_P2";
            }
            else if (databaseName == "QLDeAn_P2")
            {
                // Đang kết nối vào Site P2
                linkServerNguon = "LOCAL_P2";
                dbNguon = "QLDeAn_P2";
                linkServerDich = tenPhongMoi == "P2" ? "LOCAL_P2" : "LINK_P1";
                dbDich = tenPhongMoi == "P2" ? "QLDeAn_P2" : "QLDeAn_P1";
            }
            else
            {
                // Đang kết nối vào Site Gốc
                linkServerNguon = TimSiteNhom(maNhom);
                dbNguon = linkServerNguon == "LINK_P1" ? "QLDeAn_P1" : "QLDeAn_P2";
                linkServerDich = tenPhongMoi == "P1" ? "LINK_P1" : "LINK_P2";
                dbDich = tenPhongMoi == "P1" ? "QLDeAn_P1" : "QLDeAn_P2";
            }

            // Trường hợp đặc biệt: Cùng Site, chỉ cập nhật tên phòng
            if (linkServerNguon == linkServerDich)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string updateSql;
                        if (linkServerNguon.StartsWith("LOCAL"))
                        {
                            // Kết nối local, không cần 4-part syntax
                            updateSql = @"UPDATE nhomnc 
                                         SET tenphong = @tenphong 
                                         WHERE manhom = @manhom";
                        }
                        else
                        {
                            // Kết nối từ Site Gốc
                            updateSql = $@"UPDATE {linkServerNguon}.{dbNguon}.dbo.nhomnc 
                                         SET tenphong = @tenphong 
                                         WHERE manhom = @manhom";
                        }
                        
                        SqlCommand cmd = new SqlCommand(updateSql, conn);
                        cmd.Parameters.AddWithValue("@tenphong", tenPhongMoi);
                        cmd.Parameters.AddWithValue("@manhom", maNhom);
                        
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    return;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi khi cập nhật tên phòng: {ex.Message}");
                }
            }

            // Các bước thực hiện chuyển nhóm (không dùng transaction)
            DataTable dtNhom = new DataTable();
            DataTable dtNV = new DataTable();
            DataTable dtDA = new DataTable();
            DataTable dtTG = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // BƯỚC 1: Kiểm tra trùng lặp trên Site đích
                    string checkSql;
                    if (linkServerDich.StartsWith("LOCAL"))
                    {
                        checkSql = @"SELECT COUNT(*) FROM nhomnc WHERE manhom = @manhom";
                    }
                    else
                    {
                        checkSql = $@"SELECT COUNT(*) FROM {linkServerDich}.{dbDich}.dbo.nhomnc WHERE manhom = @manhom";
                    }
                    
                    SqlCommand cmdCheck = new SqlCommand(checkSql, conn);
                    cmdCheck.Parameters.AddWithValue("@manhom", maNhom);
                    int count = (int)cmdCheck.ExecuteScalar();

                    if (count > 0)
                    {
                        throw new Exception($"Nhóm {maNhom} đã tồn tại trên Site đích! Không thể chuyển.");
                    }

                    // BƯỚC 2: Đọc dữ liệu từ Site nguồn
                    // 2.1. Nhóm
                    string selectNhomSql;
                    if (linkServerNguon.StartsWith("LOCAL"))
                    {
                        selectNhomSql = @"SELECT manhom, tennhom FROM nhomnc WHERE manhom = @manhom";
                    }
                    else
                    {
                        selectNhomSql = $@"SELECT manhom, tennhom FROM {linkServerNguon}.{dbNguon}.dbo.nhomnc WHERE manhom = @manhom";
                    }
                    
                    SqlCommand cmdNhom = new SqlCommand(selectNhomSql, conn);
                    cmdNhom.Parameters.AddWithValue("@manhom", maNhom);
                    SqlDataAdapter daNhom = new SqlDataAdapter(cmdNhom);
                    daNhom.Fill(dtNhom);

                    if (dtNhom.Rows.Count == 0)
                    {
                        throw new Exception($"Không tìm thấy nhóm {maNhom} trên Site nguồn");
                    }

                    // 2.2. Nhân viên
                    string selectNVSql;
                    if (linkServerNguon.StartsWith("LOCAL"))
                    {
                        selectNVSql = @"SELECT manv, hoten, manhom FROM nhanvien WHERE manhom = @manhom";
                    }
                    else
                    {
                        selectNVSql = $@"SELECT manv, hoten, manhom FROM {linkServerNguon}.{dbNguon}.dbo.nhanvien WHERE manhom = @manhom";
                    }
                    
                    SqlCommand cmdNV = new SqlCommand(selectNVSql, conn);
                    cmdNV.Parameters.AddWithValue("@manhom", maNhom);
                    SqlDataAdapter daNV = new SqlDataAdapter(cmdNV);
                    daNV.Fill(dtNV);

                    // 2.3. Đề án
                    string selectDASql;
                    if (linkServerNguon.StartsWith("LOCAL"))
                    {
                        selectDASql = @"SELECT mada, tenda, manhom FROM dean WHERE manhom = @manhom";
                    }
                    else
                    {
                        selectDASql = $@"SELECT mada, tenda, manhom FROM {linkServerNguon}.{dbNguon}.dbo.dean WHERE manhom = @manhom";
                    }
                    
                    SqlCommand cmdDA = new SqlCommand(selectDASql, conn);
                    cmdDA.Parameters.AddWithValue("@manhom", maNhom);
                    SqlDataAdapter daDA = new SqlDataAdapter(cmdDA);
                    daDA.Fill(dtDA);

                    // 2.4. Tham gia
                    if (dtNV.Rows.Count > 0)
                    {
                        string manvList = string.Join("','", dtNV.AsEnumerable().Select(r => r["manv"].ToString()));
                        string selectTGSql;
                        if (linkServerNguon.StartsWith("LOCAL"))
                        {
                            selectTGSql = $@"SELECT manv, mada FROM thamgia WHERE manv IN ('{manvList}')";
                        }
                        else
                        {
                            selectTGSql = $@"SELECT manv, mada FROM {linkServerNguon}.{dbNguon}.dbo.thamgia WHERE manv IN ('{manvList}')";
                        }
                        
                        SqlCommand cmdTG = new SqlCommand(selectTGSql, conn);
                        SqlDataAdapter daTG = new SqlDataAdapter(cmdTG);
                        daTG.Fill(dtTG);
                    }

                    // BƯỚC 3: Insert vào Site đích
                    // 3.1. Insert nhóm
                    string insertNhomSql;
                    if (linkServerDich.StartsWith("LOCAL"))
                    {
                        insertNhomSql = @"INSERT INTO nhomnc (manhom, tennhom, tenphong) VALUES (@manhom, @tennhom, @tenphong)";
                    }
                    else
                    {
                        insertNhomSql = $@"INSERT INTO {linkServerDich}.{dbDich}.dbo.nhomnc (manhom, tennhom, tenphong) 
                                         VALUES (@manhom, @tennhom, @tenphong)";
                    }
                    
                    SqlCommand cmdInsertNhom = new SqlCommand(insertNhomSql, conn);
                    cmdInsertNhom.Parameters.AddWithValue("@manhom", dtNhom.Rows[0]["manhom"]);
                    cmdInsertNhom.Parameters.AddWithValue("@tennhom", dtNhom.Rows[0]["tennhom"]);
                    cmdInsertNhom.Parameters.AddWithValue("@tenphong", tenPhongMoi);
                    cmdInsertNhom.ExecuteNonQuery();

                    // 3.2. Insert nhân viên
                    foreach (DataRow row in dtNV.Rows)
                    {
                        string insertNVSql;
                        if (linkServerDich.StartsWith("LOCAL"))
                        {
                            insertNVSql = @"INSERT INTO nhanvien (manv, hoten, manhom) VALUES (@manv, @hoten, @manhom)";
                        }
                        else
                        {
                            insertNVSql = $@"INSERT INTO {linkServerDich}.{dbDich}.dbo.nhanvien (manv, hoten, manhom) 
                                           VALUES (@manv, @hoten, @manhom)";
                        }
                        
                        SqlCommand cmdInsertNV = new SqlCommand(insertNVSql, conn);
                        cmdInsertNV.Parameters.AddWithValue("@manv", row["manv"]);
                        cmdInsertNV.Parameters.AddWithValue("@hoten", row["hoten"]);
                        cmdInsertNV.Parameters.AddWithValue("@manhom", row["manhom"]);
                        cmdInsertNV.ExecuteNonQuery();
                    }

                    // 3.3. Insert đề án
                    foreach (DataRow row in dtDA.Rows)
                    {
                        string insertDASql;
                        if (linkServerDich.StartsWith("LOCAL"))
                        {
                            insertDASql = @"INSERT INTO dean (mada, tenda, manhom) VALUES (@mada, @tenda, @manhom)";
                        }
                        else
                        {
                            insertDASql = $@"INSERT INTO {linkServerDich}.{dbDich}.dbo.dean (mada, tenda, manhom) 
                                           VALUES (@mada, @tenda, @manhom)";
                        }
                        
                        SqlCommand cmdInsertDA = new SqlCommand(insertDASql, conn);
                        cmdInsertDA.Parameters.AddWithValue("@mada", row["mada"]);
                        cmdInsertDA.Parameters.AddWithValue("@tenda", row["tenda"]);
                        cmdInsertDA.Parameters.AddWithValue("@manhom", row["manhom"]);
                        cmdInsertDA.ExecuteNonQuery();
                    }

                    // 3.4. Insert tham gia
                    foreach (DataRow row in dtTG.Rows)
                    {
                        string insertTGSql;
                        if (linkServerDich.StartsWith("LOCAL"))
                        {
                            insertTGSql = @"INSERT INTO thamgia (manv, mada) VALUES (@manv, @mada)";
                        }
                        else
                        {
                            insertTGSql = $@"INSERT INTO {linkServerDich}.{dbDich}.dbo.thamgia (manv, mada) 
                                           VALUES (@manv, @mada)";
                        }
                        
                        SqlCommand cmdInsertTG = new SqlCommand(insertTGSql, conn);
                        cmdInsertTG.Parameters.AddWithValue("@manv", row["manv"]);
                        cmdInsertTG.Parameters.AddWithValue("@mada", row["mada"]);
                        cmdInsertTG.ExecuteNonQuery();
                    }

                    // BƯỚC 4: Xóa từ Site nguồn (thứ tự: thamgia -> dean -> nhanvien -> nhomnc)
                    // 4.1. Xóa tham gia
                    if (dtNV.Rows.Count > 0)
                    {
                        string manvList = string.Join("','", dtNV.AsEnumerable().Select(r => r["manv"].ToString()));
                        string deleteTGSql;
                        if (linkServerNguon.StartsWith("LOCAL"))
                        {
                            deleteTGSql = $@"DELETE FROM thamgia WHERE manv IN ('{manvList}')";
                        }
                        else
                        {
                            deleteTGSql = $@"DELETE FROM {linkServerNguon}.{dbNguon}.dbo.thamgia WHERE manv IN ('{manvList}')";
                        }
                        
                        SqlCommand cmdDeleteTG = new SqlCommand(deleteTGSql, conn);
                        cmdDeleteTG.ExecuteNonQuery();
                    }

                    // 4.2. Xóa đề án
                    string deleteDASql;
                    if (linkServerNguon.StartsWith("LOCAL"))
                    {
                        deleteDASql = @"DELETE FROM dean WHERE manhom = @manhom";
                    }
                    else
                    {
                        deleteDASql = $@"DELETE FROM {linkServerNguon}.{dbNguon}.dbo.dean WHERE manhom = @manhom";
                    }
                    
                    SqlCommand cmdDeleteDA = new SqlCommand(deleteDASql, conn);
                    cmdDeleteDA.Parameters.AddWithValue("@manhom", maNhom);
                    cmdDeleteDA.ExecuteNonQuery();

                    // 4.3. Xóa nhân viên
                    string deleteNVSql;
                    if (linkServerNguon.StartsWith("LOCAL"))
                    {
                        deleteNVSql = @"DELETE FROM nhanvien WHERE manhom = @manhom";
                    }
                    else
                    {
                        deleteNVSql = $@"DELETE FROM {linkServerNguon}.{dbNguon}.dbo.nhanvien WHERE manhom = @manhom";
                    }
                    
                    SqlCommand cmdDeleteNV = new SqlCommand(deleteNVSql, conn);
                    cmdDeleteNV.Parameters.AddWithValue("@manhom", maNhom);
                    cmdDeleteNV.ExecuteNonQuery();

                    // 4.4. Xóa nhóm
                    string deleteNhomSql;
                    if (linkServerNguon.StartsWith("LOCAL"))
                    {
                        deleteNhomSql = @"DELETE FROM nhomnc WHERE manhom = @manhom";
                    }
                    else
                    {
                        deleteNhomSql = $@"DELETE FROM {linkServerNguon}.{dbNguon}.dbo.nhomnc WHERE manhom = @manhom";
                    }
                    
                    SqlCommand cmdDeleteNhom = new SqlCommand(deleteNhomSql, conn);
                    cmdDeleteNhom.Parameters.AddWithValue("@manhom", maNhom);
                    cmdDeleteNhom.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Nếu lỗi, cố gắng rollback thủ công (xóa dữ liệu đã insert trên Site đích)
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        
                        // Xóa dữ liệu đã insert (nếu có)
                        if (dtNV.Rows.Count > 0)
                        {
                            string manvList = string.Join("','", dtNV.AsEnumerable().Select(r => r["manv"].ToString()));
                            string rollbackTG;
                            if (linkServerDich.StartsWith("LOCAL"))
                            {
                                rollbackTG = $@"DELETE FROM thamgia WHERE manv IN ('{manvList}')";
                            }
                            else
                            {
                                rollbackTG = $@"DELETE FROM {linkServerDich}.{dbDich}.dbo.thamgia WHERE manv IN ('{manvList}')";
                            }
                            new SqlCommand(rollbackTG, conn).ExecuteNonQuery();
                        }

                        string rollbackDA;
                        if (linkServerDich.StartsWith("LOCAL"))
                        {
                            rollbackDA = @"DELETE FROM dean WHERE manhom = @manhom";
                        }
                        else
                        {
                            rollbackDA = $@"DELETE FROM {linkServerDich}.{dbDich}.dbo.dean WHERE manhom = @manhom";
                        }
                        SqlCommand cmdRollbackDA = new SqlCommand(rollbackDA, conn);
                        cmdRollbackDA.Parameters.AddWithValue("@manhom", maNhom);
                        cmdRollbackDA.ExecuteNonQuery();

                        string rollbackNV;
                        if (linkServerDich.StartsWith("LOCAL"))
                        {
                            rollbackNV = @"DELETE FROM nhanvien WHERE manhom = @manhom";
                        }
                        else
                        {
                            rollbackNV = $@"DELETE FROM {linkServerDich}.{dbDich}.dbo.nhanvien WHERE manhom = @manhom";
                        }
                        SqlCommand cmdRollbackNV = new SqlCommand(rollbackNV, conn);
                        cmdRollbackNV.Parameters.AddWithValue("@manhom", maNhom);
                        cmdRollbackNV.ExecuteNonQuery();

                        string rollbackNhom;
                        if (linkServerDich.StartsWith("LOCAL"))
                        {
                            rollbackNhom = @"DELETE FROM nhomnc WHERE manhom = @manhom";
                        }
                        else
                        {
                            rollbackNhom = $@"DELETE FROM {linkServerDich}.{dbDich}.dbo.nhomnc WHERE manhom = @manhom";
                        }
                        SqlCommand cmdRollbackNhom = new SqlCommand(rollbackNhom, conn);
                        cmdRollbackNhom.Parameters.AddWithValue("@manhom", maNhom);
                        cmdRollbackNhom.ExecuteNonQuery();
                    }
                }
                catch
                {
                    // Rollback thất bại - cần xử lý thủ công
                }

                throw new Exception($"Lỗi khi chuyển nhóm sang Site khác: {ex.Message}");
            }
        }

        /// <summary>
        /// Thêm nhóm nghiên cứu mới vào Site P1 hoặc P2
        /// </summary>
        public void ThemNhomNghienCuu(string maNhom, string tenNhom, string tenPhong, string linkServer)
        {
            try
            {
                // Kiểm tra mã nhóm đã tồn tại chưa
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string checkSql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        checkSql = "SELECT COUNT(*) FROM nhomnc WHERE manhom = @manhom";
                    }
                    else
                    {
                        checkSql = "SELECT COUNT(*) FROM nhomnc_view WHERE manhom = @manhom";
                    }
                    
                    SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@manhom", maNhom);
                    conn.Open();
                    int count = (int)checkCmd.ExecuteScalar();
                    
                    if (count > 0)
                    {
                        throw new Exception($"Mã nhóm '{maNhom}' đã tồn tại!");
                    }
                }

                // Thêm nhóm mới
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string insertSql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        // Kết nối trực tiếp vào Site P1/P2
                        insertSql = @"INSERT INTO nhomnc (manhom, tennhom, tenphong) 
                                     VALUES (@manhom, @tennhom, @tenphong)";
                    }
                    else
                    {
                        // Site Gốc: sử dụng cú pháp 4 phần
                        string database = linkServer == "LINK_P1" ? "QLDeAn_P1" : "QLDeAn_P2";
                        insertSql = $@"INSERT INTO {linkServer}.{database}.dbo.nhomnc (manhom, tennhom, tenphong) 
                                      VALUES (@manhom, @tennhom, @tenphong)";
                    }

                    SqlCommand cmd = new SqlCommand(insertSql, conn);
                    cmd.Parameters.AddWithValue("@manhom", maNhom);
                    cmd.Parameters.AddWithValue("@tennhom", tenNhom);
                    cmd.Parameters.AddWithValue("@tenphong", tenPhong);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm nhóm nghiên cứu: {ex.Message}");
            }
        }

        /// <summary>
        /// Tìm Site chứa nhóm nghiên cứu cụ thể
        /// Trả về LINK_P1 hoặc LINK_P2
        /// </summary>
        public string TimSiteNhom(string maNhom)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = "SELECT manhom FROM nhomnc_view WHERE manhom = @manhom";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@manhom", maNhom);

                    conn.Open();
                    var result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        throw new Exception($"Không tìm thấy nhóm nghiên cứu có mã '{maNhom}'");
                    }

                    // Kiểm tra xem nhóm thuộc site nào
                    // Giả định: NC01, NC02 thuộc P1; NC03, NC04 thuộc P2
                    string sql2 = @"
                        SELECT CASE 
                            WHEN EXISTS (SELECT 1 FROM LINK_P1.QLDeAn_P1.dbo.nhomnc WHERE manhom = @manhom) 
                            THEN 'LINK_P1'
                            WHEN EXISTS (SELECT 1 FROM LINK_P2.QLDeAn_P2.dbo.nhomnc WHERE manhom = @manhom)
                            THEN 'LINK_P2'
                            ELSE NULL
                        END AS LinkServer";

                    SqlCommand cmd2 = new SqlCommand(sql2, conn);
                    cmd2.Parameters.AddWithValue("@manhom", maNhom);

                    var site = cmd2.ExecuteScalar()?.ToString();
                    return site ?? throw new Exception("Không xác định được Site của nhóm");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tìm Site: {ex.Message}");
            }
        }

        #region CRUD Nhân Viên

        /// <summary>
        /// Thêm nhân viên mới
        /// Logic: Tự động thêm vào Site đúng dựa trên mã nhóm
        /// </summary>
        public void ThemNhanVien(string maNV, string hoTen, string maNhom)
        {
            try
            {
                // Kiểm tra mã nhân viên đã tồn tại chưa
                if (KiemTraNhanVienTonTai(maNV))
                {
                    throw new Exception($"Mã nhân viên '{maNV}' đã tồn tại!");
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string insertSql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        // Kết nối trực tiếp vào Site P1/P2: INSERT local
                        insertSql = @"INSERT INTO nhanvien (manv, hoten, manhom) 
                                     VALUES (@manv, @hoten, @manhom)";
                    }
                    else
                    {
                        // Kết nối vào Site Gốc: dùng cú pháp 4 phần
                        string linkServer = TimSiteNhom(maNhom);
                        string database = linkServer == "LINK_P1" ? "QLDeAn_P1" : "QLDeAn_P2";
                        insertSql = $@"INSERT INTO {linkServer}.{database}.dbo.nhanvien (manv, hoten, manhom) 
                                      VALUES (@manv, @hoten, @manhom)";
                    }

                    SqlCommand cmd = new SqlCommand(insertSql, conn);
                    cmd.Parameters.AddWithValue("@manv", maNV);
                    cmd.Parameters.AddWithValue("@hoten", hoTen);
                    cmd.Parameters.AddWithValue("@manhom", maNhom);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm nhân viên: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// Logic: Không cho phép đổi nhóm sang Site khác
        /// </summary>
        public void SuaNhanVien(string maNV, string hoTen, string maNhomMoi)
        {
            try
            {
                // Lấy thông tin nhân viên hiện tại
                var nhanVienHienTai = LayThongTinNhanVien(maNV);
                if (nhanVienHienTai == null)
                {
                    throw new Exception($"Không tìm thấy nhân viên có mã '{maNV}'");
                }

                string maNhomCu = nhanVienHienTai["manhom"].ToString()!;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string updateSql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        // Kết nối trực tiếp vào Site P1/P2: UPDATE local
                        // Kiểm tra nhóm mới có tồn tại trong site này không
                        string checkSql = "SELECT COUNT(*) FROM nhomnc WHERE manhom = @manhom";
                        SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                        checkCmd.Parameters.AddWithValue("@manhom", maNhomMoi);
                        conn.Open();
                        int count = (int)checkCmd.ExecuteScalar();
                        
                        if (count == 0)
                        {
                            throw new Exception($"Không thể chuyển nhân viên sang nhóm '{maNhomMoi}' - nhóm không tồn tại trong site này!");
                        }
                        
                        updateSql = @"UPDATE nhanvien 
                                     SET hoten = @hoten, manhom = @manhom 
                                     WHERE manv = @manv";
                    }
                    else
                    {
                        // Kết nối vào Site Gốc: kiểm tra site và dùng EXEC AT
                        string siteNhomCu = TimSiteNhom(maNhomCu);
                        string siteNhomMoi = TimSiteNhom(maNhomMoi);

                        if (siteNhomCu != siteNhomMoi)
                        {
                            throw new Exception($"Không thể chuyển nhân viên sang nhóm thuộc Site khác! " +
                                              $"Nhóm hiện tại ({maNhomCu}) thuộc {siteNhomCu}, " +
                                              $"Nhóm mới ({maNhomMoi}) thuộc {siteNhomMoi}");
                        }

                        string database = siteNhomCu == "LINK_P1" ? "QLDeAn_P1" : "QLDeAn_P2";
                        updateSql = $@"UPDATE {siteNhomCu}.{database}.dbo.nhanvien 
                                      SET hoten = @hoten, manhom = @manhom 
                                      WHERE manv = @manv";
                    }

                    SqlCommand cmd = new SqlCommand(updateSql, conn);
                    cmd.Parameters.AddWithValue("@manv", maNV);
                    cmd.Parameters.AddWithValue("@hoten", hoTen);
                    cmd.Parameters.AddWithValue("@manhom", maNhomMoi);
                    
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();
                    
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi sửa nhân viên: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa nhân viên
        /// Logic: Kiểm tra ràng buộc với bảng thamgia trước khi xóa
        /// </summary>
        public void XoaNhanVien(string maNV)
        {
            try
            {
                // Kiểm tra nhân viên có đang tham gia đề án nào không
                if (KiemTraNhanVienDangThamGiaDean(maNV))
                {
                    throw new Exception($"Không thể xóa nhân viên '{maNV}' vì đang tham gia đề án!");
                }

                // Lấy thông tin nhân viên để xác định Site
                var nhanVienInfo = LayThongTinNhanVien(maNV);
                if (nhanVienInfo == null)
                {
                    throw new Exception($"Không tìm thấy nhân viên có mã '{maNV}'");
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string deleteSql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        // Kết nối trực tiếp vào Site P1/P2: DELETE local
                        deleteSql = "DELETE FROM nhanvien WHERE manv = @manv";
                    }
                    else
                    {
                        // Kết nối vào Site Gốc: dùng cú pháp 4 phần
                        string maNhom = nhanVienInfo["manhom"].ToString()!;
                        string linkServer = TimSiteNhom(maNhom);
                        string database = linkServer == "LINK_P1" ? "QLDeAn_P1" : "QLDeAn_P2";
                        deleteSql = $@"DELETE FROM {linkServer}.{database}.dbo.nhanvien 
                                      WHERE manv = @manv";
                    }

                    SqlCommand cmd = new SqlCommand(deleteSql, conn);
                    cmd.Parameters.AddWithValue("@manv", maNV);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa nhân viên: {ex.Message}");
            }
        }

        /// <summary>
        /// Kiểm tra nhân viên đã tồn tại chưa
        /// </summary>
        private bool KiemTraNhanVienTonTai(string maNV)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        // Truy vấn trực tiếp bảng nhanvien trên Site P1/P2
                        sql = "SELECT COUNT(*) FROM nhanvien WHERE manv = @manv";
                    }
                    else
                    {
                        // Truy vấn View trên Site Gốc
                        sql = "SELECT COUNT(*) FROM nhanvien_view WHERE manv = @manv";
                    }
                    
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@manv", maNV);

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy thông tin nhân viên
        /// </summary>
        private DataRow? LayThongTinNhanVien(string maNV)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        // Truy vấn trực tiếp bảng nhanvien trên Site P1/P2
                        sql = "SELECT * FROM nhanvien WHERE manv = @manv";
                    }
                    else
                    {
                        // Truy vấn View trên Site Gốc
                        sql = "SELECT * FROM nhanvien_view WHERE manv = @manv";
                    }
                    
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@manv", maNV);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Kiểm tra nhân viên có đang tham gia đề án không
        /// </summary>
        private bool KiemTraNhanVienDangThamGiaDean(string maNV)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        // Truy vấn trực tiếp bảng thamgia trên Site P1/P2
                        sql = "SELECT COUNT(*) FROM thamgia WHERE manv = @manv";
                    }
                    else
                    {
                        // Truy vấn View trên Site Gốc
                        sql = "SELECT COUNT(*) FROM thamgia_view WHERE manv = @manv";
                    }
                    
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@manv", maNV);

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy danh sách nhóm theo Site
        /// </summary>
        public DataTable LayDanhSachNhomTheoSite(string linkServer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        // Nếu đang kết nối trực tiếp vào Site P1/P2, query local
                        sql = "SELECT manhom, tennhom FROM nhomnc ORDER BY manhom";
                    }
                    else
                    {
                        // Site Gốc: query qua Link Server
                        string database = linkServer == "LINK_P1" ? "QLDeAn_P1" : "QLDeAn_P2";
                        sql = $"SELECT manhom, tennhom FROM {linkServer}.{database}.dbo.nhomnc ORDER BY manhom";
                    }

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhóm: {ex.Message}");
            }
        }

        #endregion

        #region CRUD Tham Gia (Nhân viên tham gia đề án)

        /// <summary>
        /// Lấy danh sách tất cả nhân viên (để chọn thêm vào đề án)
        /// </summary>
        public DataTable LayDanhSachTatCaNhanVien()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        sql = "SELECT manv, hoten, manhom FROM nhanvien ORDER BY manv";
                    }
                    else
                    {
                        sql = "SELECT manv, hoten, manhom FROM nhanvien_view ORDER BY manv";
                    }
                    
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách nhân viên: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả đề án
        /// </summary>
        public DataTable LayDanhSachTatCaDean()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        sql = "SELECT mada, tenda, manhom FROM dean ORDER BY mada";
                    }
                    else
                    {
                        sql = "SELECT mada, tenda, manhom FROM dean_view ORDER BY mada";
                    }
                    
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách đề án: {ex.Message}");
            }
        }

        /// <summary>
        /// Thêm nhân viên tham gia đề án
        /// Logic: Nhân viên và đề án phải cùng Site
        /// </summary>
        public void ThemThamGia(string maNV, string maDA)
        {
            try
            {
                // Kiểm tra nhân viên đã tham gia đề án này chưa
                if (KiemTraThamGiaTonTai(maNV, maDA))
                {
                    throw new Exception($"Nhân viên '{maNV}' đã tham gia đề án '{maDA}'!");
                }

                // Lấy thông tin nhân viên và đề án để kiểm tra logic
                var nhanVienInfo = LayThongTinNhanVien(maNV);
                if (nhanVienInfo == null)
                {
                    throw new Exception($"Không tìm thấy nhân viên có mã '{maNV}'");
                }

                var deanInfo = LayThongTinDean(maDA);
                if (deanInfo == null)
                {
                    throw new Exception($"Không tìm thấy đề án có mã '{maDA}'");
                }

                string maNhomNV = nhanVienInfo["manhom"].ToString()!;
                string maNhomDA = deanInfo["manhom"].ToString()!;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string insertSql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        // Kết nối trực tiếp vào Site P1/P2: INSERT local
                        // Kiểm tra cả nhân viên và đề án đều thuộc site này
                        string checkNV = "SELECT COUNT(*) FROM nhanvien WHERE manv = @manv";
                        string checkDA = "SELECT COUNT(*) FROM dean WHERE mada = @mada";
                        
                        SqlCommand checkCmd = new SqlCommand(checkNV, conn);
                        checkCmd.Parameters.AddWithValue("@manv", maNV);
                        conn.Open();
                        int countNV = (int)checkCmd.ExecuteScalar();
                        
                        checkCmd.CommandText = checkDA;
                        checkCmd.Parameters.Clear();
                        checkCmd.Parameters.AddWithValue("@mada", maDA);
                        int countDA = (int)checkCmd.ExecuteScalar();
                        
                        if (countNV == 0 || countDA == 0)
                        {
                            throw new Exception($"Nhân viên '{maNV}' và đề án '{maDA}' phải cùng thuộc site này!");
                        }
                        
                        insertSql = "INSERT INTO thamgia (manv, mada) VALUES (@manv, @mada)";
                    }
                    else
                    {
                        // Site Gốc: Kiểm tra cùng site
                        string siteNV = TimSiteNhom(maNhomNV);
                        string siteDA = TimSiteNhom(maNhomDA);

                        if (siteNV != siteDA)
                        {
                            throw new Exception($"Không thể thêm! Nhân viên '{maNV}' (nhóm {maNhomNV}) thuộc {siteNV}, " +
                                              $"nhưng đề án '{maDA}' (nhóm {maNhomDA}) thuộc {siteDA}. " +
                                              $"Nhân viên và đề án phải cùng Site!");
                        }

                        // Sử dụng cú pháp 4 phần thay vì EXEC AT (không cần RPC)
                        string database = siteNV == "LINK_P1" ? "QLDeAn_P1" : "QLDeAn_P2";
                        insertSql = $@"INSERT INTO {siteNV}.{database}.dbo.thamgia (manv, mada) 
                                      VALUES (@manv, @mada)";
                    }

                    SqlCommand cmd = new SqlCommand(insertSql, conn);
                    
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();
                        
                    cmd.Parameters.AddWithValue("@manv", maNV);
                    cmd.Parameters.AddWithValue("@mada", maDA);
                    
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm tham gia: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa nhân viên khỏi đề án
        /// </summary>
        public void XoaThamGia(string maNV, string maDA)
        {
            try
            {
                if (!KiemTraThamGiaTonTai(maNV, maDA))
                {
                    throw new Exception($"Không tìm thấy bản ghi nhân viên '{maNV}' tham gia đề án '{maDA}'");
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string deleteSql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        deleteSql = "DELETE FROM thamgia WHERE manv = @manv AND mada = @mada";
                    }
                    else
                    {
                        // Xác định site của nhân viên
                        var nhanVienInfo = LayThongTinNhanVien(maNV);
                        if (nhanVienInfo == null)
                        {
                            throw new Exception($"Không tìm thấy nhân viên có mã '{maNV}'");
                        }

                        string maNhom = nhanVienInfo["manhom"].ToString()!;
                        string linkServer = TimSiteNhom(maNhom);
                        string database = linkServer == "LINK_P1" ? "QLDeAn_P1" : "QLDeAn_P2";
                        
                        // Sử dụng cú pháp 4 phần thay vì EXEC AT
                        deleteSql = $@"DELETE FROM {linkServer}.{database}.dbo.thamgia 
                                      WHERE manv = @manv AND mada = @mada";
                    }

                    SqlCommand cmd = new SqlCommand(deleteSql, conn);
                    cmd.Parameters.AddWithValue("@manv", maNV);
                    cmd.Parameters.AddWithValue("@mada", maDA);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa tham gia: {ex.Message}");
            }
        }

        /// <summary>
        /// Kiểm tra nhân viên đã tham gia đề án chưa
        /// </summary>
        private bool KiemTraThamGiaTonTai(string maNV, string maDA)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        sql = "SELECT COUNT(*) FROM thamgia WHERE manv = @manv AND mada = @mada";
                    }
                    else
                    {
                        sql = "SELECT COUNT(*) FROM thamgia_view WHERE manv = @manv AND mada = @mada";
                    }
                    
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@manv", maNV);
                    cmd.Parameters.AddWithValue("@mada", maDA);

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy thông tin đề án
        /// </summary>
        private DataRow? LayThongTinDean(string maDA)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql;
                    
                    if (databaseName == "QLDeAn_P1" || databaseName == "QLDeAn_P2")
                    {
                        sql = "SELECT * FROM dean WHERE mada = @mada";
                    }
                    else
                    {
                        sql = "SELECT * FROM dean_view WHERE mada = @mada";
                    }
                    
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@mada", maDA);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
