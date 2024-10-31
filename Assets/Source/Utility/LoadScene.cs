using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private int sceneIndex;

    public void LoadAtIndex()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
