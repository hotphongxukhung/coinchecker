using CoinChecker.Model;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoinChecker.Crawler
{
    public class CrawlerTool
    {
        private bool disposed = false;
        public CrawlerTool()
        {

        }

        public Task Loop(string coinType, double percent, CoinBeneCrawler cbc, CoinRealCrawler crc, AppLiquidCrawler alc)
        {
            return Task.Run(async () =>
            {
                while (!disposed)
                {
                    try
                    {
                        PageResult pr = GetPageResult(coinType, cbc, crc, alc).Result;

                        string cbcString = pr.cbcString;
                        string crcString = pr.crcString;
                        string alcString = pr.alcString;

                        Console.WriteLine(cbcString);
                        Console.WriteLine(crcString);
                        Console.WriteLine(alcString);

                        double cbcValue = float.Parse(cbcString);
                        double crcValue = float.Parse(crcString);
                        double alcValue = float.Parse(alcString);

                        Console.WriteLine(cbcValue);
                        Console.WriteLine(crcValue);
                        Console.WriteLine(alcValue);

                        if (cbcValue >= crcValue * (1 + percent / 100))
                        {
                            var higherPercent = (cbcValue / crcValue - 1) * 100;
                            SlackMessage.SendMessageToSlack(coinType + " ở CoinBene cao hơn CoinReal " + higherPercent + "%");
                        }
                        if (cbcValue >= alcValue * (1 + percent / 100))
                        {
                            var higherPercent = (cbcValue / alcValue - 1) * 100;
                            SlackMessage.SendMessageToSlack(coinType + " ở CoinBene cao hơn AppLiquid " + higherPercent + "%");
                        }
                        if (crcValue >= cbcValue * (1 + percent / 100))
                        {
                            var higherPercent = (crcValue / cbcValue - 1) * 100;
                            SlackMessage.SendMessageToSlack(coinType + " ở CoinReal cao hơn CoinBene " + higherPercent + "%");
                        }
                        if (crcValue >= alcValue * (1 + percent / 100))
                        {
                            var higherPercent = (crcValue / alcValue - 1) * 100;
                            SlackMessage.SendMessageToSlack(coinType + " ở CoinReal cao hơn AppLiquid " + higherPercent + "%");
                        }
                        if (alcValue >= cbcValue * (1 + percent / 100))
                        {
                            var higherPercent = (alcValue / cbcValue - 1) * 100;
                            SlackMessage.SendMessageToSlack(coinType + " ở AppLiquid cao hơn CoinBene " + higherPercent + "%");
                        }
                        if (alcValue >= crcValue * (1 + percent / 100))
                        {
                            var higherPercent = (alcValue / crcValue - 1) * 100;
                            SlackMessage.SendMessageToSlack(coinType + " ở AppLiquid cao hơn CoinReal " + higherPercent + "%");
                        }
                    }
                    catch (WebDriverException ex)
                    {
                        if (!ex.Message.Contains("Timed out"))
                            return;
                        Thread.Sleep(10000);
                    }
                    catch (Exception ex)
                    {
                        disposed = true;
                        Console.WriteLine(ex);
                    }
                    Thread.Sleep(30000);
                }
            });
        }

        public Task<PageResult> GetPageResult(string coinType, CoinBeneCrawler cbc, CoinRealCrawler crc, AppLiquidCrawler alc)
        {
            PageResult pr = new PageResult();
            Task.Run(() =>
             {
                 var alcTask = alc.Start(coinType);
                 var crcTask = crc.Start(coinType);
                 var cbcTask = cbc.Start(coinType);
                 
                 pr.cbcString = cbcTask.Result;
                 pr.crcString = crcTask.Result;
                 pr.alcString = alcTask.Result;
             });
            return Task.FromResult(pr);
        }

        public class PageResult
        {
            public string cbcString { get; set; }
            public string crcString { get; set; }
            public string alcString { get; set; }
        }

    }
}
