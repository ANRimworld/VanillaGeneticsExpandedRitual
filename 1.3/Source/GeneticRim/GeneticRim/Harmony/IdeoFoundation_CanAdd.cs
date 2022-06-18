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

    [HarmonyPatch(typeof(Ideo))]
    [HarmonyPatch("AddPrecept")]
    //This patch is to prevent the game from adding 2 funerals
    //Im sure there's a better spot/way for this but i cant find it T_T since IdeoFoundation Canadd doesnt feel like being used for rituals
    public static class GeneticRim_Ideo_AddPrecept
    {

        [HarmonyPrefix]
        //Prefix is for a vanilla bug with required memes and Ideo reformation
        public static void Prefix(Ideo __instance, Precept precept, ref RitualPatternDef fillWith, ref bool init)
        {
            if(fillWith != null)
            {
                return;
            }
            Precept_Ritual ritual;
            if ((ritual = (precept as Precept_Ritual)) != null)
            {
                init = true;
                fillWith = ritual.def.ritualPatternBase;
            }
        }

        [HarmonyPostfix]
        public static void Postfix(Ideo __instance, Precept precept)
        {
            if (precept.def.issue.defName != "Ritual") return;


            if (precept.def == InternalDefOf.GR_ExtractorFuneral || precept.def == PreceptDefOf.Funeral)
            {
                if (__instance.HasMeme(InternalDefOf.GR_MadScientists))
                {
                    if (__instance.GetPrecept(InternalDefOf.GR_ExtractorFuneral) != null && __instance.GetPrecept(PreceptDefOf.Funeral) != null)
                    {
                        //Have 2 funerals gotta remove one, removing vanilla
                        __instance.RemovePrecept(__instance.GetPrecept(PreceptDefOf.Funeral));
                        return;
                    }

                }
            }
            return;

        }

    }

}
