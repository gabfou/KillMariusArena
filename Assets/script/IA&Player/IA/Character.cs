using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

// TO DO: change float move and float movey by Vector2 move
// NEXT GAME TO DO laisser seulement ce qui concerne le player ici et le faire heriter

public class Character : Stopmoving
{

    [Header("Basic setting")]
    public bool facingLeft = false;
    public int life = 5;
    [HideInInspector] public int maxLife;
    public float invulnTime = 1f;
    public bool dontDestroy = false;

    [Header("Mobility and groundaison")]
    public float maxSpeed = 1f;
    public float minSlideVelocity = 3f;
    public float slimeVelocityIgnore = .5f;
    public float jumpPower = 10f;
    public float jumpIdle = .0f;
    public Vector3 groundPosition;
    public Vector2 groundSize;
    public float maxYVelocity = 8f;
    public float minYVelocity = -6f;
    public bool flying = false;

    [Header("Dash")]
    public bool dashEnabled = false;
    [ConditionalHide("dashEnabled", true)] public float timedashinsc = 0.1f;
	[ConditionalHide("dashEnabled", true)] public float distanceofdash = 10f;
	[ConditionalHide("dashEnabled", true)] public bool isinvuindash = true;
	[ConditionalHide("dashEnabled", true)] public float dashcd = 0.5f;

    [Header("Sound")]
    public AudioClip ouchClip;
    [Range(0, 1)] public float ouchVolume = 0.8f;
    public AudioClip jumpClip;
    [Range(0, 1)] public float jumpingVolume = 0.5f;
    public AudioClip TappingClip;
    [Range(0, 1)] public float tappingVolume = 0.5f;
    public AudioClip DieClip;
    [Range(0, 1)] public float DieVolume = 0.5f;
    public AudioClip dashClip;
    [Range(0, 1)] public float dashVolume = 0.5f;    
    [Header("Ouch")]
    public string ouchtag = "ouch";
    public float timeStunouchBrillance = 0.2f;
    public bool stunStopMove = true;
    public bool stunStopAttack = true;
    public float ouchJumpMultPushX = 2;
    public float ouchJumpMultPushY = 4;

    [HideInInspector] public bool canJump = true;
    [HideInInspector] public bool istapping = false;
    [HideInInspector] public bool grounded;

    [HideInInspector] public float move = 0;
    [HideInInspector] public float movey = 0;

    [Header("Events")]
    public UnityEvent   onTakeDamage;
    public UnityEvent   onDie;

    Vector2 impacto = Vector2.zero;
    [HideInInspector] public CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin vcamperlin;
    Material spriteMaterial;
    protected bool isPlayer = false;
    bool canOuch = true;
    bool sliding = false;
    new protected Rigidbody2D rigidbody2D;
    protected SpriteRenderer spriteRenderer;
    protected Animator anim;
    [HideInInspector] public AudioSource audiosource;
    [HideInInspector] public bool TakingDamage = false;
    [HideInInspector] public bool IsOuchstun = false;
    [HideInInspector] public float baseGravityScale;

    protected Collider2D  col;
    [HideInInspector] public bool isDead;
    [HideInInspector] public int baseLayer;
    [HideInInspector] public Rigidbody2D rbparent = null;

    protected LayerMask groundLayer;


    /*****************************************************************************************************************
                                                        INITIALISATION
    *****************************************************************************************************************/

    virtual protected IEnumerator WaitForCam()
    {
        while (!vcam)
        {
            vcam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
            yield return new WaitForEndOfFrame();
        }

    }

    protected virtual void  reinit()
    {
        life = maxLife;
        isdashing = false;
        anim.SetBool("grounded", grounded);
		candash = true;
        if (rigidbody2D)
            rigidbody2D.gravityScale = baseGravityScale;
        gameObject.layer = baseLayer;
        
        IsOuchstun = false;
        isDead = false;
 
        StartCoroutine(WaitForCam());
        CaBouge cbtmp = GetComponentInParent<CaBouge>();
        if (cbtmp)
            rbparent = cbtmp.GetComponent<Rigidbody2D>();
    }

    protected void init()
    {
        maxLife = life;
        groundLayer = 1 << (LayerMask.NameToLayer("Ground")) | 1 << (LayerMask.NameToLayer("GroundOneWay"));
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteMaterial = spriteRenderer.material;
        rigidbody2D = GetComponent<Rigidbody2D>();
        baseLayer = gameObject.layer;
        anim = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        col = GetComponents<Collider2D>().Where(c => !c.isTrigger).FirstOrDefault();
        if (rigidbody2D)
            baseGravityScale = rigidbody2D.gravityScale;
        reinit();
    }

