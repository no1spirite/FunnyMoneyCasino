using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Resources;
using System.Windows.Threading;
using System.Xml.Linq;
using BlackJackSL.BaseClasses;
using BlackJackSL.Code;
using BlackJackSL.Model;
using BlackJackSL.ViewModels;
using System.Threading;
using BlackJackSL.SLExtensions.Input;

namespace BlackJackSL.ViewModels
{
    public class TableViewModel : ViewModelBase
    {
        public bool betting;
        public bool dealing;
        public bool dealt;
        public bool playing;
        public bool dealerPlaying;
        public bool gameFinished;
        private MediaElement playSound = new MediaElement();
        private DealerViewModel dealer = ServiceLocator.Get<DealerViewModel>();
        private ObservableCollection<PlayerViewModel> players = ServiceLocator.Get<PlayerCollectionViewModel>().PlayerCollection;
        private DispatcherTimer timer = new DispatcherTimer();
        
        public TableViewModel()
        {
            Deck = new Deck();
            OverallPlayerBank = 100000;
            Commands.Bet10Command.CanExecute += (sender, e) => e.CanExecute = CanBet10;
            Commands.Bet10Command.Executed += BetCommand_OnExecuted;
            Commands.Bet20Command.CanExecute += (sender, e) => e.CanExecute = CanBet20;
            Commands.Bet20Command.Executed += BetCommand_OnExecuted;
            Commands.Bet50Command.CanExecute += (sender, e) => e.CanExecute = CanBet50;
            Commands.Bet50Command.Executed += BetCommand_OnExecuted;
            Commands.Bet100Command.CanExecute += (sender, e) => e.CanExecute = CanBet100;
            Commands.Bet100Command.Executed += BetCommand_OnExecuted;
            Commands.DoneBettingCommand.CanExecute += (sender, e) => e.CanExecute = CanDoneBetting;
            Commands.DoneBettingCommand.Executed += DoneBetting_OnExecuted;
            Commands.StandCommand.CanExecute += (sender, e) => e.CanExecute = CanStand;
            Commands.StandCommand.Executed += StandCommand_OnExecuted;
            Commands.HitCommand.CanExecute += (sender, e) => e.CanExecute = CanHit;
            Commands.HitCommand.Executed += HitCommand_OnExecuted;
            Commands.DoubleCommand.CanExecute += (sender, e) => e.CanExecute = CanDouble;
            Commands.DoubleCommand.Executed += DoubleCommand_OnExecuted;
            Commands.SplitCommand.CanExecute += (sender, e) => e.CanExecute = CanSplit;
            Commands.SplitCommand.Executed += SplitCommand_OnExecuted;
            timer.Interval = new TimeSpan(0, 0, 0, 1, 0); // 10000 Milliseconds 
            timer.Tick += new EventHandler(Each_Tick);
        }

        private List<CardViewModel> _deck;// = ServiceLocator.Get<Deck>();
        public List<CardViewModel> Deck
        {
            get { return _deck; }
            set
            {
                _deck = value;
                OnPropertyChanged("Deck");
            }
        }

        private double _overallPlayerBank;
        public double OverallPlayerBank
        {
            get { return _overallPlayerBank; }
            set
            {
                _overallPlayerBank = value;
                OverallPlayerBankString = "£" + _overallPlayerBank;
                OnPropertyChanged("OverallPlayerBank");
            }
        }

        private string _overallPlayerBankString;
        public string OverallPlayerBankString
        {
            get { return _overallPlayerBankString; }
            set
            {
                _overallPlayerBankString = value;
                OnPropertyChanged("OverallPlayerBankString");
            }
        }

        private bool canBet10 = false;
        public bool CanBet10
        {
            get { return canBet10; }
            set
            {
                if (canBet10 == value)
                    return;
                canBet10 = value;
                OnPropertyChanged("CanBet10");
            }
        }

        private bool canBet20 = false;
        public bool CanBet20
        {
            get { return canBet20; }
            set
            {
                if (canBet20 == value)
                    return;
                canBet20 = value;
                OnPropertyChanged("CanBet20");
            }
        }

        private bool canBet50 = false;
        public bool CanBet50
        {
            get { return canBet50; }
            set
            {
                if (canBet50 == value)
                    return;
                canBet50 = value;
                OnPropertyChanged("CanBet50");
            }
        }

