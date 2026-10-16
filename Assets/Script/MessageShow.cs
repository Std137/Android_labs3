using TMPro;
using UnityEngine;
using System.Collections;

public class MessageShow: MonoBehaviour
{
    public void MsgShow(string message)
    {
        TextMeshProUGUI txtMP = GetComponent<TextMeshProUGUI>();
        txtMP.text = message;
        gameObject.SetActive(true);
        StartCoroutine(DelayedAction());
    }

    IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    public void MsgStop()
    {
        gameObject.SetActive(false);
    }
}
