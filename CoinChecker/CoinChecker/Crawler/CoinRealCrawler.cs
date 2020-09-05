using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OpenQA.Selenium.Interactions;
using System.Threading.Tasks;

namespace CoinChecker.Crawler
{
    public class CoinRealCrawler : IDisposable
    {
        public IWebDriver Driver { get; set; }
        private bool disposed = false;
        public static readonly string url = "https://www.coineal.com/trade_center.html#en_US";

        public CoinRealCrawler()
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            var options = new ChromeOptions();
            options.AddArguments("--window-size=1920,1080",
                "--disable-gpu",
                "--disable-extensions",
                "--enable-javascript",
                $"user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.125 Safari/537.36",
                "--proxy-server='direct://'",
                "--proxy-bypass-list=*",
                "--start-maximized");
            Driver = new ChromeDriver(chromeDriverService, options);
        }

        public void Dispose()
        {
            disposed = true;
            Driver.Quit();
            Driver.Dispose();
        }

        public Task<string> Start(string coinType)
        {
            var head = coinType.Split("/")[0].ToUpper();
            var tail = coinType.Split("/")[1];

            Driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            try
            {
                var value = wait.Until(driver =>
                {

                    try
                    {
                        var btn = wait.Until(drive =>
                        {
                            return drive.FindElement(By.XPath("//button[@id='btn']"));
                        });

                        btn.Click();
                    }
                    catch (WebDriverTimeoutException)
                    {

                    }

                    var tab = wait.Until(drive =>
                    {
                        return drive.FindElement(By.XPath("//li[@id='market-" + tail + "']"));
                    });
                    tab.Click();

                    var val = wait.Until(drive =>
                    {
                        return drive.FindElement(By.XPath("//div[contains(@class,'coin_compair')]/div[text()='" + head + "']/parent::div/following-sibling::div/div/span[text()!='--']")); ;
                    });

                    return val;

                });


                return Task.FromResult<string>(value.Text);
                //Console.WriteLine(value.Text);
                //Driver.Close();

            }
            catch (WebDriverTimeoutException e)
            {
                Console.WriteLine(e.Message);
                return Task.FromResult<string>("0");
            }
        }
    }
}
