using System;
using System.Diagnostics;
using System.Linq;
using Heroes.SDK;
using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;
using IReloadedHooks = Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks;

namespace SonicHeroes.Utils.StageInjector
{
    public class Program : IMod
    {
        private const string ThisModId = "sonicheroes.utils.stageinjector";

        private IModLoader _modLoader;
        private IReloadedHooks _hooks;
        private StageCollection _collection;

        public void Start(IModLoaderV1 loader)
        {
            _modLoader = (IModLoader)loader;
            _modLoader.GetController<IReloadedHooks>().TryGetTarget(out _hooks);
            SDK.Init(_hooks);

            /* Your mod code starts here. */
            _modLoader.ModLoading   += ModLoading;
            _modLoader.ModUnloading += ModUnloading;
            _collection = new StageCollection(_modLoader);
        }

        /// <summary>
        /// Remove stage mods requiring this.
        /// </summary>
        private void ModUnloading(IModV1 mod, IModConfigV1 modConfig)
        {
            if (modConfig.ModDependencies.Contains(ThisModId))
                _collection.RemoveMod(modConfig.ModId);
        }

        /// <summary>
        /// Add stage mods requiring this.
        /// </summary>
        private void ModLoading(IModV1 mod, IModConfigV1 modConfig)
        {
            if (modConfig.ModDependencies.Contains(ThisModId))
                _collection.AddMod(modConfig.ModId);
        }

        /* Mod loader actions. */
        public void Suspend() { }
        public void Resume() { }
        public void Unload() { }

        public bool CanUnload()  => false;
        public bool CanSuspend() => false;

        /* Automatically called by the mod loader when the mod is about to be unloaded. */
        public Action Disposing { get; }
    }
}
