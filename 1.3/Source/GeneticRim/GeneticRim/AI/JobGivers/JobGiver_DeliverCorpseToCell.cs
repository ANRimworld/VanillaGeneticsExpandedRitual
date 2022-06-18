using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
namespace GeneticRim
{
	public class JobGiver_DeliverCorpseToCell : JobGiver_DeliverPawnToCell
	{
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_DeliverCorpseToCell JobGiver_DeliverCorpseToCell = (JobGiver_DeliverCorpseToCell)base.DeepCopy(resolve);
			JobGiver_DeliverCorpseToCell.addArrivalTagIfTargetIsExtracted = this.addArrivalTagIfTargetIsExtracted;			
			return JobGiver_DeliverCorpseToCell;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Corpse pawn2 = pawn.mindState.duty.focusSecond.Pawn.Corpse;
			pawn.CanReach(pawn2, PathEndMode.Touch, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger));
			if (!pawn.CanReach(pawn2, PathEndMode.Touch, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger)))
			{
				return null;
			}
			Job job = JobMaker.MakeJob(InternalDefOf.GR_DeliverCorpseToCell, pawn2, pawn.mindState.duty.focus, pawn.mindState.duty.focusThird);
			job.locomotionUrgency = PawnUtility.ResolveLocomotion(pawn, this.locomotionUrgency);
			job.expiryInterval = this.jobMaxDuration;
			job.count = 1;
			job.ritualTag = "Arrived";
			return job;
		}

		public bool addArrivalTagIfTargetIsExtracted = false;
	}	
}
