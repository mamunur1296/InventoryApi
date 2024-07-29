using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Exceptions;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class MenuService : IBaseServices<MenuDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public MenuService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<ResponseDTOs<string>> CreateAsync(MenuDTOs entity)
        {
            var response = new ResponseDTOs<string>();
            var newMenu = new Menu
            {
                Id = Guid.NewGuid().ToString(),
                Name = entity.Name,
                Url = entity.Url,
                

            };
            await _unitOfWorkRepository.menuRepository.AddAsync(newMenu);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<string>> DeleteAsync(string id)
        {
            var response = new ResponseDTOs<string>();
            var deleteItem = await _unitOfWorkRepository.menuRepository.GetByIdAsync(id);

            if (deleteItem == null)
            {
                throw new NotFoundException($"Menu   with id = {id} not found");
            }
            await _unitOfWorkRepository.menuRepository.DeleteAsync(deleteItem);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public async Task<ResponseDTOs<IEnumerable<MenuDTOs>>> GetAllAsync()
        {
            var response = new ResponseDTOs<IEnumerable<MenuDTOs>>();


            var MenuList = await _unitOfWorkRepository.menuRepository.GetAllAsync();
            var result = MenuList.Select(x => new MenuDTOs()
            {
                id = x.Id,
                Url = x.Url,
                Name = x.Name
            });
            response.Data = result;
            response.Success = true;
            return response;
        }

        public Task<ResponseDTOs<MenuDTOs>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTOs<string>> UpdateAsync(MenuDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
