﻿
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
namespace GeneticRim
{
	public class JobGiver_Talk : ThinkNode_JobGiver
	{
		private const float MaxTalkingDistance = 10f;

		protected override Job TryGiveJob(Pawn pawn)
		{
			
			if (!(from p in pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction)
				  where !p.NonHumanlikeOrWildMan() && p != pawn && p.Position.InHorDistOf(pawn.Position, MaxTalkingDistance) && pawn.GetRoom() == p.GetRoom() && !p.Position.IsForbidden(pawn) && p.CanCasuallyInteractNow()
				  select p).TryRandomElement(out var result))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(InternalDefOf.GR_HumanoidHybridTalk, result);
			job.locomotionUrgency = LocomotionUrgency.Walk;
			job.expiryInterval = 3000;
			return job;
		}
	}
}