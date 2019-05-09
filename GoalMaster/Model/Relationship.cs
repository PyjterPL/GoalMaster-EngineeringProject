using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalMaster.Model
{
    [Table("Relationships")]
    public class Relationship
    {
        public Relationship()
        {

        }

        public Relationship(User userOne, User userTwo, RelationshipStatus status, User actionUser)
        {
            UserOne = userOne;
            UserTwo = userTwo;
            Status = status;
            ActionUser = actionUser;
        }
        public int ID { get; set; }
        public virtual User UserOne { get; set; }
        public virtual User UserTwo { get; set; }
        public virtual RelationshipStatus Status { get; set; }
        public virtual User ActionUser { get; set; }
        [NotMapped]
        public string UserTwoName
        {
            get { return UserTwo.Login; }
        }
        [NotMapped]
        public string StatusDescription
        {
            get
            {
                return Status.Description;
            }
        }
    }
}
