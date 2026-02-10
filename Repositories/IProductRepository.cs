using Entities;

namespace Repositories
{
    public interface IProductRepository
    {
        public Task<(List<Product> Items, int TotalCount)> GetProducts(int position, int skip, int?[] categoryIds,
          string? description, int? maxPrice, int? minPrice);
        public Task<Product> GetProductById(int id);
        public Task<Product> AddProduct(Product newProduct);
        public Task UpdateProduct(int id, Product updateProduct);




    }
}