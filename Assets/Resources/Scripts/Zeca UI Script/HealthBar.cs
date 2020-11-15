using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Image circleBar;
    public Image extraBar;

    public float _currentHealth = 0;
    public float _maxHealth = 100;
    public float _circlePercentage = 0.75f;
    public const float _circleFillAmount = 0.75f;


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
        float healthPercentage = _currentHealth / _maxHealth;
        float circleFill = healthPercentage / _circlePercentage;
        circleFill *= _circleFillAmount;
        circleBar.fillAmount = healthPercentage;
    }

    void ExtraFill()
    {
        float circleAmount = _circlePercentage * _maxHealth;

        float extraHealth = _currentHealth - circleAmount;
        float extraTotalHealth = _maxHealth - circleAmount;

        float extraFill= extraHealth/extraTotalHealth;
        extraBar.fillAmount = extraFill;
    }
}
