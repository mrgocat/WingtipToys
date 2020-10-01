using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WingtipToys.BusinessLogicLayer.Models;
using WingtipToys.DataAccessLayer;

namespace WingtipToys.BusinessLogicLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly WingtipContext _context;
        private readonly IMapper _mapper;

        public ProductService(WingtipContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public ProductDto Get(int productId)
        {
            var product = _context.Products.AsNoTracking().Include(p => p.Category).FirstOrDefault(p => p.Id == productId);
            if(product == null)
            {
                //throw new KeyNotFoundException($"A record with ID={productId} was not found.");
                return null;
            }
            else
            {
                ProductDto dto = _mapper.Map<ProductDto>(product);
                return dto;
            }
        }

        public List<CategoryDto> GetCategoryList()
        {
            var list = _context.Categories.ToList();
            return _mapper.Map<List<CategoryDto>>(list); 
        }

        public List<ProductDto> GetProductList()
        {
            var list = _context.Products.AsNoTracking().ToList();
            var dtoList = _mapper.Map<List<ProductDto>>(list);
            return dtoList;
        }

        public List<ProductDto> GetProductListbyCategory(int categoryId)
        {
            var list = _context.Products.AsNoTracking().Where(p => p.CategoryId == categoryId).ToList();
            var dtoList = _mapper.Map<List<ProductDto>>(list);
            return dtoList;
        }

        public List<ProductDto> SearchProducts(string name)
        {
            var list = _context.Products.AsNoTracking().Where(p => p.ProductName.Contains(name)).ToList();
            var dtoList = _mapper.Map<List<ProductDto>>(list);
            return dtoList;
        }
    }
}
