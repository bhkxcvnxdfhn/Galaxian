using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private GameObject[] levelObj;

    public void RefreshUI(int level)
    {
        for (int i = 0; i < levelObj.Length; i++)
        {
            if (i < level)
                levelObj[i].gameObject.SetActive(true);
            else
                levelObj[i].gameObject.SetActive(false);
        }
    }
}
