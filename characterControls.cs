using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class characterControls : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Transform chel;
    public float duration = 0.1f;
    public bool chelMoving = false;
    public bool moveInBuffer = false;
    private bool movingLeft = false;
    private bool movingRight = false;
    public bool chelMovingBuffer = false;

    public int currentlane = 2;
    public int nextlane;

    public Sprite MoveSprite;
    public Sprite IdleSprite;

    Coroutine myCoroutine;

    private List<float> laneList = new List<float>() {-4f, -2f, 0f, 2f, 4f};

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        StartCoroutine(frameCompare());

        if (movingLeft)
        {
            spriteRenderer.flipX = false;
            spriteRenderer.sprite = MoveSprite;
            // spriteRenderer.sprite = 
        }
        else if (movingRight)
        {
            spriteRenderer.flipX = true;
            spriteRenderer.sprite = MoveSprite;
        }
        else
        {
            spriteRenderer.sprite = IdleSprite;
        }

        if (Input.GetKeyDown(KeyCode.A) & (chel.transform.position.x >= -2f || movingRight == true))
        {
            if (myCoroutine != null)
            {
                StopCoroutine(myCoroutine);
            }
            
            nextlane = currentlane - 1;
            
            myCoroutine = StartCoroutine(LerpMove(new Vector3(laneList[currentlane], chel.transform.position.y, chel.transform.position.z), new Vector3(laneList[nextlane], chel.transform.position.y, chel.transform.position.z)));
            currentlane = nextlane;
        }

        if (Input.GetKeyDown(KeyCode.D) & (chel.transform.position.x <= 2f || movingLeft == true))
        {
            if (myCoroutine != null)
            {
                StopCoroutine(myCoroutine);
            }
            
            nextlane = currentlane + 1;
    
            myCoroutine = StartCoroutine(LerpMove(new Vector3(laneList[currentlane], chel.transform.position.y, chel.transform.position.z), new Vector3(laneList[nextlane], chel.transform.position.y, chel.transform.position.z)));
            currentlane = nextlane;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(collision.gameObject);
            // spriteRenderer.
        }
    
    IEnumerator LerpMove(Vector3 start, Vector3 end)
    {
        Vector3 warpPos = new Vector3((start.x + end.x) / 2f, chel.transform.position.y, chel.transform.position.z);
        chel.transform.position = warpPos;
        
        float te = 0;
        while (te < duration)
        {
            if (te > duration * 0.5f)
            {
                chelMovingBuffer = true;
            }
            else
            {
                chelMovingBuffer = false;
            }
            float t = te / duration;
            chel.position = Vector3.Lerp(warpPos, end, Mathf.SmoothStep(0f, 1f, t));
            te+= Time.deltaTime;
            yield return null;
        }
        chel.transform.position = end;
        chelMovingBuffer = false;
    }

    IEnumerator frameCompare()
    {
        float lastframe = chel.transform.position.x;
        yield return null;
        if (chel.transform.position.x == lastframe)
        {
            chelMoving = false;
            movingLeft = false;
            movingRight = false;
        }
        else if (chel.position.x < lastframe)
        {
            chelMoving = true;
            movingLeft = true;
            movingRight = false;
        }
        else if (chel.position.x > lastframe)
        {
            chelMoving = true;
            movingLeft = false;
            movingRight = true;
        }
        yield return null;
    }
}