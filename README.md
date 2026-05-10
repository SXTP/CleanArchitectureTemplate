# Noname API - .NET 9 Clean Architecture Template (5-Layer)

Bu proje; modern web uygulamaları için tasarlanmış, **Clean Architecture**, **CQRS** ve **Shared Kernel** yaklaşımlarını temel alan, yüksek ölçeklenebilirliğe sahip bir .NET 9 şablonudur.

## 🚀 Teknolojiler ve Araçlar

* **Framework:** .NET 9
* **Architecture:** Clean Architecture (5 Layers), CQRS Pattern
* **Database:** PostgreSQL / MS SQL (EF Core)
* **Identity:** Microsoft Identity & JWT Authentication
* **Communication:** MediatR
* **Documentation:** Scalar (OpenAPI 3.1)
* **Validation:** FluentValidation
* **Utilities:** Shared Kernel Helpers

## 🏗️ Mimari Katmanlar

Proje, sorumlulukların tam ayrımı (Separation of Concerns) için 5 katmana ayrılmıştır:

1.  **Domain:** Core varlıklar (Entities), Value Object'ler ve kuralları içerir. Sıfır bağımlılığa sahiptir.
2.  **Application:** İş mantığı, MediatR Handler'lar ve servis arayüzleri bu katmandadır.
3.  **Infrastructure:** Veritabanı implementasyonu, Identity yönetimi ve dış servis entegrasyonlarını barındırır.
4.  **Shared:** Tüm projede kullanılan ortak Extension Method'lar (Örn: Epoch/DateTime dönüşümleri) ve sabitleri içerir.
5.  **Web API:** Endpoint'ler, Middleware'ler ve Scalar konfigürasyonunun bulunduğu giriş kapısıdır.

## 🛠️ Kurulum ve Çalıştırma

1.  Projeyi kopyalayın:
    ```bash
    git clone [https://github.com/SXTP/distributed-event-bus-sample.git](https://github.com/SXTP/distributed-event-bus-sample.git)
    ```
2.  `appsettings.json` dosyasındaki Connection String'i kendi veritabanınıza göre düzenleyin.
3.  Veritabanı tablolarını oluşturun:
    ```bash
    dotnet ef database update --project Infrastructure --startup-project API
    ```
4.  Uygulamayı başlatın:
    ```bash
    dotnet run --project API
    ```

## 🔒 Kimlik Doğrulama

Sistem JWT tabanlı bir güvenlik yapısı kullanır:
* `/api/auth/login` üzerinden giriş yaparak Bearer Token alın.
* Alınan token'ı Scalar arayüzündeki **Authorize** kısmına ekleyerek yetki gerektiren işlemleri test edin.

## 📡 Standart API Yanıtı (Result Pattern)

Frontend entegrasyonunun tutarlı olması adına tüm yanıtlar aşağıdaki yapıda döner (Başarı durumunda her zaman HTTP 200 döner):

```json
{
  "succeeded": true,
  "errors": [],
  "data": { ... }
}
```
## 📝 Öne Çıkan Özellikler

* **Shared Logic:** Proje genelinde kullanılan yardımcı metodlar (DateTime/Epoch vb.) tek noktadan (`Noname.Shared`) yönetilir.
* **Identity Integration:** Identity tablosu (`AppUser`) ile uygulama tablosu (`Member`) arasındaki izolasyon, Clean Architecture prensiplerine uygun şekilde korunmuştur.
* **Global Handling:** Hatalar merkezi bir Middleware ile yakalanarak kullanıcıya standart `Result` formatında sunulur.
* **CQRS & MediatR:** İş mantığı (Commands) ve sorgu mantığı (Queries) tamamen birbirinden ayrılarak kodun okunabilirliği ve bakımı maksimize edilmiştir.
* **Fluent Validation:** Giriş verileri (DTOs), uygulama katmanında otomatik olarak doğrulanır ve hatalar `Result` yapısıyla döndürülür.