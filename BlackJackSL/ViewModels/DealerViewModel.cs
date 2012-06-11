using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BlackJackSL.BaseClasses;
using BlackJackSL.Code;
using BlackJackSL.ViewModels;

namespace BlackJackSL.ViewModels
{
    public class DealerViewModel : ViewModelBase
    {
        public DealerViewModel(AsyncObservableCollection<CardViewModel> hand)
        {
            _hand = hand;
        }

        private ObservableCollection<CardViewModel> _hand;
        public ObservableCollection<CardViewModel> Hand
        {
            get { return _hand; }
            set
            {
                _hand = value;
                OnPropertyChanged("Hand");
            }
        }

        private double _dealerBank;
        public double DealerBank
        {
            get { return _dealerBank; }
            set
            {
                _dealerBank = value;
                OnPropertyChanged("DealerBank");
            }
        }

        private int _dealerHandTotal;
        public int DealerHandTotal
        {
            get
            {
                return _dealerHandTotal;
            }
            set
            {
                _dealerHandTotal = value;
                OnPropertyChanged("DealerHandTotal");
            }
        }

        public void AddCard(bool isVisible)
        {
            ServiceLocator.Get<TableViewModel>().Deck[0].CardXPos = Hand.Count * 11;
            ServiceLocator.Get<TableViewModel>().Deck[0].CardYPos = 0;
            if(!isVisible)
                ServiceLocator.Get<TableViewModel>().Deck[0].IsVisible = 2;
            else
                ServiceLocator.Get<TableViewModel>().Deck[0].IsVisible = 1;

            ServiceLocator.Get<TableViewModel>().Deck[0].CardDealt = true;
            //Hand.Add(new CardViewModel
            //            {
            //                Rank = CardViewModel.CardRank.Ace.ToString(),
            //                Suit = CardViewModel.CardSuit.Heart.ToString(),
            //                CardImage = @"/BlackJackSL;component/Images/heartace.png",
            //                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
            //                IsVisible = true,
            //                CardValue = 11
            //            });
            Hand.Add(ServiceLocator.Get<TableViewModel>().Deck[0]);
            //UIThread.Run(CalculateTotal);
            ServiceLocator.Get<TableViewModel>().Deck.RemoveAt(0);
        }

        public void TurnCard()
        {
            //CardViewModel dealerCard;
            Hand[1].IsVisible = 3;
            Hand[0].IsVisible = 4;
            CalculateTotal();
            //dealerCard = Hand[0];
            //Hand.RemoveAt(0);
            //Hand.Insert(0, dealerCard);
            //UIThread.Run(CalculateTotal);
        }

        public void ClearDealer()
        {
            Hand.Clear();
            DealerHandTotal = 0;
        }

        public void CalculateTotal()
        {
            int total = 0;
            foreach (var card in Hand)
            {
                if (card.IsVisible != 2)
                    total += card.CardValue;
            }
            DealerHandTotal = total;
        }
    }
}