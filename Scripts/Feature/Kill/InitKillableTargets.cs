using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.AI;

namespace Client
{
    sealed class InitKillableTargets : IEcsInitSystem
    {
        readonly EcsWorldInject _world;

        readonly EcsPoolInject<View> _viewPool = default;
        readonly EcsPoolInject<Killable> _killablePool = default;
        readonly EcsPoolInject<GivableScores> _givableScoresPool = default;
        readonly EcsPoolInject<ScoreComponent> _scoreComponentPool = default;
        readonly EcsPoolInject<Outlinable> _outlinablePool = default;
        readonly EcsPoolInject<EnemyPointerComponent> _enemyPointerPool = default;
        readonly EcsPoolInject<Patrollable> _patrollablePool = default;
        readonly EcsPoolInject<AudioComponent> _audioPool = default;

        readonly EcsSharedInject<GameState> _state = default;

        private int _currentEntity = GameState.NULL_ENTITY;

        public void Init (IEcsSystems systems)
        {
            var killableTargetsMB = GameObject.FindObjectsOfType<KillableTargetMB>();
            GameObject canvas = GameObject.FindObjectOfType<MainCanvasMB>().gameObject;
            GameObject arrow = GameObject.FindObjectOfType<ArrowModelMB>().gameObject;

            foreach (var killableTargetMB in killableTargetsMB)
            {
                if (killableTargetMB.ObjectType == Scores.Objects.Type.Sniper)
                {
                    continue;
                }

                _currentEntity = _world.Value.NewEntity();

                ref var viewComponent = ref _viewPool.Value.Add(_currentEntity);
                viewComponent.GameObject = killableTargetMB.gameObject;
                viewComponent.Transform = killableTargetMB.transform;
                viewComponent.Animator = killableTargetMB.GetComponent<Animator>();
                viewComponent.NavMeshAgent = killableTargetMB.GetComponent<NavMeshAgent>();
                viewComponent.EcsInfoMB = killableTargetMB.GetComponent<EcsInfoMB>();
                viewComponent.EcsInfoMB.Init(_world, _currentEntity);

                ref var enemyPointerComponent = ref _enemyPointerPool.Value.Add(_currentEntity);
                enemyPointerComponent.Arrow = arrow;
                enemyPointerComponent.TransformThisEnemy = killableTargetMB.transform;
                enemyPointerComponent.TransformPrefabIndicator = GameObject.Instantiate(_state.Value.InterfaceConfig.prefabIndicator, canvas.transform);
                enemyPointerComponent.TransformPrefabIndicator.SetActive(false);
                enemyPointerComponent.openPointer = false;
                enemyPointerComponent.isKillable = true;

                ref var killableComponent = ref _killablePool.Value.Add(_currentEntity);
                killableComponent.MainBody = killableTargetMB.gameObject;
                killableComponent.RigidbodysBones = killableTargetMB.RigidbodysBones;
                killableComponent.Type = killableTargetMB.ObjectType;

                ref var givableScoresComponent = ref _givableScoresPool.Value.Add(_currentEntity);
                givableScoresComponent.Value = Scores.Objects.GetValueFromObjectType(killableTargetMB.ObjectType);

                ref var scoreComponentPool = ref _scoreComponentPool.Value.Add(_currentEntity);
                scoreComponentPool.Score = givableScoresComponent.Value;

                ref var outlinableComponent = ref _outlinablePool.Value.Add(_currentEntity);
                outlinableComponent.Outline = killableTargetMB.GetComponent<Outline>();

                ref var audioComponent = ref _audioPool.Value.Add(_currentEntity);
                audioComponent.AudioSource = killableTargetMB.GetComponent<AudioSource>();

                switch (killableComponent.Type)
                {
                    case Scores.Objects.Type.MainTarget:
                        outlinableComponent.Outline.OutlineWidth = 10;
                        outlinableComponent.Outline.OutlineColor = Color.red;
                        outlinableComponent.Outline.OutlineMode = Outline.Mode.OutlineAll;
                        break;
                    case Scores.Objects.Type.SecondTarget:
                        outlinableComponent.Outline.OutlineWidth = 10;
                        outlinableComponent.Outline.OutlineColor = Color.red;
                        outlinableComponent.Outline.OutlineMode = Outline.Mode.OutlineAll;
                        break;
                    default:
                        outlinableComponent.Outline.enabled = false;
                        break;
                }

                if (killableTargetMB.PatrolPoints.Length > 1)
                {
                    ref var patrollableComponent = ref _patrollablePool.Value.Add(_currentEntity);
                    patrollableComponent.CurrentPointNumber = 0;
                    patrollableComponent.Points = killableTargetMB.PatrolPoints;
                }
            }
        }
    }
}