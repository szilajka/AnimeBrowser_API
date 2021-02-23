using AnimeBrowser.Common.Models.ErrorModels;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AnimeBrowser.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class AlreadyExistingObjectException<T> : Exception where T : class
    {
        public ErrorModel Error { get; set; }
        public AlreadyExistingObjectException(ErrorModel error)
        {
            this.Error = error;
        }

        public AlreadyExistingObjectException(ErrorModel error, string? message) : base(message)
        {
            this.Error = error;
        }

        public AlreadyExistingObjectException(ErrorModel error, string? message, Exception? innerException) : base(message, innerException)
        {
            this.Error = error;
        }

        protected AlreadyExistingObjectException(ErrorModel error, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Error = error;
        }
    }
}
