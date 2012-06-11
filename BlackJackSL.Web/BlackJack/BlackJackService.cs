using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Timers;
using System.Threading;
using System.Xml;
using BlackJackSL.Web.BlackJack;
using BlackJackSL.Web.BlackJack.Objects;
using Timer=System.Timers.Timer;
using XmlWriter=BlackJackSL.Web.BlackJack.XML.XmlWriter;

namespace BlackJackSL.Web.BlackJack
{
    public abstract class DuplexServiceFactory<T> : ServiceHostFactoryBase
        where T : IUniversalDuplexContract, new()
    {
        T serviceInstance = new T();

        /// <summary>
        /// This method is called by WCF when it needs to construct the service.
        /// Typically this should not be overridden further.
        /// </summary>
        public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        {
            ServiceHost service = new ServiceHost(serviceInstance, baseAddresses);
            PollingDuplexBindingElement pollingDuplexBindingElement = new PollingDuplexBindingElement();
            pollingDuplexBindingElement.InactivityTimeout = new TimeSpan(0, 1, 0, 0);
            CustomBinding binding = new CustomBinding(
                pollingDuplexBindingElement,
                new BinaryMessageEncodingBindingElement(),
                new HttpTransportBindingElement());

            //WebHttpBinding webBinding = (WebHttpBinding)service.Description.Endpoints[0].Binding;

            service.Description.Behaviors.Add(new ServiceMetadataBehavior());
            service.AddServiceEndpoint(typeof(IUniversalDuplexContract), binding, "");
            service.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            //ServiceEndpoint jsonEP = service.AddServiceEndpoint(typeof(IService), new WebHttpBinding(), "http://localhost:57957/");
            WebHttpBinding webbinding=new WebHttpBinding(WebHttpSecurityMode.None); 
            webbinding.AllowCookies=true; 
            //webbinding.CrossDomainScriptAccessEnabled=true; 
            //EndpointAddress ea=new EndpointAddress("http://localhost:57957/");            
            WebScriptEnablingBehavior behavior = new WebScriptEnablingBehavior(); 
            behavior.DefaultOutgoingResponseFormat = WebMessageFormat.Json; 
           // behavior.DefaultBodyStyle = WebMessageBodyStyle.WrappedRequest;       
            behavior.DefaultOutgoingRequestFormat = WebMessageFormat.Json; 

            //jsonEP.Behaviors.Add(behavior);

            ServiceEndpoint endpoint = service.AddServiceEndpoint(typeof(IService), webbinding, "http://localhost:57957/");
            endpoint.Behaviors.Add(behavior);   
            //service.AddServiceEndpoint(typeof(IService), new WebHttpBinding(), "http://localhost:57957/");
            //ServiceDebugBehavior sdb = service.Description.Behaviors.Find<ServiceDebugBehavior>();
            //sdb.HttpHelpPageEnabled = false;
            return service;
        }
    }
    public class BlackJackServiceFactory : DuplexServiceFactory<BlackJackService> { }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class BlackJackService : IUniversalDuplexContract, IDisconnect, IService
    {
        //Base class (DuplexService) keeps track of all connected chatters
        //But it only deals with Session IDs, it has no concept of "chat nickname"
        //So this dictionary will map Session IDs to Nicknames:
        Dictionary<string,string> players = new Dictionary<string,string>();
        List<string> dealRequests = new List<string>();
        List<string> disconnectRequests = new List<string>();
        XmlWriter writer = new XmlWriter();
        Deck deck = new Deck();
        object syncRoot = new object();
        Dictionary<string, IUniversalDuplexCallbackContract> clients = new Dictionary<string, IUniversalDuplexCallbackContract>();

        public BlackJackService()
        {
            
        }

