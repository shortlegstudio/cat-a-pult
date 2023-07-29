using System.Collections;
using System.Linq;
using UnityEngine;


public class PathFollower : MonoBehaviour
{
    public Transform[] Waypoints;
    public Vector3[] WpWaypoints;
    public bool FollowBackToStart = true;
    public float MaxNumberOfCycles = 0;
    private int CyclesCompleted = 0;
    public bool Paused = false;
    public bool DestroyWhenDone = false;
    public bool DebugPathVisuals = false;
    public float Speed = 50;

    private Rigidbody2D OurRB;

    IEnumerator Follower()
    {
        if (Waypoints == null || Waypoints.Length == 0)
            yield break;

        int i = 0;
        int increment = 1;

        while (MaxNumberOfCycles == 0 || CyclesCompleted < MaxNumberOfCycles)
        {
            yield return new WaitForFixedUpdate();

            var direction = WpWaypoints[i] - transform.position;
            var nextPos = transform.position + (direction.normalized * Speed * Time.deltaTime);
            transform.position = nextPos;

            var distance = Vector2.Distance(transform.position, WpWaypoints[i]);
            if (distance < 5)
            {
                i += increment;

                if (i >= WpWaypoints.Length)
                {
                    if (FollowBackToStart)
                    {
                        increment = -1;
                        i = WpWaypoints.Length - 2;
                    }
                    else
                    {
                        increment = 1;
                        i = 0;
                    }
                }
                else if ( i < 0 )
                {
                    increment = 1;
                    i = 1;
                }
            }
        }

        if (DestroyWhenDone)
            Destroy(OurRB.gameObject);

        yield break;
    }

    private void OnDrawGizmosSelected()
    {
        if (Waypoints == null)
            return;

        Vector3 op = transform.position;
        foreach (var pt in Waypoints)
        {
            Debug.DrawLine(op, pt.transform.position, Color.white);
            op = pt.transform.position;
        }
    }

    private void Start()
    {
        OurRB = GetComponent<Rigidbody2D>();

        // Clone the array of points so we have a snapshot of the global coordinates.
        // Otherwise the points will be relative to us (the parent).
        if (Waypoints != null && Waypoints.Length > 0)
        {
            WpWaypoints = Waypoints.Select(m => m.transform.position).ToArray();
            StartCoroutine(Follower());
        }
    }
}