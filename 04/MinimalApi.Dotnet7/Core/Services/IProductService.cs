using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IProductService
    {
        Product AddProduct(Product product);
        Product? UpdateProduct(Product product);
        bool RemoveProduct(int id);

        IEnumerable<Product> GetAllProducts();
        Product? GetById(int id);
    }
}
