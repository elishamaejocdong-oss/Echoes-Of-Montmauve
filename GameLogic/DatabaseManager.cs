using Echoes_of_Montmauve.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Echoes_of_Montmauve.GameLogic
{
    internal static class DatabaseManager
    {
        // ── Models ────────────────────────────────────────────────────────────
        public class MarrowWordTile
        {
            public string WordTile { get; set; }
            public string WordText { get; set; }
            public string WordCategory { get; set; }
        }

        public class GameSessionRecord
        {
            public int SessionID { get; set; }
            public string Username { get; set; }
            public string GameName { get; set; }
            public string DistrictName { get; set; }
            public int Score { get; set; }
            public int TimeTaken { get; set; }
            public bool IsVictory { get; set; }
            public string PlayedOn { get; set; }
        }

        private static string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\JOCDONG\Downloads\FinalProject2.0-main\FinalProject2.0-main\Echoes of Montmauve\MontmauveDB.accdb;Persist Security Info=False;";

        public static OleDbConnection GetConnection()
        {
            return new OleDbConnection(connectionString);
        }

        // ── Users ─────────────────────────────────────────────────────────────
        public static bool CheckUserExists(string username)
        {
            using (OleDbConnection conn = GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Users WHERE [Username] = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                try
                {
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking user: " + ex.Message);
                    return false;
                }
            }
        }

        public static bool RegisterPlayer(string username, string password, int age, string bday, string gender)
        {
            SessionContent.Initialize(username, gender, age, gender);
            if (CheckUserExists(username))
            {
                MessageBox.Show("Username already exists!");
                return false;
            }

            using (OleDbConnection conn = GetConnection())
            {
                string query = "INSERT INTO Users ([Username],[UserPass],[Age],[Birthdate],[Gender]) VALUES (?,?,?,?,?)";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = password;
                cmd.Parameters.Add("?", OleDbType.Integer).Value = age;
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = bday;
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = gender;
                try
                {
                    conn.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        CreateInitialProgress(username);
                        EnsureDistrictRows(conn, username);
                        SessionContent.CurrentActivePlayer = new Player(username, password, age, gender);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Registration Error: " + ex.Message);
                }
            }
            return false;
        }

        public static bool ValidateLogin(string username, string password)
        {
            using (OleDbConnection conn = GetConnection())
            {
                string query = "SELECT [Age],[Gender] FROM Users WHERE [Username] = ? AND [UserPass] = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = password;
                try
                {
                    conn.Open();
                    int age = 0;
                    string gender = "";
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            age = Convert.ToInt32(reader["Age"]);
                            gender = reader["Gender"].ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(gender))
                    {
                        SessionContent.CurrentActivePlayer = new Player(username, password, age, gender);
                        // Ensure district rows exist for this player
                        EnsureDistrictRows(username);
                        // ── Sync in-memory purified set from DB so IsDistrictUnlocked()
                        //    and AreAllDistrictsPurified() are correct from the first frame.
                        SessionContent.ReloadPurifiedDistrictsFromDatabase();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Login DB Error: " + ex.Message);
                }
            }
            return false;
        }

        public static bool ResetPassword(string username, DateTime birthdate, string newPassword)
        {
            using (OleDbConnection conn = GetConnection())
            {
                string query = "SELECT [Birthdate] FROM [Users] WHERE [Username] = ?";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                            return false;

                        if (!DateTime.TryParse(result.ToString(), out DateTime savedBirthdate)
                            || savedBirthdate.Date != birthdate.Date)
                            return false;

                        using (OleDbCommand updateCmd = new OleDbCommand("UPDATE [Users] SET [UserPass] = ? WHERE [Username] = ?", conn))
                        {
                            updateCmd.Parameters.Add("?", OleDbType.VarWChar).Value = newPassword;
                            updateCmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                            return updateCmd.ExecuteNonQuery() > 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Password reset error: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        public static bool DeleteAccount(string username)
        {
            using (OleDbConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    using (OleDbTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            string[] queries = {
                                "DELETE FROM [GameSessions]            WHERE [Username] = ?",
                                "DELETE FROM [PlayerNotebookProgress]  WHERE [UserName] = ?",
                                "DELETE FROM [Districts]               WHERE [Username] = ?",
                                "DELETE FROM [PlayerProgress]          WHERE [Username] = ?",
                                "DELETE FROM [Users]                   WHERE [Username] = ?"
                            };
                            foreach (string query in queries)
                            {
                                using (OleDbCommand cmd = new OleDbCommand(query, conn, transaction))
                                {
                                    cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to wipe data: " + ex.Message);
                    return false;
                }
            }
        }

        // ── Districts ─────────────────────────────────────────────────────────
        public static void EnsureDistrictRows(string username)
        {
            using (OleDbConnection conn = GetConnection())
            {
                conn.Open();
                EnsureDistrictRows(conn, username);
            }
        }

        private static void EnsureDistrictRows(OleDbConnection conn, string username)
        {
            string[] districts = { "Lunavaire Groove", "Veloryx Spire", "Marrow Market", "Eidraxis Hall", "Cinder Pipes" };
            foreach (string district in districts)
            {
                string checkQuery = "SELECT COUNT(*) FROM [Districts] WHERE [Username] = ? AND [DistrictName] = ?";
                using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                    checkCmd.Parameters.Add("?", OleDbType.VarWChar).Value = district;

                    if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                        continue;
                }

                string insertQuery = "INSERT INTO [Districts] ([Username],[DistrictName],[IsPurified]) VALUES (?,?,False)";
                using (OleDbCommand insertCmd = new OleDbCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                    insertCmd.Parameters.Add("?", OleDbType.VarWChar).Value = district;
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        public static bool IsDistrictUnlocked(string username, string districtName)
        {
            if (districtName == "Marrow Market") return true;

            string requiredPrevious = "";
            if (districtName == "Veloryx Spire") requiredPrevious = "Marrow Market";
            if (districtName == "Lunavaire Groove") requiredPrevious = "Veloryx Spire";
            if (districtName == "Eidraxis Hall") requiredPrevious = "Lunavaire Groove";
            if (districtName == "Cinder Pipes") requiredPrevious = "Eidraxis Hall";
            if (districtName == "Aurelis Heights") requiredPrevious = "Cinder Pipes";

            using (OleDbConnection conn = GetConnection())
            {
                OleDbCommand cmd = new OleDbCommand("SELECT [IsPurified] FROM [Districts] WHERE [Username] = ? AND [DistrictName] = ?", conn);
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = requiredPrevious;
                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null && Convert.ToBoolean(result) == true;
                }
                catch { return false; }
            }
        }

        public static int GetPurifiedDistrictCount(string username)
        {
            string query = "SELECT COUNT(*) FROM Districts WHERE [Username] = ? AND [IsPurified] = True";
            using (OleDbConnection conn = GetConnection())
            {
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                try
                {
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching purified district count: " + ex.Message);
                    return 0;
                }
            }
        }

        public static int GetPurifiedCount(string username)
        {
            return GetPurifiedDistrictCount(username);
        }

        public static bool IsDistrictPurified(string username, string districtName)
        {
            string query = "SELECT [IsPurified] FROM [Districts] WHERE [Username] = ? AND [DistrictName] = ?";
            using (OleDbConnection conn = GetConnection())
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = districtName;

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null && result != DBNull.Value && Convert.ToBoolean(result);
                }
                catch
                {
                    return false;
                }
            }
        }

        public static void ResetDistrictsForPhase2(string username)
        {
            using (OleDbConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    string resetDistricts = "UPDATE [Districts] SET [IsPurified] = False WHERE [Username] = ?";
                    using (OleDbCommand cmdDistricts = new OleDbCommand(resetDistricts, conn))
                    {
                        cmdDistricts.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                        cmdDistricts.ExecuteNonQuery();
                    }

                    string resetMiasma = "UPDATE [PlayerProgress] SET [MaismaLevel] = 100, [LastSaved] = 0 WHERE [Username] = ?";
                    using (OleDbCommand cmdMiasma = new OleDbCommand(resetMiasma, conn))
                    {
                        cmdMiasma.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                        cmdMiasma.ExecuteNonQuery();
                    }

                    string clearSessions = "DELETE FROM [GameSessions] WHERE [Username] = ?";
                    using (OleDbCommand cmdSessions = new OleDbCommand(clearSessions, conn))
                    {
                        cmdSessions.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                        cmdSessions.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Phase 2 reset error: " + ex.Message);
                }
            }
        }

        public static void UpdateDistrictPurified(string username, string districtName, int timeUsed)
        {
            using (OleDbConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();

                    string queryDist = "UPDATE [Districts] SET [IsPurified] = True, [BestTime] = ? WHERE [Username] = ? AND [DistrictName] = ?";
                    using (OleDbCommand cmdDist = new OleDbCommand(queryDist, conn))
                    {
                        cmdDist.Parameters.Add("?", OleDbType.Integer).Value = Math.Min(timeUsed, 3600);
                        cmdDist.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                        cmdDist.Parameters.Add("?", OleDbType.VarWChar).Value = districtName;
                        int rows = cmdDist.ExecuteNonQuery();
                        if (rows == 0)
                        {
                            EnsureDistrictRows(conn, username);
                            rows = cmdDist.ExecuteNonQuery();
                            if (rows == 0)
                                MessageBox.Show($"Warning: district '{districtName}' not updated for '{username}'. Check the Districts table.", "DB Warning");
                        }
                    }

                    int currentMiasma = 100;
                    string queryCurrentMiasma = "SELECT [MaismaLevel] FROM [PlayerProgress] WHERE [Username] = ?";
                    using (OleDbCommand cmdCurrentMiasma = new OleDbCommand(queryCurrentMiasma, conn))
                    {
                        cmdCurrentMiasma.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                        object currentResult = cmdCurrentMiasma.ExecuteScalar();
                        if (currentResult != null && currentResult != DBNull.Value)
                            currentMiasma = Convert.ToInt32(currentResult);
                    }

                    string queryMiasma = "UPDATE [PlayerProgress] SET [MaismaLevel] = ? WHERE [Username] = ?";
                    using (OleDbCommand cmdMiasma = new OleDbCommand(queryMiasma, conn))
                    {
                        int nextMiasma = Math.Max(0, currentMiasma - 15);
                        cmdMiasma.Parameters.Add("?", OleDbType.Integer).Value = nextMiasma;
                        cmdMiasma.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                        cmdMiasma.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Update Error: " + ex.Message);
                }
            }
        }

        // ── Notebook ──────────────────────────────────────────────────────────
        public static bool IsNoteUnlocked(string username, string itemName)
        {
            string query = "SELECT COUNT(*) FROM PlayerNotebookProgress WHERE [UserName] = ? AND [ItemName] = ?";
            using (OleDbConnection conn = GetConnection())
            {
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                    cmd.Parameters.Add("?", OleDbType.VarWChar).Value = itemName;
                    try
                    {
                        conn.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                    catch { return false; }
                }
            }
        }

        public static void UnlockNotebookItem(string username, string itemName)
        {
            if (IsNoteUnlocked(username, itemName)) return;

            string query = "INSERT INTO PlayerNotebookProgress ([UserName], [ItemName]) VALUES (?,?)";
            using (OleDbConnection conn = GetConnection())
            {
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                    cmd.Parameters.Add("?", OleDbType.VarWChar).Value = itemName;
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating notebook data tracking: " + ex.Message);
                    }
                }
            }
        }

        public static DataTable GetUnlockedNotesByTable(string username, string tabType)
        {
            DataTable dt = new DataTable();
            bool cluesOnly = string.Equals(tabType, "Clue", StringComparison.OrdinalIgnoreCase);

            using (OleDbConnection conn = GetConnection())
            {
                // 1. Create a clean base command
                using (OleDbCommand cmd = new OleDbCommand("", conn))
                {
                    if (cluesOnly)
                    {
                        // Query structure for Clues matching the player's progress tracker
                        cmd.CommandText = "SELECT DISTINCT n.ItemName, n.Description, n.ImagePath FROM NotebookData n " +
                                          "INNER JOIN PlayerNotebookProgress p ON n.ItemName = p.ItemName " +
                                          "WHERE p.UserName = ? AND n.TabType = ? " +
                                          "ORDER BY n.ItemName";

                        // 1st placeholder: p.UserName = ?
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                        // 2nd placeholder: n.TabType = ?
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = tabType;
                    }
                    else
                    {
                        // Standard query structure for general SDG Notes and Game Notes
                        cmd.CommandText = "SELECT n.ItemName, n.Description, n.ImagePath FROM NotebookData n " +
                                          "WHERE n.TabType = ? " +
                                          "ORDER BY n.ItemName";

                        // Only 1 placeholder here: n.TabType = ?
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = tabType;
                    }

                    try
                    {
                        conn.Open();
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error retrieving notebook item: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return dt;
        }

        // ── Player Progress ───────────────────────────────────────────────────
        private static void CreateInitialProgress(string username)
        {
            using (OleDbConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    EnsurePlayerProgressStoryColumns(conn);

                    string checkQuery = "SELECT COUNT(*) FROM PlayerProgress WHERE [Username] = ?";
                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0) return;
                    }

                    string query = "INSERT INTO PlayerProgress (Username, MaismaLevel, LastSaved) VALUES (?, 100, 0)";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating initial progress: " + ex.Message);
                }
            }
        }

        private static void EnsurePlayerProgressStoryColumns(OleDbConnection conn)
        {
            EnsureColumn(conn, "PlayerProgress", "EndingUnlocked", "TEXT(50)");
            EnsureColumn(conn, "PlayerProgress", "EndingUnlockedOn", "TEXT(50)");
        }

        private static void EnsureColumn(OleDbConnection conn, string tableName, string columnName, string columnDefinition)
        {
            try
            {
                using (OleDbCommand check = new OleDbCommand($"SELECT TOP 1 [{columnName}] FROM [{tableName}]", conn))
                {
                    check.ExecuteScalar();
                }
            }
            catch
            {
                using (OleDbCommand alter = new OleDbCommand($"ALTER TABLE [{tableName}] ADD COLUMN [{columnName}] {columnDefinition}", conn))
                {
                    alter.ExecuteNonQuery();
                }
            }
        }

        public static int GetMiasmaLevel(string username)
        {
            using (OleDbConnection conn = GetConnection())
            {
                string query = "SELECT MaismaLevel FROM PlayerProgress WHERE Username = ?";
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                conn.Open();
                object result = cmd.ExecuteScalar();
                return (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;
            }
        }

        public static void RecordEndingUnlocked(string username, string endingName)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(endingName))
                return;

            using (OleDbConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    EnsurePlayerProgressStoryColumns(conn);

                    string query = "UPDATE [PlayerProgress] SET [EndingUnlocked] = ?, [EndingUnlockedOn] = ? WHERE [Username] = ?";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = endingName;
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;

                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            string insert = "INSERT INTO [PlayerProgress] ([Username], [MaismaLevel], [LastSaved], [EndingUnlocked], [EndingUnlockedOn]) VALUES (?, 100, 0, ?, ?)";
                            using (OleDbCommand insertCmd = new OleDbCommand(insert, conn))
                            {
                                insertCmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                                insertCmd.Parameters.Add("?", OleDbType.VarWChar).Value = endingName;
                                insertCmd.Parameters.Add("?", OleDbType.VarWChar).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ending unlock save error: " + ex.Message);
                }
            }
        }

        public static string GetPlayerEndingUnlocked(string username)
        {
            using (OleDbConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    EnsurePlayerProgressStoryColumns(conn);

                    string query = "SELECT [EndingUnlocked] FROM [PlayerProgress] WHERE [Username] = ?";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                        object result = cmd.ExecuteScalar();
                        return result != null && result != DBNull.Value && !string.IsNullOrWhiteSpace(result.ToString())
                            ? result.ToString()
                            : "Not yet unlocked";
                    }
                }
                catch
                {
                    return "Not yet unlocked";
                }
            }
        }

        // ── Wordle ────────────────────────────────────────────────────────────
        public static DataTable GetRandomWordleEntry()
        {
            using (OleDbConnection conn = GetConnection())
            {
                string query = "SELECT TOP 1 [Word],[Riddles],[Definition] FROM [WordleDictionary] WHERE LEN(TRIM([Word])) = 5 ORDER BY RND(-(Timer()*[WordID]))";
                DataTable dt = new DataTable();
                try
                {
                    new OleDbDataAdapter(query, conn).Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching Wordle entry: " + ex.Message);
                }
                return dt;
            }
        }

        public static string GetWordDescription(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return "";

            string query = "SELECT [Definition] FROM [WordleDictionary] WHERE TRIM(UCASE([Word])) = ?";
            using (OleDbConnection conn = GetConnection())
            {
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = word.Trim().ToUpper();
                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value && !string.IsNullOrWhiteSpace(result.ToString()))
                        return result.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching word description: " + ex.Message);
                }
            }
            return "";
        }

        // ── Marrow Families ───────────────────────────────────────────────────
        public static List<MarrowWordTile> GetMarrowFamiliesPuzzle(int level)
        {
            var tiles = new List<MarrowWordTile>();
            string query = "SELECT CategoryName, WordValue FROM FamiliesPuzzle WHERE Level = ?";
            using (OleDbConnection conn = GetConnection())
            {
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.Integer).Value = level;
                try
                {
                    conn.Open();
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                        while (reader.Read())
                            tiles.Add(new MarrowWordTile
                            {
                                WordText = reader["WordValue"].ToString(),
                                WordCategory = reader["CategoryName"].ToString()
                            });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching Marrow Families puzzle: " + ex.Message);
                }
            }
            return tiles;
        }

        // ── Game Sessions ─────────────────────────────────────────────────────
        public static void LogGameSession(string username, string gameName,
            string districtName, int score, int timeTaken, bool isVictory)
        {
            string query = @"INSERT INTO GameSessions
                ([Username],[GameName],[DistrictName],[Score],[TimeTaken],[IsVictory],[PlayedOn])
                VALUES (?,?,?,?,?,?,?)";

            using (OleDbConnection conn = GetConnection())
            {
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = gameName;
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = districtName;
                cmd.Parameters.Add("?", OleDbType.Integer).Value = score;
                cmd.Parameters.Add("?", OleDbType.Integer).Value = Math.Min(timeTaken, 3600);
                cmd.Parameters.Add("?", OleDbType.Boolean).Value = isVictory;
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error logging session: " + ex.Message);
                }
            }
        }

        public static List<GameSessionRecord> GetPlayerSessions(string username)
        {
            var list = new List<GameSessionRecord>();
            string query = @"SELECT Username, GameName, DistrictName, Score, TimeTaken
                             FROM   GameSessions
                             WHERE  TRIM([Username]) = TRIM(?)
                             ORDER  BY SessionID DESC";

            using (OleDbConnection conn = GetConnection())
            {
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                try
                {
                    conn.Open();
                    using (OleDbDataReader r = cmd.ExecuteReader())
                        while (r.Read())
                            list.Add(new GameSessionRecord
                            {
                                Username = r["Username"].ToString(),
                                GameName = r["GameName"].ToString(),
                                DistrictName = r["DistrictName"].ToString(),
                                Score = Convert.ToInt32(r["Score"]),
                                TimeTaken = Convert.ToInt32(r["TimeTaken"])
                            });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching sessions: " + ex.Message);
                }
            }
            return list;
        }

        public static DataTable GetPlayerAnalytics(string username)
        {
            string query = @"SELECT
                GameName,
                COUNT(*)                                        AS TotalPlays,
                SUM(IIF(IsVictory = True,  1, 0))              AS Wins,
                SUM(IIF(IsVictory = False, 1, 0))              AS Losses,
                MAX(Score)                                      AS BestScore,
                MIN(IIF(IsVictory = True, TimeTaken, 99999))    AS BestTime
            FROM  GameSessions
            WHERE TRIM([Username]) = TRIM(?)
            GROUP BY GameName
            ORDER BY GameName";

            using (OleDbConnection conn = GetConnection())
            {
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                DataTable dt = new DataTable();
                try
                {
                    conn.Open();
                    new OleDbDataAdapter(cmd).Fill(dt);

                    // Compute WinRate in C# — Access integer division loses the decimal
                    dt.Columns.Add("WinRate", typeof(string));
                    foreach (DataRow row in dt.Rows)
                    {
                        int total = Convert.ToInt32(row["TotalPlays"]);
                        int wins = Convert.ToInt32(row["Wins"]);
                        row["WinRate"] = total > 0 ? $"{wins * 100 / total}%" : "0%";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching analytics: " + ex.Message);
                }
                return dt;
            }
        }

        public static DataTable GetAdminGameAnalytics()
        {
            string query = @"SELECT
                GameName,
                COUNT(*)                                     AS TotalPlays,
                SUM(IIF(IsVictory = True,  1, 0))           AS Wins,
                SUM(IIF(IsVictory = False, 1, 0))           AS Losses,
                MAX(Score)                                  AS BestScore,
                AVG(Score)                                  AS AverageScore,
                MIN(IIF(IsVictory = True, TimeTaken, 99999)) AS FastestWin
            FROM [GameSessions]
            GROUP BY GameName
            ORDER BY COUNT(*) DESC";

            DataTable dt = new DataTable();
            using (OleDbConnection conn = GetConnection())
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                try
                {
                    conn.Open();
                    new OleDbDataAdapter(cmd).Fill(dt);
                    dt.Columns.Add("WinRate", typeof(string));

                    foreach (DataRow row in dt.Rows)
                    {
                        int plays = Convert.ToInt32(row["TotalPlays"]);
                        int wins = Convert.ToInt32(row["Wins"]);
                        int fastestWin = Convert.ToInt32(row["FastestWin"]);
                        row["WinRate"] = plays > 0 ? $"{wins * 100 / plays}%" : "0%";
                        if (fastestWin == 99999)
                            row["FastestWin"] = DBNull.Value;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching admin analytics: " + ex.Message);
                }
            }
            return dt;
        }

        public static string GetAdminAnalyticsSummary()
        {
            using (OleDbConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    EnsurePlayerProgressStoryColumns(conn);

                    int players = ExecuteCount(conn, "SELECT COUNT(*) FROM [Users]");
                    int sessions = ExecuteCount(conn, "SELECT COUNT(*) FROM [GameSessions]");
                    int wins = ExecuteCount(conn, "SELECT COUNT(*) FROM [GameSessions] WHERE [IsVictory] = True");
                    int purified = ExecuteCount(conn, "SELECT COUNT(*) FROM [Districts] WHERE [IsPurified] = True");
                    int notes = ExecuteCount(conn, "SELECT COUNT(*) FROM [PlayerNotebookProgress]");
                    int endings = ExecuteCount(conn, "SELECT COUNT(*) FROM [PlayerProgress] WHERE [EndingUnlocked] IS NOT NULL AND TRIM([EndingUnlocked]) <> ''");
                    string winRate = sessions > 0 ? $"{wins * 100 / sessions}%" : "0%";

                    return $"Players: {players}    Sessions: {sessions}    Wins: {wins}    Win Rate: {winRate}    Purified District Records: {purified}    Notebook Entries: {notes}    Endings Unlocked: {endings}";
                }
                catch (Exception ex)
                {
                    return "Analytics summary unavailable: " + ex.Message;
                }
            }
        }

        public static DataTable GetAdminEndingAnalytics()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT
                [EndingUnlocked] AS EndingUnlocked,
                COUNT(*) AS Players
            FROM [PlayerProgress]
            WHERE [EndingUnlocked] IS NOT NULL AND TRIM([EndingUnlocked]) <> ''
            GROUP BY [EndingUnlocked]
            ORDER BY COUNT(*) DESC";

            using (OleDbConnection conn = GetConnection())
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                try
                {
                    conn.Open();
                    EnsurePlayerProgressStoryColumns(conn);
                    new OleDbDataAdapter(cmd).Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching ending analytics: " + ex.Message);
                }
            }

            return dt;
        }

        public static void EnsureFeedbackTable()
        {
            using (OleDbConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    using (OleDbCommand check = new OleDbCommand("SELECT COUNT(*) FROM [PlayerFeedback]", conn))
                    {
                        check.ExecuteScalar();
                    }
                }
                catch
                {
                    try
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        string create = @"CREATE TABLE [PlayerFeedback] (
                            [FeedbackID] AUTOINCREMENT PRIMARY KEY,
                            [Username] TEXT(255),
                            [Rating] INTEGER,
                            [Comment] LONGTEXT,
                            [SubmittedOn] TEXT(50)
                        )";
                        using (OleDbCommand createCmd = new OleDbCommand(create, conn))
                        {
                            createCmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Feedback table setup error: " + ex.Message);
                    }
                }
            }
        }

        public static bool SubmitPlayerFeedback(string username, int rating, string comment)
        {
            EnsureFeedbackTable();

            string query = "INSERT INTO [PlayerFeedback] ([Username],[Rating],[Comment],[SubmittedOn]) VALUES (?,?,?,?)";
            using (OleDbConnection conn = GetConnection())
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = username;
                cmd.Parameters.Add("?", OleDbType.Integer).Value = rating;
                cmd.Parameters.Add("?", OleDbType.LongVarWChar).Value = comment;
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Feedback save error: " + ex.Message);
                    return false;
                }
            }
        }

        public static DataTable GetPlayerFeedback()
        {
            EnsureFeedbackTable();

            DataTable dt = new DataTable();
            string query = "SELECT [Username], [Rating], [Comment], [SubmittedOn] FROM [PlayerFeedback] ORDER BY [FeedbackID] DESC";
            using (OleDbConnection conn = GetConnection())
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                try
                {
                    conn.Open();
                    new OleDbDataAdapter(cmd).Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching feedback: " + ex.Message);
                }
            }
            return dt;
        }

        public static string GetFeedbackSummary()
        {
            EnsureFeedbackTable();

            using (OleDbConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    int total = ExecuteCount(conn, "SELECT COUNT(*) FROM [PlayerFeedback]");
                    if (total == 0)
                        return "No player feedback has been submitted yet.";

                    object averageResult;
                    using (OleDbCommand avgCmd = new OleDbCommand("SELECT AVG([Rating]) FROM [PlayerFeedback]", conn))
                    {
                        averageResult = avgCmd.ExecuteScalar();
                    }

                    string average = averageResult != null && averageResult != DBNull.Value
                        ? Convert.ToDouble(averageResult).ToString("0.0")
                        : "0.0";

                    int fiveStars = ExecuteCount(conn, "SELECT COUNT(*) FROM [PlayerFeedback] WHERE [Rating] = 5");
                    int lowScores = ExecuteCount(conn, "SELECT COUNT(*) FROM [PlayerFeedback] WHERE [Rating] <= 2");

                    return $"Responses: {total}    Average Rating: {average}/5    5-Star Ratings: {fiveStars}    Low Ratings: {lowScores}";
                }
                catch (Exception ex)
                {
                    return "Feedback summary unavailable: " + ex.Message;
                }
            }
        }

        private static int ExecuteCount(OleDbConnection conn, string query)
        {
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                object result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }

        public static DataTable GetLeaderboard(string gameName)
        {
            string query = @"SELECT TOP 10
                Username, Score, TimeTaken, PlayedOn
            FROM  GameSessions
            WHERE TRIM([GameName]) = TRIM(?) AND IsVictory = True
            ORDER BY Score DESC, TimeTaken ASC";

            using (OleDbConnection conn = GetConnection())
            {
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.Add("?", OleDbType.VarWChar).Value = gameName;
                DataTable dt = new DataTable();
                try
                {
                    conn.Open();
                    new OleDbDataAdapter(cmd).Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching leaderboard: " + ex.Message);
                }
                return dt;
            }
        }
    }
}