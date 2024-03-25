using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textbox;

    // Update is called once per frame
    void Update()
    {
        // enable/disable
        if (Input.GetKeyDown(KeyCode.P))
            textbox.gameObject.SetActive(!textbox.gameObject.activeSelf);

        // Only update display if object is active
        if (!textbox.gameObject.activeSelf) return;
        textbox.text = "FPS: " + (1.0f / Time.deltaTime);
    }
}
