using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Sound;
using Verse.AI.Group;
using RimWorld;

namespace GeneticRim
{
    public class RitualStageAction_GRExtractGenome :RitualStageAction
    {

		public override void Apply(LordJob_Ritual ritual)
		{
			//Start Sound
			this.behavior = (RitualBehaviorWorker_ExtractGenome)ritual.Ritual.behavior; 
			Sustainer sustainer = behavior.SoundPlaying;
			selectedtarget = ritual.selectedTarget;
			if (sustainer == null)
            {
				//Create the sound and start playing it
                sustainer = NewSustainer(selectedtarget);
				behavior.soundPlaying = sustainer;				
			}
			//Start Effect
			Pawn pawn = ritual.PawnWithRole("moralist");
			ApplyToPawn(ritual, pawn);


		}
		public override void ApplyToPawn(LordJob_Ritual ritual, Pawn pawn)
		{

			Effecter effecter = behavior.effecter;
			if(effecter == null) 
			{
				effecter = effect.Spawn();
				effecter.Trigger(pawn, selectedtarget);
				behavior.effecter = effecter;
			}
			
		}
		private Sustainer NewSustainer(TargetInfo selectedtarget)
        {
            return this.sound.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(selectedtarget.Cell, selectedtarget.Map, false), MaintenanceType.PerTick));

        }

        public override void ExposeData()
		{
			Scribe_Defs.Look<SoundDef>(ref this.sound, "sound");
			Scribe_Defs.Look<EffecterDef>(ref this.effect, "effect");
		}
		private TargetInfo selectedtarget;
		private RitualBehaviorWorker_ExtractGenome behavior;
		public SoundDef sound;

		public EffecterDef effect;

	}
}

