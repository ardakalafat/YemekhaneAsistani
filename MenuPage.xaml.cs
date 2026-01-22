using HtmlAgilityPack;
using System.Collections.ObjectModel;

namespace MauiApp1;

public partial class MenuPage : ContentPage
{
    // Ekranda göstereceğimiz liste
    public ObservableCollection<YemekModel> Menuler { get; set; } = new ObservableCollection<YemekModel>();

    public MenuPage()
    {
        InitializeComponent();
        YemekListesi.ItemsSource = Menuler; // Tasarımla kodu bağladık
        VerileriGetir(); // Sayfa açılınca işe başla
    }

    private async void VerileriGetir()
    {
        try
        {
            string url = "https://sks.deu.edu.tr/yemek-menusu/";
            HtmlWeb web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);

            var tumHucreler = doc.DocumentNode.SelectNodes("//td");

            if (tumHucreler != null)
            {
                // Başlangıç tarihi: 9 Şubat 2026 Pazartesi
                DateTime tarih = new DateTime(2026, 2, 9);

                foreach (var hucre in tumHucreler)
                {
                    string hamVeri = hucre.InnerHtml.Replace("<br>", "\n").Replace("<br/>", "\n");
                    string temizMetin = System.Net.WebUtility.HtmlDecode(hucre.InnerText).Trim();

                    // Basit filtre: Kısa metinleri atla
                    if (temizMetin.Length > 15)
                    {
                        var yemekSatirlari = hamVeri.Split('\n')
                                                    .Select(x => System.Text.RegularExpressions.Regex.Replace(x, "<.*?>", ""))
                                                    .Select(x => x.Trim())
                                                    .Where(x => !string.IsNullOrWhiteSpace(x) && x.Length > 2)
                                                    .ToList();

                        if (yemekSatirlari.Count >= 3)
                        {
                            // 🔥 HAFTA SONU KONTROLÜ (YENİ EKLENEN KISIM) 🔥
                            // Eğer tarih Cumartesi veya Pazar ise, Pazartesi olana kadar ilerlet.
                            while (tarih.DayOfWeek == DayOfWeek.Saturday || tarih.DayOfWeek == DayOfWeek.Sunday)
                            {
                                tarih = tarih.AddDays(1);
                            }

                            Menuler.Add(new YemekModel
                            {
                                Gun = tarih.ToString("dd MMMM dddd"), // Örn: 09 Şubat Pazartesi
                                Corba = yemekSatirlari.Count > 0 ? yemekSatirlari[0] : "Çorba",
                                AnaYemek = yemekSatirlari.Count > 1 ? yemekSatirlari[1] : "Ana Yemek",
                                YanYemek = yemekSatirlari.Count > 2 ? yemekSatirlari[2] : "Yan Yemek",
                                Kalori = "Afiyet Olsun"
                            });

                            // Bir sonraki gün için ilerlet
                            tarih = tarih.AddDays(1);
                        }
                    }
                }
            }

            if (Menuler.Count == 0)
            {
                OrnekVeriYukle();
                await DisplayAlert("Bilgi", "Menü bulunamadı.", "Tamam");
            }
        }
        catch (Exception ex)
        {
            OrnekVeriYukle();
            await DisplayAlert("Hata", "Bağlantı sorunu: " + ex.Message, "Tamam");
        }
        finally
        {
            YukleniyorIcon.IsRunning = false;
            YukleniyorIcon.IsVisible = false;
        }
    }

    private void OrnekVeriYukle()
    {
        // Eski sahte yemekleri sildik.
        // Yerine tek bir "Bilgi Kartı" ekliyoruz.

        Menuler.Add(new YemekModel
        {
            Gun = "⚠️ Duyuru",
            Corba = "🍽️",
            AnaYemek = "Menü Henüz Yayınlanmadı",
            YanYemek = "Okul açıldığında liste otomatik olarak buraya düşecektir.",
            Kalori = ""
        });
    }
}