using UnityEngine;

public class BallGrab : MonoBehaviour
{
    private bool isGrabbingBall;
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Ball Detector")]
    [SerializeField] private float ballDetectRadius;
    [SerializeField] private Vector2 ballDetectOffset;
    [SerializeField] private LayerMask bubbleLayer;
    [SerializeField] private float forceOnBallRelease;
    private Collider2D detectedBallCollider;

    [Header("Bubble Collider")]
    [SerializeField] private float bubbleColliderRadius;
    [SerializeField] private Vector2 bubbleColliderOffset;
    [SerializeField] private float bubbleCollidePushForce;
    private Collider2D collidedBubbleCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!isGrabbingBall)
        {
            detectedBallCollider = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(ballDetectOffset.x * transform.localScale.x, ballDetectOffset.y), ballDetectRadius, bubbleLayer);
            collidedBubbleCollider = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bubbleColliderOffset.x * transform.localScale.x, bubbleColliderOffset.y), bubbleColliderRadius, bubbleLayer);
            if (detectedBallCollider && Input.GetKeyDown(KeyCode.C))
            {
                isGrabbingBall = true;
                anim.Play("Grab");
                Debug.Log(detectedBallCollider);
            }
            if (collidedBubbleCollider)
            {
                Debug.Log("Bubble Collided");
                Vector2 direction = collidedBubbleCollider.transform.position - transform.position;
                collidedBubbleCollider.attachedRigidbody.AddForce(direction * bubbleCollidePushForce, ForceMode2D.Impulse);
            }
            Debug.Log("Normal Mode");
        }
        else
        {
            if (!detectedBallCollider)
            {
                isGrabbingBall = false;
                return;
            }

            if (Input.GetKeyUp(KeyCode.C))
            {
                isGrabbingBall = false;
                detectedBallCollider.attachedRigidbody.AddForce(rb.linearVelocity.normalized * forceOnBallRelease, ForceMode2D.Impulse);
                anim.Play("Idle");
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bubbleColliderOffset.x * transform.localScale.x, bubbleColliderOffset.y), bubbleColliderRadius);
    }
}
