using AnimeBrowser.Common.Models.ErrorModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AnimeBrowser.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ValidationException : Exception
    {
        public IList<ErrorModel> Errors { get; set; }
        public ValidationException(IList<ErrorModel> errorCodes)
        {
            this.Errors = errorCodes;
        }

        public ValidationException(IList<ErrorModel> errorCodes, string message) : base(message)
        {
            this.Errors = errorCodes;
        }

        public ValidationException(IList<ErrorModel> errorCodes, string message, Exception innerException) : base(message, innerException)
        {
            this.Errors = errorCodes;
        }

        protected ValidationException(IList<ErrorModel> errorCodes, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Errors = errorCodes;
        }
    }
}
