using CoinChecker.Crawler;
using CoinChecker.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CoinChecker
{
    class Program
    {
        static void Main(string[] args)
        {

            List<string> coinTypes = new List<string>()
            {
                "btcv/btc",
                "btcv/usdt"
            };



            var type = "btcv/btc";

            CoinRealCrawler crc = new CoinRealCrawler();
            //Console.WriteLine(crc.Start(type));

            CoinBeneCrawler cbc = new CoinBeneCrawler();
            //Console.WriteLine(cbc.Start(type));

            AppLiquidCrawler alc = new AppLiquidCrawler();
            //Console.WriteLine(alc.Start(type));

            CrawlerTool ct = new CrawlerTool();

            ct.Loop(type,1,cbc,crc,alc);

            //FacebookTool ft = new FacebookTool();
            //ft.Start();


            Console.ReadLine();
        }


    }
}
