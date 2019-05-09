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
    public class FriendsViewModel : MainViewModel
    {
        public RelayCommand InviteFriendCommand { get; set; }
        public RelayCommand AcceptFriendCommand { get; set; }
        public RelayCommand RejectFriendCommand { get; set; }
        public event EventHandler CloseWindowEvent;
        //AcceptFriendCommand
        public FriendsViewModel(User user)
        {
            User = user;
            InviteFriendCommand = new RelayCommand(InviteFriend);
            AcceptFriendCommand = new RelayCommand(AcceptFriend);
            RejectFriendCommand = new RelayCommand(RejectFriend);
            _users = new List<User>();
            Friends = new ObservableCollection<Tuple<string, string>>();
            InvitesToFriends = new ObservableCollection<Tuple<string, string>>();
            refreshListsOfFriends();
            refreshListsOfInvitesToFriends();
        }

        private void RejectFriend()
        {
            if (SelectedInvitation == null) return;

            using (var db = new GoalMasterDatabaseContext())
            {
                var relationshipStatus = db.RelationshipStatuses.FirstOrDefault(x => x.ID == (int)RelationshipStatusDefined.Declined);

                var user = db.Users.FirstOrDefault(x => x.ID == User.ID);
                var relationship = db.Relationships.FirstOrDefault(
                    x => x.ActionUser.Login == SelectedInvitation.Item1 &&
                    (x.UserOne.ID == user.ID ||
                    x.UserTwo.ID == user.ID));
                relationship.Status = relationshipStatus;

                db.SaveChanges();
            }
            refreshListsOfFriends();
            refreshListsOfInvitesToFriends();
        }

        private void refreshListsOfFriends()
        {
            Friends.Clear();

            using (var db = new GoalMasterDatabaseContext())
            {
                var user = db.Users.FirstOrDefault(x => x.ID == User.ID);
                var relations = db.Relationships.Where(x=>x.UserOne.ID == user.ID ||
                x.UserTwo.ID == user.ID);
                var relatedUsers = new List<User>();

                var friends = new List<Tuple<string, string>>();

                foreach (var relation in relations)
                {
                    if (relation.UserOne != user)
                    {
                        relatedUsers.Add(relation.UserOne);
                        friends.Add(new Tuple<string,string>(relation.UserOne.Login,relation.Status.Description));
                    }
                       
                    if (relation.UserTwo != user)
                    {
                        relatedUsers.Add(relation.UserTwo);
                        friends.Add(new Tuple<string, string>(relation.UserTwo.Login, relation.Status.Description));
                    }
                        
                }
                _users = db.Users.Where(x => x.Login != User.Login).ToList();

                _users = _users.Except(relatedUsers).ToList();

                PossibleFriends = new ObservableCollection<User>(_users);
                Friends = new ObservableCollection<Tuple<string, string>>(friends);
            }
        }

        private void refreshListsOfInvitesToFriends()//todo niech nie pokazuje nawzajem się zaporoszą
        {
            InvitesToFriends.Clear();

            using (var db = new GoalMasterDatabaseContext())
            {
                var user = db.Users.FirstOrDefault(x => x.ID == User.ID);
                var relations = db.Relationships;
                var relatedUsers = new List<User>();

                var invitesToFriends = new List<Tuple<string, string>>();

                foreach (var relation in relations)
                {
                    if(relation.Status.ID == (int)RelationshipStatusDefined.Pending && relation.ActionUser.ID != user.ID)
                    {
                        if (relation.UserOne == user || relation.UserTwo == user)
                        {
                            if (relation.UserOne != user)
                            {
                                relatedUsers.Add(relation.UserOne);
                                invitesToFriends.Add(new Tuple<string, string>(relation.UserOne.Login, relation.Status.Description));
                            }

                            if (relation.UserTwo != user)
                            {
                                relatedUsers.Add(relation.UserTwo);
                                invitesToFriends.Add(new Tuple<string, string>(relation.UserTwo.Login, relation.Status.Description));
                            }
                        }
                    }
                   

                }

                InvitesToFriends = new ObservableCollection<Tuple<string, string>>(invitesToFriends);
                SelectedInvitation = InvitesToFriends.FirstOrDefault();
            }
        }
        private void InviteFriend()
        {
            var relationship = new Relationship();
            using(var db = new GoalMasterDatabaseContext())
            {
                db.Relationships.Add(relationship);
                relationship.ActionUser = db.Users.FirstOrDefault(x => x.ID == User.ID);
                relationship.Status = db.RelationshipStatuses.FirstOrDefault(
                    x => x.ID == (int)RelationshipStatusDefined.Pending);
                relationship.UserOne = db.Users.FirstOrDefault(x => x.ID == User.ID);
                relationship.UserTwo = db.Users.FirstOrDefault(x => x.Login == SelectedPossibleFriend.Login);

                var user = db.Users.FirstOrDefault(x => x.ID == User.ID);
                user.Relations.Add(relationship);
                db.SaveChanges();
            }
            refreshListsOfFriends();
        }
        private void AcceptFriend()
        {
            if (SelectedInvitation == null) return;
            
            using (var db = new GoalMasterDatabaseContext())
            {
                var relationshipStatus = db.RelationshipStatuses.FirstOrDefault(x => x.ID == (int)RelationshipStatusDefined.Accepted);

                var user = db.Users.FirstOrDefault(x => x.ID == User.ID);
                var relationship = db.Relationships.FirstOrDefault(
                    x => x.ActionUser.Login == SelectedInvitation.Item1 &&
                    (x.UserOne.ID == user.ID ||
                    x.UserTwo.ID == user.ID));
                relationship.Status = relationshipStatus;

                db.SaveChanges();
            }
            refreshListsOfFriends();
            refreshListsOfInvitesToFriends();
        }

        private User _selectedPossibleFriend;

        public User SelectedPossibleFriend
        {
            get { return _selectedPossibleFriend; }
            set { _selectedPossibleFriend = value;
                RaisePropertyChanged("SelectedPossibleFriend");
            }
        }


        private ObservableCollection<User> _possibleFriends;

        public ObservableCollection<User> PossibleFriends
        {
            get { return _possibleFriends; }
            set
            {
                _possibleFriends = value;
                RaisePropertyChanged("PossibleFriends");
            }
        }

        private List<User> _users;

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

        private string _searchedValue;

        public string SearchedValue
        {
            get { return _searchedValue; }
            set { _searchedValue = value;
                PossibleFriends = new ObservableCollection<User>(_users.Where(x => x.Login.IndexOf(_searchedValue, StringComparison.OrdinalIgnoreCase) >= 0));
                RaisePropertyChanged("SearchedValue");
            }
        }

        private ObservableCollection<Tuple<string,string>> _friends;

        public ObservableCollection<Tuple<string, string>> Friends
        {
            get { return _friends; }
            set
            {
                _friends = value;
                RaisePropertyChanged("Friends");
            }
        }
        private ObservableCollection<Tuple<string, string>> _invitesToFriends;

        public ObservableCollection<Tuple<string, string>> InvitesToFriends
        {
            get { return _invitesToFriends; }
            set
            {
                _invitesToFriends = value;
                RaisePropertyChanged("InvitesToFriends");
            }
        }
        private Tuple<string,string> _selectedInvitation;

        public Tuple<string,string> SelectedInvitation
        {
            get { return _selectedInvitation; }
            set { _selectedInvitation = value;
                RaisePropertyChanged("SelectedInvitation"); }
        }

    }
}
