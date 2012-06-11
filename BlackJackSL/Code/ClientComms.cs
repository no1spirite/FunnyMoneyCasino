using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using BlackJackSL.BlackJackService;
using BlackJackSL.ChatService;
using BlackJackSL.Model;
using BlackJackSL.ViewModels;
using BlackJackSL.Views;
using System.Linq;
using System.Xml;
using CardViewModel=BlackJackSL.ViewModels.CardViewModel;
using BlackJackDuplexServiceClient=BlackJackSL.BlackJackService.DuplexServiceClient;
using BlackJackSendToClientReceivedEventArgs=BlackJackSL.BlackJackService.SendToClientReceivedEventArgs;
using ChatDuplexServiceClient = BlackJackSL.ChatService.DuplexServiceClient;
using ChatSendToClientReceivedEventArgs = BlackJackSL.ChatService.SendToClientReceivedEventArgs;

namespace BlackJackSL.Code
{
    public class ClientComms
    {
        BlackJackDuplexServiceClient gameReceiver;
        ChatDuplexServiceClient chatReceiver;
        CustomBinding binding = new CustomBinding(
            new PollingDuplexBindingElement(),
            new BinaryMessageEncodingBindingElement(),
            new HttpTransportBindingElement());
        string nickname;
        private PlayerCollectionViewModel pcvm = ServiceLocator.Get<PlayerCollectionViewModel>();
        private ChatViewModel cvm = ServiceLocator.Get<ChatViewModel>();
        //private TableViewModel tvm = ServiceLocator.Get<TableViewModel>();

        public ClientComms()
        {
            //string endPoint = "http://192.168.1.10/blackjack/";
            string endPoint = "http://localhost:57957/";
            //string endPoint = "http://www.funnymoneycasino.net/";

            gameReceiver = new BlackJackDuplexServiceClient(binding, new EndpointAddress(endPoint + "blackjack/BlackJackService.svc"));
            gameReceiver.SendToClientReceived += new EventHandler<BlackJackSendToClientReceivedEventArgs>(blackJackReceiver_SendToClientReceived);

            chatReceiver = new ChatDuplexServiceClient(binding, new EndpointAddress(endPoint + "chat/ChatService.svc"));
            chatReceiver.SendToClientReceived += new EventHandler<ChatSendToClientReceivedEventArgs>(chatReceiver_SendToClientReceived);
        }

