using InventoryApi.DTOs;
using InventoryApi.Entities;
using InventoryApi.Services.Interfaces;
using InventoryApi.UnitOfWork;

namespace InventoryApi.Services.Implementation
{
    public class ActionRoleService : IBaseServices<ActionRole>
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public ActionRoleService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }
        public Task<ResponseDTOs<string>> CreateAsync(ActionRole entity)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTOs<string>> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTOs<IEnumerable<ActionRole>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTOs<ActionRole>> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTOs<string>> UpdateAsync(ActionRole entity)
        {
            throw new NotImplementedException();
        }
    }
}
