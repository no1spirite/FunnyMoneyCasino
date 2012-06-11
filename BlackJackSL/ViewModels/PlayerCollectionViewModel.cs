using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Windows;
using BlackJackSL.BaseClasses;
using BlackJackSL.BlackJackService;
using BlackJackSL.Code;
using BlackJackSL.SLExtensions.Input;
using BlackJackSL.ViewModels;
using System.Threading;
using BlackJackSL.Views;

namespace BlackJackSL.ViewModels
{
    public class PlayerCollectionViewModel : ViewModelBase
    {
        private int yScreenRes = 50;
        public bool loggedIn = false;
        public string Player;
        public PlayerCollectionViewModel()
        {
            PlayerCollection = new ObservableCollection<PlayerViewModel>();
            CanAddPlayer1 = true;
            CanAddPlayer2 = true;
            CanAddPlayer3 = true;
            CanAddPlayer4 = true;
            CanAddPlayer5 = true;
            Commands.AddPlayer1Command.CanExecute += (sender, e) => e.CanExecute = CanAddPlayer1;
            Commands.AddPlayer1Command.Executed += AddPlayerCommand_OnExecuted;
            Commands.AddPlayer2Command.CanExecute += (sender, e) => e.CanExecute = CanAddPlayer2;
            Commands.AddPlayer2Command.Executed += AddPlayerCommand_OnExecuted;
            Commands.AddPlayer3Command.CanExecute += (sender, e) => e.CanExecute = CanAddPlayer3;
            Commands.AddPlayer3Command.Executed += AddPlayerCommand_OnExecuted;
            Commands.AddPlayer4Command.CanExecute += (sender, e) => e.CanExecute = CanAddPlayer4;
            Commands.AddPlayer4Command.Executed += AddPlayerCommand_OnExecuted;
            Commands.AddPlayer5Command.CanExecute += (sender, e) => e.CanExecute = CanAddPlayer5;
            Commands.AddPlayer5Command.Executed += AddPlayerCommand_OnExecuted;
            AddPlayer1ButtonContent = "Join";
            AddPlayer2ButtonContent = "Join";
            AddPlayer3ButtonContent = "Join";
            AddPlayer4ButtonContent = "Join";
            AddPlayer5ButtonContent = "Join";
            PlayerBorderPosX = 0;
            PlayerBorderPosY = 0;
        }

        private int _playerBorderPosX = 0;
        public int PlayerBorderPosX
        {
            get { return _playerBorderPosX; }
            set
            {
                _playerBorderPosX = value;
                OnPropertyChanged("PlayerBorderPosX");
            }
        }

        private int _playerBorderPosY = 0;
        public int PlayerBorderPosY
        {
            get { return _playerBorderPosY; }
            set
            {
                _playerBorderPosY = value;
                OnPropertyChanged("PlayerBorderPosY");
            }
        }

        private List<PlayerViewModel> playerPlayerCollection = new List<PlayerViewModel>();
        public PlayerViewModel AddPlayer(int playerId, string playerName, bool activePlayer, bool newPlayer)
        {
            PlayerViewModel player = new PlayerViewModel(playerName, playerId, activePlayer, newPlayer);
            
            //yPos =- yScreenRes;
            ServiceLocator.Get<TableViewModel>().CanBet(true);
            if (PlayerCollection.Count == 0)
            {
                PlayerCollection.Add(player);
                if (playerName == ServiceLocator.Get<LoginViewModel>().Nickname)
                {
                    playerPlayerCollection.Add(player);
                }
            }
            else if (playerId == 1)
            {
                PlayerCollection.Insert(0, player);
                if (playerName == ServiceLocator.Get<LoginViewModel>().Nickname)
                {
                    playerPlayerCollection.Insert(0, player);
                }
            }
            else if (playerId == 5)
            {
                PlayerCollection.Add(player);
                if (playerName == ServiceLocator.Get<LoginViewModel>().Nickname)
                {
                    playerPlayerCollection.Add(player);
                }
            }
            else if (PlayerCollection.Count == 1)
            {
                if (playerId > PlayerCollection[0].PlayerId)
                {
                    PlayerCollection.Add(player);
                    if (playerName == ServiceLocator.Get<LoginViewModel>().Nickname)
                    {
                        playerPlayerCollection.Add(player);
                    }
                }
                else
                {
                    PlayerCollection.Insert(0, player);
                    if (playerName == ServiceLocator.Get<LoginViewModel>().Nickname)
                    {
                        playerPlayerCollection.Insert(0, player);
                    }
                }
            }
            else
            {
                bool playerAdded = false;
                for (int i = 0; i < PlayerCollection.Count; i++)
                {
                    if (playerId < PlayerCollection[i].PlayerId)
                    {
                        PlayerCollection.Insert(i, player);
                        if (playerName == ServiceLocator.Get<LoginViewModel>().Nickname)
                        {
                            if (playerPlayerCollection.Count == 0)
                                playerPlayerCollection.Add(player);
                            else
                            {
                                for (int x = 0; x < playerPlayerCollection.Count; x++)
                                {
                                    if (playerId < playerPlayerCollection[x].PlayerId)
                                    {
                                        playerPlayerCollection.Insert(x, player);
                                        break;
                                    }
                                }
                            }
                        }
                        playerAdded = true;
                        break;
                    }
                }
                if (!playerAdded)
                {
                    PlayerCollection.Add(player);
                    if (playerName == ServiceLocator.Get<LoginViewModel>().Nickname)
                    {
                        playerPlayerCollection.Add(player);
                    }
                }
            }
            if (playerName == ServiceLocator.Get<LoginViewModel>().Nickname)
            {
                SetActivePlayer(PlayerCollection.Where(p => p.PlayerId == playerPlayerCollection[0].PlayerId).First());
            }
            return player;
        }

