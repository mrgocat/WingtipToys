using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WingtipToys.BusinessLogicLayer.Models;

namespace WingtipToys.BusinessLogicLayer.Services
{
    public interface IProductService
    {
        public List<ProductDto> SearchProducts(string name);
        public List<ProductDto> GetProductList();
        public List<CategoryDto> GetCategoryList();
        public List<ProductDto> GetProductListbyCategory(int categoryId);
        public ProductDto Get(int productId);

        public Task<List<ProductDto>> SearchProductsAsync(string name);
        public Task<List<ProductDto>> GetProductListAsync();
        public Task<List<CategoryDto>> GetCategoryListAsync();
        public Task<List<ProductDto>> GetProductListbyCategoryAsync(int categoryId);
        public Task<ProductDto> GetAsync(int productId);
    }
}