        public void OnMessage(string sessionId, BlackJack.DuplexMessage data)
        {
            if (data is JoinGameMessageToServer)
            {
                JoinGameMessageToServer msg = (JoinGameMessageToServer)data;  
                if(players.ContainsValue(msg.nickname))
                {
                    PlayerAlreadyExistsMessageFromServer outMsg = new PlayerAlreadyExistsMessageFromServer();
                    PushMessageToClient(sessionId, outMsg);
                }
                else
                {
                    players.Add(sessionId, msg.nickname);
                    JoinGameMessageFromServer outMsg = new JoinGameMessageFromServer();
                    outMsg.playerId = msg.playerId;
                    outMsg.nickname = players[sessionId];
                    string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
                    XmlDocument xmlDoc = new XmlDocument();
                    try
                    {
                        xmlDoc.Load(filename);
                        outMsg.xmlDoc = xmlDoc.InnerXml;
                    }
                    catch (System.IO.FileNotFoundException)
                    {

                    }
                    PushMessageToClient(sessionId, outMsg);
                }
            }
            else if (data is LeaveGameMessageToServer)
            {
                
            }
            else if (data is AddPlayerMessageToServer)
            {
                AddPlayerMessageToServer msg = (AddPlayerMessageToServer) data;

                AddPlayerMessageFromServer outMsg = new AddPlayerMessageFromServer();
                outMsg.playerId = msg.playerId;
                outMsg.nickname = players[sessionId];
                PushToAllClients(outMsg);
                writer.PlayerAdded(msg);
            }
            else if (data is RemovePlayerMessageToServer)
            {
                RemovePlayerMessageToServer msg = (RemovePlayerMessageToServer)data;

                RemovePlayerMessageFromServer outMsg = new RemovePlayerMessageFromServer();
                outMsg.playerId = msg.playerId;
                outMsg.nickname = players[sessionId];
                PushToAllClients(outMsg);
                writer.PlayerRemoved(msg);

                string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                XmlNodeList currentPlayers = xmlDoc.SelectNodes("/Table/Players/Player");

                if (currentPlayers.Count == 0)
                {
                    ClearDealerMessageFromServer clearMsg = new ClearDealerMessageFromServer();
                    PushToAllClients(clearMsg);
                    writer.DealerRemove();
                }

            }
            else if (data is BetMessageToServer)
            {
                BetMessageToServer msg = (BetMessageToServer)data;

                BetMessageFromServer outMsg = new BetMessageFromServer();
                outMsg.betAmount = msg.betAmount;
                outMsg.playerId = msg.playerId;
                outMsg.nickname = players[sessionId];
                PushToAllClients(outMsg);
                writer.PlayerBet(msg);
            }
            else if (data is DealMessageToServer)
            {
                DealMessageToServer msg = (DealMessageToServer)data;

                string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                XmlNodeList currentPlayers = xmlDoc.SelectNodes("/Table/Players/Player");

                List<string> currentActivePlayers = new List<string>();
                foreach (XmlNode player in currentPlayers)
                {
                    if(!dealRequests.Contains(player.Attributes["PlayerName"].Value))
                        currentActivePlayers.Add(player.Attributes["PlayerName"].Value);
                }

                dealRequests.Add(msg.nickname);
                bool shouldDeal = false;
                foreach (var player in currentActivePlayers)
                {
                    shouldDeal = dealRequests.Contains(player);
                    if (!shouldDeal)
                        break;
                }
                if (shouldDeal)
                {
                    DealMessageFromServer outMsg = new DealMessageFromServer();
                    deck = new Deck();
                    outMsg.deck = deck;
                    outMsg.nickname = players[sessionId];
                    PushToAllClients(outMsg);
                    writer.DealerRemove();
                    writer.DealCards(deck);
                    dealRequests.Clear();
                }
            }
            else if (data is FinishedDealingMessageToServer)
            {
                FinishedDealingMessageToServer msg = (FinishedDealingMessageToServer)data;

                FinishedDealingMessageFromServer outMsg = new FinishedDealingMessageFromServer();
                outMsg.nickname = players[sessionId];
                //outMsg.timer = new Timer(10000);
                PushToAllClients(outMsg);
            }
            else if (data is StandMessageToServer)
            {
                StandMessageToServer msg = (StandMessageToServer)data;

                StandMessageFromServer outMsg = new StandMessageFromServer();
                outMsg.nickname = players[sessionId];
                outMsg.playerId = msg.playerId;
                PushToAllClients(outMsg);
                writer.PlayerStand(msg);
            }
            else if (data is HitMessageToServer)
            {
                HitMessageToServer msg = (HitMessageToServer)data;

                HitMessageFromServer outMsg = new HitMessageFromServer();
                outMsg.nickname = players[sessionId];
                outMsg.playerId = msg.playerId;
                PushToAllClients(outMsg);
                writer.PlayerHit(msg, deck);
            }
            else if (data is DoubleMessageToServer)
            {
                DoubleMessageToServer msg = (DoubleMessageToServer)data;

                DoubleMessageFromServer outMsg = new DoubleMessageFromServer();
                outMsg.nickname = players[sessionId];
                outMsg.playerId = msg.playerId;
                PushToAllClients(outMsg);
                writer.PlayerDouble(msg, deck);
            }
            else if (data is SplitMessageToServer)
            {
                SplitMessageToServer msg = (SplitMessageToServer)data;

                SplitMessageFromServer outMsg = new SplitMessageFromServer();
                outMsg.nickname = players[sessionId];
                outMsg.playerId = msg.playerId;
                PushToAllClients(outMsg);
                writer.PlayerSplit(msg, deck);
            }
            else if (data is ClearPlayersMessageToServer)
            {
                ClearPlayersMessageToServer msg = (ClearPlayersMessageToServer)data;

                ClearPlayersMessageFromServer outMsg = new ClearPlayersMessageFromServer();
                outMsg.nickname = players[sessionId];
                PushToAllClients(outMsg);
                writer.PlayersRemoved(msg);
                
            }
        }

