namespace VanillaPsycastsExpanded
{
    using Skipmaster;
    using Verse;

    public class PsycastsMod : Mod
    {
        public PsycastsMod(ModContentPack content) : base(content)
        {
            LongEventHandler.ExecuteWhenFinished(() => { Skipdoor.Init(content); });
        }
    }
}