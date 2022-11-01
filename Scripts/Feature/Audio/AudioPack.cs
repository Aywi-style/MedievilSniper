using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Client
{
    [Serializable]
    public class AudioPack
    {
        public AudioMixerGroup AudioMixer;
        [Space]
        public AudioStorage AudioStorage;
        [Space]
        public Snapshots AudioSnapshots;

        [Serializable]
        public class Snapshots 
        {
            public AudioMixerSnapshot Normal;
            public AudioMixerSnapshot TutorialPause;
            public AudioMixerSnapshot BulletTime;
            public AudioMixerSnapshot LevelEnd;
        }
    }
}
