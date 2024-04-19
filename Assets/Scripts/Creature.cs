using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    private readonly float inititalLifeTime=10f;
    private float currentLifeTime;
    private float timerCount=0f;

    // Start is called before the first frame update
    void Start()
    {
        currentLifeTime=inititalLifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneController.PauseSimulation)
            return;

        if(timerCount>=1f) {
            timerCount=0f;
            currentLifeTime--;
        } else {
            timerCount+=Time.deltaTime;
        }

        if(currentLifeTime<=0f){
            Destroy(gameObject);
        }
    }

    public void AddLifeTime()
    {
        Debug.Log("added +1 lifetime to "+transform.tag);
        currentLifeTime++;
    }

    public void ReduceLifeTime()
    {
        Debug.Log("reduced -1 lifetime to "+transform.tag);
        currentLifeTime--;
        if(currentLifeTime<=0f){
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(transform.tag=="Herb" && col.gameObject.tag=="Food") {
            AddLifeTime();
            Destroy(col.gameObject);
        } else if(transform.tag=="Carn" && col.gameObject.tag=="Herb") {
            AddLifeTime();
            Destroy(col.gameObject);
        } else if(transform.tag=="Herb" && col.gameObject.tag=="Carn") {
            col.transform.GetComponent<Creature>().AddLifeTime();
            Destroy(transform.gameObject);
        } else if(transform.tag=="Carn" && col.gameObject.tag=="Food") {
            ReduceLifeTime();
            Destroy(col.gameObject);
        }
    }

}
