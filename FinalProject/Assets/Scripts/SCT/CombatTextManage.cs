using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SCTTYPE {DAMAGE,HEAL,XP,TEXT,MANA}
public class CombatTextManage : MonoBehaviour
{
    public Coroutine WriteRoutine;
    
    private static CombatTextManage instance;
    public static CombatTextManage MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<CombatTextManage>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject combatTextPrefab;

    protected Queue<SCTObject> SCTQueue = new Queue<SCTObject>();

    public void Start()
    {
        WriteRoutine = StartCoroutine(WriteText());
    }
    public void CreateText(Vector2 position, string text, SCTTYPE type, bool crit)
    {
        SCTQueue.Enqueue(new SCTObject() { Crit = crit, Position = position, Text = text, SCTType = type });
    }
    public IEnumerator WriteText()
    {
        while (true)
        {
            if(SCTQueue.Count > 0)
            {
                SCTObject sctObject = SCTQueue.Dequeue();
                Vector2 sctPos = sctObject.Position;

                //Offset
                sctPos.y += 0.8f;
                Text sct = Instantiate(combatTextPrefab, transform).GetComponent<Text>();
                sct.transform.position = sctPos;

                string before = string.Empty;
                string after = string.Empty;
                switch (sctObject.SCTType)
                {
                    case SCTTYPE.DAMAGE:
                        before = "-";
                        sct.color = Color.red;
                        break;
                    case SCTTYPE.HEAL:
                        before = "+";
                        sct.color = Color.green;
                        break;
                    case SCTTYPE.MANA:
                        before = "+";
                        sct.color = Color.blue;
                        break;
                    case SCTTYPE.XP:
                        before = "+";
                        after = "XP";
                        sct.color = Color.yellow;
                        break;
                    case SCTTYPE.TEXT:
                        sct.color = Color.white;
                        break;
                }
                sct.text = before + sctObject.Text + after;
                if (sctObject.Crit)
                {
                    sct.GetComponent<Animator>().SetBool("crit", sctObject.Crit);
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    //public void StartWriteText()
    //{
    //    StartCoroutine(WriteText());
    //}
    //public void StopWriteText()
    //{
    //    if(WriteRoutine != null)
    //    {
    //        StopCoroutine(WriteRoutine);
    //    }
    //}
}

public class SCTObject
{
    public Vector2 Position { get; set; }
    public string Text { get; set; }
    public SCTTYPE SCTType { get; set; }
    public bool Crit { get; set; }
}
