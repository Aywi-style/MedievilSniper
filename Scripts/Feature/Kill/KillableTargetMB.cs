using UnityEngine;

[RequireComponent(typeof(Outline))]
public class KillableTargetMB : MonoBehaviour
{
    public Scores.Objects.Type ObjectType;
    public Rigidbody[] RigidbodysBones;
    public Transform[] PatrolPoints;

    private void OnDrawGizmos()
    {
        if (PatrolPoints.Length < 2)
        {
            return;
        }

        Gizmos.color = Color.red;

        for (int i = 0; i < PatrolPoints.Length; i++)
        {
            if (i + 1 >= PatrolPoints.Length)
            {
                Gizmos.DrawSphere(PatrolPoints[i].position, 0.2f);
                Gizmos.DrawLine(PatrolPoints[i].position, PatrolPoints[0].position);
                break;
            }

            Gizmos.DrawSphere(PatrolPoints[i].position, 0.2f);
            Gizmos.DrawLine(PatrolPoints[i].position, PatrolPoints[i + 1].position);
        }
    }
}
