using UnityEngine;

namespace Client
{
    struct ChangeAmbientEvent
    {
        public AudioClip ActualAmbientClip;

        public void Invoke(AudioClip actualAmbient)
        {
            ActualAmbientClip = actualAmbient;
        }
    }
}