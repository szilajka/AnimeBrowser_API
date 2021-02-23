using AnimeBrowser.Common.Models.ErrorModels;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AnimeBrowser.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NotExistingIdException : Exception
    {
        public ErrorModel Error { get; set; }

        public NotExistingIdException(ErrorModel error)
        {
            this.Error = error;
        }

        public NotExistingIdException(ErrorModel error, string? message) : base(message)
        {
            this.Error = error;
        }

        public NotExistingIdException(ErrorModel error, string? message, Exception? innerException) : base(message, innerException)
        {
            this.Error = error;
        }

        protected NotExistingIdException(ErrorModel error, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Error = error;
        }
    }
}
