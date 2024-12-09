using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public bool open;
    public float boxLife = 1f;
    public bool waitToClose;
    public GameObject lid;
    public GameObject present;

    public float moveDistance;
    public float moveDuration;

    private void Update()
    {
        //if (waitToClose > 0) waitToClose -= Time.deltaTime;
    }


    public void MoveLid()
    {
        //Moves the lid up if the box is open and down if it is closed
        StartCoroutine(MoveLidCoroutine(open ? Vector3.up * moveDistance : Vector3.down * moveDistance, moveDuration));
        waitToClose = true;
    }

    public void InstantiatePresent()
    {
        GameObject newPresent = Instantiate(present);
        newPresent.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        newPresent.transform.SetParent(lid.transform);
    }

    public IEnumerator RemoveBox(TMP_Text scoreText, int score)
    {
        open = false;
        yield return new WaitForSeconds(boxLife);
        lid.transform.position = new Vector3(lid.transform.position.x, lid.transform.position.y - moveDistance,
            lid.transform.position.z);
        foreach (Transform child in lid.transform)
        {
            Destroy(child.gameObject);
        }
        
        if (scoreText) scoreText.SetText(score.ToString());
        waitToClose = false;
        gameObject.SetActive(false);
    }

    public IEnumerator CloseBox()
    {
        open = false;
        yield return new WaitForSeconds(boxLife);
        MoveLid();
        yield return new WaitForSeconds(moveDuration);
        waitToClose = false;
    }

    private IEnumerator MoveLidCoroutine(Vector3 targetOffset, float duration)
    {
        Vector3 startPosition = lid.transform.position;
        Vector3 targetPosition = startPosition + targetOffset;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            lid.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null; // Wait for the next frame
        }

        lid.transform.position = targetPosition; // Ensure the final position is accurate
    }
}
