using GalaSoft.MvvmLight.Command;
using GoalMaster.Helpers;
using GoalMaster.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GoalMaster.ViewModel
{
    class AddGoalRecordViewModel : MainViewModel
    {
        private int _goalTypeID = 0;
        private bool isInEditMode = false;
        double editModeValue;
        private GoalRecord existingGoalRecord = null;
        public RelayCommand AddNewGoalRecordCommand { get; set; }
        public RelayCommand DeleteGoalRecordCommand { get; set; }
        public event EventHandler CloseWindowEvent;

        public AddGoalRecordViewModel(User user, GoalDefinition goalDefinition, DateTime dateTime)
        {
            SelectedDate = dateTime;
            ActivityBtnMessage = "Add activity";
            using (var db = new GoalMasterDatabaseContext())
            {
                var goalDef = db.GoalDefinitions.FirstOrDefault(
                    x => x.ID == goalDefinition.ID);
                existingGoalRecord = db.GoalRecords.FirstOrDefault(x => x.Date == SelectedDate
                && x.GoalDefinition.ID == goalDefinition.ID &&
                x.User.ID == user.ID);
                if (existingGoalRecord != null)
                {
                    isInEditMode = true;
                    ActivityBtnMessage = "Edit activity";
                    editModeValue = existingGoalRecord.Value;
                    Note = existingGoalRecord.Note;
                }
                GoalDefinition = goalDef;
                GoalDefinition.GoalType = goalDef.GoalType;

            }

            User = user;

            AddNewGoalRecordCommand = new RelayCommand(AddNewGoalRecord);
            DeleteGoalRecordCommand = new RelayCommand(DeleteGoalRecord);

            _goalTypeID = GoalDefinition.GoalType.ID;

            if (GoalDefinition.GoalType.ID == 1)//DoneOrNotDone
            {
                VisibilityValueBox = Visibility.Hidden;
                VisibilityBoolBox = Visibility.Visible;

                TrueFalseOptions = new ObservableCollection<BoolHelper>();
                TrueFalseOptions.Add(new BoolHelper(true, "Done"));
                TrueFalseOptions.Add(new BoolHelper(false, "Not done"));
                if (isInEditMode)
                {
                    TrueOption = TrueFalseOptions.FirstOrDefault(x => x.Value == Convert.ToBoolean(editModeValue));//editModeValue ? 1 : 0;
                }
                else
                {
                    TrueOption = TrueFalseOptions.First();
                }


            }
            else//Numeric values
            {
                VisibilityValueBox = Visibility.Visible;
                VisibilityBoolBox = Visibility.Hidden;
                if (isInEditMode)
                {
                    _value = editModeValue.ToString();
                }
            }


        }


        #region Functions

        private void DeleteGoalRecord()
        {
            if (existingGoalRecord != null)
            {
                using (var db = new GoalMasterDatabaseContext())
                {
                    var record = db.GoalRecords.FirstOrDefault(x => x.ID == existingGoalRecord.ID);
                    if(record == null)
                    {
                        MessageBox.Show("No record selected");
                        return;
                    }
                    db.GoalRecords.Remove(record);
                    db.SaveChanges();
                }
                Value = null;
                Note = null;
                if (CloseWindowEvent != null)
                    CloseWindowEvent(this, null);
            }
            else
            {
                MessageBox.Show("No record selected");
            }
            //throw new NotImplementedException();
        }

        private void AddNewGoalRecord()
        {


            if (isInEditMode)
            {
                using (var db = new GoalMasterDatabaseContext())
                {
                    var result = db.GoalRecords.FirstOrDefault(x => x.ID == existingGoalRecord.ID);
                    if (_goalTypeID == 1)
                    {
                        result.Value = TrueOption.Value ? 1 : 0;

                    }
                    else
                    {
                        if (NumericHelper.isLastLetterDotOrComma(Value))
                        {
                            ErrorMessage = "Invalid numeric value!";
                            return;
                        }

                        result.Value = Double.Parse(Value.Replace('.', ','));
                    }
                    result.Note = Note;
                    db.SaveChanges();
                }
            }
            else
            {
                var goalRecord = new GoalRecord();
                goalRecord.Date = SelectedDate;
                goalRecord.Note = Note;
                if (_goalTypeID == 1)
                {
                    goalRecord.Value = TrueOption.Value ? 1 : 0;
                }
                else
                {
                    if (NumericHelper.isLastLetterDotOrComma(Value))
                    {
                        ErrorMessage = "Invalid numeric value!";
                        return;
                    }
                    goalRecord.Value = Double.Parse(Value.Replace('.', ','));
                }
                using (var db = new GoalMasterDatabaseContext())
                {
                    db.GoalRecords.Add(goalRecord);
                    goalRecord.User = db.Users.FirstOrDefault(x => x.ID == User.ID);
                    goalRecord.GoalDefinition = db.GoalDefinitions.FirstOrDefault(x => x.ID == GoalDefinition.ID);
                    db.SaveChanges();
                }
            }
            if (CloseWindowEvent != null)
                CloseWindowEvent(this, null);
        }

        #endregion


        #region Properties
        private string _note;

        public string Note
        {
            get { return _note; }
            set
            {
                _note = value;
                RaisePropertyChanged("Note");
            }
        }

        private DateTime _selectedDate;

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                //todo set a noew and value
                _selectedDate = value;
                RaisePropertyChanged("SelectedDate");

                if (_goalDefinition == null)
                    return;

                using (var db = new GoalMasterDatabaseContext())
                {
                    var goalDef = db.GoalDefinitions.FirstOrDefault(
                        x => x.ID == _goalDefinition.ID);
                    existingGoalRecord = db.GoalRecords.FirstOrDefault(x => x.Date == SelectedDate
                    && x.GoalDefinition.ID == _goalDefinition.ID &&
                    x.User.ID == _user.ID);
                    if (existingGoalRecord != null)
                    {
                        isInEditMode = true;
                        editModeValue = existingGoalRecord.Value;
                        Note = existingGoalRecord.Note;
                        ActivityBtnMessage = "Edit activity";
                    }
                    else
                    {
                        isInEditMode = false;
                        Value = null;
                        Note = null;
                        ActivityBtnMessage = "Add activity";
                        return;
                    }
                    GoalDefinition = goalDef;
                    GoalDefinition.GoalType = goalDef.GoalType;

                }


                _goalTypeID = GoalDefinition.GoalType.ID;

                if (GoalDefinition.GoalType.ID == 1)//DoneOrNotDone
                {
                    VisibilityValueBox = Visibility.Hidden;
                    VisibilityBoolBox = Visibility.Visible;

                    TrueFalseOptions = new ObservableCollection<BoolHelper>();
                    TrueFalseOptions.Add(new BoolHelper(true, "Done"));
                    TrueFalseOptions.Add(new BoolHelper(false, "Not done"));
                    if (isInEditMode)
                    {
                        TrueOption = TrueFalseOptions.FirstOrDefault(x => x.Value == Convert.ToBoolean(editModeValue));//editModeValue ? 1 : 0;
                    }
                    else
                    {
                        TrueOption = TrueFalseOptions.First();
                    }


                }
                else//Numeric values
                {
                    VisibilityValueBox = Visibility.Visible;
                    VisibilityBoolBox = Visibility.Hidden;
                    if (isInEditMode)
                    {
                        Value = editModeValue.ToString();
                    }
                }


            }
        }



        private GoalDefinition _goalDefinition;

        public GoalDefinition GoalDefinition
        {
            get { return _goalDefinition; }
            set
            {
                _goalDefinition = value;
                RaisePropertyChanged("GoalDefinition");
            }
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
        private Visibility _visibilityValueBox;

        public Visibility VisibilityValueBox
        {
            get { return _visibilityValueBox; }
            set
            {
                _visibilityValueBox = value;
                RaisePropertyChanged("VisibilityValueBox");
            }
        }
        private Visibility _visibilityBoolBox;

        public Visibility VisibilityBoolBox
        {
            get { return _visibilityBoolBox; }
            set
            {
                _visibilityBoolBox = value;
                RaisePropertyChanged("VisibilityBoolBox");
            }
        }
        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                RaisePropertyChanged("ErrorMessage");
            }
        }


        private ObservableCollection<BoolHelper> _trueFalseOptions;

        public ObservableCollection<BoolHelper> TrueFalseOptions
        {
            get { return _trueFalseOptions; }
            set
            {
                _trueFalseOptions = value;
                RaisePropertyChanged("TrueFalseOptions");
            }
        }
        private BoolHelper _trueOption;

        public BoolHelper TrueOption
        {
            get { return _trueOption; }
            set
            {
                _trueOption = value;
                RaisePropertyChanged("TrueOption");
            }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                if (NumericHelper.IsTextNumericOnly(value))
                {
                    _value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }
        private string btnActivityMessage;

        public string ActivityBtnMessage
        {
            get { return btnActivityMessage; }
            set { btnActivityMessage = value;
                RaisePropertyChanged("ActivityBtnMessage");
            }
        }



        #endregion
    }
}
