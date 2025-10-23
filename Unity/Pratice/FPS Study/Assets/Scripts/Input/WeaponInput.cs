using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 사격 / 재장전 / ADS 입력을 수집해 WeaponController로 전달.
/// 입력 자체는 상태를 가지지 않으며, 상태 판단은 WeaponController가 담당.
/// </summary>

[DisallowMultipleComponent]
public class WeaponInput : MonoBehaviour
{
    public WeaponController weapon; // 이벤트를 전달할 대상.
    public CameraEffectsMixer mixer;

    private bool fireHeld;      // 마우스 좌클릭 누름 상태
    private bool adsHeld;       // 우클릭 누름 상태.
    private bool reloadPressed; // 재장전 버튼이 눌린 프레임.

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

        // 매 프레임 WeaponController에 현재 입력 상태 전달.
        weapon.SetFireHeld(fireHeld);
        weapon.SetAdsHeld(adsHeld);

        if (reloadPressed == true)
        {
            weapon.RequestReload();
            reloadPressed = false;
        }
    }
}
