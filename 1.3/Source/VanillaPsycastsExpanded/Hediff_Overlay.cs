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

        public virtual float OverlaySize => 1f;
        public virtual Color OverlayColor => Color.yellow;
        public virtual string OverlayPath { get; }
        public virtual void Draw()
        {

        }
    }
}