using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
    public Rigidbody2D Projectile;
    public float angleprecision = 0;
    public float nbshootfor10s = 8;
    public float nbofshotbyburst = 1;

    public float deregulatorvalue = 0;
    public float delay = 1;
    public float impulsionForce = 10;
    Animator anim = null;
    bool willShoot = false;

    // Use this for initialization
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        delay = 10 / nbshootfor10s + Random.Range(-deregulatorvalue, deregulatorvalue);
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 0 && willShoot)
            delay -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            willShoot = true;
            anim.SetBool("willshoot", true);
        }
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (delay > 0 || collision.tag != "Player")
            return;

        delay = 10 / nbshootfor10s + Random.Range(-deregulatorvalue, deregulatorvalue);
        shoot(collision.transform.position);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            willShoot = true;
            anim.SetBool("willshoot", false);
            delay = 10 / nbshootfor10s + Random.Range(-deregulatorvalue, deregulatorvalue);
        }
    }
}