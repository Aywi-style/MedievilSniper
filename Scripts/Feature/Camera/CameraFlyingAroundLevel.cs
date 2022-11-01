using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Cinemachine;

namespace Client
{
    sealed class CameraFlyingAroundLevel : IEcsRunSystem
    {
        readonly EcsSharedInject<GameState> _gameState;

        readonly EcsPoolInject<CameraComponent> _cameraPool = default;
        readonly EcsPoolInject<InterfaceComponent> _intefacePool = default;

        private const float END_POSITION = 1;

        private bool _firstWork = true;

        private bool _isEndCameraFly = false; //need if in game state "isEndCameraFly" will be forced changed

        public void Run (IEcsSystems systems)
        {
            if (_isEndCameraFly)
            {
                return;
            }

            ref var cameraComponent = ref _cameraPool.Value.Get(_gameState.Value.CameraEntity);

            if (Input.GetMouseButtonUp(0))
            {
                _isEndCameraFly = true;
            }

            if (_gameState.Value.IsEndCameraFly)
            {
                _gameState.Value.LaunchingSystems = true;
                cameraComponent.FlyingVirtualCamera.Priority = 0;
                _isEndCameraFly = true;
            }

            if (_firstWork)
            {
                cameraComponent.FlyingVirtualCamera.Priority = 10;

                _firstWork = false;
            }

            if (cameraComponent.FlyingCameraCart.m_Position == END_POSITION)
            {
                _isEndCameraFly = true;
            }

            if (_isEndCameraFly)
            {
                _gameState.Value.IsEndCameraFly = true;
                cameraComponent.FlyingVirtualCamera.Priority = 0;
                _gameState.Value.LaunchingSystems = true;
                if (cameraComponent.SniperVirtualCamera.isActiveAndEnabled)
                    _intefacePool.Value.Get(_gameState.Value.InterfaceEntity).PlayPanel.gameObject.SetActive(true);
            }
        }
    }
}