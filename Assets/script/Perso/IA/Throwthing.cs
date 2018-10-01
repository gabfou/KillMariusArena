using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwthing : MonoBehaviour {
    public Rigidbody2D Projectile;
    public float angleprecision = 0;
    public float nbofshotbyburst = 1;
    public float timetoshoot = 1;
    public float cd = 1;


    float delay = 0;
    public float impulsionForce = 10;
    Animator anim = null;
    bool willShoot = false;
    Agro agro = null;
    bool playerInSight = false;
    Transform cible = null;

    // Use this for initialization
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        delay = timetoshoot;
        agro = GetComponentInParent<Agro>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((delay > 0 && willShoot) || delay > timetoshoot)
            delay -= Time.deltaTime;
        if (agro && delay < timetoshoot && (agro.grounded == false || agro.move != 0))
            delay = timetoshoot;

        if (!playerInSight)
            return;

        if ( willShoot == false && delay <= timetoshoot)
        {
            willShoot = true;
            anim.SetBool("willshoot", true);
        }

        if (delay > 0)
            return;

        willShoot = false;
        anim.SetBool("willshoot", false);
        delay = cd + timetoshoot;
        shoot(cible.position);
    }

    public void shoot(Vector2 cible)
    {
        anim.SetTrigger("shooting");
        for (int i = 0; i < nbofshotbyburst; i++)
        {
            Rigidbody2D lastmp = GameObject.Instantiate(Projectile, transform.position, Quaternion.identity);
            lastmp.transform.right = (cible - (Vector2)transform.position).normalized;

            var rotation = transform.rotation;
            transform.Rotate(transform.forward, Random.Range(-angleprecision / 2, angleprecision / 2));
            transform.rotation = rotation;

            lastmp.AddForce(lastmp.transform.right * impulsionForce, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInSight = true;
            cible = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInSight = false;
            willShoot = false;
            anim.SetBool("willshoot", false);
            delay = cd + timetoshoot;
            cible = null;
        }
    }
}
