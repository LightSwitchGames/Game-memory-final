using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;


public class GameManager : MonoBehaviour
{
    public GameObject[] boxes1;
    public GameObject[] boxes2;

    private List<GameObject> openBoxes1 = new();
    private List<GameObject> openBoxes2 = new();

    private GameObject selectedBox1;
    private GameObject selectedBox2;

    public GameObject[] presents;

    private int score1 = 0;
    private int score2 = 0;

    public TMP_Text scoreText1;
    public TMP_Text scoreText2;

    public TMP_Text timerText;
    public int startSeconds;
    private float remainingTime;

    public GameObject playButton;
    public GameObject menuButton;
    public GameObject player1Won;
    public GameObject player2Won;
    public GameObject player1Lost;
    public GameObject player2Lost;
    public GameObject player1Draw;
    public GameObject player2Draw;



    [Header("Inputs")]
    public KeyCode inputLeft1 = KeyCode.A;
    public KeyCode inputRight1 = KeyCode.D;
    public KeyCode inputOpen1 = KeyCode.Space;
    public KeyCode inputLeft2 = KeyCode.LeftArrow;
    public KeyCode inputRight2 = KeyCode.RightArrow;
    public KeyCode inputOpen2 = KeyCode.RightShift;



    private void Awake()
    {
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        AssignBoxes1();
        AssignBoxes2();
        SelectBox1(boxes1[0]);
        SelectBox2(boxes2[0]);

        remainingTime = startSeconds;
        StartCoroutine(StartCountdown());
    }

    private void Update()
    {
        CheckOpenBoxes1();
        CheckOpenBoxes2();
        CheckInputs();
        
        
        if (CheckForActiveBoxes1()) ResetBoxes1();
        if (CheckForActiveBoxes2()) ResetBoxes2();

        if (remainingTime <= 0f)
        {
            if (score1 > score2)
            {
                player1Won.SetActive(true);
                player2Lost.SetActive(true);
            }
            else if (score2 > score1)
            {
                player2Won.SetActive(true);
                player1Lost.SetActive(true);
            }
            else
            {
                player1Draw.SetActive(true);
                player2Draw.SetActive(true);
            }

            playButton.SetActive(true);
            menuButton.SetActive(true);
            Cursor.visible = true;

            Time.timeScale = 0f;
            GetComponent<GameManager>().enabled = false;
        }
    }

    private void SelectBox1(GameObject selectedBox)
    {
        selectedBox1 = selectedBox;
        
        //Set the outline to enabled only for the selected box
        foreach (GameObject box in boxes1)
        {
            box.GetComponent<Outline>().enabled = box == selectedBox;
        }
    }

    private void SelectBox2(GameObject selectedBox)
    {
        selectedBox2 = selectedBox;
        
        //Set the outline to enabled only for the selected box
        foreach (GameObject box in boxes2)
        {
            box.GetComponent<Outline>().enabled = box == selectedBox;
        }
    }

    private void OpenBox1(GameObject box)
    {
        //Check if the box is already open or if there are 2 open boxes
        if (openBoxes1.Count >= 2 || box.GetComponent<BoxController>().waitToClose || !box.activeSelf) return;

        box.GetComponent<BoxController>().open = true;
        box.GetComponent<BoxController>().MoveLid();
        openBoxes1.Add(box);
    }
    
    private void OpenBox2(GameObject box)
    {
        //Check if the box is already open or if there are 2 open box
        if (openBoxes2.Count >= 2 || box.GetComponent<BoxController>().waitToClose || !box.activeSelf) return;

        box.GetComponent<BoxController>().open = true;
        box.GetComponent<BoxController>().MoveLid();
        openBoxes2.Add(box);
    }

    private void CheckOpenBoxes1()
    {
        if (openBoxes1.Count >= 2)
        {
            //Check if the presents have the same tag
            if (openBoxes1[0].GetComponent<BoxController>().present.CompareTag(openBoxes1[1].GetComponent<BoxController>().present.tag))
            {
                score1++;
                StartCoroutine(openBoxes1[0].GetComponent<BoxController>().RemoveBox(scoreText1, score1));
                StartCoroutine(openBoxes1[1].GetComponent<BoxController>().RemoveBox(null, 0));
            }
            else
            {
                StartCoroutine(openBoxes1[0].GetComponent<BoxController>().CloseBox());
                StartCoroutine(openBoxes1[1].GetComponent<BoxController>().CloseBox());
            }
            
            openBoxes1.Clear();
        }
    }
    
