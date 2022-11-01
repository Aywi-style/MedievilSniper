using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Lofelt.NiceVibrations;

namespace Client
{
    sealed class VibrationEventSystem : IEcsRunSystem
    {
        readonly EcsSharedInject<GameState> _gameState;

        readonly EcsFilterInject<Inc<VibrationEvent>> _vibrationFilter = default;

        readonly EcsPoolInject<VibrationEvent> _vibrationEventPool = default;

        private int _eventEntity = GameState.NULL_ENTITY;

        public void Run (IEcsSystems systems)
        {
            foreach (var eventEntity in _vibrationFilter.Value)
            {
                _eventEntity = eventEntity;

                if (!_gameState.Value.EnabledVibration)
                {
                    DeleteEvent();
                    continue;
                }

                ref var vibrationEvent = ref _vibrationEventPool.Value.Get(eventEntity);

                switch (vibrationEvent.Vibration)
                {
                    case VibrationEvent.VibrationType.HeavyImpact:
                        HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
                        //Debug.Log("сильна€ вибраци€");
                        break;
                    case VibrationEvent.VibrationType.LightImpack:
                        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
                        //Debug.Log("легка€ вибраци€");
                        break;
                    case VibrationEvent.VibrationType.MediumImpact:
                        HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
                        //Debug.Log("средн€€ вибраци€");
                        break;
                    case VibrationEvent.VibrationType.RigitImpact:
                        HapticPatterns.PlayPreset(HapticPatterns.PresetType.RigidImpact);
                        break;
                    case VibrationEvent.VibrationType.Selection:
                        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
                        break;
                    case VibrationEvent.VibrationType.SoftImpact:
                        HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
                        break;
                    case VibrationEvent.VibrationType.Success:
                        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
                        break;
                    case VibrationEvent.VibrationType.Warning:
                        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
                        break;
                }

                DeleteEvent();
            }
        }

        private void DeleteEvent()
        {
            _vibrationEventPool.Value.Del(_eventEntity);

            _eventEntity = GameState.NULL_ENTITY;
        }
    }
}