using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI speedometerText;

    public void UpdateSpeedometer(float speed)
    {
        float speedMetric = speed * 3.6f; // Speed in km/h
        speedometerText.text = Mathf.Clamp(Mathf.Round(speedMetric), 0f, 100000f).ToString() + " km/h";
    }
}
