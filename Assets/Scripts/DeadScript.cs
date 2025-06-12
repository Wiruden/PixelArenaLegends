using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeadScript : MonoBehaviour
{
    public float delayBeforeLoad = 5f;

    void Start()
    {
        StartCoroutine(LoadMenuAfterDelay());
    }

    private IEnumerator LoadMenuAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene("Menu");
    }
}
