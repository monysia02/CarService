# System Zarządzania Warsztatem Samochodowym

## Opis projektu
System Zarządzania Warsztatem Samochodowym to aplikacja umożliwiająca efektywne zarządzanie klientami, samochodami, pracownikami oraz naprawami.
## Wymagania funkcjonalne

### Zarządzanie klientami:
- System umożliwia dodawanie nowych klientów.
- System pozwala edytować dane klienta.
- System pozwala przeglądać listę klientów.
- System umożliwia wyszukiwanie klientów po nazwisku.

### Zarządzanie samochodami:
- System umożliwia dodawanie nowych samochodów i przypisywanie ich do klientów.
- System pozwala edytować dane samochodu (marka, model, rocznik, numer rejestracyjny).
- System pozwala przeglądać listę samochodów przypisanych do klientów.
- System umożliwia wyszukiwanie samochodów po numerze rejestracyjnym.

### Zarządzanie pracownikami:
- System umożliwia dodawanie nowych pracowników.
- System pozwala edytować dane pracownika.
- System pozwala przeglądać listę pracowników.
- System umożliwia przypisywanie pracowników do napraw.

### Zarządzanie naprawami:
- System umożliwia dodawanie nowych napraw.
- System pozwala edytować istniejące naprawy (opis, koszt, termin, status).
- System pozwala przeglądać historię napraw dla danego samochodu.
- System umożliwia przypisanie naprawy do klienta, samochodu i pracownika.
- System pozwala zmieniać status naprawy (New, InProgress, OnHold, Cancelled, Finished).
- System umożliwia dodanie kosztów naprawy i ich modyfikację.
- System obsługuje terminy napraw:
  - Ustawianie terminu naprawy.
  - Edycja terminu naprawy.

## Wymagania niefunkcjonalne
- System powinien być dostępny 24/7 z maksymalnym czasem niedostępności 1h/miesiąc.
- Interfejs API powinien być zgodny ze standardem RESTful.
- System powinien być intuicyjny dla użytkownika – maksymalnie 3 kroki do dodania naprawy.
- Dokumentacja API powinna być dostępna przez Swagger UI.
- System powinien działać w przeglądarkach: Chrome, Firefox, Edge (najnowsze wersje).
- System powinien być zgodny z RODO w zakresie przechowywania danych klientów.
