using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PrepNextLevel : MonoBehaviour
{
    public string nextSceneName;
    AsyncOperation ao = null;

    public virtual void stopLoad(Scene scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }


    public void ActiveScene(bool levelChangeReinit)
    {
        if (ao != null)
        {
            ao.allowSceneActivation = true;
            Application.backgroundLoadingPriority = ThreadPriority.High; // HAAAAAAAAAA LOAD THE SCENE WE ARE LATE
        }
        else
            SceneManager.LoadScene(nextSceneName);
        if (levelChangeReinit)
            GameManager.instance.save.levelChangeReinit(nextSceneName);
        else
            GameManager.instance.save.lastCheckpointReinit();
    }
    public void ActiveScene()
    {
        ActiveScene(true);
    }

    public void PreLoadScene()
    {
        if (ao == null)
        {
            Application.backgroundLoadingPriority = ThreadPriority.Low;
        	ao = SceneManager.LoadSceneAsync(nextSceneName);
    		ao.allowSceneActivation = false;
        }
    }

}
