using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4Special : MonoBehaviour
{
	Agro agro;
	Rigidbody2D rb;
	Animator animator;
	bool isInSpecial = false;

	[Header("General")]
	public float timeBetweenTwoAction = 5;
	public float addRandomTime = 2;

	[Header("Tourbilol")]
	public float distMin = 5;
	public float speed = 20;
	public int nbRedirige = 0;
	public float timeToStop = 1;
	public float impulseOfFlee = 30;
	public float timeOfFlee = 0.8f;
	public GameObject colliderTourbilol;

	[Header("PIOUPIOUPIOU")]
	public float speedOfJump = 40;
	public float timeOfBackFlip = 0.1f;
	public List<Vector2> posofJump = new List<Vector2>();
	public float TimeOfShoot = 2;
	public GameObject throwThing;

	
	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
		agro = GetComponent<Agro>();
		rb = GetComponent<Rigidbody2D>();
		time = timeBetweenTwoAction + Random.Range(0, addRandomTime);
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
		rb.AddForce(new Vector2(Mathf.Sign(transform.position.x - agro.Cible.position.x), 0.5f).normalized * impulseOfFlee, ForceMode2D.Impulse);
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
				Debug.Log("dasd");
		isInSpecial = true;
		agro.cannotmove = true;
		gameObject.layer = 19;
		rb.gravityScale = 0;

		Quaternion rotateO = transform.rotation;
		while (Vector2.Distance(transform.position, posofJump[0]) > 0.5f)
		{
			rb.velocity = (posofJump[0] - (Vector2)transform.position).normalized * speedOfJump;
			// transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + (360f / timeOfBackFlip) * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
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
		rb.gravityScale = agro.baseGravityScale;
	}


	float time;

	// Update is called once per frame
	void Update()
	{
		if (isInSpecial == false)
			time -= Time.deltaTime;
		if (time < 0)
		{
			int r = (Random.Range(0, 5));
			if (r < 4)
				StartCoroutine(FleeThenTourbilol());
			else
				StartCoroutine(Pioupiou());
			time = timeBetweenTwoAction + Random.Range(0, addRandomTime);
		}
		if (!isInSpecial && rb.gravityScale != agro.baseGravityScale)
			rb.gravityScale = agro.baseGravityScale;
	}
}
