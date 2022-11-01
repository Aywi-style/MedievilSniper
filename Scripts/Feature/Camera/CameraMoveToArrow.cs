using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class CameraMoveToArrow : IEcsRunSystem
    {
        readonly EcsSharedInject<GameState> _gameState;

        readonly EcsFilterInject<Inc<CameraComponent>> _cameraFilter = default;

        readonly EcsPoolInject<CameraComponent> _cameraPool = default;
        readonly EcsPoolInject<Arrow> _arrowPool = default;

        private Vector3 _startPosition = new Vector3(2.3f, 1.6f, 0f);
        private Quaternion _startRotation = Quaternion.Euler(40, 270, 270);
        private float _startFieldOfView = 90;

        private float _endFieldOfView = 60;

        private float _currentTime = 0;
        private float _timeToMove = 0;
        private float _totalTime = 0;

        private int _cameraEntity;

        private bool _cameraIsOnArrow = false;

        public void Run (IEcsSystems systems)
        {
            if (_cameraIsOnArrow)
            {
                return;
            }

            foreach (var cameraEntity in _cameraFilter.Value)
            {
                _cameraEntity = cameraEntity;

                ref var cameraComponent = ref _cameraPool.Value.Get(_cameraEntity);

                if (_timeToMove == 0)
                {
                    _totalTime = cameraComponent.MoveToArrowCurve.keys[cameraComponent.MoveToArrowCurve.keys.Length - 1].time;
                }

                _currentTime = cameraComponent.MoveToArrowCurve.Evaluate(_timeToMove);

                ref var arrowComponent = ref _arrowPool.Value.Get(_gameState.Value.ArrowEntity);

                var endPosition = arrowComponent.Tip.transform.position;
                //var endRotation = arrowComponent.Tip.transform.rotation;

                cameraComponent.CameraTransform.localPosition = Vector3.Lerp(_startPosition, endPosition, _currentTime);
                //cameraComponent.CameraTransform.localRotation = Quaternion.Slerp(_startRotation, endRotation, _currentTime);
                cameraComponent.Camera.fieldOfView = Mathf.Lerp(_startFieldOfView, _endFieldOfView, _currentTime);

                _timeToMove += Time.deltaTime;

                /*if (_timeToMove >= _totalTime)
                {
                    _cameraIsOnArrow = true;
                }*/
            }
        }
    }
}