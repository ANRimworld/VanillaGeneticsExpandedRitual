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
    [HarmonyPatch(typeof(RitualPosition_OnInteractionCell))]
    [HarmonyPatch("GetCell")]
    public static class GeneticRim_RitualPosition_OnInteractionCell_GetCell
    {
        [HarmonyPrefix]
        public static void Orientation(RitualPosition_OnInteractionCell __instance, IntVec3 spot, Pawn p, LordJob_Ritual ritual)
        {
            if(ritual.Ritual.def.defName== "GR_ExtractorFuneral")
            {
                Thing thing = ritual.selectedTarget.Thing;
                Rot4 rotation = thing.Rotation;
                __instance.facing = rotation;
            }
            

        }

    }
}
