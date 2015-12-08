using CodeWarriors.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarriors.BLL.Interfaces
{
    public interface IPostBLL
    {
        string CreatePost(PostViewModel userVM);

        Boolean UpatePost(PostViewModel userVM);

        Boolean DeletePost(string postId);

        Boolean HidePost(string postId, string userId);

        Boolean AddComments(CommentViewModel commentVM);

        Boolean AddLike(string userId, string postId);

        Boolean RemoveLike(string userId, string postId);

        IEnumerable<PostViewModel> GetAllPost(string userId);

        IEnumerable<PostViewModel> GetAllPostByUser(string userId);

        PostViewModel GetPostByID(string id);
    }
}
