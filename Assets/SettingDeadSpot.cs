using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingDeadSpot : MonoBehaviour
{
    public GameObject deadSpot;

    // Start is called before the first frame update
    void Start()
    {
        deadSpot.SetActive(true);
        StartCoroutine(RemoveOnAwake());
    }

    IEnumerator RemoveOnAwake()
    {
        yield return new WaitForSeconds(3f);

        deadSpot.SetActive(false);
    }
}
