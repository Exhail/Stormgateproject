using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Media;

namespace Stormgate_Op_Db
{

    public class OpponentDB
    {
        private static List<Opponent> _opponentList;
        private string _textCachePath = @"C:\Users\ellio\Desktop\C# Learning Fun times\Projects\Stormgate Op Db\Cache\Opp_DB.txt";

        public void AddOpponent(Opponent opponent)
        {
            var jsonTextCache = File.ReadAllText(_textCachePath);

            if (!string.IsNullOrEmpty(jsonTextCache))
            {
                _opponentList = JsonConvert.DeserializeObject<List<Opponent>>(jsonTextCache);
            }
            _opponentList.Add(opponent);

            string updatedJsonPackage = JsonConvert.SerializeObject(_opponentList);
            File.WriteAllText(_textCachePath, updatedJsonPackage);
        }

        public bool HistoryCheck(string playerID)
        {
            foreach (Opponent player in _opponentList)
            {
                if (playerID == player._playerID)
                {
                    return true;
                }
            }
            return false;
        }

        public Opponent LoadOpponent(string playerID)
        {
            foreach (var player in _opponentList)
            {
                if (playerID == player._playerID)
                {
                    return player;
                }
            }
            Console.WriteLine(@"Player: " + playerID + "not found"); //Change to throw an Exception?
            return null;
        }

        public List<Opponent> LoadList()
        {
            var jsonPackage = File.ReadAllText(_textCachePath);

            if (!string.IsNullOrEmpty(jsonPackage))
            {
                _opponentList = JsonConvert.DeserializeObject<List<Opponent>>(jsonPackage);
            }
            else _opponentList = new List<Opponent>();
            return _opponentList;
        }
    }
}
