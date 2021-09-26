using System;
using RocketLeague.ReplayLib;

var file = "/Users/razfriman/Desktop/f635c7e8-51ce-43cb-b43e-cb46a574fc70.replay";
var replayReader = new RocketLeagueReplayReader(null);
var replay = replayReader.ReadReplay(file);
Console.WriteLine("Done");