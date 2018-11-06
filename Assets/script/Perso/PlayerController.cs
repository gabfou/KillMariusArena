using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Stopmoving
{

    [Header("Basic setting")]
    public Text lifeText = null; 
    public bool facingLeft = false;
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
    public AudioClip run;
    [Range(0, 1)] public float runVolume = 0.5f;    
    public AudioClip DieClip;
    [Range(0, 1)] public float DieVolume = 0.5f;

    [Header("Ouch")]
    public float ouchJumpMultPushX = 2;
    public float ouchJumpMultPushY = 4;

    [HideInInspector] public bool canJump = true;
    [HideInInspector] public bool istapping = false;
    [HideInInspector] public bool grounded;

    [HideInInspector] public float move = 0;
    [HideInInspector] public float movey = 0;

    [Header("Events")]
    public UnityEvent   onTakeDamage;

    Vector2 impacto = Vector2.zero;
    CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin vcamperlin;
    Material spriteMaterial;
    bool isPlayer = false;
    bool canOuch = true;
    bool sliding = false;
    new Rigidbody2D rigidbody2D;
    protected SpriteRenderer spriteRenderer;
    protected Animator anim;
    [HideInInspector] public AudioSource audiosource;
    [HideInInspector] public AudioSource audiosource2;
    [HideInInspector] public bool TakingDamage = false;
    [HideInInspector] public bool IsOuchstun = false;
    float baseGravityScale;

    protected Collider2D  col;
    [HideInInspector] public bool isDead;

	[HideInInspector] public Vector2 lastCheckpoint = Vector2.negativeInfinity;
    int baseLayer;
    [HideInInspector] public Rigidbody2D rbparent= null;


    /*****************************************************************************************************************
                                                        INITIALISATION
    *****************************************************************************************************************/

    protected void reinit()
    {
        if (Vector2.negativeInfinity == lastCheckpoint)
            lastCheckpoint = transform.position;
        isdashing = false;
        // Flip();
        // anim.SetBool("facingLeft", facingLeft);
        anim.SetBool("grounded", grounded);
		candash = true;
        rigidbody2D.gravityScale = baseGravityScale;
        gameObject.layer = baseLayer;
        if (lifeText)
            lifeText.text = life.ToString();
        IsOuchstun = false;
        isDead = false;
        if (isPlayer && GameManager.instance && GameManager.instance.save.lastCheckpoint != Vector2.zero)
            transform.position = GameManager.instance.save.lastCheckpoint;
    }

    protected void init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteMaterial = spriteRenderer.material;
        rigidbody2D = GetComponent<Rigidbody2D>();
        baseLayer = gameObject.layer;
        rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
        anim = GetComponent<Animator>();
        audiosource = Camera.main.GetComponent<AudioSource>();
        audiosource2 = GetComponent<AudioSource>();
        vcam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        if (vcam)
            vcamperlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (tag == "Player")
            isPlayer = true;
        col = GetComponents<Collider2D>().Where(c => !c.isTrigger).FirstOrDefault();
        baseGravityScale = rigidbody2D.gravityScale;
        if (isPlayer && GameManager.instance)
            GameManager.instance.player = this;
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

    public void DoingDamage(Vector2 pos)
    {
        if (tag == "player")
        {
            rigidbody2D.velocity = new Vector2(Mathf.Sign(transform.position.x - pos.x) * 10, rigidbody2D.velocity.y);
        }
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
            // if (!istapping && move > 0 && !facingLeft)
            //     Flip();
            // else if (!istapping && move < 0 && facingLeft)
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

    public void Move(float move, float movey = 0)
    {
		if (isdashing || rigidbody2D.bodyType != RigidbodyType2D.Dynamic)
			return ;
        if (audiosource2 && run)
        {
            if (grounded && audiosource2.isPlaying == false && move != 0 && grounded)
            {
                audiosource2.loop = true;
                audiosource2.clip = run;
                audiosource2.volume = runVolume;
                audiosource2.Play();
            }
            else if ((move == 0 || !grounded) && audiosource2.clip == run)
            {
                audiosource2.clip = null;
                audiosource2.Stop();
            }
        }
        if (IsOnLadder)
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, ((IsOnLadder) ?  movey : 0) * maxSpeed);
        if (/*!istapping && */move > 0 && facingLeft)
            Flip();
        else if (/*!istapping && */move < 0 && !facingLeft)
            Flip();
        anim.SetFloat("velx", move);
        anim.SetBool("ismoving", move != 0 || (IsOnLadder && movey != 0));
        rigidbody2D.velocity = new Vector2( Mathf.Clamp(move * maxSpeed + impacto.x, -maxSpeed, maxSpeed),
                                            Mathf.Clamp(rigidbody2D.velocity.y + impacto.y, minYVelocity, maxYVelocity));
        anim.SetFloat("vely", rigidbody2D.velocity.y);
        if (rbparent)
            rigidbody2D.velocity +=  new Vector2(rbparent.velocity.x, 0);
    }

    Collider2D actualGround;
    protected virtual void GroundCheck()
    {
        RaycastHit2D[] results = new RaycastHit2D[10];
        int collisionNumber = Physics2D.BoxCastNonAlloc(transform.position + groundPosition, groundSize, 0, Vector2.down, results, .0f, 1 << (LayerMask.NameToLayer("Ground")) | 1 << (LayerMask.NameToLayer("GroundOneWay")));

        grounded = collisionNumber != 0;
        if (collisionNumber != 0)
            actualGround = results.First().collider;
        collisionNumber = Physics2D.BoxCastNonAlloc(transform.position + groundPosition, groundSize, 0, Vector2.down, results, .0f, 1 << LayerMask.NameToLayer("Ladder"));
        grounded = grounded || collisionNumber != 0;
        if (IsOnLadder || move == 0 || movey != 0)
            IsOnLadder = collisionNumber != 0;
        rigidbody2D.gravityScale = (IsOnLadder) ? 0 : baseGravityScale;

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

    protected void Flip()
    {
        facingLeft = !facingLeft;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }


    /*****************************************************************************************************************
                                                        OUCH & DIE
    *****************************************************************************************************************/

    int coroutineisplayingcount = 0;

    IEnumerator    waitbefordying()
    {
        yield return new WaitForSeconds(1);
        // spriteRenderer.enabled = true;
        while (coroutineisplayingcount > 0)
            yield return new WaitForEndOfFrame();;
        gameObject.SetActive(false);
        if (tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // SaveLoad.Save();
        }
        GameObject.Destroy(gameObject);
    }

    void Die()
    {
        if (audiosource && DieClip)
            audiosource.PlayOneShot(DieClip, DieVolume);
        // spriteRenderer.enabled = false;
        isDead = true;
        IsOuchstun = true;
        StopTapping();
        gameObject.layer = LayerMask.NameToLayer("TouchNothing");
        rigidbody2D.gravityScale = 0;
        cannotmove = true;
        // GetComponentsInChildren<GameObject>();
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
        onTakeDamage.Invoke();
        rigidbody2D.velocity = Vector2.zero;
        canOuch = false;
        life--;
        if (lifeText)
            lifeText.text = life.ToString();
        StopTapping();
        if (impact2.x > 0 && !facingLeft)
            Flip();
        else if (impact2.x < 0 && facingLeft)
            Flip();
        if (audiosource && ouchClip)
            audiosource.PlayOneShot(ouchClip, ouchVolume);
        if (life < 1)
        {
            eventOnDie();
            Die();
        }
        else
        {

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
        anim.SetBool("ouch", true);
        IsOuchstun = true;
        cannotmove = true;
        rigidbody2D.velocity = impact;
        spriteMaterial.SetFloat("_isflashing", 1);
        yield return new WaitForSeconds(timestunouch);
        spriteMaterial.SetFloat("_isflashing", 0);
        cannotmove = false;
        IsOuchstun = false;
        anim.SetBool("ouch", false);
        coroutineisplayingcount--;
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

    IEnumerator tryGoUnderCo()
    {
        Collider2D col2 = actualGround;

        Physics2D.IgnoreCollision(col, col2, true);
        yield return new WaitForSeconds(1);
        Physics2D.IgnoreCollision(col, col2, false);
    }

    public void tryGoUnder()
    {
        if (actualGround.usedByEffector == true && actualGround.GetComponent<PlatformEffector2D>() != null)
            StartCoroutine(tryGoUnderCo());
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
                col.isTrigger = true;
            else
                gameObject.layer = 12;
               
			float timer = 0;
			float movebysecond = distanceofdash / timedashinsc;
			float sign = Mathf.Sign(move);
            isdashing = true;
			while (timer < timedashinsc)
			{
				// rigidbody2D.MovePosition(transform.position + new Vector3(sign * movebysecond * Time.deltaTime, 0, 0));
                rigidbody2D.velocity = new Vector2(sign * movebysecond * Time.fixedDeltaTime, 0);
				timer += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
            isdashing = false;
            rigidbody2D.velocity = new Vector2(0, 0);
            if (isinvuindash == false)
                col.isTrigger = false;
            else
                gameObject.layer = baseLayer;
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
            return;
        move = Input.GetAxisRaw("Horizontal");
        movey = Input.GetAxisRaw("Vertical");
        bool iscrouching = false;
       // move = (istapping) ? move / 2 : move;


        if ((Input.GetKey(KeyCode.Space)
            #if UNITY_STANDALONE_OSX
            || Input.GetKey(KeyCode.Joystick1Button16)
            #else
            || Input.GetKey(KeyCode.Joystick1Button0)
            #endif
            )
            && movey < 0)
            tryGoUnder();

        else if (Input.GetKey(KeyCode.Space)
            #if UNITY_STANDALONE_OSX
            || Input.GetKey(KeyCode.Joystick1Button16)
            #else
            || Input.GetKey(KeyCode.Joystick1Button0)
            #endif
            )
            tryjump();
        else if(movey < 0 && grounded)
        {
            move = 0;
            iscrouching = true;
        }
        anim.SetBool("iscrouching", iscrouching);

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)
            #if UNITY_STANDALONE_OSX
            || Input.GetKey(KeyCode.Joystick1Button18)
            #else
            || Input.GetKey(KeyCode.Joystick1Button2)
            #endif
            )
            StartCoroutine(Tapping());

		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftCommand)
            #if UNITY_STANDALONE_OSX
            || Input.GetKey(KeyCode.Joystick1Button13) || Input.GetKey(KeyCode.Joystick1Button14)
            #else
            || Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.Joystick1Button5)
            #endif
            )
            StartCoroutine(dash());
    }

    protected virtual void FixedUpdate()
    {
        PCFixedUpdate();
    }

    protected void PCFixedUpdate()
    {
        if (life <= 0)
            return;
        allCheck();

        if (base.cannotmove == true)
            return;

        Move(move, movey);
    }

    /*****************************************************************************************************************
                                                        COLLISION
    *****************************************************************************************************************/
    protected bool IsOnLadder = false;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Slime")
        {
            if (rigidbody2D.velocity.y < slimeVelocityIgnore)
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        }
        OnTriggerStay2D(other.collider);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        OnTriggerStay2D(collision.collider);
    }

    virtual protected void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "OS")
        {
            life = 0;
            Die();
            return;
        }

        if (canOuch && (other.tag == ouchtag || other.tag == "ouchbam"))
        {
            PlayerController oponnent = other.GetComponentInParent<PlayerController>();
            if (oponnent)
                oponnent.DoingDamage(transform.position);
            ouch(new Vector2(Mathf.Sign(transform.position.x - other.transform.position.x) * ouchJumpMultPushX, ouchJumpMultPushY));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + groundPosition, groundSize);
    }
}
