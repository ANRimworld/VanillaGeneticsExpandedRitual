using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace GeneticRim
{
    
    public class RitualOutcomeEffectWorker_GRExtractGenome : RitualOutcomeEffectWorker_FromQuality
    {
        public RitualOutcomeEffectWorker_GRExtractGenome() 
        { 
        }
        public RitualOutcomeEffectWorker_GRExtractGenome(RitualOutcomeEffectDef def) : base(def)
        {
        }
        protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
        {
            extraOutcomeDesc = null;
            Pawn corpse = jobRitual.PawnWithRole("GR_RitualRoleCorpse");           

            ThingDef thingToSpawn = null;
            
            if(outcome.positivityIndex == -2)
            {
                //Failed Extraction
                extraOutcomeDesc = "GR_RitaulExtractGenomeFail".Translate(jobRitual.Ritual.Label.Named("RITUAL"), corpse.Name.Named("CORPSE"));
                corpse.Corpse.Destroy();
                return;
            }
            //Get Genome Type	
            HashSet<ExtractableAnimalsList> allLists = DefDatabase<ExtractableAnimalsList>.AllDefsListForReading.ToHashSet();
            foreach (ExtractableAnimalsList individualList in allLists)
            {                    
                if ((individualList.needsHumanLike && corpse.def.race.Humanlike) || (individualList.extractableAnimals.Contains(corpse.def) == true))
                {
                    thingToSpawn = individualList.itemProduced;
                }
            }

            if (thingToSpawn != null)
			{
				Thing newThing = ThingMaker.MakeThing(thingToSpawn);
                if (outcome.BestPositiveOutcome(jobRitual))
                {
                    //Double genome
                    newThing.stackCount = 2;
                    extraOutcomeDesc = "GR_RitaulExtractAmazing".Translate(jobRitual.Ritual.Label.Named("RITUAL"), corpse.Name.Named("CORPSE"));
                }
                GenPlace.TryPlaceThing(newThing, corpse.Corpse.Position, corpse.Corpse.Map, ThingPlaceMode.Near);
            }
            //Delete Corpse
            corpse.Corpse.Destroy();
        }
    }
}
