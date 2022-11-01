using UnityEngine;

namespace Client
{
    struct Destructible
    {
        public Scores.Objects.Type Type;
        public ObjectMaterial[] ObjectMaterials;
        public Rigidbody[] RigidbodyParts;
    }
}