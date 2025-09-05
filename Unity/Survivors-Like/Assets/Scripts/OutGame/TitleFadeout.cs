using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class TitleFadeout : MonoBehaviour, IPointerClickHandler
{
    public CanvasGroup canvasGroup;
    public float timer = 1f;

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        
        float t = 0f;

        while (t < timer)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1 - (t / timer);  // 알파 1 → 0으로 감소
            yield return null;
        }

        canvasGroup.alpha = 0;

        // Panel 비활성화
        gameObject.SetActive(false);

        // 여기서 씬 전환 or 게임 시작 로직
        SceneManager.LoadScene("LobbyScene");
    }
}
