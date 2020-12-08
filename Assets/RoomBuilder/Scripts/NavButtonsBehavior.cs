using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavButtonsBehavior : MonoBehaviour
{
    public Scene Scene;
    // Start is called before the first frame update
    public void OnNavButtonClick()
    {
        SceneManager.LoadScene(Scene.handle);
    }
}