    private void CheckOpenBoxes2()
    {
        if (openBoxes2.Count >= 2)
        {
            //Check if the presents have the same tag
            if (openBoxes2[0].GetComponent<BoxController>().present.CompareTag(openBoxes2[1].GetComponent<BoxController>().present.tag))
            {
                score2++;
                StartCoroutine(openBoxes2[0].GetComponent<BoxController>().RemoveBox(scoreText2, score2));
                StartCoroutine(openBoxes2[1].GetComponent<BoxController>().RemoveBox(null, 0));
            }
            else
            {
                StartCoroutine(openBoxes2[0].GetComponent<BoxController>().CloseBox());
                StartCoroutine(openBoxes2[1].GetComponent<BoxController>().CloseBox());
            }

            openBoxes2.Clear();
        }
    }

    private void CheckInputs()
    {
        //Check inputs for left player
        if (Input.GetKeyDown(inputLeft1))
        {
            //Checks if selectedBox is first in the array
            SelectBox1(Array.IndexOf(boxes1, selectedBox1) > 0
                ? boxes1[Array.IndexOf(boxes1, selectedBox1) - 1] 
                : boxes1[^1]);
            //moves on to the next box until it finds an active one
            while (!selectedBox1.activeSelf)
            {
                SelectBox1(Array.IndexOf(boxes1, selectedBox1) > 0
                    ? boxes1[Array.IndexOf(boxes1, selectedBox1) - 1] 
                    : boxes1[^1]);
            }
        }
        else if (Input.GetKeyDown(inputRight1))
        {
            //Checks if selectedBox is last in the array
            SelectBox1(Array.IndexOf(boxes1, selectedBox1) < boxes1.Length -1
                ? boxes1[Array.IndexOf(boxes1, selectedBox1) + 1]
                : boxes1[0]);
            while (!selectedBox1.activeSelf)
            {
                SelectBox1(Array.IndexOf(boxes1, selectedBox1) < boxes1.Length -1
                    ? boxes1[Array.IndexOf(boxes1, selectedBox1) + 1]
                    : boxes1[0]);
            }
        }
        else if (Input.GetKeyDown(inputOpen1))
        {
            OpenBox1(selectedBox1);
        }
        
        
        //Check Inputs for right player
        if (Input.GetKeyDown(inputLeft2))
        {
            //Checks if selectedBox is first in the array
            SelectBox2(Array.IndexOf(boxes2, selectedBox2) > 0
                ? boxes2[Array.IndexOf(boxes2, selectedBox2) - 1] 
                : boxes2[^1]);
            while (!selectedBox2.activeSelf)
            {
                SelectBox2(Array.IndexOf(boxes2, selectedBox2) > 0
                    ? boxes2[Array.IndexOf(boxes2, selectedBox2) - 1] 
                    : boxes2[^1]);
            }
        }
        else if (Input.GetKeyDown(inputRight2))
        {
            //Checks if selectedBox is last in the array
            SelectBox2(Array.IndexOf(boxes2, selectedBox2) < boxes2.Length -1
                ? boxes2[Array.IndexOf(boxes2, selectedBox2) + 1]
                : boxes2[0]);
            while (!selectedBox2.activeSelf)
            {
                SelectBox2(Array.IndexOf(boxes2, selectedBox2) < boxes2.Length -1
                    ? boxes2[Array.IndexOf(boxes2, selectedBox2) + 1]
                    : boxes2[0]);
            }
        }
        else if (Input.GetKeyDown(inputOpen2))
        {
            OpenBox2(selectedBox2);
        }
    }
    
    private void ShufflePresents(GameObject[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1); // Random index between 0 and i (inclusive)

            // Swap elements
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    private void AssignBoxes1()
    {
        ShufflePresents(presents);
        
        for (int i = 0; i < boxes1.Length; i++)
        {
            boxes1[i].GetComponent<BoxController>().present = presents[i];
            boxes1[i].GetComponent<BoxController>().InstantiatePresent();
        }
    }

    private void AssignBoxes2()
    {
        ShufflePresents(presents);
        
        for (int i = 0; i < boxes2.Length; i++)
        {
            boxes2[i].GetComponent<BoxController>().present = presents[i];
            boxes2[i].GetComponent<BoxController>().InstantiatePresent();
        }
    }

    private bool CheckForActiveBoxes1()
    {
        foreach (GameObject box in boxes1)
        {
            if (box.activeSelf) return false;
        }

        return true;
    }

    private bool CheckForActiveBoxes2()
    {
        foreach (GameObject box in boxes2)
        {
            if (box.activeSelf) return false;
        }

        return true;
    }

    private void ResetBoxes1()
    {
        foreach (GameObject box in boxes1)
        {
            box.SetActive(true);
        }
        AssignBoxes1();
    }

    private void ResetBoxes2()
    {
        foreach (GameObject box in boxes2)
        {
            box.SetActive(true);
        }
        AssignBoxes2();
    }

    private IEnumerator StartCountdown()
    {
        while (remainingTime > 0)
        {
            // Convert remaining time to minutes and seconds
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            
            timerText.SetText($"{minutes}:{seconds:D2}");

            yield return new WaitForSeconds(1f);

            remainingTime--;
        }
        
        timerText.SetText("0:00");
    }



}
