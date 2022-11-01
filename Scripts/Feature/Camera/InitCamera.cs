using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using Cinemachine;

namespace Client
{
    sealed class InitCamera : IEcsInitSystem
    {
        readonly EcsWorldInject _world;

        readonly EcsSharedInject<GameState> _gameState;

        readonly EcsPoolInject<CameraComponent> _cameraPool = default;

        public void Init (IEcsSystems systems)
        {
            var cameraEntity = _world.Value.NewEntity();

            _gameState.Value.CameraEntity = cameraEntity;

            var cameraGameObject = GameObject.FindObjectOfType<MainCameraMB>();

            ref var cameraComponent = ref _cameraPool.Value.Add(cameraEntity);
            cameraComponent.CameraObject = cameraGameObject.gameObject;
            cameraComponent.CameraTransform = cameraGameObject.transform;
            cameraComponent.Camera = cameraComponent.CameraObject.GetComponent<Camera>();
            cameraComponent.MoveToArrowCurve = cameraGameObject.MoveToArrowCurve;

            cameraComponent.HolderObject = cameraGameObject.transform.parent.gameObject;
            cameraComponent.HolderTransform = cameraComponent.HolderObject.transform;

            cameraComponent.SniperVirtualCamera = GameObject.FindObjectOfType<SniperVirtualCameraMB>().GetComponent<CinemachineVirtualCamera>();
            cameraComponent.ArrowVirtualCamera = GameObject.FindObjectOfType<ArrowVirtualCameraMB>().GetComponent<CinemachineVirtualCamera>();

            cameraComponent.LoseVirtualCamera = GameObject.FindObjectOfType<LoseVirtualCameraMB>().GetComponent<CinemachineVirtualCamera>();
            cameraComponent.WinVirtualCamera = GameObject.FindObjectOfType<WinVirtualCameraMB>().GetComponent<CinemachineVirtualCamera>();

            cameraComponent.WinVirtualCamera.LookAt = GameObject.FindObjectOfType<WinLookingChestMB>()?.transform;

            cameraComponent.FlyingVirtualCamera = GameObject.FindObjectOfType<FlyingVirtualCameraMB>().GetComponent<CinemachineVirtualCamera>();
            cameraComponent.FlyingCameraCart = GameObject.FindObjectOfType<CinemachineDollyCart>();
        }
    }
}