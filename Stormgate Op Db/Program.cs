using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Xml;

namespace Stormgate_Op_Db
{
    internal class Program
    {
        private static ScreenAnalyzer _screenAnalyzer = new ScreenAnalyzer();

        private static Opponent _currentOpponent = null;
        private static string _opponentNick;
        private static string _opponentRace;

        private static readonly string _userNick = "Exhail"; //Set UserNick Here.
        private static string _userRace;

        static void Main(string[] args)
        {
            bool inGame;
            bool newOpp;
            bool ReadSuccess;
            var opponentDB = new OpponentDB();
            opponentDB.LoadList();

            while (true)
            {
                if (OnLoadingScreen())
                {
                    inGame = true;
                    Dictionary<string, string> playersInfo = FindPlayersInfo();

                    if (playersInfo["P1Nick"] == _userNick)
                    {
                        _userRace = playersInfo["P1Race"];
                        _opponentNick = playersInfo["P2Nick"];
                        _opponentRace = playersInfo["P2Race"];
                    }
                    if (playersInfo["P2Nick"] == _userNick)
                    {
                        _userRace = playersInfo["P2Race"];
                        _opponentNick = playersInfo["P1Nick"];
                        _opponentRace = playersInfo["P1Race"];
                    }
                    //else throw new Exception("Neither Nicknames matched User's Nickname");

                    if (opponentDB.QConfirmHistory(_opponentNick))
                    {
                        Console.WriteLine("Previous games found. Loading history.");
                        _currentOpponent = opponentDB.LoadOpponent(_opponentNick);

                        Dictionary<string, int> oppBehaviour = _currentOpponent.BehaviourHistory(_opponentRace);
                        int[] oppStats = _currentOpponent.Statistics(_opponentRace);

                        Console.WriteLine($"Total {_opponentRace} Games: {oppStats[0]}");
                        Console.WriteLine($"Total Won {_opponentRace} Games: {oppStats[1]}");
                        Console.WriteLine($"{_opponentRace} Winrate: {oppStats[2]}%");

                        foreach (KeyValuePair<string, int> style in oppBehaviour)
                        {
                            Console.WriteLine($"Previous {style.Key} style games: {style.Value}");
                        }

                        ReadSuccess = true;
                        newOpp = false;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("No history found, this is a new player");
                        _currentOpponent = new Opponent(_opponentNick);
                        opponentDB.AddOpponent(_currentOpponent);
                        ReadSuccess = true;
                        newOpp = true;
                        break;
                    }
                }
                Thread.Sleep(500);
            }

            while (inGame && ReadSuccess)
            {
                if (OnEndScreen())
                {
                    inGame = false;
                    Console.WriteLine("\nThe Game has finished.\nWhat kind of behaviour did they exibit?\n Cheesey, Aggresive, or Defensive?");

                    var userReview = Console.ReadLine().Trim().ToLower();
                    _currentOpponent.InputPlayerstyle(userReview, _opponentRace);

                    opponentDB.AddOpponent(_currentOpponent);
                }
                Thread.Sleep(500);
            }

            Console.WriteLine("Program done");
            Console.ReadLine();
        }


        public static bool OnLoadingScreen()
        {
            string loadingScreenReff = @"C:\Users\ellio\Desktop\C# Learning Fun times\Projects\Stormgate Op Db\Cache\ScreenCapture\Refference Images\LoadingReff.png";
            Rectangle loadingScreenSpace = new Rectangle(870, 475, 175, 120);

            bool output = _screenAnalyzer.QImgMatch(loadingScreenSpace, loadingScreenReff);
            return output;
        }

        public static bool OnEndScreen()
        {
            string endScreenReff = @"C:\Users\ellio\Desktop\C# Learning Fun times\Projects\Stormgate Op Db\Cache\ScreenCapture\Refference Images\EndscreenReff.png";
            Rectangle endGameScreenSpace = new Rectangle(185, 148, 200, 40);

            return _screenAnalyzer.QImgMatch(endGameScreenSpace, endScreenReff);
        }

        public static Dictionary<string, string> FindPlayersInfo()
        {
            var output = new Dictionary<string, string>();
            Rectangle p1PlayerIDScreenSpace = new Rectangle(450, 520, 300, 30);
            Rectangle p2PlayerIDScreenSpace = new Rectangle(1170, 520, 300, 30);
            Rectangle p1RaceScreenspace = new Rectangle(388, 515, 60, 60);
            Rectangle p2RaceScreenspace = new Rectangle(1474, 515, 60, 60);

            output["P1Nick"] = _screenAnalyzer.ReadCS(p1PlayerIDScreenSpace);
            output["P2Nick"] = _screenAnalyzer.ReadCS(p2PlayerIDScreenSpace);
            output["P1Race"] = CheckRace(p1RaceScreenspace);
            output["P2Race"] = CheckRace(p2RaceScreenspace);

            return output;
        }

        public static string CheckRace(Rectangle screenspace)
        {
            var vanReffPath = @"C:\Users\ellio\Desktop\C# Learning Fun times\Projects\Stormgate Op Db\Cache\ScreenCapture\Refference Images\van_reff.png";
            var infReffPath = @"C:\Users\ellio\Desktop\C# Learning Fun times\Projects\Stormgate Op Db\Cache\ScreenCapture\Refference Images\inf_reff.png";
            var acgReffPath = @"C:\Users\ellio\Desktop\C# Learning Fun times\Projects\Stormgate Op Db\Cache\ScreenCapture\Refference Images\acg_reff.png";

            if (_screenAnalyzer.QImgMatch(screenspace, vanReffPath)) return "van";
            if (_screenAnalyzer.QImgMatch(screenspace, infReffPath)) return "inf";
            if (_screenAnalyzer.QImgMatch(screenspace, acgReffPath)) return "acg";
            throw new Exception("P2 Race not found");
        }
    }
}
