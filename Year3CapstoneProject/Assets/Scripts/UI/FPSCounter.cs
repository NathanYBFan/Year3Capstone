using System.Collections;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textbox;

    [SerializeField]
    private int fpsUpdateTime = 50;

    private void Start()
    {
        StartCoroutine(FpsCounterUpdate());
    }

    private IEnumerator FpsCounterUpdate()
    {
        int counter = 0;

        while (true)
        {
            counter++;
            yield return null;
            if (textbox.gameObject.activeSelf && counter >= fpsUpdateTime)
            {
                textbox.text = "FPS: " + Mathf.Round((1.0f / Time.deltaTime) / 5) * 5;
                counter = 0;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // enable/disable
        if (Input.GetKeyDown(KeyCode.P))
            textbox.gameObject.SetActive(!textbox.gameObject.activeSelf);
    }
}
