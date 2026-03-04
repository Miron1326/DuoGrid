using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public int numberTurorial = 1;
    private bool ToStart;
    public bool FirstComplete;
    public bool TwoComplete;
    public bool ThreeComplete;
    private CanvasGroup canvasGroup;
    private Vector3 StartPos;
    void Start()
    {
        StartPos = GameObject.Find("Panel1").transform.position;
        canvasGroup = GameObject.Find("Canvas").GetComponent<CanvasGroup>();
        Animator animator1 = GameObject.Find("Panel1").GetComponent<Animator>();
        animator1.Play("Show1");
    }
    private void Update()
    {
        if (ToStart)
        {
            DisableClickCanvas();
        }
        if (Input.anyKeyDown)
        {
            FirstComplete = true;
            if (numberTurorial == 1)
            {
                TutorialStart();
            }
            
        }
        if (numberTurorial == 2)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                TwoComplete = true;
                TutorialStart();
            }
        }
        if(numberTurorial == 3)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ThreeComplete = true;
                TutorialStart();
            }
        }
        if (numberTurorial == 2 && ToStart)
        {
            Animator animator2 = GameObject.Find("Panel2").GetComponent<Animator>();
            GameObject Panel2 = GameObject.Find("Panel2");
            Panel2.transform.position = StartPos;
            animator2.Play("show2");
            ToStart = false;
        }
        if (numberTurorial == 3 && ToStart)
        {
            Animator animator2 = GameObject.Find("FinalPanel").GetComponent<Animator>();
            animator2.Play("ShowFinal");
            ToStart = false;
        }
    }
    private void TutorialStart()
    {
        EnableClickCanvas();
        if (ThreeComplete)
        {
            Animator animator1 = GameObject.Find("FinalPanel").GetComponent<Animator>();
            animator1.SetBool("StopFinal", true);
            DisableClickCanvas();
            return;
        }
        if (TwoComplete)
        {
            Animator animator1 = GameObject.Find("Panel2").GetComponent<Animator>();
            animator1.SetBool("Stop2", true);
            DisableClickCanvas();
            return;
        }
        if (FirstComplete)
        {
            numberTurorial++;
            Animator animator1 = GameObject.Find("Panel1").GetComponent<Animator>();
            animator1.SetBool("Stop1", true);
            StartCoroutine(TimeToNew());
            DisableClickCanvas();
        }
    }

    private IEnumerator TimeToNew()
    {
        yield return new WaitForSeconds(10);
        ToStart = true;
    }
    public void CreateOpen()
    {
        if (numberTurorial != 2) return;
        numberTurorial++;
        StartCoroutine(TimeToNew());
    }
    private void EnableClickCanvas()
    {
        canvasGroup.blocksRaycasts = true;    // Разрешить клики
        canvasGroup.interactable = true;      // Разрешить взаимодействие
    }

    private void DisableClickCanvas()
    {
        canvasGroup.blocksRaycasts = false;   // Запретить клики
        canvasGroup.interactable = false;     // Запретить взаимодействие
    }
}
