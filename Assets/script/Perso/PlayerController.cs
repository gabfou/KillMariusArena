using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Stopmoving
{

    [Header("Basic setting")]
    public bool facingRight = false;
    public int life = 5;
    public float invulnTime = 1f;
    public string ouchtag = "ouch";
    public float timestunouch = 0.2f;

    [Header("Mobility and groundaison")]
    public float maxSpeed = 1f;
    public float minSlideVelocity = 3f;
    public float slimeVelocityIgnore = .5f;
    public float jumpPower = 10f;
    public float jumpIdle = .3f;
    public Vector3 groundPosition;
    public Vector2 groundSize;
    public float maxYVelocity = 8f;
    public float minYVelocity = -6f;

    [Header("Dash")]
	public float timedashinsc = 0.1f;
	public float distanceofdash = 10f;
	public bool isinvuindash = true;
	public float dashcd = 0.5f;

    [Header("Sound")]
    public AudioClip ouchClip;
    [Range(0, 1)] public float ouchVolume = 0.8f;
    public AudioClip jumpClip;
    [Range(0, 1)] public float jumpingVolume = 0.5f;
    public AudioClip TappingClip;
    [Range(0, 1)] public float tappingVolume = 0.5f;

    [HideInInspector] public bool canJump = true;
    [HideInInspector] public bool istapping = false;
    [HideInInspector] public bool grounded;
    public AudioClip run;

    [HideInInspector] public float move = 0;

    Vector2 impacto = Vector2.zero;
    CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin vcamperlin;
    Material spriteMaterial;
    bool isPlayer = false;
    bool canOuch = true;
    bool sliding = false;
    new Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    Animator anim;
    [HideInInspector] public AudioSource audiosource;
    [HideInInspector] public bool TakingDamage = false;
    [HideInInspector] public bool IsOuchstun = false;

    Collider2D  col;


    /*****************************************************************************************************************
                                                        INITIALISATION
    *****************************************************************************************************************/

    protected void reinit()
    {
        isdashing = false;
        // Flip();
        // anim.SetBool("facingright", facingRight);
        anim.SetBool("grounded", grounded);
		candash = true;
    }

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
        if (vcam)
            vcamperlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (tag == "Player")
            isPlayer = true;
        col = GetComponents<Collider2D>().Where(c => !c.isTrigger).FirstOrDefault();
        reinit();
    }

    protected void OnEnable()
    {
        init();
    }

    void Start()
    {
        reinit();
    }


    /*****************************************************************************************************************
                                                        PAS RANGÉ
    *****************************************************************************************************************/

    protected void allCheck()
    {
         GroundCheck();
         SlideCheck();
    }

    IEnumerator Tapping()
    {
        if (istapping == false)
        {
            if (TappingClip)
                audiosource.PlayOneShot(TappingClip, tappingVolume);
            istapping = true;
            anim.SetBool("istapping", true);
            // move = transform.position.x - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)).x; // tape ducoter de la sourie (en gros la ca sert a rien)
            // if (!istapping && move > 0 && !facingRight)
            //     Flip();
            // else if (!istapping && move < 0 && facingRight)
            //     Flip();

            yield return new WaitForSeconds(0.3f);
            StopTapping();
        }
    }

    void    StopTapping()
    {
        StopCoroutine(Tapping());
        anim.SetBool("istapping", false);
        istapping = false;
    }

	bool isdashing = false;

    public void Move(float move)
    {
		if (isdashing)
			return ;
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

        if (!istapping && move > 0 && facingRight)
            Flip();
        else if (!istapping && move < 0 && !facingRight)
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
        gameObject.SetActive(false);
        spriteRenderer.enabled = true;
        // GameObject.Destroy(gameObject);
    }

    void Die()
    {
        spriteRenderer.enabled = false;
        anim.SetTrigger("death");
        StartCoroutine(waitbefordying());
    }

    void eventOnDie()
    {
        if (tag == "BadGuy")
        {
            if (Random.value < 1f)
                audiosource.PlayOneShot(Camera.main.GetComponent<BankSoundStupid>().PlayerMockingHAHAHAHA, 0.7f);
        }
    }

    public void ouch(Vector2 impact2)
    {
        canOuch = false;
        life--;
        StopTapping();
        if (life < 1)
        {
            eventOnDie();
            Die();
        }
        else
        {
            if (audiosource && ouchClip)
                audiosource.PlayOneShot(ouchClip, ouchVolume);
            anim.SetTrigger("ouch");
            StartCoroutine(ResetCanOuch());
            StartCoroutine(stunouch(impact2));
            StartCoroutine(impactoEffect());
        }
    }

    IEnumerator impactoEffect()
    {
		coroutineisplayingcount++;
        TakingDamage = true;
        vcamperlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        vcamperlin.m_AmplitudeGain = 0.5f;
        vcamperlin.m_FrequencyGain = 30;
        yield return new WaitForSeconds(0.1f);
        TakingDamage = false;
        yield return new WaitForSeconds(0.2f);
        vcamperlin.m_AmplitudeGain = 0;
        cannotmove = false;
        coroutineisplayingcount--;
    }

    IEnumerator stunouch(Vector2 impact)
    {
		coroutineisplayingcount++;
        IsOuchstun = true;
        cannotmove = true;
        rigidbody2D.velocity = impact;
        spriteMaterial.SetFloat("_isflashing", 1);
        yield return new WaitForSeconds(timestunouch);
        spriteMaterial.SetFloat("_isflashing", 0);
        cannotmove = false;
        coroutineisplayingcount--;
        IsOuchstun = false;
    }

    IEnumerator ResetCanOuch()
    {
		coroutineisplayingcount++;
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
            audiosource.PlayOneShot(jumpClip, jumpingVolume);
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
														DASH
    *******************************************************************************************************************/

	bool candash = true;

	IEnumerator dash()
	{
		coroutineisplayingcount++;
		if (move == 0)
			Debug.Log(gameObject.name + " trying to dodge without moving");
		else if (candash == true)
		{
			candash = false;
			anim.SetBool("isdashing", true);
            if (isinvuindash == false)
                col.enabled = false;
            else
                col.isTrigger = true;
			float timer = 0;
			float movebysecond = distanceofdash / timedashinsc;
			float sign = Mathf.Sign(move);
            isdashing = true;
			while (timer < timedashinsc)
			{
				// rigidbody2D.MovePosition(transform.position + new Vector3(sign * movebysecond * Time.deltaTime, 0, 0));
                rigidbody2D.velocity = new Vector2(sign * movebysecond * Time.deltaTime, 0);
				timer += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
            isdashing = false;
            rigidbody2D.velocity = new Vector2(0, 0);
            if (isinvuindash == false)
                col.enabled = true;
            else
                col.isTrigger = false;
            anim.SetBool("isdashing", false);
			yield return new WaitForSeconds(dashcd);
			candash = true;
		}
		coroutineisplayingcount--;
	}


    /*****************************************************************************************************************
                                                        UPDATE
    *****************************************************************************************************************/

    void Update()
    {
        if (life < 0)
            return;
        if (base.cannotmove == true)
        {
            return;
        }
        move = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space)
            #if UNITY_STANDALONE_OSX
            || Input.GetKeyDown(KeyCode.Joystick1Button16)
            #else
            || Input.GetKeyDown(KeyCode.Joystick1Button0)
            #endif
            )
            tryjump();

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)
            #if UNITY_STANDALONE_OSX
            || Input.GetKeyDown(KeyCode.Joystick1Button18)
            #else
            || Input.GetKeyDown(KeyCode.Joystick1Button2)
            #endif
            )
            StartCoroutine(Tapping());

		if (Input.GetKeyDown(KeyCode.X)
            #if UNITY_STANDALONE_OSX
            || Input.GetKeyDown(KeyCode.Joystick1Button13) || Input.GetKeyDown(KeyCode.Joystick1Button14)
            #else
            || Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Joystick1Button5)
            #endif
            )
            StartCoroutine(dash());
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

        if (canOuch && other.tag == ouchtag)
            ouch((transform.position - other.transform.position).normalized * 2 + Vector3.up * 4);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + groundPosition, groundSize);
    }
}
