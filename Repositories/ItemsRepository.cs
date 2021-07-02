using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private const string dbName = "catalog";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;
        public ItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(dbName);
            itemsCollection = database.GetCollection<Item>(collectionName);
        }

         public async Task CreateItemAsync(Item item)
        {
            await itemsCollection.InsertOneAsync(item);
        }
        
        public async Task DeleteItemAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
            await itemsCollection.DeleteOneAsync(filter);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
            return await itemsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await itemsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await itemsCollection.ReplaceOneAsync(filter, item);
        }
    }
}