        private bool canBet100 = false;
        public bool CanBet100
        {
            get { return canBet100; }
            set
            {
                if (canBet100 == value)
                    return;
                canBet100 = value;
                OnPropertyChanged("CanBet100");
            }
        }

        private bool canDoneBetting = false;
        public bool CanDoneBetting
        {
            get { return canDoneBetting; }
            set
            {
                if (canDoneBetting == value)
                    return;
                canDoneBetting = value;
                OnPropertyChanged("CanDoneBetting");
            }
        }

        private bool canHit = false;
        public bool CanHit
        {
            get { return canHit; }
            set
            {
                if (canHit == value)
                    return;
                canHit = value;
                OnPropertyChanged("CanHit");
            }
        }

        private bool canStand = false;
        public bool CanStand
        {
            get { return canStand; }
            set
            {
                if (canStand == value)
                    return;
                canStand = value;
                OnPropertyChanged("CanStand");
            }
        }

        private bool canSplit = false;
        public bool CanSplit
        {
            get { return canSplit; }
            set
            {
                if (canSplit == value)
                    return;
                canSplit = value;
                OnPropertyChanged("CanSplit");
            }
        }

        private bool canDouble = false;
        public bool CanDouble
        {
            get { return canDouble; }
            set
            {
                if (canDouble == value)
                    return;
                canDouble = value;
                OnPropertyChanged("CanDouble");
            }
        }

        public void CanBet(bool canBet)
        {
            CanBet10 = canBet;
            CanBet20 = canBet;
            CanBet50 = canBet;
            CanBet100 = canBet;
        }

        public void CanPlay()
        {
            if (players.Where(p => p.ActivePlayer).Any())
            {
                PlayerViewModel player = players.Where(p => p.ActivePlayer).First();
                if (dealt && player.PlayerName == ServiceLocator.Get<LoginViewModel>().Nickname)
                {
                    CanHit = true;
                    CanStand = true;
                    if (player.HandCollection.Where(h => h.ActiveHand).First().CardCollection[0].Rank ==
                        player.HandCollection.Where(h => h.ActiveHand).First().CardCollection[1].Rank &&
                        player.HandCollection.Where(h => h.ActiveHand).First().CardCollection.Count == 2
                        && player.HandCollection.Count() < 2)
                        CanSplit = true;
                    else
                        CanSplit = false;
                    if (player.HandCollection.Where(h => h.ActiveHand).First().HandTotal == 9 ||
                        player.HandCollection.Where(h => h.ActiveHand).First().HandTotal == 10 ||
                        player.HandCollection.Where(h => h.ActiveHand).First().HandTotal == 11)
                        CanDouble = true;
                    else
                        CanDouble = false;
                    //foreach (var playerViewModel in players)
                    //{
                    //    if (dealer.Hand[1].CardValue == 11)
                    //        playerViewModel.ActiveInsurance = true;
                    //    else
                    //        playerViewModel.ActiveInsurance = false;
                    //}
                    //if (dealer.Hand[1].CardValue == 11)
                    //    player.ActiveInsurance = true;
                    //else
                    //    player.ActiveInsurance = false;
                }
                else
                {
                    CanHit = false;
                    CanStand = false;
                    CanSplit = false;
                    CanDouble = false;
                }
            }
            else
            {
                CanHit = false;
                CanStand = false;
                CanSplit = false;
                CanDouble = false;
            }
        }

        private void BetCommand_OnExecuted(object sender, ExecutedEventArgs e)
        {
            //ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer = false;
            if (gameFinished)
            {
                dealer.ClearDealer();
                foreach (var player in players.Where(p => p.PlayerName == ServiceLocator.Get<LoginViewModel>().Nickname))
                {
                    ServiceLocator.Get<ClientComms>().ClearPlayers();
                    player.ClearPlayer();
                    betting = false;
                    dealing = false;
                    dealt = false;
                    playing = false;
                    dealerPlaying = false;
                    gameFinished = false;                   
                }
            }
            betting = true;
            double betAmount = 0;
            switch (e.Command.Name)
            {
                case "Bet10Command":
                    betAmount = 10.00;
                    break;
                case "Bet20Command":
                    betAmount = 20.00;
                    break;
                case "Bet50Command":
                    betAmount = 50.00;
                    break;
                case "Bet100Command":
                    betAmount = 100.00;
                    break;
            }
            PlayerViewModel activePlayer = players.Where(p => p.ActivePlayer).First();
            if (activePlayer.HandCollection.Count == 0)
            {
                HandViewModel NewHand = new HandViewModel(0, 0, 1, 1);
                activePlayer.HandCollection.Add(NewHand);
            }
            activePlayer.HandCollection[0].HandPot += betAmount;
            OverallPlayerBank -= betAmount;
            playSound.SetSource(Application.GetResourceStream(new Uri("/BlackJackSL;component/Sounds/betchip.mp3", UriKind.RelativeOrAbsolute)).Stream); 
            activePlayer.HandCollection[0].AddChip(betAmount);
            ServiceLocator.Get<ClientComms>().AddPlayerChips(activePlayer.PlayerId, betAmount);
            CanDoneBetting = true;
        }

