## Brevo HTTP API Yapılandırması

Uygulama artık tüm e-posta trafiğini doğrudan Brevo'nun HTTP API'si üzerinden gönderiyor; SMTP desteği tamamen kaldırıldı. API'nin çalışması için aşağıdaki environment variable'ların tanımlı olması zorunludur.

### Gerekli Environment Variable'lar

| Değişken | Açıklama |
| --- | --- |
| `BREVO_API_KEY` | Brevo Transactional API anahtarınız (`xkeysib-` ile başlar) |
| `BREVO_SENDER_EMAIL` | Gönderici e-posta adresi (default: `SMTP_FROM_EMAIL`) |
| `BREVO_SENDER_NAME` | Gönderici adı (default: `SMTP_FROM_NAME`) |

### Jenkins Örneği
```bash
export BREVO_API_KEY="xkeysib-..."
export BREVO_SENDER_EMAIL="runner@cmkkablo.com"
export BREVO_SENDER_NAME="CMK KABLO"
```

### Docker Run Örneği
```bash
docker run -d \
  -e BREVO_API_KEY="xkeysib-..." \
  -e BREVO_SENDER_EMAIL="runner@cmkkablo.com" \
  -e BREVO_SENDER_NAME="CMK KABLO" \
  ...
```

### Çalışma Mantığı
1. API anahtarı ve sender bilgileri doğrulandıktan sonra tüm mailler doğrudan `https://api.brevo.com/v3/smtp/email` endpoint'ine gönderilir.
2. Ekli dosya varsa Brevo API'ye Base64 olarak eklenir.

API isteği `https://api.brevo.com/v3/smtp/email` endpoint'ine yapılır; yanıt 200/202 değilse hata loglanır ve üst katmana fırlatılır.

