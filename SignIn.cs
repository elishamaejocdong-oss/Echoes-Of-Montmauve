using System.Media;
using Echoes_of_Montmauve.GameLogic;
namespace Echoes_of_Montmauve
{
    public partial class SignIn : Form
    {
        private SoundPlayer _bgMusic;

        private LogIn logInForm;

        public SignIn()
        {
            InitializeComponent();
            this.Opacity = 0; // Start invisible for fade-in effect
            UIHelper.AddButtonScaleEffect(SignInBtn);
            UIHelper.AddButtonScaleEffect(SignUpBtn);
            UIHelper.AddButtonScaleEffect(ExitBtn);
            _bgMusic = new SoundPlayer(Properties.Resources.SignInBGM);
        }

        private void SignIn_Load(object sender, EventArgs e)
        {
            using(SplashForm splash = new SplashForm())
            {
                splash.ShowDialog();
            }

            this.Opacity = 100;
            this.Refresh();
            _bgMusic.PlayLooping();
        }

        private void SignInBtn_Click(object sender, EventArgs e)
        {
            LogIn logInForm = new LogIn();
            this.Hide();
            logInForm.Show();
        }

        private void SignUpBtn_Click(object sender, EventArgs e)
        {
            SignUp signUpForm = new SignUp();
            this.Hide();
            signUpForm.Show();
        }

        private void SignIn_Shown(object sender, EventArgs e)
        {
        }
    }
}
