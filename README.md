# Love Quest

A tiny ASP.NET Core (.NET 10) game for telling a player you love them: catch falling
hearts, dodge broken ones, and every 5 hearts unlocks one more line of a love letter.
The signature is revealed when the letter is complete.

Built as the application for a Jenkins CI lab (controller + Docker `.NET` agent).

## Run

```bash
dotnet run --project src/LoveQuest.Web
```

Personalise without rebuilding (`appsettings.json` or environment variables):

```bash
LoveQuest__PlayerName=Sara LoveQuest__From=Me dotnet run --project src/LoveQuest.Web
```

| endpoint | purpose |
|---|---|
| `/` | the game |
| `/api/quest` | player name, hearts per line, number of lines (never the letter) |
| `/api/letter?hearts=N` | the lines earned with N hearts |
| `/health` | health check |

## CI pipeline (Jenkinsfile)

Runs on an agent labelled `dotnet`:

1. **Checkout** — `checkout scm`
2. **Restore** — `dotnet restore`
3. **Build** — `dotnet build -c Release --no-restore`
4. **Test** — `dotnet test -c Release --no-build` (8 unit + 4 integration tests, TRX results)
5. **Publish** — `dotnet publish -c Release --no-build -o publish`
6. **Archive** — `publish/**` archived and fingerprinted in Jenkins