    void Start()
    {
        init();
    }


    /*****************************************************************************************************************
                                                        PAS RANGÉ
    *****************************************************************************************************************/

    float DistToPlayer()
    {
        return Vector2.Distance(GameManager.instance.player.transform.position, transform.position);
    }

    void    StopConglomération()
    {
        impacto = Vector2.zero;
        if (move != 0 && Physics2D.OverlapCircleNonAlloc(transform.position, 1.4f, ctmp, 1 << gameObject.layer) > 1)
        {
            Collider2D col;
            if (col = ctmp.FirstOrDefault(c => c && !c.transform.IsChildOf(transform) && c.gameObject != gameObject && !c.isTrigger))
            {
                Vector2 v = (transform.position - col.transform.position).normalized;
                move += v.x * 0.3f;
                if (!flying)
                    movey += v.y * 0.3f;
            }
            
        }
    }

    protected void allCheck()
    {
        if (rigidbody2D && Mathf.Approximately(rigidbody2D.velocity.magnitude, 0))
            return ;
        if (Time.frameCount % 4 == 0)
            StopConglomération();
        if (rigidbody2D)
             GroundCheck();
         SlideCheck();
    }

    public virtual void DoingDamage(Character character)
    {

    }

    virtual protected void    StopTapping()
    {
    }

	bool isdashing = false;
    Vector2 oldV = Vector2.zero;
    Vector2 currentV = Vector2.zero;


    public void Move(float move, float movey = 0)
    {
        Vector2 newVCible = Vector2.zero;
		if (isdashing || rigidbody2D == null || rigidbody2D.bodyType == RigidbodyType2D.Static)
			return ;

        newVCible.x = Mathf.Clamp(move * maxSpeed, -maxSpeed, maxSpeed);
        newVCible.y = Mathf.Clamp((IsOnLadder) ? movey * maxSpeed : rigidbody2D.velocity.y, minYVelocity, maxYVelocity);

        if ((!istapping || isPlayer) && move > 0 && facingLeft)
            Flip();
        else if ((!istapping || isPlayer) && move < 0 && !facingLeft)
            Flip();
        
        rigidbody2D.velocity = (flying) ? Vector2.SmoothDamp(oldV, newVCible, ref currentV, 0.2f) : newVCible;
        oldV = rigidbody2D.velocity;

        anim.SetBool("ismoving", move != 0 || (IsOnLadder && movey != 0));
        anim.SetFloat("velx", rigidbody2D.velocity.x);
        anim.SetFloat("vely", rigidbody2D.velocity.y);

        if (rbparent)
            rigidbody2D.velocity += new Vector2(rbparent.velocity.x, (IsOnLadder) ? rbparent.velocity.y : 0);
        rigidbody2D.velocity += impacto;
    }

