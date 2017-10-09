using System;
using System.Linq;
using PacktWebapp.Entities;
using PacktWebapp.Controllers;
using PacktWebapp.QueryParameters;

namespace PacktWebapp.Repositories
{
    public interface ICustomerRepository
    {
        void Add(Customer item);
        void Delete(Guid id);
        IQueryable<Customer> GetAll(CustomerQueryParameters customerQueryParameters);
        Customer GetSingle(Guid id);
        bool Save();

        int Count();

        void Update(Customer item);
    }
}