        public void SetActivePlayer(PlayerViewModel player)
        {
            if (!PlayerCollection[PlayerCollection.IndexOf(player)].ActivePlayer)
            {
                if (player.PlayerInPlay)
                {
                    if (PlayerCollection.Where(p => p.ActivePlayer).Any())
                        PlayerCollection.Where(p => p.ActivePlayer).First().ActivePlayer = false;
                    player.ActivePlayer = true;
                    if (player.HandCollection[0].CardCollection.Count != 0)
                        player.SetNextActiveHand();
                }
            }
        }

        public void SetNextActivePlayer()
        {
            List<PlayerViewModel> players;
            if(ServiceLocator.Get<TableViewModel>().betting)
            {
                players =
                    PlayerCollection.Where(p => p.PlayerName == ServiceLocator.Get<LoginViewModel>().Nickname).ToList();
            }
            else
            {
                players = PlayerCollection.ToList();
            }
            if (!players.Where(p => p.ActivePlayer).Any())
            {
                if (players.Where(p => p.PlayerInPlay).Any())
                {
                    players.Where(p => p.PlayerInPlay).ToList()[0].ActivePlayer = true;
                    players.Where(p => p.PlayerInPlay).ToList()[0].SetNextActiveHand();
                }
            }
            else
            {
                int indexOfCurrentActivePlayer =
                    players.IndexOf(players.Where(p => p.ActivePlayer).First());
                foreach (var player in players)
                {
                    if (indexOfCurrentActivePlayer == players.Count - 1)
                    {
                        players.Where(p => p.ActivePlayer).First().ActivePlayer = false;
                        //PlayerCollection[indexOfCurrentActivePlayer].ActivePlayer = false;
                        break;
                    }
                    else if (players[indexOfCurrentActivePlayer + 1].PlayerInPlay)
                    {
                        SetActivePlayer(players[indexOfCurrentActivePlayer + 1]);
                        break;
                    }
                    else
                        indexOfCurrentActivePlayer++;
                }
            }
        }

        public void RemovePlayer(int playerId, string playerName)
        {
            PlayerCollection.Remove(PlayerCollection.Where(p => p.PlayerId == playerId).First());
            if (playerName == ServiceLocator.Get<LoginViewModel>().Nickname)
            {
                playerPlayerCollection.Remove(playerPlayerCollection.Where(p => p.PlayerId == playerId).First());
                if(playerPlayerCollection.Count != 0)
                    SetActivePlayer(PlayerCollection.Where(p => p.PlayerId == playerPlayerCollection[0].PlayerId).First());
            }
            if(PlayerCollection.Count == 0)
                ServiceLocator.Get<TableViewModel>().CanBet(false);
        }

        private ObservableCollection<PlayerViewModel> _playerCollection;
        public ObservableCollection<PlayerViewModel> PlayerCollection
        {
            get { return _playerCollection; }
            set
            {
                _playerCollection = value;
                OnPropertyChanged("PlayerCollection");
            }
        }

        private bool canAddPlayer1;
        public bool CanAddPlayer1
        {
            get { return canAddPlayer1; }
            set
            {
                if(canAddPlayer1 == value)
                    return;
                canAddPlayer1 = value;
                OnPropertyChanged("CanAddPlayer1");
            }
        }

        private bool canAddPlayer2;
        public bool CanAddPlayer2
        {
            get { return canAddPlayer2; }
            set
            {
                if (canAddPlayer2 == value)
                    return;
                canAddPlayer2 = value;
                OnPropertyChanged("CanAddPlayer2");
            }
        }

        private bool canAddPlayer3;
        public bool CanAddPlayer3
        {
            get { return canAddPlayer3; }
            set
            {
                if (canAddPlayer3 == value)
                    return;
                canAddPlayer3 = value;
                OnPropertyChanged("CanAddPlayer3");
            }
        }

        private bool canAddPlayer4;
        public bool CanAddPlayer4
        {
            get { return canAddPlayer4; }
            set
            {
                if (canAddPlayer4 == value)
                    return;
                canAddPlayer4 = value;
                OnPropertyChanged("CanAddPlayer4");
            }
        }

