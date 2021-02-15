using System;
using System.Runtime.Serialization;

namespace AnimeBrowser.Common.Exceptions
{
    public class NotFoundObjectException<T> : Exception where T : class
    {
        public NotFoundObjectException()
        {
        }

        public NotFoundObjectException(string message) : base(message)
        {
        }

        public NotFoundObjectException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotFoundObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
