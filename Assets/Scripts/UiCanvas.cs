using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiCanvas : MonoBehaviour
{
    public TMP_Text generationText;
    public TMP_Text bestCarnScore;
    public TMP_Text bestHerbScore;

    public void UpdateGenerationText()
    {
        generationText.SetText("Generation: "+SceneController.instance.Generation.ToString());
    }

    public void UpdateBestText()
    {
        bestCarnScore.SetText("Best Carn Score: "+SceneController.instance.BestCarn.GetComponent<Creature>().Score.ToString());
        bestHerbScore.SetText("Best Herb Score: "+SceneController.instance.BestHerb.GetComponent<Creature>().Score.ToString());
    }
}
