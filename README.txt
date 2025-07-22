# TEKsystem Test – Systembeskrivning

## User Story

Som användare av försäkringssystemet vill jag kunna slå upp fordonsinformation via registreringsnummer och kunna hämta en persons 
försäkringar via personnummer så att jag snabbt får en samlad bild av försäkringsprodukter och kostnader, inklusive fordonsinformation 
för bilförsäkringar.

Acceptanskriterier / Krav

    Det finns två separata API-projekt (microservices):

        Endpoint 1: Tar emot ett fordonsregistreringsnummer och returnerar fordonsdata.
        Endpoint 2: Tar emot ett personnummer och returnerar en lista över personens försäkringar med månadskostnader.

    Om personen har bilförsäkring ska fordonsinformationen hämtas från Endpoint 1 och inkluderas i svaret från Endpoint 2.

    Försäkringsprodukter och månadskostnader:

        Djursförsäkring: 10 USD
        Personlig sjukvårdsförsäkring: 20 USD
        Bilförsäkring: 30 USD

    API:erna ska kunna köras och testas separat, men Endpoint 2 integrerar dynamiskt med Endpoint 1.

    Lösningen ska hantera felaktiga eller saknade indata på ett meningsfullt sätt (t.ex. 404 vid ej hittad data).

---

## Onboarding

Det här projektet består av flera separata komponenter och API:er byggda i .NET 8.0, organiserade i en lösning (WebApplication_TEKsystem-Test.sln). 
För att snabbt komma igång rekommenderas följande:

    Miljö: Använd Windows 11 och Visual Studio 2022 (version 17.14.8 eller senare). Ingen extern konfiguration eller databas behövs, allt är 
    självförsörjande.

    Bygg och kör: Bygg projekten i den ordning som anges i lösningen — först datamodeller och bibliotek, sedan API-projekten och till sist 
    testkonsolen. Detta säkerställer att beroenden hanteras korrekt.

    Starta: Öppna tre instanser av Visual Studio och starta varje API-projekt i debugläge var för sig på respektive port (7077 och 7240). Starta 
    sedan testkonsolen som automatiskt kör integrationstester mot båda API:erna.

    Test och felsök: Testerna körs i konsolfönstret och visar direkt svaren från API:erna. Fel eller avvikelser loggas för enkel felsökning.

    Feature toggles: Funktionalitet i API:erna kan aktiveras eller inaktiveras via appsettings.json under "FeatureToggles", vilket gör det enkelt 
    att testa olika scenarier.

    Se YAML-filen för en mer exakt flödesbeskrivning av de olika projekten som ingår.

---

## Arkitektur & Designbeslut

Systemet är uppdelat i flera återanvändbara komponenter för att möjliggöra separation av ansvar, ökad testbarhet och framtida utbyggnad. 
Projektet använder .NET och bygger på ett modulärt angreppssätt:

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

---

### Designval

- **Lös koppling mellan moduler**: Genom att separera datamodeller och datalager i egna projekt är det enkelt att byta ut implementationer 
eller testa i isolering.
- **Delade modeller**: Alla komponenter använder samma `DataModels`-projekt, vilket ger konsekvent datadefinition.
- **Fler API:er för parallell testning**: Genom att ha två Web API:er kan vi testa och jämföra olika implementeringar eller konfigurationer.
- **Inga externa beroenden**: Projektet är helt självförsörjande och kräver inga externa databaser eller tredjepartsbibliotek.

---

##  Så kör man lösningen lokalt i din PC

### Förberedelser

- Windows11
- Visual Studio 2022, version 17.14.8 eller senare
- .NET 8.0 (beroende på implementation)
- Ingen extern konfiguration krävs

### Bygglösning i rätt ordning

1. Öppna lösningen i Visual Studio.
2. Bygg lösningen, om ordningsföljd krävs, gör i följande ordning:
    - `ThreadPilot_DataModels`
    - `ThreadPilot_Costs`
    - `ThreadPilot_Customers_Database`
    - `ThreadPilot_Vehicles_Databases`
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

## API-versionering

I nuläget har våra Web API:er (WebApplication_TEKsystem-Test och WebApplication_TEKsystem-Test-B) ingen explicit API-versionering. 
Detta kan bli en begränsning när vi vill införa nya funktioner eller ändra befintlig funktionalitet utan att bryta existerande klienter. 
API-versionering är därför en rekommenderad best practice för att kunna hantera utveckling och förändringar på ett kontrollerat sätt.
Varför API-versionering?

    Möjliggör att flera versioner av samma API kan existera samtidigt.
    Skyddar existerande användare från plötsliga förändringar och brytningar.
    Underlättar vidareutveckling och migrering av klienter i olika takt.
    Gör det tydligt för konsumenter vilken version av API:et de använder.

