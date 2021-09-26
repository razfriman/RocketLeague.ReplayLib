namespace RocketLeague.ReplayLib.NetFieldExports.Archetypes.Ball
{
    public class Ball_BasketBall_Mutator : TAGame.Ball_TA { }
    
    public class Ball_Basketball : TAGame.Ball_TA { }
    public class Ball_BasketBall : TAGame.Ball_TA { }
    public class Ball_Beachball : TAGame.Ball_TA { }
    public class Ball_Breakout : TAGame.Ball_Breakout_TA { }
    public class Ball_Default : TAGame.Ball_TA { }
    public class Ball_Trajectory : TAGame.Ball_TA { }
    public class Ball_Haunted : TAGame.Ball_Haunted_TA { }
    public class Ball_Puck : TAGame.Ball_TA { }
    public class Ball_Anniversary : TAGame.Ball_TA { }
    public class CubeBall : TAGame.Ball_TA { }
    public class Ball_Training : TAGame.Ball_TA { }
    public class Ball_Football : TAGame.Ball_TA { }
    public class Ball_God : TAGame.Ball_God_TA { }

}

namespace RocketLeague.ReplayLib.NetFieldExports.Archetypes.Car
{
    public class Car_Default : TAGame.Car_TA { }
}

namespace RocketLeague.ReplayLib.NetFieldExports.Archetypes.CarComponents
{
    public class CarComponent_Boost : TAGame.CarComponent_Boost_TA { }
    public class CarComponent_Dodge : TAGame.CarComponent_Dodge_TA { }
    public class CarComponent_DoubleJump : TAGame.CarComponent_DoubleJump_TA { }
    public class CarComponent_FlipCar : TAGame.CarComponent_FlipCar_TA { }
    public class CarComponent_Jump : TAGame.CarComponent_Jump_TA { }
}

namespace RocketLeague.ReplayLib.NetFieldExports.Archetypes.GameEvent
{
    public class GameEvent_Basketball : TAGame.GameEvent_Soccar_TA { }
    public class GameEvent_BasketballPrivate : TAGame.GameEvent_SoccarPrivate_TA { }
    public class GameEvent_BasketballSplitscreen : TAGame.GameEvent_SoccarSplitscreen_TA { }
    public class GameEvent_Breakout : TAGame.GameEvent_Soccar_TA { }
    public class GameEvent_Hockey : TAGame.GameEvent_Soccar_TA { }
    
    public class GameEvent_HockeyPrivate : TAGame.GameEvent_SoccarPrivate_TA { }
    public class GameEvent_HockeySplitscreen : TAGame.GameEvent_SoccarSplitscreen_TA { }
    public class GameEvent_Items : TAGame.GameEvent_Soccar_TA { }
    public class GameEvent_Season : TAGame.GameEvent_Season_TA
    {
        public TAGame.Car_TA CarArchetype { get; set; }
    }
    public class GameEvent_Soccar : TAGame.GameEvent_Soccar_TA { }
    public class GameEvent_SoccarLan : TAGame.GameEvent_Soccar_TA { }
    public class GameEvent_SoccarPrivate : TAGame.GameEvent_SoccarPrivate_TA { }
    public class GameEvent_SoccarSplitscreen : TAGame.GameEvent_SoccarSplitscreen_TA { }
}

namespace RocketLeague.ReplayLib.NetFieldExports.Archetypes.SpecialPickups
{
    public class SpecialPickup_BallFreeze : TAGame.SpecialPickup_BallFreeze_TA { }
    public class SpecialPickup_BallGrapplingHook : TAGame.SpecialPickup_GrapplingHook_TA { }
    public class SpecialPickup_BallLasso : TAGame.SpecialPickup_BallLasso_TA { }
    public class SpecialPickup_BallSpring : TAGame.SpecialPickup_BallCarSpring_TA { }
    public class SpecialPickup_BallVelcro : TAGame.SpecialPickup_BallVelcro_TA { }
    public class SpecialPickup_Batarang : TAGame.SpecialPickup_Batarang_TA { }
    public class SpecialPickup_BoostOverride : TAGame.SpecialPickup_BoostOverride_TA { }
    public class SpecialPickup_CarSpring : TAGame.SpecialPickup_BallCarSpring_TA { }
    public class SpecialPickup_GravityWell : TAGame.SpecialPickup_BallGravity_TA { }
    public class SpecialPickup_StrongHit : TAGame.SpecialPickup_HitForce_TA { }
    public class SpecialPickup_Swapper : TAGame.SpecialPickup_Swapper_TA { }
    public class SpecialPickup_Tornado : TAGame.SpecialPickup_Tornado_TA { }
    public class SpecialPickup_HauntedBallBeam : TAGame.SpecialPickup_HauntedBallBeam_TA { }
    public class SpecialPickup_Rugby : TAGame.SpecialPickup_Rugby_TA { }
    public class SpecialPickup_Football : TAGame.SpecialPickup_Football_TA { }
}

