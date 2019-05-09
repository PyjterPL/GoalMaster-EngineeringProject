using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoalMaster.Model
{
    [Table("GoalDefinitions")]
    public class GoalDefinition
    {
        public GoalDefinition()
        {
            Users = new HashSet<User>();
        }

        public GoalDefinition(string name, string description, User ownerUserID, bool shared, GoalType goalType)
        {
            Name = name;
            Description = description;
            OwnerUserID = ownerUserID;
            Shared = shared;
            GoalType = goalType;
            Users = new HashSet<User>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual User OwnerUserID { get; set; }
        public bool Shared { get; set; }
        public virtual GoalType GoalType{ get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}