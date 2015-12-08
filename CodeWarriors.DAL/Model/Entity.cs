using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace CodeWarriors.DAL.Model
{
    /// <summary>
    /// This is model class for Entity. This class is used to manipulate mongoDB id field.
    /// </summary>
    public class Entity
    {
        public ObjectId Id { get; set; }
    }
}
