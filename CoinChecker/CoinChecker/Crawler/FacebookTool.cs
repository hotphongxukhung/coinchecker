using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinChecker.Crawler
{
    class FacebookTool
    {
        public IWebDriver Driver { get; set; }
        private bool disposed = false;
        public static readonly string url = "https://www.facebook.com/messages/t/baohoang.dev";
        public FacebookTool()
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
                "--disable-notifications",
                "--start-maximized");
            Driver = new ChromeDriver(chromeDriverService, options);
        }

        public void Start()
        {
            Driver.Navigate().GoToUrl(url);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            try
            {
                var result = wait.Until(driver =>
                {
                    wait.Until(drive =>
                    {
                        drive.FindElement(By.XPath("//input[@id='email']")).SendKeys("blassreiter0610@gmail.com");
                        drive.FindElement(By.XPath("//input[@id='pass']")).SendKeys("K@zen0sakura27861o1999");
                        drive.FindElement(By.XPath("//input[@data-testid='royal_login_button']")).Click();
                        return "";
                    });

                    wait.Until(drive =>
                    {
                        var focus = drive.FindElement(By.XPath("//div[@class='_kmc _7kpg navigationFocus']"));
                        Actions act = new Actions(drive);
                        act.Click(focus);
                        return "";
                    });

                    wait.Until(drive =>
                    {
                        drive.FindElement(By.XPath("//div[@class='_kmc _7kpg navigationFocus']/descendant::div[@class='_1mf _1mj']/span/*"));
                        drive.FindElement(By.XPath("//a[@data-hover='tooltip']")).Click();
                        return "";
                    });
                    return "";
                });

                //Console.WriteLine(value.Text);
                //Driver.Close();
            }
            catch (WebDriverTimeoutException e)
            {
                Console.WriteLine(e.Message);
            }

            
        }

    }
}
