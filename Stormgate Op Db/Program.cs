using System;
using System.Drawing;
using System.Threading;

namespace Stormgate_Op_Db
{
    internal class Program
    {
        private static Opponent _currentOpponent = null;

        static void Main(string[] args)
        {
            bool inGame;
            string myPlayerName = "exhail";
            string loadingScreenRef = "av";
            string endingScreenRef = "overview";

            Rectangle p1ScreenSpace = new Rectangle(450, 520, 300, 30);    
            Rectangle p2ScreenSpace = new Rectangle(1170, 520, 300, 30);    
            Rectangle LoadingScreenSpace = new Rectangle(870, 475, 175, 120);
            Rectangle endGameScreenSpace = new Rectangle(420, 155, 150, 30);

            bool ReadSuccess;
            var screenReader = new ScreenReader();
            var opponentDB = new OpponentDB();
            opponentDB.LoadList();
            
            //screenReader.Save(LoadingScreenSpace); //TESTING

            while (true)
            {
                //Console.WriteLine(screenReader.Read(LoadingScreenSpace)); //TESTING

                if (screenReader.Read(LoadingScreenSpace) == loadingScreenRef)
                {
                    inGame = true;
                    string opponentID;
                    var p1Text = screenReader.Read(p1ScreenSpace);
                    var p2Text = screenReader.Read(p2ScreenSpace);

                    //Console.WriteLine(p1Text); //TESTING 
                    //Console.WriteLine(p2Text); //TESTING

                    if (p1Text == null || p2Text == null)
                    {
                        Console.WriteLine("ERROR: Failed to read");
                        ReadSuccess = false;
                        break;
                    }

                    if (p1Text == myPlayerName)
                    {
                        opponentID = p2Text;
                    }
                    else opponentID = p1Text;

                    if (opponentDB.HistoryCheck(opponentID))
                    {
                        _currentOpponent = opponentDB.LoadOpponent(opponentID);
                        Console.WriteLine("Previous games found, this player is " + _currentOpponent.QPlayerType());
                        ReadSuccess = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No history found, this is a new player");
                        _currentOpponent = new Opponent(opponentID);
                        opponentDB.AddOpponent(_currentOpponent);
                        ReadSuccess = true;
                        break;
                    }
                    //Console.WriteLine("The bad ending"); //TESTING
                }
                Thread.Sleep(3000);
            }

            while (inGame && ReadSuccess)
            {
                Console.WriteLine(screenReader.Read(endGameScreenSpace));

                if (screenReader.Read(endGameScreenSpace) == endingScreenRef)
                {
                    inGame = false;
                    Console.WriteLine("The Game has finished.\nWas this player playing a Cheesey or Macro game?");
                    var userReview = Console.ReadLine();
                    if (userReview == "Macro") _currentOpponent._pastMacroCount++;
                    if (userReview == "Cheesy") _currentOpponent._pastCheeseCount++;
                   
                    opponentDB.AddOpponent(_currentOpponent);
                }
                Thread.Sleep(3000);
            }

            Console.WriteLine("Program done");
            Console.ReadLine();
        }


        public string RaceCheck()
        {
            //Impliment ScreenCapture Logic and stuff
        }
    }
}
