using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSensor : MonoBehaviour
{
    private float sensorSize=5f;
    private float[] angles=new float[8]{0f, 30f, -30f, 60f, -60f, 90f, -90f, 180f};
    private float[] normalizedSensorResults = new float[8];

    // Update is called once per frame
    void Update()
    {
        CastSensors();
        //GetSensorResults();
    }

    private void CastSensors()
    {
        for(int i=0; i<angles.Length;i++){
            RaycastHit2D hit = new RaycastHit2D();
            hit=Physics2D.Raycast(transform.position, Quaternion.Euler(0f, 0f, angles[i])*transform.TransformDirection(Vector2.up), sensorSize);
            if(hit){
                //Debug.Log(angles[i]+" sensor detectou algo "+hit.collider.tag);
                normalizedSensorResults[i]=(sensorSize-hit.distance)/sensorSize;
            } else {
                normalizedSensorResults[i]=0f;
            }
            //Debug.DrawLine(transform.position, transform.position+Quaternion.Euler(0f, 0f, angles[i])*transform.TransformDirection(Vector2.up)*sensorSize, Color.cyan, 0.5f);
        }
    }

    public float[] GetSensorResults(){
        /*string str="";
        for(int i=0; i<normalizedSensorResults.Length; i++) {
            str+=normalizedSensorResults[i]+" , ";
        }
        Debug.Log(str);*/
        return normalizedSensorResults;
    }
}
