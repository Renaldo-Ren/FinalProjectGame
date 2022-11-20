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

    [SerializeField]
    private Guider guider;

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
    public void SetDialog(Dialog dialog)
    {
        text.text = string.Empty;
        this.dialog = dialog;
        this.currentNode = dialog.Nodes[0];
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        StartCoroutine(RunDialog(currentNode.Text));
    }
    private IEnumerator RunDialog(string dialog)
    {
        for (int i = 0; i < dialog.Length; i++)
        {
            text.text += dialog[i]; //This is for the text appear one by one
            yield return new WaitForSeconds(speed);
        }
        ShowAnswers();
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
        if (answers.Count > 0)
        {
            ansTransform.gameObject.SetActive(true);
            foreach (DialogNode node in answers)
            {
                GameObject go = Instantiate(btnAnswerPrefab, ansTransform);
                buttons.Add(go);
                go.GetComponentInChildren<TextMeshProUGUI>().text = node.Answer;
                go.GetComponent<Button>().onClick.AddListener(delegate { PickAnswer(node); });
            }
        }
        else
        {
            ansTransform.gameObject.SetActive(true);
            GameObject go = Instantiate(btnAnswerPrefab, ansTransform);
            buttons.Add(go);
            go.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
            go.GetComponent<Button>().onClick.AddListener(delegate { CloseDialog(); });
        }
    }
    private void PickAnswer(DialogNode node)
    {
        this.currentNode = node;
        Clear();
        StartCoroutine(RunDialog(currentNode.Text));
    }
    public void CloseDialog()
    {
        Close();
        Clear();
    }
    private void Clear()
    {
        text.text = string.Empty;
        ansTransform.gameObject.SetActive(false);
        foreach(GameObject gameObject in buttons)
        {
            Destroy(gameObject);
        }
        buttons.Clear();
    }
    private void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        guider.Isinteracting = false;
    }
}
