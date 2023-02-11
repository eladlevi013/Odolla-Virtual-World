using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onStoreClick : MonoBehaviour
{
    public GameObject script;

    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        if(OnSceneSpawn.isFocus == false && OnSceneSpawn.isMoving == false)
            script.GetComponent<Store>().onStoreClick();
    }

    void Update()
    {
        
    }
}
