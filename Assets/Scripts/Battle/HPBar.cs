using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;

    public void setHP(float hpNormalized)
    {//横向缩放血条大小
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }
    public IEnumerator SetHPSmooth(float newHP)
    {//利用IEnumerator特性平滑缩放
        float curHP = health.transform.localScale.x;
        float changeAmt = curHP - newHP;

        //Mathf.Epsilon-微小浮点数，两个浮点数之间可以相差的最小值
        while(curHP - newHP > Mathf.Epsilon)
        {//以时间为单位减少changeAmt并更新
            curHP -= changeAmt * Time.deltaTime;
            health.transform.localScale = new Vector3(curHP, 1f);
            yield return null;
        }
        health.transform.localScale = new Vector3(newHP, 1f);
    }
}
