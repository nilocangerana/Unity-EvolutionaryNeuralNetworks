using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    private readonly float inititalLifeTime=10f;
    private float currentLifeTime;
    private float timerCount=0f;

    private int score=0;
    public int Score{
        get{return score;}
    }

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
            //Destroy(gameObject);
            gameObject.SetActive(false);
            if(transform.tag=="Herb") {
                SceneController.instance.DecrementHerbPopulation();
            } else {
                SceneController.instance.DecrementCarnPopulation();
            }
        }
    }

    public void AddLifeTime()
    {
        currentLifeTime++;
    }

    public void ReduceLifeTime()
    {
        currentLifeTime--;
        if(currentLifeTime<=0f){
            //Destroy(gameObject);
            gameObject.SetActive(false);
            SceneController.instance.DecrementCarnPopulation();
        }
    }

    private void AddScore(int amount)
    {
        score+=amount;
    }

    private void ReduceScore(int amount)
    {
        score-=amount;
        if(score<0){
            score=0;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(transform.tag=="Herb" && col.gameObject.tag=="Food") {
            AddLifeTime();
            AddScore(2);
            Destroy(col.gameObject);
            SceneController.instance.DecrementFoodCount();
        } else if(transform.tag=="Carn" && col.gameObject.tag=="Herb") {
            AddLifeTime();
            AddScore(2);
            col.gameObject.SetActive(false);
            SceneController.instance.UpdateHerbPopulation();
            //Destroy(col.gameObject);
        } else if(transform.tag=="Herb" && col.gameObject.tag=="Carn") {
            col.transform.GetComponent<Creature>().AddLifeTime();
            col.transform.GetComponent<Creature>().AddScore(1);
            ReduceScore(1);
            gameObject.SetActive(false);
            SceneController.instance.UpdateHerbPopulation();
            //Destroy(transform.gameObject);
        } else if(transform.tag=="Carn" && col.gameObject.tag=="Food") {
            ReduceLifeTime();
            ReduceScore(1);
            Destroy(col.gameObject);
            SceneController.instance.DecrementFoodCount();
        }
    }

}
