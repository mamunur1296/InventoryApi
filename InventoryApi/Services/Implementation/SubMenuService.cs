using AutoMapper;
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
        private readonly IMapper _mapper;

        public SubMenuService(IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(SubMenuDTOs entity)
        {
            var newSubMenu = new SubMenu
            {
                Id = Guid.NewGuid().ToString(),
                MenuId=entity.MenuId,
                Url=entity.Url?.Trim(),
                Name=entity.Name.Trim(),
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
                SubMenuid = x.Id,
                Name = x.Name,
                Url = x.Url,
              //  Menu = x.Menu,
                MenuId = x.MenuId,
              //  SubMenuRoles = x.SubMenuRoles   
            });
            return result;
        }

        public async Task<SubMenuDTOs> GetByIdAsync(string id)
        {
            var item = await _unitOfWorkRepository.subMenuRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"Company with id = {id} not found");
            }
            var result = _mapper.Map<SubMenuDTOs>(item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, SubMenuDTOs entity)
        {
            var item = await _unitOfWorkRepository.subMenuRepository.GetByIdAsync(id);
            if (item == null || item?.Id != id)
            {
                throw new NotFoundException($"SubMenu with id = {id} not found");
            }

            // Update properties with validation
            item.MenuId = string.IsNullOrWhiteSpace(entity.MenuId) ? item.MenuId : entity.MenuId.Trim();
            item.Url = string.IsNullOrWhiteSpace(entity.Url) ? item.Url : entity.Url.Trim();
            item.Name = string.IsNullOrWhiteSpace(entity.Name) ? item.Name : entity.Name.Trim();

            // Perform update operation
            await _unitOfWorkRepository.subMenuRepository.UpdateAsync(item);
            await _unitOfWorkRepository.SaveAsync();

            return true;
        }

    }
}
