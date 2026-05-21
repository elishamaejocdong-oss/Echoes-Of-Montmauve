using System;
using System.Collections.Generic;
using System.Text;

namespace Echoes_of_Montmauve
{
    internal class PlayerProgress
    {
        public string PlayerName { get; set; }
        public int TotalWins { get; set; }

        public int CurrentDay {  get; set; }
        public int LoopCount { get; set; }
        public int MaismaLevel { get; set; }

        public PlayerProgress()
        {
            CurrentDay = 1;
            LoopCount = 0;
            MaismaLevel = 100;
        }

        public List<Artifact> Inventory { get; set; }
        public int MiasmaLevel { get; private set; } = 100;

        public PlayerProgress(string playerName)
        {
            PlayerName = playerName;
            TotalWins = 0;
            Inventory = new List<Artifact>();
        }

        public void AddArtifact(Artifact artifact)
        {
            if (!artifact.IsUnlocked)
            {
                artifact.IsUnlocked = true;
                Inventory.Add(artifact);
                TotalWins++;
            }
        }

       
    }
}
