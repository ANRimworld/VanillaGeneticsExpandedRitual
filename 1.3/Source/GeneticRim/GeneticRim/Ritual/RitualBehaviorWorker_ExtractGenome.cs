using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Sound;
using Verse.AI;
using Verse.AI.Group;
using RimWorld;


namespace GeneticRim
{
    public class RitualBehaviorWorker_ExtractGenome : RitualBehaviorWorker
    {
        public RitualBehaviorWorker_ExtractGenome()
        {
        }
        public RitualBehaviorWorker_ExtractGenome(RitualBehaviorDef def) : base(def)
        {
        }
        //Found a need Try Execute on needs to be overriden to not add corpse to lord and prevent other fails from using the corpses pawn
        public override void TryExecuteOn(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments, bool playerForced = false)
        {
            if (CanStartRitualNow(target, ritual, null, assignments.ForcedRolesForReading) != null)
            {
                return;
            }
            if (!base.CanExecuteOn(target, obligation))
            {
                return;
            }
            LordJob_Ritual lordJob = (LordJob_Ritual)this.CreateLordJob(target, organizer, ritual, obligation, assignments);
            LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, target.Map, assignments.Participants.Where(delegate (Pawn p)
            {
                bool flag = p.Dead;
                return !flag;
            }));

            GR_ExtractGenomePreparePawn(assignments);
            base.PostExecute(target, organizer, ritual, obligation, assignments);
            if (playerForced)
            {
                foreach (Pawn pawn in assignments.Participants)
                {
                    if (!pawn.Dead) 
                    { 
                        pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, false, true);
                    }
                }
            }

        }
        public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
        {
            if (ritual.activeObligations == null)
            {
                return null;
            }
            bool flag = false;
            List<Thing> list = target.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
            for (int i = 0; i < list.Count; i++)
            {
                Pawn innerPawn = ((Corpse)list[i]).InnerPawn;
                if (innerPawn.CanBeBuried() && innerPawn.IsColonist)
                {
                    
                    if (innerPawn.ideo.Ideo.HasMeme(InternalDefOf.GR_MadScientists))
                    {
                        if (forcedForRole == null)
                        {
                            forcedForRole = new Dictionary<string, Pawn>();
                        }
                        if (forcedForRole.TryGetValue("GR_RitualRoleCorpse")==null)
                        {
                            forcedForRole.Add("GR_RitualRoleCorpse", innerPawn);                           
                        }
                        flag = true;
                    }
                }
            }
            if (!flag)
            {
                foreach (RitualObligation obligation in ritual.activeObligations)
                {
                    if(obligation.targetB.ThingDestroyed) //Corpse was destroyed don't have great handling for this atm. Just removing from active obligations. In future possibly add standard emptygrave funeral like base game?
                    {
                        ritual.activeObligations.Remove(obligation); //Would also prefer this to be on a trigger, nothing in standard rituals for remove obligation trigger. Maybe with custom ritual obligation trigger for not adding.

                    }
                }
                return "GR_DisabledCorpseInaccessible".Translate(); 

            }
            return base.CanStartRitualNow(target, ritual, selectedPawn, forcedForRole); //Forced role just to by pass first set of no corpse checks
        }
        public void GR_ExtractGenomePreparePawn(RitualRoleAssignments assignments)
        {
            foreach (Pawn participant in assignments.Participants)
            {
                if (participant.drafter != null)
                {
                    participant.drafter.Drafted = false;
                }
                if (!participant.Awake())
                {
                    RestUtility.WakeUp(participant);
                }
            }
        }
        public override void Tick(LordJob_Ritual ritual)
        {
            base.Tick(ritual);

            if (soundPlaying != null)
            {
                soundPlaying.Maintain();
            }
            if(effecter != null)
            {
                effecter.EffectTick(ritual.PawnWithRole("moralist"), ritual.selectedTarget);
            }
        }
        public override Sustainer SoundPlaying
        {
            get
            {
                return this.soundPlaying;
            }
        }
        public override void Cleanup(LordJob_Ritual ritual)
        {
            if(this.soundPlaying != null)
            {
                soundPlaying.End();
                soundPlaying = null;
            }
            if(effecter != null)
            {
                effecter.Cleanup();
                effecter = null;
            }
        }

        public Sustainer soundPlaying; //Cant override set        
        public Effecter effecter;
    }
}
