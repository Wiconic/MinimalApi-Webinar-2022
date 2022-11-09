using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ProductService : IProductService
    {
        private readonly Dictionary<int, Product> _products = new();

        public Product AddProduct(Product product)
        {
            int lastKey = _products.Count() > 0 ? _products.Keys.Max() : 0;
            
            product.Id = lastKey + 1;
            _products.Add(product.Id, product);
            
            return product;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _products.Values;
        }

        public Product? GetById(int id)
        {
            if (_products.ContainsKey(id))
                    {
                return _products[id];
            }
            return null;
        }

        public bool RemoveProduct(int id)
        {
            if (_products.ContainsKey(id))
            {
                _products.Remove(id);
                return true;
            }
            return false;
        }

        public Product? UpdateProduct(Product product)
        {
            var exsists = _products.ContainsKey(product.Id);
            if(exsists)
            {
                _products[product.Id] = product;
                return _products[product.Id];
            }
            return null;
            
        }
    }
}
