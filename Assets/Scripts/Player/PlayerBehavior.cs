using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public static PlayerBehavior Instance { get; private set; }

    private Rigidbody2D rb;
    private KnockBack knockBack;

    [SerializeField] private float movingSpeed = 5f;
    private float minSpeed = 0.1f;
    private bool isRunning = false;
    private Vector2 inputVector;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
    }



    private void Update()
    {

        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (knockBack.IsGetKnockedBack)
            return;

        HandleMovement();

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
    public void TakeDamage(Transform damageSource, int damage)
    {
        knockBack.GetKnockedBack(damageSource);
    }


    private void HandleMovement()
    {

        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > minSpeed || Mathf.Abs(inputVector.y) > minSpeed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }


    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }


}
























