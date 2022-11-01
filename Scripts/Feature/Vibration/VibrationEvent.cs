namespace Client
{
    struct VibrationEvent
    {
        public enum VibrationType
        {
            HeavyImpact,
            LightImpack,
            MediumImpact,
            RigitImpact,
            Selection,
            SoftImpact,
            Success,
            Warning
        }
        public VibrationType Vibration;

        public void Invoke(VibrationType vibration)
        {
            Vibration = vibration;
        }
    }
}