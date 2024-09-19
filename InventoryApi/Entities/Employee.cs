﻿using InventoryApi.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApi.Entities
{
    public class Employee : BaseEntity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string? Title { get; set; }
        public string? TitleOfCourtesy { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string HomePhone { get; set; }
        public string? Extension { get; set; }
        public byte[]? Photo { get; set; }
        public string? Notes { get; set; }
        public int? ReportsTo { get; set; }
        public string? PhotoPath { get; set; }
        public string ? ManagerId { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string ?CompanyId { get; set; }
        public string ?BranchId { get; set; }
        public Company? Company { get; set; }
        public Branch? Branch { get; set; }
        public Employee? Manager { get; set; }
        public ICollection<Employee> ?  Subordinates { get; set; }
        public ICollection<Order>? Orders { get; set; }

    }
}
