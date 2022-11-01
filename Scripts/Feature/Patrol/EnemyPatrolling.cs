using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class EnemyPatrolling : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<Patrollable>, Exc<Killed>> _alivePatrollableFilter = default;

        readonly EcsPoolInject<Patrollable> _patrollablePool = default;
        readonly EcsPoolInject<View> _viewPool = default;

        public void Run (IEcsSystems systems)
        {
            foreach (var alivePatrollEntity in _alivePatrollableFilter.Value)
            {
                ref var viewComponent = ref _viewPool.Value.Get(alivePatrollEntity);
                ref var patrollableComponent = ref _patrollablePool.Value.Get(alivePatrollEntity);

                if (viewComponent.NavMeshAgent.hasPath)
                {
                    continue;
                }

                patrollableComponent.CurrentPointNumber++;

                if (patrollableComponent.CurrentPointNumber >= patrollableComponent.Points.Length)
                {
                    patrollableComponent.CurrentPointNumber = 0;
                }

                viewComponent.Animator.SetBool("isWalk", true);
                viewComponent.NavMeshAgent.SetDestination(patrollableComponent.Points[patrollableComponent.CurrentPointNumber].position);
            }
        }
    }
}