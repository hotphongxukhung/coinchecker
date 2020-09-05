﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace CoinChecker.Crawler
{
    public class AppLiquidCrawler: IDisposable
    {
        public IWebDriver Driver { get; set; }
        private bool disposed = false;
        public static readonly string url = "https://app.liquid.com/exchange";

        public AppLiquidCrawler()
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
            coinType = coinType.ToUpper().Replace("/", "");
            Driver.Navigate().GoToUrl(url + "/" + coinType);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            try
            {
                var value = wait.Until(driver =>
                {
                    return driver.FindElement(By.XPath("//span[@class='numbercell__significant']"));
                });

                //Console.WriteLine(value.Text);
                //Driver.Close();

                return Task.FromResult<string>(value.Text);
            }
            catch (WebDriverTimeoutException e)
            {
                Console.WriteLine(e.Message);
                return Task.FromResult<string>("0");
            }
        }
    }
}
