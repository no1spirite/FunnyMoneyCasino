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

namespace BlackJackSL.ViewModels
{
    public class ChipViewModel
    {
        public string ChipImage { get; set; }
        public double ChipValue { get; set; }
        public int ChipXPos { get; set; }
        public int ChipYPos { get; set; }
        public int ChipZPos { get; set; }
    }
}
