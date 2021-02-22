using AnimeBrowser.Common.Models.ErrorModels;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace AnimeBrowser.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class EmptyObjectException<T> : Exception where T : class
    {
        public ErrorModel Error { get; set; }

        public EmptyObjectException(ErrorModel error)
        {
            this.Error = error;
        }

        public EmptyObjectException(ErrorModel error, string message) : base(message)
        {
            this.Error = error;
        }

        public EmptyObjectException(ErrorModel error, string message, Exception innerException) : base(message, innerException)
        {
            this.Error = error;
        }

        protected EmptyObjectException(ErrorModel error, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Error = error;
        }
    }
}
