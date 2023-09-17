using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // That I would be able to call the coroutine functions on WEB from another script.
    // like that: StartCoroutine(Main.Instance.Web.GetCurrOutfit());

    public static Main Instance;
    public FirebaseHelper FirebaseHelper;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        FirebaseHelper = GetComponent<FirebaseHelper>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
