using GalaSoft.MvvmLight.Command;
using GoalMaster.Helpers;
using GoalMaster.Model;
using GoalMaster.View;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LiveCharts.Events;
using LiveCharts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;
using System.Globalization;

namespace GoalMaster.ViewModel
{
    class MainUserWindowViewModel : MainViewModel
    {
        public RelayCommand AddNewGoalCommand { get; set; }
        public RelayCommand FriendsCommand { get; set; }
        public RelayCommand AddNewActivityCommand { get; set; }
        public RelayCommand OptionsCommand { get; set; }
        public RelayCommand EditOrDeleteGoalCommand { get; set; }
        public RelayCommand<object> DataClickCommand { get; set; }
        //public CustomCommand<ChartPoint> DataClickCommand { get; set; }
        public event EventHandler CloseWindowEvent;

        public MainUserWindowViewModel(User user, EventHandler closeEvent)
        {
            User = user;
            AddNewGoalCommand = new RelayCommand(AddnewGoal);
            FriendsCommand = new RelayCommand(Friends);
            AddNewActivityCommand = new RelayCommand(AddNewActivity);
            OptionsCommand = new RelayCommand(Options);
            DataClickCommand = new RelayCommand<object>(DataClick);
            EditOrDeleteGoalCommand = new RelayCommand(EditOrDeleteGoal);

             SelectedDate = DateTime.Today;
            GoalRecords = new ObservableCollection<GoalRecord>();
            CloseWindowEvent += closeEvent;
            UserInfo = $"User info: {User.Login} {User.Mail}";

            //DataClickCommand = new CustomCommand<ChartPoint>
            //{
            //      ExecuteDelegate = p => MessageBox_Show(null, $"Kliknięto {p.X} {p.Y} ", "")
            //};


            //TODO sprawdzać internet
            using (var db = new GoalMasterDatabaseContext())
            {
                var result = db.Users.FirstOrDefault(x => x.ID == user.ID).GoalDefinitions;//db.GoalDefinitions.Where(x => x.Users.Contains(user));//
                GoalDefinitions = new ObservableCollection<GoalDefinition>(result);
                if (GoalDefinitions.Count <= 0)
                {
                    AddnewGoal();
                }
                else
                {
                    FirstGoalDefinition = GoalDefinitions.First();
                }
                refreshGoalRecords();
            }

        }

        private void EditOrDeleteGoal()
        {
            var editOrDeleteGoalWindow = new EditOrDeleteGoalDefWindow(FirstGoalDefinition,User);
            editOrDeleteGoalWindow.ShowDialog();
            using (var db = new GoalMasterDatabaseContext())
            {
                var result = db.Users.FirstOrDefault(x => x.ID == User.ID).GoalDefinitions;//db.GoalDefinitions.Where(x => x.Users.Contains(user));//
                GoalDefinitions = new ObservableCollection<GoalDefinition>(result);
                if (GoalDefinitions.Count <= 0)
                {
                    AddnewGoal();
                }
                else
                {
                    FirstGoalDefinition = GoalDefinitions.First();
                }
                refreshGoalRecords();
            }
            //throw new NotImplementedException();
        }

        private void DataClick(object obj)
        {
            var point = (ChartPoint)obj;
            var userName = point.SeriesView.Title;

            var serie = GoalRecords2.FirstOrDefault(view => view.Title == userName);

            var date = Dates.ElementAt((int)point.X);
            date=  date.Replace('.', '-');
            DateTime result = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            string note;
            using (var db = new GoalMasterDatabaseContext())
            {
                var user = db.Users.FirstOrDefault(us => us.Login == userName);
                var goalRecord = db.GoalRecords.FirstOrDefault(gr => gr.Date == result
                && gr.User.ID == user.ID);
                 note = goalRecord.Note;
            }

                MessageBox_Show(null, $"{note}", "Note");
        }

        private void Options()
        {
            var optionsWindow = new OptionsWindow(User);
            optionsWindow.ShowDialog();
        }

