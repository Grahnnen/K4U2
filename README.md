# Översikt

Lösningen består av två separata Web API-projekt:

Service A – Content API <br />
Service B – LLM Proxy API

Service A anropar Service B för att generera AI-innehåll.<br />
Därför måste båda projekten köras samtidigt.

## Starta projekten lokalt

### Lösningen består av två separata Web API-projekt:

```
 Service A / Content API
Service B / LLM Proxy API
```
Båda måste köras samtidigt eftersom Service A anropar Service B för att generera text.

```
Starta båda i Visual Studio
Öppna solutionen i Visual Studio.
Högerklicka på solutionen i Solution Explorer.
Välj Properties.
Gå till Startup Project.
Välj Multiple startup projects.
Sätt både Content API och LLM Proxy API till Start.
Tryck Start eller F5.
```

Kontrollera sedan att båda API:erna startat utan fel.

# Sätta upp User Secrets

Projektet använder hemligheter för två saker:

intern API-nyckel mellan Service A och Service B
OpenAI API-nyckel i Service B

## Kör kommandona i rätt projektmapp, eftersom User Secrets sparas per projekt.

### Service A / Content API

Gå till mappen för Content API och kör:
```
dotnet user-secrets init
dotnet user-secrets set "ServiceB:BaseUrl" "https://localhost:7002"
dotnet user-secrets set "ServiceB:ApiKey" "min-interna-nyckel-123"
```
Porten i ServiceB:BaseUrl måste matcha den port som Service B faktiskt kör på lokalt.

### Service B / LLM Proxy API

Gå till mappen för LLM Proxy API och kör:
```
dotnet user-secrets init
dotnet user-secrets set "InternalApiKey:Value" "min-interna-nyckel-123"
dotnet user-secrets set "OpenAI:BaseUrl" "https://api.openai.com/"
dotnet user-secrets set "OpenAI:ApiKey" "DIN_OPENAI_API_NYCKEL"
dotnet user-secrets set "OpenAI:Model" "gpt-5.4"
```
### Viktigt om interna nyckeln

Dessa två värden måste vara identiska:

```ServiceB:ApiKey``` i Service A
```InternalApiKey:Value``` i Service B

Om de inte matchar kommer Service B att svara med 401 Unauthorized.

## Exempel på lokal körning
Starta båda Api
Testa API:et via Scalar eller annan klient
Skapa först ett innehåll i Service A
Kör sedan generate-endpointen i Service A för att låta Service B anropa OpenAI

Kontrollera att:

båda projekten körs
```ServiceB:ApiKey``` och ```InternalApiKey:Value``` är exakt samma
```ServiceB:BaseUrl``` pekar på rätt port

Kontrollera att:

```OpenAI:ApiKey``` är korrekt satt i Service B
modellen i ```OpenAI:Model``` är korrekt
Service B kan nå OpenAI API
