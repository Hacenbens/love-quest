using LoveQuest.Web;

namespace LoveQuest.Tests;

public class LoveLetterTests
{
    private static LoveLetter Letter(int perLine = 5) => new(new QuestOptions
    {
        From = "Me",
        HeartsPerLine = perLine,
        Lines = ["one", "two", "three"],
    });

    [Fact]
    public void No_hearts_unlocks_nothing()
    {
        var p = Letter().Progress(0);
        Assert.Empty(p.Unlocked);
        Assert.Equal(5, p.HeartsToNextLine);
        Assert.False(p.Complete);
        Assert.Null(p.From);
    }

    [Theory]
    [InlineData(4, 0, 1)]
    [InlineData(5, 1, 5)]
    [InlineData(12, 2, 3)]
    public void Each_batch_of_hearts_unlocks_one_line(int hearts, int unlocked, int toNext)
    {
        var p = Letter().Progress(hearts);
        Assert.Equal(unlocked, p.Unlocked.Length);
        Assert.Equal(toNext, p.HeartsToNextLine);
    }

    [Fact]
    public void Signature_is_revealed_only_when_letter_is_complete()
    {
        var p = Letter().Progress(15);
        Assert.True(p.Complete);
        Assert.Equal(["one", "two", "three"], p.Unlocked);
        Assert.Equal("Me", p.From);
    }

    [Fact]
    public void Extra_hearts_never_overflow_the_letter()
    {
        Assert.Equal(3, Letter().Progress(999).Unlocked.Length);
    }

    [Fact]
    public void Negative_hearts_are_treated_as_zero()
    {
        Assert.Empty(Letter().Progress(-7).Unlocked);
    }

    [Fact]
    public void Non_positive_hearts_per_line_is_a_configuration_error()
    {
        Assert.Throws<InvalidOperationException>(() => Letter(perLine: 0).Progress(1));
    }
}
