using BlackJackSL.SLExtensions.Input;
using SLExtensions.Input;

namespace BlackJackSL
{
    public class Commands
    {
        static Commands()
        {
            HitCommand = new Command("HitCommand");
            StandCommand = new Command("StandCommand");
            DoubleCommand = new Command("DoubleCommand");
            SplitCommand = new Command("SplitCommand");
            InsuranceCommand = new Command("InsuranceCommand");
            Bet10Command = new Command("Bet10Command");
            Bet20Command = new Command("Bet20Command");
            Bet50Command = new Command("Bet50Command");
            Bet100Command = new Command("Bet100Command");
            DoneBettingCommand = new Command("DoneBettingCommand");
            AddPlayer1Command = new Command("AddPlayer1Command");
            AddPlayer2Command = new Command("AddPlayer2Command");
            AddPlayer3Command = new Command("AddPlayer3Command");
            AddPlayer4Command = new Command("AddPlayer4Command");
            AddPlayer5Command = new Command("AddPlayer5Command");
            SendMessageCommand = new Command("SendMessageCommand");
            LoginCommand = new Command("LoginCommand");
        }

        public static Command HitCommand{ get; set; }
        public static Command StandCommand { get; set; }
        public static Command DoubleCommand { get; set; }
        public static Command SplitCommand { get; set; }
        public static Command InsuranceCommand { get; set; }
        public static Command Bet10Command { get; set; }
        public static Command Bet20Command { get; set; }    
        public static Command Bet50Command { get; set; }
        public static Command Bet100Command { get; set; }
        public static Command DoneBettingCommand { get; set; }
        public static Command AddPlayer1Command { get; set; }
        public static Command AddPlayer2Command { get; set; }
        public static Command AddPlayer3Command { get; set; }
        public static Command AddPlayer4Command { get; set; }
        public static Command AddPlayer5Command { get; set; }
        public static Command SendMessageCommand { get; set; }
        public static Command LoginCommand { get; set; }
    }
}