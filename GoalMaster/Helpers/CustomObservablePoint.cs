using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalMaster.Helpers
{
    public class CustomObservablePoint : ObservablePoint,INotifyPropertyChanged
    {
        private double _x;
        private double _y;

        /// <summary>
        /// Initializes a new instance of ObservablePoint class
        /// </summary>
        public CustomObservablePoint()
        {

        }

        /// <summary>
        /// Initializes a new instance of ObservablePoint class giving the x and y coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public CustomObservablePoint(double x, double y)
        {
            X = x;
            Y = y;
        }
        public CustomObservablePoint(double x, string date)
        {
            X = x;
            Date = date;
        }

        /// <summary>
        /// X coordinate
        /// </summary>
        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged("X");
            }
        }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged("Y");
            }
        }
        private string _date;

        public string Date
        {
            get { return _date; }
            set { _date = value;
                OnPropertyChanged("Date");
            }
        }


        #region INotifyPropertyChangedImplementation

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
