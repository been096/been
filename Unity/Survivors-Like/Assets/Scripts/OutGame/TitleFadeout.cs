using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleFadeout : MonoBehaviour
{
    public Image image;
    public Color color;
    public float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        timer += Time.deltaTime;
        Color color = image.color;
        color.a = 0.5f;
        image.color = color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Onclick()
    {
        FadeOut();
    }

    private IEnumerator FadeOut()
    {
        Color color = image.color;
        float t = 0f;

        while (t < timer)
        {
            t += Time.deltaTime;
            color.a = 1 - (t / timer);  // 알파 1 → 0으로 감소
            image.color = color;
            yield return null;
        }

        color.a = 0;
        image.color = color;

        // Panel 비활성화
        gameObject.SetActive(false);

        // 여기서 씬 전환 or 게임 시작 로직
        // SceneManager.LoadScene("GameScene");
    }
}
