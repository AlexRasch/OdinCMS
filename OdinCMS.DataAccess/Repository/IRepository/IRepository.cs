using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OdinCMS.DataAccess.Repository.IRepository
{
    internal interface IRepository<T> where T : class
    {
        // Create
        void Create(T entity);
        // Read 
        T GetFirstOrDefault(Expression<Func<T, bool>> filter); 
        IEnumerable<T> GetAll();

        // Update

        // Delete

    }
}
