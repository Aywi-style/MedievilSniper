using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class InitSoundSystem : IEcsInitSystem
    {
        readonly EcsWorldInject _world;

        readonly EcsSharedInject<GameState> _gameState;

        readonly EcsPoolInject<ChangeAmbientEvent> _changeAmbientEventPool = default;

        readonly EcsPoolInject<SoundSystemComponent> _soundSystemPool = default;

        private AudioClip _actualAudioClip;

        public void Init (IEcsSystems systems)
        {
            _gameState.Value.AudioPack.AudioSnapshots.Normal.TransitionTo(0.5f);

            var soundSystems = GameObject.FindObjectsOfType<SoundSystemMB>();

            foreach (var soundSystem in soundSystems)
            {
                _gameState.Value.SoundSystemEntity = _world.Value.NewEntity();

                ref var soundSystemComponent = ref _soundSystemPool.Value.Add(_gameState.Value.SoundSystemEntity);

                if (soundSystem.GetComponentInChildren<AmbientMB>().TryGetComponent<AudioSource>(out var ambientSource))
                {
                    soundSystemComponent.AmbientSource = ambientSource;
                }

                if (soundSystem.GetComponentInChildren<LevelEndMB>().TryGetComponent<AudioSource>(out var levelEndSource))
                {
                    soundSystemComponent.LevelEndSource = levelEndSource;
                }

                if (soundSystem.GetComponentInChildren<UIAudioMB>().TryGetComponent<AudioSource>(out var uiAudioSource))
                {
                    soundSystemComponent.UIAudioSource = uiAudioSource;
                }

                CheckActualAudioClip();

                CheckNeedsChangeAmbient();

                if (_gameState.Value.IsNeedChangeEmbient)
                {
                    InvokeChangeAmbientEvent();
                }
            }
        }

        private void CheckActualAudioClip()
        {
            switch (_gameState.Value.AmbientType)
            {
                case AmbientType.Greece:
                    _actualAudioClip = _gameState.Value.AudioPack.AudioStorage.Ambients.Greece;
                    break;
                case AmbientType.Rome:
                    _actualAudioClip = _gameState.Value.AudioPack.AudioStorage.Ambients.Rome;
                    break;
                case AmbientType.Medival:
                    _actualAudioClip = _gameState.Value.AudioPack.AudioStorage.Ambients.Medieval;
                    break;
                default:
                    break;
            }
        }

        private void CheckNeedsChangeAmbient()
        {
            ref var soundSystemComponent = ref _soundSystemPool.Value.Get(_gameState.Value.SoundSystemEntity);

            _gameState.Value.IsNeedChangeEmbient = soundSystemComponent.AmbientSource.clip != _actualAudioClip;
        }

        private void InvokeChangeAmbientEvent()
        {
            _changeAmbientEventPool.Value.Add(_world.Value.NewEntity()).Invoke(_actualAudioClip);
        }
    }
}