using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class LoseSystem : IEcsRunSystem
    {
        readonly EcsSharedInject<GameState> _gameState = default;

        readonly EcsFilterInject<Inc<LoseEvent>> _loseFilter = default;

        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;
        readonly EcsPoolInject<CameraComponent> _cameraPool = default;
        readonly EcsPoolInject<SoundSystemComponent> _soundSystemPool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var entity in _loseFilter.Value)
            {
                if (Time.timeScale != 1) Time.timeScale = 1;

                if (Tutorial.CurrentStage == Tutorial.Stage.ArrowControle)
                {
                    Tutorial.ArrowControle.SetIsPrematureEnd();
                }

                _gameState.Value.AfterLaunchingSystems = false;

                ref var interfaceComponent = ref _interfacePool.Value.Get(_gameState.Value.InterfaceEntity);
                interfaceComponent.LosePanelMB.StartLoseEvent(_gameState.Value.CurrentPlayerScore);
                interfaceComponent.SlowMotionPanelMB.GetPaticles().SetActive(false);
                interfaceComponent.ComboSystemMB.HolderCombo.gameObject.SetActive(false); 
                interfaceComponent.StrengthBarMB.HidePanel();
                ref var cameraComponent = ref _cameraPool.Value.Get(_gameState.Value.CameraEntity);
                cameraComponent.LoseVirtualCamera.Follow = null;
                cameraComponent.LoseVirtualCamera.Priority = 100;

                PlayLoseMusic();

                _loseFilter.Pools.Inc1.Del(entity);
            }
        }

        private void PlayLoseMusic()
        {
            ref var soundSystemComponent = ref _soundSystemPool.Value.Get(_gameState.Value.SoundSystemEntity);
            _gameState.Value.AudioPack.AudioSnapshots.LevelEnd.TransitionTo(0.1f);
            soundSystemComponent.LevelEndSource.PlayOneShot(_gameState.Value.AudioPack.AudioStorage.LevelEnd.Lose[0]);
        }
    }
}