using System;
namespace server.exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base($"404: {message}"){

        }
    }
}