        private void chatReceiver_SendToClientReceived(object sender, ChatSendToClientReceivedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.msg is TextChatMessageFromServer)
                {
                    TextChatMessageFromServer msg = (TextChatMessageFromServer) (e.msg);
                    AppendLineToTextView(
                        String.Format("{0}:[{1}] {2}", msg.nickname, DateTime.Now.ToShortTimeString(), msg.text),
                        msg.nickname);

                    //TODO: Use TextColor
                }
                else if (e.msg is JoinChatMessage)
                {
                    JoinChatMessage msg = (JoinChatMessage) (e.msg);
                    AppendLineToTextView(String.Format("{0} has entered the room [{1}]", msg.nickname, DateTime.Now),
                                         msg.nickname);
                }
                else if (e.msg is LeaveChatMessage)
                {
                    LeaveChatMessage msg = (LeaveChatMessage) (e.msg);
                    AppendLineToTextView(String.Format("{0} has left the room [{1}]", msg.nickname, DateTime.Now),
                                         msg.nickname);
                }
            }
        }

        private void blackJackReceiver_SendToClientReceived(object sender, BlackJackSendToClientReceivedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.msg is JoinGameMessageFromServer)
                {
                    JoinGameMessageFromServer msg = (JoinGameMessageFromServer) (e.msg);
                    try
                    {
                        XDocument xDoc = XDocument.Parse(msg.xmlDoc);
                        ServiceLocator.Get<TableViewModel>().LoadXml(xDoc);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if(e.msg is LeaveGameMessageFromServer)
                {
                    LeaveGameMessageFromServer msg = (LeaveGameMessageFromServer)(e.msg);
                    if (msg.nickname != ServiceLocator.Get<LoginViewModel>().Nickname)
                    {
                        List<PlayerViewModel> tempPlayerCollection = pcvm.PlayerCollection.Where(
                            p => p.PlayerName == msg.nickname).ToList();
                        foreach (var player in tempPlayerCollection)
                        {
                            if (player.PlayerId == 1)
                            {
                                pcvm.RemovePlayer(player.PlayerId, player.PlayerName);
                                pcvm.AddPlayer1ButtonContent = "Join";
                                pcvm.CanAddPlayer1 = true;
                            }
                            else if (player.PlayerId == 2)
                            {
                                pcvm.RemovePlayer(player.PlayerId, player.PlayerName);
                                pcvm.AddPlayer2ButtonContent = "Join";
                                pcvm.CanAddPlayer2 = true;
                            }
                            else if (player.PlayerId == 3)
                            {
                                pcvm.RemovePlayer(player.PlayerId, player.PlayerName);
                                pcvm.AddPlayer3ButtonContent = "Join";
                                pcvm.CanAddPlayer3 = true;
                            }
                            else if (player.PlayerId == 4)
                            {
                                pcvm.RemovePlayer(player.PlayerId, player.PlayerName);
                                pcvm.AddPlayer4ButtonContent = "Join";
                                pcvm.CanAddPlayer4 = true;
                            }
                            else if (player.PlayerId == 5)
                            {
                                pcvm.RemovePlayer(player.PlayerId, player.PlayerName);
                                pcvm.AddPlayer5ButtonContent = "Join";
                                pcvm.CanAddPlayer5 = true;
                            }
                            
                        }
                    }
                }
                else if (e.msg is AddPlayerMessageFromServer)
                {
                    AddPlayerMessageFromServer msg = (AddPlayerMessageFromServer) (e.msg);
                    if (msg.nickname != ServiceLocator.Get<LoginViewModel>().Nickname)
                    {
                        if (msg.playerId == 1)
                        {
                            pcvm.AddPlayer(1, msg.nickname, false, true);
                            pcvm.AddPlayer1ButtonContent = "Remove";
                            pcvm.CanAddPlayer1 = false;
                        }
                        else if (msg.playerId == 2)
                        {
                            pcvm.AddPlayer(2, msg.nickname, false, true);
                            pcvm.AddPlayer2ButtonContent = "Remove";
                            pcvm.CanAddPlayer2 = false;
                        }
                        else if (msg.playerId == 3)
                        {
                            pcvm.AddPlayer(3, msg.nickname, false, true);
                            pcvm.AddPlayer3ButtonContent = "Remove";
                            pcvm.CanAddPlayer3 = false;
                        }
                        else if (msg.playerId == 4)
                        {
                            pcvm.AddPlayer(4, msg.nickname, false, true);
                            pcvm.AddPlayer4ButtonContent = "Remove";
                            pcvm.CanAddPlayer4 = false;
                        }
                        else if (msg.playerId == 5)
                        {
                            pcvm.AddPlayer(5, msg.nickname, false, true);
                            pcvm.AddPlayer5ButtonContent = "Remove";
                            pcvm.CanAddPlayer5 = false;
                        }
                    }
                }
                else if (e.msg is PlayerAlreadyExistsMessageFromServer)
                {
                    ServiceLocator.Get<LoginViewModel>().LoginText = "This Name Already Exists";
                    ServiceLocator.Get<LoginView>().Show();
                }
                else if (e.msg is RemovePlayerMessageFromServer)
                {
                    RemovePlayerMessageFromServer msg = (RemovePlayerMessageFromServer) (e.msg);
                    if (msg.nickname != ServiceLocator.Get<LoginViewModel>().Nickname)
                    {
                        if (msg.playerId == 1)
                        {
                            pcvm.RemovePlayer(1, msg.nickname);
                            pcvm.AddPlayer1ButtonContent = "Join";
                            pcvm.CanAddPlayer1 = true;
                        }
                        else if (msg.playerId == 2)
                        {
                            pcvm.RemovePlayer(2, msg.nickname);
                            pcvm.AddPlayer2ButtonContent = "Join";
                            pcvm.CanAddPlayer2 = true;
                        }
                        else if (msg.playerId == 3)
                        {
                            pcvm.RemovePlayer(3, msg.nickname);
                            pcvm.AddPlayer3ButtonContent = "Join";
                            pcvm.CanAddPlayer3 = true;
                        }
                        else if (msg.playerId == 4)
                        {
                            pcvm.RemovePlayer(4, msg.nickname);
                            pcvm.AddPlayer4ButtonContent = "Join";
                            pcvm.CanAddPlayer4 = true;
                        }
                        else if (msg.playerId == 5)
                        {
                            pcvm.RemovePlayer(5, msg.nickname);
                            pcvm.AddPlayer5ButtonContent = "Join";
                            pcvm.CanAddPlayer5 = true;
                        }
                    }
                }
                else if (e.msg is BetMessageFromServer)
                {
                    BetMessageFromServer msg = (BetMessageFromServer) (e.msg);
                    if (msg.nickname != ServiceLocator.Get<LoginViewModel>().Nickname)
                    {
                        PlayerViewModel player = pcvm.PlayerCollection.Where(p => p.PlayerId == msg.playerId).First();
                        player.HandCollection[0].AddChip(msg.betAmount);
                        player.HandCollection[0].HandPot += msg.betAmount;
                    }
                }
                else if (e.msg is DealMessageFromServer)
                {
                    DealMessageFromServer msg = (DealMessageFromServer) (e.msg);
                    ServiceLocator.Get<TableViewModel>().Deck.Clear(); // = msg.deck;
                    foreach (var card in msg.deck)
                    {
                        ServiceLocator.Get<TableViewModel>().Deck.Add(new CardViewModel
                                                                          {
                                                                              Rank = card.Rank.ToString(),
                                                                              Suit = card.Suit.ToString(),
                                                                              CardImage = card.CardImage,
                                                                              CardBackImage = card.CardBackImage,
                                                                              IsVisible = card.IsVisible,
                                                                              CardValue = card.CardValue
                                                                          });
                    }
                    ServiceLocator.Get<TableViewModel>().Deal();
                }
                else if (e.msg is FinishedDealingMessageFromServer)
                {
                    FinishedDealingMessageFromServer msg = (FinishedDealingMessageFromServer)(e.msg);
                    //ServiceLocator.Get<TableViewModel>().SetTimer();
                }
                else if (e.msg is StandMessageFromServer)
                {
                    StandMessageFromServer msg = (StandMessageFromServer)(e.msg);
                    if (msg.nickname != ServiceLocator.Get<LoginViewModel>().Nickname)
                    {
                        ServiceLocator.Get<TableViewModel>().Stand();
                    }
                    //ServiceLocator.Get<TableViewModel>().SetTimer();
                }
                else if (e.msg is HitMessageFromServer)
                {
                    HitMessageFromServer msg = (HitMessageFromServer)(e.msg);
                    if (msg.nickname != ServiceLocator.Get<LoginViewModel>().Nickname)
                    {
                        ServiceLocator.Get<TableViewModel>().Hit();
                    }
                }
                else if (e.msg is DoubleMessageFromServer)
                {
                    DoubleMessageFromServer msg = (DoubleMessageFromServer)(e.msg);
                    if (msg.nickname != ServiceLocator.Get<LoginViewModel>().Nickname)
                    {
                        ServiceLocator.Get<TableViewModel>().DoubleDown();
                    }
                    //ServiceLocator.Get<TableViewModel>().SetTimer();
                }
                else if (e.msg is SplitMessageFromServer)
                {
                    SplitMessageFromServer msg = (SplitMessageFromServer)(e.msg);
                    if (msg.nickname != ServiceLocator.Get<LoginViewModel>().Nickname)
                    {
                        ServiceLocator.Get<TableViewModel>().Split();
                    }
                    
                }
                else if (e.msg is ClearPlayersMessageFromServer)
                {
                    ClearPlayersMessageFromServer msg = (ClearPlayersMessageFromServer)(e.msg);
                    if (msg.nickname != ServiceLocator.Get<LoginViewModel>().Nickname)
                    {
                        foreach (
                            var player in
                                pcvm.PlayerCollection.Where(
                                    p => p.PlayerName == msg.nickname))
                        {
                            player.ClearPlayer();
                        }
                    }
                }
                else if (e.msg is ClearDealerMessageFromServer)
                {
                    ClearDealerMessageFromServer msg = (ClearDealerMessageFromServer)(e.msg);
                    ServiceLocator.Get<DealerViewModel>().ClearDealer();
                }
            }
        }

        public void GameConnect()
        {
            JoinGameMessageToServer msg = new JoinGameMessageToServer();
            msg.nickname = ServiceLocator.Get<LoginViewModel>().Nickname;
            gameReceiver.SendToServiceAsync(msg);
        }

        public void GameDisconnect()
        {
            JoinGameMessageToServer msg = new JoinGameMessageToServer();
            msg.nickname = ServiceLocator.Get<LoginViewModel>().Nickname;
            gameReceiver.SendToServiceAsync(msg);
            //GameDisconnectMessage msg = new GameDisconnectMessage();
            ////msg.nickname = ServiceLocator.Get<LoginViewModel>().Nickname;
            //gameReceiver.SendToServiceAsync(msg);
        }

        public void AddPlayerToGame(int id, bool playerInPlay)
        {
            AddPlayerMessageToServer msg = new AddPlayerMessageToServer();
            msg.nickname = ServiceLocator.Get<LoginViewModel>().Nickname;
            msg.playerId = id;
            msg.playerInPlay = playerInPlay;
            gameReceiver.SendToServiceAsync(msg);
        }

        public void RemovePlayerFromGame(int id)
        {
            RemovePlayerMessageToServer msg = new RemovePlayerMessageToServer();
            msg.nickname = ServiceLocator.Get<LoginViewModel>().Nickname;
            msg.playerId = id;
            gameReceiver.SendToServiceAsync(msg);
        }

        public void AddPlayerChips(int id, double betAmount)
        {
            BetMessageToServer msg = new BetMessageToServer();
            msg.nickname = ServiceLocator.Get<LoginViewModel>().Nickname;
            msg.betAmount = betAmount;
            msg.playerId = id;
            gameReceiver.SendToServiceAsync(msg);
        }

        public void DealCards()
        {
            DealMessageToServer msg = new DealMessageToServer();
            msg.nickname = ServiceLocator.Get<LoginViewModel>().Nickname;
            gameReceiver.SendToServiceAsync(msg);
        }

        public void Stand(int id)
        {
            StandMessageToServer msg = new StandMessageToServer();
            msg.nickname = ServiceLocator.Get<LoginViewModel>().Nickname;
            msg.playerId = id;
            gameReceiver.SendToServiceAsync(msg);
        }

        public void Hit(int playerId, int handId)
        {
            HitMessageToServer msg = new HitMessageToServer();
            msg.nickname = ServiceLocator.Get<LoginViewModel>().Nickname;
            msg.playerId = playerId;
            msg.handId = handId;
            gameReceiver.SendToServiceAsync(msg);
        }

        public void Double(int playerId, int handId)
        {
            DoubleMessageToServer msg = new DoubleMessageToServer();
            msg.nickname = ServiceLocator.Get<LoginViewModel>().Nickname;
            msg.playerId = playerId;
            msg.handId = handId;
            gameReceiver.SendToServiceAsync(msg);
        }

        public void Split(int playerId, int handId)
        {
            SplitMessageToServer msg = new SplitMessageToServer();
            msg.nickname = ServiceLocator.Get<LoginViewModel>().Nickname;
            msg.playerId = playerId;
            msg.handId = handId;
            gameReceiver.SendToServiceAsync(msg);
        }

        public void ClearPlayers()
        {
            ClearPlayersMessageToServer msg = new ClearPlayersMessageToServer();
            msg.nickname = ServiceLocator.Get<LoginViewModel>().Nickname;
            gameReceiver.SendToServiceAsync(msg);
        }


        public void FinishedDealing()
        {
            FinishedDealingMessageToServer msg = new FinishedDealingMessageToServer();
            msg.nickname = ServiceLocator.Get<LoginViewModel>().Nickname;
            gameReceiver.SendToServiceAsync(msg);
        }

        void AppendLineToTextView(string line, string name)
        {
            cvm.TextItems.Insert(0, new Chat { Message = line });
            if(name == ServiceLocator.Get<LoginViewModel>().Nickname)
                cvm.Message = "";
        }

        public void ChatConnect(string name)
        {
            JoinChatMessage msg = new JoinChatMessage();
            this.nickname = msg.nickname = name;
            chatReceiver.SendToServiceAsync(msg);
        }

        public void ChatDisconnect()
        {
            DisconnectMessage msg = new DisconnectMessage();
            //jcm.nickname = ServiceLocator.Get<LoginViewModel>().Nickname;
            chatReceiver.SendToServiceAsync(msg);
        }

        public void Send()
        {
            TextChatMessageToServer msg = new TextChatMessageToServer();
            msg.text = cvm.Message;
            msg.textColor = 0;

            chatReceiver.SendToServiceAsync(msg);
        }
    }
}
