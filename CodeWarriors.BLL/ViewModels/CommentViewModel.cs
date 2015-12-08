using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarriors.BLL.ViewModels
{
    public class CommentViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CommentDetails { get; set; }
        public string PostId { get; set; }
        public string CreatedTime { get; set; }
    }
}
