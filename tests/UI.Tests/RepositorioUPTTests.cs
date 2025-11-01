using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using Xunit;

namespace UI.Tests
{
    public class RepositorioUPTTests : IDisposable
    {
        private IWebDriver? _driver;

        [Theory]
        [InlineData("chrome")]
        [InlineData("firefox")]
        public void Buscar_Tesis_Tecnologia(string browser)
        {
            _driver = CreateDriver(browser);
            _driver.Navigate().GoToUrl("https://repositorio.upt.edu.pe/");

            // Usar WebDriverWait para esperar al elemento interactuable
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver!, TimeSpan.FromSeconds(15));

            IWebElement? searchBox = null;
            try
            {
                // Intentar localizar un elemento interactuable
                searchBox = wait.Until(drv =>
                {
                    try
                    {
                        var el = drv.FindElement(By.Name("query"));
                        return (el.Displayed && el.Enabled) ? el : null;
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });

                // Enviar búsqueda (usar versión sin acento para mayor compatibilidad)
                searchBox.Clear();
                searchBox.SendKeys("tecnologia");
                searchBox.Submit();
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException)
            {
                // Fallback: usar JavaScript para establecer el valor y enviar el formulario (evita overlays/cookies)
                if (_driver is IJavaScriptExecutor js)
                {
                    var script = @"(function(){ var e = document.getElementsByName('query')[0]; if(!e) return false; e.value = 'tecnologia'; var f = e.form; if(!f){ f = document.querySelector('form'); } if(f){ f.submit(); return true; } return false; })();";
                    js.ExecuteScript(script);
                }
            }

            // Esperar resultados (selector multi-target usado por el sitio)
            wait.Until(drv => drv.FindElements(By.CssSelector("a.ds-artifact-title, .artifact-title a, .artifact-title")).Count > 0);

            var results = _driver.FindElements(By.CssSelector("a.ds-artifact-title, .artifact-title a, .artifact-title"));
            Assert.True(results.Count > 0, "No se encontraron resultados de tesis.");

            // (Opcional) Evidencia: screenshot
            Directory.CreateDirectory("artifacts");
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            screenshot.SaveAsFile(Path.Combine("artifacts", $"resultados_{browser}.png"));
        }

        private static IWebDriver CreateDriver(string browser)
        {
            if (browser == "chrome")
            {
                var options = new ChromeOptions();
                options.AddArgument("--headless=new");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                return new ChromeDriver(options);
            }
            else
            {
                var options = new FirefoxOptions();
                options.AddArgument("-headless");
                return new FirefoxDriver(options);
            }
        }

        public void Dispose()
        {
            try { _driver?.Quit(); } catch { /* ignore */ }
            _driver = null;
        }
    }
}
