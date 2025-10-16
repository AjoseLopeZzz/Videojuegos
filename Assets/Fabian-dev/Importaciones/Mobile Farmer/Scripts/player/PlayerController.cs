using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour
{
    [Header("Elementos")]
    [SerializeField] private MobileJoystick joystick;
    private PlayerAnimator playerAnimator;
    private CharacterController characterController;

    [Header("Configuraciones")]
    [SerializeField] private float moveSpeed = 5f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    void Update()
    {
        ManageMovement();
    }

    private void ManageMovement()
    {
        Vector3 moveVector = Vector3.zero;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (joystick != null)
        {
            Vector2 joyInput = joystick.GetMoveVector();
            horizontal += joyInput.x;
            vertical += joyInput.y;
        }

        moveVector = new Vector3(horizontal, 0f, vertical);

        if (moveVector.magnitude > 1f)
            moveVector.Normalize();

        characterController.Move(moveVector * moveSpeed * Time.deltaTime);

        playerAnimator.ManageAnimations(moveVector);
    }
}
