using GoalMaster.Helpers;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();

            var vm = (RegisterWindowViewModel)this.DataContext;

            vm.CloseWindowEvent += Vm_CloseWindowEvent;
            vm.MessageBoxRequest += new EventHandler<MvvmMessageBoxEventArgs>(RegisterWindow_MessageBoxRequest);
        }

        private void RegisterWindow_MessageBoxRequest(object sender, MvvmMessageBoxEventArgs e)
        {
            e.Show();
        }

        private void Vm_CloseWindowEvent(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
