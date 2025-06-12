using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float destructTime;
    private float timer;

    void Start()
    {
        timer = destructTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0 )
        {
            Destroy(gameObject);
        }
    }
}
