using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using GoalMaster.View;
using GoalMaster.Model;
using GoalMaster.Helpers;
using System.Windows.Threading;
using System.Data.Entity;

namespace GoalMaster.ViewModel
{
    class WelcomeWindowViewModel : MainViewModel
    {
        public event EventHandler CloseWindowEvent;
        public RelayCommand SignInCommand { get; set; }
        public RelayCommand SignUpCommand { get; set; }


        public WelcomeWindowViewModel()
        {
            SignInCommand = new RelayCommand(SignIn);
            SignUpCommand = new RelayCommand(SignUp);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(InternetChecker);
            timer.Start();
        }

        private void InternetChecker(object sender, EventArgs e)
        {
            IsConnectionOn = InternetAvailability.IsInternetAvailable();
        }

        private string _loginOrMail;

        public string LoginOrMail
        {
            get { return _loginOrMail; }
            set
            {
                _loginOrMail = value;
                RaisePropertyChanged("LoginOrMail");
            }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }
        private string _userValidation;

        public string UserValidation
        {
            get { return _userValidation; }
            set
            {
                _userValidation = value;
                RaisePropertyChanged("UserValidation");
            }
        }
        private string _isInternetConnection;

        public string IsInternetConnection
        {
            get
            {
                if (!IsConnectionOn)
                {
                    _isInternetConnection = "Internet connection lost!";
                }
                else
                {
                    _isInternetConnection = "";
                }
                return _isInternetConnection;
            }
            set
            {
                RaisePropertyChanged("IsInternetConnection");
            }
        }

        private bool _isConnectionOn;

        public bool IsConnectionOn
        {
            get { return _isConnectionOn; }
            set
            {
                _isConnectionOn = value;
                RaisePropertyChanged("IsInternetConnection");
                RaisePropertyChanged("IsConnectionOn");
            }
        }





        private void SignUp()
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            if (CloseWindowEvent != null)
                CloseWindowEvent(this, null);
        }

        private async void SignIn()
        {
            using (var db = new GoalMasterDatabaseContext())
            {
                var encodedPassword = new RijndaelCrypter().Encode(Password);
                var user =await db.Users.FirstOrDefaultAsync(x => (x.Password == encodedPassword &&
                x.Login == LoginOrMail)
                ||
                (x.Mail == LoginOrMail &&
                x.Password == encodedPassword));

                //    await db.Users.Where(x => (x.Password == encodedPassword &&
                //x.Login == LoginOrMail)
                //||
                //(x.Mail == LoginOrMail &&
                //x.Password == encodedPassword)).FirstOrDefaultAsync();




                if (user != null)
                {
                    UserValidation = "Succes!";
                    var userMainWindow = new MainUserWindow(user);
                    if(!userMainWindow.IsClosed)//todo  
                    userMainWindow.Show();
                    if (CloseWindowEvent != null)
                        CloseWindowEvent(this, null);
                }
                else
                {
                    UserValidation = "Login or password wrong";
                }

            }

        }


    }
}
