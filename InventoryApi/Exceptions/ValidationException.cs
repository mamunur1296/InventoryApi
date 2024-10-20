using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace InventoryApi.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string message)
            : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base(CreateErrorMessage(failures))
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidationException(IEnumerable<IdentityError> errors)
            : base(CreateErrorMessage(errors))
        {
            Errors = errors
                .GroupBy(e => e.Code, e => e.Description)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidationException(string message, IEnumerable<ValidationFailure> failures)
            : base($"{message}: {CreateErrorMessage(failures)}")
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidationException(string message, IEnumerable<IdentityError> errors)
            : base($"{message}: {CreateErrorMessage(errors)}")
        {
            Errors = errors
                .GroupBy(e => e.Code, e => e.Description)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public IDictionary<string, string[]> Errors { get; }

        // This method creates a user-friendly error message from ValidationFailure objects
        private static string CreateErrorMessage(IEnumerable<ValidationFailure> failures)
        {
            return string.Join("; ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));
        }

        // This method creates a user-friendly error message from IdentityError objects
        private static string CreateErrorMessage(IEnumerable<IdentityError> errors)
        {
            return string.Join("; ", errors.Select(e => $"{e.Code}: {e.Description}"));
        }

        // New method to generate a summary of errors for logging or display
        public string GetSummary()
        {
            return string.Join("\n", Errors.Select(kvp => $"{kvp.Key}: {string.Join(", ", kvp.Value)}"));
        }
    }
}
