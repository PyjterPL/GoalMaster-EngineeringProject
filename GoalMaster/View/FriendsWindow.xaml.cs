using GoalMaster.Model;
using GoalMaster.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GoalMaster.View
{
    /// <summary>
    /// Logika interakcji dla klasy FriendsWindow.xaml
    /// </summary>
    public partial class FriendsWindow : Window
    {
        public FriendsWindow(User user)
        {
            InitializeComponent();
            var vm = new FriendsViewModel(user);
            this.DataContext = vm;
            vm.CloseWindowEvent += Vm_CloseWindowEvent;
        }
        private void Vm_CloseWindowEvent(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //sender.selectedVAlue
            if (sender is ListBox send)
            {
                 //send = (ListBox)sender;
                if (send.SelectedItem is User user)
                {
                    var optionsWindow = new OptionsWindow(user, false);
                    optionsWindow.ShowDialog();
                }
                else if (send.SelectedItem is Tuple<string, string> userName)
                {
                    using (var db = new GoalMasterDatabaseContext())
                    {
                        var userFromDB = db.Users.FirstOrDefault(us => us.Login == userName.Item1);
                        var optionsWindow = new OptionsWindow(userFromDB, false);
                        optionsWindow.ShowDialog();
                    }
                }
            }
            else if(sender is DataGrid dgSend)
            {
                var user = (Tuple<string, string>)dgSend.SelectedItem;
                using (var db = new GoalMasterDatabaseContext())
                {
                    var userFromDB = db.Users.FirstOrDefault(us => us.Login == user.Item1);
                    var optionsWindow = new OptionsWindow(userFromDB, false);
                    optionsWindow.ShowDialog();
                }
            }




        }

    }
}
