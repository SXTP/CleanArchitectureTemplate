# Noname API - .NET 9 Clean Architecture Template (5-Layer)

Bu proje; modern web uygulamaları için tasarlanmış, **Clean Architecture**, **CQRS** ve **Shared Kernel** 
yaklaşımlarını temel alan, yüksek ölçeklenebilirliğe sahip bir .NET 9 şablonudur.

## 🚀 Teknolojiler ve Araçlar

* **Framework:** .NET 9
* **Architecture:** Clean Architecture (5 Layers), CQRS Pattern
* **Database:** PostgreSQL (EF Core)
* **Identity:** Microsoft Identity & JWT Authentication
* **Communication:** MediatR
* **Containerization:** Docker & Docker Compose
* **Documentation:** Scalar
* **Validation:** FluentValidation

## 🏗️ Mimari Katmanlar

Proje, sorumlulukların tam ayrımı için 5 katmana ayrılmıştır:

1. **Domain:** Core varlıklar (Entities) ve kurallar. Sıfır bağımlılık.
2. **Application:** İş mantığı, MediatR Handler'lar ve servis arayüzleri.
3. **Infrastructure:** Veritabanı, Identity yönetimi ve dış servis entegrasyonları.
4. **Shared:** Proje genelinde kullanılan Extension Method'lar ve sabitler (Noname.Shared).
5. **Web API:** Endpoint'ler ve Middleware yapılandırmaları.

## 🛠️ Kurulum ve Çalıştırma

Projenin çalışması için bilgisayarınızda **Docker** ve **.NET 9 SDK** kurulu olmalıdır.

1. Projeyi klonlayın:
```bash
git clone [https://github.com/SXTP/CleanArchitectureTemplate.git]
```

2. Docker üzerinden veritabanını ayağa kaldırın (Gereklidir):
```bash
docker-compose up -d
```

3. Veritabanı tablolarını oluşturun (Migration):
```bash
dotnet ef database update --project Noname.Infrastructure --startup-project Noname.API
```

4. Uygulamayı başlatın:
```bash
dotnet run --project Noname.API
```

## 🔒 Kimlik Doğrulama

Sistem JWT tabanlı bir güvenlik yapısı kullanır:
* /api/auth/login üzerinden giriş yaparak Bearer Token alın.
* Scalar arayüzündeki **Authorize** kısmına bu token'ı yapıştırarak test edin.

## 📡 Standart API Yanıtı (Result Pattern)

Tüm yanıtlar, frontend tutarlılığı için sabit bir modelle döner:

```json
{
  "succeeded": true,
  "errors": [],
  "data": { ... }
}
```

## 📝 Öne Çıkan Özellikler

* **Shared Logic:** Proje genelinde kullanılan yardımcı metodlar (DateTime/Epoch vb.) tek noktadan (`Noname.Shared`) yönetilir.
* **Identity Integration:** Identity tablosu (`AppUser`) ile uygulama tablosu (`Member`) arasındaki izolasyon korunmuştur.
* **Global Handling:** Hatalar merkezi bir Middleware ile yakalanarak kullanıcıya standart `Result` formatında sunulur.
* **CQRS & MediatR:** İş mantığı (Commands) ve sorgu mantığı (Queries) tamamen birbirinden ayrılarak kod bakımı maksimize edilmiştir.
* **Fluent Validation:** Giriş verileri uygulama katmanında otomatik olarak doğrulanır.