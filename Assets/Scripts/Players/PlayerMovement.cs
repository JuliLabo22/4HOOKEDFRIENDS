using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask layerMask;

    [Space]
    [Header("Inputs")]
    [SerializeField] private KeyCode leftInput;
    [SerializeField] private KeyCode rightInput;
    [SerializeField] private KeyCode jumpInput;

    //PRIVATES
    private Rigidbody2D rb;
    private SpriteRenderer sp;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Movement();
        Jump();
    }

    #region MOVEMENT AND JUMP

    private void Movement()
    {
        if (Input.GetKey(leftInput))
        {
            sp.flipX = false;
            rb.velocity = new Vector2(-1 * movementSpeed, rb.velocity.y);
        }

        if (Input.GetKey(rightInput))
        {
            sp.flipX = true;
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        }

        if (Input.GetKeyUp(leftInput)) rb.velocity = new Vector2(0, rb.velocity.y);
        if (Input.GetKeyUp(rightInput)) rb.velocity = new Vector2(0, rb.velocity.y);

        Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.75f, Color.blue);
    }

    private void Jump()
    {
        if(!CanJump()) return;

        if (Input.GetKeyDown(jumpInput)) rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool CanJump()
    {
        var canjump = false;

        if (Physics2D.Raycast(transform.position, Vector2.down, 0.75f, layerMask)) canjump = true;

        return canjump;
    }

    #endregion

}