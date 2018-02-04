using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour 
{
    // Update is called once per frame
    void Update ()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
