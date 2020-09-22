using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//simple script to make text have the effect of popping out.
public class TextPop : MonoBehaviour
{
    public int startSize;
    public int maxPopSize;
    public float popSpeed;

    Text popText;

    bool popping = false;
    bool reachedPeak = false;

    void Start() { popText = this.GetComponent<Text>(); }

    // Update is called once per frame
    void Update()
    {
        if (popping)
        {
            if (!reachedPeak)
            {
                //incline
                popText.fontSize += (int) (popSpeed * Time.deltaTime);

                //check if the target was hit
                if(popText.fontSize >= maxPopSize)
                {
                    popText.fontSize = maxPopSize;
                    reachedPeak = true;
                }
            }
            else
            {
                //decline
                popText.fontSize -= (int)(popSpeed * Time.deltaTime);

                //check if the target was hit
                if (popText.fontSize <= startSize)
                {
                    popText.fontSize = startSize;
                    popping = false;
                }
            }
        }
    }


    public void pop()
    {
        popping = true;
        reachedPeak = false;
    }
}
