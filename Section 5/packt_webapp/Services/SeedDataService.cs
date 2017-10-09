using PacktWebapp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PacktWebapp.Services
{
    public class SeedDataService : ISeedDataService
    {
        private PacktDbContext _context;
        public SeedDataService(PacktDbContext context)
        {
            _context = context;
        }

        public async Task EnsureSeedData()
        {
            _context.Database.EnsureCreated();

            _context.Customers.RemoveRange(_context.Customers);
            _context.SaveChanges();

            Customer customer = new Customer();
            customer.Firstname = "Chuck";
            customer.Lastname = "Norris";
            customer.Age = 30;
            customer.Id = Guid.NewGuid();

            _context.Add(customer);

            Customer customer2 = new Customer();
            customer2.Firstname = "Fabian";
            customer2.Lastname = "Gosebrink";
            customer2.Age = 31;
            customer2.Id = Guid.NewGuid();

            Customer customer3 = new Customer();
            customer3.Firstname = "Damien";
            customer3.Lastname = "Bowden";
            customer3.Age = 32;
            customer3.Id = Guid.NewGuid();

            Customer customer4 = new Customer();
            customer4.Firstname = "Obi-Wan";
            customer4.Lastname = "Kenobi";
            customer4.Age = 33;
            customer4.Id = Guid.NewGuid();

            Customer customer5 = new Customer();
            customer5.Firstname = "Phil";
            customer5.Lastname = "Collins";
            customer5.Age = 34;
            customer5.Id = Guid.NewGuid();

            _context.Add(customer2);
            _context.Add(customer3);
            _context.Add(customer4);
            _context.Add(customer5);

            await _context.SaveChangesAsync();
        }

    }
}
