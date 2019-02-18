using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeMainCamFocusOnTriggerStay : MonoBehaviour
{
    public Transform follow = null;
    public Transform lookAt = null;
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D other)
    {
        CinemachineVirtualCamera vcam;
        if (other.tag != "Player" || (vcam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera) == null)
            return ;
        vcam.Follow = follow;
        vcam.LookAt = lookAt;
        // GameObject.Destroy(this);
    }
}