namespace RocketLeague.ReplayLib.NetFieldExports.Archetypes.Teams
{
    public class Team0 : TAGame.Team_Soccar_TA { }
    public class Team1 : TAGame.Team_Soccar_TA { }
}

namespace RocketLeague.ReplayLib.NetFieldExports.Archetypes.Tutorial
{
    public class Cannon : TAGame.Cannon_TA { }
}

namespace RocketLeague.ReplayLib.NetFieldExports.Archetypes.Tutorial
{
    public class Cannon : TAGame.Cannon_TA { }
}

//         ("GameInfo_Basketball.GameInfo.GameInfo_Basketball:GameReplicationInfoArchetype", "TAGame.GRI_TA"),
//         ("GameInfo_Breakout.GameInfo.GameInfo_Breakout:GameReplicationInfoArchetype", "TAGame.GRI_TA"),
//         ("Gameinfo_Hockey.GameInfo.Gameinfo_Hockey:GameReplicationInfoArchetype", "TAGame.GRI_TA"),
//         ("GameInfo_Items.GameInfo.GameInfo_Items:GameReplicationInfoArchetype", "TAGame.GRI_TA"),
//         ("GameInfo_Season.GameInfo.GameInfo_Season:GameReplicationInfoArchetype", "TAGame.GRI_TA"),
//         ("GameInfo_Soccar.GameInfo.GameInfo_Soccar:GameReplicationInfoArchetype", "TAGame.GRI_TA"),
//         ("GameInfo_Tutorial.GameInfo.GameInfo_Tutorial:GameReplicationInfoArchetype", "TAGame.GRI_TA"),
//         ("gameinfo_godball.GameInfo.gameinfo_godball:GameReplicationInfoArchetype", "TAGame.GRI_TA"),
//         ("GameInfo_FootBall.GameInfo.GameInfo_FootBall:GameReplicationInfoArchetype", "TAGame.GRI_TA"),
//         ("GameInfo_Tutorial.GameEvent.GameEvent_Tutorial_Aerial", "TAGame.GameEvent_Tutorial_TA"),

//         ("TheWorld:PersistentLevel.BreakOutActor_Platform_TA", "TAGame.BreakOutActor_Platform_TA"),
//         ("TheWorld:PersistentLevel.CrowdActor_TA", "TAGame.CrowdActor_TA"),
//         ("TheWorld:PersistentLevel.CrowdManager_TA", "TAGame.CrowdManager_TA"),
//         ("TheWorld:PersistentLevel.InMapScoreboard_TA", "TAGame.InMapScoreboard_TA"),
//         ("TheWorld:PersistentLevel.VehiclePickup_Boost_TA", "TAGame.VehiclePickup_Boost_TA"),

//         ("Haunted_TrainStation_P.TheWorld:PersistentLevel.HauntedBallTrapTrigger_TA_1", "TAGame.HauntedBallTrapTrigger_TA"),
//         ("Haunted_TrainStation_P.TheWorld:PersistentLevel.HauntedBallTrapTrigger_TA_0", "TAGame.HauntedBallTrapTrigger_TA"),

//         ("gameinfo_godball.GameInfo.gameinfo_godball:Archetype", "TAGame.GameEvent_GodBall_TA"),

//         ("GameInfo_FootBall.GameInfo.GameInfo_FootBall:Archetype", "TAGame.GameEvent_Football_TA"),
//     ]
// }
}