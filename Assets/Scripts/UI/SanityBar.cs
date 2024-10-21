using UnityEngine;
using UnityEngine.UI;

public class SanityBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill; 

    public void SetValue(int sanity)
    {   
        slider.value = sanity;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
