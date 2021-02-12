using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using FluentValidation.Results;
using System.Collections.Generic;

namespace AnimeBrowser.BL.Helpers
{
    public static class ValidationErrorConverter
    {
        public static IList<ErrorModel> ConvertToErrorModel(this IList<ValidationFailure> failures)
        {
            var errorList = new List<ErrorModel>();
            foreach (var failure in failures)
            {
                ErrorModel errModel = new ErrorModel
                {
                    Code = failure.ErrorCode,
                    Description = failure.ErrorMessage,
                    Source = failure.PropertyName,
                    Title = EnumHelper.GetDescriptionFromValue(failure.ErrorCode, typeof(ErrorCodes))
                };
                errorList.Add(errModel);
            }
            return errorList;
        }
    }
}
