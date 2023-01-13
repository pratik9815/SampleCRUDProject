using DAL.DataContext;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public class ProductRepositry  : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepositry(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Product> GetProducts()
        {
            return _context.Products.OrderBy(p => p.Id).ToList();
        }
        
    }
}
