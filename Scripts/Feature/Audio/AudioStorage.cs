using UnityEngine;
using System;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioStorage", menuName = "Data/AudioStorage")]
public class AudioStorage : ScriptableObject
{
    public EnemysHittings EnemysHittings;
    [Space]
    public ObjectsHittings ObjectsHitiings;
    [Space]
    public SniperAudio Sniper;
    [Space]
    public Ambients Ambients;
    [Space]
    public LevelEndAudio LevelEnd;
    [Space]
    public UIAudio UIAudio;
}

[Serializable]
public class EnemysHittings
{
    public AudioClip HeadHitting;
    [Space]
    public AudioClip[] BodyHittings;
    public AudioClip[] BodyScreams;
}

[Serializable]
public class ObjectsHittings
{
    public AudioClip[] WoodsHittings;
    public AudioClip[] MetallHittings;
    public AudioClip[] RockHittings;
    public AudioClip[] GlassHittings;
}

[Serializable]
public class Ambients
{
    public AudioClip Greece;
    public AudioClip Rome;
    public AudioClip Medieval;
}

[Serializable]
public class SniperAudio
{
    public AudioClip ArrowIsFailed;
    public AudioClip[] ArrowLaunching;
    public AudioClip PullingBowstringUp;
    public AudioClip PullingBowstringDown;
}

[Serializable]
public class LevelEndAudio
{
    public AudioClip[] Win;
    public AudioClip[] Lose;
    public AudioClip GoldReward;
}

[Serializable]
public class UIAudio
{
    public AudioClip StoreBuying;
    public AudioClip StoreEquiping;
}