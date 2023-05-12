using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    
    //---------------------------------------------------------
    [field: SerializeField] public float CurrentStamina = 100.0f;
    [field: SerializeField] public float MaxStamina = 100.0f;
    [field: SerializeField] public const float StaminaIncreasePerFrame = 25.0f;
    [field: SerializeField] public bool isActing = false;
    [field: SerializeField] public GameObject slider;
    [field: SerializeField] public GameObject stam_ui;
    [field: SerializeField] public Vector2 original_ui = new Vector2(-12.50f, 3.50f);
    [field: SerializeField] public ParticleSystem ResourceEmitPS {get; private set;}
    //---------------------------------------------------------
    private float StaminaRegenTimer = 0.0f;
    private const float StaminaTimeToRegen = 3.0f;
    private float StaminaAnimationTimer = 0f;
    [field: SerializeField] private float StaminaAnimationValue = 20f;
    //---------------------------------------------------------

    public void SetSlider(GameObject sl) {
        slider = sl;
    }
    public void SetStamina(GameObject sl) {
        stam_ui = sl;
    }
    private void Update()
    {
        if(slider == null) {
            return;
        }
        if (isActing)
        {
            CurrentStamina = Mathf.Clamp(CurrentStamina - 15.0f, 0.0f, MaxStamina);
            StaminaRegenTimer = 0.0f;
            isActing = false;
        }
        else if (CurrentStamina < MaxStamina)
        {
            if (StaminaRegenTimer >= StaminaTimeToRegen)
                CurrentStamina = Mathf.Clamp(CurrentStamina + (StaminaIncreasePerFrame * Time.deltaTime), 0.0f, MaxStamina);
            else
                StaminaRegenTimer += Time.deltaTime;
        }

        slider.GetComponent<Slider>().value = CurrentStamina;
        if (StaminaAnimationTimer > 0) {
            if (StaminaAnimationTimer == 1) {
                EndStaminaAnimation();
            }
            StaminaAnimationTimer--;
        }
    }

    public float getStamina()
    {
        return CurrentStamina;
    }

    public void StartStaminaAnimation() {
        RectTransform rect = stam_ui.GetComponent<RectTransform>();
        StaminaAnimationTimer = StaminaAnimationValue;
        rect.anchoredPosition = new Vector2(-10f, 2f);
        ResourceEmitPS.Emit(1);

    }

    public void EndStaminaAnimation() {
        RectTransform rect = stam_ui.GetComponent<RectTransform>();
        rect.anchoredPosition = original_ui;
    }


    public bool canAct()
    {
        if (CurrentStamina >= 25.0f && !isActing)
        {
            return true;
        } else
        {
            StartStaminaAnimation();
            return false;
        }
        
    }
}
