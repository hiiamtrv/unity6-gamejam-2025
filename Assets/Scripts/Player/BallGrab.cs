using UnityEngine;

public class BallGrab : MonoBehaviour
{
    private bool isGrabbingBall;
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private float ballDetectRadius;
    [SerializeField] private Vector2 ballDetectOffset;
    [SerializeField] private LayerMask bubbleLayer;
    [SerializeField] private float forceOnBallRelease;
    private Collider2D detectedBallCollider;

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
            if (detectedBallCollider && Input.GetKeyDown(KeyCode.C))
            {
                isGrabbingBall = true;
                anim.Play("Grab");
                Debug.Log(detectedBallCollider);
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
    }
}
