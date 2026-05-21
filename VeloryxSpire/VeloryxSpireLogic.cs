using Echoes_of_Montmauve.GameLogic;
using Echoes_of_Montmauve.Models;
using Echoes_of_Montmauve.Properties;
using Echoes_of_Montmauve.SharedUI;
using Echoes_of_Montmauve.VeloryxSpire;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Echoes_of_Montmauve.NewFolder
{

    internal class VeloryxSpireLogic:District
    {
       
        public List<Button>  SelectedCards = new List<Button>();
        public int MatchesFound = 0;
        public const int TotalPairs = 6;

        public override void StartPuzzle()
        {
            VeloryxMatch memoryGame = new VeloryxMatch(this);
            memoryGame.Show();
        }

        public bool CheckMatch()
        {
            if(SelectedCards.Count !=2) return false;

            bool isMatch = SelectedCards[0].Tag == SelectedCards[1].Tag;

            if (isMatch)
            {
                MatchesFound++;
                foreach(var btn in SelectedCards)
                {
                    btn.Enabled = false;
                    btn.BackColor = Color.Gold;
                }
                SelectedCards.Clear();
                return true;
            }
            return false;
        }

        public void ResetCards()
        {
            foreach(var btn in SelectedCards)
            {
                if (btn.Enabled)
                {
                    btn.BackgroundImage = Properties.Resources.HiddenPart;
                }
            }
            SelectedCards.Clear();
        }

        public bool IsGameWon()
        {
            return MatchesFound == TotalPairs;
        }

    }
}