        private User _user;

        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                RaisePropertyChanged("User");
            }
        }

        private string _userInfo;

        public string UserInfo
        {
            get { return _userInfo; }
            set
            {
                _userInfo = value;
                RaisePropertyChanged("UserInfo");
            }
        }
        private ObservableCollection<GoalDefinition> _goalDefinitions;

        public ObservableCollection<GoalDefinition> GoalDefinitions
        {
            get { return _goalDefinitions; }
            set
            {
                _goalDefinitions = value;
                RaisePropertyChanged("GoalDefinitions");
            }
        }
        private GoalDefinition _firstGoalDefinition;

        public GoalDefinition FirstGoalDefinition
        {
            get
            {
                return _firstGoalDefinition;
            }
            set
            {
                _firstGoalDefinition = value;
                RaisePropertyChanged("FirstGoalDefinition");
                refreshGoalRecords();
            }
        }

        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                RaisePropertyChanged("SelectedDate");

            }
        }

        private void Friends()
        {
            try
            {
                var friendsWindow = new FriendsWindow(this.User);
                friendsWindow.ShowDialog();
            }
            catch
            {

            }
         }

        private void AddnewGoal()
        {
            

            var addGoalDefinition = new AddGoalDefinitionWindow(User);
          

            addGoalDefinition.ShowDialog();
           
            refreshGoalRecords();
            if (CloseWindowEvent != null)
                CloseWindowEvent(this, null);
        }
        private void AddNewActivity()
        {
            if (FirstGoalDefinition == null) return;
            var addGoalRecordWindow = new AddGoalRecordWindow(User, FirstGoalDefinition, DateTime.Today);//SelectedDate);
            addGoalRecordWindow.ShowDialog();
            //TODO change datetime, check GoalDefinitoon
            refreshGoalRecords();
        }

        private void refreshGoalRecords()
        {
            using (var db = new GoalMasterDatabaseContext())
            {
                var resultGoals = db.GoalRecords.Where(x =>
                x.GoalDefinition.ID == FirstGoalDefinition.ID); //db.Users.FirstOrDefault(x => x.ID == User.ID).GoalRecords.Where(x => x.GoalDefinition.ID == FirstGoalDefinition.ID);
                //if (resultGoals == null)
                //    return;
                //GoalRecords = new ObservableCollection<GoalRecord>(resultGoals);
                GoalRecords2 = new SeriesCollection();

                if (FirstGoalDefinition == null)
                    return;
                var goalDefinitionUsers = db.GoalDefinitions.FirstOrDefault(x =>
                x.ID == FirstGoalDefinition.ID).Users;

                Dates = new ObservableCollection<string>();


                foreach (var goal in resultGoals)
                {
                    if (!Dates.Contains(goal.OnlyDate))
                    {
                        Dates.Add(goal.OnlyDate);
                    }
                }
                var orderedList = Dates.OrderBy(x => DateTime.Parse(x)).ToList();
                Dates = new ObservableCollection<string>(orderedList);

                foreach (var user in goalDefinitionUsers)
                {
                    var lineSerie = new LiveCharts.Wpf.LineSeries();
                    lineSerie.Title = user.Login;
                    lineSerie.DataLabels = true;

                    var userGoals = db.GoalRecords.Where(x =>
                    x.User.ID == user.ID &&
                    x.GoalDefinition.ID == FirstGoalDefinition.ID);
                    lineSerie.Values = new ChartValues<ObservablePoint>();

                    for (int i=0;i<Dates.Count;i++)
                    {
                        bool isAdded = false;
                        foreach (var goal in userGoals)
                        {
                            if (Dates[i] == goal.OnlyDate)
                            {
                                lineSerie.Values.Add(new ObservablePoint(i, goal.Value));
                                break;
                            }
                        }
                    }

                    lineSerie.LineSmoothness = 0;
                    GoalRecords2.Add(lineSerie);
                }
            }
        }
        private ObservableCollection<GoalRecord> _goalRecords;

        public ObservableCollection<GoalRecord> GoalRecords
        {
            get { return _goalRecords; }
            set
            {
                _goalRecords = value;
                RaisePropertyChanged("GoalRecords");//todo tylko data w wyświetlaniu
            }
        }
        private SeriesCollection _goalRecords2;

        public SeriesCollection GoalRecords2
        {
            get { return _goalRecords2; }
            set
            {
                _goalRecords2 = value;
                RaisePropertyChanged("GoalRecords2");
            }
        }
        private ObservableCollection<string> _dates;

        public ObservableCollection<string> Dates
        {
            get { return _dates; }
            set
            {
                _dates = value;
                RaisePropertyChanged("Dates");
            }
        }

        //public string[] Dates { get; set; }



    }
}
