using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarriors.BLL.ViewModels
{
    public class PostViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public string PostDetails { get; set; }
        public List<string> LikedUserIds { get; set; }
        public List<UserViewModel> LikedUserName { get; set; }

        public Boolean IsLike { get; set; }

        public int? NoOfLike { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public string CreatedTime { get; set; }
    }
}
