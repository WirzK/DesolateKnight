using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect;

    private float xPosition;
    private float yPosition;
    private float length;
    private float high;

    void Start()
    {
        cam = GameObject.Find("Main Camera");

        length = GetComponent<SpriteRenderer>().bounds.size.x;
        high = GetComponent<SpriteRenderer>().bounds.size.y;
        xPosition = transform.position.x;
        yPosition = transform.position.y;
    }

    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;

        float highMoved = cam.transform.position.y * (1 - parallaxEffect);
        float highToMove = cam.transform.position.y * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);
        transform.position = new Vector3(transform.position.x, yPosition + highToMove);


        if (distanceMoved > xPosition + length)
            xPosition = xPosition + length;
        else if (distanceMoved < xPosition - length)
            xPosition = xPosition - length;

    }
}
