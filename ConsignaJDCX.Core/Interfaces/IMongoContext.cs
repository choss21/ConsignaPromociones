using MongoDB.Driver;

namespace ConsignaJDCX.Core.Interfaces
{
    public interface IMongoContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
