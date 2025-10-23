using UnityEngine;
using TMPro;

/// <summary>
/// "źâ/����' �������� ź���� ǥ��.
/// </summary>
public class AmmoHUD : MonoBehaviour
{
    public TextMeshProUGUI label;

    private void Awake()
    {
        if (label == null)
        {
            label = GetComponent<TextMeshProUGUI>();
        }
    }
    
    public void SetAmmo(int mag, int reserve)
    {
        if (label == null)
        {
            return;
        }

        label.text = $"{mag} / {reserve}";
    }
}
