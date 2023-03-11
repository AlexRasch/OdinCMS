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
            Product = new ProductRepository(_db);
            Category = new CategoryRepository(_db);
            CoverType = new CoverTypeRepositroy(_db);
        }

        public IProductRepository Product { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
