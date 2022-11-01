using UnityEngine;

[RequireComponent(typeof(Outline))]
public class DestructibleObjectMB : MonoBehaviour
{
    public Scores.Objects.Type ObjectType;
    public ObjectMaterial[] ObjectMaterials;
    public Rigidbody[] RigidbodyParts;
}

public enum ObjectMaterial
{
    Wood,
    Rock,
    Metall,
    Glass,
}
