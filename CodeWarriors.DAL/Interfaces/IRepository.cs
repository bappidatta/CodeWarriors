using CodeWarriors.DAL.Model;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWarriors.DAL.Interfaces
{
    /// <summary>
    /// This is interface class for Repository.
    /// </summary>
    /// <typeparam name="T">Generic type class</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// This is insert method
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Boolean Add(T entity);

        Boolean Update(T entity);

        Boolean UpdateAll(IList<T> entities);

        Boolean Delete(ObjectId id);

        Boolean DeleteAll(IList<ObjectId> ids);

        IQueryable<T> Get();
    }
}
