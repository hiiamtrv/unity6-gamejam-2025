using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Move")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        // Player Movement
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var topVel = moveSpeed * moveInput;
        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, topVel, acceleration * Time.deltaTime);
        
        // Flip Sprite
        if (moveInput.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        if (Input.GetKeyDown(KeyCode.Escape))
            UIManager.Instance.PauseGame();
    }
}
