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
            canvasGroup.alpha = 1 - (t / timer);  // ���� 1 �� 0���� ����
            yield return null;
        }

        canvasGroup.alpha = 0;

        // Panel ��Ȱ��ȭ
        gameObject.SetActive(false);

        // ���⼭ �� ��ȯ or ���� ���� ����
        SceneManager.LoadScene("LobbyScene");
    }
}
