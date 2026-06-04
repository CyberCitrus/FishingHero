using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class battleHUD : MonoBehaviour
{
    [SerializeField] public HPBar hpBar;
    [SerializeField] public GameObject expBar;
    [SerializeField] public TextMeshProUGUI lvText;
    [SerializeField] public TextMeshProUGUI name;

    Monster _monster;
    private void Start()
    {
        _monster.OnHPChanged += UpdateHP;
    }
    public void setData(Monster player)
    {
        _monster = player;
        name.text = _monster.Name;
        //lvText.text = "Lv." + _monster.level;
        SetLevel();
        //Debug.Log($"当前HP为{_monster.HP}，满血量为{_monster.MaxHp}，setHP参数为{_monster.HP / _monster.MaxHp}");
        hpBar.setHP(((float)_monster.HP / (float)_monster.MaxHp));
        SetExp();
    }
    public void UpdateHP()
    {
        StartCoroutine(UpdateHPAsync());
    }
    public IEnumerator UpdateHPAsync()
    {
        yield return hpBar.SetHPSmooth((float)_monster.HP / _monster.MaxHp);
    }

    public void SetLevel()
    {
        lvText.text = "Lv." + _monster.level;
    }
    public void SetExp()
    {
        if (expBar == null) { return; }

        float normalizedExp = GetNormalizedExp();
        expBar.transform.localScale = new Vector3(normalizedExp, 1, 1);
    }

    public IEnumerator SetExpSmooth(bool reset=false)
    {
        if (expBar == null) { yield break; }

        if(reset)
        {
            expBar.transform.localScale = new Vector3(0, 1, 1);
        }

        float normalizedExp = GetNormalizedExp();
        yield return expBar.transform.DOScaleX(normalizedExp, 1.5f).WaitForCompletion();
    }

    float GetNormalizedExp()
    {
        int currentExp = _monster.Base.GetExpForLevel(_monster.level);
        int nextLevelExp = _monster.Base.GetExpForLevel(_monster.level + 1);

        float normalizedExp = (float)(_monster.ExpNow - currentExp) / (nextLevelExp - currentExp);
        return Mathf.Clamp01(normalizedExp);
    }
    public void ClearData()
    {
        if (_monster != null)
        {
            _monster.OnHPChanged -= UpdateHP;
        }
    }
}
