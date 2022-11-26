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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Cast castSkill(int index)
    {
        
        if(player.myTarget != null)
        {
            if (skills[index].myCheckManaSufficient && !player.isMoving && player.myTarget.IsAlive)
            {
                skillIcon[index].fillAmount = 0;
                //skillIcon[index].color = skills[index].myBarColor;
                skillRoutine = StartCoroutine(Progress(index));
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
            //player.isCoolDown = true;
            skillIcon[index].fillAmount = Mathf.Lerp(0, 1,progress);
            progress += rate * Time.deltaTime;

            timePassed += Time.deltaTime;
            txtCastCD[index].text = (skills[index].myCastCD - timePassed).ToString("F1");
            if(skills[index].myCastCD - timePassed < 0)
            {
                txtCastCD[index].text = "";
                skills[index].myCheckCD = false;
                //player.isCoolDown = false;
            }
            yield return null;
        }
        //skillIcon[index].color = initSkillIcon;
        //StopCasting();
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
    //public bool checkCastCD(int index)
    //{
    //    if (Progress(index) != null)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    public void StopCasting()
    {
        if (skillRoutine != null)
        {
            StopCoroutine(skillRoutine);
            skillRoutine = null;
        }
    }
}
