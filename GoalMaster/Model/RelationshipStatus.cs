using System.ComponentModel.DataAnnotations.Schema;

namespace GoalMaster.Model
{
    [Table("Statuses")]
    public class RelationshipStatus
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }
    public enum RelationshipStatusDefined
    {
        Pending =1,
        Accepted,
        Declined,
        Blocked
    }
}