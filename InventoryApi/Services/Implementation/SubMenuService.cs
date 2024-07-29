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

        public async Task<ResponseDTOs<string>> CreateAsync(SubMenuDTOs entity)
        {
            var response = new ResponseDTOs<string>();
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
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<string>> DeleteAsync(string id)
        {
            var response = new ResponseDTOs<string>();
            var deleteItem = await _unitOfWorkRepository.subMenuRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Sub menu   with id = {id} not found");
            }
            await _unitOfWorkRepository.subMenuRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<IEnumerable<SubMenuDTOs>>> GetAllAsync()
        {
            var response = new ResponseDTOs<IEnumerable<SubMenuDTOs>>();


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
            response.Data = result;
            response.Success = true;
            return response;
        }

        public Task<ResponseDTOs<SubMenuDTOs>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTOs<string>> UpdateAsync(SubMenuDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
