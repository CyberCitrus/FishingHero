using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Color highlightedColor;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<TextMeshProUGUI> actionTexts;
    [SerializeField] List<TextMeshProUGUI> moveTexts;
    [SerializeField] TextMeshProUGUI ppText;
    [SerializeField] TextMeshProUGUI typeText;

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray()) 
         {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / 20);
         }

        yield return new WaitForSeconds(1f);
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }
    public void EnableMoveSelector(bool enabled) 
    {
        moveSelector.SetActive(enabled); 
    }
    public void EnableMoveDetails(bool enabled)
    {
        moveDetails.SetActive(enabled);
    }
    public void UpdateActionSelection(int selectedAction)
    {//根据键盘选择改变UI
        for (int i = 0; i < actionTexts.Count; ++i)
        {
            if (i == selectedAction)
                actionTexts[i].color = Color.yellow;
            else
                actionTexts[i].color = Color.white;
        }
    }
    public void SetMoveNames(List<Move> moves)
    {
        for(int i = 0; i<moveTexts.Count; i++)
        {
            if (i < moves.Count)
                moveTexts[i].text = moves[i].mbase.Name;
            else
                moveTexts[i].text = "-";
        }
    }
    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for(int i=0; i<moveTexts.Count; ++i)
        {
            if(i == selectedMove)
                moveTexts[i].color = Color.yellow;
            else
                moveTexts[i].color= Color.white;
        }
        ppText.text = $"PP {move.Point}/{move.mbase.Point}";
        typeText.text = $"{move.mbase.Info}";
    }
}
