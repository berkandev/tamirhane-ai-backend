# Tamirhane.AI Backend

.NET Core 8.0 ile geliştirilmiş, araç tamir atölyeleri için ECU programlama ve yönetim API'si.

## Özellikler

### Tamamlanan Modüller:

1. **Kullanıcı Yönetimi** ✅
   - Kullanıcı kaydı, giriş, profil görüntüleme ve güncelleme
   - JWT tabanlı kimlik doğrulama
   - Rol tabanlı yetkilendirme (Admin/Kullanıcı)

2. **Lisans Yönetimi** ✅
   - Lisans oluşturma, atama, devre dışı bırakma ve silme
   - Otomatik lisans anahtarı oluşturma
   - Kullanıcı bazlı lisans takibi

3. **Araç ve ECU Yönetimi** ✅
   - Araç ekleme, listeleme, güncelleme ve silme
   - ECU modeli ekleme, listeleme ve silme
   - Araç-ECU ilişkisi kurma ve görüntüleme

4. **Yazılım Güncellemeleri (TuneCatalog)** ✅
   - Yazılım güncellemesi yükleme ve indirme
   - ECU modeline göre yazılım güncellemelerini listeleme
   - Dosya yönetimi

5. **HEX Modifikasyonları** ✅
   - HEX değişikliklerini JSONB formatında saklama ve listeleme
   - Yazılım güncellemelerine bağlı modifikasyonlar
   - JSON veri yapısıyla zengin metadata desteği

6. **API Log Yönetimi** ✅
   - API isteklerini loglama
   - Hata loglarını görüntüleme
   - Eski logları temizleme

## Teknik Detaylar

- **Backend:** .NET Core 8.0 (ASP.NET Web API)
- **Veritabanı:** PostgreSQL
- **Kimlik Doğrulama:** JWT Authentication
- **Şifreleme:** BCrypt
- **API Dokümantasyonu:** Swagger / OpenAPI

## Kullanım

1. Veritabanı bağlantısını kontrol edin (appsettings.json)
2. Projeyi çalıştırın: `dotnet run`
3. Swagger arayüzü üzerinden API'yi test edin: `https://localhost:5001/swagger`
4. İlk olarak bir kullanıcı oluşturun ve token alın
5. Alınan token ile diğer API uç noktalarını kullanın

## JSONB Veri Örneği

HEX değişiklikleri için JSONB formatında veri saklama örneği:

```json
{
  "bytes": [25, 48, 120, 255],
  "description": "Motor kontrol ünitesi parametreleri",
  "metadata": {
    "engineType": "1.6 TDI",
    "parameter": "Yakıt Enjeksiyon Zamanlaması",
    "unit": "ms"
  },
  "tags": ["motor", "yakıt", "enjeksiyon"]
}
```