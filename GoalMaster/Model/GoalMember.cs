using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalMaster.Model
{
    [Table("GoalMembers")]
    public class GoalMember
    {
        public int ID { get; set; }
        public virtual User User { get; set; }
        public virtual GoalDefinition GoalDefinition { get; set; }
    }
}