URL-baserad versionering görs enkelt såhär i en framtida version av källkoden:

namespace WebApplication_TEKsystem_Test.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VehicleController : ControllerBase
    {
        // ... befintlig kod ...
    }
}

namespace WebApplication_TEKsystem_Test_B.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class InsuranceController : ControllerBase
    {
        // ... befintlig kod ...
    }
}

Akti

I Program.cs (eller Startup.cs) lägger vi till följande konfiguration för att aktivera API-versionering:

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

---

## 🧪 Testning

Teststrategin bygger på integrationstestning via en separat konsolapplikation (ConsoleApp_Tester) som anropar de två 
Web API:erna i lösningen. Genom att simulera riktiga anrop testas hela flödet från indata till svar, inklusive validering, 
domänlogik och felhantering. Syftet är att säkerställa att både kunduppslagning, fordonsdata och kostnadsberäkning 
fungerar som avsett.

Testerna består av anrop till båda API:erna för att utvärdera svar givet olika indata. Exempelvis:

    Sökning av fordon
    Hämtning av kunddata
    Beräkning av kostnader
    Edge cases (t.ex. saknade kunder eller fordon)

Systemets modulära arkitektur med löst kopplade komponenter möjliggör isolerad testning av varje del. Alla API:er använder 
inbyggda datastrukturer utan beroende till externa databaser, vilket ger reproducerbara och kontrollerade testscenarier. Feature 
toggles i appsettings.json används för att slå av och på specifika funktioner, vilket möjliggör test av olika konfigurationer 
utan kodändringar.

Alla svar visas direkt i konsolfönstret. Vid fel eller avvikelser loggas meddelanden, vilket ger snabb återkoppling under 
utveckling och felsökning. Strategin lämpar sig särskilt väl för lokal testning och validering innan eventuell automatisering 
eller produktionssättning.

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

## YAML

workflows build pipeline added in .github/workflows/
YAML file => TEKsystems-Test-99.2.yml

---

## Reflektion

Mitt senaste större projekt handlade om att migrera data som en del av ett omfattande ERP-byte. Där arbetade jag med 
att bygga domänspecifika datamodeller i C# som hanterade konverteringslogik mellan gamla och nya strukturer, och som 
samtidigt säkerställde datakvalitet och fullständig spårbarhet. Det arbetet påminde om denna uppgift i hur viktigt 
det är att arbeta modulärt och med tydlig separation mellan data, logik och gränssnitt – särskilt när man bygger 
system som behöver skalas eller förändras i takt med nya krav.

Jag har arbetat länge med Web API:er, både i affärskritiska miljöer och i prototypstadier, så uppgiften att bygga 
två parallella API:er med olika ansvar var både bekant och intressant. Det som stack ut här var möjligheten att testa 
hela lösningen helt isolerat från externa beroenden – något som är ovärderligt i tidiga utvecklingsfaser. Att sätta 
upp en testmiljö där flera API:er startas parallellt och sedan testas automatiskt via en konsolapplikation gav en 
känsla av systemintegration i miniformat, och påminde om verkliga pipelines där flera tjänster samverkar.

En utmaning var att skapa en balans mellan enkelhet och flexibilitet. Eftersom uppgiften inte krävde autentisering eller 
externa datakällor, behövde man istället lägga större vikt vid strukturen och flödet – och på att skapa kod som var 
förberedd för framtida utbyggnad. Jag valde att designa lösningen på ett sätt som gör det enkelt att införa fler 
API:er, riktiga databaskopplingar, eller t.ex. mockning för enhetstestning längre fram.

Om jag hade haft mer tid hade jag gärna utökat lösningen med enhetstester per modul, CI/CD-flöden med deployment 
till staging-miljö, samt ett mer avancerat felhanteringssystem med central loggning och larm. Jag hade även lagt 
till prestandatester och validerat hur lösningen uppför sig under last, något som är viktigt i verkliga produktionsmiljöer. 
Totalt sett var detta ett givande projekt som knöt ihop flera kompetensområden och visade hur mycket värde som kan 
skapas genom tydlig kodstruktur och testbarhet redan från start.

C. Ekman
2025-07-01

