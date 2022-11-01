using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using System;

namespace Client
{
    sealed class ComboTimerSystem : IEcsRunSystem
    {
        readonly EcsSharedInject<GameState> _gameState;

        readonly EcsFilterInject<Inc<ComboComponent>> _comboComponentFilter = default;

        readonly EcsPoolInject<ComboComponent> _comboPool = default;
        readonly EcsPoolInject<InterfaceComboComponent> _interfaceComboPool = default;
        readonly EcsPoolInject<Arrow> _arrowPool = default;
        readonly EcsPoolInject<InterfaceComponent> _interfacePool = default;

        private float _lastTimerValue;
        private float _roundedCurrentTimerValue;

        private bool _firstWork = true;

        private bool _epicTrailIsEnable = false;

        public void Run (IEcsSystems systems)
        {
            foreach (var comboComponentEntity in _comboComponentFilter.Value)
            {
                ref var comboComponent = ref _comboPool.Value.Get(comboComponentEntity);
                ref var interfaceComponent = ref _interfacePool.Value.Get(_gameState.Value.InterfaceEntity);

                if (comboComponent.TimerCurrentValue <= 0)
                {
                    _comboPool.Value.Del(comboComponentEntity);

                    DisableComboInterface(ref interfaceComponent);

                    DisableEpicTrail();

                    _epicTrailIsEnable = false;

                    _firstWork = true;

                    continue;
                }

                ref var interfaceComboComponent = ref _interfaceComboPool.Value.Get(_gameState.Value.InterfaceEntity);

                if (_firstWork)
                {
                    interfaceComponent.ComboSystemMB.HolderCombo.gameObject.SetActive(true);
                    interfaceComboComponent.TimerText.text = comboComponent.TimerMaxValue.ToString();
                    _lastTimerValue = comboComponent.TimerMaxValue;

                    _firstWork = false;
                }
                else
                {
                    _roundedCurrentTimerValue = (float)Math.Round(comboComponent.TimerCurrentValue, 1);

                    if (_roundedCurrentTimerValue == _lastTimerValue)
                    {
                        comboComponent.TimerCurrentValue -= Time.unscaledDeltaTime;
                        continue;
                    }
                }

                _lastTimerValue = _roundedCurrentTimerValue;
                interfaceComboComponent.TimerText.text = _roundedCurrentTimerValue.ToString();

                interfaceComboComponent.TimerBarImage.color = Color.green;
                float localScaleX = 1 * (comboComponent.TimerCurrentValue / comboComponent.TimerMaxValue);
                //interfaceComboComponent.TimerBarTransform.localScale = new Vector3(localScaleX, 1f, 1f);
                interfaceComponent.ComboSystemMB.UpdateSliders(localScaleX);

                if (Scores.Combo.GetComboLevelFromComboCount(comboComponent.Count) == Scores.Combo.Level.S && !_epicTrailIsEnable)
                {
                    EnableEpicTrail();

                    _epicTrailIsEnable = true;
                }

                comboComponent.TimerCurrentValue -= Time.unscaledDeltaTime;
            }
        }

        private void DisableComboInterface(ref InterfaceComponent interfaceComponent)
        {
            ref var interfaceComboComponent = ref _interfaceComboPool.Value.Get(_gameState.Value.InterfaceEntity);

            interfaceComponent.ComboSystemMB.ResetComboSystem();
            interfaceComponent.ComboSystemMB.HolderCombo.gameObject.SetActive(false);
            //interfaceComboComponent.TimerText.gameObject.SetActive(false);
            //interfaceComboComponent.TimerBarTransform.gameObject.SetActive(false);
        }

        private void DisableEpicTrail()
        {
            ref var arrowComponent = ref _arrowPool.Value.Get(_gameState.Value.ArrowEntity);

            arrowComponent.UsualTrail.gameObject.SetActive(true);
            arrowComponent.EpicTrail.gameObject.SetActive(false);
        }

        private void EnableEpicTrail()
        {
            ref var arrowComponent = ref _arrowPool.Value.Get(_gameState.Value.ArrowEntity);

            arrowComponent.UsualTrail.gameObject.SetActive(false);
            arrowComponent.EpicTrail.gameObject.SetActive(true);
        }

        private void CheckOnActive(params GameObject[] gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                if (!gameObject.activeSelf) gameObject.SetActive(true);
            }
        }
    }
}