using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject FoodPrefab;
    public GameObject HerbPrefab;
    public GameObject CarnPrefab;
    public UiCanvas uiCanvas;

    [HideInInspector]
    public static bool PauseSimulation=false;
    
    private readonly int carnivoreMaxPopulation=20;
    private int currentCarnivorePopulation=0;
    public Transform carnPopTransform;

    private readonly int herbivoreMaxPopulation=40;
    private int currentHerbivorePopulation=0;
    public Transform herbPopTransform;

    private readonly int maxFoodQtd=70;
    private int currentFoodQtd=0;
    public Transform foodPopTransform;

    //Screen Bounds
    private Vector2 topLeftFood = new(-11f, 6.74f);
    private Vector2 bottomRightFood = new(-7f, -7f);
    private Vector2 topLeftHerb = new(-6f, 6.74f);
    private Vector2 bottomRightHerb = new(2f, -7f);
    private Vector2 topLeftCarn = new(5.5f, 6.74f);
    private Vector2 bottomRightCarn = new(11f, -7f);

    public static SceneController instance;

    private GameObject bestCarn;
    public GameObject BestCarn{
        get{return bestCarn;}
    }
    private GameObject bestHerb;
    public GameObject BestHerb{
        get{return bestHerb;}
    }
    private int generation=0;
    public int Generation{
        get{return generation;}
    }

    private int[] networkLayersArray = new int[3]{5,4,2};
    private float mutationRate=0.2f;

    private bool getBestFlag=false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //SpawnObjects();
        SpawnNewGeneration(true, true);
    }

    void Update()
    {
        if(currentCarnivorePopulation==0 && currentHerbivorePopulation==0 && !getBestFlag)
        {
            GetBestHerbCarn();
            DestroyWorstCreatures();
            return;
        }

        if(getBestFlag) {
            if(bestCarn.GetComponent<Creature>().Score==0 && bestHerb.GetComponent<Creature>().Score==0)
                SpawnNewGeneration(true, true);
            else if(bestCarn.GetComponent<Creature>().Score!=0 && bestHerb.GetComponent<Creature>().Score==0)
                SpawnNewGeneration(false, true);
            else if(bestCarn.GetComponent<Creature>().Score==0 && bestHerb.GetComponent<Creature>().Score!=0)
                SpawnNewGeneration(true, false);
            else if(bestCarn.GetComponent<Creature>().Score!=0 && bestHerb.GetComponent<Creature>().Score!=0)
                SpawnNewGeneration(false, false);

            bestCarn.GetComponent<NeuralNetwork>().PrintNetwork();
            carnPopTransform.GetChild(2).GetComponent<NeuralNetwork>().PrintNetwork();
        }
    }

    /*private void SpawnObjects()
    {
        for(int i=0; i<maxFoodQtd; i++) {
            float x = Random.Range(topLeft.x, bottomRight.x);
            float y = Random.Range(topLeft.y, bottomRight.y);
            GameObject f = Instantiate(FoodPrefab, new Vector2(x, y), Quaternion.identity);
            f.transform.SetParent(foodPopTransform);
        }

        for(int i=0; i<herbivoreMaxPopulation; i++) {
            float x = Random.Range(topLeft.x, bottomRight.x);
            float y = Random.Range(topLeft.y, bottomRight.y);
            GameObject h = Instantiate(HerbPrefab, new Vector2(x, y), Quaternion.identity);
            h.GetComponent<NeuralNetwork>().BuildNetworkLayers(new int[3]{4,3,2}, true);
            h.transform.SetParent(herbPopTransform);
        }

        for(int i=0; i<carnivoreMaxPopulation; i++) {
            float x = Random.Range(topLeft.x, bottomRight.x);
            float y = Random.Range(topLeft.y, bottomRight.y);
            GameObject c = Instantiate(CarnPrefab, new Vector2(x, y), Quaternion.identity);
            c.GetComponent<NeuralNetwork>().BuildNetworkLayers(new int[3]{4,3,2}, true);
            c.transform.SetParent(carnPopTransform);
        }

        currentFoodQtd=foodPopTransform.childCount;
        currentHerbivorePopulation=herbPopTransform.childCount;
        currentCarnivorePopulation=carnPopTransform.childCount;
    }*/

    private void SpawnNewGeneration(bool generateRandomCarn, bool generateRandomHerb)
    {
        for(int i=0; i<maxFoodQtd; i++) {
            float x = Random.Range(topLeftFood.x, bottomRightFood.x);
            float y = Random.Range(topLeftFood.y, bottomRightFood.y);
            GameObject f = Instantiate(FoodPrefab, new Vector2(x, y), Quaternion.identity);
            f.transform.SetParent(foodPopTransform);
        }

        for(int i=0; i<herbivoreMaxPopulation; i++) {
            float x = Random.Range(topLeftHerb.x, bottomRightHerb.x);
            float y = Random.Range(topLeftHerb.y, bottomRightHerb.y);
            GameObject h = Instantiate(HerbPrefab, new Vector2(x, y), Quaternion.identity);
            NeuralNetwork hnn=h.GetComponent<NeuralNetwork>();
            hnn.BuildNetworkLayers(networkLayersArray, generateRandomHerb);
            if(!generateRandomHerb) {
                hnn.SetBestValues(bestHerb.GetComponent<NeuralNetwork>());
                hnn.ApplyMutation(mutationRate);
            }
            h.transform.SetParent(herbPopTransform);
        }

        for(int i=0; i<carnivoreMaxPopulation; i++) {
            float x = Random.Range(topLeftCarn.x, bottomRightCarn.x);
            float y = Random.Range(topLeftCarn.y, bottomRightCarn.y);
            GameObject c = Instantiate(CarnPrefab, new Vector2(x, y), Quaternion.identity);
            NeuralNetwork cnn=c.GetComponent<NeuralNetwork>();
            cnn.BuildNetworkLayers(networkLayersArray, generateRandomCarn);
            if(!generateRandomCarn) {
                cnn.SetBestValues(bestCarn.GetComponent<NeuralNetwork>());
                cnn.ApplyMutation(mutationRate);
            }
            c.transform.SetParent(carnPopTransform);
        }

        currentFoodQtd=foodPopTransform.childCount;
        currentHerbivorePopulation=herbPopTransform.childCount;
        currentCarnivorePopulation=carnPopTransform.childCount;
        generation++;
        uiCanvas.UpdateGenerationText();
        getBestFlag=false;
    }

    public void UpdateFoodCount()
    {
        currentFoodQtd=foodPopTransform.childCount;
    }

    public void UpdateHerbPopulation()
    {
        int x=0;
        for(int i=0; i<herbPopTransform.childCount; i++){
            if(herbPopTransform.GetChild(i).gameObject.activeInHierarchy){
                x++;
            }
        }
        currentHerbivorePopulation=x;
    }

    public void UpdateCarnPopulation()
    {
        int x=0;
        for(int i=0; i<carnPopTransform.childCount; i++){
            if(carnPopTransform.GetChild(i).gameObject.activeInHierarchy){
                x++;
            }
        }
        currentCarnivorePopulation=x;
    }

    public void DecrementFoodCount()
    {
        currentFoodQtd--;
    }

    public void DecrementHerbPopulation()
    {
        currentHerbivorePopulation--;
    }

    public void DecrementCarnPopulation()
    {
        currentCarnivorePopulation--;
    }

    private void GetBestHerbCarn()
    {
        int bestScore=0;
        int bestIdx=-1;
        if(bestHerb!=null) {
            bestScore=bestHerb.GetComponent<Creature>().Score;
        } else {
            bestScore=0;
        }

        for(int i=0; i<herbPopTransform.childCount; i++)
        {
            Creature c = herbPopTransform.GetChild(i).GetComponent<Creature>();
            if(bestScore<=c.Score){
                bestScore=c.Score;
                bestIdx=i;
            }
        }
        if(bestIdx!=-1)
            bestHerb=herbPopTransform.GetChild(bestIdx).gameObject;

        if(bestCarn!=null) {
            bestScore=bestCarn.GetComponent<Creature>().Score;
        } else {
            bestScore=0;
        }
        bestIdx=-1;
        for(int i=0; i<carnPopTransform.childCount; i++)
        {
            Creature c = carnPopTransform.GetChild(i).GetComponent<Creature>();
            if(bestScore<=c.Score){
                bestScore=c.Score;
                bestIdx=i;
            }
        }
        if(bestIdx!=-1)
            bestCarn=carnPopTransform.GetChild(bestIdx).gameObject;

        bestHerb.transform.SetParent(null);
        bestHerb.name = "Best Herb Gen "+generation;
        bestCarn.transform.SetParent(null);
        bestCarn.name = "Best Carn Gen "+generation;
        getBestFlag=true;
        uiCanvas.UpdateBestText();
    }

    private void DestroyWorstCreatures()
    {
        foreach(Transform child in herbPopTransform)
        {
            Destroy(child.gameObject);
        }

        foreach(Transform child in carnPopTransform)
        {
            Destroy(child.gameObject);
        }

        foreach(Transform child in foodPopTransform)
        {
            Destroy(child.gameObject);
        }
    }

}
