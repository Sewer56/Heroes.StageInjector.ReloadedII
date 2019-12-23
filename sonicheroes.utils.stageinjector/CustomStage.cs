using System;
using System.IO;
using Heroes.SDK.Definitions.Structures.Stage.Splines;
using Reloaded.Memory.Interop;
using Reloaded.Memory.Pointers;
using Reloaded.Messaging.Interfaces;
using Reloaded.Universal.Redirector.Interfaces;
using SonicHeroes.Utils.StageInjector.Common;
using SonicHeroes.Utils.StageInjector.Heroes;

namespace SonicHeroes.Utils.StageInjector
{
    public unsafe class CustomStage : StageBase, IDisposable
    {
        private string ConfigPath => $"{_stageFolder}\\Stage.json";
        private string SplineFilePath => $"{_stageFolder}\\Splines.json";
        private string RedirectionFolder => $"{_stageFolder}\\Files";

        private PinnedStageConfig _config;                           // This keeps the spawn pointers above pinned, do not remove!
        private PinnableDisposable<Spline>[] _splines;               // This keeps the splines pinned for unmanaged memory, do not remove.
        private Pinnable<BlittablePointer<Spline>> _managedSplines;  // This keeps the pointer to array of splines pinned for unmanaged memory, do not remove.

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
            _splines = new PinnableDisposable<Spline>[splineFile.Splines.Length];

            for (int x = 0; x < _splines.Length; x++)
                _splines[x] = new PinnableDisposable<Spline>(new Spline(splineFile.Splines[x]));

            // Make unmanaged pointer to array of spline pointers.
            var splinePointers = new BlittablePointer<Spline>[_splines.Length + 1];
            for (int x = 0; x < _splines.Length; x++)
                splinePointers[x] = _splines[x].Pointer;

            splinePointers[^1] = (Spline*)0; // This list finishes with a null pointer.

            _managedSplines = new Pinnable<BlittablePointer<Spline>>(splinePointers);
            Splines = (Spline**) _managedSplines.Pointer;
        }

        private void ReadConfig()
        {
            var stageConfig = StageConfig.FromPath(ConfigPath);
            _config         = new PinnedStageConfig(stageConfig);
            StartPositions  = _config.StartPositions.Pointer;
            EndPositions    = _config.EndPositions.Pointer;
            BragPositions   = _config.BragPositions.Pointer;
            StageId         = _config.StageId;
        }
    }
}
