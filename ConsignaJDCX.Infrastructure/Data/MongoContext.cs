using ConsignaJDCX.Core.Entities;
using ConsignaJDCX.Core.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;

namespace ConsignaJDCX.Infrastructure.Data
{
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MongoContext(IOptions<Mongosettings> configuration)
        {
            _mongoClient = new MongoClient(configuration.Value.Connection);
            _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);
            if (!BsonClassMap.IsClassMapRegistered(typeof(Promotion)))
            {
                BsonClassMap.RegisterClassMap<Promotion>(cm =>
                {
                    cm.AutoMap();
                    // cm.MapIdMember(c => c.Id).SetIdGenerator(CombGuidGenerator.Instance);
                    cm.MapIdMember(x => x.Id)
                        .SetSerializer(new NullableSerializer<Guid>(new GuidSerializer(BsonType.String)))
                        //.SetSerializer(new  GuidSerializer(BsonType.String))
                        .SetIdGenerator(GuidGenerator.Instance)
                        ;
                    //cm.MapMember(c => c.SupplementName).SetElementName("Name");
                    //cm.MapMember(c => c.Price).SetElementName("Price");
                    //cm.MapMember(c => c.Size).SetElementName("Size");
                    //cm.MapMember(c => c.Type).SetElementName("Type");
                    //cm.MapMember(c => c.Brand).SetElementName("Brand");

                });
            }

        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
