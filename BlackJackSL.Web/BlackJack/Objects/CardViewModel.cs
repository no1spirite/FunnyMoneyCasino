
namespace BlackJackSL.Web.BlackJack.Objects
{
    public class CardViewModel
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
            }
        }


        private int _cardXPos;
        public int CardXPos
        {
            get { return _cardXPos; }
            set
            {
                _cardXPos = value;
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