﻿using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class PayrollRepository : Repository<Payroll>, IPayrollRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PayrollRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to BranchRepository here

    }
}