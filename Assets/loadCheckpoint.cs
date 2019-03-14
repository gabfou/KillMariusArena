using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadCheckpoint : PrepNextLevel
{
    // Start is called before the first frame update
    public void load()
    {
        GameManager.instance.save.load();
    }

}
