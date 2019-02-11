using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PrepNextLevel : MonoBehaviour
{
    public string nextSceneName;
    AsyncOperation ao = null;



    public ActiveScene()
    {
        if (ao != null)
            ao.allowSceneActivation = true;
    }

    public PreLoadScene()
    {
        if (ao == null)
        {
        	ao = SceneManager.LoadSceneAsync(nextSceneName);
    		ao.allowSceneActivation = false;
        }
    }
}
