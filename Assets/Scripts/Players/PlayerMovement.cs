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
    private Animator am;                        //te marque con las lineas verdes loq ue toque

    private bool isOnAir = false;
    private bool isReallyOnAir = false;
    private float timeToIsOnAir;
    private float timeCanMoveOnAir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        am = GetComponent<Animator>();              ////
    }

    private void Update()
    {
        if (!isReallyOnAir) Movement();
        else MovementOnAir();
        Jump();

        if (isOnAir)
        {
            timeToIsOnAir += Time.deltaTime;

            if (timeToIsOnAir >= 0.8f)
            {
                isReallyOnAir = true;
            }
        }
    }

    #region MOVEMENT AND JUMP

    private void MovementOnAir()
    {
        rb.drag = 0;

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
        }
    }

    private void Movement()
    {
        rb.drag = 8;

        if (Input.GetKey(leftInput))
        {
            sp.flipX = false;
            rb.velocity = new Vector2(-1 * movementSpeed, rb.velocity.y);
            am.SetBool("iddle", false);                 ///
            am.SetBool("corrida", true);                ///

        }

        if (Input.GetKey(rightInput))
        {
            sp.flipX = true;
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
            am.SetBool("iddle", false);                                 ///
            am.SetBool("corrida", true);                                ///

        }

        if (Input.GetKeyUp(leftInput))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);        ///te cambie esta parte porque queria agregarle la animacionjeje
            am.SetBool("corrida", false);                       ///estaba todo en la misma linea nomas le agregue el animator
            am.SetBool("iddle", true);                          ///si esta mal cambialo we
        }
        if (Input.GetKeyUp(rightInput))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            am.SetBool("corrida", false);                       ///
            am.SetBool("iddle", true);                          ///
        }

        Debug.DrawLine(transform.position, transform.position + Vector3.down * 0.75f, Color.blue);
    }

    private void Jump()
    {
        if (!CanJump()) return;

        if (Input.GetKeyDown(jumpInput))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private bool CanJump()
    {
        var canjump = false;

        if (Physics2D.Raycast(transform.position, Vector2.down, 0.75f, layerMask))
        {
            canjump = true;
            isOnAir = false;
            isReallyOnAir = false;
            timeToIsOnAir = 0;

            timeCanMoveOnAir = 0;
        }
        else isOnAir = true;

        return canjump;
    }

    #endregion

}