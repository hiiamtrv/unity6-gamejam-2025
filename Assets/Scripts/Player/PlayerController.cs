using NUnit.Framework;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool IsUsingMouse;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Move")] [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;

    private void Awake()
    {
        IsUsingMouse = false;
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (IsUsingMouse && Input.anyKey)
        {
            IsUsingMouse = false;
        }

        if (!IsUsingMouse && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
        {
            IsUsingMouse = true;
        }

        var moveInput = Vector2.zero;
        if (!IsUsingMouse)
        {
            // Player Movement
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else
        {
            var mousePos = Input.mousePosition;
            var playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            moveInput = (mousePos - playerScreenPos).normalized;
        }

        var topVel = moveSpeed * moveInput;
        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, topVel, acceleration * Time.deltaTime);

        // Flip Sprite
        if (moveInput.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y, transform.localScale.z);
        }
    }
}