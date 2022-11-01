using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class InitDestructibleObjects : IEcsInitSystem
    {
        readonly EcsWorldInject _world;

        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<Destructible> _destructiblePool = default;
        readonly EcsPoolInject<GivableScores> _givableScores = default;
        readonly EcsPoolInject<Outlinable> _outlinablePool = default;

        private int _currentEntity = GameState.NULL_ENTITY;

        public void Init (IEcsSystems systems)
        {
            var destructibleObjectsMB = GameObject.FindObjectsOfType<DestructibleObjectMB>();

            foreach (var destructibleObjectMB in destructibleObjectsMB)
            {
                _currentEntity = _world.Value.NewEntity();

                ref var viewComponent = ref _viewPool.Value.Add(_currentEntity);
                viewComponent.GameObject = destructibleObjectMB.gameObject;
                viewComponent.Transform = destructibleObjectMB.transform;
                viewComponent.EcsInfoMB = destructibleObjectMB.GetComponent<EcsInfoMB>();
                viewComponent.EcsInfoMB.Init(_world, _currentEntity);

                ref var destructibleComponent = ref _destructiblePool.Value.Add(_currentEntity);
                destructibleComponent.Type = destructibleObjectMB.ObjectType;
                destructibleComponent.ObjectMaterials = destructibleObjectMB.ObjectMaterials;
                destructibleComponent.RigidbodyParts = destructibleObjectMB.RigidbodyParts;

                ref var givableScoresComponent = ref _givableScores.Value.Add(_currentEntity);
                givableScoresComponent.Value = Scores.Objects.GetValueFromObjectType(destructibleObjectMB.ObjectType);

                ref var outlinableComponent = ref _outlinablePool.Value.Add(_currentEntity);
                outlinableComponent.Outline = destructibleObjectMB.GetComponent<Outline>();

                switch (destructibleComponent.Type)
                {
                    case Scores.Objects.Type.Small:
                        outlinableComponent.Outline.OutlineWidth = 10;
                        outlinableComponent.Outline.OutlineColor = Color.yellow;
                        break;
                    case Scores.Objects.Type.Medium:
                        outlinableComponent.Outline.OutlineWidth = 10;
                        outlinableComponent.Outline.OutlineColor = Color.yellow;
                        break;
                    case Scores.Objects.Type.Large:
                        outlinableComponent.Outline.OutlineWidth = 10;
                        outlinableComponent.Outline.OutlineColor = Color.yellow;
                        break;
                    default:
                        outlinableComponent.Outline.enabled = false;
                        break;
                }
            }
        }
    }
}