using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerController : Character
{

    [Header("Player setting")]
    public bool resetCamOnPlayer = true;

    GamePref pref;

    override protected IEnumerator WaitForCam()
    {
        yield return StartCoroutine(base.WaitForCam());

        if (resetCamOnPlayer)
        {
            Transform transform2;
            if ((transform2 = ((transform.parent) ?? transform)) != vcam.Follow)
                vcam.Follow = transform2;
            if ((transform2 = ((transform.parent) ?? transform)) != vcam.LookAt)
                vcam.LookAt = transform2;
            float blend = Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time;
            Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = 0f;
            yield return null;
            Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = blend;
        }
    }

    private void PlayerSpecificReinit()
    {
        if(!GameManager.instance)
        {
            Debug.LogError("No GameManager Instance In PlayerSpecificReinit");
            return ;
        }

        if (GameManager.instance.save.replaceBy != null && GameManager.instance.replacedPlayer == false)
        {
            GameManager.instance.replacedPlayer = true;
            GameObject.Instantiate(GameManager.instance.save.replaceBy);
            GameObject.Destroy(gameObject);
            return ;
        }
		if (GameManager.instance.save.parent)
			transform.parent = GameManager.instance.save.parent.transform;
        GameManager.instance.player = this;
     
        if (GameManager.instance.save.lastCheckpoint != Vector2.zero && GameManager.instance?.playerSpawned == false)
            transform.position = GameManager.instance.save.lastCheckpoint;
        if (GameManager.instance.save.camToActive != null)
            GameManager.instance.save.camToActive.SetActive(true);
        GameManager.instance.playerSpawned = true;
        maxLife = (int)GameManager.instance.save.difficulty;
        if (GameManager.instance.life)
            GameManager.instance.life.text = maxLife.ToString();
        pref = GameManager.instance.pref;

    }

    override protected void reinit()
    {
        isPlayer = true;
        PlayerSpecificReinit();
        base.reinit();
    }





    override protected IEnumerator    waitbefordying()
    {
        yield return new WaitForSeconds(1);
        GameManager.instance.save.lastCheckpointReinit();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    override protected void    StopTapping()
    {
        StopCoroutine(Tapping());
        anim.SetBool("istapping", false);
        istapping = false;
    }

    override public void ouch(Vector2 impact2)
    {
        base.ouch(impact2);
        if (GameManager.instance?.life)
            GameManager.instance.life.text = life.ToString();
    }

    IEnumerator Tapping()
    {
        if (istapping == false)
        {
            float y = transform.localScale.y;
            if (flying && movey < 0)
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            if (TappingClip)
                audiosource.PlayOneShot(TappingClip, tappingVolume);
            istapping = true;
            anim.SetBool("istapping", true);
            yield return new WaitForSeconds(0.3f);
            anim.SetBool("istapping", false);
            yield return new WaitForSeconds(0.05f);
            StopTapping();
            if (flying)
                transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
        }
    }


    override protected void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "OSPlayer")
        {
            life = 0;
            Die();
            return;
        }
        base.OnTriggerStay2D(other);
    }

    int nbKilled = 0;
    Coroutine MedalBamCheckTimerCurrent = null;

    IEnumerator MedalBamCheckTimer()
    {
        yield return new WaitForSeconds(0.2f);
        nbKilled = 0;
    }

    public override void DoingDamage(Character character)
    {
        if (character.life < 1)
        {
            nbKilled++;
            if (MedalBamCheckTimerCurrent != null)
                StopCoroutine(MedalBamCheckTimerCurrent);
            MedalBamCheckTimerCurrent = StartCoroutine(MedalBamCheckTimer());
        }
        if (nbKilled >= 3)
            GameManager.instance.medalManager.TryToUnlockMedal("BAM");
        if (nbKilled >= 5)
            GameManager.instance.medalManager.TryToUnlockMedal("BOOM!!!");
        if (nbKilled >= 10)
            GameManager.instance.medalManager.TryToUnlockMedal("BLAM!!!!!!");
            
    }

    [HideInInspector] public bool canControle = false;
    void Update()
    {
        if (life < 0)
            return;
        if (base.cannotmove == true || canControle == true)
            return;
        move = Input.GetAxisRaw("Horizontal");
        movey = Input.GetAxisRaw("Vertical");
        bool iscrouching = false;

        if ((Input.GetKey(pref.jump1) || Input.GetKey(pref.jump2))
            && Mathf.Abs(move) < Mathf.Abs(movey) - 0.4f && movey < 0)
            tryGoUnder();

        else if (Input.GetKey(pref.jump1) || Input.GetKey(pref.jump2))
            tryjump();

        else if(Mathf.Abs(move) < Mathf.Abs(movey) - 0.4f && movey < 0 && grounded)
        {
            move = 0;
            iscrouching = true;
        }
        anim.SetBool("iscrouching", iscrouching);

        if (Input.GetKey(pref.attack1) || Input.GetKey(pref.attack2))
            StartCoroutine(Tapping());

		if (dashEnabled && (Input.GetKey(pref.dash1) || Input.GetKey(pref.dash2)))
            StartCoroutine(dash());
    }

}
