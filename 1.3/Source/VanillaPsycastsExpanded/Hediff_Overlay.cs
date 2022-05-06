namespace VanillaPsycastsExpanded
{
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;

    [StaticConstructorOnStartup]
    public abstract class Hediff_Overlay : Hediff_Ability
    {
        public static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

        private Material material;
        public Material ShieldMat
        {
            get
            {
                if (material == null)
                {
                    material = MaterialPool.MatFrom(OverlayPath, ShaderDatabase.MoteGlow);
                }
                return material;
            }
        }
        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            base.pawn.Map.GetComponent<MapComponent_PsycastsManager>().hediffsToDraw.Add(this);
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            base.pawn.Map.GetComponent<MapComponent_PsycastsManager>().hediffsToDraw.Remove(this);
        }


        public virtual float OverlaySize => 1f;
        public virtual Color OverlayColor => Color.yellow;
        public virtual string OverlayPath { get; }
        public virtual void Draw()
        {

        }
    }
}