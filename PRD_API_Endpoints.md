# API PRD (Product Requirements Document)

## 1️⃣ **Giriş**
Bu doküman, PostgreSQL veritabanına bağlı olarak geliştirilecek .NET Core API'nin gereksinimlerini ve uç noktalarını tanımlar. API, **kullanıcı yönetimi, lisans yönetimi, araç & ECU verisi yönetimi, HEX modifikasyonları ve API loglarını** kapsayacaktır.

---

## 2️⃣ **API Modülleri**
API, aşağıdaki ana modüllerden oluşacaktır:
- [x] **Kullanıcı Yönetimi** (`/api/users`)
- [x] **Lisans Yönetimi** (`/api/licenses`)
- [x] **Araç ve ECU Yönetimi** (`/api/vehicles`, `/api/ecu_models`)
- [x] **Yazılım Güncellemeleri (TuneCatalog)** (`/api/tune_catalog`)
- [x] **HEX Modifikasyonları (Diff Modifications)** (`/api/diff_modifications`)
- [x] **API Logları** (`/api/logs`)

---

## 3️⃣ **API Uç Noktaları (Endpoints)**

### **1. Kullanıcı Yönetimi** (`/api/users`)
- [x] **`POST /register`** → Yeni kullanıcı kaydı oluşturur.
- [x] **`POST /login`** → Kullanıcı giriş yapar ve token döner.
- [x] **`GET /profile`** → Giriş yapan kullanıcının bilgilerini getirir.
- [x] **`GET /{id}`** → Belirli bir kullanıcının bilgilerini getirir.
- [x] **`PUT /{id}`** → Kullanıcı bilgilerini günceller.
- [x] **`DELETE /{id}`** → Kullanıcıyı siler.

### **2. Lisans Yönetimi** (`/api/licenses`)
- [x] **`POST /assign`** → Kullanıcıya yeni lisans atar.
- [x] **`GET /{userId}`** → Belirli bir kullanıcının lisanslarını getirir.
- [x] **`PUT /{id}/deactivate`** → Lisansı devre dışı bırakır.
- [x] **`DELETE /{id}`** → Lisansı siler.

### **3. Araç ve ECU Yönetimi**
#### **Araçlar (`/api/vehicles`)**
- [x] **`POST /`** → Yeni araç ekler.
- [x] **`GET /`** → Tüm araçları getirir.
- [x] **`GET /{id}`** → Belirli bir aracı getirir.
- [x] **`PUT /{id}`** → Araç bilgilerini günceller.
- [x] **`DELETE /{id}`** → Aracı siler.

#### **ECU Modelleri (`/api/ecu_models`)**
- [x] **`POST /`** → Yeni ECU modeli ekler.
- [x] **`GET /`** → Tüm ECU modellerini getirir.
- [x] **`GET /{id}`** → Belirli bir ECU modelini getirir.
- [x] **`DELETE /{id}`** → ECU modelini siler.

#### **Araç-ECU İlişkisi (`/api/vehicle_ecumodel`)**
- [x] **`POST /assign`** → Araca ECU modeli bağlar.
- [x] **`GET /{vehicleId}`** → Belirli bir aracın ECU modellerini getirir.

### **4. Yazılım Güncellemeleri (`/api/tune_catalog`)**
- [x] **`POST /`** → Yeni yazılım güncellemesi ekler.
- [x] **`GET /ecu/{ecuModelId}`** → Belirli bir ECU modeline ait yazılım güncellemelerini getirir.
- [x] **`DELETE /{id}`** → Yazılım güncellemesini siler.

### **5. HEX Modifikasyonları (`/api/diff_modifications`)**
- [x] **`POST /`** → Yeni HEX modifikasyonu ekler.
- [x] **`GET /tune/{tuneCatalogId}`** → Belirli bir yazılım güncellemesine ait HEX değişikliklerini getirir.
- [x] **`DELETE /{id}`** → HEX modifikasyon kaydını siler.

### **6. API Log Yönetimi (`/api/logs`)**
- [x] **`GET /`** → Son 50 API logunu getirir.
- [x] **`GET /user/{userId}`** → Belirli bir kullanıcının API loglarını getirir.
- [x] **`GET /errors`** → Son 500 ve üzeri hata kodlarını getirir.
- [x] **`DELETE /cleanup`** → Eski logları temizler.

---

## 4️⃣ **Yetkilendirme & Güvenlik**
- [x] Kullanıcı işlemleri için **JWT (JSON Web Token) tabanlı kimlik doğrulama** kullanılacaktır.
- [x] **Admin yetkisi gerektiren işlemler:** Kullanıcı silme, lisans yönetimi, log temizleme.
- [x] **Veri güvenliği için** `password_hash` alanı **BCrypt** ile hashlenerek saklanacaktır.

---

## 5️⃣ **Geliştirme Teknolojileri**
- [x] **Backend:** .NET Core 8.0 (ASP.NET Web API)
- [x] **Veritabanı:** PostgreSQL 15+
    - **Bağlantı Bilgileri:**
        - **Host:** `localhost`
        - **Port:** `5432`
        - **Kullanıcı Adı:** `postgres`
        - **Şifre:** `postgres`
        - **Veritabanı Adı:** `decoder`
- [x] **Kimlik Doğrulama:** JWT Authentication + OAuth2
- [x] **Şifreleme:** BCrypt
- [x] **API Dokümantasyonu:** Swagger / OpenAPI

---

## 6️⃣ **Ekstra Geliştirmeler**
- [ ] **Log temizleme için otomatik cron job** (belirli aralıklarla eski logları silmek için).
- [ ] **Lisans süresi bitiminde otomatik erişim kapatma mekanizması.**
- [ ] **HEX verilerinin otomatik karşılaştırma (diff) analizi** için bir iş mantığı.

---

## 7️⃣ **Sonuç**
Bu API, **kullanıcı, lisans, araç-ECU, yazılım güncellemeleri, HEX modifikasyonları ve log yönetimi** için optimize edilmiştir. Geliştirme aşamasında modüler bir yapı benimsenerek, genişletilebilir bir altyapı sağlanacaktır. **Eklemek istediğin bir şey var mı?** 🚀
