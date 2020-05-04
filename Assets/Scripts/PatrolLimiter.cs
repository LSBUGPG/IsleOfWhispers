using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLimiter : MonoBehaviour
{
    public float radius;
    
    Vector3 movePoint;

    void Start()
    {
        movePoint = generatePoint();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, movePoint, (2 *Time.deltaTime));

        if(transform.position == movePoint)
        {
            movePoint = generatePoint();
        }
    }

    Vector3 generatePoint()
    {
        float x = Random.Range(-radius, radius);
        float y = Mathf.Sqrt((radius * radius) - (x * x));
        y = Random.Range(-y, y);
        Vector3 mov2 = new Vector3(x, transform.position.y, y);
        return mov2;
    }
}
