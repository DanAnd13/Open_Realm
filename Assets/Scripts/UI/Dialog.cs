using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Dialog : MonoBehaviour
{
    [SerializeField] private DialogOutput _dialogOutput;
    [SerializeField] private List<DialogObject> _dialogObjects;
    [SerializeField] private PlayableDirector _director;

    private Coroutine _dialogCoroutine;

    [ContextMenu("Start Dialog")]
    public void StartDialog()
    {
        if (_dialogCoroutine != null)
        {
            StopCoroutine(_dialogCoroutine);
            _dialogCoroutine = null;
        }
        _director.Pause();
        _dialogCoroutine = StartCoroutine(DialogRoutine());
    }

    private IEnumerator DialogRoutine()
    {
        _dialogOutput.DialogTextObject.SetActive(true);

        foreach (DialogObject dialog in _dialogObjects)
        {
            yield return StartCoroutine(TypeText(dialog));

            yield return new WaitForSeconds(dialog.TextShowDuration);
        }

        _dialogOutput.DialogTMP.text = "";
        _dialogOutput.DialogTextObject.SetActive(false);
        _director.Play();
    }

    private IEnumerator TypeText(DialogObject dialog)
    {
        string text = dialog.DialogText;

        _dialogOutput.DialogTMP.text = "";

        if (string.IsNullOrEmpty(text))
            yield break;

        if (dialog.FadeDuration <= 0f)
        {
            _dialogOutput.DialogTMP.text = text;
            yield break;
        }

        float timePerCharacter = dialog.FadeDuration / text.Length;

        for (int i = 1; i <= text.Length; i++)
        {
            _dialogOutput.DialogTMP.text = text.Substring(0, i);
            yield return new WaitForSeconds(timePerCharacter);
        }
    }
}
