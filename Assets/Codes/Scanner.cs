using UnityEngine;

public class Scanner : MonoBehaviour
{

    public float scanRange;

    public LayerMask targetLayer;

    public RaycastHit2D[] targets;

    public Transform nearestTarget;

    void FixedUpdate()
    {
        // CirclecastAll = 원형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearset();
    }

    Transform GetNearset()
    {
        Transform result = null;

        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;

            float curDiff = Vector3.Distance(myPos, targetPos);           // Distance : 백터 A 와 B의 거리를 계산해주는 함수

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;        
            }
        }

        return result;
     }

}
