using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdinCMS.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        /* Cart */
        IShoppingCartRepository ShoppingCart { get; }
        /* Product  */
        IProductRepository Product { get; }
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }
        /* Order */
        IOrderDetailRepository OrderDetail { get; }
        IOrderHeaderRepository OrderHeader { get; }

        /* User & Company */
        ICompanyRepository Company { get; }
        IApplicationUserRepository ApplicationUser { get; }

        void Save();
    }
}
