using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSet : MonoBehaviour
{
    [SerializeField]
    private Image[] skillIcon;

    [SerializeField]
    private Color initSkillIcon;

    [SerializeField]
    private Text[] txtCastCD;

    [SerializeField]
    private Cast[] skills;

    [SerializeField]
    private Player player;

    private Coroutine skillRoutine;

    private static SkillSet instance;
    public static SkillSet MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SkillSet>();
            }
            return instance;
        }
    }

    public Cast castSkill(int index)
    {
        if(index == 0)
        {
            if (player.myTarget != null)
            {
                if (skills[index].myCheckManaSufficient && player.myTarget.IsAlive)
                {
                    skillIcon[index].fillAmount = 0;
                    skillRoutine = StartCoroutine(Progress(index));
                }
                else if (skills[index].myManaCost > Player.MyInstance.mana.MyCurrentValue)
                {
                    CombatTextManage.MyInstance.CreateText(player.transform.position, "Out of MP", SCTTYPE.TEXT, false);
                }
            }
        }
        else
        {
            if (skills[index].myCheckManaSufficient && player.IsAlive)
            {
                skillIcon[index].fillAmount = 0;
                skillRoutine = StartCoroutine(Progress(index));
            }
            else if (skills[index].myManaCost > Player.MyInstance.mana.MyCurrentValue)
            {
                CombatTextManage.MyInstance.CreateText(player.transform.position, "Out of MP", SCTTYPE.TEXT, false);
            }
        }
        return skills[index];
    }
    private IEnumerator Progress(int index)
    {
        float timePassed = Time.deltaTime;
        float rate = 1.0f / skills[index].myCastCD;
        float progress = 0.0f;
        while(progress<= 1.0)
        {
            skills[index].myCheckCD = true;
            skillIcon[index].fillAmount = Mathf.Lerp(0, 1,progress);
            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;
            txtCastCD[index].text = (skills[index].myCastCD - timePassed).ToString("F1");
            if(skills[index].myCastCD - timePassed < 0)
            {
                txtCastCD[index].text = "";
                skills[index].myCheckCD = false;
            }
            yield return null;
        }
    }
    public bool CD1()
    {
        return skills[0].myCheckCD;
    }
    public bool CD2()
    {
        return skills[1].myCheckCD;
    }
    public bool CD3()
    {
        return skills[2].myCheckCD;
    }

    public void StopCasting()
    {
        if (skillRoutine != null)
        {
            StopCoroutine(skillRoutine);
            skillRoutine = null;
        }
    }
}
