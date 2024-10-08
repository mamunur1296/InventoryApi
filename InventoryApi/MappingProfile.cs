using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Entities;

namespace InventoryApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Category, CategoryDTOs>().ReverseMap();
            CreateMap<Company, CompanyDTOs>().ReverseMap();
            CreateMap<CartItem, CartItemDTOs>().ReverseMap();
            CreateMap<Supplier, SupplierDTOs>().ReverseMap();
            CreateMap<Stock, StockDTOs>().ReverseMap();
            CreateMap<Customer, CustomerDTOs>().ReverseMap();
            CreateMap<DeliveryAddress, DeliveryAddressDTOs>().ReverseMap();
            CreateMap<Employee, EmployeeDTOs>().ReverseMap();
            CreateMap<Menu, MenuDTOs>().ReverseMap();
            CreateMap<MenuRole, MenuRoleDTOs>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTOs>().ReverseMap();
            CreateMap<Order, OrderDTOs>().ReverseMap();
            CreateMap<Payment, PaymentDTOs>().ReverseMap();
            CreateMap<Product, ProductDTOs>().ReverseMap();
            CreateMap<Review, ReviewDTOs>().ReverseMap();
            CreateMap<Shipper, ShipperDTOs>().ReverseMap();
            CreateMap<SubMenu, SubMenuDTOs>().ReverseMap();
            CreateMap<SubMenuRole, SubMenuRoleDTOs>().ReverseMap();
            CreateMap<Warehouse, WarehouseDTOs>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartDTOs>().ReverseMap();
            CreateMap<Prescription, PrescriptionDTOs>().ReverseMap();
            CreateMap<OrderProduct, OrderProductDTOs>().ReverseMap();
            CreateMap<Branch, BranchDTOs>().ReverseMap();
            CreateMap<UnitMaster, UnitMasterDTOs>().ReverseMap();
            CreateMap<UnitChild, UnitChildhDTOs>().ReverseMap();
            CreateMap<Purchase, PurchaseDTOs>().ReverseMap();
            CreateMap<PurchaseDetail, PurchaseDetailDTOs>().ReverseMap();
            CreateMap<Attendance, AttendanceDTOs>().ReverseMap();
            CreateMap<Department, DepartmentDTOs>().ReverseMap();
            CreateMap<Holiday, HolidayDTOs>().ReverseMap();
            CreateMap<Leave, LeaveDTOs>().ReverseMap();
            CreateMap<Payroll, PayrollDTOs>().ReverseMap();
            CreateMap<Shift, ShiftDTOs>().ReverseMap();
        } 
    }
}