# API Ürün Gereksinimleri Dokümanı (PRD)

## API Modülleri

- [x] Kullanıcı Yönetimi
- [x] Araç ve ECU Yönetimi
- [x] Yazılım Güncellemeleri (TuneCatalog)
- [x] HEX Modifikasyonları (DiffModification) - **JSONB desteği eklendi**
- [x] Lisans Yönetimi
- [x] API Loglama

## API Endpoints

### Kullanıcı Yönetimi (User Management)
- [x] `POST /register`: Yeni kullanıcı kaydı oluşturur
- [x] `POST /login`: Kullanıcı girişi sağlar ve JWT token döndürür
- [x] `GET /profile`: Giriş yapmış kullanıcının profilini getirir
- [x] `PUT /profile`: Kullanıcı profilini günceller

### Araç Yönetimi (Vehicle Management)
- [x] `POST`: Yeni araç ekler
- [x] `GET`: Kullanıcıya ait araçları listeler
- [x] `GET /{id}`: Belirli bir aracın detaylarını getirir
- [x] `PUT /{id}`: Araç bilgilerini günceller
- [x] `DELETE /{id}`: Aracı siler

### ECU Yönetimi (ECU Management)
- [x] `POST`: Araca yeni ECU ekler
- [x] `GET /vehicle/{vehicleId}`: Araca ait ECU'ları listeler
- [x] `GET /{id}`: Belirli bir ECU'nun detaylarını getirir
- [x] `PUT /{id}`: ECU bilgilerini günceller
- [x] `DELETE /{id}`: ECU'yu siler

### Yazılım Güncellemeleri (TuneCatalog)
- [x] `POST`: Yeni yazılım güncellemesi ekler
- [x] `GET /ecu/{ecuId}`: ECU'ya ait yazılım güncellemelerini listeler
- [x] `GET /{id}`: Belirli bir yazılım güncellemesinin detaylarını getirir
- [x] `PUT /{id}`: Yazılım güncellemesi bilgilerini günceller
- [x] `DELETE /{id}`: Yazılım güncellemesini siler

### HEX Modifikasyonları (DiffModification)
- [x] `POST`: Yeni HEX modifikasyonu ekler (JSONB formatında)
- [x] `GET /tuneCatalog/{tuneCatalogId}`: Yazılım güncellemesine ait HEX modifikasyonlarını listeler
- [x] `GET /{id}`: Belirli bir HEX modifikasyonunun detaylarını getirir
- [x] `DELETE /{id}`: HEX modifikasyonunu siler
- [x] `POST /PreviewJson`: JSON verisinin geçerli olup olmadığını kontrol eder

### Lisans Yönetimi (License Management)
- [x] `POST /assign`: Kullanıcıya yeni lisans atar
- [x] `GET /{userId}`: Kullanıcıya ait lisansları getirir
- [x] `PUT /{id}/deactivate`: Lisansı deaktif eder
- [x] `DELETE /{id}`: Lisansı siler

### API Loglama (API Logging)
- [x] `GET`: Tüm API loglarını listeler
- [x] `GET /filter`: Belirli kriterlere göre API loglarını filtreler
- [x] `GET /{id}`: Belirli bir log kaydının detaylarını getirir
- [x] `DELETE /{id}`: Log kaydını siler

## JSONB Geliştirmeleri

### Eklenen Özellikler
- **Zengin Veri Yapısı**: DiffModification modelinde `OriginalBytes` ve `ModifiedBytes` alanları yerine `OriginalDataJson` ve `ModifiedDataJson` JSONB alanları eklendi
- **Veri Doğrulama**: JSON formatı geçerliliği API katmanında kontrol ediliyor
- **Metadata Desteği**: HEX değişikliklerinde ek bilgilerin (birim, değer aralığı vb.) saklanabilmesi
- **Önizleme Fonksiyonu**: `/PreviewJson` endpoint'i ile gönderilen JSON verisinin doğruluğu test edilebiliyor

### Veri Örneği
```json
{
  "bytes": [0x25, 0xFA, 0x45, 0x12],
  "description": "Yakıt Basıncı Kalibrasyonu",
  "metadata": {
    "dataType": "uint16",
    "unit": "kPa",
    "validRange": {
      "min": 100,
      "max": 500
    }
  },
  "tags": ["yakıt", "kalibrasyon", "basınç"]
}
```

## Yetkilendirme ve Güvenlik

- API JWT tabanlı kimlik doğrulama kullanır
- Şifreler bcrypt ile hashlenir
- Rol tabanlı yetkilendirme sistemi desteklenir (Admin, User)
- API istekleri loglanır
- Hata yönetimi için global exception handler kullanılır

## Geliştirme Teknolojileri

- Backend: .NET Core 8.0
- Veritabanı: PostgreSQL 15
- ORM: Entity Framework Core
- Dokümantasyon: Swagger/OpenAPI
- Auth: JWT
- Deployment: Docker ve Docker Compose

## Ekstra Geliştirmeler

- [x] Log sistemi eklendi
- [x] Lisans yönetimi sistemi eklendi
- [x] JSONB desteği eklendi