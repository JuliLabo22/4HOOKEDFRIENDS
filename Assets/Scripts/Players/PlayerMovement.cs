using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LayerMask triggerColLayerMask;

    [Space]
    [Header("Inputs")]
    [SerializeField] private KeyCode leftInput;
    [SerializeField] private KeyCode rightInput;
    [SerializeField] private KeyCode jumpInput;

    //PRIVATES
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private Animator am;                        //te marque con las lineas verdes loq ue toque

    private bool isOnAir = false;
    private bool isReallyOnAir = false;
    private float timeToIsOnAir;
    private float timeCanMoveOnAir;
    private bool hasJumped;
    private float jumpTimeAnim;
    private bool cantmoveToRight;
    private bool cantmovetoLeft;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        am = GetComponent<Animator>();              ////

        am.SetBool("iddle", true);
    }

    private void Update()
    {
        if (!isReallyOnAir) Movement();
        else MovementOnAir();
        Jump();

        if (isOnAir)
        {
            timeToIsOnAir += Time.deltaTime;
            am.SetBool("jump", true);   ///lo puse aca

            if (timeToIsOnAir >= 0.6f)
            {
                isReallyOnAir = true;
            }
        }

        if (Input.GetKey(rightInput) && !isOnAir && !hasJumped && !cantmoveToRight)
        {
            am.SetBool("corrida", true);
            am.SetBool("iddle", false);
            am.SetBool("jump", false);
        }

        if (Input.GetKey(leftInput) && !isOnAir && !hasJumped && !cantmovetoLeft)
        {
            am.SetBool("corrida", true);
            am.SetBool("iddle", false);
            am.SetBool("jump", false);
        }


        if (Input.GetKeyUp(leftInput) || Input.GetKeyUp(rightInput))
        {
            am.SetBool("corrida", false);
            am.SetBool("iddle", true);
            am.SetBool("jump", false);
        }

        if(cantmovetoLeft || cantmoveToRight && rb.velocity.x == 0) 
        am.SetBool("corrida", false);

        if (Input.GetKeyDown(jumpInput))
        {

            am.SetBool("iddle", false);
            am.SetBool("corrida", false);
            ///estaba aca el anim de saltar
            hasJumped = true;
        }

        if (hasJumped)
        {
            jumpTimeAnim += Time.deltaTime;

        }
        else
        {
            jumpTimeAnim = 0;
        }
    }

    #region MOVEMENT AND JUMP

    private void MovementOnAir()
    {
        //rb.drag = 0;

        timeCanMoveOnAir += Time.deltaTime;

        if (timeCanMoveOnAir <= 0.3f) return;

        if (!CanJump())
        {
            if (Input.GetKeyDown(leftInput))
            {
                sp.flipX = false;
                rb.AddForce(Vector2.left * 10, ForceMode2D.Impulse);

                timeCanMoveOnAir = 0;
            }

            if (Input.GetKeyDown(rightInput))
            {
                sp.flipX = true;
                rb.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

                timeCanMoveOnAir = 0;
            }

            if (Input.GetKeyUp(leftInput) || Input.GetKeyUp(rightInput)) rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void Movement()
    {
        // rb.drag = 8;

        cantmoveToRight = CanMoveToRight();
        cantmovetoLeft = CanMoveToLeft();

        if (Input.GetKey(rightInput) && !cantmoveToRight)
        {
            sp.flipX = true;
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        }

        if (Input.GetKey(leftInput) && !cantmovetoLeft)
        {
            sp.flipX = false;
            rb.velocity = new Vector2(-1 * movementSpeed, rb.velocity.y);
        }

        if (Input.GetKeyUp(leftInput) || Input.GetKeyUp(rightInput)) rb.velocity = new Vector2(0, rb.velocity.y);

        Debug.DrawLine(transform.position - new Vector3(0.15f, 0, 0), transform.position - new Vector3(0.15f, 0, 0) + Vector3.down * 0.5f, Color.blue);
        Debug.DrawLine(transform.position + new Vector3(0.15f, 0, 0), transform.position + new Vector3(0.15f, 0, 0) + Vector3.down * 0.5f, Color.blue);
    }

    bool CanMoveToRight()
    {
        Debug.DrawRay(transform.position + new Vector3(0.3f, 0.3f, 0), Vector2.right * 0.2f, Color.blue);

        RaycastHit2D rightRay = Physics2D.Raycast(transform.position + new Vector3(0.3f, 0.3f, 0), Vector2.right, 0.2f, triggerColLayerMask);

        Debug.DrawRay(transform.position + new Vector3(0.3f, 0, 0) - new Vector3(0, 0.3f, 0), Vector2.right * 0.2f, Color.blue);

        RaycastHit2D rightRay2 = Physics2D.Raycast(transform.position + new Vector3(0.3f, 0, 0) - new Vector3(0, 0.3f, 0), Vector2.right, 0.2f, triggerColLayerMask);

        if ((rightRay.collider != null && !rightRay.collider.gameObject.Equals(gameObject)) || (rightRay2.collider != null && !rightRay2.collider.gameObject.Equals(gameObject)))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            am.SetBool("corrida", false);
            am.SetBool("iddle", true);
            return true;
        }
        else return false;
    }

    bool CanMoveToLeft()
    {
        Debug.DrawRay(transform.position - new Vector3(0.3f, 0.3f, 0), Vector2.left * 0.2f, Color.blue);

        RaycastHit2D rightRay = Physics2D.Raycast(transform.position - new Vector3(0.3f, 0.3f, 0), Vector2.left, 0.2f, triggerColLayerMask);

        Debug.DrawRay(transform.position - new Vector3(0.3f, 0, 0) + new Vector3(0, 0.3f, 0), Vector2.left * 0.2f, Color.blue);

        RaycastHit2D rightRay2 = Physics2D.Raycast(transform.position - new Vector3(0.3f, 0, 0) + new Vector3(0, 0.3f, 0), Vector2.left, 0.2f, triggerColLayerMask);

        if ((rightRay.collider != null && !rightRay.collider.gameObject.Equals(gameObject)) || (rightRay2.collider != null && !rightRay2.collider.gameObject.Equals(gameObject)))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            am.SetBool("corrida", false);
            am.SetBool("iddle", true);

            return true;
        }
        else return false;
    }

    private void Jump()
    {
        if (!CanJump()) return;

        if (Input.GetKeyDown(jumpInput))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce * Time.fixedDeltaTime);
        }
    }

    private bool CanJump()
    {

        var canjump = false;
        am.SetBool("jump", false);///y puse esto
        if (Physics2D.Raycast(transform.position - new Vector3(0.15f, 0, 0), Vector2.down, 0.5f, layerMask) ||
            Physics2D.Raycast(transform.position + new Vector3(0.15f, 0, 0), Vector2.down, 0.5f, layerMask))
        {
            canjump = true;
            isOnAir = false;
            isReallyOnAir = false;
            timeToIsOnAir = 0;

            if (isOnAir) rb.velocity = new Vector2(0, rb.velocity.y);

            timeCanMoveOnAir = 0;

            if (jumpTimeAnim > 0.1f)
            {
                jumpTimeAnim = 0;
                hasJumped = false;
                am.SetBool("iddle", true);
                am.SetBool("jump", false);
            }
        }
        else isOnAir = true;

        return canjump;
    }

    #endregion

}