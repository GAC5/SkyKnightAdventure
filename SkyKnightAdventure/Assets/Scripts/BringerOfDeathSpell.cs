using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathSpell : MonoBehaviour
{
    private GameObject boss;
    private BringerOfDeathBoss bossScript; 
    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.Find("BringerOfDeathBoss");
        bossScript = boss.GetComponent<BringerOfDeathBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DestroySelf()
    {
        if (bossScript.destroySpell == true)
        {
            Destroy(gameObject);
        }
    }
}
