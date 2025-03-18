using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class playerM : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravity = 2f;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float cameraSmoothSpeed = 0.125f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject feet;

    private float setRunSpeed;
    public static bool isGrounded;

    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Camera playerCamera;

    public override void OnStartAuthority()
    {
        enabled = true;
    }

    [ClientCallback]
    private void Start()
    {
        playerCamera = Camera.main;
        playerCamera.transform.position = transform.position + cameraOffset;
        rb.gravityScale = gravity;
    }

    [ClientCallback]
    private void Update()
    {
        if (!isOwned) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        Move();
    }

    [ClientCallback]
    private void LateUpdate()
    {
        if (playerCamera == null) return;

        FollowPlayer();
    }

    [Client]
    private void Move()
    {
        float dir = Input.GetAxisRaw("Horizontal");

        if (dir > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (dir < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (dir != 0)
        {
            transform.Translate(new Vector2((.01f * moveSpeed) + setRunSpeed, 0));
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            setRunSpeed = runSpeed;
        }
        else
        {
            setRunSpeed = 0;
        }
    }

    [Client]
    private void FollowPlayer()
    {
        Vector3 desiredPosition = transform.position + cameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(playerCamera.transform.position, desiredPosition, cameraSmoothSpeed);
        playerCamera.transform.position = smoothedPosition;
    }

    [Command]
    public void CmdSetFeet()
    {
        feet.SetActive(true);
    }
}