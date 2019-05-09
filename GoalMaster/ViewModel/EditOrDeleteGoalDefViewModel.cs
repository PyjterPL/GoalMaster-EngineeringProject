using GalaSoft.MvvmLight.Command;
using GoalMaster.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalMaster.ViewModel
{
    public class EditOrDeleteGoalDefViewModel : MainViewModel
    {
        public event EventHandler CloseWindowEvent;
        public event EventHandler MessageBoxRequest;
        public RelayCommand DeleteGoalCommand { get; set; }
        public RelayCommand EditGoalCommand { get; set; }
        public RelayCommand AddFriendCommand { get; set; }
        public RelayCommand RemoveFriendCommand { get; set; }

        private GoalDefinition _goalDefinition { get; set; }
        public EditOrDeleteGoalDefViewModel(GoalDefinition goalDefinition,User user)
        {
            DeleteGoalCommand = new RelayCommand(DeleteGoal);
            EditGoalCommand = new RelayCommand(EditGoal);
            RemoveFriendCommand = new RelayCommand(RemoveFriend);
            AddFriendCommand = new RelayCommand(AddFriend);

            Friends = new ObservableCollection<User>();
            InvitedFriends = new ObservableCollection<User>();

            User = user;

            using (var db = new GoalMasterDatabaseContext())
            {
                var goalDef = db.GoalDefinitions.FirstOrDefault(gd => gd.ID == goalDefinition.ID);
                _goalDefinition = goalDef;
                Name = goalDef.Name;
                Description = goalDef.Description;

                if(goalDef.OwnerUserID.ID == user.ID)
                {
                    IsOwner = true;
                    DelOrUnsubscribeMessage = "Delete goal";
                }
                else
                {
                    IsOwner = false;
                    DelOrUnsubscribeMessage = "Unsubscribe from goal";
                    Info = "Only goal owner can edit invited friends";
                }
            }

            refreshListsOfFriends();
            SelectedFriend = Friends.FirstOrDefault();
            SelectedInvitedFriend = InvitedFriends.FirstOrDefault();
        }

        private void EditGoal()
        {
            using (var db = new GoalMasterDatabaseContext())
            {
                var goalDef = db.GoalDefinitions.FirstOrDefault(gd => gd.ID == _goalDefinition.ID);
                var user = db.Users.FirstOrDefault(x => x.ID == User.ID);
                goalDef.Name = Name;
                goalDef.Description = Description;
                goalDef.Users.Clear();
                goalDef.Users.Add(user);
                foreach (var invitedFriend in InvitedFriends)
                {
                    var friend = db.Users.FirstOrDefault(x => x.ID == invitedFriend.ID);
                    goalDef.Users.Add(friend);
                }

                db.SaveChanges();
            }
            if (this.CloseWindowEvent != null)
                CloseWindowEvent(this, null);
        }

        private void DeleteGoal()
        {
            using (var db = new GoalMasterDatabaseContext())
            {
                if (IsOwner)
                {
                    var goalDef = db.GoalDefinitions.FirstOrDefault(gd => gd.ID == _goalDefinition.ID);
                    var goalRecords = db.GoalRecords.Where(gr => gr.GoalDefinition.ID == goalDef.ID);
                    db.GoalRecords.RemoveRange(goalRecords);
                    db.GoalDefinitions.Remove(goalDef);
                    db.SaveChanges();
                }
                else
                {
                    var goalDef = db.GoalDefinitions.FirstOrDefault(gd => gd.ID == _goalDefinition.ID);
                    var user = db.Users.FirstOrDefault(x => x.ID == User.ID);
                    goalDef.Users.Remove(user);
                    db.SaveChanges();
                    //unsubscribe totdo
                }
            }
            if (this.CloseWindowEvent != null)
                CloseWindowEvent(this, null);
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

                var invitedUsers = db.GoalDefinitions.FirstOrDefault(x => x.ID == _goalDefinition.ID).Users.Where(x => x.ID != User.ID);
                var relatedUsers = new List<User>();

                var friends = new List<User>();

                foreach (var relation in relations)
                {
                    if (relation.UserOne != user)
                    {
                        friends.Add(relation.UserOne);
                    }

                    if (relation.UserTwo != user)
                    {
                        friends.Add(relation.UserTwo);
                    }

                }
                friends = friends.Except(invitedUsers).ToList();
                Friends = new ObservableCollection<User>(friends);
                InvitedFriends = new ObservableCollection<User>(invitedUsers);
            }
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
        private User user;

        public User User
        {
            get { return user; }
            set { user = value; }
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
            set
            {
                _selectedFriend = value;
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
            set
            {
                _selectedInvitedFriend = value;
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
        private bool _isOwner;

        public bool IsOwner
        {
            get { return _isOwner; }
            set
            {
                _isOwner = value;
                RaisePropertyChanged("IsOwner");
            }
        }
        private string _info;

        public string Info
        {
            get { return _info; }
            set { _info = value;
                RaisePropertyChanged("Info");
            }
        }
        private string _delOrUnsubMess;

        public string DelOrUnsubscribeMessage
        {
            get { return _delOrUnsubMess; }
            set { _delOrUnsubMess = value;
                RaisePropertyChanged("DelOrUnsubscribeMessage");
            }
        }




    }
}
