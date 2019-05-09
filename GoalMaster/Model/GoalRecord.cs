using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;

namespace GoalMaster.Model
{
    [Table("GoalRecords")]
    public class GoalRecord 
    {
        public int ID { get; set; }
        public virtual User User { get; set; }
        public virtual GoalDefinition GoalDefinition { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public string Note { get; set; }
        [NotMapped]
        public string OnlyDate
        {
            get { return Date.ToString("dd/MM/yyyy"); }
        }
        //[NotMapped]
        //public ObservableCollection<object> LegendItems => throw new NotImplementedException();
        //[NotMapped]
        //public ISeriesHost SeriesHost { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
