using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class InvalidCommandException : Exception
    {
        public const string InvalidCommand = "The command {0} is invalid";

        public InvalidCommandException()
            : base(InvalidCommand)
        {
        }

        public InvalidCommandException(string message)
            : base(message)
        {
        }
    }
}
