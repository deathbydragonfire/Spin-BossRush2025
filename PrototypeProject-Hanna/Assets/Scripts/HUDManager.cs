using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    public Image playerHealth;
    public Image BossHealth;
    public Image TempoSlider;
    public PlayerHealth pHealth;
    public BossManager bossManager;
    public MusicHandler musicHandler;

    public float SliderMinX = 80;
    public float SliderMaxX = 230;

    //TODO: Player HP needs to actually exist, just plug it in here as shown for BossHP and it'll work
    private float p_health_max = 100;
    private float b_health_max = 100;
    


    // Update is called once per frame
    void Update()
    {
        if (pHealth)
        {
            playerHealth.fillAmount = pHealth.currentHP / p_health_max;  //currently an arbitrary number for testing, replace with real HP.  
        }
        
        if (bossManager)
        {
            BossHealth.fillAmount = bossManager.GetCurrentBossHP() / b_health_max;
        }

        if (musicHandler)
        {
            var tempo = musicHandler.GetCurrentTempo() / musicHandler.GetMaxTempo();
            TempoSlider.rectTransform.anchoredPosition = new Vector2(GetXValue(tempo), TempoSlider.rectTransform.anchoredPosition.y);
        }
    }

    private float GetXValue(float percentage)
    {
        // Clamp the percentage to ensure it's between 0 and 1
        percentage = Mathf.Clamp01(percentage);

        // Calculate the X value using linear interpolation
        float xValue = Mathf.Lerp(SliderMinX, SliderMaxX, percentage);
        return xValue;
    }
}
