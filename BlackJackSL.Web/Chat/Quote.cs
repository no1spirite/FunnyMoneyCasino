using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlackJackSL.Web.Chat
{
    public class Quote
    {
        public string Ticker { get; set; }
        public string LastTrade { get; set; }
        public string Time { get; set;}
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Change { get; set; }

        public Quote(){}

        public Quote( string ticker, string lastTrade, string time, string open, string high, string low, string change)
        {
            this.Ticker = ticker;
            this.LastTrade = lastTrade;
            this.Time = time;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Change = change;
        }
    }
}