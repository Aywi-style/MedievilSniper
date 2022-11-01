using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class ChangeAmbientEventSystem : IEcsRunSystem
    {
        readonly EcsSharedInject<GameState> _gameState;

        readonly EcsFilterInject<Inc<ChangeAmbientEvent>> _changeAmbientEventFilter = default;

        readonly EcsPoolInject<ChangeAmbientEvent> _changeAmbientEventPool = default;

        readonly EcsPoolInject<SoundSystemComponent> _soundSystemPool = default;

        private float _timeToTransitionMaxValue = 5f;
        private float _timeToTransitionHalfMaxValue;
        private float _timeToTransitionCurrentValue = 0;

        private float _maxAmbientVolume;

        private int _eventEntity = GameState.NULL_ENTITY;

        private bool _firstWork = true;
        private bool _clipIsChanged = false;

        public void Run (IEcsSystems systems)
        {
            foreach (var eventEntity in _changeAmbientEventFilter.Value)
            {
                _eventEntity = eventEntity;

                ref var soundSystemComponent = ref _soundSystemPool.Value.Get(_gameState.Value.SoundSystemEntity);

                if (soundSystemComponent.AmbientSource.clip == null)
                {
                    ref var changeAmbientEvent = ref _changeAmbientEventPool.Value.Get(_eventEntity);
                    soundSystemComponent.AmbientSource.clip = changeAmbientEvent.ActualAmbientClip;
                    soundSystemComponent.AmbientSource.Play();
                    DeleteEvent();
                    continue;
                }

                if (_firstWork)
                {
                    _maxAmbientVolume = soundSystemComponent.AmbientSource.volume;
                    _timeToTransitionHalfMaxValue = _timeToTransitionMaxValue / 2;

                    _firstWork = false;
                }

                if (_timeToTransitionCurrentValue >= _timeToTransitionHalfMaxValue && !_clipIsChanged)
                {
                    ref var changeAmbientEvent = ref _changeAmbientEventPool.Value.Get(_eventEntity);

                    soundSystemComponent.AmbientSource.clip = changeAmbientEvent.ActualAmbientClip;
                    soundSystemComponent.AmbientSource.Play();

                    _timeToTransitionCurrentValue = 0;

                    _clipIsChanged = true;
                }

                if (!_clipIsChanged)
                {
                    soundSystemComponent.AmbientSource.volume = _maxAmbientVolume * (1 - (_timeToTransitionCurrentValue / _timeToTransitionHalfMaxValue));
                }
                else
                {
                    soundSystemComponent.AmbientSource.volume = _maxAmbientVolume * (_timeToTransitionCurrentValue / _timeToTransitionHalfMaxValue);
                }

                _timeToTransitionCurrentValue += Time.deltaTime;

                if (_timeToTransitionCurrentValue >= _timeToTransitionHalfMaxValue && _clipIsChanged)
                {
                    soundSystemComponent.AmbientSource.volume = _maxAmbientVolume;

                    _gameState.Value.IsNeedChangeEmbient = false;

                    DeleteEvent();
                }
            }
        }

        private void DeleteEvent()
        {
            _changeAmbientEventPool.Value.Del(_eventEntity);

            _eventEntity = GameState.NULL_ENTITY;

            _firstWork = true;
    }
    }
}