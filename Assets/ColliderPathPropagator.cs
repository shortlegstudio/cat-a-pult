using UnityEngine;

public class ColliderPathPropagator : MonoBehaviour
{
    public PolygonCollider2D SrcCollider;

    private void OnEnable()
    {
        var srcCollider = SrcCollider.GetComponent<PolygonCollider2D>();
        var dstCollider = transform.parent.GetComponent<PolygonCollider2D>();

        dstCollider.points = srcCollider.points;        
    }
}