        protected virtual void OnConnected(string sessionId) { }

        public void OnDisconnected(string sessionId)
        {
            string nickname;
            if (players.TryGetValue(sessionId, out nickname))
            {
                LeaveGameMessageFromServer lcm = new LeaveGameMessageFromServer();
                lcm.nickname = nickname;
                PushToAllClients(lcm);
                players.Remove(sessionId);
                writer.PlayersRemoved(lcm);

                string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"/Table1.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                XmlNodeList currentPlayers = xmlDoc.SelectNodes("/Table/Players/Player");

                if (currentPlayers.Count == 0)
                {
                    ClearDealerMessageFromServer clearMsg = new ClearDealerMessageFromServer();
                    PushToAllClients(clearMsg);
                    writer.DealerRemove();
                }
            }
        }

        protected void PushToAllClients(DuplexMessage message)
        {
            lock (syncRoot)
            {
                foreach (string session in clients.Keys)
                {
                    PushMessageToClient(session, message);
                }
            }
        }

        protected void PushToSelectedClients(DuplexMessage message, List<string> sessions)
        {
            lock (syncRoot)
            {
                // send stock symbol update to every client who is subscribed to this stock ticker...
                foreach (string session in sessions)
                {
                    PushMessageToClient(session, message);
                }
            }
        }


        /// <summary>
        /// Pushes a message to one specific client
        /// </summary>
        /// <param name="clientSessionId">Session ID of the client that should receive the message</param>
        /// <param name="message">The message to push</param>
        protected void PushMessageToClient(string clientSessionId, DuplexMessage message)
        {
            if (!clients.ContainsKey(clientSessionId)) return;

            IUniversalDuplexCallbackContract ch = clients[clientSessionId];

            IAsyncResult iar = ch.BeginSendToClient(message, new AsyncCallback(OnPushMessageComplete), new PushMessageState(ch, clientSessionId));
            if (iar.CompletedSynchronously)
            {
                CompletePushMessage(iar);
            }
        }

        void OnPushMessageComplete(IAsyncResult iar)
        {
            if (iar.CompletedSynchronously)
            {
                return;
            }
            else
            {
                CompletePushMessage(iar);
            }
        }

        void CompletePushMessage(IAsyncResult iar)
        {
            IUniversalDuplexCallbackContract ch = ((PushMessageState)(iar.AsyncState)).ch;
            try
            {
                ch.EndSendToClient(iar);
            }
            catch (Exception ex)
            {
                //Any error while pushing out a message to a client
                //will be treated as if that client has disconnected
                System.Diagnostics.Debug.WriteLine(ex);
                ClientDisconnected(((PushMessageState)(iar.AsyncState)).sessionId);
            }
        }

        void IDisconnect.HelloWorld(string name)
        {

        }

        void IUniversalDuplexContract.SendToService(DuplexMessage msg)
        {
            //We get here when we receive a message from a client

            IUniversalDuplexCallbackContract ch = OperationContext.Current.GetCallbackChannel<IUniversalDuplexCallbackContract>();
            string session = OperationContext.Current.Channel.SessionId;

            //Any message from a client we haven't seen before causes the new client to be added to our list
            //(Basically, treated as a "Connect" message)
            lock (syncRoot)
            {
                if (!clients.ContainsKey(session))
                {
                    clients.Add(session, ch);
                    OperationContext.Current.Channel.Closing += new EventHandler(Channel_Closing);
                    OperationContext.Current.Channel.Faulted += new EventHandler(Channel_Faulted);
                    OnConnected(session);
                }
            }

            //If it's a Disconnect message, treat as disconnection
            if (msg is GameDisconnectMessage)
            {
                ClientDisconnected(session);
            }
            //Otherwise, if it's a payload-carrying message (and not just a simple "Connect"), process it
            else if (!(msg is ConnectMessage))
            {
                OnMessage(session, msg);
            }
        }

        void Channel_Closing(object sender, EventArgs e)
        {
            IContextChannel channel = (IContextChannel)sender;
            ClientDisconnected(channel.SessionId);
        }

