using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarriors.DAL.Model
{
    /// <summary>
    /// This is model class for Friend
    /// </summary>
    public class Friend : Entity
    {
        public string  UserId { get; set; }
        public string FriendId { get; set; }
        public Boolean IsAccepted { get; set; }
    }
}
