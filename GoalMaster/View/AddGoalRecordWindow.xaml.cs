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
    /// Interaction logic for AddGoalRecordWindow.xaml
    /// </summary>
    public partial class AddGoalRecordWindow : Window
    {
        public AddGoalRecordWindow(User user,GoalDefinition goalDefinition,DateTime date)
        {
            InitializeComponent();
            var vm = new AddGoalRecordViewModel(user, goalDefinition, date);
            this.DataContext = vm;
            vm.CloseWindowEvent += Vw_CloseWindowEvent;
        }

        private void Vw_CloseWindowEvent(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
