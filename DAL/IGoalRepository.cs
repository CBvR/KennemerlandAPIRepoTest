﻿using System.Threading.Tasks;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;

namespace DAL
{
    public interface IGoalRepository
    {
        Goal CreateGoal(Goal goal);
        BsonDocument GetGoal(int goalId);
        IEnumerable<BsonDocument> GetAllGoals();

        bool DeleteGoal(int goalId);
    }

    public class GoalRepository : IGoalRepository
    {
        public Goal CreateGoal(Goal goal)
        {
            IMongoCollection<BsonDocument> collection = MongoSingleton.getMongoCollection("goals");
            var newGoal = BsonDocument.Parse(goal.ToJson());
            collection.InsertOne(newGoal);
            return goal;
        }

        public bool DeleteGoal(int GoalId)
        {
            IMongoCollection<BsonDocument> collection = MongoSingleton.getMongoCollection("goals");
            var filter = Builders<BsonDocument>.Filter.Eq("GoalId", GoalId);
            try { 
                collection.DeleteOne(filter);
                return true;
            }
            catch {
                return false;
            }
        }

        public BsonDocument GetGoal(int GoalId)
        {
            IMongoCollection<BsonDocument> collection = MongoSingleton.getMongoCollection("goals");

            var filter = Builders<BsonDocument>.Filter.Eq("GoalId", GoalId);
            var goal = collection.Find(filter).FirstOrDefault();
            goal.Remove("_id");
            return goal;
        }

        public IEnumerable<BsonDocument> GetAllGoals()
        {
            IMongoCollection<BsonDocument> collection = MongoSingleton.getMongoCollection("goals");
            IEnumerable<BsonDocument> documents = collection.Find(_ => true).ToList();
            return documents;
        }
    }
}