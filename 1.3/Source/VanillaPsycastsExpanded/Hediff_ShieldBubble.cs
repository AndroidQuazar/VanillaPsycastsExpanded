namespace VanillaPsycastsExpanded
{
    using UnityEngine;
    using Verse;
    using VFECore.Abilities;

    [StaticConstructorOnStartup]
    public abstract class Hediff_ShieldBubble : Hediff_Ability
    {
        public static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

        private Material material;
        public Material ShieldMat
        {
            get
            {
                if (material == null)
                {
                    material = MaterialPool.MatFrom(ShieldPath, ShaderDatabase.MoteGlow);
                }
                return material;
            }
        }

        public virtual float ShieldSize => 1f;
        public virtual Color ShieldColor => Color.yellow;
        public virtual string ShieldPath { get; }
        public virtual void Draw()
        {

        }
    }
}