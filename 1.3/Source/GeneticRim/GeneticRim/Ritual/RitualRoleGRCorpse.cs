using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace GeneticRim
{
    public class RitualRoleGRCorpse : RitualRole
    {
		public override bool AppliesToPawn(Pawn p, out string reason, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			reason = null;			
			if (!p.Faction.IsPlayerSafe())
			{
				if (!skipReason)
				{
					
				}
				return false;
			}	
			if (!p.CanBeBuried() || !p.Dead)
			{
				if (!skipReason)
				{
					reason = "GR_RoleNotDeadExtractGenome".Translate(base.LabelCap);
				}
				return false;
			}	

			return true;
		}

		
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}
		public bool addTolord = false;
		
	}
}

