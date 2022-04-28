namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;
	[StaticConstructorOnStartup]
    public class Hediff_Overshield : HediffWithComps
    {
		const float ShieldSize = 2.5f;
		public static Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent, Color.yellow);
		public override void Tick()
        {
            base.Tick();
			foreach (var thing in GenRadial.RadialDistinctThingsAround(pawn.Position, pawn.Map, ShieldSize + 1, true))
            {
				if (thing is Projectile projectile && projectile.Launcher != pawn && Vector3.Distance(projectile.ExactPosition, pawn.DrawPos) <= (ShieldSize * 0.7f))
                {
					projectile.Position = pawn.Map.AllCells.RandomElement();
					var anotherCell = pawn.Map.AllCells.RandomElement();
					projectile.Launch(projectile.Launcher, anotherCell, anotherCell, projectile.HitFlags, false, ThingMaker.MakeThing(projectile.EquipmentDef));
				}
            }
        }
        public void Draw()
        {
			float num = ShieldSize;
			Vector3 drawPos = pawn.Drawer.DrawPos;
			drawPos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
			float angle = Rand.Range(0, 360);
			Vector3 s = new Vector3(num, 1f, num);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawPos, Quaternion.AngleAxis(angle, Vector3.up), s);
			UnityEngine.Graphics.DrawMesh(MeshPool.plane10, matrix, BubbleMat, 0);
		}
    }
}