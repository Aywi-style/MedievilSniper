using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    sealed class StartComboEventSystem : IEcsRunSystem
    {
        readonly EcsWorldInject _world;
        readonly EcsSharedInject<GameState> _gameState = default;

        readonly EcsFilterInject<Inc<StartComboEvent>> _startComboEventFilter = default;
        readonly EcsFilterInject<Inc<ComboComponent>> _comboComponentFilter = default;

        readonly EcsPoolInject<StartComboEvent> _startComboEventPool = default;

        readonly EcsPoolInject<ComboComponent> _comboPool = default;
        readonly EcsPoolInject<Arrow> _arrowPool = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;

        private const float _timerMaxValue = 5f;
        private const float _bulletTimePersentBonus = 0.1f;

        private bool _comboIsExist = false;

        private int _eventEntity = GameState.NULL_ENTITY;

        public void Run (IEcsSystems systems)
        {
            foreach (var startComboEventEntity in _startComboEventFilter.Value)
            {
                _eventEntity = startComboEventEntity;

                foreach (var comboComponentEntity in _comboComponentFilter.Value)
                {
                    ref var existComboComponent = ref _comboPool.Value.Get(comboComponentEntity);
                    existComboComponent.TimerCurrentValue = existComboComponent.TimerMaxValue;
                    existComboComponent.Count++;

                    ref var interfaceComp = ref _interfacePool.Value.Get(_gameState.Value.InterfaceEntity);
                    CalculateRatingInPlay(ref interfaceComp, ref existComboComponent);

                    _comboIsExist = true;

                    AddBulletTimeBonus();

                    DeleteEvent();
                }

                if (_comboIsExist) // this for multi collision moment
                {
                    _comboIsExist = false;

                    continue;
                }

                ref var comboComponent = ref _comboPool.Value.Add(_world.Value.NewEntity());
                comboComponent.TimerMaxValue = _timerMaxValue;
                comboComponent.TimerCurrentValue = comboComponent.TimerMaxValue;
                comboComponent.Count = 1;

                AddBulletTimeBonus();

                DeleteEvent();
            }
        }
        private void CalculateRatingInPlay(ref InterfaceComponent interfaceComponent, ref ComboComponent existComboComponent)
        {
            var result = (int)Scores.Combo.GetComboLevelFromComboCount(existComboComponent.Count);
            switch (result)
            {
                case 0:
                    interfaceComponent.ComboSystemMB.UpdateRating(LevelStage.zeroLevel);
                    break;
                case 1:
                    interfaceComponent.ComboSystemMB.UpdateRating(LevelStage.firstLevel);
                    break;
                case 2:
                    interfaceComponent.ComboSystemMB.UpdateRating(LevelStage.secondLevel);
                    break;
                case 3:
                    interfaceComponent.ComboSystemMB.UpdateRating(LevelStage.thirdLevel);
                    break;
                case 4:
                    interfaceComponent.ComboSystemMB.UpdateRating(LevelStage.fourthLevel);
                    break;
                case 5:
                    interfaceComponent.ComboSystemMB.UpdateRating(LevelStage.fifthLevel);
                    break;
                case 6:
                    interfaceComponent.ComboSystemMB.UpdateRating(LevelStage.sixthLevel);
                    break;
                default:
                    break;
            }
        }

        private void AddBulletTimeBonus()
        {
            ref var arrowComponent = ref _arrowPool.Value.Get(_gameState.Value.ArrowEntity);
            ref var interfaceComponent = ref _interfacePool.Value.Get(_gameState.Value.InterfaceEntity);

            arrowComponent.BulletTimeChargeCurrent += (arrowComponent.BulletTimeChargeMax - arrowComponent.BulletTimeChargeCurrent) * _bulletTimePersentBonus;

            interfaceComponent.StrengthBarMB.UpdateSlider(arrowComponent.BulletTimeChargeCurrent / arrowComponent.BulletTimeChargeMax);
        }

        private void DeleteEvent()
        {
            _startComboEventPool.Value.Del(_eventEntity);

            _eventEntity = GameState.NULL_ENTITY;
        }
    }
}