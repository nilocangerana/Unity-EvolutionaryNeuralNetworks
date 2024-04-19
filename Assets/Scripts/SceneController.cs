using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject FoodPrefab;
    public GameObject HerbPrefab;
    public GameObject CarnPrefab;

    [HideInInspector]
    public static bool PauseSimulation=false;
    
    private int carnivoreMaxPopulation=5;
    private int currentCarnivorePopulation=0;

    private int herbivoreMaxPopulation=10;
    private int currentHerbivorePopulation=0;

    private int maxFoodQtd=20;
    private int currentFoodQtd=0;

    //Screen Bounds
    private Vector2 topLeft = new Vector2(-13f, 7f);
    private Vector2 bottomRight = new Vector2(13f, -7f);

    void Start()
    {
        SpawnObjects();
    }


    private void SpawnObjects()
    {
        for(int i=0; i<maxFoodQtd; i++) {
            float x = Random.Range(topLeft.x, bottomRight.x);
            float y = Random.Range(topLeft.y, bottomRight.y);
            Instantiate(FoodPrefab, new Vector2(x, y), Quaternion.identity);
        }

        for(int i=0; i<herbivoreMaxPopulation; i++) {
            float x = Random.Range(topLeft.x, bottomRight.x);
            float y = Random.Range(topLeft.y, bottomRight.y);
            Instantiate(HerbPrefab, new Vector2(x, y), Quaternion.identity);
        }

        for(int i=0; i<carnivoreMaxPopulation; i++) {
            float x = Random.Range(topLeft.x, bottomRight.x);
            float y = Random.Range(topLeft.y, bottomRight.y);
            Instantiate(CarnPrefab, new Vector2(x, y), Quaternion.identity);
        }
    }

}
