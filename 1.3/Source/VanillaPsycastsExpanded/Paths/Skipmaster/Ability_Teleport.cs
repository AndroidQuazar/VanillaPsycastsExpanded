namespace VanillaPsycastsExpanded.Skipmaster
{
    using RimWorld;
    using RimWorld.Planet;
    using UnityEngine;
    using Verse;
    using Verse.AI;
    using Verse.Sound;
    using VFECore.Abilities;
    using Ability = VFECore.Abilities.Ability;

    public class Ability_Teleport : Ability
    {
        public override void WarmupToil(Toil toil)
        {
            base.WarmupToil(toil);
            toil.AddPreTickAction(delegate
            {
                if (this.pawn.jobs.curDriver.ticksLeftThisToil != 5) return;
                for (int i = 0; i < this.Comp.currentlyCastingTargets.Length; i += 2)
                    if (this.Comp.currentlyCastingTargets[i].Thing is { } t)
                    {
                        if (t is Pawn p)
                        {
                            FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(p, FleckDefOf.PsycastSkipFlashEntry, Vector3.zero);
                            dataAttachedOverlay.link.detachAfterTicks = 5;
                            p.Map.flecks.CreateFleck(dataAttachedOverlay);
                        }
                        else
                            FleckMaker.Static(t.TrueCenter(), t.Map, FleckDefOf.PsycastSkipFlashEntry);

                        GlobalTargetInfo dest = this.Comp.currentlyCastingTargets[i + 1];
                        FleckMaker.Static(dest.Cell, dest.Map, FleckDefOf.PsycastSkipInnerExit);
                        FleckMaker.Static(dest.Cell, dest.Map, FleckDefOf.PsycastSkipOuterRingExit);
                        SoundDefOf.Psycast_Skip_Entry.PlayOneShot(t);
                        SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(dest.Cell, dest.Map));
                        this.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(t, t.Map),           t.Position, 60);
                        this.AddEffecterToMaintain(EffecterDefOf.Skip_Exit.Spawn(dest.Cell, dest.Map), dest.Cell,  60);
                    }
            });
        }

        public override void Cast(params GlobalTargetInfo[] targets)
        {
            AbilityExtension_Clamor clamor = this.def.GetModExtension<AbilityExtension_Clamor>();
            for (int i = 0; i < targets.Length; i += 2)
                if (targets[i].Thing is { } t)
                {
                    t.TryGetComp<CompCanBeDormant>()?.WakeUp();
                    GlobalTargetInfo dest = targets[i + 1];
                    if (t.Map != dest.Map)
                    {
                        if (t is not Pawn p) continue;
                        p.teleporting = true;
                        p.ExitMap(true, Rot4.Invalid);
                        p.teleporting = false;
                        GenSpawn.Spawn(p, dest.Cell, dest.Map);
                    }

                    t.Position = dest.Cell;
                    AbilityUtility.DoClamor(t.Position, clamor.clamorRadius, this.pawn, clamor.clamorType);
                    AbilityUtility.DoClamor(dest.Cell,  clamor.clamorRadius, this.pawn, clamor.clamorType);
                    (t as Pawn)?.Notify_Teleported();
                }

            base.Cast(targets);
        }
    }

    public class AbilityExtension_Stun : AbilityExtension_AbilityMod
    {
        public IntRange stunTicks;

        public override void Cast(GlobalTargetInfo[] targets, Ability ability)
        {
            base.Cast(targets, ability);
            for (int i = 0; i < targets.Length; i++)
                if (targets[i].Thing is Pawn {Spawned: true} p)
                    p.stances.stunner.StunFor(this.stunTicks.RandomInRange, ability.pawn);
        }
    }
}