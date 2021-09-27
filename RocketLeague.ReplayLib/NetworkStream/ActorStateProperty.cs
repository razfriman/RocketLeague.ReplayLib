﻿using System;
using System.Collections.Generic;
using RocketLeague.ReplayLib.Extensions;

namespace RocketLeague.ReplayLib.NetworkStream
{
    public class ActorStateProperty
    {
        public uint PropertyId { get; protected set; }
        public object Data { get; protected set; }

        // These can be looked up, but keeping a reference makes things a lot easier
        public string PropertyName { get; protected set; }
        protected ClassNetCache ClassNetCache;

        private ActorStateProperty()
        {
        }

        protected ActorStateProperty(ActorStateProperty copyFrom)
        {
            PropertyId = copyFrom.PropertyId;
            PropertyName = copyFrom.PropertyName;
            Data = copyFrom.Data;
            ClassNetCache = copyFrom.ClassNetCache;
        }

        public static ActorStateProperty Deserialize(ClassNetCache classMap, string[] objectIndexToName,
            uint engineVersion, uint licenseeVersion, uint netVersion, BitReader br)
        {
            var asp = new ActorStateProperty
            {
                ClassNetCache = classMap
            };

            var maxPropId = classMap.MaxPropertyId;

            asp.PropertyId = br.ReadUInt32Max(maxPropId + 1);
            asp.PropertyName = objectIndexToName[classMap.GetProperty((int)asp.PropertyId).Index];

            asp.Data = new List<object>();

            switch (asp.PropertyName)
            {
                case "TAGame.GameEvent_TA:ReplicatedStateIndex":
                    asp.Data = br
                        .ReadUInt32Max(
                            140); // number is made up, I dont know the max yet // TODO: Revisit this. It might work well enough, but looks fishy
                    break;
                case "TAGame.RBActor_TA:ReplicatedRBState":
                    asp.Data = RigidBodyState.Deserialize(br, netVersion);
                    break;
                case "TAGame.Team_TA:LogoData":
                    asp.Data = LogoData.Deserialize(br);
                    break;
                case "TAGame.CrowdManager_TA:GameEvent":
                case "TAGame.CrowdActor_TA:GameEvent":
                case "TAGame.PRI_TA:PersistentCamera":
                case "TAGame.Team_TA:GameEvent":
                case "TAGame.Ball_TA:GameEvent":
                case "Engine.PlayerReplicationInfo:Team":
                case "Engine.Pawn:PlayerReplicationInfo":
                case "TAGame.PRI_TA:ReplicatedGameEvent":
                case "TAGame.CarComponent_TA:Vehicle":
                case "TAGame.Car_TA:AttachedPickup":
                case "TAGame.SpecialPickup_Targeted_TA:Targeted":
                case "TAGame.CameraSettingsActor_TA:PRI":
                case "TAGame.GameEvent_Soccar_TA:MVP":
                case "TAGame.GameEvent_Soccar_TA:MatchWinner":
                case "TAGame.GameEvent_Soccar_TA:GameWinner":
                case "Engine.ReplicatedActor_ORS:ReplicatedOwner":
                case "TAGame.Car_TA:RumblePickups":
                case "TAGame.RumblePickups_TA:AttachedPickup":
                case "TAGame.SpecialPickup_Football_TA:WeldedBall":
                    asp.Data = ActiveActor.Deserialize(br);
                    break;
                case "TAGame.CrowdManager_TA:ReplicatedGlobalOneShotSound":
                case "TAGame.CrowdActor_TA:ReplicatedOneShotSound":
                case "TAGame.GameEvent_TA:MatchTypeClass":
                case "Engine.GameReplicationInfo:GameClass":
                case "TAGame.GameEvent_Soccar_TA:SubRulesArchetype":
                case "TAGame.Ball_TA:ReplicatedPhysMatOverride":
                    var objectTarget = ObjectTarget.Deserialize(br);
                    asp.Data = objectTarget;
                    break;
                case "Engine.GameReplicationInfo:ServerName":
                case "Engine.PlayerReplicationInfo:PlayerName":
                case "TAGame.Team_TA:CustomTeamName":
                case "Engine.PlayerReplicationInfo:RemoteUserData":
                case "TAGame.GRI_TA:NewDedicatedServerIP":
                case "ProjectX.GRI_X:MatchGUID":
                    asp.Data = br.ReadString();
                    break;
                case "TAGame.GameEvent_Soccar_TA:SecondsRemaining":
                case "TAGame.GameEvent_TA:ReplicatedGameStateTimeRemaining":
                case "TAGame.CrowdActor_TA:ReplicatedCountDownNumber":
                case "TAGame.GameEvent_Team_TA:MaxTeamSize":
                case "Engine.PlayerReplicationInfo:PlayerID":
                case "TAGame.PRI_TA:TotalXP":
                case "TAGame.PRI_TA:MatchScore":
                case "TAGame.GameEvent_Soccar_TA:RoundNum":
                case "TAGame.GameEvent_TA:BotSkill":
                case "TAGame.PRI_TA:MatchShots":
                case "TAGame.PRI_TA:MatchSaves":
                case "ProjectX.GRI_X:ReplicatedGamePlaylist":
                case "Engine.TeamInfo:Score":
                case "Engine.PlayerReplicationInfo:Score":
                case "TAGame.PRI_TA:MatchGoals":
                case "TAGame.PRI_TA:MatchAssists":
                case "TAGame.PRI_TA:Title":
                case "TAGame.GameEvent_TA:ReplicatedStateName":
                case "TAGame.Team_Soccar_TA:GameScore":
                case "TAGame.GameEvent_Soccar_TA:GameTime":
                case "TAGame.CarComponent_Boost_TA:UnlimitedBoostRefCount":
                case "TAGame.CrowdActor_TA:ReplicatedRoundCountDownNumber":
                case "TAGame.Ball_Breakout_TA:DamageIndex":
                case "TAGame.PRI_TA:MatchBreakoutDamage":
                case "TAGame.PRI_TA:BotProductName":
                case "TAGame.GameEvent_TA:ReplicatedRoundCountDownNumber":
                case "TAGame.GameEvent_Soccar_TA:SeriesLength":
                case "TAGame.PRI_TA:SpectatorShortcut":
                case "Engine.Pawn:HealthMax":
                case "TAGame.GameEvent_Soccar_TA:MaxScore":
                case "TAGame.Team_TA:Difficulty":
                case "TAGame.RumblePickups_TA:ConcurrentItemCount":
                case "TAGame.PRI_TA:BotBannerProductID":
                    asp.Data = br.ReadUInt32();
                    break;
                case "ProjectX.GRI_X:ReplicatedGameMutatorIndex":
                case "TAGame.PRI_TA:TimeTillItem":
                case "TAGame.PRI_TA:MaxTimeTillItem":
                    asp.Data = br.ReadInt32();
                    break;
                case "TAGame.VehiclePickup_TA:ReplicatedPickupData":
                    asp.Data = ReplicatedPickupData.Deserialize(br);
                    break;
                case "Engine.PlayerReplicationInfo:Ping":
                case "TAGame.Vehicle_TA:ReplicatedSteer":
                case "TAGame.Vehicle_TA:ReplicatedThrottle"
                    : // 0: full reverse, 128: No throttle.  255 full throttle/boosting
                case "TAGame.PRI_TA:CameraYaw":
                case "TAGame.PRI_TA:CameraPitch":
                case "TAGame.Ball_TA:HitTeamNum":
                case "TAGame.GameEvent_Soccar_TA:ReplicatedScoredOnTeam":
                case "TAGame.CarComponent_Boost_TA:ReplicatedBoostAmount":
                case "TAGame.CameraSettingsActor_TA:CameraPitch":
                case "TAGame.CameraSettingsActor_TA:CameraYaw":
                case "TAGame.PRI_TA:PawnType":
                case "TAGame.Ball_Breakout_TA:LastTeamTouch":
                case "TAGame.Ball_Haunted_TA:LastTeamTouch":
                case "TAGame.PRI_TA:ReplicatedWorstNetQualityBeyondLatency":
                case "TAGame.GameEvent_Soccar_TA:ReplicatedServerPerformanceState":
                case "TAGame.Ball_Haunted_TA:TotalActiveBeams":
                case "TAGame.Ball_Haunted_TA:DeactivatedGoalIndex":
                case "TAGame.Ball_Haunted_TA:ReplicatedBeamBrokenValue":
                    asp.Data = br.ReadByte();
                    break;
                case "TAGame.PRI_TA:SkillTier":
                    asp.Data = br
                        .ReadUInt32Max(
                            500); // 9 bits. I picked a value that works, but could just be 1 bit + 1 byte instead of a single value.
                    break;
                case "Engine.Actor:Location":
                case "TAGame.CarComponent_Dodge_TA:DodgeTorque":
                    asp.Data = Vector3D.Deserialize(br, netVersion);
                    break;
                case "Engine.Actor:bCollideWorld":
                case "Engine.PlayerReplicationInfo:bReadyToPlay":
                case "TAGame.Vehicle_TA:bReplicatedHandbrake":
                case "TAGame.Vehicle_TA:bDriving":
                case "Engine.Actor:bNetOwner":
                case "Engine.Actor:bBlockActors":
                case "TAGame.GameEvent_TA:bHasLeaveMatchPenalty":
                case "TAGame.PRI_TA:bUsingBehindView":
                case "TAGame.PRI_TA:bUsingSecondaryCamera": // Ball cam on when true
                case "TAGame.GameEvent_TA:ActivatorCar":
                case "TAGame.GameEvent_Soccar_TA:bOverTime":
                case "ProjectX.GRI_X:bGameStarted":
                case "Engine.Actor:bCollideActors":
                case "TAGame.PRI_TA:bReady":
                case "TAGame.RBActor_TA:bFrozen":
                case "Engine.Actor:bHidden":
                case "TAGame.CarComponent_FlipCar_TA:bFlipRight":
                case "Engine.PlayerReplicationInfo:bBot":
                case "Engine.PlayerReplicationInfo:bWaitingPlayer":
                case "TAGame.RBActor_TA:bReplayActor":
                case "TAGame.PRI_TA:bIsInSplitScreen":
                case "Engine.GameReplicationInfo:bMatchIsOver":
                case "TAGame.CarComponent_Boost_TA:bUnlimitedBoost":
                case "Engine.PlayerReplicationInfo:bIsSpectator":
                case "TAGame.GameEvent_Soccar_TA:bBallHasBeenHit":
                case "TAGame.CameraSettingsActor_TA:bUsingSecondaryCamera":
                case "TAGame.CameraSettingsActor_TA:bUsingBehindView":
                case "TAGame.PRI_TA:bOnlineLoadoutSet":
                case "TAGame.PRI_TA:bMatchMVP":
                case "TAGame.PRI_TA:bOnlineLoadoutsSet":
                case "TAGame.RBActor_TA:bIgnoreSyncing":
                case "TAGame.SpecialPickup_BallVelcro_TA:bHit":
                case "TAGame.GameEvent_TA:bCanVoteToForfeit":
                case "TAGame.SpecialPickup_BallVelcro_TA:bBroken":
                case "TAGame.GameEvent_Team_TA:bForfeit":
                case "TAGame.PRI_TA:bUsingItems":
                case "TAGame.VehiclePickup_TA:bNoPickup":
                case "TAGame.CarComponent_Boost_TA:bNoBoost":
                case "TAGame.PRI_TA:PlayerHistoryValid":
                case "TAGame.GameEvent_Soccar_TA:bUnlimitedTime":
                case "TAGame.GameEvent_Soccar_TA:bClubMatch":
                case "TAGame.GameEvent_Soccar_TA:bMatchEnded":
                case "TAGame.GameEvent_TA:bAllowReadyUp":
                case "Engine.Actor:bTearOff":
                case "Engine.PlayerReplicationInfo:bTimedOut":
                case "TAGame.CameraSettingsActor_TA:bMouseCameraToggleEnabled":
                case "TAGame.CameraSettingsActor_TA:bUsingSwivel":
                case "TAGame.Ball_Haunted_TA:bIsBallBeamed":
                case "TAGame.SpecialPickup_Rugby_TA:bBallWelded":
                case "TAGame.PRI_TA:bIsDistracted":
                case "TAGame.GameEvent_TA:bIsBotMatch":
                    asp.Data = br.ReadBit();
                    break;
                case "TAGame.CarComponent_TA:ReplicatedActive":
                    // The car component is active if (ReplicatedValue%2)!=0 
                    // For now I am only adding that logic to the JSON serializer
                    asp.Data = br.ReadByte();
                    break;
                case "Engine.PlayerReplicationInfo:UniqueId":
                    asp.Data = UniqueId.Deserialize(br, licenseeVersion, netVersion);
                    break;
                case "TAGame.PRI_TA:PartyLeader":
                    asp.Data = PartyLeader.Deserialize(br, licenseeVersion, netVersion);
                    break;
                case "TAGame.PRI_TA:ClientLoadout":
                    asp.Data = ClientLoadout.Deserialize(br);
                    break;
                case "TAGame.PRI_TA:CameraSettings":
                case "TAGame.CameraSettingsActor_TA:ProfileSettings":
                    asp.Data = CameraSettings.Deserialize(br, engineVersion, licenseeVersion);
                    break;
                case "TAGame.Car_TA:TeamPaint":
                    asp.Data = TeamPaint.Deserialize(br);
                    break;
                case "ProjectX.GRI_X:GameServerID":
                    asp.Data = br.ReadBytes(8);
                    break;
                case "ProjectX.GRI_X:Reservations":
                    asp.Data = Reservation.Deserialize(engineVersion, licenseeVersion, netVersion, br);
                    break;
                case "TAGame.Car_TA:ReplicatedDemolish":
                    asp.Data = ReplicatedDemolish.Deserialize(br, netVersion);
                    break;
                case "TAGame.Car_TA:ReplicatedDemolishGoalExplosion":
                    asp.Data = ReplicatedDemolishGoalExplosion.Deserialize(br, netVersion);
                    break;
                case "TAGame.GameEvent_Soccar_TA:ReplicatedMusicStinger":
                    asp.Data = ReplicatedMusicStinger.Deserialize(br);
                    break;
                case "TAGame.CarComponent_FlipCar_TA:FlipCarTime":
                case "TAGame.Ball_TA:ReplicatedBallScale":
                case "TAGame.CarComponent_Boost_TA:RechargeDelay":
                case "TAGame.CarComponent_Boost_TA:RechargeRate":
                case "TAGame.Ball_TA:ReplicatedAddedCarBounceScale":
                case "TAGame.Ball_TA:ReplicatedBallMaxLinearSpeedScale":
                case "TAGame.Ball_TA:ReplicatedWorldBounceScale":
                case "TAGame.CarComponent_Boost_TA:BoostModifier":
                case "Engine.Actor:DrawScale":
                case "TAGame.CrowdActor_TA:ModifiedNoise":
                case "TAGame.CarComponent_TA:ReplicatedActivityTime":
                case "TAGame.SpecialPickup_BallFreeze_TA:RepOrigSpeed":
                case "TAGame.SpecialPickup_BallVelcro_TA:AttachTime":
                case "TAGame.SpecialPickup_BallVelcro_TA:BreakTime":
                case "TAGame.Car_TA:AddedCarForceMultiplier":
                case "TAGame.Car_TA:AddedBallForceMultiplier":
                case "TAGame.PRI_TA:SteeringSensitivity":
                case "TAGame.Car_TA:ReplicatedCarScale":
                case "Engine.WorldInfo:WorldGravityZ":
                case "Engine.WorldInfo:TimeDilation":
                case "TAGame.Ball_God_TA:TargetSpeed":
                    asp.Data = br.ReadFloat();
                    break;
                case "TAGame.GameEvent_SoccarPrivate_TA:MatchSettings":
                    asp.Data = PrivateMatchSettings.Deserialize(br);
                    break;
                case "TAGame.PRI_TA:ClientLoadoutOnline":
                    asp.Data = ClientLoadoutOnline.Deserialize(br, engineVersion, licenseeVersion, objectIndexToName);
                    break;
                case "TAGame.GameEvent_TA:GameMode":
                    if (engineVersion >= 868 && licenseeVersion >= 12)
                    {
                        asp.Data = br.ReadByte();
                    }
                    else
                    {
                        asp.Data = br.ReadUInt32Max(4);
                    }

                    break;
                case "TAGame.PRI_TA:ClientLoadoutsOnline":
                    asp.Data = ClientLoadoutsOnline.Deserialize(br, engineVersion, licenseeVersion, objectIndexToName);
                    break;
                case "TAGame.PRI_TA:ClientLoadouts":
                    asp.Data = ClientLoadouts.Deserialize(br);
                    break;
                case "TAGame.Team_TA:ClubColors":
                case "TAGame.Car_TA:ClubColors":
                    asp.Data = ClubColors.Deserialize(br);
                    break;
                case "TAGame.RBActor_TA:WeldedInfo":
                    asp.Data = WeldedInfo.Deserialize(br, netVersion);
                    break;
                case "TAGame.BreakOutActor_Platform_TA:DamageState":
                    asp.Data = DamageState.Deserialize(br, netVersion);
                    break;
                case "TAGame.Ball_Breakout_TA:AppliedDamage":
                    asp.Data = AppliedDamage.Deserialize(br, netVersion);
                    break;
                case "TAGame.Ball_TA:ReplicatedExplosionData":
                    asp.Data = ReplicatedExplosionData.Deserialize(br, netVersion);
                    break;
                case "TAGame.Ball_TA:ReplicatedExplosionDataExtended":
                    asp.Data = ReplicatedExplosionDataExtended.Deserialize(br, netVersion);
                    break;
                case "TAGame.PRI_TA:SecondaryTitle":
                case "TAGame.PRI_TA:PrimaryTitle":
                    asp.Data = Title.Deserialize(br);
                    break;
                case "TAGame.PRI_TA:PlayerHistoryKey":
                    // Betting ReadUInt32Max is more likely, since 14 bits is a weird number
                    asp.Data = br.ReadUInt32FromBits(14);
                    break;
                case "TAGame.GameEvent_Soccar_TA:ReplicatedStatEvent":
                    asp.Data = ReplicatedStatEvent.Deserialize(br);
                    break;
                case "TAGame.Team_TA:ClubID":
                case "TAGame.PRI_TA:ClubID":
                case "TAGame.MaxTimeWarningData_TA:EndGameWarningEpochTime":
                case "TAGame.MaxTimeWarningData_TA:EndGameEpochTime":
                    asp.Data = br.ReadUInt64();
                    break;
                case "TAGame.PRI_TA:RepStatTitles":
                    asp.Data = RepStatTitle.Deserialize(br);
                    break;
                case "TAGame.VehiclePickup_TA:NewReplicatedPickupData":
                    asp.Data = NewReplicatedPickupData.Deserialize(br);
                    break;
                case "TAGame.Car_TA:ReplicatedDemolish_CustomFX":
                    asp.Data = ReplicatedDemolishCustomFx.Deserialize(br, netVersion);
                    break;
                case "TAGame.RumblePickups_TA:PickupInfo":
                    asp.Data = PickupInfo.Deserialize(br);
                    break;
                default:
                    throw new NotSupportedException(
                        $"Unknown property {asp.PropertyName}. Next bits in the data are {br.GetBits(br.Position, Math.Min(4096, br.Length - br.Position)).ToBinaryString()}. Figure it out!");
            }

            return asp;
        }

        public string ToDebugString()
        {
            var s = $"Property: ID {PropertyId} Name {PropertyName}\r\n";
            s += "    Data: " + string.Join(", ", Data) + "\r\n";

            return s;
        }
    }
}