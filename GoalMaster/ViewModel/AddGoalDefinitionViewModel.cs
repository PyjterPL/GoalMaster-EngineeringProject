using GalaSoft.MvvmLight.Command;
using GoalMaster.Model;
using GoalMaster.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalMaster.ViewModel
{
    class AddGoalDefinitionViewModel : MainViewModel
    {
        public RelayCommand AddNewGoalCommand { get; set; }
        public RelayCommand AddFriendCommand { get; set; }
        public RelayCommand RemoveFriendCommand { get; set; }
        

        public event EventHandler CloseWindowEvent;
        public AddGoalDefinitionViewModel(User user)
        {
            User = user;
            AddNewGoalCommand = new RelayCommand(AddNewGoal);
            AddFriendCommand = new RelayCommand(AddFriend);
            RemoveFriendCommand = new RelayCommand(RemoveFriend);
            Friends = new ObservableCollection<User>();
            InvitedFriends = new ObservableCollection<User>();

            
            using (var db = new GoalMasterDatabaseContext())
            {
                var result = db.GoalTypes;
                GoalTypes = new ObservableCollection<GoalType>(result);
                
                FirstGoalType = GoalTypes.First();
            }
            refreshListsOfFriends();
            SelectedFriend = Friends.FirstOrDefault();
            SelectedInvitedFriend = InvitedFriends.FirstOrDefault();
        }


        private void RemoveFriend()
        {
            if (SelectedInvitedFriend == null) return;
            Friends.Add(SelectedInvitedFriend);
            InvitedFriends.Remove(SelectedInvitedFriend);
        }

        private void AddFriend()
        {
            if (SelectedFriend == null) return;
            InvitedFriends.Add(SelectedFriend);
            Friends.Remove(SelectedFriend);
        }

        private void AddNewGoal()
        {
            //todo Zrobić dodawanie do goalDefinitions zaproszonych userów us.GoalDefinitions.Add///
            GoalDefinition goalDefinition = new GoalDefinition();
            if (InvitedFriends.Count == 0)
            {
                goalDefinition.Shared = false;
            }
            else
            {
                goalDefinition.Shared = true;
            }

            goalDefinition.Name = Name;
            goalDefinition.Description = Description;
            
            using (var db = new GoalMasterDatabaseContext())
            {

                var gt = db.GoalTypes.FirstOrDefault(x => x.ID == FirstGoalType.ID);
                var us = db.Users.FirstOrDefault(x => x.ID == User.ID);
               //Important 
                db.GoalDefinitions.Add(goalDefinition);
                db.SaveChanges();
                goalDefinition.OwnerUserID = us;
                goalDefinition.GoalType = gt;
                goalDefinition.Users.Add(us);
                

                //friends  todo!

                //us.GoalDefinitions.Add(goalDefinition);
                //db.SaveChanges();
                foreach (var invitedFriend in InvitedFriends)
                {
                    var friend = db.Users.FirstOrDefault(x => x.ID == invitedFriend.ID);
                    goalDefinition.Users.Add(friend);
                    //var goalDef = new GoalDefinition();
                    //friend.GoalDefinitions.Add(goalDef);
                    //goalDef = goalDefinition;
                    //friend.GoalDefinitions.Add(goalDefinition); //dodaje tylko do ostatniego usera

                    //db.SaveChanges();
                }
                db.SaveChanges();
            }
            if (CloseWindowEvent != null)
                CloseWindowEvent(this, null);
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
        private ObservableCollection<GoalType> _goalTypes;

        public ObservableCollection<GoalType> GoalTypes
        {
            get { return _goalTypes; }
            set
            {
                _goalTypes = value;
                RaisePropertyChanged("GoalType");
            }
        }
        private GoalType _firstGoalType;

        public GoalType FirstGoalType
        {
            get { return _firstGoalType; }
            set
            {
                _firstGoalType = value;
                RaisePropertyChanged("FirstGoalType");
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }

        private User _selectedFriend;

        public User SelectedFriend
        {
            get { return _selectedFriend; }
            set { _selectedFriend = value;
                RaisePropertyChanged("SelectedFriend");
            }
        }

        private ObservableCollection<User> _friends;

        public ObservableCollection<User> Friends
        {
            get { return _friends; }
            set
            {
                _friends = value;
                RaisePropertyChanged("Friends");
            }
        }

        private User _selectedInvitedFriend;

        public User SelectedInvitedFriend
        {
            get { return _selectedInvitedFriend; }
            set { _selectedInvitedFriend = value;
                RaisePropertyChanged("SelectedInvitedFriend");
            }
        }

        private ObservableCollection<User> _invitedFriends;

        public ObservableCollection<User> InvitedFriends
        {
            get { return _invitedFriends; }
            set
            {
                _invitedFriends = value;
                RaisePropertyChanged("InvitedFriends");
            }
        }
        public void BackToMainUserWindow(object sender, CancelEventArgs e)
        {
            //if (CloseWindowEvent != null)
            //    CloseWindowEvent(this, null);
            var userWindow = new MainUserWindow(User);
            userWindow.Show();

        }

        private void refreshListsOfFriends()
        {
            Friends.Clear();

            using (var db = new GoalMasterDatabaseContext())
            {
                var user = db.Users.FirstOrDefault(x => x.ID == User.ID);
                var relations = db.Relationships.Where(x => (x.UserOne.ID == user.ID ||
                x.UserTwo.ID == user.ID) && 
                x.Status.ID == (int)RelationshipStatusDefined.Accepted
                );
                var relatedUsers = new List<User>();

                var friends = new List<User>();

                foreach (var relation in relations)
                {
                    if (relation.UserOne != user)
                    {
                        friends.Add(relation.UserOne);
                        //relatedUsers.Add(relation.UserOne);
                        //friends.Add(new Tuple<string, string>(relation.UserOne.Login, relation.Status.Description));
                    }

                    if (relation.UserTwo != user)
                    {
                        friends.Add(relation.UserTwo);
                        //    relatedUsers.Add(relation.UserTwo);
                        //    friends.Add(new Tuple<string, string>(relation.UserTwo.Login, relation.Status.Description));
                    }

            }
                //_users = db.Users.Where(x => x.Login != User.Login).ToList();

                //_users = _users.Except(relatedUsers).ToList();

                //PossibleFriends = new ObservableCollection<User>(_users);
                Friends = new ObservableCollection<User>(friends);
            }
        }
    }
}
