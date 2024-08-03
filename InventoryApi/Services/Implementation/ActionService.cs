using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class ActionService : IBaseServices<ActionNameDTOs>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ActionService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<ResponseDTOs<string>> CreateAsync(ActionNameDTOs entity)
        {
            var response = new ResponseDTOs<string>();
            var newCompany = new ActionName
            {
                Id = Guid.NewGuid().ToString(),
                Name = entity.Name,
            };
            await _unitOfWorkRepository.actionRepository.AddAsync(newCompany);
            await _unitOfWorkRepository.SaveAsync();
            response.Success = true;
            return response;
        }

        public Task<ResponseDTOs<string>> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTOs<IEnumerable<ActionNameDTOs>>> GetAllAsync()
        {
            var response = new ResponseDTOs<IEnumerable<ActionNameDTOs>>();


            var MenuList = await _unitOfWorkRepository.actionRepository.GetAllAsync();
            var result = MenuList.Select(x => new ActionNameDTOs()
            {
                id = x.Id,
                Name = x.Name
            });
            response.Data = result;
            response.Success = true;
            return response;

        }

        public Task<ResponseDTOs<ActionNameDTOs>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTOs<string>> UpdateAsync(ActionNameDTOs entity)
        {
            throw new NotImplementedException();
        }
    }
}
