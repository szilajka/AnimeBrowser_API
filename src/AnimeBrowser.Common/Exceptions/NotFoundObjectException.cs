using AnimeBrowser.Common.Models.ErrorModels;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AnimeBrowser.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NotFoundObjectException<T> : Exception where T : class
    {
        public ErrorModel Error { get; set; }

        public NotFoundObjectException(ErrorModel error)
        {
            this.Error = error;
        }

        public NotFoundObjectException(ErrorModel error, string message) : base(message)
        {
            this.Error = error;
        }

        public NotFoundObjectException(ErrorModel error, string message, Exception innerException) : base(message, innerException)
        {
            this.Error = error;
        }

        protected NotFoundObjectException(ErrorModel error, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Error = error;
        }
    }
}
