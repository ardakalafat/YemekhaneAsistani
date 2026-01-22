using Plugin.LocalNotification;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        // Uygulama açılınca eski kayıtları geri yükle
        LoadUserPreferences();
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        // 1. Validasyon (Kullanıcı her şeyi seçmiş mi?)
        if (UniversityPicker.SelectedIndex == -1 || DayPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Eksik Bilgi", "Lütfen üniversite ve hatırlatma gününü seçiniz.", "Tamam");
            return;
        }

        // 2. Verileri Al
        string secilenUni = UniversityPicker.SelectedItem.ToString();
        int secilenGunIndex = DayPicker.SelectedIndex; // 0=Pazartesi, 1=Salı...
        TimeSpan secilenSaat = TimePicker.Time;

        // 3. Verileri Kaydet (Preferences)
        Preferences.Default.Set("KayitliUni", secilenUni);
        Preferences.Default.Set("KayitliGun", secilenGunIndex);
        Preferences.Default.Set("KayitliSaatTicks", secilenSaat.Ticks); // Saati sayısal olarak saklıyoruz

        // 4. İzin İste
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        // 5. Hesaplama Yap: C#'ın DayOfWeek yapısına çevir (Pazar=0, Pzt=1... olduğu için ufak bir dönüşüm lazım)
        // Bizim listemiz: Pzt(0), Sal(1)... C#'ın listesi: Pazar(0), Pzt(1)...
        // Bu yüzden basit bir dönüşüm yapıyoruz:
        DayOfWeek hedefGun = ConvertIndexToDayOfWeek(secilenGunIndex);

        var notification = new NotificationRequest
        {
            NotificationId = 100,
            Title = "Yemekhane Hatırlatıcı",
            Description = $"{secilenUni} rezervasyonunu yapma zamanı geldi!",
            BadgeNumber = 1,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = GetNextReminderTime(hedefGun, secilenSaat),
                RepeatType = NotificationRepeat.Weekly
            }
        };

        await LocalNotificationCenter.Current.Show(notification);
        await DisplayAlert("Başarılı", $"Hatırlatıcı her {DayPicker.SelectedItem} saat {secilenSaat:hh\\:mm}'e kuruldu.", "Tamam");
    }

    private async void OnOpenWebClicked(object sender, EventArgs e)
    {
        // Seçili üniversiteyi al (Eğer boşsa varsayılan DEÜ olsun)
        string secilenUni = (string)UniversityPicker.SelectedItem ?? "Dokuz Eylül Üniversitesi";

        // 🌟 DEÜ İÇİN ÖZEL MUAMELE 🌟
        if (secilenUni == "Dokuz Eylül Üniversitesi")
        {
            // DEÜ seçiliyse bizim yaptığımız özel uygulamayı aç
            await Navigation.PushAsync(new MenuPage());
        }
        else
        {
            // 🌍 DİĞER ÜNİVERSİTELER İÇİN TARAYICIYI AÇ 🌍
            string url = "";

            if (secilenUni == "Ege Üniversitesi")
                url = "https://sksdb.ege.edu.tr/tr-1658/yemek_listesi.html";
            else if (secilenUni == "İzmir Yüksek Teknoloji Enstitüsü")
                url = "https://sks.iyte.edu.tr/yemek-hizmetleri/yemek-listesi/";
            else if (secilenUni == "İzmir Katip Çelebi Üniversitesi")
                url = "https://sks.ikcu.edu.tr/YemekListesi";

            try
            {
                Uri uri = new Uri(url);
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch
            {
                await DisplayAlert("Hata", "Site açılamadı.", "Tamam");
            }
        }
    }

    // --- YARDIMCI METOTLAR ---

    private DateTime GetNextReminderTime(DayOfWeek hedefGun, TimeSpan hedefSaat)
    {
        DateTime simdi = DateTime.Now;
        int gunFarki = ((int)hedefGun - (int)simdi.DayOfWeek + 7) % 7;

        // Eğer gün bugünse ama saat geçtiyse, haftaya at
        if (gunFarki == 0 && simdi.TimeOfDay > hedefSaat)
            gunFarki = 7;

        return simdi.Date.AddDays(gunFarki).Add(hedefSaat);
    }

    private DayOfWeek ConvertIndexToDayOfWeek(int pickerIndex)
    {
        // Bizim Picker: 0=Pzt, 1=Sal, 2=Çar, 3=Per, 4=Cum, 5=Cmt, 6=Paz
        // C# DayOfWeek: 1=Pzt, ... 6=Cmt, 0=Pazar
        if (pickerIndex == 6) return DayOfWeek.Sunday;
        return (DayOfWeek)(pickerIndex + 1);
    }

    private void LoadUserPreferences()
    {
        // Kayıtlı verileri geri yükle
        if (Preferences.Default.ContainsKey("KayitliUni"))
            UniversityPicker.SelectedItem = Preferences.Default.Get("KayitliUni", "");

        if (Preferences.Default.ContainsKey("KayitliGun"))
            DayPicker.SelectedIndex = Preferences.Default.Get("KayitliGun", 0);

        if (Preferences.Default.ContainsKey("KayitliSaatTicks"))
            TimePicker.Time = new TimeSpan(Preferences.Default.Get("KayitliSaatTicks", 0L));
    }
}