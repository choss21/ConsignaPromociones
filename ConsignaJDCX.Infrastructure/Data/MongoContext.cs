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
                BsonClassMap.RegisterClassMap<BaseEntity>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(x => x.Id)
                        .SetSerializer(new NullableSerializer<Guid>(new GuidSerializer(BsonType.String)))
                        .SetIdGenerator(GuidGenerator.Instance)
                        ;

                    cm.MapMember(x => x.FechaCreacion)
                        .SetElementName("FechaCreacion")
                        .SetIsRequired(true)
                        .SetSerializer(new DateTimeSerializer(DateTimeKind.Local));
                    ;
                    cm.MapMember(x => x.FechaModificacion)
                        .SetElementName("FechaModificacion")
                        .SetSerializer(new NullableSerializer<DateTime>(new DateTimeSerializer(DateTimeKind.Local)))
                    ;
                    cm.MapMember(c => c.Activo).SetElementName("Ativo");


                });
                BsonClassMap.RegisterClassMap<Promotion>(cm =>
                {
                    cm.AutoMap();
                    cm.MapMember(c => c.FechaInicio)
                        .SetIsRequired(true)
                        .SetElementName("FechaInicio");
                    cm.MapMember(c => c.FechaFin)
                        .SetIsRequired(true)
                        .SetElementName("FechaFin");






                });
            }

        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
