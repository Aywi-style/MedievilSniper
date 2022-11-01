using UnityEngine;

namespace Client
{
    struct Killable
    {
        public GameObject MainBody;
        public Rigidbody[] RigidbodysBones;
        public Scores.Objects.Type Type;
    }
}