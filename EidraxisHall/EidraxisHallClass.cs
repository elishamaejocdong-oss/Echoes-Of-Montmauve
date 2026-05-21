using Echoes_of_Montmauve.GameLogic;
using Echoes_of_Montmauve.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Echoes_of_Montmauve.EidraxisHall
{
    internal class EidraxisHallClass: District
    {
        public string TargetWord { get; set; } = "CIVIC";

        public string CurrentRiddle { get; set; } = "A five-letter word for public life and shared city responsibility.";
        public string TrueDefinition { get; set; } = "Connected with citizens, community duties, and public life.";

        public override void StartPuzzle()
        {
            DataTable dt = DatabaseManager.GetRandomWordleEntry();

            if(dt !=null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string loadedWord = NormalizeWord(row["Word"]?.ToString());

                if (loadedWord.Length == 5)
                {
                    TargetWord = loadedWord;
                    CurrentRiddle = string.IsNullOrWhiteSpace(row["Riddles"]?.ToString())
                        ? CurrentRiddle
                        : row["Riddles"].ToString();
                    TrueDefinition = string.IsNullOrWhiteSpace(row["Definition"]?.ToString())
                        ? TrueDefinition
                        : row["Definition"].ToString();
                }
                else
                {
                    UseFallbackPuzzle();
                }
            }
            else
            {
                UseFallbackPuzzle();
            }
        }

        public int[] CheckGuess(string guess)
        {
            int[] results = new int[5]; 
            string target = NormalizeWord(TargetWord).PadRight(5, '_').Substring(0, 5);
            string normalizedGuess = NormalizeWord(guess).PadRight(5, '_').Substring(0, 5);
            char[] targetArr = target.ToCharArray();
            char[] guessArr = normalizedGuess.ToCharArray();

           
            for (int i = 0; i < 5; i++)
            {
                if (guessArr[i] == targetArr[i])
                {
                    results[i] = 2;
                    targetArr[i] = ' ';
                    guessArr[i] = '_';
                }
            }

            
            for (int i = 0; i < 5; i++)
            {
                if (guessArr[i] == '_') continue;
                for (int j = 0; j < 5; j++)
                {
                    if (guessArr[i] == targetArr[j])
                    {
                        results[i] = 1;
                        targetArr[j] = ' ';
                        break;
                    }
                }
            }
            return results;
        }

        private static string NormalizeWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return "";

            StringBuilder clean = new StringBuilder();
            foreach (char c in word.Trim().ToUpperInvariant())
            {
                if (c >= 'A' && c <= 'Z')
                    clean.Append(c);
            }

            return clean.ToString();
        }

        private void UseFallbackPuzzle()
        {
            TargetWord = "CIVIC";
            CurrentRiddle = "A five-letter word for public life and shared city responsibility.";
            TrueDefinition = "Connected with citizens, community duties, and public life.";
        }
    }
}
