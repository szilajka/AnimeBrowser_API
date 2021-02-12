using System;
using System.Runtime.Serialization;

namespace AnimeBrowser.Common.Exceptions
{
    public class EmptyObjectException<T> : Exception where T : class
    {
        public EmptyObjectException()
        {
        }

        public EmptyObjectException(string message) : base(message)
        {
        }

        public EmptyObjectException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmptyObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
