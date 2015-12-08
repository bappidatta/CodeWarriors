using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarriors.DAL.Model
{
    /// <summary>
    /// This is model class for Restricted Post.
    /// </summary>
    public class RestrictedPost  :Entity
    {
        public string PostId { get; set; }
        public string UserId { get; set; }
    }
}
