namespace VanillaPsycastsExpanded.Graphics
{
    using UnityEngine;
    using Verse;

    public class Graphic_Fleck_Animated : Graphic_FleckCollection
    {
        private float? startTick;

        public override void DrawFleck(FleckDrawData drawData, DrawBatch batch)
        {
            float curTick = Current.Game?.tickManager?.TicksGame ?? 0f;
            this.startTick ??= curTick;
            GraphicData_Animated dataAnimated = (GraphicData_Animated) this.data;
            int frame = Mathf.FloorToInt((curTick - (dataAnimated.random ? 0 : this.startTick.Value)) / dataAnimated.ticksPerFrame) % this.subGraphics.Length;
            Log.Message($"curTick: {curTick}, startTick: {this.startTick}, frame: {frame}, frames: {this.subGraphics.Length}");
            this.subGraphics?[frame].DrawFleck(drawData, batch);
        }
    }
}