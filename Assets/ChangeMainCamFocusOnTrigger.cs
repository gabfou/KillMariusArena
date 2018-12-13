using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeMainCamFocusOnTrigger : MonoBehaviour
{
    public Transform follow = null;
    public Transform lookAt = null;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
            return ;
        CinemachineVirtualCamera vcam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        vcam.Follow = follow;
        vcam.LookAt = lookAt;
        GameObject.Destroy(this);
    }
}
