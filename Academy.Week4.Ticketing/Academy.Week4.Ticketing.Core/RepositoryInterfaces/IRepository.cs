using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.Ticketing.Core.RepositoryInterfaces
{
    public interface IRepository<T>
    {
        List<T> GetAll(Func<T, bool> filter = null);

        bool Add(T item);
        bool Update(T item);   
        bool Delete(T item);

    }
}
