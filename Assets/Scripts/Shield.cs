using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public float cooldown;

    [HideInInspector] public bool isCooldown;

    private Image shieldImage;
    private Girl girl;
    void Start()
    {
        shieldImage = GetComponent<Image>();
        girl = GameObject.FindGameObjectWithTag("Player").GetComponent<Girl>();
        isCooldown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooldown)
        {
            shieldImage.fillAmount -= 1 / cooldown * Time.deltaTime;
            if (shieldImage.fillAmount <= 0)
            {
                shieldImage.fillAmount = 1;
                isCooldown = false;
                girl.shield.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    public void ResetTimer()
    {
        shieldImage.fillAmount = 1;
    }

    public void ReduceTime(int damage)
    {
        shieldImage.fillAmount += damage / 6f;
    }
}
