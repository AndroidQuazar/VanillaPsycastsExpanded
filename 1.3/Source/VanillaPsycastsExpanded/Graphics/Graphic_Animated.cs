namespace VanillaPsycastsExpanded.Graphics
{
    using UnityEngine;
    using Verse;

    public class Graphic_Animated : Graphic_Collection
    {
        private readonly int offset = Rand.Range(1, 1000);

        public override Material MatSingle => this.CurFrame?.MatSingle;

        private Graphic CurFrame =>
            this.subGraphics?[
                Mathf.FloorToInt(((Current.Game?.tickManager?.TicksGame ?? 0f) + this.offset) / ((GraphicData_Animated) this.data).ticksPerFrame) %
                this.subGraphics.Length];

        public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
        {
            this.CurFrame?.DrawWorker(loc, rot, thingDef, thing, extraRotation);
        }
    }

    public class GraphicData_Animated : GraphicData
    {
        public int  ticksPerFrame;
        public bool random;
    }
}