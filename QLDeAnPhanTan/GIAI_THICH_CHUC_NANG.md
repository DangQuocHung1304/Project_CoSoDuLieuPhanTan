# GIẢI THÍCH CÁC CHỨC NĂNG CHÍNH

## Câu 1: Truy vấn phân tán (2 mức độ trong suốt)

### Mục đích
Tìm các đề án thuộc nhóm X mà có nhân viên của nhóm khác tham gia.

### Ví dụ thực tế
- Nhóm NC01 (AI & Học Máy) có đề án DA001
- Nhân viên NV0005 thuộc nhóm NC04 (Dữ Liệu Lớn) nhưng tham gia đề án DA001
- → DA001 là đề án cần tìm (thuộc NC01 nhưng có NV của nhóm khác)

### Mức 1: TRONG SUỐT PHÂN MẢNH (Sử dụng View)
```sql
-- Kết nối: Site Gốc (MSI\YLC, DB: QLDeAn)
-- Sử dụng View toàn cục đã được tạo sẵn

SELECT DISTINCT DA.mada, DA.tenda 
FROM dean_view DA 
JOIN thamgia_view TG ON DA.mada = TG.mada 
JOIN nhanvien_view NV ON TG.manv = NV.manv 
WHERE DA.manhom = @manhom       -- Đề án thuộc nhóm X
  AND NV.manhom <> @manhom      -- Nhân viên KHÔNG thuộc nhóm X
```

**Đặc điểm:**
- ✅ Đơn giản, dễ sử dụng
- ✅ View che giấu sự phân mảnh
- ❌ Phụ thuộc vào View (phải tạo trước)

### Mức 2: TRONG SUỐT VỊ TRÍ (Sử dụng UNION ALL + 4-part syntax)
```sql
-- Kết nối: Site Gốc (MSI\YLC, DB: QLDeAn)
-- Không dùng View, truy vấn trực tiếp từ các Site

SELECT DISTINCT A.mada, A.tenda 
FROM 
(
    -- UNION đề án từ P1 và P2
    SELECT mada, tenda, manhom
    FROM LINK_P1.QLDeAn_P1.dbo.dean 
    UNION ALL 
    SELECT mada, tenda, manhom
    FROM LINK_P2.QLDeAn_P2.dbo.dean
) AS A
JOIN 
(
    -- UNION tham gia từ P1 và P2
    SELECT manv, mada
    FROM LINK_P1.QLDeAn_P1.dbo.thamgia 
    UNION ALL 
    SELECT manv, mada
    FROM LINK_P2.QLDeAn_P2.dbo.thamgia
) AS TG ON A.mada = TG.mada
JOIN 
(
    -- UNION nhân viên từ P1 và P2
    SELECT manv, manhom
    FROM LINK_P1.QLDeAn_P1.dbo.nhanvien 
    UNION ALL 
    SELECT manv, manhom
    FROM LINK_P2.QLDeAn_P2.dbo.nhanvien
) AS NV ON TG.manv = NV.manv
WHERE A.manhom = @manhom AND NV.manhom <> @manhom
```

**Đặc điểm:**
- ✅ Không phụ thuộc View
- ✅ Linh hoạt hơn
- ❌ Phức tạp hơn
- ❌ Phải biết cấu trúc các Site

**Lưu ý quan trọng:**
- Cả 2 mức đều cho KẾT QUẢ GIỐNG NHAU
- Chỉ khác cách thực hiện (View vs UNION ALL)
- Mức 1 phù hợp cho người dùng cuối
- Mức 2 phù hợp cho lập trình viên

---

## Câu 2: Chuyển nhóm giữa các Site (Cập nhật phân tán)

### Bản chất chức năng
**Đây KHÔNG phải là chỉ đổi tên phòng!**

Đây là chức năng **CHUYỂN TOÀN BỘ NHÓM** từ Site này sang Site khác.

### Tại sao?
Trong CSDL phân tán theo phòng ban:
- **tenphong = "P1"** → Dữ liệu lưu tại Site P1 (QLDeAn_P1)
- **tenphong = "P2"** → Dữ liệu lưu tại Site P2 (QLDeAn_P2)

