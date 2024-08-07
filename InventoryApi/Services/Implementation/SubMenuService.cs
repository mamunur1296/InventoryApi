using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class SubMenuService : IBaseServices<SubMenuDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public SubMenuService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<bool> CreateAsync(SubMenuDTOs entity)
        {
            var newSubMenu = new SubMenu
            {
                Id = Guid.NewGuid().ToString(),
               // Menu=entity.Menu,
               // SubMenuRoles=entity.SubMenuRoles,
                MenuId=entity.MenuId,
                Url=entity.Url,
                Name=entity.Name,
            };
            await _unitOfWorkRepository.subMenuRepository.AddAsync(newSubMenu);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var deleteItem = await _unitOfWorkRepository.subMenuRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Sub menu   with id = {id} not found");
            }
            await _unitOfWorkRepository.subMenuRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<SubMenuDTOs>> GetAllAsync()
        {
            var companyList = await _unitOfWorkRepository.subMenuRepository.GetAllAsync();
            var result = companyList.Select(x => new SubMenuDTOs()
            {
                id = x.Id,
                Name = x.Name,
                Url = x.Url,
              //  Menu = x.Menu,
                MenuId = x.MenuId,
              //  SubMenuRoles = x.SubMenuRoles   
            });
            return result;
        }

        public Task<SubMenuDTOs> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(SubMenuDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
