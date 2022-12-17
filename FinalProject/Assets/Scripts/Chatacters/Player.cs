using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player : Character
{
    [SerializeField]
    public Stat mana;
    [SerializeField]
    private float initMP = 50;
    [SerializeField]
    private Block[] blocks;
    [SerializeField]
    private Transform[] exitPoints;

    public string scenePassword;
    public GameObject btnInteract;
    private Vector2 initPos;
    private int exitIndex =2;
    private IInteractable interactable;
    private bool inRangeInteract = false;
    private bool isInvulnerable = false;
    private SkillSet skillSet;
    private Vector3 min, max;

    public bool InCombat { get; set; } = false;
    private static Player instance;
    public static Player MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }
    protected override void Start()
    {
        skillSet = SkillSet.MyInstance.GetComponent<SkillSet>();
        
        MyAnim.SetFloat("x", 0);
        MyAnim.SetFloat("y", 1);
        mana.Initialize(initMP, initMP);
        initPos = transform.parent.position;
        base.Start();
        StartCoroutine(Regen());
    }

    protected override void Update()
    {
        if(Guider.MyInstance != null)
        {
            if (!Guider.MyInstance.Isinteracting)
            {
                GetInput();
            }
            if (!Guider.MyInstance.Isinteracting && inRangeInteract)
            {
                btnInteract.gameObject.SetActive(true);
            }
            else
            {
                btnInteract.gameObject.SetActive(false);
            }
        }
        else
        {
            GetInput();
        }
        
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
        base.Update();
    }

    private void GetInput()
    {
        Direction = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;
        }

        if (Input.GetKey(KeyCode.W))
        {
            exitIndex = 0;
            Direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            Direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 2;
            Direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            Direction += Vector2.right;
        }
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (!IsAttacking && !UIManage.isPaused)
            {
                StartCoroutine(Attack());
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (inRangeInteract && !Guider.MyInstance.Isinteracting)
            {
                Interact();
            }
        }
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    private IEnumerator Attack()
    {
        IsAttacking = true;
        MyAnim.SetBool("attack", IsAttacking);
        yield return new WaitForSeconds(0.2f);
        StopAttack();
    }
    private IEnumerator Cast(int skillIndex)
    {
        Cast newSkill = skillSet.castSkill(skillIndex);
        isCasting = true;
        MyAnim.SetBool("cast", isCasting); 
        yield return new WaitForSeconds(newSkill.myCastTime);
        if (skillIndex == 0)
        {
            if(myTarget != null && InLineofSight())
            {
                CastScript s = Instantiate(newSkill.myCastPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<CastScript>();
                s.Initialize(myTarget.MyHitbox, newSkill.myDamage, this);
                mana.MyCurrentValue -= newSkill.myManaCost;
            }
        }
        else
        {
            GameObject Clone = Instantiate(newSkill.myCastPrefab, transform);
            mana.MyCurrentValue -= newSkill.myManaCost;
            if(skillIndex == 1)
            {
                StartCoroutine(Shield(Clone));
            }
            else if (skillIndex == 2)
            {
                StartCoroutine(Booster(Clone));
            }
        }
        newSkill.myCheckManaSufficient = false;
        StopCast();
    }
    private IEnumerator Shield(GameObject shieldPrefab)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(6f);
        isInvulnerable = false;
        Destroy(shieldPrefab);
    }

    private IEnumerator Booster(GameObject boosterPrefab)
    {
        Speed += 2;
        yield return new WaitForSeconds(6f);
        Speed -= 2;
        Destroy(boosterPrefab);
    }

    public void Casting(int skillIndex)
    {
        Cast manaCost = skillSet.castSkill(skillIndex);
        Block();
        if(manaCost.myManaCost <= mana.MyCurrentValue && !manaCost.myCheckCD)
        {
            manaCost.myCheckManaSufficient = true;
            if(skillIndex == 0)
            {
                if (myTarget != null && myTarget.GetComponentInParent<Character>().IsAlive && !manaCost.myCheckCD && !isCasting && InLineofSight() && IsAlive && !UIManage.isPaused)
                {
                    attackCoroutine = StartCoroutine(Cast(skillIndex));
                }
            }
            else
            {
                if (!isCasting && !manaCost.myCheckCD && IsAlive && !UIManage.isPaused)
                {
                    attackCoroutine = StartCoroutine(Cast(skillIndex));
                }
            }
        }
        else
        {
            manaCost.myCheckManaSufficient = false;
        }
    }
    private bool InLineofSight()
    {
        if(myTarget != null)
        {
            Vector3 targetDir = (myTarget.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir, Vector2.Distance(transform.position, myTarget.transform.position), 128);
            if (hit.collider == null)
            {
                return true;
            }
        }
        return false;
    }
    private void Block()
    {
        foreach(Block b in blocks)
        {
            b.Deactive();
        }
        blocks[exitIndex].Active();
    }

    public override void TakeDmg(float dmg, Character source, Vector2 knockback)
    {
        if (isInvulnerable)
        {
            return;
        }
        else
        {
            if (IsAlive)
            {
                base.TakeDmg(dmg, source, knockback);
                if (health.MyCurrentValue <= 0)
                {
                    knockback = Vector2.zero;
                }
                MyRb.AddForce(knockback);
            }
        }
    }

    public void StopAttack()
    {
        IsAttacking = false;
        MyAnim.SetBool("attack", IsAttacking);
    }
    public void StopCast()
    {
        isCasting = false;
        MyAnim.SetBool("cast", isCasting);
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }

    private IEnumerator Regen()
    {
        while (true)
        {
            if (!InCombat && IsAlive)
            {
                if (health.MyCurrentValue < health.MyMaxVal)
                {
                    int value = Mathf.FloorToInt(health.MyMaxVal * 0.05f); //The value how much we will regen
                    health.MyCurrentValue += value;
                    CombatTextManage.MyInstance.CreateText(transform.position, value.ToString(), SCTTYPE.HEAL, false);
                }

                if (mana.MyCurrentValue < mana.MyMaxVal)
                {
                    int value = Mathf.FloorToInt(mana.MyMaxVal * 0.05f); //The value how much we will regen
                    mana.MyCurrentValue += value;
                    CombatTextManage.MyInstance.CreateText(transform.position, value.ToString(), SCTTYPE.MANA, false);
                }
            }
            yield return new WaitForSeconds(1.5f); //This is how often we will regen
        }
    }

    public void Interact()
    {
        if(interactable != null)
        {
            interactable.Interact();
        }
    }

    public IEnumerator Respawn()
    {
        MySpriteRenderer.enabled = false;
        yield return new WaitForSeconds(3f);
        health.Initialize(initHP, initHP);
        mana.Initialize(initMP, initMP);
        SceneManager.LoadScene("ForestIntro");
        MyAnim.SetFloat("x", 0);
        MyAnim.SetFloat("y", -1);
        transform.parent.position = initPos;
        MySpriteRenderer.enabled = true;
        MyAnim.SetTrigger("respawn");
    }
    public void ResetPlayer()
    {
        health.Initialize(initHP, initHP);
        mana.Initialize(initMP, initMP);
        transform.parent.position = initPos;
        UIManage.myInstance.HideTargetFrame();
        MyAnim.SetFloat("x", 0);
        MyAnim.SetFloat("y", 1);
    }

    public override void AddAttacker(Character attacker)
    {
        int count = Attackers.Count;
        base.AddAttacker(attacker);
        if(count == 0)
        {
            InCombat = true;
            Debug.Log("In Combat");
            CombatTextManage.MyInstance.CreateText(transform.position, "IN COMBAT", SCTTYPE.TEXT, false);
        }

    }
    public override void RemoveAttacker(Character attacker)
    {
        base.RemoveAttacker(attacker);
        if(Attackers.Count == 0)
        {
            InCombat = false;
            Debug.Log("Not In Combat");
            CombatTextManage.MyInstance.CreateText(transform.position, "OUT OF COMBAT", SCTTYPE.TEXT, false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="Interactable")
        {
            interactable = collision.GetComponent<IInteractable>();
            inRangeInteract = true;
            btnInteract.gameObject.SetActive(true);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interactable")
        {
            if(interactable != null)
            {
                interactable.StopInteract();
                interactable = null;
                inRangeInteract = false;
                
            }
            btnInteract.gameObject.SetActive(false);
        }
    }

}
