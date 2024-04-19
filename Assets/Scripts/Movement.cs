using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float translation, rotation;
    public float moveSpeed=5f;
    private float rotationSpeed=200f;

    public NeuralNetwork neuralNetwork;

    // Update is called once per frame
    //2 outputs needed:
    /*
        forward:    0/1;
        turn: -1/1;
    */
    void Update()
    {
        if(SceneController.PauseSimulation)
            return;

        float[] commands = neuralNetwork.GenerateOutput();
        /////Debug.Log(transform.tag+ "["+commands[0]+ ", " + commands[1]+ "] lenght: "+commands.Length);
        translation = commands[0];
        rotation = commands[1];

        //rotation = Input.GetAxisRaw("Horizontal");
        //translation = Input.GetAxisRaw("Vertical");

        if(translation<0f)
            translation=0f;
    }

    void FixedUpdate()//calculos de fisica
    {
        if(SceneController.PauseSimulation)
            return;
        //translation
        transform.Translate(0f, translation*Time.deltaTime*moveSpeed, 0f);

        //rotation
        transform.Rotate(0f, 0f, -rotation*Time.deltaTime*rotationSpeed);
    }
}
