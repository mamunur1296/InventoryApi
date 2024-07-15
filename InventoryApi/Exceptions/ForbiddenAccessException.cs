

namespace InventoryApi.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message) : base(message) { }
    }
}
