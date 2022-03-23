﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticRim
{
    using RimWorld;
    using RimWorld.Planet;
    using Verse;


    public class WorldComponent_RoamingMonstrosities : WorldComponent
    {
        public int tickCounter;
        public int ticksToNextAssault = 60000 * 15;


        public WorldComponent_RoamingMonstrosities(World world) : base(world)
        {
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();



        }

        public override void WorldComponentTick()
        {
            base.WorldComponentTick();

            if (Current.Game.storyteller.difficultyDef != DifficultyDefOf.Peaceful)
            {


                if (tickCounter > ticksToNextAssault)
                {
                    if (Find.FactionManager.FirstFactionOfDef(InternalDefOf.GR_RoamingMonstrosities) != null)
                    {
                        IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatBig, this.world);
                        IncidentDef def = InternalDefOf.GR_ManhunterMonstrosities;
                        if (def.Worker.CanFireNow(parms))
                        {
                            def.Worker.TryExecute(parms);
                        }
                        ticksToNextAssault = 60000 * Rand.RangeInclusive(10,30);
                        tickCounter = 0;
                    }

                    

                }
                tickCounter++;
            }



            
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref this.tickCounter, nameof(this.tickCounter));
            Scribe_Values.Look(ref this.ticksToNextAssault, nameof(this.ticksToNextAssault));
        }
    }
}