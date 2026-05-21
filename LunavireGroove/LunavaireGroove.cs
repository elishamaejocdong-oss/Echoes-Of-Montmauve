using Echoes_of_Montmauve.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Drawing;
using Echoes_of_Montmauve.GameLogic;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Echoes_of_Montmauve.SharedUI;

namespace Echoes_of_Montmauve.LunavireGroove
{
    internal class LunavaireGroove: District
    {

        private const int GridSize = 7;
        private const int SeedsRemaining = 5;
        private const int MaismaSpreadeInterval = 10000;

        private int _seedsRemaining;
        public int seedsRemaining => _seedsRemaining;
        private bool[,] isMaismic = new bool[GridSize, GridSize];
        private System.Windows.Forms.Timer spreadTimer;
        private Random rand = new Random();
        private DateTime _startTime;
        private bool _isGameOver = false;


        public override void StartPuzzle()
        {
            _isGameOver = false;
            _seedsRemaining = SeedsRemaining ;
            isMaismic = new bool[GridSize, GridSize];

            _startTime = DateTime.Now;

            InitializeGrid();
            StartSpreadTime();
        }

        private void InitializeGrid()
        {
            int planted = 0;
            while(planted < 5)
            {
                int r = rand.Next(0, GridSize);
                int c = rand.Next(0, GridSize);
                if(!isMaismic[r,c])
                {
                    isMaismic[r, c] = true;
                    planted++;
                }
            }
        }

        private void StartSpreadTime()
        {
            if (spreadTimer != null) { spreadTimer.Stop(); }
            spreadTimer = new System.Windows.Forms.Timer();
            spreadTimer.Interval = MaismaSpreadeInterval;
            spreadTimer.Tick += (s, e) => SpreadMaisma();
            spreadTimer.Start();
        }

        public bool PlantSeed(int row, int col)
        {
            if(_isGameOver || seedsRemaining <= 0) return false;

            if (!isMaismic[row, col])
            {
                MessageBox.Show("You can only plant seeds on infected tiles!");
                return false;
            }
                _seedsRemaining--;
                
                PurifyTile(row, col);
                PurifyTile(row - 1, col);
                PurifyTile(row + 1, col);
                PurifyTile(row, col - 1);
                PurifyTile(row, col + 1);

                return CheckWinCondition();

        }

        private void PurifyTile(int row, int col)
        {
            if(row >= 0 && row < GridSize && col >= 0 && col < GridSize)
            {
                isMaismic[row, col] = false;
            }
        }

        private void SpreadMaisma()
        {
            if (_isGameOver)
            {
                return;
            }
            List<Point> newContaminations = new List<Point>();

            for(int r = 0; r<GridSize; r++)
            {
                for(int c = 0; c < GridSize; c++)
                {
                    if(!isMaismic[r, c] && HasMaismicNeighbor(r, c))
                    {
                        if(rand.Next(0, 10) > 6)
                        {
                            newContaminations.Add(new Point(r,c));
                        }
                    }
                }
            }

            foreach(var p in newContaminations)
            {
                isMaismic[p.X, p.Y] = true;
            }
        }

        private bool HasMaismicNeighbor(int r, int c)
        {
            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };

            for(int i = 0; i <4; i++)
            {
                int nr = r + dr[i];
                int nc = c + dc[i];
                if(nr >= 0 && nr < GridSize && nc >= 0 && nc < GridSize && isMaismic[nr,nc])
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckWinCondition()
        {
            if (_isGameOver)
            {
                return false;
            }

            bool hasMiasma = false;

            foreach (bool cell in isMaismic)
            {
                if (cell)
                {
                    hasMiasma = true;
                    break;
                }
            }

            if (!hasMiasma)
            {
               _isGameOver = true;
                spreadTimer.Stop();
                return true;
            }

            return false;
        }

        public bool IsMaismic(int row, int col)
        {
            return isMaismic[row, col];
        }
        
        public bool CheckLossConditions()
        {
            if(!_isGameOver && _seedsRemaining <= 0)
            {
                _isGameOver = true;
                spreadTimer.Stop();
                return true;
            }
            return false;
        }

        public int GetTimeTaken()=>(int)(DateTime.Now-_startTime).TotalSeconds;
    }
}
