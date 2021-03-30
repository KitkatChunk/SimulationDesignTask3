using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public GameObject trackObject;

    public float height=0.5f;
    public float desiredDistance=4;
    public float heightDampFactor=0.5f;
    public float rotationDampFactor=0.01f;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotationAndTransform();    
    }
    private float Lerp1D(float v1, float v2, float t)
    {
        // (v-v1)/(v2-v1)=(t-t1)/(t2-t1)=t
        // if t1=0 and t2=1 =>
        // v=t(v2-v1)+v1=v1*(1-t)+v2*t

        return (1 - t) * v1 + t * v2; 
    }
    private Vector2 Lerp2D(Vector2 v1, Vector2 v2, float t)
    {
        // (v-v1)/(v2-v1)=(t-t1)/(t2-t1)=t
        // if t1=0 and t2=1 =>
        // v=t(v2-v1)+v1=v1*(1-t)+v2*t

        return (1 - t) * v1 + t * v2;
    }
    private Vector3 Lerp3D(Vector3 v1, Vector3 v2, float t)
    {
        // (v-v1)/(v2-v1)=(t-t1)/(t2-t1)=t
        // if t1=0 and t2=1 =>
        // v=t(v2-v1)+v1=v1*(1-t)+v2*t

        return (1 - t) * v1 + t * v2;
    }
    private Vector4 Lerp4D(Vector4 v1, Vector4 v2, float t)
    {
        // (v-v1)/(v2-v1)=(t-t1)/(t2-t1)=t
        // if t1=0 and t2=1 =>
        // v=t(v2-v1)+v1=v1*(1-t)+v2*t

        return (1 - t) * v1 + t * v2;
    }

    private void UpdateRotationAndTransform()
    {
        if (trackObject)
        {
            //update
            float desiredRotationAngle = trackObject.transform.eulerAngles.y;
            float desiredHeight = trackObject.transform.position.y + height;

            float rotationAngle = transform.transform.eulerAngles.y;
            float thisHeight = transform.position.y;

            rotationAngle = Lerp1D(rotationAngle, desiredRotationAngle, rotationDampFactor);
            thisHeight = Lerp1D(thisHeight, desiredHeight, heightDampFactor * Time.deltaTime);

            Quaternion currentRotation = Quaternion.Euler(0, rotationAngle, 0);
            Vector3 pos = trackObject.transform.position;
            pos -= currentRotation * Vector3.forward * desiredDistance;
            pos.y = thisHeight;
            transform.position = pos;

            transform.LookAt(trackObject.transform.position);
        }
        else
        {
            Debug.LogError("*** GameCamera.cs Error - No trackObject!!");
        }
    }
}
