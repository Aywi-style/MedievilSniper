using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class InitFirstVibrations : IEcsRunSystem
    {
        readonly EcsWorldInject _world;

        readonly EcsPoolInject<VibrationEvent> _vibroPool = default;

        private bool _workIsDone = false;

        public void Run (IEcsSystems systems)
        {
            if (_workIsDone)
            {
                return;
            }

            _vibroPool.Value.Add(_world.Value.NewEntity()).Invoke(VibrationEvent.VibrationType.LightImpack);

            _workIsDone = true;
        }
    }
}