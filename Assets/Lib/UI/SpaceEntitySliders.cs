using UnityEngine;
using UnityEngine.UI;

public class SpaceEntitySliders : MonoBehaviour
{

    public SpaceEntity entity;
    Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider = GetComponent<Slider>();
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float x = (float)entity.hp / (float)entity.maxHp;
        slider.value = x;

    }
}
