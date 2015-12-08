using CodeWarriors.DAL.Interfaces;
using CodeWarriors.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;

namespace CodeWarriors.DAL.Repositories
{
    /// <summary>
    /// This is Repository Class.
    /// </summary>
    /// <typeparam name="T">Generic Class</typeparam>
    public class Repository<T> : IRepository<T>
    {
        private string collectionName;

        /// <summary>
        /// This is repository constructor
        /// </summary>
        /// <param name="collectionName">Repository constructor takes String collection parameter.</param>
        public Repository(string collectionName)
        {
            this.collectionName = collectionName;
        }

        /// <summary>
        /// This method Initialize Mongo DB client, server and return collection.
        /// </summary>
        /// <returns>Returns Mongo DB Collection</returns>
        private MongoCollection<T> GetCollection()
        {
            var client = new MongoClient("mongodb://localhost");
            var server = client.GetServer();
            var database = server.GetDatabase("cw");
            var collection = database.GetCollection<T>(collectionName);

            return collection;
        }

        /// <summary>
        /// This is insert method
        /// </summary>
        /// <param name="entity">Takes Generic Class T</param>
        /// <returns>return boolean insert operation status value</returns>
        public bool Add(T entity)
        {
            this.GetCollection().Insert(entity);

            return true;
        }

        /// <summary>
        /// This is Update method
        /// </summary>
        /// <param name="entity">Takes Generic Class T</param>
        /// <returns>return boolean update operation status value</returns>
        public bool Update(T entity)
        {
            this.GetCollection().Save<T>(entity);

            return true;
        }

        /// <summary>
        /// Updates all value against matching entry available in entities
        /// </summary>
        /// <param name="entities">IList of generic type entity</param>
        /// <returns>return boolean bulk update operation status value</returns>
        public bool UpdateAll(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                this.GetCollection().Save<T>(entity);
            }
            return true;
        }

        /// <summary>
        /// Delete Perticular Object from given Object ID
        /// </summary>
        /// <param name="id">Takes Bson Object ID</param>
        /// <returns>return boolean delete operation status value</returns>
        public bool Delete(ObjectId id)
        {
            this.GetCollection().Remove(Query.EQ("_id", id));

            return true;
        }

        /// <summary>
        /// Delete All Object matching Bson Object available in ids IList type variable
        /// </summary>
        /// <param name="id">Takes IList of Bson Object ID</param>
        /// <returns>return boolean delete operation status value</returns>
        public bool DeleteAll(IList<ObjectId> ids)
        {
            foreach (var id in ids)
            {
                this.GetCollection().Remove(Query.EQ("_id", id));
            }
            return true;
        }

        /// <summary>
        /// Get Method Returns IQueryable Collection
        /// </summary>
        /// <returns>IQueryable Collection</returns>
        public IQueryable<T> Get()
        {
            var entities = GetCollection().AsQueryable<T>();

            return entities;
        }
    }
}
