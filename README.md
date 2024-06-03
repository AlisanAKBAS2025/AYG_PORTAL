# AYG PORTAL

AYG PORTAL, hırdavatçı ve nalbur satış işlemlerini otomatikleştirmek için geliştirilmiş kapsamlı bir satış otomasyon yazılımıdır. Bu proje, C# ve MsSQL kullanılarak tasarlanmıştır ve çeşitli kullanıcı dostu özellikler sunar.
# Videolu Anlatımı
https://docs.google.com/spreadsheets/d/1jvmCgujyXGaPU3TJMhZhUNxt4myuHHAQAcmcSfLI63s/edit?usp=sharing

## Özellikler

- **Personel ve Yönetici Girişi**: Hem personel hem de yöneticiler için özel giriş ve yönetim paneli.
- **Ürün Yönetimi**: 4500 farklı ürün çeşidini kolayca yönetme imkanı.
- **Dinamik Fiyatlandırma**: Şirketin döviz bazlı alımlarını TL cinsinden satışa dönüştürmesini kolaylaştıran anlık döviz kuru entegrasyonu.
- **Otomatik Veri Çekme**: Python kullanarak web sitelerinden ürün isimleri, fiyatlar ve fotoğrafları otomatik olarak çekme.
- **Karmaşık Fiyat Hesaplamaları**: KDV, kar oranları gibi çeşitli formüllerle ürün fiyatlarını hesaplayarak anlık dolar kuruna göre kasa fiyatlandırması yapma.

## Kurulum

Projeyi yerel ortamınıza kurmak için aşağıdaki adımları izleyin:

1. Bu repo'yu klonlayın:
    ```bash
    git clone https://github.com/kullanici-adi/AYG-PORTAL.git
    ```

2. Proje dizinine gidin:
    ```bash
    cd AYG-PORTAL
    ```

3. Gereken bağımlılıkları yükleyin:
    - **C# ve MsSQL**: Proje için gerekli olan bağımlılıkları yüklemek için Visual Studio kullanabilirsiniz.
    - **Python**: Gerekli Python kütüphanelerini yüklemek için aşağıdaki komutu kullanın:
        ```bash
        pip install -r requirements.txt
        ```

4. Veritabanı yapılandırmasını yapın:
    - MsSQL sunucunuzda yeni bir veritabanı oluşturun.
    - `appsettings.json` dosyasındaki veritabanı bağlantı ayarlarını güncelleyin.

5. Projeyi çalıştırın:
    - Visual Studio kullanarak projeyi başlatın.

## Kullanım

1. Uygulamayı çalıştırın.
2. Personel veya yönetici girişi yapın.
3. Ürün yönetimi sekmesinden ürünleri yönetin.
4. Satış ekranında döviz kurlarına göre fiyatlandırma yapın.

## Katkıda Bulunma

Katkıda bulunmak isterseniz, lütfen önce bir konu açın ve değişikliklerinizi tartışın. Büyük değişiklikler için lütfen önce neyi değiştirmek istediğinizi tartışmak üzere bir konu açın.

## Lisans

Bu proje MIT Lisansı altında lisanslanmıştır. Daha fazla bilgi için `LICENSE` dosyasına bakın.

---

Herhangi bir sorunuz veya geri bildiriminiz varsa, lütfen benimle iletişime geçmekten çekinmeyin!
