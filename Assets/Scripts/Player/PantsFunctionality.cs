using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PantsFunctionality : MonoBehaviour
{
    public RuntimeAnimatorController pants_controller;

    void Start()
    {
    }

    void Update()
    {
    }

    public void playAnime()
    {
        gameObject.AddComponent<Animator>();
        GetComponent<Animator>().runtimeAnimatorController = pants_controller;

        if (GetComponent<SpriteRenderer>().sprite.name != "empty" && GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().Play("Base Layer." + GetComponent<SpriteRenderer>().sprite.name, 0, 0f);
        }
    }

    public void stop()
    {
        Destroy(GetComponent<Animator>());

        if (GetComponent<Animator>() != null)
            GetComponent<Animator>().Rebind();
    }
}
