using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public static PlayerBehavior Instance {  get; private set; }

    [SerializeField] private float movingSpeed = 5f;
    private float minSpeed = 0.1f;
    private bool isRunning = false;

    private Rigidbody2D rb;
    
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        inputVector = inputVector.normalized;
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > minSpeed || Mathf.Abs(inputVector.y)>minSpeed)
        {
            isRunning = true;
        }
        else
        {
            isRunning= false;
        }
    }

    public bool IsRunning()
    {
    

        return isRunning;
    }

    public Vector3 GetPlayeScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }
}
























