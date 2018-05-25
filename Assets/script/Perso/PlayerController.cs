using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Stopmoving
{

    public float maxSpeed = 1f;
    public bool facingRight = false;

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

    [HideInInspector] public bool istapping = false;

    [HideInInspector] public bool grounded;
    public Vector3 groundPosition;
    public Vector2 groundSize;
    public AudioClip run;

    new Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    Animator anim;
    AudioSource audiosource;
    public string ouchtag = "ouch";

    [HideInInspector] public float move = 0;

    Vector2 impacto = Vector2.zero;
    CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin vcamperlin;
    Material spriteMaterial;

    // Use this for initialization

    protected void init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteMaterial = spriteRenderer.material;
        rigidbody2D = GetComponent<Rigidbody2D>();

        rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
        anim = GetComponent<Animator>();
        audiosource = Camera.main.GetComponent<AudioSource>();
        vcam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        vcamperlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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

    IEnumerator Tapping()
    {
        istapping = true;
        anim.SetBool("istapping", true);
        // move = transform.position.x - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)).x; // tape ducoter de la sourie (en gros la ca sert a rien)
        // if (!istapping && move > 0 && !facingRight)
        //     Flip();
        // else if (!istapping && move < 0 && facingRight)
        //     Flip();

        yield return new WaitForSeconds(0.3f);
        anim.SetBool("istapping", false);
        istapping = false;
    }

    public void Move(float move)
    {
        if (audiosource && run)
        {
            if (grounded && audiosource.isPlaying == false && move != 0)
            {
                audiosource.loop = true;
                audiosource.clip = run;
                audiosource.Play();
            }
            else if (move == 0 && audiosource.clip == run)
                audiosource.Stop();
        }

        if (move > 0 && facingRight)
            Flip();
        else if (move < 0 && !facingRight)
            Flip();
        anim.SetFloat("velx", move);
        anim.SetBool("ismoving", move != 0);
        rigidbody2D.velocity = new Vector2( Mathf.Clamp(move * maxSpeed + impacto.x, -maxSpeed, maxSpeed),
                                            Mathf.Clamp(rigidbody2D.velocity.y + impacto.y, minYVelocity, maxYVelocity));
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


    /*****************************************************************************************************************
                                                        OUCH & DIE
    *****************************************************************************************************************/

    int coroutineisplayingcount = 0;

    IEnumerator    waitbefordying()
    {
        while (coroutineisplayingcount > 0)
            yield return new WaitForEndOfFrame();
        GameObject.Destroy(gameObject);
    }

    void Die()
    {
        anim.SetTrigger("death");
        gameObject.SetActive(false);
        StartCoroutine(waitbefordying());
    }

    void ouch(Vector2 impact2)
    {
        canOuch = false;
        life--;
        if (life < 1)
            Die();
        else
        {
            if (audiosource && ouchClip)
                audiosource.PlayOneShot(ouchClip, .6f);
            anim.SetTrigger("ouch");
        }
		coroutineisplayingcount += 3;
        StartCoroutine(ResetCanOuch());
        StartCoroutine(stunouch(impact2));
        StartCoroutine(impactoEffect());
    }

    IEnumerator impactoEffect()
    {
        vcamperlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        vcamperlin.m_AmplitudeGain = 0.5f;
        vcamperlin.m_FrequencyGain = 30;
        yield return new WaitForSeconds(0.3f);
        vcamperlin.m_AmplitudeGain = 0;
        cannotmove = false;
        coroutineisplayingcount--;
    }

    public float timestunouch = 0.2f;
    IEnumerator stunouch(Vector2 impact)
    {
        cannotmove = true;
        rigidbody2D.velocity = impact;
        spriteMaterial.SetFloat("_isflashing", 1);
        yield return new WaitForSeconds(timestunouch);
        spriteMaterial.SetFloat("_isflashing", 0);
        cannotmove = false;
        coroutineisplayingcount--;
    }

    IEnumerator ResetCanOuch()
    {
        yield return new WaitForSeconds(invulnTime);
        canOuch = true;
        coroutineisplayingcount--;
    }

    /*****************************************************************************************************************
                                                        JUMP
    *******************************************************************************************************************/

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

    /*****************************************************************************************************************
                                                        UPDATE
    *****************************************************************************************************************/

    void Update()
    {
        if (life < 0)
            return;
        if (base.cannotmove == true)
            return;
        move = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
            tryjump();

        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCoroutine(Tapping());
    }

    protected virtual void FixedUpdate()
    {
        if (life < 0)
            return;
        allCheck();

        if (base.cannotmove == true)
            return;

        Move(move);
    }

    /*****************************************************************************************************************
                                                        COLLISION
    *****************************************************************************************************************/

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Slime")
        {
            if (rigidbody2D.velocity.y < slimeVelocityIgnore)
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        }
    }


    virtual protected void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "OS")
        {
            life = 0;
            Die();
            return;
        }

        if (other.tag == "bam")
            Debug.Log(name);
        if (canOuch && other.tag == ouchtag)
            ouch((transform.position - other.transform.position).normalized * 6 + Vector3.up * 12);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + groundPosition, groundSize);
    }
}
