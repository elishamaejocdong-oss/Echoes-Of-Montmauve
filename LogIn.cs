using Echoes_of_Montmauve.Admin;
using Echoes_of_Montmauve.GameLogic;
using Echoes_of_Montmauve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Echoes_of_Montmauve
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
            LogInPnl.BackColor = Color.FromArgb(140, Color.PeachPuff);

            PasswordTxtBox.UseSystemPasswordChar = true;
        }

        private void LogInBtn_Click(object sender, EventArgs e)
        {
            string user = UsernameTxtBox.Text.Trim();
            string pass = PasswordTxtBox.Text.Trim();

            // Debug: Check if fields are empty
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            try
            {
                if (user == "admin_acc123" && pass== "Montmauve2026!")
                {
                    MessageBox.Show("Administrator Access Granted.", "System Access");

                    // Open the Admin Form
                    AdminMenu adminUI = new AdminMenu();
                    adminUI.Show();

                    this.Hide(); // Hide the login screen
                    return; // Stop execution so it doesn't try to log in as a player
                }

                if (DatabaseManager.ValidateLogin(user, pass))
                {
                    MainMenu mainMenu = new MainMenu();
                    mainMenu.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Login Failed: Username or Password not found in database.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("System Error: " + ex.Message);
            }
        }

        private void ForgotPasswordBtn_Click(object sender, EventArgs e)
        {
            ForgotPasswordForm forgotPasswordForm = new ForgotPasswordForm();
            forgotPasswordForm.ShowDialog();
        }
    }
}
