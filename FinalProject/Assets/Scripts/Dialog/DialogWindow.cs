using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogWindow : MonoBehaviour
{
    private static DialogWindow instance;
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject btnAnswerPrefab;

    [SerializeField]
    private Transform ansTransform;

    //[SerializeField]
    //private Guider guider;

    private int index;
    private bool isAnswering = false;
    //public bool isFinishDialog = true;

    private Dialog dialog;
    private DialogNode currentNode;
    private List<DialogNode> answers = new List<DialogNode>();
    private List<GameObject> buttons = new List<GameObject>();

    
    public static DialogWindow MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<DialogWindow>();
            }
            return instance;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && Guider.MyInstance.Isinteracting)
        {
            if (text.text == currentNode.Text[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = currentNode.Text[index];
            }
        }
    }

    public void SetDialog(Dialog dialog)
    {
        index = 0;
        text.text = string.Empty;
        this.dialog = dialog;
        this.currentNode = dialog.Nodes[0];
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        StartCoroutine(RunDialog(currentNode.Text[0]));
        ShowAnswers();
    }
    private IEnumerator RunDialog(string dialog)
    {
        for (int i = 0; i < dialog.Length; i++)
        {
            text.text += dialog[i]; //This is for the text appear one by one
            yield return new WaitForSeconds(speed);
        }
        //ShowAnswers();
    }
    private void ShowAnswers()
    {
        answers.Clear();
        foreach (DialogNode node in dialog.Nodes)
        {
            if (node.Parent == currentNode.Name)
            {
                answers.Add(node);
            }
        }
        if (answers.Count > 0 && index == currentNode.Text.Length - 1)
        {
            ansTransform.gameObject.SetActive(true);
            isAnswering = true;
            foreach (DialogNode node in answers)
            {
                GameObject go = Instantiate(btnAnswerPrefab, ansTransform);
                buttons.Add(go);
                go.GetComponentInChildren<TextMeshProUGUI>().text = node.Answer;
                go.GetComponent<Button>().onClick.AddListener(delegate { PickAnswer(node); });
            }
        }
        //else
        //{
        //    ansTransform.gameObject.SetActive(true);
        //    isAnswering = true;
        //    GameObject go = Instantiate(btnAnswerPrefab, ansTransform);
        //    buttons.Add(go);
        //    go.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
        //    go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialog(); });
        //}
    }
    public void NextLine()
    {
        if(index < currentNode.Text.Length - 1)
        {
            index++;
            //Debug.Log("currentNode.Text : " + currentNode.Text.Length + "Index : " + index);
            text.text = string.Empty;
            StartCoroutine(RunDialog(currentNode.Text[index]));
            ShowAnswers();
        }
        else
        {
            if (!isAnswering)
            {
                CloseDialog();
                
            }
        }
    }
    private IEnumerator WaitToInteract()
    {
        yield return new WaitForSeconds(0.2f);
        Guider.MyInstance.Isinteracting = false;
    }
    private void PickAnswer(DialogNode node)
    {
        this.currentNode = node;
        Clear();
        index = 0;
        StartCoroutine(RunDialog(currentNode.Text[0]));
        ShowAnswers();
    }
    public void CloseDialog()
    {
        Close();
        Clear();
        StartCoroutine(WaitToInteract());
    }
    private void Clear()
    {
        text.text = string.Empty;
        ansTransform.gameObject.SetActive(false);
        isAnswering = false;
        foreach (GameObject gameObject in buttons)
        {
            Destroy(gameObject);
        }
        buttons.Clear();
    }
    private void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        //guider.Isinteracting = false;
    }
}
