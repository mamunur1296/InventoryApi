﻿using InventoryApi.DataContext;
using InventoryApi.Entities;
using InventoryApi.Repository.Interfaces;

namespace InventoryApi.Repository.Implementation
{
    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AttendanceRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        // Implement additional methods specific to AttendanceRepository here

    }
}
