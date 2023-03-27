using OdinCMS.DataAccess.Data;
using OdinCMS.DataAccess.Repository.IRepository;
using OdinCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdinCMS.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            ShoppingCart = new ShoppingCartRepository(_db);

            Product = new ProductRepository(_db);
            Category = new CategoryRepository(_db);
            CoverType = new CoverTypeRepositroy(_db);
            
            OrderDetail = new OrderDetailRepository(_db);
            OrderHeader = new OrderheaderRepository(_db);

            Company = new CompanyRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            

        }

        /* Cart */
        public IShoppingCartRepository ShoppingCart { get; private set; }

        /* Products */
        public IProductRepository Product { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }

        /* Order */
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }

        /* User / Company */
        public ICompanyRepository Company { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
