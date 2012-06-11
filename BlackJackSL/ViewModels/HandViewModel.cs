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

namespace BlackJackSL.ViewModels
{
    public class HandViewModel : ViewModelBase
    {
        private Object _lock = new object();
        public HandViewModel()
        {
            CardCollection = new AsyncObservableCollection<CardViewModel>();
            ChipCollection = new AsyncObservableCollection<ChipViewModel>();
            InsuranceChipCollection = new AsyncObservableCollection<ChipViewModel>();
            //ActiveHand = false;
            HandOpacity = 0;
            HandInPlay = true;
        }

        public HandViewModel(int handXPos, int handYPos, int handZPos, int handId)
        {
            HandId = handId;
            HandXPos = handXPos;
            HandYPos = handYPos;
            HandZPos = handZPos;
            CardCollection = new AsyncObservableCollection<CardViewModel>();
            ChipCollection = new AsyncObservableCollection<ChipViewModel>();
            InsuranceChipCollection = new AsyncObservableCollection<ChipViewModel>();
            ActiveHand = false;
            HandOpacity = 0;
            HandInPlay = true;
        }

        public int HandId { get; set; }

        private int _handOpacity;
        public int HandOpacity
        {
            get { return _handOpacity; }
            set
            {
                if (_handOpacity == value)
                    return;
                _handOpacity = value;
            }
        }

        private int _handXPos;
        public int HandXPos
        {
            get { return _handXPos; }
            set
            {
                _handXPos = value;
                OnPropertyChanged("HandXPos");
            }
        }

        public int HandYPos { get; set; }

        private int _handZPos;
        public int HandZPos
        {
            get { return _handZPos; }
            set
            {
                _handZPos = value;
                OnPropertyChanged("HandZPos");
            }
        }

        public bool HandInPlay { get; set; }

        private bool _activeHand;
        public bool ActiveHand
        {
            get { return _activeHand; }
            set
            {
                _activeHand = value;
                OnPropertyChanged("ActiveHand");
            }
        }

        private bool _isSplitRight;
        public bool IsSplitRight
        {
            get { return _isSplitRight; }
            set
            {
                if (_isSplitRight == value)
                    return;
                _isSplitRight = value;
                OnPropertyChanged("IsSplitRight");
            }
        }

        private bool _isSplitLeft;
        public bool IsSplitLeft
        {
            get { return _isSplitLeft; }
            set
            {
                if (_isSplitLeft == value)
                    return;
                _isSplitLeft = value;
                OnPropertyChanged("IsSplitLeft");
            }
        }

        private bool _isCenter;
        public bool IsCenter
        {
            get { return _isCenter; }
            set
            {
                _isCenter = value;
                OnPropertyChanged("IsCenter");
            }
        }

        private AsyncObservableCollection<CardViewModel> _cardCollection;
        public AsyncObservableCollection<CardViewModel> CardCollection
        {
            get { return _cardCollection; }
            set
            {
                _cardCollection = value;
                OnPropertyChanged("CardCollection");
            }
        }

        private AsyncObservableCollection<ChipViewModel> _chipCollection;
        public AsyncObservableCollection<ChipViewModel> ChipCollection
        {
            get { return _chipCollection; }
            set
            {
                _chipCollection = value;
                OnPropertyChanged("ChipCollection");
            }
        }

        private AsyncObservableCollection<ChipViewModel> _insuranceChipCollection;
        public AsyncObservableCollection<ChipViewModel> InsuranceChipCollection
        {
            get { return _insuranceChipCollection; }
            set
            {
                _insuranceChipCollection = value;
                OnPropertyChanged("InsuranceChipCollection");
            }
        }

        private int _handTotal;
        public int HandTotal
        {
            get
            {
                return _handTotal;
            }
            set
            {
                _handTotal = value;
                HandTotalString = _handTotal.ToString();
                OnPropertyChanged("HandTotal");
            }
        }

        private string _handTotalString;
        public string HandTotalString
        {
            get
            {
                return _handTotalString;
            }
            set
            {
                _handTotalString = value;
                OnPropertyChanged("HandTotalString");
            }
        }

        private double _handPot;
        public double HandPot
        {
            get { return _handPot; }
            set
            {
                _handPot = value;
                HandPotString = "£" + _handPot;
                OnPropertyChanged("HandPot");
            }
        }

        private string _handPotString;
        public string HandPotString
        {
            get { return _handPotString; }
            set
            {
                _handPotString = value;
                OnPropertyChanged("HandPotString");
            }
        }

        private double _handInsurancePot;
        public double HandInsurancePot
        {
            get { return _handInsurancePot; }
            set
            {
                _handInsurancePot = value;
                HandInsurancePotString = "£" + _handInsurancePot;
                OnPropertyChanged("HandInsurancePot");
            }
        }

        private string _handInsurancePotString;
        public string HandInsurancePotString
        {
            get { return _handInsurancePotString; }
            set
            {
                _handInsurancePotString = value;
                OnPropertyChanged("HandInsurancePotString");
            }
        }

        private string _handTimer;
        public string HandTimer
        {
            get
            {
                return _handTimer;
            }
            set
            {
                _handTimer = value;
                OnPropertyChanged("HandTimer");
            }
        }

