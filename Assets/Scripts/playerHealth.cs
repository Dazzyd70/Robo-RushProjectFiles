using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    public float maxHealth;
    public Image HPBar;
    public Image damageOverlay;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        GlobalReferences.Instance.playerHealth = 100;
        damageOverlay.color = new Color(damageOverlay.color.r, damageOverlay.color.b, damageOverlay.color.g, 0);
    }

    // Update is called once per frame
    void Update()
    {
        HPBar.fillAmount = Mathf.Clamp(GlobalReferences.Instance.playerHealth / maxHealth, 0, 1);

        if (GlobalReferences.Instance.playerHealth > maxHealth)
        {
            GlobalReferences.Instance.playerHealth = maxHealth;
        }

        
    }


}
