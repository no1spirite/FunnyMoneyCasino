using System.Runtime.Serialization;
using System.ServiceModel;
using System.Timers;
using System.Xml;
using BlackJackSL.Web.BlackJack.Objects;

namespace BlackJackSL.Web.BlackJack
{
    [DataContract]
    public class JoinGameMessageToServer : DuplexMessage
    {
        [DataMember]
        public string nickname;

        [DataMember] 
        public int playerId;
    }

    [DataContract]
    public class JoinGameMessageFromServer : JoinGameMessageToServer
    {
        [DataMember]
        public string xmlDoc;
    }

    [DataContract]
    public class LeaveGameMessageToServer : DuplexMessage
    {
        [DataMember]
        public string nickname;
    }

    [DataContract]
    public class LeaveGameMessageFromServer : LeaveGameMessageToServer
    {

    }

    [DataContract]
    public class PlayerAlreadyExistsMessageFromServer : DuplexMessage
    {

    }

    [DataContract]
    public class AddPlayerMessageToServer : DuplexMessage
    {
        [DataMember]
        public string nickname;

        [DataMember]
        public int playerId;

        [DataMember] 
        public bool playerInPlay;
    }

    [DataContract]
    public class AddPlayerMessageFromServer : JoinGameMessageToServer
    {

    }

    [DataContract]
    public class RemovePlayerMessageToServer : DuplexMessage
    {
        [DataMember]
        public string nickname;

        [DataMember]
        public int playerId;
    }

    [DataContract]
    public class RemovePlayerMessageFromServer : JoinGameMessageToServer
    {

    }

    [DataContract]
    public class BetMessageToServer : DuplexMessage
    {
        [DataMember]
        public string nickname;

        [DataMember]
        public int playerId;

        [DataMember]
        public double betAmount;
    }

    [DataContract]
    public class BetMessageFromServer : BetMessageToServer
    {

    }

    [DataContract]
    public class DealMessageToServer : DuplexMessage
    {
        [DataMember]
        public string nickname;
    }

    [DataContract]
    public class DealMessageFromServer : DealMessageToServer
    {
        [DataMember]
        public Deck deck;
    }

    [DataContract]
    public class StandMessageToServer : DuplexMessage
    {
        [DataMember]
        public string nickname;

        [DataMember]
        public int playerId;
    }

    [DataContract]
    public class StandMessageFromServer : StandMessageToServer
    {
    }

    [DataContract]
    public class HitMessageToServer : DuplexMessage
    {
        [DataMember]
        public string nickname;

        [DataMember]
        public int playerId;

        [DataMember]
        public int handId;
    }

    [DataContract]
    public class HitMessageFromServer : HitMessageToServer
    {
    }

    [DataContract]
    public class DoubleMessageToServer : DuplexMessage
    {
        [DataMember]
        public string nickname;

        [DataMember]
        public int playerId;

        [DataMember]
        public int handId;
    }

    [DataContract]
    public class DoubleMessageFromServer : DoubleMessageToServer
    {
    }

    [DataContract]
    public class SplitMessageToServer : DuplexMessage
    {
        [DataMember]
        public string nickname;

        [DataMember]
        public int playerId;

        [DataMember]
        public int handId;
    }

    [DataContract]
    public class SplitMessageFromServer : SplitMessageToServer
    {
    }

    [DataContract]
    public class ClearPlayersMessageToServer : DuplexMessage
    {
        [DataMember]
        public string nickname;
    }

    [DataContract]
    public class ClearPlayersMessageFromServer : JoinGameMessageToServer
    {

    }

    [DataContract]
    public class FinishedDealingMessageToServer : DuplexMessage
    {
        [DataMember]
        public string nickname;
    }

    [DataContract]
    public class FinishedDealingMessageFromServer : FinishedDealingMessageToServer
    {
        //[DataMember] 
        //public Timer timer;
    }

    [DataContract]
    public class ClearDealerMessageFromServer : DuplexMessage
    {

    }
}
