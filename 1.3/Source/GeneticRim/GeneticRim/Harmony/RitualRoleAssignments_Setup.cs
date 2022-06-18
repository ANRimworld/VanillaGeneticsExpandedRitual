using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using RimWorld.Planet;
using System;


namespace GeneticRim
{

    [HarmonyPatch(typeof(RitualRoleAssignments))]
    [HarmonyPatch("Setup")]
    //This patch is to get the corpses in the list of selectable pawns
    public static class GeneticRim_RitualRoleAssignments_Setup_Patch
    {
        [HarmonyPrefix]
        public static void SelectViableCorpse(RitualRoleAssignments __instance, ref List<Pawn> allPawns, ref Dictionary<string, Pawn> forcedRoles)
        {


            //Access tools
            Precept_Ritual ritual = (Precept_Ritual)AccessTools.Field(typeof(RitualRoleAssignments),"ritual").GetValue(__instance);           
            Pawn selectedPawn = (Pawn)AccessTools.Field(typeof(RitualRoleAssignments), "selectedPawn").GetValue(__instance);
                        
            
            if (ritual.def.defName == "GR_ExtractorFuneral")           
            {
                Map map = allPawns[0].Map; //Grab the map of the pawns to rebuild it
                List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse);

                for (int i = 0; i < list.Count; i++) 
                {

                    Pawn innerPawn = ((Corpse)list[i]).InnerPawn;
                    if (innerPawn.CanBeBuried())
                    {
                        ;
                        if (innerPawn.ideo.Ideo.HasMeme(InternalDefOf.GR_MadScientists))
                        {
                            
                            if (!allPawns.Contains(innerPawn))
                            {
                               
                                allPawns.Add(innerPawn);

                            }
                            
                        }
                    }
                }
            }

        }

    }

}
