using TMPro;
using UnityEngine;

[RequireComponent(typeof(FadeOut))]
[RequireComponent(typeof(TextMeshProUGUI))]
public class DamageDisplay : MonoBehaviour
{
    public TextMeshProUGUI Display;
    public FadeOut Fader;

    public void Activate(float label)
    {
        if (label < 0)
            Display.color = GamePreferences.Current.ColorAndTheme.HealthChangeMarkerDamageColor;
        else 
            Display.color = GamePreferences.Current.ColorAndTheme.HealthChangeMarkerHealColor;

        Display.text = Mathf.Abs(label).ToString();
        Fader.StartFade();
    }
}
