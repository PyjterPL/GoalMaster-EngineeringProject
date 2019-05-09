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
    /// Interaction logic for AddGoalDefinitionWindow.xaml
    /// </summary>
    public partial class AddGoalDefinitionWindow : Window
    {
        public AddGoalDefinitionWindow(User user)
        {
            InitializeComponent();
            var vw = new AddGoalDefinitionViewModel(user);
            this.DataContext = vw;
            this.Closing += vw.BackToMainUserWindow;
            vw.CloseWindowEvent += Vw_CloseWindowEvent;
        }

        private void Vw_CloseWindowEvent(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
