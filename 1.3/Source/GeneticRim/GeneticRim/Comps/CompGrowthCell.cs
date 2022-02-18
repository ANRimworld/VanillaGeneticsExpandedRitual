﻿namespace GeneticRim
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Verse;
    using Verse.AI;

    public class CompGrowthCell : ThingComp
    {
        public ThingDef genomeDominant;
        public ThingDef genomeSecondary;
        public ThingDef genoframe;
        public ThingDef booster;

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            foreach (FloatMenuOption option in base.CompFloatMenuOptions(selPawn)) 
                yield return option;

            foreach (Building building in this.parent.Map.listerBuildings.allBuildingsColonist)
            {
                if (building.TryGetComp<CompElectroWomb>()?.Free ?? false)
                {
                    yield return new FloatMenuOption("GR_GrowthCell_InsertInElectroWomb".Translate(building.LabelCap), () =>
                                                                                                                       {
                                                                                                                           Job makeJob = JobMaker.MakeJob(InternalDefOf.GR_InsertGrowthCell, building,
                                                                                                                               this.parent);
                                                                                                                           makeJob.haulMode = HaulMode.ToCellNonStorage;
                                                                                                                           makeJob.count    = 1;
                                                                                                                           selPawn.jobs.StartJob(makeJob, JobCondition.InterruptForced);
                                                                                                                       });
                }
            }
        }

        public override string CompInspectStringExtra()
        {
            StringBuilder sb = new StringBuilder(base.CompInspectStringExtra());

            sb.AppendLine("GR_GrowthCell_InspectDominant".Translate(this.genomeDominant.LabelCap));
            sb.AppendLine("GR_GrowthCell_InspectSecondary".Translate(this.genomeSecondary.LabelCap));
            sb.AppendLine("GR_GrowthCell_InspectGenoframe".Translate(this.genoframe.LabelCap));
            if(this.booster != null)
                sb.AppendLine("GR_GrowthCell_InspectBooster".Translate(this.booster.LabelCap));

            return sb.ToString().Trim();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_Defs.Look(ref this.genomeDominant, nameof(this.genomeDominant));
            Scribe_Defs.Look(ref this.genomeSecondary, nameof(this.genomeSecondary));
            Scribe_Defs.Look(ref this.genoframe, nameof(this.genoframe));
            Scribe_Defs.Look(ref this.booster, nameof(this.booster));
        }
    }
}