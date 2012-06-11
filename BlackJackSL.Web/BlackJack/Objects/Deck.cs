using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BlackJackSL.Web.BlackJack.Objects
{
    public class Deck : List<CardViewModel>
    {
        private Object _lock = new object();
        public Deck()
        {
            CreateDeck(4);
            //StackDeck();
            ShuffleDeck();
        }

        public void StackDeck()
        {
            for (int i = 0; i < 100; i++)
            {
                Add(new CardViewModel
                        {
                            Rank = CardViewModel.CardRank.Ace.ToString(),
                            Suit = CardViewModel.CardSuit.Heart.ToString(),
                            CardImage = @"/BlackJackSL;component/Images/" + CardViewModel.CardSuit.Heart.ToString() + CardViewModel.CardRank.Ace.ToString() + ".png",
                            CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                            IsVisible = 0,
                            CardValue = 11
                        });
                Add(new CardViewModel
                        {
                            Rank = CardViewModel.CardRank.King.ToString(),
                            Suit = CardViewModel.CardSuit.Heart.ToString(),
                            CardImage = @"/BlackJackSL;component/Images/" + CardViewModel.CardSuit.Heart.ToString() + CardViewModel.CardRank.King.ToString() + ".png",
                            CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                            IsVisible = 0,
                            CardValue = 10
                        });
                Add(new CardViewModel
                        {
                            Rank = CardViewModel.CardRank.Queen.ToString(),
                            Suit = CardViewModel.CardSuit.Heart.ToString(),
                            CardImage = @"/BlackJackSL;component/Images/" + CardViewModel.CardSuit.Heart.ToString() + CardViewModel.CardRank.Queen.ToString() + ".png",
                            CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                            IsVisible = 0,
                            CardValue = 10
                        });
                Add(new CardViewModel
                        {
                            Rank = CardViewModel.CardRank.Jack.ToString(),
                            Suit = CardViewModel.CardSuit.Heart.ToString(),
                            CardImage = @"/BlackJackSL;component/Images/" + CardViewModel.CardSuit.Heart.ToString() + CardViewModel.CardRank.Jack.ToString() + ".png",
                            CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                            IsVisible = 0,
                            CardValue = 10
                        });
            }
        }

        public void CreateDeck(int deckCount)
        {
            lock (_lock)
            {
                this.Clear();
                for (int i = 0; i < deckCount; i++)
                {
                    List<object> cardSuits = EnumHelper.GetValues(typeof (CardViewModel.CardSuit)).ToList();
                    List<object> cardRanks = EnumHelper.GetValues(typeof (CardViewModel.CardRank)).ToList();
                    foreach (CardViewModel.CardSuit suit in cardSuits)
                        foreach (CardViewModel.CardRank rank in cardRanks)
                        {
                            switch (rank)
                            {
                                case CardViewModel.CardRank.Ace:
                                    Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 11
                                            });
                                    break;
                                case CardViewModel.CardRank.Two:
                                    Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 2
                                            });
                                    break;
                                case CardViewModel.CardRank.Three:
                                    Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 3
                                            });
                                    break;
                                case CardViewModel.CardRank.Four:
                                    Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 4
                                            });
                                    break;
                                case CardViewModel.CardRank.Five:
                                    Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 5
                                            });
                                    break;
                                case CardViewModel.CardRank.Six:
                                    Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 6
                                            });
                                    break;
                                case CardViewModel.CardRank.Seven:
                                    Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 7
                                            });
                                    break;
                                case CardViewModel.CardRank.Eight:
                                    Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 8
                                            });
                                    break;
                                case CardViewModel.CardRank.Nine:
                                    Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 9
                                            });
                                    break;
                                case CardViewModel.CardRank.Jack:
                                    Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 10
                                            });
                                    break;
                                case CardViewModel.CardRank.Queen:
                                    Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 10
                                            });
                                    break;
                                case CardViewModel.CardRank.King:
                                    Add(new CardViewModel
                                            {
                                                Rank = rank.ToString(),
                                                Suit = suit.ToString(),
                                                CardImage =
                                                    @"/BlackJackSL;component/Images/" + suit.ToString() + rank.ToString() +
                                                    ".png",
                                                CardBackImage = @"/BlackJackSL;component/Images/backside1.png",
                                                IsVisible = 0,
                                                CardValue = 10
                                            });
                                    break;
                            }

                        }
                }
            }
        }

        public void ShuffleDeck()
        {
            lock (_lock)
            {
                var randomList = new List<CardViewModel>();
                var r = new Random();
                while (Count > 0)
                {
                    int randomIndex = r.Next(0, Count);
                    randomList.Add(this[randomIndex]);
                    RemoveAt(randomIndex);
                }
                Clear();
                while (randomList.Count > 0)
                {
                    int index = r.Next(0, randomList.Count);
                    Add(randomList[index]);
                    randomList.RemoveAt(index);
                }
            }
        }
    }
}