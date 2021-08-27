using ETender.Products.Entities;
using MongoDB.Driver;

namespace ETender.Products.Data.Interfaces
{
    public interface IProductContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
