using System; 
using Music.Core;
using TechTalk.SpecFlow;
using Xunit;

namespace Music.Bdd.Steps
{
    [Binding]
    public class MusicPlayerSteps
    {
        private MusicPlayer? _player;
        private MusicRemote? _remote;
        private string? _lastOutput;
        private Exception? _lastException;

        [Given(@"que tengo un reproductor de música")]
        public void GivenTengoUnReproductor() => _player = new MusicPlayer();

        [Given(@"tengo un control remoto")]
        public void GivenTengoUnControlRemoto() => _remote = new MusicRemote();

        [Given(@"configuro el comando ""(.*)""")]
        public void GivenConfiguroElComando(string comando)
        {
            Assert.NotNull(_player);
            Assert.NotNull(_remote);

            IMusicCommand cmd = comando switch
            {
                "play" => new PlayCommand(_player!),
                "pause" => new PauseCommand(_player!),
                "skip" => new SkipCommand(_player!),
                _ => throw new ArgumentOutOfRangeException(nameof(comando))
            };

            _remote!.SetCommand(cmd);
        }

        [When(@"presiono el botón del control")]
        public void WhenPresionoElBoton()
        {
            try { _lastOutput = _remote!.PressButton(); }
            catch (Exception ex) { _lastException = ex; }
        }

        [Then(@"debo ver el mensaje ""(.*)""")]
        public void ThenDeboVerElMensaje(string esperado)
        {
            Assert.Null(_lastException);
            Assert.Equal(esperado, _lastOutput);
        }

        [Then(@"debe lanzarse un error por falta de comando")]
        public void ThenDebeLanzarseError()
        {
            Assert.NotNull(_lastException);
            Assert.IsType<InvalidOperationException>(_lastException);
            Assert.Equal("No command set.", _lastException!.Message);
        }
    }
}
