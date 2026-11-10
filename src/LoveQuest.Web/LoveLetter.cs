namespace LoveQuest.Web;

/// <summary>Who the quest is for, who it is from, and the letter the player earns.</summary>
public sealed class QuestOptions
{
    public string PlayerName { get; set; } = "Player Two";
    public string From { get; set; } = "Someone who adores you";
    public int HeartsPerLine { get; set; } = 5;
    public string[] Lines { get; set; } = [];
}

public sealed record LetterProgress(string[] Unlocked, int TotalLines, int HeartsToNextLine, bool Complete, string? From);

/// <summary>
/// The game rule, kept out of the browser so it can be unit-tested:
/// every <see cref="QuestOptions.HeartsPerLine"/> hearts caught unlocks one more line.
/// </summary>
public sealed class LoveLetter(QuestOptions options)
{
    public LetterProgress Progress(int heartsCaught)
    {
        if (options.HeartsPerLine <= 0)
            throw new InvalidOperationException("HeartsPerLine must be positive.");

        var hearts = Math.Max(0, heartsCaught);
        var total = options.Lines.Length;
        var unlockedCount = Math.Min(total, hearts / options.HeartsPerLine);
        var complete = unlockedCount == total;
        var toNext = complete ? 0 : options.HeartsPerLine - hearts % options.HeartsPerLine;

        return new LetterProgress(
            options.Lines[..unlockedCount],
            total,
            toNext,
            complete,
            complete ? options.From : null); // the signature is the final reveal
    }
}
