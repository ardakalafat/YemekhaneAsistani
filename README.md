# 🍽️ Dining Hall Assistant (Yemekhane Asistanı)

> **A smart mobile assistant developed with .NET MAUI for Dokuz Eylul University and other universities in Izmir, featuring web scraping and intelligent data parsing.**

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white) ![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white) ![MAUI](https://img.shields.io/badge/MAUI-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

---

## 🇬🇧 English Description

**Dining Hall Assistant** is a mobile application designed to solve a common problem for university students: accessing weekly dining menus easily. Instead of navigating through complex websites, this app scrapes, cleans, and presents the data in a modern, user-friendly mobile interface.

### 🚀 Key Features

* **🔍 Smart Web Scraping:** Uses `HtmlAgilityPack` to parse complex HTML table structures from the university's SKS website, filtering out non-food data (dates, empty cells).
* **🧠 Intelligent Algorithm:** Automatically detects weekends and holidays to prevent date shifting, ensuring the correct menu is displayed for the correct day.
* **🔀 Hybrid Routing System:** Provides a custom native UI for **Dokuz Eylul University**, while offering smart browser integration for **Ege, IYTE, and Katip Celebi Universities**.
* **🛡️ Fail-Safe Error Handling:** Includes a dynamic notification system that informs the user if the menu hasn't been published yet, preventing application crashes.
* **📱 Modern UI:** Designed with a clean, card-based interface (CardView) using .NET MAUI.

### 🛠️ Tech Stack
* **Language:** C#
* **Framework:** .NET 9 MAUI (Multi-platform App UI)
* **Libraries:** HtmlAgilityPack (for Data Scraping)
* **IDE:** Visual Studio 2022

---

## 🇹🇷 Türkçe Açıklama

**Yemekhane Asistanı**, üniversite öğrencilerinin haftalık yemek menüsüne kolayca ulaşmasını sağlayan modern bir mobil uygulamadır. Üniversite web sitesindeki karmaşık verileri otomatik olarak çeker, temizler ve şık bir arayüzde sunar.

### 🚀 Özellikler

* **🔍 Akıllı Veri Kazıma (Scraping):** SKS web sitesindeki karmaşık tablo yapısını analiz eder, tarihleri ve gereksiz verileri ayıklayarak sadece yemekleri listeler.
* **🧠 Dinamik Algoritma:** Hafta sonlarını ve tatil günlerini otomatik algılayarak tarih kaymalarını önler, sadece okul günlerini gösterir.
* **🔀 Hibrit Yönlendirme:** **Dokuz Eylül Üniversitesi** için özel tasarlanmış yerleşik arayüz sunarken; **Ege, İYTE ve Katip Çelebi** üniversiteleri için entegre tarayıcı yönlendirmesi yapar.
* **🛡️ Hata Yönetimi:** Menü henüz yayınlanmamışsa uygulamanın çökmesini engeller ve kullanıcıya bilgilendirici bir "Duyuru Kartı" gösterir.
* **📱 Modern Arayüz:** .NET MAUI ile geliştirilmiş, kullanıcı dostu kart (CardView) tasarımı.

### 👨‍💻 Developer / Geliştirici
**Mehmet Arda Kalafat** - Computer Engineering Student @ Dokuz Eylul University
