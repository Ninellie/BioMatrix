using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject trackingObject;

    // float moveSpeed = 1;

    public void LookAt2D(Vector3 lookTarget)
    {
        // the direction we want the X axis to face (from this object, towards the target)
        //Vector3 xDirection = (lookTarget - transform.position).normalized;

        // Y axis is 90 degrees away from the X axis
        //Vector3 yDirection = Quaternion.Euler(0, 0, 90) * xDirection;

        // Z should stay facing forward for 2D objects
        //Vector3 zDirection = Vector3.forward;

        // apply the rotation to this object
        //transform.rotation = Quaternion.LookRotation(zDirection, yDirection);




        float angle = Mathf.Atan2(lookTarget.y, lookTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        Debug.Log(angle);

    }





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 trackingPosition = trackingObject.transform.position;

        //        transform.Translate(trackingPosition * moveSpeed * Time.deltaTime);

        LookAt2D(trackingPosition);
    }
}
