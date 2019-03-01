using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4Special : MonoBehaviour
{
	Agro agro;
	Rigidbody2D rb;
	Animator animator;
	bool isInSpecial = false;
	float lifeLast;

	[Header("General")]
	public float timeBetweenTwoAction = 5;
	public float addRandomTime = 2;
	public int	lifeLostForceAction = 3;

	[Header("Tourbilol")]
	public float distMin = 5;
	public float speed = 20;
	public int nbRedirige = 0;
	public float timeToStop = 1;
	public float impulseOfFlee = 30;
	public float timeOfFlee = 0.8f;
	public GameObject colliderTourbilol;

	[Header("PIOUPIOUPIOU")]
	public float timeOfBackJumpInSec = 0.5f;
	public float timeOfBackFlip = 0.1f;
	public List<Vector2> posofJump = new List<Vector2>();
	public float TimeOfShoot = 2;
	public GameObject throwThing;
	public float baseStunOuch;


	
	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
		agro = GetComponent<Agro>();
		rb = GetComponent<Rigidbody2D>();
		time = timeBetweenTwoAction + Random.Range(0, addRandomTime);
		lifeLast = agro.life;
		baseStunOuch = agro.timestunouch;
	}

	IEnumerator TourbilolToutDroit()
	{
		if (Vector2.Distance(agro.Cible.position, transform.position) > distMin)
		{
			isInSpecial = true;	
			agro.cannotmove = true;
			agro.istapping = true;
			colliderTourbilol.SetActive(true);
			rb.gravityScale = 0;
			gameObject.layer = 13;
			animator.SetBool("tourbilol", true);
			int nbRedirigeleft = nbRedirige;

			while (nbRedirigeleft >= 0)
			{
				Vector2 velocity = (agro.Cible.position - transform.position).normalized * speed;
				rb.velocity = velocity;
				while (!(Vector2.Distance((Vector2)transform.position + rb.velocity.normalized, agro.Cible.position) > Vector2.Distance(transform.position, agro.Cible.position)))
				{
					rb.velocity = velocity;
					yield return new WaitForEndOfFrame();
				}

				nbRedirigeleft--;
				float time = timeToStop;
				while (time > 0)
				{
					rb.velocity = velocity;
					time -= Time.deltaTime;
					yield return new WaitForEndOfFrame();
				}
			}
			agro.cannotmove = false;
			rb.gravityScale = agro.baseGravityScale;
			animator.SetBool("tourbilol", false);
			gameObject.layer = agro.baseLayer;
			colliderTourbilol.SetActive(false);
			isInSpecial = false;
		}
		agro.istapping = false;
	}

	IEnumerator FleeThenTourbilol()
	{
		isInSpecial = true;
		agro.cannotmove = true;
		agro.istapping = true;
		rb.AddForce(new Vector2(Mathf.Sign(transform.position.x - agro.Cible.position.x), 0.6f).normalized * impulseOfFlee, ForceMode2D.Impulse);
		float time = timeOfFlee;
		while (time > 0)
		{
			time -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		agro.cannotmove = false;
		isInSpecial = false;
		StartCoroutine(TourbilolToutDroit());
	}

	IEnumerator Pioupiou()
	{
		isInSpecial = true;
		agro.cannotmove = true;
		gameObject.layer = 19;
		rb.bodyType = RigidbodyType2D.Kinematic;
		float	speed = Vector2.Distance(posofJump[0], (Vector2)transform.position) / timeOfBackJumpInSec;
		int r = Random.Range(0, posofJump.Count);

		Quaternion rotateO = transform.rotation;
		while (Vector2.Distance(transform.position, posofJump[r]) > speed * Time.fixedDeltaTime * 2)
		{
			// rb.velocity = (posofJump[0] - (Vector2)transform.position).normalized * speedOfJump;
			// transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + (360f / timeOfBackFlip) * Time.deltaTime);
			rb.MovePosition(Vector2.MoveTowards(transform.position, posofJump[r], speed * Time.fixedDeltaTime));

			yield return new WaitForFixedUpdate();
		}
		rb.bodyType = RigidbodyType2D.Dynamic;
		transform.rotation = rotateO;
		gameObject.layer = agro.baseLayer;
		rb.velocity = Vector2.zero;
		float time = TimeOfShoot;
		throwThing.SetActive(true);
		while (time > 0)
		{
			agro.cannotmove = true;
			rb.velocity = Vector2.zero;
			time -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		throwThing.SetActive(false);
		agro.cannotmove = false;
		isInSpecial = false;
	}

	IEnumerator BerzerkCoroutine(float time)
	{
		transform.localScale *= 1.2f;
		agro.timestunouch = 0.01f;
		yield return new WaitForSeconds(time);
		agro.timestunouch = 0.01f;
		transform.localScale /= 1.2f;
	}

	public void Berzerk(float time)
	{
		StartCoroutine(BerzerkCoroutine(time));
	}


	float time;

	// Update is called once per frame
	void Update()
	{
		if (agro.Cible == null)
			return;
		if (isInSpecial == false)
			time -= Time.deltaTime;
		if (time < 0 || lifeLast - agro.life >= lifeLostForceAction)
		{
			lifeLast = agro.life;
			int r = (Random.Range(0, 100));
			if (r < 75)
				StartCoroutine(FleeThenTourbilol());
			else
				StartCoroutine(Pioupiou());
			time = timeBetweenTwoAction + Random.Range(0, addRandomTime);
		}
		if (!isInSpecial && rb.gravityScale != agro.baseGravityScale)
			rb.gravityScale = agro.baseGravityScale;
	}
}
