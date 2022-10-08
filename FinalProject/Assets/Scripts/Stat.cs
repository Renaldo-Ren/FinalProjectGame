using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Stat : MonoBehaviour
{
    [SerializeField]
    private Image content;

    [SerializeField]
    private Text statVal;

    [SerializeField]
    private float lerpSpeed;
    private float currentFill;
    private float curVal;
    public float MyMaxVal { get; set; }

    public float MyCurrentValue
    {
        get
        {
            return curVal;
        }
        set
        {
            if (value > MyMaxVal)
            {
                curVal = MyMaxVal;
            }
            else if(value < 0)
            {
                curVal = 0;
            }
            else
            {
                curVal = value;
            }
            currentFill = curVal / MyMaxVal;
            if (statVal != null)
            {
                statVal.text = curVal + "/" + MyMaxVal;
            }
            
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
        //content.fillAmount = currentFill;
    }
    public void Initialize(float curVal, float maxVal)
    {
        if(content == null)
        {
            content = GetComponent<Image>();
        }
        MyMaxVal = maxVal;
        MyCurrentValue = curVal;
        content.fillAmount = MyCurrentValue / MyMaxVal; //for the hp will immediately start at full hp without lerp to 1 when change target
    }
}
