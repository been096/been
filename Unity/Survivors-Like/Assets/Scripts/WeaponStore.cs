using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponStore : MonoBehaviour
{
    public SceneManager GameScene;
    public static WeaponStore Instance;

    public enum WeaponType { Melee, Ranged }
    public WeaponType currentWeapon = WeaponType.Melee;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ¾À ¹Ù²î¾îµµ ¾È »ç¶óÁü
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SceneManager.LoadScene("WeaponStore");
        }
    }
}
