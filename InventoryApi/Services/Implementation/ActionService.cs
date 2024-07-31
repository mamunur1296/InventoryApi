using InventoryApi.DTOs;
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

        public Task<ResponseDTOs<string>> CreateAsync(ActionNameDTOs entity)
        {
            throw new NotImplementedException();
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
