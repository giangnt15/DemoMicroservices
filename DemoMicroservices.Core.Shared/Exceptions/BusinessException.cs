using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMicroservices.Core.Shared.Exceptions
{
    public class BusinessException : Exception
    {
        private readonly string _errorCode;
        public BusinessException(string message) : base(message)
        {
            _errorCode = "e422";
        }

        public BusinessException(string message, string errorCode) : base(message)
        {
            _errorCode = errorCode;
        }

        public string ErrorCode
        {
            get { return _errorCode; }
        }
    }
}
