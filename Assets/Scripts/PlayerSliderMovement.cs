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
        transform.position = new Vector2(transform.position.x, sliderMovement.value);
        sliderMovement.onValueChanged.AddListener(OnSliderChange);
    }

    void OnSliderChange(float val)
    {
        PlayerAutoRun autoRun = GetComponent<PlayerAutoRun>();
        if (autoRun != null && autoRun.IsAutoRunning) return;

        transform.position = new Vector2(transform.position.x, val);
    }

    private void OnDestroy()
    {
        sliderMovement.onValueChanged.RemoveListener(OnSliderChange);
    }
}
