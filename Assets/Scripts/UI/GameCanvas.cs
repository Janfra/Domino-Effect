using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    public static GameCanvas instance { get; private set; }

    private void Awake()
    {
        if (instance != null) Destroy(this);

        instance = this;
        DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
