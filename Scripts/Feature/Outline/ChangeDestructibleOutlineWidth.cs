using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class ChangeDestructibleOutlineWidth : IEcsRunSystem
    {
        readonly EcsSharedInject<GameState> _gameState;

        readonly EcsFilterInject<Inc<Outlinable, Destructible>, Exc<Destroyed>> _outlinableFilter = default;

        readonly EcsPoolInject<Outlinable> _outlinablePool = default;
        readonly EcsPoolInject<View> _viewPool = default;

        private float _timeToChangeMaxValue = 0.25f;
        private float _timeToChangeCurrentValue = 0;

        private float _maxDistanceForOutline = 20f;

        private float _maxWidthForOutline = 10f;

        private int _arrowEntity = GameState.NULL_ENTITY;

        private bool _firstWork = true;

        public void Run (IEcsSystems systems)
        {
            if (_firstWork)
            {
                _arrowEntity = _gameState.Value.ArrowEntity;
            }

            if (_timeToChangeCurrentValue < _timeToChangeMaxValue)
            {
                _timeToChangeCurrentValue += Time.unscaledDeltaTime;
                return;
            }

            ref var arrowViewCompoennt = ref _viewPool.Value.Get(_arrowEntity);

            foreach (var outlineEntity in _outlinableFilter.Value)
            {
                ref var viewComponent = ref _viewPool.Value.Get(outlineEntity);

                float distanceToObject = Vector3.Distance(viewComponent.Transform.position, arrowViewCompoennt.Model.transform.position);

                ref var outlinableComponent = ref _outlinablePool.Value.Get(outlineEntity);

                if (distanceToObject > _maxDistanceForOutline)
                {
                    outlinableComponent.Outline.OutlineWidth = 0;
                }
                else
                {
                    outlinableComponent.Outline.OutlineWidth = _maxWidthForOutline * (1 - (distanceToObject / _maxDistanceForOutline));
                }
            }

            _timeToChangeCurrentValue = 0;
        }
    }
}