using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSensor : MonoBehaviour
{
    public const int sensorQtd=24;
    private float[] angles=new float[sensorQtd]{-180f, -165f, -150f, -135f, -120f, -105f, -90f, -75f, -60f, -45f, -30f, -15f, 0f, 15f, 30f, 45f, 60f, 75f, 90f, 105f, 120f, 135f, 150f, 165f};
    //private float[] angles=new float[sensorQtd]{0f, 30f, -30f, 60f, -60f, 90f, -90f, 180f};
    private float sensorSize=6.5f;
    private float[] normalizedSensorResults = new float[sensorQtd*2];
    /*
    Categorical values
    Nada = 0f;
    Food = 0.333f;
    Herb = 0.667f;
    Carn = 1f
    */

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
                //Categorical inputs
                if(hit.transform.tag=="Food") {
                    normalizedSensorResults[i+sensorQtd]=0.333f;
                } else if(hit.transform.tag=="Herb") {
                    normalizedSensorResults[i+sensorQtd]=0.667f;
                } else if(hit.transform.tag=="Carn") {
                    normalizedSensorResults[i+sensorQtd]=1f;
                } else {
                    normalizedSensorResults[i+sensorQtd]=0f;
                }
                
            } else {
                normalizedSensorResults[i]=0f;
                normalizedSensorResults[i+sensorQtd]=0f;
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