        private void DoneBetting_OnExecuted(object sender, ExecutedEventArgs e)
        {
            //ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer = false;
            CanDoneBetting = false;
            List<PlayerViewModel> myPlayers =
            ServiceLocator.Get<PlayerCollectionViewModel>().PlayerCollection.Where(
                p => p.PlayerName == ServiceLocator.Get<LoginViewModel>().Nickname).ToList();
            if (myPlayers.IndexOf(myPlayers.Where(p => p.ActivePlayer).First()) == myPlayers.Count - 1)
            {
                dealing = true;
                ServiceLocator.Get<ClientComms>().DealCards();
                //Deal();
            }
            ServiceLocator.Get<PlayerCollectionViewModel>().SetNextActivePlayer();
        }

        public void Deal()
        {
            ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer1 = false;
            ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer2 = false;
            ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer3 = false;
            ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer4 = false;
            ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer5 = false;
            
            CanBet(false);
            betting = false;
            dealing = true;
            //ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer = false;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(DealCards);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(FinishedDealing);
            bw.RunWorkerAsync();
            
        }

        private void FinishedDealing(object sender, RunWorkerCompletedEventArgs e)
        {
            dealing = false;
            dealt = true;
            ServiceLocator.Get<PlayerCollectionViewModel>().SetNextActivePlayer();
            if (!players.Where(p => p.ActivePlayer).Any())
                DealerPlay();
            //foreach (var player in players)
            //{
            //    if (player.PlayerName == ServiceLocator.Get<LoginViewModel>().Nickname || ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer1ButtonContent == "Join")
            //        if(player.PlayerId == 1)
            //            ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer1 = true;
            //    if (player.PlayerName == ServiceLocator.Get<LoginViewModel>().Nickname || ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer2ButtonContent == "Join")
            //        if (player.PlayerId == 2)
            //            ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer2 = true;
            //    if (player.PlayerName == ServiceLocator.Get<LoginViewModel>().Nickname || ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer3ButtonContent == "Join")
            //        if (player.PlayerId == 3)
            //            ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer3 = true;
            //    if (player.PlayerName == ServiceLocator.Get<LoginViewModel>().Nickname || ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer4ButtonContent == "Join")
            //        if (player.PlayerId == 4)
            //            ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer4 = true;
            //    if (player.PlayerName == ServiceLocator.Get<LoginViewModel>().Nickname || ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer5ButtonContent == "Join")
            //        if (player.PlayerId == 5)
            //            ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer5 = true;
            //}
            SetJoinButtons();
            CanPlay();
            ServiceLocator.Get<ClientComms>().FinishedDealing();
        }

        //public void SetTimer()
        //{
        //    timer.Start();
        //}

        int timerCount = 11;

        private void Each_Tick(object sender, EventArgs e)
        {
            if (players.Where(p => p.ActivePlayer).Any())
            {
                PlayerViewModel player = players.Where(p => p.ActivePlayer).First();
                HandViewModel hand = player.HandCollection.Where(h => h.ActiveHand).First();

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                              {
                                                                  hand.HandTimer = timerCount.ToString();
                                                              });
                timerCount--;
                if (timerCount == 0)
                {     
                    //StopTimer();
                    ServiceLocator.Get<ClientComms>().Stand(players.Where(p => p.ActivePlayer).First().PlayerId);
                    Stand();
                }
            }
        }

        //private void StopTimer()
        //{
        //    PlayerViewModel player = players.Where(p => p.ActivePlayer).First();
        //    HandViewModel hand = player.HandCollection.Where(h => h.ActiveHand).First();
        //    timer.Stop();
        //    Deployment.Current.Dispatcher.BeginInvoke(() =>
        //    {
        //        hand.HandTimer = "";
        //        timerCount = 11;
        //    });
        //}

