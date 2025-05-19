using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public static PlayerControls Instance { get; private set; } = null;

    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private Transform interactionChild;
    private PlayerInteractableHandler interactionScript;

    private float currentSpeed = 0f;

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
        interactionScript = interactionChild.GetComponent<PlayerInteractableHandler>();
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
        if (interactionChild == null)
        {
            Debug.Log("child is null");
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
        playerControls.Disable();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * (currentSpeed * Time.fixedDeltaTime));
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
}
