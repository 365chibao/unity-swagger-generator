# Hướng Dẫn Đẩy Lên GitHub & Phát Hành Trên OpenUPM (Miễn Phí)

Tài liệu này ghi lại các bước chi tiết để bạn đưa dự án `unity-swagger-generator` lên GitHub và đăng ký phát hành miễn phí trên hệ thống OpenUPM cho cộng đồng sử dụng.

---

## BƯỚC 1: Đẩy Mã Nguồn Lên GitHub

### 1. Tạo file `.gitignore` để tránh đẩy file thừa
Trước khi đẩy code lên Git, cần tạo file `.gitignore` để bỏ qua các file đóng gói tạm thời (`.tgz`) hoặc thư mục kiểm thử. 
Tôi đã tạo sẵn file `.gitignore` cho bạn ở thư mục gốc với nội dung:
```text
# Bỏ qua file nén đóng gói cục bộ
*.tgz

# Bỏ qua các file cấu hình AI cá nhân (nếu không muốn chia sẻ)
.gemini/
```

### 2. Khởi tạo Git và Commit code tại máy local
Mở terminal (Git Bash, Command Prompt hoặc PowerShell) tại thư mục `tool_unity-swagger-generator` và chạy các lệnh:
```bash
# Khởi tạo Git repo
git init

# Thêm tất cả các file vào hàng đợi commit (trừ các file trong .gitignore)
git add .

# Tạo commit đầu tiên
git commit -m "First release: Unity Swagger DTO Generator UPM package"
```

### 3. Tạo Repository trên GitHub và Push code
1. Đăng nhập vào tài khoản [GitHub](https://github.com).
2. Click chọn **`New`** để tạo một repository mới.
3. Cấu hình repository:
   * **Repository name**: `unity-swagger-generator` (hoặc tên tùy chọn).
   * **Public/Private**: **Bắt buộc chọn `Public`** để có thể phát hành miễn phí trên OpenUPM.
   * *Không tích chọn các mục Add README, .gitignore hay license* vì chúng ta đã có sẵn.
4. Bấm **`Create repository`**.
5. Copy đường link Git repository vừa được tạo (dạng: `https://github.com/tai-khoan-cua-ban/unity-swagger-generator.git`).
6. Chạy tiếp các lệnh sau tại terminal máy local:
   ```bash
   # Thiết lập nhánh chính tên là main
   git branch -M main

   # Liên kết local repo với repo trên GitHub (Thay link bằng link bạn vừa copy)
   git remote add origin https://github.com/tai-khoan-cua-ban/unity-swagger-generator.git

   # Đẩy code lên GitHub
   git push -u origin main
   ```

---

## BƯỚC 2: Phát Hành Miễn Phí Trên OpenUPM

Sau khi code đã lên GitHub công khai, bạn có thể đăng ký phát hành lên OpenUPM rất đơn giản:

1. Truy cập vào trang web: **`https://openupm.com/packages/add/`**
2. Điền thông tin vào biểu mẫu:
   * **GitHub Repository URL**: Nhập link GitHub của bạn (ví dụ: `https://github.com/tai-khoan-cua-ban/unity-swagger-generator`).
   * **Parent Branch/Tag**: Nhập tên nhánh chính là **`main`**.
   * **Sub-directory**: Để trống (vì file `package.json` nằm ngay thư mục gốc).
   * **Package Name**: Điền đúng tên khai báo trong file `package.json` của bạn (ở đây là **`com.swagger-dto-generator`**).
   * **Category**: Chọn danh mục phù hợp (ví dụ: `utility` or `editor-tool`).
3. Click vào nút **`Submit`**.
4. Hệ thống OpenUPM sẽ xếp lịch build tự động (thường mất 1 - 2 phút). Khi build thành công, Package của bạn sẽ chính thức xuất hiện trên trang chủ OpenUPM!

---

## BƯỚC 3: Cách Người Dùng Tải Về Từ OpenUPM

Khi bạn đã phát hành thành công trên OpenUPM, người dùng trên thế giới có hai cách cực kỳ đơn giản để cài đặt tool của bạn vào dự án Unity của họ:

### Cách A: Cài đặt qua giao diện Unity (Dễ nhất cho người dùng)
Họ chỉ cần thêm OpenUPM vào danh sách nguồn tải (Scoped Registries) trong dự án Unity của họ bằng cách:
1. Trong Unity, mở **`Edit -> Project Settings -> Package Manager`**.
2. Thêm một Registry mới:
   * **Name**: `package.openupm.com`
   * **URL**: `https://package.openupm.com`
   * **Scope(s)**: Thêm scope **`com.swagger-dto-generator`**
3. Bấm **Apply**.
4. Mở cửa sổ **`Window -> Package Manager`**, chuyển bộ lọc sang **`Packages: My Registries`**, chọn **`Swagger DTO Generator`** và bấm **`Install`**.

### Cách B: Cài đặt nhanh qua OpenUPM CLI
Nếu người dùng sử dụng Node.js và OpenUPM CLI, họ chỉ cần mở terminal tại thư mục dự án Unity và chạy lệnh:
```bash
openupm add com.swagger-dto-generator
```
Hệ thống sẽ tự động thêm và cấu hình package vào dự án Unity của họ mà không cần mở Unity lên cấu hình thủ công!
