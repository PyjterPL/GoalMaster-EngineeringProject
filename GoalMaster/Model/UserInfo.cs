using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoalMaster.Model
{
    [Table("UsersInfo")]
    public class UserInfo
    {
        public UserInfo()
        {
            BirthDate = new DateTime(1900, 1, 1);
        }

        public UserInfo(byte[] profileImage, User user, string userDescription)
        {
            ProfileImage = profileImage;
            User = user;
            UserDescription = userDescription;
        }
        public int ID { get; set; }
        public byte[] ProfileImage { get; set; }
        public virtual User User { get; set; }
        public string UserDescription { get; set; }
        public string  Address { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
