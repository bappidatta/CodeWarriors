using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarriors.BLL.ViewModels
{
    public class FriendViewModel
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Boolean IsAccepted { get; set; }
    }
}
