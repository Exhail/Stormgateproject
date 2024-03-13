using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics.Eventing.Reader;

namespace Stormgate_Op_Db
{
    public class Opponent
    {
        public string _playerID;

        //All arrays: [0]Vangaurd, [1]Infernal, [2]Anime Cat Girls
        private int[] _totalGames;
        private int[] _winRate;
        private Dictionary<string, int> _infPlaystyle;
        private Dictionary<string, int> _vanPlaystyle;
        private Dictionary<string, int> _acgPlaystyle;

        
        public Opponent(string playerID)
        {
            _playerID = playerID;

            _totalGames = new int[3];
            _winRate = new int[3];

            _infPlaystyle ["Cheesey"] = 0;
            _infPlaystyle ["Aggresive"] = 0;
            _infPlaystyle ["Defensive"] = 0;

            _vanPlaystyle["Cheesey"] = 0;
            _vanPlaystyle["Aggresive"] = 0;
            _vanPlaystyle["Defensive"] = 0;

            _acgPlaystyle["Cheesey"] = 0;
            _acgPlaystyle["Aggresive"] = 0;
            _acgPlaystyle["Defensive"] = 0;
            _acgPlaystyle["Nya!!!"] = 99;
        }


        public string QPlayerType(string race)
        {
            switch (race)
            {
                case "van":
                    return _vanPlaystyle.Max().ToString();

                case "inf":
                    return _infPlaystyle.Max().ToString();

                case "acg":
                    return _acgPlaystyle.Max().ToString();

                default:
                    return null;
            }
        }

        public void inputPlayerstyle(string inputPlaystyle, Opponent Opponent)
        {
            switch (inputPlaystyle.ToLower()) 
            {
                case "cheesey":
                    Opponent._infPlaystyle["Cheesey"]++;
                    break;

                case "aggresive":
                    Opponent._infPlaystyle["Aggresive"]++;
                    break;

                case "defensive":
                    Opponent._infPlaystyle["Defensive"]++;
                    break;

                default:
                    Console.WriteLine("Non-valid playstyle was entered");
                    break;
            }
        }
    }
}