        void Channel_Faulted(object sender, EventArgs e)
        {
            IContextChannel channel = (IContextChannel)sender;
            ClientDisconnected(channel.SessionId);
        }

        void ClientDisconnected(string sessionId)
        {
            lock (syncRoot)
            {
                if (clients.ContainsKey(sessionId))
                    clients.Remove(sessionId);
            }
            try
            {
                OnDisconnected(sessionId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        //Helper class for tracking both a channel and its session ID together
        class PushMessageState
        {
            internal IUniversalDuplexCallbackContract ch;
            internal string sessionId;
            internal PushMessageState(IUniversalDuplexCallbackContract channel, string session)
            {
                ch = channel;
                sessionId = session;
            }
        }

        public String SayHello(String name)
        {
            return String.Format("Hello {0}", name);
        }
    }

    [ServiceContract(Name = "DuplexService")]
    public interface IDisconnect
    {
        [OperationContract]
        void HelloWorld(string name);
    }

    //[ServiceContract]
    //public interface IService
    //{
    //    [OperationContract]
    //    [WebGet(ResponseFormat = WebMessageFormat.Json)]
    //    String SayHello(String name);
    //}

    /// <summary>
    /// "Regular" part of Duplex contract:  From Silverlight to the Service
    /// </summary>
    [ServiceContract(Name = "DuplexService", CallbackContract = typeof(IUniversalDuplexCallbackContract))]
    public interface IUniversalDuplexContract
    {
        [OperationContract(IsOneWay = true)]
        void SendToService(DuplexMessage msg);

    }

    /// <summary>
    /// "Callback" part of Duplex contract: From the Service to Silverlight
    /// </summary>
    [ServiceContract]
    public interface IUniversalDuplexCallbackContract
    {
        //[OperationContract(IsOneWay = true)]
        //void SendToClient(DuplexMessage msg);

        [OperationContract(IsOneWay = true, AsyncPattern = true)]
        IAsyncResult BeginSendToClient(DuplexMessage msg, AsyncCallback acb, object state);
        void EndSendToClient(IAsyncResult iar);


    }

    /// <summary>
    /// Standard "Connect" message - clients may use this message to connect, when no other payload is required
    /// </summary>
    [DataContract(Namespace = "http://samples.microsoft.com/silverlight2/duplex")]
    public class ConnectMessage : DuplexMessage { }

    /// <summary>
    /// Standard "Disconnect" message - clients must use this message to disconnect
    /// </summary>
    [DataContract(Namespace = "http://samples.microsoft.com/silverlight2/duplex")]
    public class GameDisconnectMessage : DuplexMessage { }

    /// <summary>
    /// Base message class. Please add [KnownType] attributes as necessary for every 
    /// derived message type.
    /// </summary>
    [DataContract(Namespace = "http://samples.microsoft.com/silverlight2/duplex")]
    [KnownType(typeof(ConnectMessage))]
    [KnownType(typeof(GameDisconnectMessage))]
    [KnownType(typeof(JoinGameMessageToServer))]
    [KnownType(typeof(JoinGameMessageFromServer))]
    [KnownType(typeof(LeaveGameMessageToServer))]
    [KnownType(typeof(LeaveGameMessageFromServer))]
    [KnownType(typeof(PlayerAlreadyExistsMessageFromServer))]
    [KnownType(typeof(AddPlayerMessageToServer))]
    [KnownType(typeof(AddPlayerMessageFromServer))]
    [KnownType(typeof(RemovePlayerMessageToServer))]
    [KnownType(typeof(RemovePlayerMessageFromServer))]
    [KnownType(typeof(BetMessageToServer))]
    [KnownType(typeof(BetMessageFromServer))]
    [KnownType(typeof(DealMessageToServer))]
    [KnownType(typeof(DealMessageFromServer))]
    [KnownType(typeof(StandMessageToServer))]
    [KnownType(typeof(StandMessageFromServer))]
    [KnownType(typeof(HitMessageToServer))]
    [KnownType(typeof(HitMessageFromServer))]
    [KnownType(typeof(DoubleMessageToServer))]
    [KnownType(typeof(DoubleMessageFromServer))]
    [KnownType(typeof(SplitMessageToServer))]
    [KnownType(typeof(SplitMessageFromServer))]
    [KnownType(typeof(ClearPlayersMessageToServer))]
    [KnownType(typeof(ClearPlayersMessageFromServer))]
    [KnownType(typeof(FinishedDealingMessageToServer))]
    [KnownType(typeof(FinishedDealingMessageFromServer))]
    [KnownType(typeof(ClearDealerMessageFromServer))]
    public class DuplexMessage { }
}