→ Đổi tenphong = đổi Site lưu trữ!

### Ví dụ thực tế
```
TRƯỚC KHI CHUYỂN:
================
Site P2 (LINK_P2):
- Nhóm NC04: "Nhóm Dữ Liệu Lớn" (tenphong = "P2")
- Nhân viên NV0005: "Lê Thị Mai" (thuộc NC04)
- Đề án: (nếu có)
- Tham gia: (nếu có)

Site P1 (LINK_P1):
- Nhóm NC01, NC02
- Nhân viên, đề án của NC01, NC02

SAU KHI CHUYỂN NC04 SANG P1:
============================
Site P1 (LINK_P1):
- Nhóm NC01, NC02, NC04 ← MỚI CHUYỂN ĐẾN
- Nhân viên NV0005 ← MỚI CHUYỂN ĐẾN
- Đề án (nếu có) ← MỚI CHUYỂN ĐẾN
- Tham gia ← MỚI CHUYỂN ĐẾN

Site P2 (LINK_P2):
- Nhóm NC03
- (NC04 đã bị XÓA)
```

### Quy trình chuyển nhóm (8 bước)

#### Bước 1: Xác định Site nguồn và Site đích
```csharp
string linkServerNguon = TimSiteNhom(maNhom);  // VD: LINK_P2
string linkServerDich = (tenPhongMoi == "P1") ? "LINK_P1" : "LINK_P2";
```

#### Bước 2: Kiểm tra trùng lặp
```sql
SELECT COUNT(*) FROM LINK_P1.QLDeAn_P1.dbo.nhomnc WHERE manhom = 'NC04'
```
→ Nếu > 0: BÁO LỖI (không thể chuyển)

#### Bước 3: Copy NHÓM sang Site đích
```sql
INSERT INTO LINK_P1.QLDeAn_P1.dbo.nhomnc (manhom, tennhom, tenphong)
SELECT manhom, tennhom, 'P1'
FROM LINK_P2.QLDeAn_P2.dbo.nhomnc 
WHERE manhom = 'NC04'
```

#### Bước 4: Copy NHÂN VIÊN thuộc nhóm
```sql
INSERT INTO LINK_P1.QLDeAn_P1.dbo.nhanvien (manv, hoten, manhom)
SELECT manv, hoten, manhom
FROM LINK_P2.QLDeAn_P2.dbo.nhanvien
WHERE manhom = 'NC04'
```

#### Bước 5: Copy ĐỀ ÁN của nhóm
```sql
INSERT INTO LINK_P1.QLDeAn_P1.dbo.dean (mada, tenda, manhom)
SELECT mada, tenda, manhom
FROM LINK_P2.QLDeAn_P2.dbo.dean
WHERE manhom = 'NC04'
```

#### Bước 6: Copy THAM GIA của nhân viên
```sql
-- Lấy danh sách mã NV của nhóm NC04
-- VD: NV0005
INSERT INTO LINK_P1.QLDeAn_P1.dbo.thamgia (manv, mada)
SELECT manv, mada
FROM LINK_P2.QLDeAn_P2.dbo.thamgia
WHERE manv IN ('NV0005')  -- Nhân viên thuộc NC04
```

#### Bước 7: Xóa THAM GIA từ Site nguồn
```sql
DELETE FROM LINK_P2.QLDeAn_P2.dbo.thamgia WHERE manv IN ('NV0005')
```

#### Bước 8: Xóa lần lượt từ Site nguồn
```sql
-- Thứ tự quan trọng (do Foreign Key):
DELETE FROM LINK_P2.QLDeAn_P2.dbo.dean WHERE manhom = 'NC04'
DELETE FROM LINK_P2.QLDeAn_P2.dbo.nhanvien WHERE manhom = 'NC04'
DELETE FROM LINK_P2.QLDeAn_P2.dbo.nhomnc WHERE manhom = 'NC04'
```

### Ràng buộc và xử lý

#### 1. Ràng buộc khóa ngoại (Foreign Key)
```
nhanvien.manhom → nhomnc.manhom
dean.manhom → nhomnc.manhom
thamgia.manv → nhanvien.manv
thamgia.mada → dean.mada
```
→ Phải xóa theo thứ tự: thamgia → dean → nhanvien → nhomnc

