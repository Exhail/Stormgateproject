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
        public string _OpponentNick;

        private Dictionary<string, int> _infBehaviour;
        private Dictionary<string, int> _vanBehaviour;
        private Dictionary<string, int> _acgBehaviour;

        private int _infGameCount;
        private int _infWinCount;
        private int _infWinrate;

        private int _vanGameCount;
        private int _vanWinCount;
        private int _vanWinrate;

        private int _acgGameCount;
        private int _acgWinCount;
        private int _acgWinrate;


        public Opponent(string opponentNick)
        {
            _OpponentNick = opponentNick;

            _infBehaviour = new Dictionary<string, int>();
            _vanBehaviour = new Dictionary<string, int>();
            _acgBehaviour = new Dictionary<string, int>();



            _infGameCount = 0;
            _infWinCount = 0;
            if (_infGameCount > 0)
            {
                _infWinrate = _infGameCount / _infWinCount;
            }
            else _infGameCount = 0;

            _vanGameCount = 0;
            _vanWinCount = 0;
            if (_vanGameCount > 0)
            {
                _vanWinrate = _vanGameCount / _vanWinCount;
            }
            else _vanGameCount = 0;

            _acgGameCount = 0;
            _acgWinCount = 0;
            if (_acgGameCount > 0)
            {
                _acgWinrate = _acgGameCount / _acgWinCount;
            }
            else _acgGameCount = 0;

            //Infernal Behaviour
            _infBehaviour["Cheesey"] = 0;
            _infBehaviour["Aggresive"] = 0;
            _infBehaviour["Defensive"] = 0;
            
            //Vangard Behaviour
            _vanBehaviour["Cheesey"] = 0;
            _vanBehaviour["Aggresive"] = 0;
            _vanBehaviour["Defensive"] = 0;

            //3rd Race Behaviour
            _acgBehaviour["Cheesey"] = 0;
            _acgBehaviour["Aggresive"] = 0;
            _acgBehaviour["Defensive"] = 0;
            _acgBehaviour["Nya!!!"] = 99;
        }


        public Dictionary<string, int> BehaviourHistory(string race)
        {
            switch (race)
            {
                case "van":
                    return _vanBehaviour;

                case "inf":
                    return _infBehaviour;

                case "acg":
                    return _acgBehaviour;

                default:
                    throw new Exception("Incorrect Race Entered to BehaviourHistory Method");
            }
        }

        public int[] Statistics(string race)
        {
            //Game Count, Win Count, Win Ratio.
            int[] GC_WC_WR = new int[3];

            switch (race)
            {
                case "van":
                    GC_WC_WR[0] = _vanGameCount;
                    GC_WC_WR[1] = _vanWinCount;
                    GC_WC_WR[2] = _vanWinrate;
                    return GC_WC_WR;

                case "inf":
                    GC_WC_WR[0] = _infGameCount;
                    GC_WC_WR[1] = _infWinCount;
                    GC_WC_WR[2] = _infWinrate;
                    return GC_WC_WR;

                case "acg":
                    GC_WC_WR[0] = _acgGameCount;
                    GC_WC_WR[1] = _acgWinCount;
                    GC_WC_WR[2] = _acgWinrate;
                    return GC_WC_WR;

                default:
                    throw new Exception("Incorrect Race Entered to Statistics Method");
            }
        }

        public void InputPlayerstyle(string inputPlaystyle, string oppRace)
        {
            switch (inputPlaystyle.ToLower()) 
            {
                case "cheesey":
                    this._infBehaviour["Cheesey"]++;
                    break;

                case "aggresive":
                    this._infBehaviour["Aggresive"]++;
                    break;

                case "defensive":
                    this._infBehaviour["Defensive"]++;
                    break;

                default:
                    Console.WriteLine("Non-valid playstyle was entered");
                    break;
            }
        }
    }
}
