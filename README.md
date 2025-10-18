# LaLigaApp

## ⚽ LaLiga App – System analizy i zarządzania danymi piłkarskimi

LaLiga App to kompleksowa aplikacja webowa stworzona w technologii ASP.NET Core MVC, umożliwiająca przeglądanie, analizowanie i zarządzanie danymi dotyczącymi piłkarskiej ligi hiszpańskiej (La Liga). Projekt integruje dane pochodzące z zewnętrznego API oraz źródeł statystycznych (np. Kaggle), przechowuje je w lokalnej bazie danych SQLite i umożliwia ich wizualizację w interfejsie webowym.

## 🔍 Funkcjonalności

- Logowanie i rejestracja użytkowników (z haszowaniem haseł).
- **System ról**: użytkownik (user) i administrator (admin) z różnymi poziomami dostępu.
- Zarządzanie drużynami (DruzynaController) – przeglądanie, dodawanie, edycja i usuwanie danych.
- Zarządzanie meczami (MeczController) – możliwość przypisywania drużyn gospodarzy i gości, dodawania nowych spotkań, edycji i usuwania.
- Wyświetlanie zwycięskich drużyn – ranking na podstawie punktów i liczby zdobytych goli.
- Panel administratora – dostęp tylko dla konta admin.

## 🧠 Warstwa danych
### 📦 Baza danych – SQLite (laliga.db)

Struktura bazy została zdefiniowana w klasie LaLigaContext z wykorzystaniem Entity Framework Core.
Relacje między tabelami odwzorowują rzeczywiste powiązania w świecie piłki nożnej:

- Druzyna – drużyny z ligi La Liga.
- Mecz – mecze z relacjami do gospodarzy i gości.
- Zawodnik – zawodnicy przypisani do drużyn.
- Strzelec – dane o strzelcach i powiązanie z meczami.
- Statystyki – szczegółowe dane meczowe.
- Uzytkownik – dane logowania i profilu użytkowników.

## 🌐 Integracja z API

Dane są automatycznie pobierane i inicjalizowane za pomocą klasy DatabaseInitializer.
Aplikacja korzysta z API Football (RapidAPI), pobierając:

- listę drużyn,
- listy zawodników,
- statystyki meczów,
- dane o strzelcach bramek.
- Pobrane informacje są zapisywane w lokalnej bazie SQLite.

## 🤖 Moduł Machine Learning (Python + TensorFlow)

W projekcie znajduje się również moduł analityczny napisany w Pythonie, wykorzystujący TensorFlow i Pandas.
Celem jest predykcja wartości rynkowej zawodników na podstawie ich statystyk meczowych.

## 🏗️ Architektura projektu

- **LaLiga/**
  - **APIs/** - _Łączenie z zewnętrznym API i pobranie danych_
    - **Data/** - _Zapis plików z danymi_
    - **Match/** - _Klasy pomocnicze do wczytania danych o meczach_
      - `Fixture.cs`
      - `MatchWrapper.cs`
      - `Root.cs`
      - `Team.cs`
      - `TeamsWrapper.cs`
    - **Player/** - _Klasy pomocnicze do wczytania danych o piłkarzach_
      -  `Playre.cs`
      -  `PlayreInfo.cs`
      -  `PlayerInfoWrapper.cs`
      -  `PlayerWrapper.cs`
      -  `RootPlayer.cs`
      -  `RootPlayerInfo.cs`
    - **Shooter/** - _Klasy pomocnicze do wczytania danych o strzelcach_
      - `Event.cs`
      - `ShooterRoot.cs`
      - `ShooterWrapper.cs`
    - **Statistics/** - _Klasy pomocnicze do wczytania danych o statystykach_
      - `RootStats.cs`
      - `Stats.cs`
      - `StatsWrapper.cs`
      - `Team.cs`
    - **Team/** - _Klasy pomocnicze do wczytania danych o drużynach_
      - `RootTeam.cs`
      - `Team.cs`
      - `TeamWrapper.cs`
      - `Venue.cs`
    - `APIManager.cs` 
  - **Controllers/** — _Kontrolery aplikacji MVC_
    - `DruzynaController.cs`
    - `MeczController.cs`
    - `LoginController.cs`
    - `RegisterController.cs`
    - `HomeController.cs`
    - `StatystykiController.cs`
    - `StrzelecController.cs`
    - `UzytkownikController.cs`
    - `ZawodnikController.cs`
  - **Data/** — _Warstwa danych i inicjalizacja bazy_
    - `LaLigaContext.cs`
    - `laliga.db`
    - `DatabaseInitializer.cs`
  - **Filters/** — _Filtry autoryzacji i uprawnień_
    - `RequireLoginAttribute.cs`
    - `RequireRoleAttribute.cs`
  - **Models/** — _Modele danych (Entity Framework)_
    - `Druzyna.cs`
    - `Mecz.cs`
    - `Zawodnik.cs`
    - `Strzelec.cs`
    - `Statystyki.cs`
    - `Uzytkownik.cs`
    - `ErrorViewModel.cs`
    - `PlayerOverallStats.cs`
  - **Service/** — _Usługi pomocnicze i integracja z API_
    - `MyBackgroundService.cs`
    - `HashHelper.cs`
  - **Python/** — _Moduły uczenia maszynowego_
    - `import_date.py`
    - `save_to_database.py`
    - `ml_model.py`
  - **Migrations/** - _Migracje bazy danych_ 
  - **Views/** — _Warstwa prezentacji (Razor)_
    - `Druzyna/`
    - `Home/`
    - `Mecz/`
    - `Login/`
    - `Register/`
    - `Shared/`
    - `Statystyki/`
    - `Strzelec/`
    - `Uzytkownik/`
    - `Zawodnik/`

## ⚙️ Technologie

### Backend:
- ASP.NET Core MVC
- Entity Framework Core
- SQLite

### Frontend: 
- Razor Pages
- Bootstrap 5

### Machine Learning:
- Python 3.10+
- TensorFlow
- Pandas, NumPy
