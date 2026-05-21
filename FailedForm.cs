using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Echoes_of_Montmauve
{
    public partial class FailedForm : Form
    {
        private readonly Action _tryAgainAction;
        public FailedForm(Action tryAgainAction)
        {
            InitializeComponent();
            _tryAgainAction = tryAgainAction;
        }

        private void btnTryAgain_Click(object sender, EventArgs e)
        {
            _tryAgainAction?.Invoke();
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            MainMenu mainMenu = new MainMenu();
            mainMenu.Show();
            this.Close();
        }
    }
}
