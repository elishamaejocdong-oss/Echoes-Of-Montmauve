using Echoes_of_Montmauve.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Echoes_of_Montmauve.GameLogic
{
    public static class SessionContent
    {
        public enum GamePhase
        {
            Phase1_Containment,
            Phase2_Madman,
            Phase3_Accusation
        }

        public static GamePhase CurrentPhase { get; private set; } = GamePhase.Phase1_Containment;
        public static bool MinigameClearedForCurrentDistrict { get; set; } = false;

        internal static Player CurrentActivePlayer { get; set; }
        internal static District CurrentDistrict { get; set; }

        public static readonly string[] DistrictOrder = new[]
        {
            "Marrow Market",
            "Veloryx Spire",
            "Lunavaire Groove",
            "Eidraxis Hall",
            "Cinder Pipes"
        };

        public static HashSet<string> PurifiedDistricts { get; private set; } = new HashSet<string>();
        public static string LastPurifiedDistrict { get; private set; } = "";

        public static List<string> GatheredClues { get; private set; } = new List<string>();

        public static int Phase2SecondsRemaining { get; set; } = 72 * 60 * 60;
        public static bool Phase2TimerActive { get; set; } = false;

        // ── District state ────────────────────────────────────────────────────

        public static bool AreAllDistrictsPurified()
        {
            return PurifiedDistricts.Count >= DistrictOrder.Length;
        }

        public static bool IsDistrictUnlocked(string districtName)
        {
            int index = Array.IndexOf(DistrictOrder, districtName);

            if (index <= 0) return true;

            string previousDistrict = DistrictOrder[index - 1];
            string user = CurrentActivePlayer?.Username;

            // Check in-memory first; fall back to DB for robustness
            return PurifiedDistricts.Contains(previousDistrict) ||
                   (user != null && DatabaseManager.IsDistrictPurified(user, previousDistrict));
        }

        /// <summary>
        /// Syncs the in-memory PurifiedDistricts set from the database.
        /// Call this after login and after any Phase 2 reset so the HashSet
        /// always matches the DB truth.
        /// </summary>
        public static void ReloadPurifiedDistrictsFromDatabase()
        {
            PurifiedDistricts.Clear();

            if (CurrentActivePlayer == null) return;

            foreach (string district in DistrictOrder)
            {
                if (DatabaseManager.IsDistrictPurified(CurrentActivePlayer.Username, district))
                    PurifiedDistricts.Add(district);
            }
        }

        public static void AddClue(string clue)
        {
            if (!GatheredClues.Contains(clue))
                GatheredClues.Add(clue);
        }

        /// <summary>
        /// Marks a district as purified in BOTH memory and the database.
        /// This keeps the two sources of truth in sync so that:
        ///   - the Map can read IsPurified from the DB correctly, and
        ///   - IsDistrictUnlocked() works from the in-memory set.
        /// Also reduces the Miasma level in PlayerProgress by 15 per district.
        /// </summary>
        public static void PurifyDistrict(string districtName)
        {
            if (CurrentActivePlayer == null) return;

            // ── 1. Update in-memory set ───────────────────────────────────────
            if (!PurifiedDistricts.Contains(districtName))
            {
                PurifiedDistricts.Add(districtName);
                LastPurifiedDistrict = districtName;
            }

            // ── 2. Persist to database (IsPurified = True + Miasma - 15) ─────
            // This is the call that was missing — without it the DB never knew
            // a district was purified, so the Map showed wrong state and the
            // Phase 2 reset had nothing to roll back.
            int timeTaken = 0; // time is already logged by LogGameSession; pass 0 here
            DatabaseManager.UpdateDistrictPurified(
                CurrentActivePlayer.Username, districtName, timeTaken);
        }

        // ── Phase transitions ─────────────────────────────────────────────────

        public static void AdvanceToPhase3()
        {
            CurrentPhase = GamePhase.Phase3_Accusation;
            Phase2TimerActive = false;
        }

        public static void StartPhase2Timer()
        {
            CurrentPhase = GamePhase.Phase2_Madman;
            Phase2TimerActive = true;
            if (Phase2SecondsRemaining <= 0)
                Phase2SecondsRemaining = 72 * 60 * 60;
        }

        /// <summary>
        /// Called at the end of the Cinder Pipes Madman encounter to begin
        /// Phase 2. Resets districts and Miasma to 100 in the DB, clears the
        /// in-memory state, and starts the 72-hour countdown.
        /// </summary>
        public static void StartPhase2Loop()
        {
            if (CurrentActivePlayer != null)
            {
                // ── Reset DB: IsPurified = False, MaismaLevel = 100 ───────────
                DatabaseManager.ResetDistrictsForPhase2(CurrentActivePlayer.Username);

                // ── Sync in-memory set FROM DB so it matches the reset ────────
                // (ReloadPurifiedDistrictsFromDatabase will now find all False,
                //  so PurifiedDistricts will be empty after this call.)
                ReloadPurifiedDistrictsFromDatabase();
            }

            // ── Clear remaining in-memory state ───────────────────────────────
            LastPurifiedDistrict = "";
            MinigameClearedForCurrentDistrict = false;
            Phase2SecondsRemaining = 72 * 60 * 60;

            StartPhase2Timer();
        }

        // ── Session lifecycle ─────────────────────────────────────────────────

        public static void ClearSession()
        {
            CurrentActivePlayer = null;
            CurrentDistrict = null;
            CurrentPhase = GamePhase.Phase1_Containment;
            PurifiedDistricts.Clear();
            GatheredClues.Clear();
            LastPurifiedDistrict = "";
            Phase2TimerActive = false;
            Phase2SecondsRemaining = 72 * 60 * 60;
            MinigameClearedForCurrentDistrict = false;
        }

        public static string GetNextDistrict(string currentDistrictName)
        {
            int index = Array.IndexOf(DistrictOrder, currentDistrictName);
            if (index >= 0 && index < DistrictOrder.Length - 1)
                return DistrictOrder[index + 1];

            return "Final Confrontation";
        }

        public static void Initialize(string username, string password, int age, string gender)
        {
            CurrentActivePlayer = new Player(username, password, age, gender);
        }

        public static string GetFormattedPhase2Time()
        {
            TimeSpan t = TimeSpan.FromSeconds(Phase2SecondsRemaining);
            return string.Format("{0:D2}h:{1:D2}m:{2:D2}s", (int)t.TotalHours, t.Minutes, t.Seconds);
        }
    }
}