        public void AddChip(double chipValue)
        {
            lock (_lock)
            {
                int yPos = 0;
                int addYPos = 0;
                int xPos = int.Parse(chipValue.ToString())/2;
                int zPos = 0;
                switch (int.Parse(chipValue.ToString()))
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
                if (!ChipCollection.Where(v => v.ChipValue == chipValue).Any())
                    yPos = 0 + addYPos;
                else
                {
                    List<ChipViewModel> tempChipCollection =
                        ChipCollection.Where(v => v.ChipValue == chipValue).ToList();
                    yPos = (tempChipCollection.Count*-3) + addYPos;
                }
                ChipViewModel chip = new ChipViewModel
                                         {
                                             ChipImage =
                                                 @"../../Images/Chip" + int.Parse(chipValue.ToString()).ToString() +
                                                 "3dsmall.png",
                                             ChipXPos = xPos,
                                             ChipYPos = yPos,
                                             ChipZPos = zPos,
                                             ChipValue = chipValue
                                         };
                ChipCollection.Add(chip);
            }
        }

        private void AddChips()
        {
            List<ChipViewModel> winningChips = ChipCollection.ToList();

            foreach (var chip in winningChips)
            {
                AddChip(chip.ChipValue);
                Thread.Sleep(20);
            }
        }

        public void AddCard()
        {
            ServiceLocator.Get<TableViewModel>().Deck[0].CardXPos = CardCollection.Count * 11;
            int test = ServiceLocator.Get<TableViewModel>().Deck[0].CardXPos;
            ServiceLocator.Get<TableViewModel>().Deck[0].CardZPos = CardCollection.Count;
            ServiceLocator.Get<TableViewModel>().Deck[0].CardYPos = 0;
            //ServiceLocator.Get<TableViewModel>().Deck[0].CardDealt = true;
            //ServiceLocator.Get<TableViewModel>().Deck[0].CardRotationY = 0;
            CardCollection.Add(ServiceLocator.Get<TableViewModel>().Deck[0]);
            
            //CalculateTotal();
            //Deployment.Current.Dispatcher.BeginInvoke(CalculateTotal);
            ServiceLocator.Get<TableViewModel>().Deck.RemoveAt(0);
            CardCollection[CardCollection.Count - 1].IsVisible = 1;
        }


        public void CalculateTotal()
        {
            int total = 0;
            foreach (var card in CardCollection)
            {
                if (card.IsVisible == 0 || card.IsVisible == 1)
                    total += card.CardValue;
            }
            HandTotal = total;
        }

        public void HandLose(string playerName)
        {
            int outNum;
            if (int.TryParse(HandTotalString, out outNum))
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                              {
                                                                  HandTotalString = "Lose";
                                                                  ChipCollection.Clear();
                                                                  HandPot = 0;
                                                                  HandInPlay = false;
                                                              });
            }
        }

        public void HandPush(string playerName)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                                                          {
                                                              HandTotalString = "Push";
                                                              HandInPlay = false;
                                                              if(playerName == ServiceLocator.Get<LoginViewModel>().Nickname)
                                                                ServiceLocator.Get<TableViewModel>().OverallPlayerBank += HandPot;
                                                          });
        }

        private double multiplyer = 0;
        public void HandWin(double winningsMultiplyer, string playerName)
        {
            multiplyer = winningsMultiplyer;
            int outNum;
            if (int.TryParse(HandTotalString, out outNum))
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    HandTotalString = "Win";
                    HandInPlay = false;
                    HandPot += HandPot * multiplyer;
                    if (playerName == ServiceLocator.Get<LoginViewModel>().Nickname)
                        ServiceLocator.Get<TableViewModel>().OverallPlayerBank += HandPot;
                });
                Thread addChipsThread = new Thread((AddChips));
                addChipsThread.Start();
            }
        }

        public void DoubleChips(object sender, DoWorkEventArgs e)
        {
            Thread addChipsThread = new Thread((AddChips));
            addChipsThread.Start();
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                HandPot += HandPot;
            });
            Thread.Sleep(500);
        }

        public void SplitChips(HandViewModel hand)
        {
            foreach (var chip in hand.ChipCollection)
            {
                AddChip(chip.ChipValue);
                Thread.Sleep(20);
            }
            Deployment.Current.Dispatcher.BeginInvoke(() => 
                {
                    HandPot = hand.HandPot;
                });
        }

        public void InsuranceChips(object sender, DoWorkEventArgs e)
        {
            double insurancePot = HandPot / 2;
            List<ChipViewModel> tempInsuranceChips = new List<ChipViewModel>();

            double chips = Math.Floor(insurancePot / 100);
            for (int i = 0; i < chips; i++)
                tempInsuranceChips.Add(new ChipViewModel { ChipImage = @"../../Images/Chip1003dsmall.png", ChipYPos = tempInsuranceChips.Count * -3, ChipValue = 100.00 });
            insurancePot = insurancePot % 100;

            chips = Math.Floor(insurancePot / 50);
            for (int i = 0; i < chips; i++)
                tempInsuranceChips.Add(new ChipViewModel { ChipImage = @"../../Images/Chip503dsmall.png", ChipYPos = tempInsuranceChips.Count * -3, ChipValue = 50.00 });
            insurancePot = insurancePot % 50;

            chips = Math.Floor(insurancePot / 20);
            for (int i = 0; i < chips; i++)
                tempInsuranceChips.Add(new ChipViewModel { ChipImage = @"../../Images/Chip203dsmall.png", ChipYPos = tempInsuranceChips.Count * -3, ChipValue = 20.00 });
            insurancePot = insurancePot % 20;

            chips = Math.Floor(insurancePot / 10);
            for (int i = 0; i < chips; i++)
                tempInsuranceChips.Add(new ChipViewModel { ChipImage = @"../../Images/Chip103dsmall.png", ChipYPos = tempInsuranceChips.Count * -3, ChipValue = 10.00 });

            foreach (var chip in tempInsuranceChips)
            {
                InsuranceChipCollection.Add(chip);
                Thread.Sleep(20);
            }

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                HandInsurancePot += HandPot/2;
            });
            
        }
    }
}