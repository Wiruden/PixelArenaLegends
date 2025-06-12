using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject healthBarPrefab;

    private Image fillImage;
    private GameObject healthBarInstance;

    private void Start()
    {
        if (healthBarPrefab == null)
        {
            return;
        }

        healthBarInstance = Instantiate(healthBarPrefab, transform);

        // Position above enemy
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        float height = capsule != null ? capsule.size.y : 2f;
        healthBarInstance.transform.localPosition = new Vector3(0, height + 0.3f, 0);

        // Find all Images in children and pick the one named "Fill"
        Image[] images = healthBarInstance.GetComponentsInChildren<Image>();
        foreach (var img in images)
        {
            if (img.name == "HealthBarFill")
            {
                fillImage = img;
                break;
            }
        }

        if (fillImage == null)
        {
            Debug.LogError("Fill image not found in health bar prefab children.");
        }
    }

    public void UpdateHealthBar(float current, float max)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = current / max;
        }
    }
}