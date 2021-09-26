namespace RocketLeague.ReplayLib.Classes.TAGame
{
    [NetFieldExportGroup("TAGame.Ball_Breakout_TA")]
    public class Ball_Breakout_TA
    {
        [NetFieldExport("AppliedDamage", RepLayoutCmdType.)]
        public string AppliedDamage { get; set; }
        [NetFieldExport("DamageIndex")]
        public string DamageIndex { get; set; }
        [NetFieldExport("LastTeamTouch")]
        public string LastTeamTouch { get; set; }
    }
}