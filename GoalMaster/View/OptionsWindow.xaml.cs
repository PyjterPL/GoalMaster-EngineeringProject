using GalaSoft.MvvmLight.Command;
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
    /// Logika interakcji dla klasy OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {

        public OptionsWindow(User user, bool isOwner = true)
        {
            InitializeComponent();
            var vm = new OptionsViewModel(user, isOwner);
            this.DataContext = vm;
            vm.CloseWindowEvent += Vm_CloseWindowEvent;

        }
        private void Vm_CloseWindowEvent(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
