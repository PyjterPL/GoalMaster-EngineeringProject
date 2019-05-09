using GoalMaster.Helpers;
using GoalMaster.Model;
using GoalMaster.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace GoalMaster.View
{
    /// <summary>
    /// Interaction logic for MainUserWindow.xaml
    /// </summary>
    public partial class MainUserWindow : Window
    {
        public bool IsClosed;
        public MainUserWindow(User user)
        {
            IsClosed = false;
            InitializeComponent();
            this.DataContext = new MainUserWindowViewModel(user, Vm_CloseWindowEvent);
           // showColumnChart();

            var vm = (MainUserWindowViewModel)this.DataContext;
            
            vm.CloseWindowEvent += Vm_CloseWindowEvent;
            vm.MessageBoxRequest += new EventHandler<MvvmMessageBoxEventArgs>(RegisterWindow_MessageBoxRequest);

        }

        private void RegisterWindow_MessageBoxRequest(object sender, MvvmMessageBoxEventArgs e)
        {
            e.Show();
        }

        private void Vm_CloseWindowEvent(object sender, EventArgs e)
        {
            IsClosed = true;
            this.Close();
        }
        
        private void showColumnChart()
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            valueList.Add(new KeyValuePair<string, int>("Developer", 60));
            valueList.Add(new KeyValuePair<string, int>("Misc", 20));
            valueList.Add(new KeyValuePair<string, int>("Tester", 50));
            valueList.Add(new KeyValuePair<string, int>("QA", 30));
            valueList.Add(new KeyValuePair<string, int>("Project Manager", 40));
            //Setting data for line chart
            //lineChart.DataContext = valueList;
        }
    }
}
