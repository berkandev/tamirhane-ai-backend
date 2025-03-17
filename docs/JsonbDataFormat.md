# JSONB Veri Formatı Dokümantasyonu

## Genel Bakış

DiffModification modeli, araç ECU'larında yapılan HEX değişiklikleri için JSONB veri tipini kullanmaktadır. Bu format, basit bir byte dizisi yerine daha zengin ve sorgulama yapılabilir bir veri yapısı sağlar.

## Avantajlar

1. **Zengin Veri Yapısı**: Basit byte dizileri yerine karmaşık veri yapıları saklayabilir
2. **Sorgulanabilirlik**: JSON içindeki belirli alanlara göre sorgulama yapılabilir
3. **Esneklik**: Şema değişikliği gerektirmeden yeni alanlar eklenebilir
4. **Okunabilirlik**: Binary veriye göre daha okunabilir ve anlaşılabilir

## JSON Veri Yapısı Örneği

### Original Data Format

```json
{
  "bytes": [0x25, 0xFA, 0x45, 0x12, 0x34, 0x56, 0x78, 0x9A],
  "description": "Orijinal değer - Yakıt Basıncı Kalibrasyonu",
  "metadata": {
    "dataType": "uint16",
    "unit": "kPa",
    "validRange": {
      "min": 100,
      "max": 500
    },
    "originalValue": 250
  },
  "tags": ["yakıt", "kalibrasyon", "basınç"]
}
```

### Modified Data Format

```json
{
  "bytes": [0x25, 0xFA, 0x45, 0x12, 0x34, 0x56, 0x78, 0xBB],
  "description": "Modifiye değer - Yakıt Basıncı Kalibrasyonu (Artırılmış)",
  "metadata": {
    "dataType": "uint16",
    "unit": "kPa",
    "validRange": {
      "min": 100,
      "max": 500
    },
    "modifiedValue": 300,
    "percentageChange": 20
  },
  "tags": ["yakıt", "kalibrasyon", "basınç", "performans"],
  "changeReason": "Daha iyi yanma oranı elde etmek için yakıt basıncı artırıldı",
  "riskLevel": "medium",
  "recommendedFor": ["yarış", "spor sürüş"]
}
```

## Kullanım Örnekleri

### Veri Ekleme

```csharp
var diffModification = new CreateDiffModificationDto
{
    TuneCatalogId = 1,
    Name = "Yakıt Basıncı Kalibrasyonu",
    Description = "Performans için yakıt basıncı değişikliği",
    OffsetAddress = 0x1234,
    OriginalDataJson = JsonSerializer.Serialize(originalData),
    ModifiedDataJson = JsonSerializer.Serialize(modifiedData)
};
```

### Veri Sorgulama Örneği (PostgreSQL)

```sql
SELECT * FROM "DiffModifications" 
WHERE "ModifiedDataJson"->>'changeReason' LIKE '%yakıt%'
AND ("ModifiedDataJson"->'metadata'->>'percentageChange')::int > 15;
```

## Öneriler

1. JSON veri yapısı tutarlı olmalı - aynı anahtarlar ve veri tipleri kullanılmalı
2. Performans için JSON içeriğini çok büyük tutmamak önemlidir
3. Sorgulama yapılacak alanlar için indeksler oluşturulmalıdır
4. Kritik değerleri hem ayrı alanlarda hem de JSON içinde saklayarak veri bütünlüğü sağlanabilir

## Hata Yönetimi

DiffModificationsController, gelen JSON verisini otomatik olarak doğrular ve geçersiz JSON verisi için:

```json
{
  "isValid": false,
  "error": "JSON formatı geçersiz: Unexpected end of data while reading JSON."
}
```

şeklinde bir hata döndürür.