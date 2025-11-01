using Music.Core;
using Xunit;

namespace Music.Tests
{
    public class MusicUnitTests
    {
        [Fact]
        public void PlayCommand_Returns_Playing()
        {
            var player = new MusicPlayer();
            var cmd = new PlayCommand(player);
            Assert.Equal("Playing the song.", cmd.Execute());
        }

        [Fact]
        public void PauseCommand_Returns_Pausing()
        {
            var player = new MusicPlayer();
            var cmd = new PauseCommand(player);
            Assert.Equal("Pausing the song.", cmd.Execute());
        }

        [Fact]
        public void SkipCommand_Returns_Skipping()
        {
            var player = new MusicPlayer();
            var cmd = new SkipCommand(player);
            Assert.Equal("Skipping to the next song.", cmd.Execute());
        }

        [Fact]
        public void Remote_Without_Command_Throws()
        {
            var remote = new MusicRemote();
            Assert.Throws<InvalidOperationException>(() => remote.PressButton());
        }

        [Theory]
        [InlineData("play", "Playing the song.")]
        [InlineData("pause", "Pausing the song.")]
        [InlineData("skip", "Skipping to the next song.")]
        public void Remote_With_Command_Works(string action, string expected)
        {
            var player = new MusicPlayer();
            IMusicCommand cmd = action switch
            {
                "play" => new PlayCommand(player),
                "pause" => new PauseCommand(player),
                "skip" => new SkipCommand(player),
                _ => throw new ArgumentOutOfRangeException(nameof(action))
            };

            var remote = new MusicRemote();
            remote.SetCommand(cmd);
            Assert.Equal(expected, remote.PressButton());
        }
    }
}
