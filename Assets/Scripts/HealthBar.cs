using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Image circleBar;
    public Image extraBar;

    public float currentHealth = 0;
    public float maxHealth = 100;
    public float circlePercentage = 0.3f;
    public const float circleFillAmount = 0.75f;


    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        CircleFill();
        ExtraFill();
    }

    void CircleFill()
    {
        float healthPercentage = currentHealth / maxHealth;
        float circleFill = healthPercentage / circlePercentage;
        circleFill *= circleFillAmount;
        circleBar.fillAmount = healthPercentage;
    }

    void ExtraFill()
    {
        float circleAmount = circlePercentage * maxHealth;

        float extraHealth = currentHealth - circleAmount;
        float extraTotalHealth = maxHealth - circleAmount;

        float extraFill= extraHealth/extraTotalHealth;
        extraBar.fillAmount = extraFill;
    }
}