#### 2. Ràng buộc khóa chính (Primary Key)
- Kiểm tra trùng `manhom` trên Site đích
- Kiểm tra trùng `manv`, `mada` trên Site đích

#### 3. Transaction (ACID)
```csharp
SqlTransaction transaction = conn.BeginTransaction();
try 
{
    // Thực hiện 8 bước
    transaction.Commit();
}
catch 
{
    transaction.Rollback();  // Hoàn tác nếu lỗi
    throw;
}
```
→ Đảm bảo: Hoặc chuyển TOÀN BỘ, hoặc KHÔNG chuyển gì cả

### Trường hợp đặc biệt

#### Case 1: Cùng Site (P1 → P1 hoặc P2 → P2)
```
Nhóm NC04 đang ở P2, nhập tên phòng mới = "P2"
→ CHỈ cập nhật tenphong (không chuyển dữ liệu)
```

#### Case 2: Nhóm có nhân viên tham gia đề án ở Site khác
```
VD: NV0005 (thuộc NC04 ở P2) tham gia DA001 (ở P1)

Khi chuyển NC04 từ P2 → P1:
- NV0005 chuyển sang P1 ✓
- Tham gia (NV0005, DA001) chuyển sang P1 ✓
- DA001 VẪN Ở P1 (không di chuyển) ✓
```

#### Case 3: Nhóm không có nhân viên/đề án
```
Nhóm trống → Chỉ chuyển bản ghi nhomnc
```

### Kiểm tra sau khi chuyển

```sql
-- 1. Kiểm tra nhóm đã chuyển
SELECT * FROM LINK_P1.QLDeAn_P1.dbo.nhomnc WHERE manhom = 'NC04'
SELECT * FROM LINK_P2.QLDeAn_P2.dbo.nhomnc WHERE manhom = 'NC04'  -- NULL

-- 2. Kiểm tra nhân viên
SELECT * FROM LINK_P1.QLDeAn_P1.dbo.nhanvien WHERE manhom = 'NC04'

-- 3. Kiểm tra View toàn cục (tự động cập nhật)
SELECT * FROM nhomnc_view WHERE manhom = 'NC04'  -- Site: LINK_P1
```

---

## So sánh hai chức năng

| Tiêu chí | Câu 1: Truy vấn | Câu 2: Chuyển nhóm |
|----------|-----------------|-------------------|
| **Mục đích** | Đọc dữ liệu | Ghi/Cập nhật dữ liệu |
| **Tính phức tạp** | Đơn giản (SELECT) | Phức tạp (INSERT + DELETE + Transaction) |
| **Ảnh hưởng** | Không thay đổi DB | Thay đổi nhiều bảng, nhiều Site |
| **Rollback** | Không cần | Bắt buộc (Transaction) |
| **Ràng buộc** | Không kiểm tra | Phải kiểm tra FK, PK |
| **Thời gian** | Nhanh | Chậm hơn (nhiều thao tác) |

---

## Kết luận

### Câu 1 (Truy vấn):
- **Dễ hiểu**: Chỉ là câu SELECT phức tạp
- **2 cách thực hiện**: View (Mức 1) hoặc UNION ALL (Mức 2)
- **Không rủi ro**: Không làm thay đổi dữ liệu

### Câu 2 (Chuyển nhóm):
- **Phức tạp**: Di chuyển dữ liệu giữa các Site
- **Nguy hiểm**: Có thể làm mất dữ liệu nếu không cẩn thận
- **Quan trọng**: 
  * Transaction để đảm bảo ACID
  * Kiểm tra ràng buộc kỹ lưỡng
  * Xóa theo đúng thứ tự (FK)
  * Hiển thị rõ ràng cho người dùng điều gì sẽ xảy ra

**Lưu ý quan trọng nhất:**
> Trong CSDL phân tán theo địa lý (horizontal partitioning by location),
> việc thay đổi thuộc tính quyết định phân mảnh (tenphong) 
> = chuyển dữ liệu sang Site khác!
