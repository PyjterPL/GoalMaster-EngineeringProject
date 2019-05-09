using GalaSoft.MvvmLight.Command;
using GoalMaster.Helpers;
using GoalMaster.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace GoalMaster.ViewModel
{
    class RegisterWindowViewModel : MainViewModel
    {
        public event EventHandler CloseWindowEvent;
        public RelayCommand RegisterCommand { get; set; }
        public RelayCommand BackCommand { get; set; }

        public RegisterWindowViewModel()
        {
            RegisterCommand = new RelayCommand(Registration);
            BackCommand = new RelayCommand(Back);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(InternetChecker);
            timer.Start();
        }

        private void InternetChecker(object sender, EventArgs e)
        {
            IsConnectionOn = InternetAvailability.IsInternetAvailable();
        }

        private string _login;

        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    RaisePropertyChanged("LoginValidation");
                    RaisePropertyChanged("Login");
                }
            }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    PasswordStrength = "Your password is " + PasswordAdvisor.CheckStrength(_password).ToString();
                    if (_password != string.Empty)
                    {
                        RaisePropertyChanged("PasswordIdentical");
                    }
                    RaisePropertyChanged("Password");
                }
            }
        }
        private string _passwordRepeated;

        public string PasswordRepeated
        {
            get { return _passwordRepeated; }
            set
            {
                if (_passwordRepeated != value)
                {
                    _passwordRepeated = value;
                    if (_passwordRepeated != string.Empty)
                    {
                        RaisePropertyChanged("PasswordIdentical");
                    }
                    RaisePropertyChanged("PasswordRepeated");
                }
            }
        }
        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    RaisePropertyChanged("MailWalidation");
                    RaisePropertyChanged("Email");
                }
            }
        }
        private string _passwordStrength;

        public string PasswordStrength
        {
            get { return _passwordStrength; }
            set
            {
                _passwordStrength = value;
                RaisePropertyChanged("PasswordStrength");
            }
        }
        private string _passwordIdentical;

        public string PasswordIdentical
        {
            get
            {
                if (Password != null && PasswordRepeated != null)
                {
                    if (arePasswordsIdentical())
                    {
                        _passwordIdentical = "Passwords are the same";
                    }
                    else
                    {
                        _passwordIdentical = "Passwords are different";
                    }
                }
                return _passwordIdentical;
            }
            set
            {
                _passwordIdentical = value;
                RaisePropertyChanged("PasswordIdentical");
            }
        }
        private string _mailWalidation;

        public string MailWalidation
        {
            get
            {
                var thread = new Thread(
                    () =>
                {
                    var res = isMailValidAndFree(Email);
                    if (res.Result)
                    {
                        _mailWalidation = string.Empty;

                    }
                    else
                    {
                        _mailWalidation = "Email is invalid or taken!";
                    }
                });
                thread.Start();
                thread.Join();
                return _mailWalidation;

            }
            set
            {
                _mailWalidation = value;
                RaisePropertyChanged("MailWalidation");
            }
        }
        private string _loginValidation;

        public string LoginValidation
        {
            get
            {
                var thread = new Thread(
                    () =>
                    {
                        var res = isLoginValidAndFree(Login);
                        if (res.Result)
                        {
                            _loginValidation = string.Empty;

                        }
                        else
                        {
                            _loginValidation = "Login is invalid or taken!";
                        }
                    });
                thread.Start();
                thread.Join();
                return _loginValidation;
            }
            set
            {
                _loginValidation = value;
                RaisePropertyChanged("LoginValidation");
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

        private void Registration()
        {
            try
            {
                new Thread(() =>
                {
                    //  Thread.CurrentThread.IsBackground = true;
                    bool passIdentical = arePasswordsIdentical();
                    bool validMail = isMailValidAndFree(Email).Result;
                    bool loginValid = isLoginValidAndFree(Login).Result;

                    if (passIdentical && validMail && loginValid)
                    {
                        using (var db = new GoalMasterDatabaseContext())
                        {
                            var users = db.Users;
                            var user = new User(_login, _password, _email);
                            users.Add(user);
                            db.SaveChangesAsync();
                            MessageBox_Show(null, "Registration complete!", "Info");
                            Back();
                        }
                    }
                    else
                    {
                        MessageBox_Show(null, "Error", "Something went wrong. Try again.");
                    }
                }).Start();

            }
            catch (Exception e)
            {
                MessageBox_Show(null, "Error", e.ToString());
            }
        }
        private void Back()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                var welcomeWindow = new MainWindow();
                welcomeWindow.Show();
                if (CloseWindowEvent != null)
                    CloseWindowEvent(this, null);
            });

        }
        private async Task<bool> isMailValidAndFree(string mail)
        {

            if (mail != null && new RegexUtilities().IsValidEmail(mail))
            {
                using (var db = new GoalMasterDatabaseContext())
                {
                    var usersMails = await db.Users.FirstOrDefaultAsync<User>(x => x.Mail == mail);//(db.Users.Where(x => x.Mail == mail)).FirstOrDefaultAsync<User>();
                    if (usersMails == null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        private async Task<bool> isLoginValidAndFree(string login)
        {
            if (login != null && login.Count() > 3)
            {
                using (var db = new GoalMasterDatabaseContext())
                {
                    var usersLogin = await db.Users.FirstOrDefaultAsync<User>(x => x.Login == login);
                    if (usersLogin == null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool arePasswordsIdentical()
        {
            if (Password == PasswordRepeated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}
