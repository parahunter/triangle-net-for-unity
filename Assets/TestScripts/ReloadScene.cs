using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReloadScene : MonoBehaviour 
{	
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

	}
}
