﻿using Crm.DataAccess;
using Crm.Model;
using Crm.UI.Data;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Crm.UI.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        //private Func<CrmDbContext> _contextCreator;
        private CrmDbContext _context { get; set; }

        public CustomerRepository(CrmDbContext context)
        {
            _context = context;
        }
        public async Task<Customer> GetByIdAsync(int customerId)
        {
            return await _context.Customers.SingleAsync(f => f.Id == customerId);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
