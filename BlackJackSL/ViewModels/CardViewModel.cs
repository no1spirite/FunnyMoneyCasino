using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using BlackJackSL.BaseClasses;

namespace BlackJackSL.ViewModels
{
    public class CardViewModel : ViewModelBase
    {
        public string Rank { get; set; }
        public string Suit { get; set; }
        public string CardBackImage { get; set; }
        public string CardImage { get; set; }
        public int CardValue { get; set; }
        private int _isVisible;
        public int IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        private int _cardOpacity;
        public int CardOpacity
        {
            get { return _cardOpacity; }
            set { _cardOpacity = value; }
        }

        private bool _cardDealt;
        public bool CardDealt
        {
            get { return _cardDealt; }
            set
            {
                _cardDealt = value;
                OnPropertyChanged("CardDealt");
            }
        }


        private int _cardXPos;
        public int CardXPos
        {
            get { return _cardXPos; }
            set
            {
                _cardXPos = value;
                OnPropertyChanged("CardXPos");
            }
        }

        public int CardYPos { get; set; }
        public int CardZPos { get; set; }

        private int _cardRotationY;
        public int CardRotationY
        {
            get { return _cardRotationY; }
            set
            {
                _cardRotationY = value;
                OnPropertyChanged("CardRotationY");
            }
        }

        private string _currentImage;
        public string CurrentImage
        {
            get
            {
                //if (!IsVisible)
                //    return CardBackImage;
                //else
                //    return CardImage;
                return _currentImage;
            }
            set
            {
                _currentImage = value;
                OnPropertyChanged("CurrentImage");
            }
        }

        public enum CardSuit
        {
            Spade,
            Heart,
            Diamond,
            Club
        } ;

        public enum CardRank
        {
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
            Ace
        } ;

        public enum FlipStatus
        {
            FaceUp,
            FaceDown,
            Flip
        } ;
    }
}