using System.Runtime.Serialization;
using System.ServiceModel;

namespace BlackJackSL.Web.Chat
{
    [DataContract]
    public class JoinChatMessage: DuplexMessage
    {
        [DataMember]
        public string nickname;

    }

    [DataContract]
    public class LeaveChatMessage : DuplexMessage
    {
        [DataMember]
        public string nickname;
    }

    [DataContract]
    public class TextChatMessageToServer : DuplexMessage
    {
        [DataMember]
        public string text;
        [DataMember]
        public int textColor;
    }

    [DataContract]
    public class TextChatMessageFromServer : TextChatMessageToServer
    {
        [DataMember]
        public string nickname;
    }

    [DataContract]
    public class StockTickerMessage : DuplexMessage
    {
        [DataMember]
        public string stock;
        [DataMember]
        public decimal price;
        [DataMember] 
        public string LastTradeTime;
        [DataMember] 
        public string Change;
        [DataMember]
        public string Open;
        [DataMember]
        public string High;
        [DataMember]
        public string Low;


    }
}