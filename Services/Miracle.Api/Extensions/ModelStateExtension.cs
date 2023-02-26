using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Miracle.Api.Extensions
{
    public static class ModelStateExtension
    {
        public static IEnumerable<string> GetModelStateErrors(this ControllerBase controllerBase)
        {
            if (controllerBase == null)
                return null;

            var errors = controllerBase.ModelState.Values
                .Where(s => s.ValidationState == ModelValidationState.Invalid)?
                .Select(s => s.Errors)?
                .FirstOrDefault()?
                .Select(s => s.ErrorMessage)?
                .ToList();

            return errors;
        }
    }
}