    Collider2D actualGround;
    RaycastHit2D[] results = new RaycastHit2D[10];
    protected virtual void GroundCheck()
    {
        if (flying)
        {
            IsOnLadder = true;
            return ;
        }
        if (groundPosition == Vector3.zero && groundSize == Vector2.zero)
        {
            
            grounded = true;
            return ;
        }

        if (rigidbody2D.IsSleeping())
            return ;
        
        int collisionNumber = Physics2D.BoxCastNonAlloc(transform.position + groundPosition, groundSize, 0, Vector2.down, results, .0f, groundLayer);
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
        // float arx = Mathf.Abs(rigidbody2D.velocity.x);
        // if (arx > minSlideVelocity && Input.GetKeyDown(KeyCode.DownArrow))
        //     sliding = true;
        // if (arx < minSlideVelocity || !grounded)
        //     sliding = false;

        // anim.SetBool("sliding", sliding);
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

    protected virtual IEnumerator    waitbefordying()
    {
        yield return new WaitForSeconds(1);
        while (coroutineisplayingcount > 0)
            yield return new WaitForEndOfFrame();
        if (dontDestroy == false)
            GameObject.Destroy(gameObject);
    }

    protected virtual void Die()
    {
        if (audiosource && DieClip && DistToPlayer() < GameManager.instance.DistanceOfSound)
            audiosource.PlayOneShot(DieClip, DieVolume);
        isDead = true;
        IsOuchstun = true;
        StopTapping();
        gameObject.layer = LayerMask.NameToLayer("TouchNothing");
        if (rigidbody2D)
            rigidbody2D.gravityScale = 0;
        cannotmove = true;
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
        onDie.Invoke();
    }

    public virtual void ouch(Vector2 impact2)
    {
        onTakeDamage.Invoke();
        if (stunStopMove && rigidbody2D && rigidbody2D.bodyType != RigidbodyType2D.Static)
            rigidbody2D.velocity = Vector2.zero;
        canOuch = false;
        life--;
        StopTapping();
        if (impact2.x > 0 && !facingLeft)
            Flip();
        else if (impact2.x < 0 && facingLeft)
            Flip();
        if (audiosource && ouchClip && DistToPlayer() < GameManager.instance.DistanceOfSound)
            audiosource.PlayOneShot(ouchClip, ouchVolume);
        if (life < 1)
        {
            eventOnDie();
            Die();
        }
        else
        {

            StartCoroutine(ResetCanOuch());
            // if (stun)
            StartCoroutine(stunOuchAndFlashing(impact2));
            StartCoroutine(impactoEffect());
        }
    }

    IEnumerator impactoEffect()
    {
		coroutineisplayingcount++;
        TakingDamage = true;
        vcamperlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        vcamperlin.m_AmplitudeGain = 0.35f;
        vcamperlin.m_FrequencyGain = 30;
        yield return new WaitForSeconds(0.1f);
        TakingDamage = false;
        yield return new WaitForSeconds(0.1f);
        vcamperlin.m_AmplitudeGain = 0;
        coroutineisplayingcount--;
    }

	public void BecomeStunned()
	{
		anim.SetBool("ouch", true);
    	IsOuchstun = true;
	}

	public void UnStun()
	{
		IsOuchstun = false;
        anim.SetBool("ouch", false);
	}

    IEnumerator stunOuchAndFlashing(Vector2 impact)
    {
		coroutineisplayingcount++;
		if (stunStopAttack)
			BecomeStunned();
        if (stunStopMove)
            cannotmove = true;
        if (rigidbody2D)
            rigidbody2D.velocity = impact;
        spriteMaterial.SetFloat("_isflashing", 1);
        yield return new WaitForSeconds(timeStunouchBrillance);
        spriteMaterial.SetFloat("_isflashing", 0);
        if (isDead == false)
            cannotmove = false;
		if (stunStopAttack)
			UnStun();
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
        if (grounded && canJump && IsOnLadder == false)
        {
            if (jumpClip && audiosource)
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
        rigidbody2D.AddForce(new Vector2(0, -10), ForceMode2D.Impulse);
        yield return new WaitForSeconds(1);
        Physics2D.IgnoreCollision(col, col2, false);
    }

    public void tryGoUnder()
    {
        if (actualGround && actualGround.usedByEffector == true && actualGround.GetComponent<PlatformEffector2D>() != null)
            StartCoroutine(tryGoUnderCo());
    }

    /*****************************************************************************************************************
														DASH
    *******************************************************************************************************************/

	bool candash = true;

	protected IEnumerator dash()
	{
		coroutineisplayingcount++;
		if (move == 0 && (!flying || movey == 0))
			Debug.Log(gameObject.name + " trying to dodge without moving");
		else if (candash == true)
		{
			candash = false;
			anim.SetBool("isdashing", true);
            if (isinvuindash == false)
                gameObject.layer = 12;
            else
                gameObject.layer = 13;
            if (audiosource && dashClip)
                audiosource.PlayOneShot(dashClip, dashVolume);

               
			float timer = 0;
			float movebysecond = distanceofdash / timedashinsc;
			Vector2 sign = new Vector2(Utils.SignOr0(move), (flying) ? Utils.SignOr0(movey) : 0).normalized;
            isdashing = true;
			while (timer < timedashinsc)
			{
                rigidbody2D.velocity = sign * movebysecond * Time.fixedDeltaTime;
				timer += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
            isdashing = false;
            rigidbody2D.velocity = new Vector2(0, 0);
            if (isinvuindash == false)
                gameObject.layer = baseLayer;
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

        Collider2D[] ctmp = new Collider2D[52];

    protected void PCFixedUpdate()
    {
        if (life <= 0)
            return;

        allCheck();

        if (base.cannotmove == true)
            return;

        Move(move, movey);
    }

    protected virtual void FixedUpdate()
    {
        PCFixedUpdate();
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
        OnTriggerEnter2D(other.collider);
    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "OS")
        {
            life = 0;
            Die();
            return;
        }

        if (canOuch && (other.tag == ouchtag || other.tag == "ouchbam"))
        {
            Character oponnent = other.GetComponentInParent<Character>();
            ouch(new Vector2(Mathf.Sign(transform.position.x - other.transform.position.x) * ouchJumpMultPushX, ouchJumpMultPushY));
            if (oponnent)
                oponnent.DoingDamage(this);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + groundPosition, groundSize);
    }
}
