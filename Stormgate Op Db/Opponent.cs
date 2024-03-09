using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Stormgate_Op_Db
{
    public class Opponent
    {

        public string _playerID;
        public int _pastCheeseCount = 0;
        public int _pastMacroCount = 0;
        public int _pastGameCount;

        public Opponent(string playerID)
        {
            _playerID = playerID;
            _pastGameCount = _pastMacroCount + _pastCheeseCount;
            _pastCheeseCount = 0;
            _pastMacroCount = 0;
        }


        public string QPlayerType()
        {
            string playerType;
            if (_pastCheeseCount != _pastMacroCount)
            {
                playerType = (_pastCheeseCount > _pastMacroCount ? "Cheesey" : "Macro");
            }
            else playerType = "Mixed";
            return playerType;
        }

        public void inputPlayerstyle(string playstyle, Opponent Opponent)
        {
            if (string.IsNullOrEmpty(playstyle)) throw new ArgumentNullException(nameof(playstyle));

            if (playstyle.ToLower() == "cheese")
            {
                Opponent._pastCheeseCount++;
            }

            if (playstyle.ToLower() == "macro")
            {
                Opponent._pastMacroCount++;
            }
        }
    }
}
