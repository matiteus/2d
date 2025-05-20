using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControls : MonoBehaviour
{
    public static PlayerControls Instance { get; private set; } = null;

    [SerializeField]
    private float movementSpeed = 5f;
    private PlayerInteractableHandler interactionScript;

    private float currentSpeed = 0f;
    [SerializeField]
    private float speedMultiplier = 1;

    private Controls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            
            Destroy(gameObject);
        }
        playerControls = new Controls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        playerControls.Movement.Enable();
        playerControls.Movement.Move.performed += InputMovePerformed;
        playerControls.Movement.Move.canceled += InputMoveCanceled;
        playerControls.Movement.Interact.performed += OnInteractPerformed;
        playerControls.Menu.ReadLetter.performed += OnReadLetterPerformed;
        playerControls.Movement.Run.performed += RunActionPerformed;
        playerControls.Movement.Run.canceled += RunActionCanceled;

        interactionScript = this.GetComponent<PlayerInteractableHandler>();
        Checks();
    }

    private void Checks()
    {
        if (playerControls == null)
        {
            Debug.Log("player controls is null");
        }
        if (rb == null)
        {
            Debug.Log("Rb is null");
        }
        if (myAnimator == null)
        {
            Debug.Log("animator is null");
        }
        if (mySpriteRenderer == null)
        {
            Debug.Log("sprite is null");
        }
        if (interactionScript == null)
        {
            Debug.Log("script is null");
        }
        if (rb == null)
        {
            Debug.Log("Rb is null");
        }
    }
    private void OnDestroy()
    {
        playerControls.Movement.Move.performed -= InputMovePerformed;
        playerControls.Movement.Move.canceled -= InputMoveCanceled;
        playerControls.Movement.Interact.performed -= OnInteractPerformed;
        playerControls.Menu.ReadLetter.performed -= OnReadLetterPerformed;
        playerControls.Movement.Run.performed -= RunActionPerformed;
        playerControls.Movement.Run.canceled -= RunActionCanceled;
        playerControls.Disable();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * (currentSpeed * Time.fixedDeltaTime)*speedMultiplier);
    }
    private void InputMovePerformed(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
        myAnimator.SetBool("IsMoving", movement.magnitude > 0);
        currentSpeed = movementSpeed;
        CheckFlip(movement.x);
    }
    private void InputMoveCanceled(InputAction.CallbackContext context)
    {
        myAnimator.SetBool("IsMoving", false);
        currentSpeed = 0f;
    }
    private void OnReadLetterPerformed(InputAction.CallbackContext context)
    {
        ReReadLetter.Instance.ReadLetter();
    }
    private void RunActionPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Run action performed");
        speedMultiplier = 1.5f;
    }
    private void RunActionCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Run action canceled");
        speedMultiplier = 1f;
    }

    private void CheckFlip(float horizontalInput)
    {
        if (horizontalInput * transform.right.x < 0f)
        {
            Flip();
        }

    }
    private void Flip()
    {
        if (movement.x != 0)
        {
            transform.right = -transform.right;
        }

    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        interactionScript.StartInteraction();
    }
    public void EnableMovement()
    {
        playerControls.Movement.Enable();
    }
    public void DisableMovement()
    {
        playerControls.Movement.Disable();
    }
    public void EnableReReadLetter()
    {
        playerControls.Menu.Enable();
    }
    public Vector2 GetPlayerPosition()
    {
        return rb.position;
    }
    public void SetPlayerPosition(Vector2? pos)
    {
        if (pos != null)
        {
            rb.position = (Vector2)pos;
        }
        else
        {
            Debug.LogError("Player position is null big boo boo big man");
        }
    }
}
