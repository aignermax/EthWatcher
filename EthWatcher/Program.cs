using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EthWatcher
{
    
    public class CoinMarketCapResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public int rank { get; set; }
        public double price_usd { get; set; }
        public double price_eur { get; set; }
        public double price_btc { get; set; }
    }

    public class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        static string LinkEthData = "https://api.coinmarketcap.com/v1/ticker/?convert=EUR&limit=10";
        static void Main(string[] args)
        {

            // hide console window
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
            while (true)
            {
                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString(LinkEthData);
                    List<CoinMarketCapResponse> result = JsonConvert.DeserializeObject<List<CoinMarketCapResponse>>(json);
                    foreach (CoinMarketCapResponse c in result)
                    {
                        if (c.id == "ethereum")
                        {
                            if (c.price_eur > 1000)
                            {
                                ShowWindow(handle, SW_SHOW);
                                Console.WriteLine("Ether is over 1000 Euro");
                                Console.ReadLine();
                            }
                        }
                    }
                }
                Thread.Sleep(600 * 1000); // check all 10 seconds
            }
        }
    }
}
