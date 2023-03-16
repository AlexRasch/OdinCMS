using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdinCMS.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {

        /* Product  */
        IProductRepository Product { get; }
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }

        /* User & Company */
        ICompanyRepository Company { get; }


        void Save();
    }
}
