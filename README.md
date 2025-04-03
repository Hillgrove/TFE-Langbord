## 🌡️ Temp Flow – 3. Semester Eksamen  
**Fag: Programmering, Teknik og Systemudvikling**

Dette projekt er udviklet som en del af vores 3. semester eksamensopgave med fokus på energieffektiv temperaturstyring i hjemmet. Systemet giver brugeren mulighed for at overvåge og kontrollere temperaturen i forskellige rum via en brugervenlig grænseflade.

## 🧩 Funktionalitet

Platformen tilbyder følgende funktioner:

### 📱 Brugeroplevelse
- **Oversigt over rum og tilknyttede sensorer:** Brugeren kan se en liste over alle rum og de tilhørende sensorer.
- **Realtidsvisning af temperatur og luftfugtighed:** Aktuelle målinger vises i realtid for hvert rum.
- **Tildeling af sensorer til rum:** Mulighed for at knytte specifikke sensorer til bestemte rum.
- **Visualisering af historiske temperaturdata:** Grafisk fremstilling af tidligere temperaturmålinger for hvert rum.

### 🔗 Integration og Udvidelse
- **Kommunikation via UDP mellem Raspberry Pi og backend:** Sensorer tilsluttet en Raspberry Pi sender data til backend-serveren via UDP-protokollen.
- **REST API til frontend-integration:** Backend eksponerer et RESTful API, som frontend-applikationen bruger til at hente og vise data.

## 🛠️ Teknologi-stack

| Lag          | Teknologi                        |
|--------------|----------------------------------|
| **Frontend** | Vue.js, HTML, CSS, JavaScript    |
| **Backend**  | ASP.NET Core Web API (C#)        |
| **Database** | SQLite via Entity Framework Core |
| **Kommunikation** | UDP (Raspberry Pi) + REST API |
| **Test**     | MSTest                           |
| **DevOps**   | Visual Studio Publish + FTP      |

## 🗄️ Databasemodel

- **`Rooms`**: Indeholder information om de forskellige rum.
- **`Sensors`**: Indeholder oplysninger om sensorer, såsom type og placering.
- **`Readings`**: Gemmer historiske målinger fra sensorerne.

## 🏗️ Videreudviklingsforslag

- **Integration med smarte enheder:** Udvidelse af systemet til at understøtte kommunikation med IoT-enheder som smarte termostater.
- **Forbedret sikkerhed:** Implementering af to-faktor autentifikation og andre sikkerhedsforanstaltninger.
- **Udvidet testdækning:** Øget fokus på testdrevet udvikling og implementering af flere enhedstests.
- **CI/CD-pipeline:** Opsætning af en kontinuerlig integrations- og leveringspipeline for at automatisere build- og deploy-processer.
