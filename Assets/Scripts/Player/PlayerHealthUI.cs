using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private GameObject healthPrefab;
    private List<GameObject> healthObj;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void InitHealthUI(int hp)
    {
        healthObj = new List<GameObject>();
        while (hp > 0)
        {
            hp--;
            healthObj.Add(Instantiate(healthPrefab, transform));
        }
    }

    public void UpdateUI(int hp)
    {
        if (hp <= 0) return;

        for (int i = 0; i < healthObj.Count; i++)
        {
            if (i < hp - 1)
                healthObj[i].gameObject.SetActive(true);
            else
                healthObj[i].gameObject.SetActive(false);
        }
    }
}
