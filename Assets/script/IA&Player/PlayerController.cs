using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Character
{

    [Header("Player setting")]
    public bool resetCamOnPlayer = true;

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
        maxLife = (int)GameManager.instance.difficulty;
        if (GameManager.instance.life)
            GameManager.instance.life.text = maxLife.ToString();

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

        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)
            #if UNITY_STANDALONE_OSX
            || Input.GetKey(KeyCode.Joystick1Button0)
            #else
            || Input.GetKey(KeyCode.Joystick1Button0)
            #endif
            )
            && Mathf.Abs(move) < Mathf.Abs(movey) - 0.4f && movey < 0)
        {
            Debug.Log(movey);
            tryGoUnder();
        }

        else if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)
            #if UNITY_STANDALONE_OSX
            || Input.GetKey(KeyCode.Joystick1Button0)
            #else
            || Input.GetKey(KeyCode.Joystick1Button0)
            #endif
            )
            tryjump();
        else if(Mathf.Abs(move) < Mathf.Abs(movey) - 0.4f && movey < 0 && grounded)
        {
            move = 0;
            iscrouching = true;
        }
        anim.SetBool("iscrouching", iscrouching);

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)
            #if UNITY_STANDALONE_OSX
            || Input.GetKey(KeyCode.Joystick1Button2)
            #else
            || Input.GetKey(KeyCode.Joystick1Button2)
            #endif
            )
            StartCoroutine(Tapping());

		if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightShift)
            #if UNITY_STANDALONE_OSX
            || Input.GetKey(KeyCode.Joystick1Button6) || Input.GetKey(KeyCode.Joystick1Button7)
            #else
            || Input.GetKey(KeyCode.Joystick1Button6) || Input.GetKey(KeyCode.Joystick1Button7)
            #endif
            )
            StartCoroutine(dash());
    }

}
