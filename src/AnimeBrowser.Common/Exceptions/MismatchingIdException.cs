using AnimeBrowser.Common.Models.ErrorModels;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AnimeBrowser.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class MismatchingIdException : Exception
    {
        public ErrorModel Error { get; set; }

        public MismatchingIdException(ErrorModel error)
        {
            this.Error = error;
        }

        public MismatchingIdException(ErrorModel error, string? message) : base(message)
        {
            this.Error = error;
        }

        public MismatchingIdException(ErrorModel error, string? message, Exception? innerException) : base(message, innerException)
        {
            this.Error = error;
        }

        protected MismatchingIdException(ErrorModel error, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Error = error;
        }
    }
}
