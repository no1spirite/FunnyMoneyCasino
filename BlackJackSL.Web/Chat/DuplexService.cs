using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace BlackJackSL.Web.Chat
{
    /// <summary>
    /// Derive from this class to create a duplex Service Factory to use in an .svc file
    /// </summary>
    /// <typeparam name="T">The Duplex Service type (typically derived from DuplexService)</typeparam>
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
            CustomBinding binding = new CustomBinding(
                new PollingDuplexBindingElement(),
                new BinaryMessageEncodingBindingElement(),
                new HttpTransportBindingElement());
   
            service.Description.Behaviors.Add(new ServiceMetadataBehavior());
            service.AddServiceEndpoint(typeof(IUniversalDuplexContract), binding, "");
            service.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
            return service;
        }
    }

    /// <summary>
    /// Derive your own Duplex service from this class
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public abstract class DuplexService : IUniversalDuplexContract
    {    
        object syncRoot = new object();
        Dictionary<string, IUniversalDuplexCallbackContract> clients = new Dictionary<string, IUniversalDuplexCallbackContract>();
        
        /// <summary>
        /// This will be called when a new client is connected
        /// </summary>
        /// <param name="sessionId">Session ID of the newly-connected client</param>
        protected virtual void OnConnected(string sessionId) { }
        
        /// <summary>
        /// This will be called when a client is disconnected
        /// </summary>
        /// <param name="sessionId">Session ID of the newly-disconnected client</param>
        protected virtual void OnDisconnected(string sessionId) { }

        /// <summary>
        /// This will be called when a message is received from a client
        /// </summary>
        /// <param name="sessionId">Session ID of the client sending the message</param>
        /// <param name="message">The message that was received</param>
        protected virtual void OnMessage(string sessionId, DuplexMessage message) { }

        /// <summary>
        /// Pushes a message to all connected clients
        /// </summary>
        /// <param name="message">The message to push</param>
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

        protected void PushToSelectedClients(DuplexMessage message,List<string> sessions)
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
            if(!clients.ContainsKey(clientSessionId )) return;
            
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
            if (msg is DisconnectMessage)
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
    }

    /// <summary>
    /// "Regular" part of Duplex contract:  From Silverlight to the Service
    /// </summary>
    [ServiceContract(Name="DuplexService", CallbackContract = typeof(IUniversalDuplexCallbackContract))]
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
    public class ConnectMessage : DuplexMessage  { }

    /// <summary>
    /// Standard "Disconnect" message - clients must use this message to disconnect
    /// </summary>
    [DataContract(Namespace = "http://samples.microsoft.com/silverlight2/duplex")]
    public class DisconnectMessage :DuplexMessage { }

    /// <summary>
    /// Base message class. Please add [KnownType] attributes as necessary for every 
    /// derived message type.
    /// </summary>
    [DataContract(Namespace = "http://samples.microsoft.com/silverlight2/duplex")]
    [KnownType(typeof(ConnectMessage))]
    [KnownType(typeof(DisconnectMessage))]
    [KnownType(typeof(JoinChatMessage))]
    [KnownType(typeof(LeaveChatMessage))]
    [KnownType(typeof(TextChatMessageToServer))]
    [KnownType(typeof(TextChatMessageFromServer))]
    [KnownType(typeof(StockTickerMessage))]
    public class DuplexMessage { }
}