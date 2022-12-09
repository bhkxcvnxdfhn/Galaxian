using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UICtrl : MonoBehaviour
{
    [SerializeField] private GameObject pauseObj;
    [SerializeField] private GameObject prepareObj;
    [SerializeField] private GameObject[] menuObj;
    [SerializeField] private GameObject[] playGameObj;

    private RectTransform rect;

    private Coroutine moveCoroutine;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        MenuMoveIn();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit") && moveCoroutine != null)
        {
            moveCoroutine.Stop(this);
            rect.anchoredPosition = Vector2.zero;
            GameSceneManager.Instance.CanStart = true;
        }
    }

    private void MenuMoveIn()
    {
        OpenMenuUI();
        GameSceneManager.Instance.CanStart = false;
        Vector2 from = new Vector2(0, -Camera.main.pixelHeight);

        MoveToTarget(Vector2.zero, 2f, from, () => GameSceneManager.Instance.CanStart = true);
    }

    private void MoveToTarget(Vector2 target, float duration, Vector2 fromPos, Action OnComplete)
    {
        moveCoroutine = StartCoroutine(DoMoveToTarget(target, duration, fromPos, OnComplete));
    }
    private IEnumerator DoMoveToTarget(Vector2 target, float duration, Vector2 fromPos, Action OnComplete)
    {
        rect.anchoredPosition = fromPos;
        Vector2 speed = (target - rect.anchoredPosition) / duration;
        while (duration > 0)
        {
            rect.anchoredPosition += speed * Time.deltaTime;
            duration -= Time.deltaTime;
            yield return null;
        }
        rect.anchoredPosition = target;
        OnComplete?.Invoke();
    }

    public void GameOverMove()
    {
        ClosePlayUI();
        MoveToTarget(new Vector2(0, 150), 1, Vector2.zero, () => MenuMoveIn());
    }

    public void OpenGamePlayUI()
    {
        CloseMenuUI();
        OpenPlayUI();
    }

    private void OpenMenuUI()
    {
        foreach(GameObject obj in menuObj)
        {
            obj.SetActive(true);
        }
    }

    private void CloseMenuUI()
    {
        foreach (GameObject obj in menuObj)
        {
            obj.SetActive(false);
        }
    }

    private void OpenPlayUI()
    {
        foreach (GameObject obj in playGameObj)
        {
            obj.SetActive(true);
        }
    }

    private void ClosePlayUI()
    {
        foreach (GameObject obj in playGameObj)
        {
            obj.SetActive(false);
        }
    }

    public void SetPauseUIActive(bool value)
    {
        pauseObj.SetActive(value);
    }

    public void SetPrepareUIActive(bool value)
    {
        prepareObj.SetActive(value);
    }
}
