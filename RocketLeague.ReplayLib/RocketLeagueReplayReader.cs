using Microsoft.Extensions.Logging;
using RocketLeague.ReplayLib.Models;

namespace RocketLeague.ReplayLib
{
    public class RocketLeagueReplay : Replay
    {
        
    }

    public class RocketLeagueReplayReader : ReplayReader<RocketLeagueReplay>
    {
        public RocketLeagueReplayReader(ILogger logger) : base(logger)
        {
        }
    }
}