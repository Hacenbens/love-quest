using LoveQuest.Web;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Personalise without rebuilding: appsettings.json, or env vars such as
// LoveQuest__PlayerName=Sara  LoveQuest__Lines__0="..."
builder.Services.Configure<QuestOptions>(builder.Configuration.GetSection("LoveQuest"));
builder.Services.AddSingleton(sp => new LoveLetter(sp.GetRequiredService<IOptions<QuestOptions>>().Value));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

// What the game needs to start — never the letter itself.
app.MapGet("/api/quest", (IOptions<QuestOptions> o) => new
{
    playerName = o.Value.PlayerName,
    heartsPerLine = o.Value.HeartsPerLine,
    totalLines = o.Value.Lines.Length,
});

// Lines are released by the server only as they are earned.
app.MapGet("/api/letter", (int hearts, LoveLetter letter) => letter.Progress(hearts));

app.Run();

public partial class Program; // lets the integration tests host the app
