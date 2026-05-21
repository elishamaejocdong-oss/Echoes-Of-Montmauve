using Echoes_of_Montmauve.GameLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Echoes_of_Montmauve.Models;
using System.Diagnostics.Eventing.Reader;
using System.Security.Principal;

namespace Echoes_of_Montmauve
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
            SignUpPnl.BackColor = Color.FromArgb(140, Color.PeachPuff);

            PasswordTxtBox.UseSystemPasswordChar = true;
        }

        public object Messagebox { get; private set; }

        private void SignUp2Btn_Click(object sender, EventArgs e)
        {
            string user = UsernameTxtBox.Text.Trim();
            string pass = PasswordTxtBox.Text;
            string gender = GenderCombBox.Text;
            DateTime bday = BdaydateTimePicker.Value;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(gender))
            {
                MessageBox.Show("Please fill in all fields.", "Input Error");
                return;
            }

            if(user.Length < 8)
            {
                MessageBox.Show("Username must be at least 8 characters long.", "Input Error");
                return;
            }

            if(user.Contains(" "))
            {
                MessageBox.Show("Username cannot contain spaces.", "Input Error");
                return;
            }

            int age;

            if(!int.TryParse(AgetxtBox.Text, out age) || age < 13)
            {
                MessageBox.Show("You must be at least 13 years old to create an account.", "Input Error");
                return;
            }

            int calcultedAge = DateTime.Now.Year - BdaydateTimePicker.Value.Year;

            if(bday > DateTime.Now.AddYears(-calcultedAge))
            {
                calcultedAge--;
            }

            if(age != calcultedAge)
            {
                MessageBox.Show("The age you entered does not match the birthdate. Please check your inputs.", "Input Error");
                return;
            }

            if (!IsPasswordStrong(pass)) 
            { 
                MessageBox.Show("Password must be at least 8 characters long and contain at least one uppercase letter and one number.", "Input Error");
                return;
            }

            if (!int.TryParse(AgetxtBox.Text, out age))
            {
                MessageBox.Show("Please enter a valid number for age.", "Input Error");
                return;
            }

            bool success = DatabaseManager.RegisterPlayer(user, pass, age, bday.ToShortDateString(), gender);

            if (success)
            {
                MessageBox.Show("Welcome to Montmauve! Account created.", "Success");
                
                MainMenu mainMenu = new MainMenu();
                mainMenu.Show();
                this.Hide();
            }
        }


        private void SignUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                SignUp2Btn_Click(this, new EventArgs());
            }
        }

        private bool IsPasswordStrong(string pw)
        {
            if (pw.Length < 8)
            {
                return false;
            }

            bool hasUpper = false;
            bool hasNumber = false;

            foreach(char c in pw)
            {
                if (char.IsUpper(c))
                {
                    hasUpper = true;
                }
                if(char.IsDigit(c))
                {
                    hasNumber = true;
                }
            }

            return hasUpper && hasNumber;
        }

    }
}
