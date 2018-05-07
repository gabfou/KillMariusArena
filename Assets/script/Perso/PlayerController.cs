using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Stopmoving
{

    public float maxSpeed = 1f;
    [HideInInspector] public bool facingRight = false;

    [Space]
    public float jumpPower = 10f;
    public float jumpIdle = .3f;
    public float maxYVelocity = 8f;
    public float minYVelocity = -6f;
    [HideInInspector] public bool canJump = true;

    [Space]
    public float minSlideVelocity = 3f;

    [Space]
    public float slimeVelocityIgnore = .5f;

    [Space]
    public AudioClip ouchClip;
    public AudioClip jumpClip;

    public int life = 5;
    public float invulnTime = 1f;
    bool canOuch = true;
    bool sliding = false;

    bool istapping = false;
    public Collider2D zonebam;

    [HideInInspector] public bool grounded;
    public Vector3 groundPosition;
    public Vector2 groundSize;
    public AudioClip run;

    new Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    Animator anim;
    AudioSource audiosource;
    protected string ouchtag = "ouch";

    [HideInInspector] public float move = 0;

    // Use this for initialization

    protected void init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
        anim = GetComponent<Animator>();
        audiosource = Camera.main.GetComponent<AudioSource>();
        // Flip();
        // anim.SetBool("facingright", facingRight);
        anim.SetBool("grounded", grounded);

    }

    void Start()
    {
        init();
    }

    protected void allCheck()
    {
         GroundCheck();
         SlideCheck();
    }
    protected virtual void FixedUpdate()
    {
        if (life < 0)
            return;
        allCheck();

        Tapping(move);

        if (base.cannotmove == true)
            return;
        // Debug.Log(name);

        Move(move);


    }

    IEnumerator Tapping(float move)
    {
        istapping = true;
        anim.SetTrigger("istapping");
        // move = transform.position.x - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)).x; // tape ducoter de la sourie (en gros la ca sert a rien)
        // if (!istapping && move > 0 && !facingRight)
        //     Flip();
        // else if (!istapping && move < 0 && facingRight)
        //     Flip();

        yield return new WaitForSeconds(0.1f); // TODO: faire une coroutine
        zonebam.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        zonebam.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        istapping = false;
    }

    public void Move(float move)
    {
        Debug.Log(name);
        /*if (grounded && audiosource.isPlaying == false && move != 0)
        {
            audiosource.loop = true;
            audiosource.clip = run;
            audiosource.Play();
        }
        else if (move == 0 && audiosource.clip == run)
            audiosource.Stop();*/

        if (!istapping && move > 0 && facingRight)
            Flip();
        else if (!istapping && move < 0 && !facingRight)
            Flip();
        anim.SetFloat("velx", move);
        anim.SetBool("ismoving", move != 0);
        rigidbody2D.velocity = new Vector2(move * maxSpeed, Mathf.Clamp(rigidbody2D.velocity.y, minYVelocity, maxYVelocity));
        anim.SetFloat("vely", rigidbody2D.velocity.y);
    }

    void GroundCheck()
    {
        RaycastHit2D[] results = new RaycastHit2D[10];
        int collisionNumber = Physics2D.BoxCastNonAlloc(transform.position + groundPosition, groundSize, 0, Vector2.down, results, .0f, 1 << LayerMask.NameToLayer("Ground"));

        grounded = collisionNumber != 0;

        anim.SetBool("grounded", grounded);
    }

    void SlideCheck()
    {
        float arx = Mathf.Abs(rigidbody2D.velocity.x);
        if (arx > minSlideVelocity && Input.GetKeyDown(KeyCode.DownArrow))
            sliding = true;
        if (arx < minSlideVelocity || !grounded)
            sliding = false;

        anim.SetBool("sliding", sliding);
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void Die()
    {
        anim.SetTrigger("death");
    }

    void ouch()
    {
        canOuch = false;
        StartCoroutine(ResetCanOuch());
        life--;
        if (life < 1)
            Die();
        else
        {
            audiosource.PlayOneShot(ouchClip, .6f);
            anim.SetTrigger("ouch");
        }
    }

    IEnumerator ResetCanOuch()
    {
        yield return new WaitForSeconds(invulnTime);
        canOuch = true;
    }

    IEnumerator JumpDelay()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpIdle);
        canJump = true;
    }

    public void tryjump(float jumppower)
    {
        if (grounded && canJump)
        {
            rigidbody2D.AddForce(new Vector2(0, jumppower), ForceMode2D.Impulse);
            StartCoroutine(JumpDelay());
            rigidbody2D.velocity = new Vector2(move * maxSpeed, Mathf.Clamp(rigidbody2D.velocity.y, minYVelocity, maxYVelocity));
        }
    }

    public void tryjump()
    {
        tryjump(jumpPower);
    }

    void Update()
    {
        if (life < 0)
            return;
        if (base.cannotmove == true)
            return;
        move = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
            tryjump();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Slime")
        {
            if (rigidbody2D.velocity.y < slimeVelocityIgnore)
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "OS")
        {
            life = 0;
            Die();
            return;
        }

        if (canOuch && other.tag == ouchtag)
            ouch();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + groundPosition, groundSize);
    }
}
