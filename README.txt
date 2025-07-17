# TEKsystem Test – Systembeskrivning

## Arkitektur & Designbeslut

Systemet är uppdelat i flera återanvändbara komponenter för att möjliggöra separation av ansvar, ökad testbarhet och framtida utbyggnad. Projektet använder .NET och bygger på ett modulärt angreppssätt:

### ## Projektstruktur + Komponentöversikt

/Solution: WebApplication_TEKsystem-Test

├── ThreadPilot_DataModels/
├── ThreadPilot_Customers_Database/
├── ThreadPilot_Vehicles_Databases/
├── ThreadPilot_Costs/
├── WebApplication_TEKsystem-Test/
├── WebApplication_TEKsystem-Test-B/
└── ConsoleApp-Test-API/


- **`Class Library DataModels`**  
  Innehåller alla gemensamma objekt-modeller som används av databibliotek och API:er. Exempel: Customer, Vehicle, Insurance etc.

- **`Class Library Customers_Database`**  
  Abstraherar logik och datahantering relaterat till kunder. Kan enkelt bytas ut mot en riktig datakälla i framtiden.

- **`Class Library Vehicles_Database`**  
  Abstraherar logik och datahantering relaterat till fordon. Kan enkelt bytas ut mot en riktig datakälla i framtiden.

- **`Class Library Costs`**  
  Innehåller logik för kostnadsberäkning för försäkringen och kopplas mot kunder och fordon. Kapslar domänlogik om typ och pris.

- **`WebApplication TEKsystem-Test`**  
  Web API som hanterar uppslagning av fordon.

- **`WebApplication TEKsystem-Test-B`**  
  Web API som hanterar uppslagning av försäkringar.

- **`ConsoleApp_Tester`**  
  Separat konsolapplikation som kör testanrop mot båda WEP API-projekten och visar resultat direkt i terminalfönstret.

### Designval

- **Lös koppling mellan moduler**: Genom att separera datamodeller och datalager i egna projekt är det enkelt att byta ut implementationer eller testa i isolering.
- **Delade modeller**: Alla komponenter använder samma `DataModels`-projekt, vilket ger konsekvent datadefinition.
- **Fler API:er för parallell testning**: Genom att ha två Web API:er kan vi testa och jämföra olika implementeringar eller konfigurationer.
- **Inga externa beroenden**: Projektet är helt självförsörjande och kräver inga externa databaser eller tredjepartsbibliotek.

---

##  Så kör du lösningen lokalt

### Förberedelser

- Windows11
- Visual Studio 2022, version 17.14.8 eller senare
- .NET 8.0 (beroende på implementation)
- Ingen extern konfiguration krävs

### Bygglösning i rätt ordning

1. Öppna lösningen i Visual Studio.
2. Bygg lösningen, om ordningsföljd krävs, gör i följande ordning:
    - `DataModels`
    - `Cost`
    - `Customers_Database`
    - `Vehicles_Database`
    - `WebApplication_TEKsystem-Test`
    - `WebApplication_TEKsystem-Test-B`
    - `ConsoleApp-Test-API`

### Starta komponenterna

1. Öppna **tre instanser** av lösningen i Visual Studio 2022 på en Windows11-pc.

2. I första instansen, starta:  
   Projektet `WebApplication_TEKsystem-Test` i debug mode.
   Kontrollera att den kör på `https://localhost:7077`

3. I andra instansen, starta:  
   Projektet `WebApplication_TEKsystem-Test-B` i debug mode.
   Kontrollera att den kör på `https://localhost:7240`

4. I tredje instansen, starta:  
   Projektet `ConsoleApp_Tester` i debug mode.
   Programmet kör utvalda tester och visar svar från API:erna i terminalens output-fönster. Testet är sedan klart.

### Slå av och på funktionaliteten i API:erna (feature Toggling)
 - i appsettings.json finns under "FeatureToggles" möjligheten att slå av och på web appens funktioner via true eller false
  
  t ex => {
    "EnableFeatureVehiclesLookup": true
  }

---

## 🧪 Testning

Testerna består av anrop till båda API:erna för att utvärdera svar givet olika indata. Exempelvis:

- Sökning av fordon
- Hämtning av kunddata
- Beräkning av kostnader
- Edge cases (t.ex. saknade kunder eller fordon)

Alla svar visas direkt i konsolfönstret. Vid fel eller avvikelser loggas meddelanden.

---

## Error Handling

- **Try-Catch i API-lagret**: API:erna fångar exception och returnerar meningsfulla HTTP-svar (400/500 med felmeddelande).
- **Validation**: Input kontrolleras där det är relevant. Saknade parametrar returnerar `400 Bad Request`.
- **Loggning**: Konsol-loggning används i detta testscenario. I produktionsläge kan `ILogger` och t.ex. Application Insights användas.

---

## Extensibility

Systemet är byggt för att enkelt kunna utökas:

- Nya datakällor (databaser, API:er) kan kopplas in genom att ersätta nuvarande bibliotek.
- Ytterligare API:er eller testfall kan läggas till utan att bryta befintlig funktionalitet.
- Enhetstester eller mockar kan införas tack vare separationen i logik och infrastruktur.

---

## Säkerhet

Eftersom detta är ett testscenario finns ingen autentisering eller kryptering. Men designen möjliggör:

- Enkel integration av autentisering (t.ex. JWT, OAuth)
- HTTPS används för lokal testning (localhost)
- Alla beroenden är interna, vilket minskar attackytan

---

C. Ekman
2025-07-01