        private void DealCards(object sender, DoWorkEventArgs e)
        {
            //Deck.CreateDeck(4);
            //Deck.StackDeck();
            //Deck.ShuffleDeck();
            Deployment.Current.Dispatcher.BeginInvoke(dealer.ClearDealer);
            Thread.Sleep(800);
            for (int i = 0; i < 2; i++)
            {
                foreach (var player in players)
                {
                    if (player.PlayerInPlay)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(PlayDealSound); 
                        player.HandCollection[0].AddCard();
                        Deployment.Current.Dispatcher.BeginInvoke(player.HandCollection[0].CalculateTotal);
                        Thread.Sleep(500);
                        if (player.HandCollection[0].CardCollection.Count == 2)
                        {
                            if (player.HandCollection[0].HandTotal == 21)
                            {
                                Deployment.Current.Dispatcher.BeginInvoke(PlayWinSound);
                                player.HandCollection[0].HandWin(1.5, player.PlayerName);
                                Thread.Sleep(500);
                                player.PlayerInPlay = false;
                            }
                            else if (player.HandCollection[0].HandTotal > 21)
                            {
                                foreach (CardViewModel card in player.HandCollection[0].CardCollection)
                                {
                                    if (card.CardValue == 11)
                                    {
                                        card.CardValue = 1;
                                        Deployment.Current.Dispatcher.BeginInvoke(player.HandCollection[0].CalculateTotal);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (dealer.Hand.Count == 0)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(PlayDealSound); 
                    dealer.AddCard(false);
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(PlayDealSound); 
                    dealer.AddCard(true);
                }
                Deployment.Current.Dispatcher.BeginInvoke(dealer.CalculateTotal);
                Thread.Sleep(500);
            }
        }

        private void StandCommand_OnExecuted(object sender, ExecutedEventArgs e)
        {
            //StopTimer();
            ServiceLocator.Get<ClientComms>().Stand(players.Where(p => p.ActivePlayer).First().PlayerId);
            Stand();
        }

        public void Stand()
        {
            players.Where(p => p.ActivePlayer).First().SetNextActiveHand();
            if (!players.Where(p => p.ActivePlayer).Any())
            {
                DealerPlay();
            }
            CanPlay();
        }

        private void HitCommand_OnExecuted(object sender, ExecutedEventArgs e)
        {
            PlayerViewModel player = players.Where(p => p.ActivePlayer).First();
            HandViewModel hand = player.HandCollection.Where(h => h.ActiveHand).First();
            ServiceLocator.Get<ClientComms>().Hit(player.PlayerId, hand.HandId);
            Hit();
        }

        public void Hit()
        {
            PlayerViewModel player = players.Where(p => p.ActivePlayer).First();
            HandViewModel hand = player.HandCollection.Where(h => h.ActiveHand).First();
            Deployment.Current.Dispatcher.BeginInvoke(PlayDealSound);
            hand.AddCard();
            hand.CalculateTotal();
            if (hand.HandTotal > 21)
            {
                if (hand.CardCollection.Where(c => c.CardValue == 11).Any())
                {
                    hand.CardCollection.Where(c => c.CardValue == 11).First().CardValue = 1;
                    hand.CalculateTotal();
                }
                if (hand.HandTotal > 21)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(PlayLoseSound);
                    hand.HandLose(player.PlayerName);
                    player.SetNextActiveHand();
                    if (!players.Where(p => p.ActivePlayer).Any())
                        DealerPlay();
                    CanPlay();
                }
            }
        }

        private void DoubleCommand_OnExecuted(object sender, ExecutedEventArgs e)
        {
            //StopTimer();
            PlayerViewModel player = players.Where(p => p.ActivePlayer).First();
            HandViewModel hand = player.HandCollection.Where(h => h.ActiveHand).First();
            ServiceLocator.Get<ClientComms>().Double(player.PlayerId, hand.HandId);
            DoubleDown();
        }

        public void DoubleDown()
        {
            PlayerViewModel player =
                players.Where(p => p.ActivePlayer).First();
            HandViewModel hand = player.HandCollection.Where(h => h.ActiveHand).First();
            Deployment.Current.Dispatcher.BeginInvoke(PlayWinSound);
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(hand.DoubleChips);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(FinishedDouble);
            bw.RunWorkerAsync();
        }

        private void FinishedDouble(object sender, RunWorkerCompletedEventArgs e)
        {
            PlayerViewModel player =
                players.Where(p => p.ActivePlayer).First();
            HandViewModel hand = player.HandCollection.Where(h => h.ActiveHand).First();
            Deployment.Current.Dispatcher.BeginInvoke(PlayDealSound);
            hand.AddCard();
            hand.CalculateTotal();
            if (hand.HandTotal > 21)
            {
                if (hand.CardCollection.Where(c => c.CardValue == 11).Any())
                {
                    hand.CardCollection.Where(c => c.CardValue == 11).First().CardValue = 1;
                    hand.CalculateTotal();
                }
            }
            player.SetNextActiveHand();
            if (!players.Where(p => p.ActivePlayer).Any())
                DealerPlay();
            CanPlay();
        }

        private void SplitCommand_OnExecuted(object sender, ExecutedEventArgs e)
        {
            //StopTimer();
            PlayerViewModel player = players.Where(p => p.ActivePlayer).First();
            HandViewModel hand = player.HandCollection.Where(h => h.ActiveHand).First();
            ServiceLocator.Get<ClientComms>().Split(player.PlayerId, hand.HandId);
            Split();
        }

        public void Split()
        {
            PlayerViewModel player = players.Where(p => p.ActivePlayer == true).First();
            player.HandCollection.Add(new HandViewModel(11, 0, 0, 2));
            //player.HandCollection[0].HandXPos = -50;
            //player.HandCollection[0].CardCollection[1].CardDealt = false;
            player.HandCollection[0].CardCollection[1].IsVisible = 0;
            player.HandCollection[0].CardCollection[1].CardBackImage =
                player.HandCollection[0].CardCollection[1].CardImage;
            player.HandCollection[1].CardCollection.Add(player.HandCollection[0].CardCollection[1]);

            player.HandCollection[1].CardCollection[0].CardXPos = 0;
            player.HandCollection[0].CardCollection.RemoveAt(1);

            player.HandCollection[0].IsSplitLeft = true;
            player.HandCollection[1].IsSplitRight = true;
            player.HandCollection[0].ActiveHand = false;
            //player.IsSplitLeftPlayer = true;
            //player.HandCollection[0].IsSplitLeft = true;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(SplitCards);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(FinishedSplit);
            bw.RunWorkerAsync(); 
        }

        private void FinishedSplit(object sender, RunWorkerCompletedEventArgs e)
        {
            PlayerViewModel player =
                players.Where(p => p.ActivePlayer).First();
            //HandViewModel hand = player.HandCollection.Where(h => h.ActiveHand).First();
            //Deployment.Current.Dispatcher.BeginInvoke(PlayDealSound);
            //hand.AddCard();
            //hand.CalculateTotal();
            player.SetNextActiveHand();
            if (!players.Where(p => p.ActivePlayer).Any())
                DealerPlay();
            CanPlay();
            //SetTimer();
        }

        private void SplitCards(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(500);
            PlayerViewModel player = players.Where(p => p.ActivePlayer == true).First();
            //HandViewModel activeHand = player.HandCollection.Where(h => h.ActiveHand).First();
            Deployment.Current.Dispatcher.BeginInvoke(PlayWinSound);
            player.HandCollection[1].SplitChips(player.HandCollection[0]);
            Thread.Sleep(500);
            foreach (var hand in player.HandCollection)
            {
                Deployment.Current.Dispatcher.BeginInvoke(PlayDealSound);
                hand.AddCard();
                Deployment.Current.Dispatcher.BeginInvoke(hand.CalculateTotal);
                Thread.Sleep(500);
                if (hand.HandTotal == 21)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(PlayWinSound);
                    hand.HandWin(1.5, player.PlayerName);
                    Thread.Sleep(500);
                    //player.SetNextActiveHand();
                }
                else if (hand.HandTotal > 21)
                {
                    foreach (var card in hand.CardCollection)
                    {
                        if (card.CardValue == 11)
                        {
                            card.CardValue = 1;
                            Deployment.Current.Dispatcher.BeginInvoke(hand.CalculateTotal);
                            break;
                        }
                    }
                }
            }
        }

        

        private void DealerPlay()
        {
            dealer.TurnCard();
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(DealerCards);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DealerFinished);
            bw.RunWorkerAsync(); 
        }

        private void DealerFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker bwt = new BackgroundWorker();
            bwt.DoWork += new DoWorkEventHandler(WinLose);
            bwt.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WinLoseFinished);
            bwt.RunWorkerAsync(); 
        }

        private void WinLoseFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            ServiceLocator.Get<PlayerCollectionViewModel>().SetAllHandsInPlay();
            gameFinished = true;
            ServiceLocator.Get<PlayerCollectionViewModel>().SetActivePlayer(players.Where(p => p.PlayerName == ServiceLocator.Get<LoginViewModel>().Nickname).ToList()[0]);
            //StopTimer();
            CanBet(true);
        }

        private void WinLose(object sender, DoWorkEventArgs e)
        {
            if(players.Where(p => p.PlayerInPlay).Any())
                foreach (var player in players.Where(p => p.PlayerInPlay))
                {
                    if (player.HandCollection.Where(h => h.HandInPlay).Any())
                        foreach (var hand in player.HandCollection.Where(h => h.HandInPlay))
                        {
                            if (dealer.DealerHandTotal > 21)
                            {
                                Deployment.Current.Dispatcher.BeginInvoke(PlayWinSound);
                                hand.HandWin(1, player.PlayerName);
                                Thread.Sleep(500);
                            }
                            else
                            {
                                if (dealer.DealerHandTotal > hand.HandTotal)
                                {
                                    Deployment.Current.Dispatcher.BeginInvoke(PlayLoseSound);
                                    hand.HandLose(player.PlayerName);
                                    Thread.Sleep(500);
                                }
                                else if (dealer.DealerHandTotal == hand.HandTotal)
                                {
                                    hand.HandPush(player.PlayerName);
                                    Thread.Sleep(500);
                                }
                                else
                                {
                                    Deployment.Current.Dispatcher.BeginInvoke(PlayWinSound);
                                    hand.HandWin(1, player.PlayerName);
                                    Thread.Sleep(500);
                                }
                            }
                        }   
                }
        }

        private void DealerCards(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(800);
            do
            {
                if (dealer.DealerHandTotal >= 17 && dealer.DealerHandTotal <= 21)
                    break;
                if (dealer.DealerHandTotal + Deck[0].CardValue >= 22 && dealer.DealerHandTotal != 21)
                {
                    if (Deck[0].CardValue == 11)
                        Deck[0].CardValue = 1;
                    else
                    {
                        foreach (CardViewModel card in dealer.Hand)
                        {
                            if (card.CardValue == 11)
                            {
                                card.CardValue = 1;
                                Deployment.Current.Dispatcher.BeginInvoke(dealer.CalculateTotal);
                                break;
                            }
                        }
                    }
                }
                Deployment.Current.Dispatcher.BeginInvoke(PlayDealSound);
                dealer.AddCard(true);
                Deployment.Current.Dispatcher.BeginInvoke(dealer.CalculateTotal);
                Thread.Sleep(500);
            } while (dealer.DealerHandTotal <= 21);
        }

        private void PlayDealSound()
        {
            playSound.SetSource(
                Application.GetResourceStream(new Uri("/BlackJackSL;component/Sounds/dealcard.mp3",
                                                      UriKind.RelativeOrAbsolute)).Stream);
        }
        public void PlayWinSound()
        {
            playSound.SetSource(
                Application.GetResourceStream(new Uri("/BlackJackSL;component/Sounds/winchips.mp3",
                                                      UriKind.RelativeOrAbsolute)).Stream);
        }
        private void PlayLoseSound()
        {
            playSound.SetSource(
                Application.GetResourceStream(new Uri("/BlackJackSL;component/Sounds/losechips.mp3",
                                                      UriKind.RelativeOrAbsolute)).Stream);
        }

        public void LoadXml(XDocument xDoc)
        {
            PlayerViewModel activePlayer = new PlayerViewModel();
            PlayerCollectionViewModel pcvm = ServiceLocator.Get<PlayerCollectionViewModel>();
            DealerViewModel dvm = ServiceLocator.Get<DealerViewModel>();
            List<PlayerViewModel> currentPlayers = (from player in xDoc.Descendants("Player")
                                                    select new PlayerViewModel()
                                                               {
                                                                   PlayerId =
                                                                       int.Parse((string) player.Attribute("PlayerId")),
                                                                   PlayerName = (string) player.Attribute("PlayerName"),
                                                                   ActivePlayer = bool.Parse((string) player.Attribute("PlayerActive"))
                                                               }).ToList();

            foreach (var player in currentPlayers)
            {
                pcvm.AddPlayer(player.PlayerId, player.PlayerName, player.ActivePlayer, false);
                //pcvm.PlayerCollection.Add(player);
                List<HandViewModel> currentHands = (from hand in xDoc.Descendants("Hand")
                                                    where (string)hand.Parent.Parent.Attribute("PlayerId") ==
                                                           player.PlayerId.ToString()
                                                    select new HandViewModel()
                                                               {
                                                                   HandId = int.Parse((string) hand.Attribute("HandId")),
                                                                   ActiveHand = bool.Parse((string)hand.Attribute("HandActive")),
                                                                   HandInPlay = bool.Parse((string)hand.Attribute("HandInPlay"))
                                                               }).ToList();

                foreach (var hand in currentHands)
                {

                    if (currentHands.Count > 1)
                    {
                        if(hand.HandId == 1)
                        {
                            hand.HandXPos = -47;
                            hand.HandYPos = 0;
                            hand.HandZPos = 1;
                        }
                        else if(hand.HandId == 2)
                        {
                            hand.HandXPos = 47;
                            hand.HandYPos = 0;
                            hand.HandZPos = 2;
                        }
                    }
                    else
                    {
                        hand.HandXPos = 0;
                        hand.HandYPos = 0;
                        hand.HandZPos = 1;
                    }
                    
                    List<CardViewModel> playerCards = (from card in xDoc.Descendants("Card")
                                                       where
                                                           (string) card.Parent.Parent.Attribute("HandId") ==
                                                           hand.HandId.ToString() && (string)card.Parent.Parent.Parent.Parent.Attribute("PlayerId") ==
                                                           player.PlayerId.ToString() 
                                                       select new CardViewModel()
                                                                  {
                                                                      Rank = (string) card.Attribute("CardRank"),
                                                                      Suit = (string) card.Attribute("CardSuit"),
                                                                      CardImage =
                                                                          @"/BlackJackSL;component/Images/" +
                                                                          (string) card.Attribute("CardSuit") +
                                                                          (string) card.Attribute("CardRank") +
                                                                          ".png",
                                                                      CardBackImage =
                                                                          @"/BlackJackSL;component/Images/backside1.png",
                                                                      IsVisible = 1,
                                                                      CardValue =
                                                                          int.Parse((string) card.Attribute("CardValue"))
                                                                  }).ToList();
                    int count = 0;
                    foreach (var card in playerCards)
                    {
                        count++;
                        card.CardXPos = count*11;
                        hand.CardCollection.Add(card);
                        //pcvm.PlayerCollection.Where(p => p.PlayerId == player.PlayerId).First().HandCollection[0].
                        //    CardCollection.Add(card);
                    }
                    hand.CalculateTotal();

                    List<ChipViewModel> playerChips = (from chip in xDoc.Descendants("Chip")
                                                       where
                                                           (string)chip.Parent.Parent.Parent.Parent.Attribute("PlayerId") ==
                                                           player.PlayerId.ToString() && (string)chip.Parent.Parent.Attribute("HandId") ==
                                                           hand.HandId.ToString()
                                                       select new ChipViewModel()
                                                                  {
                                                                      ChipValue =
                                                                          int.Parse(
                                                                          (string) chip.Attribute("ChipAmount")),
                                                                      ChipImage =
                                                                          @"../../Images/Chip" +
                                                                          (string) chip.Attribute("ChipAmount") +
                                                                          "3dsmall.png"
                                                                  }).ToList();

                    foreach (var chip in playerChips)
                    {
                        int yPos = 0;
                        int addYPos = 0;
                        int xPos = int.Parse(chip.ChipValue.ToString())/2;
                        int zPos = 0;
                        switch (int.Parse(chip.ChipValue.ToString()))
                        {
                            case 10:
                                xPos = 15;
                                addYPos = -5;
                                zPos = 1;
                                break;
                            case 20:
                                xPos = 0;
                                addYPos = 0;
                                zPos = 2;
                                break;
                            case 50:
                                xPos = 30;
                                addYPos = 0;
                                zPos = 3;
                                break;
                            case 100:
                                xPos = 15;
                                addYPos = 5;
                                zPos = 4;
                                break;
                        }
                        if (
                            !hand.ChipCollection.Where(v => v.ChipValue == chip.ChipValue).Any())
                            yPos = 0 + addYPos;
                        else
                        {
                            List<ChipViewModel> tempChipCollection =
                                hand.ChipCollection.Where(v => v.ChipValue == chip.ChipValue).ToList();
                            yPos = (tempChipCollection.Count*-3) + addYPos;
                        }
                        chip.ChipXPos = xPos;
                        chip.ChipYPos = yPos;
                        chip.ChipZPos = zPos;
                        hand.ChipCollection.Add(chip);
                        hand.HandPot += chip.ChipValue;
                        //pcvm.PlayerCollection.Where(p => p.PlayerId == player.PlayerId).First().HandCollection[0].
                        //    ChipCollection.Add(chip);
                    }
                    pcvm.PlayerCollection.Where(p => p.PlayerId == player.PlayerId).First().HandCollection.Add(hand);
                }
                //if(player.ActivePlayer == true)
                //    activePlayer = player;
            }
            List<CardViewModel> dealerCard = (from card in xDoc.Descendants("Card")
                                              where card.Parent.Name == "DealerCards"
                                              select new CardViewModel()
                                                         {
                                                             Rank = (string) card.Attribute("CardRank"),
                                                             Suit = (string) card.Attribute("CardSuit"),
                                                             CardImage =
                                                                 @"/BlackJackSL;component/Images/" +
                                                                 (string) card.Attribute("CardSuit") +
                                                                 (string) card.Attribute("CardRank") +
                                                                 ".png",
                                                             CardBackImage =
                                                                 @"/BlackJackSL;component/Images/backside1.png",
                                                             IsVisible = 1,
                                                             CardValue = int.Parse((string) card.Attribute("CardValue"))
                                                         }).ToList();
            int cardCounter = 0;
            foreach (var card in dealerCard)
            {
                cardCounter++;
                card.CardXPos = cardCounter*11;
                if (cardCounter == 1)
                    card.IsVisible = 0;
                dvm.Hand.Add(card);
            }
            dealer.CalculateTotal();
            foreach (var player in players)
            {
                if (player.PlayerId == 1)
                    ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer1ButtonContent = "Remove";

                if (player.PlayerId == 2)
                    ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer2ButtonContent = "Remove";

                if (player.PlayerId == 3)
                    ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer3ButtonContent = "Remove";

                if (player.PlayerId == 4)
                    ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer4ButtonContent = "Remove";

                if (player.PlayerId == 5)
                    ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer5ButtonContent = "Remove";
            }
            SetJoinButtons();
            //if(pcvm.PlayerCollection.Where(p => p.ActivePlayer).Any())
            //    pcvm.SetActivePlayer(pcvm.PlayerCollection.Where(p => p.ActivePlayer).First());
        }

        private void SetJoinButtons()
        {
            foreach (var player in players)
            {
                if (player.PlayerName == ServiceLocator.Get<LoginViewModel>().Nickname)
                {
                    if (player.PlayerId == 1)
                        ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer1 = true;

                    if (player.PlayerId == 2)
                        ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer2 = true;

                    if (player.PlayerId == 3)
                        ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer3 = true;

                    if (player.PlayerId == 4)
                        ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer4 = true;

                    if (player.PlayerId == 5)
                        ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer5 = true;
                }
                else
                {
                    if (player.PlayerId == 1)
                        ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer1 = false;

                    if (player.PlayerId == 2)
                        ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer2 = false;

                    if (player.PlayerId == 3)
                        ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer3 = false;

                    if (player.PlayerId == 4)
                        ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer4 = false;

                    if (player.PlayerId == 5)
                        ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer5 = false;
                }
            }
            if (ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer1ButtonContent == "Join")
            {
                ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer1 = true;
            }
            if (ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer2ButtonContent == "Join")
            {
                ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer2 = true;
            }
            if (ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer3ButtonContent == "Join")
            {
                ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer3 = true;
            }
            if (ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer4ButtonContent == "Join")
            {
                ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer4 = true;
            }
            if (ServiceLocator.Get<PlayerCollectionViewModel>().AddPlayer5ButtonContent == "Join")
            {
                ServiceLocator.Get<PlayerCollectionViewModel>().CanAddPlayer5 = true;
            }
        }
    }
}
