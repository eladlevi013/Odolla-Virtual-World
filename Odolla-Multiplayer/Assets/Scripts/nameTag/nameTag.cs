using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class nameTag : MonoBehaviourPun
{
    public GameObject name;

    // Start is called before the first frame update
    void Start()
    {
        name.GetComponent<TextMeshPro>().text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
