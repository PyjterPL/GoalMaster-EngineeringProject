using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoalMaster.Helpers
{
    class NoInternetConnectionException : Exception
    {
        public NoInternetConnectionException()
        {
        }

        public NoInternetConnectionException(string message) : base(message)
        {
        }

        public NoInternetConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoInternetConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
