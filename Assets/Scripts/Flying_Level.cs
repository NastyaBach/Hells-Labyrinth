using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flying_Level : MonoBehaviour
{
    private int sceneNumber;
    private TextMesh textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMesh>();
        sceneNumber = SceneManager.GetActiveScene().buildIndex;
        textMesh.text = "Уровень " + sceneNumber;
    }

    public void OnAnimationOver()
    {
        Destroy(gameObject);
    }
}
