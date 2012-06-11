using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using BlackJackSL.BaseClasses;
using BlackJackSL.Code;
using BlackJackSL.SLExtensions.Input;

namespace BlackJackSL.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        
        public PlayerViewModel()
        {
            HandCollection = new AsyncObservableCollection<HandViewModel>();
            //HandViewModel NewHand = new HandViewModel(0, 0, 1, 1);
            //HandCollection.Add(NewHand);
            //ActivePlayer = false;
            PlayerPlaceHolder = @"/BlackJackSL;component/Images/Player" + PlayerId + "PlaceHolder.png";
            SetPlayerPositions();
            IsInPlay();
            Commands.InsuranceCommand.CanExecute += (sender, e) => e.CanExecute = ActiveInsurance;
            Commands.InsuranceCommand.Executed += InsuranceCommand_OnExecuted;
        }

        public PlayerViewModel(string playerName, int playerId, bool activePlayer, bool newPlayer)
        {
            PlayerName = playerName;
            PlayerId = playerId;
            HandCollection = new AsyncObservableCollection<HandViewModel>();
            if (newPlayer)
            {
                HandViewModel NewHand = new HandViewModel(0, 0, 1, 1);
                HandCollection.Add(NewHand);
            }
            ActivePlayer = activePlayer;
            PlayerPlaceHolder = @"/BlackJackSL;component/Images/Player" + playerId + "PlaceHolder.png";
            SetPlayerPositions();
            IsInPlay();
            Commands.InsuranceCommand.CanExecute += (sender, e) => e.CanExecute = ActiveInsurance;
            Commands.InsuranceCommand.Executed += InsuranceCommand_OnExecuted;
        }

        private void SetPlayerPositions()
        {
            if(PlayerId == 1)
            {
                PlayerXPos = 700;
                PlayerYPos = 0;
                PlaceHolderXPos = 27;
                PlaceHolderYPos = 76;
            }
            else if(PlayerId == 2)
            {
                PlayerXPos = 550;
                PlayerYPos = 115;
                PlaceHolderXPos = 37;
                PlaceHolderYPos = 62;
            }
            else if (PlayerId == 3)
            {
                PlayerXPos = 350;
                PlayerYPos = 150;
                PlaceHolderXPos = 30;
                PlaceHolderYPos = 63;
            }
            else if (PlayerId == 4)
            {
                PlayerXPos = 150;
                PlayerYPos = 115;
                PlaceHolderXPos = -7;
                PlaceHolderYPos = 57;
            }
            else if (PlayerId == 5)
            {
                PlayerXPos = 0;
                PlayerYPos = 0;
                PlaceHolderXPos = -15;
                PlaceHolderYPos = 77;
            }
        }

        private int _playerOpacity;
        public int PlayerOpacity
        {
            get { return _playerOpacity; }
            set
            {
                if(_playerOpacity == value)
                    return;
                _playerOpacity = value;
            }
        }

        private void IsInPlay()
        {
            if (ServiceLocator.Get<PlayerCollectionViewModel>().PlayerCollection.Count != 0)
            {
                if (ServiceLocator.Get<TableViewModel>().betting || ServiceLocator.Get<TableViewModel>().playing || ServiceLocator.Get<TableViewModel>().dealt)
                    PlayerInPlay = false;
                else
                {
                    PlayerInPlay = true;
                }
            }
            else
            {
                PlayerInPlay = true;
                if (PlayerName == ServiceLocator.Get<LoginViewModel>().Nickname)
                    ActivePlayer = true;
            }
        }

        public string PlayerName { get; set; }
        public int PlayerId { get; set; }
        public int PlayerXPos { get; set; }
        public int PlayerYPos { get; set; }
        public bool PlayerInPlay { get; set; }
        public double PlayerBank { get; set; }
        public bool ToBeRemoved { get; set; }
        public string PlayerPlaceHolder { get; set; }
        public int PlaceHolderXPos { get; set; }
        public int PlaceHolderYPos { get; set; }

        private bool _activePlayer;
        public bool ActivePlayer
        {
            get { return _activePlayer; }
            set 
            { 
                if(_activePlayer == value)
                    return;
                _activePlayer = value;
                OnPropertyChanged("ActivePlayer");
            }
        }

        private bool _activeInsurance;
        public bool ActiveInsurance
        {
            get { return _activeInsurance; }
            set
            {
                if (_activeInsurance == value)
                    return;
                _activeInsurance = value;
                OnPropertyChanged("ActiveInsurance");
            }
        }


        private int _insuranceOpacity;
        public int InsuranceOpacity
        {
            get { return _insuranceOpacity; }
            set
            {
                if (_insuranceOpacity == value)
                    return;
                _insuranceOpacity = value;
                OnPropertyChanged("InsuranceOpacity");
            }
        }

        private AsyncObservableCollection<HandViewModel> _handCollection;
        public AsyncObservableCollection<HandViewModel> HandCollection
        {
            get { return _handCollection; }
            set
            {
                _handCollection = value;
                OnPropertyChanged("HandCollection");
            }
        }

        public void SetActiveHand(HandViewModel hand)
        {
            foreach (var player in ServiceLocator.Get<PlayerCollectionViewModel>().PlayerCollection)
            {
                foreach (var h in player.HandCollection)
                {
                    h.ActiveHand = false;
                }
            }
            //if (HandCollection.Where(p => p.ActiveHand).Any())
            //    HandCollection.Where(p => p.ActiveHand).First().ActiveHand = false;
            hand.ActiveHand = true;
        }

        public void SetNextActiveHand()
        {
            bool test = !HandCollection.Where(p => p.ActiveHand).Any();
            
            if (!HandCollection.Where(p => p.ActiveHand).Any())
            {
                if (HandCollection.Where(h => h.HandInPlay).Any())
                {
                    SetActiveHand(HandCollection.Where(h => h.HandInPlay).ToList()[0]);
                }
            }
            else
            {
                int indexOfCurrentActiveHand =
                HandCollection.IndexOf(HandCollection.Where(p => p.ActiveHand).First());
                foreach (var hand in HandCollection)
                {
                    if (indexOfCurrentActiveHand == HandCollection.Count - 1)
                    {
                        HandCollection[indexOfCurrentActiveHand].ActiveHand = false;
                        ServiceLocator.Get<PlayerCollectionViewModel>().SetNextActivePlayer();
                        break;
                    }
                    else if (HandCollection[indexOfCurrentActiveHand + 1].HandInPlay)
                    {
                        SetActiveHand(HandCollection[indexOfCurrentActiveHand + 1]);
                        break;
                    }
                    else
                        indexOfCurrentActiveHand++;
                }
            }
        }

        public void ClearPlayer()
        {
            //foreach (var hand in HandCollection)
            //{
            //    hand.IsSplitLeft = false;
            //    hand.ActiveHand = false;
            //    hand.CardCollection.Clear();
            //    hand.HandTotal = 0;
            //    hand.HandPot = 0;
            //}

            if (!PlayerInPlay)
                PlayerInPlay = true;
            HandCollection.Clear();
            HandViewModel NewHand = new HandViewModel(0, 0, 1, 1);
            HandCollection.Add(NewHand);
        }

        private void InsuranceCommand_OnExecuted(object sender, ExecutedEventArgs e)
        {
            ActiveInsurance = false;
        }
    }
}
