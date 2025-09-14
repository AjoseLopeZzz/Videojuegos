using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour
{
    [Header("Elementos")]
    [SerializeField] private MobileJoystick joystick;
    private PlayerAnimator playerAnimator;
    private CharacterController characterController;


    [Header(" Configuraciones")]
    [SerializeField] private float moveSpeed;
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
        Vector3 moveVector = joystick.GetMoveVector() * moveSpeed * Time.deltaTime / Screen.width;

        moveVector.z = moveVector.y;
        moveVector.y = 0f;

        characterController.Move(moveVector);

        playerAnimator.ManageAnimations(moveVector);
    }
}
