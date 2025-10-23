using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ��� / ������ / ADS �Է��� ������ WeaponController�� ����.
/// �Է� ��ü�� ���¸� ������ ������, ���� �Ǵ��� WeaponController�� ���.
/// </summary>

[DisallowMultipleComponent]
public class WeaponInput : MonoBehaviour
{
    public WeaponController weapon; // �̺�Ʈ�� ������ ���.
    public CameraEffectsMixer mixer;

    private bool fireHeld;      // ���콺 ��Ŭ�� ���� ����
    private bool adsHeld;       // ��Ŭ�� ���� ����.
    private bool reloadPressed; // ������ ��ư�� ���� ������.

    public void onFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == true)
        {
            fireHeld = true;
            Debug.Log("Fire Clicked Down!!!!!");
        }
        if (ctx.canceled == true)
        {
            fireHeld = false;
            Debug.Log("Fire Clicked Up!!!!!!");
        }
    }

    public void OnADS(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == true)
        {
            adsHeld = true;
            if (mixer != null)
            {
                mixer.SetFOV(false);
            }
            Debug.Log("ADS Clicked Down!!!!!");
        }
        if (ctx.canceled == true)
        {
            if (mixer != null)
            {
                mixer.SetFOV(true);
            }
            Debug.Log("ADS Clicked Up!!!!!");
        }
    }

    public void OnReload(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == true)
        {
            reloadPressed = true;
            Debug.Log("Reload Clicked Down!!!!!");
        }
    }
 

    // Update is called once per frame
    private void Update()
    {
        if (weapon == null)
        {
            return;
        }

        // �� ������ WeaponController�� ���� �Է� ���� ����.
        weapon.SetFireHeld(fireHeld);
        weapon.SetAdsHeld(adsHeld);

        if (reloadPressed == true)
        {
            weapon.RequestReload();
            reloadPressed = false;
        }
    }
}
