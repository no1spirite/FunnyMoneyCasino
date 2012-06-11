using System;
using System.Collections.Generic;
using System.Threading;

namespace BlackJackSL.Web.Chat
{
    public class ChatServiceFactory : DuplexServiceFactory<ChatService> {}

    public class ChatService : DuplexService
    {
        //Base class (DuplexService) keeps track of all connected chatters
        //But it only deals with Session IDs, it has no concept of "chat nickname"
        //So this dictionary will map Session IDs to Nicknames:
        Dictionary<string,string> chatters = new Dictionary<string,string>();

        Timer stockTimer;
        // MasterQuotes is a Dictionary of stock symbols and a list of client sessionIds that are subscribed to that stock
        private Dictionary<string, List<string>> MasterQuotes = new Dictionary<string, List<String>>();

        public ChatService()
        {
            //Set up a stock update every 30 seconds
            this.stockTimer = new Timer(new TimerCallback(StockUpdate),null, 0, 30000);
        }

        //This is the modified StockUpdate method that now sends out realtime quotes to each subscriber
        void StockUpdate(object o)
        {
            // iterate all the symbols we have stored for clients
            foreach (string symbol in MasterQuotes.Keys)
            {
                StockTickerMessage stm = new StockTickerMessage();
                // get the list of sessionIds that are subscribed to this stock symbol...
                var clients = MasterQuotes[symbol];
                stm.stock = symbol;
                // get the quote for this stock (can also do multiple symbols but keeping it simple for now)
                var quotes = QuoteUtility.GetQuotes(new string[] {stm.stock});
                stm.price = Decimal.Parse(quotes[0].LastTrade);
                stm.LastTradeTime = quotes[0].Time;
                stm.Change = quotes[0].Change;
                stm.High = quotes[0].High;
                stm.Open = quotes[0].Open;
                stm.Low = quotes[0].Low;
                // send out the stock update message to all the clients subscribed to this symbol
                PushToSelectedClients(stm, clients);
            }
        }

        protected override void OnMessage(string sessionId, DuplexMessage data)
        {
            if (data is JoinChatMessage)
            {
                //If a chatter joined, let all other chatters know
                JoinChatMessage msg = (JoinChatMessage)data;
                if (!chatters.ContainsValue(msg.nickname))
                {
                    chatters.Add(sessionId, msg.nickname);
                    PushToAllClients(data);
                }
            }
            else if(data is LeaveChatMessage)
            {
                OnDisconnected(sessionId);
            }
            else if (data is TextChatMessageToServer)
            {
                //If a chatter sent a message, broadcast it to all other chatters
                TextChatMessageToServer msg = (TextChatMessageToServer)data;
                // Check for a stock subscription---
                if (msg.text.ToLower().Contains("subscribe"))
                {
                    string symbol = msg.text.Split(' ')[1];
                    symbol = symbol.ToUpper();
                    // is the symbol already there? If not, add it:
                    if(!MasterQuotes.ContainsKey( symbol))
                    {
                        MasterQuotes.Add(symbol, new List<string> {sessionId });
                    }
                    else
                    {
                        //sessionIds is List<String> containing the sessionIds subscribed to this stock symbol
                        var sessionIds = MasterQuotes[symbol];
                        sessionIds.Add(sessionId);
                    }
                }
                else
                {
                    TextChatMessageFromServer outMsg = new TextChatMessageFromServer();
                    outMsg.text = msg.text;
                    outMsg.textColor = msg.textColor;
                    //Incoming chat message does not have the chatter's nickname, so we add it
                    outMsg.nickname = chatters[sessionId];
                    PushToAllClients(outMsg);
                }
            }
        }

        protected override void OnDisconnected(string sessionId)
        {
            //If a chatter disconnected, let all other chatters know
            string nickname;
            if (chatters.TryGetValue(sessionId, out nickname))
            {
                LeaveChatMessage lcm = new LeaveChatMessage();
                lcm.nickname = nickname;
                PushToAllClients(lcm);
                chatters.Remove(sessionId);
                // we could also unsubscribe from all his stock symbols here
            }
        }
    }
}