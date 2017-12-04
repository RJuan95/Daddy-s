using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceAnim : MonoBehaviour {

    public string sliceDir;
    public int numSegments;
    public float lineLength;
    Vector3 start;
    Vector3 finish;
    Transform parent;
    LineRenderer line;
    int currIdx;
    bool lineDrawn;

	// Use this for initialization
	void Start () {
        line = GetComponent<LineRenderer>();
        parent = transform.parent;
        line.positionCount = numSegments;
        initializeLine();
        lineDrawn = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!lineDrawn && currIdx < numSegments)
            animate(currIdx++);
        else if (!lineDrawn && currIdx >= numSegments)
        {
            currIdx = 0;
            lineDrawn = true;
        }

        if (lineDrawn && currIdx < numSegments)
        {
            line.SetPosition(currIdx, finish);
            currIdx++;
        }
        else if (lineDrawn && currIdx >= numSegments)
            Destroy(gameObject);
	}

    void animate(int currIdx)
    {
        Vector3 nextPos = start + (finish - start) / numSegments * currIdx;
        line.SetPosition(currIdx, nextPos);
    }

    void initializeLine()
    {
        switch (sliceDir)
        {
            case "up":
                start = Vector3.down * lineLength;
                finish = Vector3.up * lineLength;
                break;
            case "down":
                start = Vector3.up * lineLength;
                finish = Vector3.down * lineLength;
                break;
            case "left":
                start = Vector3.right * lineLength;
                finish = Vector3.left * lineLength;
                break;
            case "right":
                start = Vector3.left * lineLength;
                finish = Vector3.right * lineLength;
                break;
            default:
                break;
        }

        for (int i=0; i<numSegments; i++)
        {
            line.SetPosition(i, start);
        }
    }



}
