using GoalMaster.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoalMaster.Model
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            Relations = new HashSet<Relationship>();
            GoalDefinitions = new HashSet<GoalDefinition>();
            GoalRecords = new HashSet<GoalRecord>();
        }

        public User(string login, string password, string mail)
        {
            Login = login;
            this.password = new RijndaelCrypter().Encode(password);
            Mail = mail;
        }

        public int ID { get; set; }
        public string Login { get; set; }
        private string password;

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = new RijndaelCrypter().Encode(password);
            }
        }


        public string Mail { get; set; }
        public virtual ICollection<Relationship> Relations { get; set; }
        public virtual ICollection<GoalDefinition> GoalDefinitions { get; set; }
        public virtual ICollection<GoalRecord> GoalRecords { get; set; }

    }
}
