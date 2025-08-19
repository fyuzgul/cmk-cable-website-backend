# SendGrid Kurulum ve Güvenlik Rehberi

## 🔐 Güvenlik Önlemleri

Bu proje artık SendGrid API anahtarını güvenli bir şekilde yönetiyor. API anahtarı artık kod içinde hardcoded değil, configuration dosyalarından okunuyor.

## 📁 Konfigürasyon Dosyaları

### Development Ortamı
- `appsettings.Development.json` - Geliştirme ortamı için
- API anahtarı burada saklanabilir (sadece local development için)

### Production Ortamı
- `appsettings.Production.json` - Production ortamı için
- API anahtarı boş bırakılmalı
- Environment variable kullanılmalı

## 🌍 Environment Variable Kullanımı

### Windows (PowerShell)
```powershell
$env:SENDGRID_API_KEY="SG.your-api-key-here"
$env:SENDGRID_FROM_EMAIL="your-email@domain.com"
$env:SENDGRID_FROM_NAME="Your Company Name"
```

### Windows (Command Prompt)
```cmd
set SENDGRID_API_KEY=SG.your-api-key-here
set SENDGRID_FROM_EMAIL=your-email@domain.com
set SENDGRID_FROM_NAME=Your Company Name
```

### Linux/macOS
```bash
export SENDGRID_API_KEY="SG.your-api-key-here"
export SENDGRID_FROM_EMAIL="your-email@domain.com"
export SENDGRID_FROM_NAME="Your Company Name"
```

### Docker
```dockerfile
ENV SENDGRID_API_KEY=SG.your-api-key-here
ENV SENDGRID_FROM_EMAIL=your-email@domain.com
ENV SENDGRID_FROM_NAME=Your Company Name
```

## 🚀 Production Deployment

### Azure App Service
1. App Service > Configuration > Application settings
2. Aşağıdaki environment variable'ları ekleyin:
   - `SENDGRID_API_KEY`
   - `SENDGRID_FROM_EMAIL`
   - `SENDGRID_FROM_NAME`

### Docker Container
```bash
docker run -e SENDGRID_API_KEY="SG.your-key" -e SENDGRID_FROM_EMAIL="email@domain.com" -e SENDGRID_FROM_NAME="Company" your-app
```

### Kubernetes
```yaml
env:
- name: SENDGRID_API_KEY
  value: "SG.your-api-key-here"
- name: SENDGRID_FROM_EMAIL
  value: "your-email@domain.com"
- name: SENDGRID_FROM_NAME
  value: "Your Company Name"
```

## 🔒 Güvenlik Kontrol Listesi

- [ ] API anahtarı kod içinde hardcoded değil
- [ ] Production ortamında environment variable kullanılıyor
- [ ] API anahtarı version control'e commit edilmedi
- [ ] API anahtarı güvenli bir şekilde saklanıyor
- [ ] API anahtarı düzenli olarak rotate ediliyor

## 📝 Notlar

- **ÖNEMLİ**: API anahtarını asla kod içinde hardcoded olarak yazmayın
- **ÖNEMLİ**: API anahtarını version control'e commit etmeyin
- **ÖNEMLİ**: Production ortamında environment variable kullanın
- API anahtarını düzenli olarak değiştirin (en az 3 ayda bir)
- SendGrid dashboard'dan eski API anahtarlarını silin

## 🆘 Sorun Giderme

### API Anahtarı Bulunamadı Hatası
```
SendGrid API Key not configured. Please set SendGrid:ApiKey in appsettings.json.
```

**Çözüm**: Environment variable'ı doğru şekilde ayarlayın veya configuration dosyasında API anahtarını belirtin.

### Email Gönderim Hatası
```
SendGrid email failed: 401 - Unauthorized
```

**Çözüm**: API anahtarının doğru olduğundan emin olun ve SendGrid hesabınızın aktif olduğunu kontrol edin.
