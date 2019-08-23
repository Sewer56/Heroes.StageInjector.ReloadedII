using System;
using System.Collections.Generic;
using System.Text;
using Reloaded.Universal.Redirector.Interfaces;
using SonicHeroes.Utils.StageInjector.Common;
using SonicHeroes.Utils.StageInjector.Common.Shared;
using SonicHeroes.Utils.StageInjector.Common.Shared.Splines;
using SonicHeroes.Utils.StageInjector.Common.Structs.Positions.Substructures;
using SonicHeroes.Utils.StageInjector.Common.Utilities;

namespace SonicHeroes.Utils.StageInjector
{
    public unsafe class CustomStage : Stage, IDisposable
    {
        private string ConfigPath => $"{_stageFolder}\\Stage.json";
        private string SplineFilePath => $"{_stageFolder}\\Splines.json";
        private string RedirectionFolder => $"{_stageFolder}\\Files";

        private PinnedConfig _config;                                   // This keeps the spawn pointers above pinned, do not remove!
        private PinnedDisposableNativeStruct<Spline>[] _splines;        // This keeps the splines pinned for unmanaged memory, do not remove.
        private PinnedManagedObject<Ptr<Spline>[]>     _managedSplines; // This keeps the pointer to array of splines pinned for unmanaged memory, do not remove.

        private string _stageFolder;
        private WeakReference<IRedirectorController> _redirectorController;

        public CustomStage(string directory, WeakReference<IRedirectorController> redirectorController)
        {
            // Setup file redirection.
            _redirectorController = redirectorController;
            _stageFolder = directory;
            if (_redirectorController.TryGetTarget(out var controller))
                controller.AddRedirectFolder(RedirectionFolder);

            // Setup Config
            ReadConfig();

            // Setup Splines.
            SetupSplines();

            // TODO: Bobsled Splines
        }

        ~CustomStage()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_redirectorController.TryGetTarget(out var controller))
                controller.RemoveRedirectFolder(RedirectionFolder);

            foreach (var spline in _splines)
                spline.Dispose();

            _config.Dispose();
            GC.SuppressFinalize(this);
        }

        private void SetupSplines()
        {
            var splineFile = SplineFile.FromPath(SplineFilePath);
            _splines = new PinnedDisposableNativeStruct<Spline>[splineFile.Splines.Length];

            for (int x = 0; x < _splines.Length; x++)
                _splines[x] = new PinnedDisposableNativeStruct<Spline>(new Spline(splineFile.Splines[x]));

            // Make unmanaged pointer to array of spline pointers.
            var splinePointers = new Ptr<Spline>[_splines.Length + 1];
            for (int x = 0; x < _splines.Length; x++)
                splinePointers[x] = _splines[x].StructPtr;

            splinePointers[^1] = (Spline*)0; // This list finishes with a null pointer.

            _managedSplines = new PinnedManagedObject<Ptr<Spline>[]>(splinePointers);
            Splines = (Spline**) _managedSplines.ObjectPtr;
        }

        private void ReadConfig()
        {
            _config         = new PinnedConfig(Common.Config.FromPath(ConfigPath));
            StartPositions  = _config.StartPositions.AsPointer<PositionStart>();
            EndPositions    = _config.EndPositions.AsPointer<PositionEnd>();
            BragPositions   = _config.BragPositions.AsPointer<PositionEnd>();
            StageId         = _config.StageId;
        }
    }
}