        private bool canAddPlayer5;
        public bool CanAddPlayer5
        {
            get { return canAddPlayer5; }
            set
            {
                if (canAddPlayer5 == value)
                    return;
                canAddPlayer5 = value;
                OnPropertyChanged("CanAddPlayer5");
            }
        }

        private string _addPlayer1ButtonContent;
        public string AddPlayer1ButtonContent
        {
            get { return _addPlayer1ButtonContent; }
            set
            {
                _addPlayer1ButtonContent = value;
                OnPropertyChanged("AddPlayer1ButtonContent");
            }
        }

        private string _addPlayer2ButtonContent;
        public string AddPlayer2ButtonContent
        {
            get { return _addPlayer2ButtonContent; }
            set
            {
                _addPlayer2ButtonContent = value;
                OnPropertyChanged("AddPlayer2ButtonContent");
            }
        }

        private string _addPlayer3ButtonContent;
        public string AddPlayer3ButtonContent
        {
            get { return _addPlayer3ButtonContent; }
            set
            {
                _addPlayer3ButtonContent = value;
                OnPropertyChanged("AddPlayer3ButtonContent");
            }
        }
        
        private string _addPlayer4ButtonContent;
        public string AddPlayer4ButtonContent
        {
            get { return _addPlayer4ButtonContent; }
            set
            {
                _addPlayer4ButtonContent = value;
                OnPropertyChanged("AddPlayer4ButtonContent");
            }
        }

        private string _addPlayer5ButtonContent;
        public string AddPlayer5ButtonContent
        {
            get { return _addPlayer5ButtonContent; }
            set
            {
                _addPlayer5ButtonContent = value;
                OnPropertyChanged("AddPlayer5ButtonContent");
            }
        }


        private void AddPlayerCommand_OnExecuted(object sender, ExecutedEventArgs e)
        {
            string name = ServiceLocator.Get<LoginViewModel>().Nickname;
            switch (e.Command.Name)
            {
                case "AddPlayer1Command" :
                    if (AddPlayer1ButtonContent == "Join")
                    {
                        PlayerViewModel player = AddPlayer(1, name, false, true);
                        AddPlayer1ButtonContent = "Remove";
                        ServiceLocator.Get<ClientComms>().AddPlayerToGame(player.PlayerId, player.PlayerInPlay);
                    }
                    else
                    {
                        RemovePlayer(1, name);
                        AddPlayer1ButtonContent = "Join";
                        ServiceLocator.Get<ClientComms>().RemovePlayerFromGame(1);
                    }
                    break;
                case "AddPlayer2Command" :
                    if (AddPlayer2ButtonContent == "Join")
                    {
                        PlayerViewModel player = AddPlayer(2, name, false, true);
                        AddPlayer2ButtonContent = "Remove";
                        ServiceLocator.Get<ClientComms>().AddPlayerToGame(player.PlayerId, player.PlayerInPlay);
                    }
                    else
                    {
                        RemovePlayer(2, name);
                        AddPlayer2ButtonContent = "Join";
                        ServiceLocator.Get<ClientComms>().RemovePlayerFromGame(2);
                    }
                    break;
                case "AddPlayer3Command":
                    if (AddPlayer3ButtonContent == "Join")
                    {
                        PlayerViewModel player = AddPlayer(3, name, false, true);
                        AddPlayer3ButtonContent = "Remove";
                        ServiceLocator.Get<ClientComms>().AddPlayerToGame(player.PlayerId, player.PlayerInPlay);
                    }
                    else
                    {
                        RemovePlayer(3, name);
                        AddPlayer3ButtonContent = "Join";
                        ServiceLocator.Get<ClientComms>().RemovePlayerFromGame(3);
                    }
                    break;
                case "AddPlayer4Command":
                    if (AddPlayer4ButtonContent == "Join")
                    {
                        PlayerViewModel player = AddPlayer(4, name, false, true);
                        AddPlayer4ButtonContent = "Remove";
                        ServiceLocator.Get<ClientComms>().AddPlayerToGame(player.PlayerId, player.PlayerInPlay);
                    }
                    else
                    {
                        RemovePlayer(4, name);
                        AddPlayer4ButtonContent = "Join";
                        ServiceLocator.Get<ClientComms>().RemovePlayerFromGame(4);
                    }
                    break;
                case "AddPlayer5Command":
                    if (AddPlayer5ButtonContent == "Join")
                    {
                        PlayerViewModel player = AddPlayer(5, name, false, true);
                        AddPlayer5ButtonContent = "Remove";
                        ServiceLocator.Get<ClientComms>().AddPlayerToGame(player.PlayerId, player.PlayerInPlay);
                    }
                    else
                    {
                        RemovePlayer(5, name);
                        AddPlayer5ButtonContent = "Join";
                        ServiceLocator.Get<ClientComms>().RemovePlayerFromGame(5);
                    }
                    break;
            }
        }

        public void SetAllHandsInPlay()
        {
            foreach (var player in PlayerCollection)
            {
                player.PlayerInPlay = true;
                foreach (var hand in player.HandCollection)
                {
                    hand.HandInPlay = true;
                }
            }
        }
    }
}