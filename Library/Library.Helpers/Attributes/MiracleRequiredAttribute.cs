using System.ComponentModel.DataAnnotations;

namespace Library.Helpers.Attributes
{
    public class MiracleRequiredAttribute : RequiredAttribute
    {
        public MiracleRequiredAttribute(string errorMessage = "Value is cannot be empty")
        {
            ErrorMessage = errorMessage;
        }
    }
}