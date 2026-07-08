using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 10.0f;
    public float JumpSpeed = 50.0f;

    // Awake is called once before OnEnable() before Start()
    void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        mMoveAction = playerInput.actions.FindAction("Move");
        mJumpAction = playerInput.actions.FindAction("Jump");

        mRigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created (only if the component is Enabled)
    void Start()
    {
    }

    void OnJump()
    {
        mOnJump = true;
    }

    // FixedUpdate is called at 50 FPS / fixed interval 20 ms
    void FixedUpdate()
    {
        // Horizontal movement with instantaneous accelaration and decelleration
        var moveValue = mMoveAction.ReadValue<Vector2>();
        mRigidBody.linearVelocityX = moveValue.x * MoveSpeed;

        // Jump only on the "press down" button event, requiring the right timing
        if (mOnJump)
        {
            bool isGrounded = IsGrounded();
            if (isGrounded)
            {
                mRigidBody.linearVelocityY = JumpSpeed;
            }

            mOnJump = false;
        }

        // Long/Short jump: interrupt jump when stop pressing
        if (mRigidBody.linearVelocityY > 0.0f)
        {
            var jumpValue = mJumpAction.ReadValue<float>();
            bool isJumpPressed = (jumpValue > 0.0f);
            if (!isJumpPressed)
            {
                mRigidBody.linearVelocityY = 0.0f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    bool IsGrounded()
    {
        // TODO handle jump on overhang right before falling?
        // TODO handle jump again right before touching the ground?
        return mRigidBody.linearVelocityY == 0.0f;
    }

    InputAction mMoveAction;
    InputAction mJumpAction;

    bool mOnJump;

    Rigidbody2D mRigidBody; // Handmle physics (gravity & collisions)
}
