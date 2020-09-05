using HtmlAgilityPack;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoinChecker.Crawler
{
    public class CoinBeneCrawler : IDisposable
    {
        public IWebDriver Driver { get; set; }
        private bool disposed = false;
        public static readonly string url = "https://www.coinbene.com/spot/exchange";

        public CoinBeneCrawler()
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
            coinType = coinType.ToUpper().Replace("/", "_");
            Driver.Navigate().GoToUrl(url + "/" + coinType);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            try
            {
                var value = wait.Until(driver =>
                {
                    return driver.FindElement(By.XPath("//div[contains(@class,'typo mr5 position-relative zindex-1 ff-number cursor-pointer typo-nowrap-visible typo-text-1-down')]"));
                });

                //Console.WriteLine(value.Text);
                //Driver.Close();

                return Task.FromResult<string>(value.Text);
            }
            catch (WebDriverTimeoutException e)
            {
                Driver.Close();
                Console.WriteLine(e.Message);
                return Task.FromResult<string>("0");
            }
        }

       
    }
}
