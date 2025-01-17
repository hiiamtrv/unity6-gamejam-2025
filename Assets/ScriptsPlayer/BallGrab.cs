using UnityEngine;

public class BallGrab : MonoBehaviour
{
    private bool isGrabbingBall;
    private Rigidbody2D rb;
    [SerializeField] private float ballDetectRadius;
    [SerializeField] private Vector2 ballDetectOffset;
    [SerializeField] private LayerMask ballLayer;
    [SerializeField] private float forceOnBallRelease;
    private Collider2D detectedBallCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isGrabbingBall)
        {
            detectedBallCollider = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(ballDetectOffset.x * transform.localScale.x, ballDetectOffset.y), ballDetectRadius, ballLayer);
            if (detectedBallCollider && Input.GetKeyDown(KeyCode.C))
            {
                isGrabbingBall = true;
                Debug.Log(detectedBallCollider);
            }
            Debug.Log("Normal Mode");
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.C))
            {
                isGrabbingBall = false;
                detectedBallCollider.attachedRigidbody.AddForce(rb.linearVelocity.normalized * forceOnBallRelease, ForceMode2D.Impulse);
                //if (detectedBallCollider.TryGetComponent<Rigidbody2D>(out Rigidbody2D ballRigidBody))
                //{
                //    ballRigidBody.AddForce(Vector2.right * transform.localScale.x * forceOnBallRelease, ForceMode2D.Impulse);
                //}
                detectedBallCollider = null;
                return;
            }
            detectedBallCollider.transform.position = (Vector2)transform.position + new Vector2(ballDetectOffset.x * transform.localScale.x, ballDetectOffset.y);
            Debug.Log("Grab Mode");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(ballDetectOffset.x * transform.localScale.x, ballDetectOffset.y), ballDetectRadius);
    }
}
