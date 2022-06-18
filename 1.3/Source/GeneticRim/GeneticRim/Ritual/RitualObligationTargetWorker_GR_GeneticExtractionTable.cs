using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace GeneticRim
{
    public class RitualObligationTargetWorker_GR_GeneticExtractionTable : RitualObligationTargetWorker_ThingDef
    {
        public RitualObligationTargetWorker_GR_GeneticExtractionTable()
        {
        }
        public RitualObligationTargetWorker_GR_GeneticExtractionTable(RitualObligationTargetFilterDef def) : base(def)
        {
        }
        protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
        {
            
            if (!base.CanUseTargetInternal(target, obligation).canUse)
            {
                
                return false;               
            }
            
            Thing thing = target.Thing;
           
            CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			if (compPowerTrader != null)
			{
				if (compPowerTrader.PowerNet == null || !compPowerTrader.PowerNet.HasActivePowerSource)
				{
                    return "GR_ExtractorNeedsPower".Translate();
                    
                }				
			}
            
            return true;
		}
        public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
        {
            
            yield return "GR_GrinderRitualInfo".Translate();
            yield break;
        }
    }
}
