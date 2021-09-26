using System;

namespace RocketLeague.ReplayLib.Exceptions
{
    public class InvalidReplayException : Exception
    {
        public InvalidReplayException()
        {
        }

        public InvalidReplayException(string msg) : base(msg)
        {
        }
    }
}