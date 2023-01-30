using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private Image toggleImg;
    [SerializeField] private Sprite spriteOn;
    [SerializeField] private Sprite spriteOff;
    
    private Toggle toggle;
    
    private void Awake()
    {
        toggle = GetComponent<Toggle>();

        toggleImg.sprite = toggle.isOn ? spriteOn : spriteOff; //safety, to check if will disabled by default

        toggle.onValueChanged.AddListener(OnSwitch);
    }

    void OnSwitch(bool isOn)
    {
        toggleImg.sprite = isOn ? spriteOn : spriteOff;
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }
}
