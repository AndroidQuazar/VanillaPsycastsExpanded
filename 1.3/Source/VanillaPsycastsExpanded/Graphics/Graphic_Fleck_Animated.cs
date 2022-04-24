namespace VanillaPsycastsExpanded.Graphics
{
    using UnityEngine;
    using Verse;

    public class Graphic_Fleck_Animated : Graphic_FleckCollection
    {
        private float? startTick;

        public override void DrawFleck(FleckDrawData drawData, DrawBatch batch)
        {
            this.startTick ??= Current.Game?.tickManager?.TicksGame ?? 0f;
            this.subGraphics?[
                Mathf.FloorToInt(((Current.Game?.tickManager?.TicksGame ?? 0f) - this.startTick.Value) / ((GraphicData_Animated) this.data).ticksPerFrame) %
                this.subGraphics.Length].DrawFleck(drawData, batch);
        }
    }
}