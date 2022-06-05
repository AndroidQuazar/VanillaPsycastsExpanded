namespace VanillaPsycastsExpanded
{
    using RimWorld;
    using UnityEngine;
    using Verse;

    public class MoteAttachedMovingAround : MoteAttached
    {
        private Vector3 curPosition;
        protected override void TimeInterval(float deltaTime)
        {
            base.TimeInterval(deltaTime);
            curPosition = GetNewMoveVector();
            exactPosition = GetRootPosition() + curPosition;
        }

        public Vector3 GetNewMoveVector()
        {
            var realPosition = new Vector2(curPosition.x, curPosition.z);
            var newPosition = realPosition.Moved(Rand.Range(0, 360), 0.001f);
            var pos = new Vector3(newPosition.x, exactPosition.y, newPosition.y);
            return pos;
        }
        public Vector3 GetRootPosition()
        {
            Vector3 vector = def.mote.attachedDrawOffset;
            if (def.mote.attachedToHead)
            {
                Pawn pawn = link1.Target.Thing as Pawn;
                if (pawn != null && pawn.story != null)
                {
                    vector = pawn.Drawer.renderer.BaseHeadOffsetAt((pawn.GetPosture() == PawnPosture.Standing) ? Rot4.North : pawn.Drawer.renderer.LayingFacing()).RotatedBy(pawn.Drawer.renderer.BodyAngle());
                }
            }
            return link1.LastDrawPos + vector;
        }
    }
}