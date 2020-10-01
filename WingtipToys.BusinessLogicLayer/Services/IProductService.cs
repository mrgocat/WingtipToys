using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
