namespace VanillaPsycastsExpanded.Graphics
{
    using UnityEngine;
    using Verse;

    public class DistortMaterialsPool
    {
        public static Material DistortedMaterial(string matPath, string texPath, float intesity, float brightness)
        {
            Material mat = MaterialPool.MatFrom(matPath, ShaderDatabase.MoteGlowDistortBG);
            mat.SetTexture("_DistortionTex", ContentFinder<Texture2D>.Get(texPath));
            mat.SetFloat("_distortionIntensity",  intesity);
            mat.SetFloat("_brightnessMultiplier", brightness);
            return mat;
        }
    }
}