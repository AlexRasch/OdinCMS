using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdinCMS.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {


        IProductRepository Product { get; }
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }


        void Save();
    }
}
