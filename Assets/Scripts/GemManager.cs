using UnityEngine;
using UnityEngine.UI;
public class GemManager : MonoBehaviour
{
    public int gemCount;
    public Text gemText;

    private void Start()
    {
        UpdateGemUI();
    }

    public void AddGem(int amount = 1)
    {
        gemCount += amount;
        UpdateGemUI();
    }

    private void UpdateGemUI()
    {
        gemText.text = ":" + gemCount.ToString();
    }
}
