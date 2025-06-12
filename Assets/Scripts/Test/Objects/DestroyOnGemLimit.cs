using UnityEngine;

public class DestroyOnGemLimit : MonoBehaviour
{
    [SerializeField] private int gemLimit = 10;
    [SerializeField] private GemManager gemManager;
    private void Update()
    {
        if (gemManager != null && gemManager.gemCount >= gemLimit)
        {
            Destroy(gameObject);
        }
    }
}