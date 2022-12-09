using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuCtrl : MonoBehaviour
{
    [SerializeField] private GameObject pauseObj;
    [SerializeField] private GameObject prepareObj;
    [SerializeField] private GameObject[] menuObj;
    [SerializeField] private GameObject[] playGameObj;

    private RectTransform rectTransform;

    public bool isMoving { get; private set; }
    private Coroutine moveCoroutine;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        MenuMoveIn();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            moveCoroutine.Stop(this);
            rectTransform.anchoredPosition = Vector2.zero;
            GameSceneManager.Instance.CanStart = true;
        }
    }

    private void MenuMoveIn()
    {
        ClosePlayUI();
        GameSceneManager.Instance.CanStart = false;
        MoveToTarget(Vector2.zero, 2f, new Vector2(0, -Camera.main.pixelHeight), () => GameSceneManager.Instance.CanStart = true);
        OpenMenuUI();
    }

    private void MoveToTarget(Vector2 target, float duration, Vector2 fromPos, Action OnComplete  = null)
    {
        moveCoroutine = StartCoroutine(DoMoveToTarget(target, duration, fromPos, OnComplete));
    }

    private IEnumerator DoMoveToTarget(Vector2 target, float duration, Vector2 fromPos, Action OnComplete)
    {
        rectTransform.anchoredPosition = fromPos;
        Vector2 speed = (target - rectTransform.anchoredPosition) / duration;
        for (float f = duration; f >= 0.0f; f -= Time.deltaTime)
        {
            rectTransform.anchoredPosition += speed * Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = target;
        OnComplete?.Invoke();
    }

    public void GameOverMove()
    {
        ClosePlayUI();
        MoveToTarget(new Vector2(0, 150), 1, Vector2.zero, () => MenuMoveIn());
    }

    public void GameStart()
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

    public void SetPauseUI(bool value)
    {
        pauseObj.SetActive(value);
    }

    public void SetPrepareUI(bool value)
    {
        prepareObj.SetActive(value);
    }
}
