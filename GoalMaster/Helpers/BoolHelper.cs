using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalMaster.Helpers
{
    public class BoolHelper
    {
        public bool Value { get; set; }
        public string Description { get; set; }

        public BoolHelper(bool value, string description)
        {
            Value = value;
            Description = description;
        }
    }
}
