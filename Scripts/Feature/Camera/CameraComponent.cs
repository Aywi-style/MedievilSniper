using UnityEngine;
using Cinemachine;

namespace Client
{
    struct CameraComponent
    {
        public Camera Camera;
        public GameObject CameraObject;
        public Transform CameraTransform;
        public AnimationCurve MoveToArrowCurve;

        public GameObject HolderObject;
        public Transform HolderTransform;

        public CinemachineVirtualCamera SniperVirtualCamera;
        public CinemachineVirtualCamera ArrowVirtualCamera;
        public CinemachineVirtualCamera LoseVirtualCamera;
        public CinemachineVirtualCamera WinVirtualCamera;

        public CinemachineVirtualCamera FlyingVirtualCamera;
        public CinemachineDollyCart FlyingCameraCart;
    }
}