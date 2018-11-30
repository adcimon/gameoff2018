using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class MovementController : MonoBehaviour
{
    [Header("Controls")]
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    [Header("Settings")]
    public float horizontalSpeed = 1;
    public float verticalSpeed = 1;

    [Space(10)]
    public Vector3 faceDirection = Vector3.down;

    private new Rigidbody2D rigidbody2D;
    private Animator animator;
    private ShootController shootController;

    private void Awake()
    {
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        // Get input.
        Vector2 input = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));

        // Constraint the movement to one axis.
        if( Mathf.Abs(input.x) > Mathf.Abs(input.y) )
        {
            input.y = 0;
        }
        else
        {
            input.x = 0;
        }

        // Set the animator movement values.
        animator.SetInteger("X", ((input.x == 0) ? 0 : 1));
        animator.SetInteger("Y", ((input.y == 0) ? 0 : 1));

        if( input != Vector2.zero )
        {
            // Set the direction.
            if( input.x > 0 )
            {
                // Right.
                faceDirection = Vector3.right;
                animator.SetInteger("Direction", 1);
            }
            else if( input.x < 0 )
            {
                // Left.
                faceDirection = Vector3.left;
                animator.SetInteger("Direction", 3);
            }
            else if( input.y > 0 )
            {
                // Up.
                faceDirection = Vector3.up;
                animator.SetInteger("Direction", 0);
            }
            else if( input.y < 0 )
            {
                // Down.
                faceDirection = Vector3.down;
                animator.SetInteger("Direction", 2);
            }

            // Move.
            Vector2 movement = Vector2.zero;
            movement.x = input.x * horizontalSpeed * Time.deltaTime;
            movement.y = input.y * verticalSpeed * Time.deltaTime;
            movement.x += gameObject.transform.position.x;
            movement.y += gameObject.transform.position.y;
            rigidbody2D.MovePosition(movement);
        }
    }
}