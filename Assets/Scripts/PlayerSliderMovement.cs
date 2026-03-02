using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSliderMovement : MonoBehaviour
{
    public Slider sliderMovement;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(sliderMovement.value, transform.position.y);
        sliderMovement.onValueChanged.AddListener(OnSliderChange);
    }

    void OnSliderChange(float val)
    {
        transform.position = new Vector2(val, transform.position.y);
    }

    private void OnDestroy()
    {
        sliderMovement.onValueChanged.RemoveListener(OnSliderChange);
    }
}
