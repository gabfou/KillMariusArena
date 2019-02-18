using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PrepNextLevel : MonoBehaviour
{
    public string nextSceneName;
    AsyncOperation ao = null;



    public void ActiveScene()
    {
        if (ao != null)
            ao.allowSceneActivation = true;
        else
            SceneManager.LoadScene(nextSceneName);
    }

    public void PreLoadScene()
    {
        if (ao == null)
        {
        	ao = SceneManager.LoadSceneAsync(nextSceneName);
    		ao.allowSceneActivation = false;
        }
    }
}
