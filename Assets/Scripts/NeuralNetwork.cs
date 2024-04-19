using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    private List<NetworkLayer> neuralNetwork;
    public CreatureSensor creatureSensor;

    void Start()
    {
        //BuildNetworkLayers(new int[3]{4,3,2}, true);
    }

    void Update()
    {
        //PrintNetwork();
    }

    public void BuildNetworkLayers(int[] qtdNeuronsPerLayer, bool isRandomValues)
    {
        neuralNetwork = new List<NetworkLayer>();

        if(isRandomValues) {
            for(int i=0; i<qtdNeuronsPerLayer.Length; i++) {
                if(i==0) {
                    NetworkLayer l1 = new NetworkLayer(qtdNeuronsPerLayer[i], CreatureSensor.sensorQtd*2);
                    l1.GenerateRandomValues();
                    neuralNetwork.Add(l1);
                } else {
                    NetworkLayer l1 = new NetworkLayer(qtdNeuronsPerLayer[i], qtdNeuronsPerLayer[i-1]);
                    l1.GenerateRandomValues();
                    neuralNetwork.Add(l1);
                }
            }
        } else {
            for(int i=0; i<qtdNeuronsPerLayer.Length; i++) {
                if(i==0) {
                    NetworkLayer l1 = new NetworkLayer(qtdNeuronsPerLayer[i], CreatureSensor.sensorQtd);
                    neuralNetwork.Add(l1);
                } else {
                    NetworkLayer l1 = new NetworkLayer(qtdNeuronsPerLayer[i], qtdNeuronsPerLayer[i-1]);
                    neuralNetwork.Add(l1);
                }
            }
        }
    }

    public void SetBestValues(NeuralNetwork best){
        for(int i=0; i<neuralNetwork.Count; i++) {
            for(int k=0; k<neuralNetwork[i].Weights.Length; k++) {
                neuralNetwork[i].SetWeightValue(best.neuralNetwork[i].Weights[k], k);
            }

            for(int k=0; k<neuralNetwork[i].Biases.Length; k++) {
                neuralNetwork[i].SetBiasValue(best.neuralNetwork[i].Biases[k], k);
            }
        }
    }

    public void ApplyMutation(float mutationRate) {
        for(int i=0; i<neuralNetwork.Count; i++) {
            for(int k=0; k<neuralNetwork[i].Weights.Length; k++) {
                float r = Random.Range(0f, 1f);
                if(r<=mutationRate) {
                    float mutateValue = Random.Range(-1f, 1f);
                    neuralNetwork[i].SetWeightValue(mutateValue, k);
                }
            }

            for(int k=0; k<neuralNetwork[i].Biases.Length; k++) {
                float r = Random.Range(0f, 1f);
                if(r<=mutationRate) {
                    float mutateValue = Random.Range(-1f, 1f);
                    neuralNetwork[i].SetBiasValue(mutateValue, k);
                }
            }
        }
    }


    public float[] GenerateOutput()
    {
        float[] r=null;
        float[] commands=null;
        for(int i=0; i<neuralNetwork.Count; i++) {
            if(i==0) {
                r=neuralNetwork[i].ForwardPass(creatureSensor.GetSensorResults() ,false);
            } else if(i==neuralNetwork.Count-1) {
                commands=neuralNetwork[i].ForwardPass(r,true);
            } else {
                r=neuralNetwork[i].ForwardPass(r, false);
            }
        }

        return commands;
    }

    public void PrintNetwork()
    {
        string g = "------Inputs ------- \n";
        float[] inputs=creatureSensor.GetSensorResults();
        for(int i=0; i<inputs.Length; i++) {
            g+="i"+(i+1)+": "+inputs[i]+"\n";
        }
        Debug.Log(g);
        g="";

        for(int i=0; i<neuralNetwork.Count; i++) {
            g = "------Layer "+(i+1)+" ------- \n";
            g+="Weights: \n";
            for(int k=0; k<neuralNetwork[i].Weights.Length; k++) {
                g+="w"+(k+1)+": "+neuralNetwork[i].Weights[k]+"\n";
            }
            g+="Biases: \n";
            for(int k=0; k<neuralNetwork[i].Biases.Length; k++) {
                g+="b"+(k+1)+": "+neuralNetwork[i].Biases[k]+"\n";
            }
            Debug.Log(g);
        }
    }

}
