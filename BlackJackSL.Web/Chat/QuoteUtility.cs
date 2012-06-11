using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace BlackJackSL.Web.Chat
{
    public static class QuoteUtility
    {
       
        public static string quoteUrl1 = "http://download.finance.yahoo.com/d/quotes.csv?s=";
        // symbol-last trade-last trade time -Open-High-Low-Change (real time)
        public static string quoteUrl2 = "&f=sl1t1ohgc6"; 

        public static List<Quote> GetQuotes (string[] tickers)
        {
            List<Quote> quotes = new List<Quote>();
            string fullQuoteUrl = quoteUrl1;
            string symbolsString = String.Empty;
            foreach(string q in tickers)
            {
                fullQuoteUrl += q + "+";
            }
            // remove the "+" sign from the end
            fullQuoteUrl = fullQuoteUrl.TrimEnd(new char[] {'+'});
            fullQuoteUrl += quoteUrl2;
            WebClient wc = new WebClient();
            string rawData = wc.DownloadString(fullQuoteUrl);
            // clear out quote marks - don't want
            rawData = rawData.Replace("\"", "");
            wc.Dispose();
            string[] quoteLines = rawData.Split(new char[] {'\r', '\n'});
            foreach(string ql in quoteLines )
            {
                if (ql != String.Empty)
                {
                    string[] rawQuote = ql.Split(',');
                    Quote quote = new Quote(rawQuote[0], rawQuote[1], rawQuote[2], rawQuote[3], rawQuote[4], rawQuote[5],
                                            rawQuote[6]);
                    quotes.Add(quote);
                }
            }
            return quotes;
        }
    }
}