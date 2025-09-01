using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponShopUI : MonoBehaviour
{
    public void BuyMeleeWeapon()
    {
        WeaponStore.Instance.currentWeapon = WeaponStore.WeaponType.Melee;
        SceneManager.LoadScene("SampleScene");
    }

    public void BuyRangedWeapon()
    {
        WeaponStore.Instance.currentWeapon = WeaponStore.WeaponType.Ranged;
        SceneManager.LoadScene("SampleScene");
    }
}
