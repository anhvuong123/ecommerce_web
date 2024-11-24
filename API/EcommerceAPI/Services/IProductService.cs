
using EcommerceLib.Dtos;

namespace EcommerceAPI.Services
{
    public interface IProductService
    {
        Task<ProductDto> CreateProduct(ProductDto productDto);
        IEnumerable<ProductDto> GetProductsByCategory(int categoryId);
        Task<ProductDto> GetProductById(int productId);
        IEnumerable<ProductDto> GetProducts(int pageNumber = 1, int pageSize = 10);
        Task<ProductDto> UpdateProduct(int productId, ProductDto productDto);
        Task<bool> DeleteProduct(int productId);
        int GetTotalProductCount();


    }
}