using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalMaster.Model
{
    [Table("GoalTypes")]
   public class GoalType
    {
        public GoalType()
        {

        }

        public GoalType(int iD, string description)
        {
            ID = iD;
            Description = description;
        }

        public GoalType(string description)
        {
            Description = description;
        }

        public int ID { get; set; }
        public string Description { get; set; }
    }
}
