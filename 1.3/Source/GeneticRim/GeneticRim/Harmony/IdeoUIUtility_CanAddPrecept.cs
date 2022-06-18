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

    [HarmonyPatch(typeof(IdeoUIUtility))]
    [HarmonyPatch("CanAddPrecept")]
    //Patch to prevent user from adding 2 funerals
    public static class GeneticRim_IdeoUIUtility_CanAddPrecept_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref AcceptanceReport __result, PreceptDef def, RitualPatternDef pat, Ideo ideo)
        {
            if (def.defName == "GR_ExtractorFuneral")
            {
                if (ideo.HasMeme(InternalDefOf.GR_MadScientists))
                {
                    if (ideo.HasPrecept(PreceptDefOf.Funeral))
                    {
                        __result = "GR_CantHaveVanillaFuneral".Translate(def.label.Named("EXTRACT"), ideo.GetPrecept(PreceptDefOf.Funeral).Label.Named("FUNERAL"));
                    }
                }
            }
            if (def.defName == "Funeral")
            {
                if (ideo.HasMeme(InternalDefOf.GR_MadScientists))
                {
                    if (ideo.HasPrecept(InternalDefOf.GR_ExtractorFuneral))
                    {
                        __result = "GR_CantHaveVanillaFuneral".Translate(def.label.Named("EXTRACT"), ideo.GetPrecept(InternalDefOf.GR_ExtractorFuneral).Label.Named("FUNERAL"));
                    }
                }
            }



        }

    }


}
