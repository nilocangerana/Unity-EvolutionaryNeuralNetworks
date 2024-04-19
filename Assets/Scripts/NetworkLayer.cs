using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkLayer
{
    public int neurons;
    private float[] weights;
    public float[] Weights{
        get{return weights;}
    }
    private float[] biases;
    public float[] Biases{
        get{return biases;}
    }

    public NetworkLayer(int neurons, int qtdInputs){
        this.neurons=neurons;
        weights = new float[neurons*qtdInputs];
        biases = new float[neurons];
    }

    public void GenerateRandomValues()
    {
        for(int i=0; i<weights.Length; i++){
            weights[i]=Random.Range(-1f, 1f);
        }

        for(int i=0; i<biases.Length; i++){
            biases[i]=Random.Range(-1f, 1f);
        }
    }

    private float ReluActivationFunction(float value, float b) {
        if(value<=b) {
            return 0f;
        } else {
            return value;
        }
    }

    private float OutputActivationFunction(float value, float b) {
        if(value<=b) {
            return 0f;
        } else {
            if(value>0f) {
                return 1f;
            } else {
                return -1f;
            }
        }
    }

    public float[] ForwardPass(float[] inputs, bool lastLayer)
    {
        float[] layerOutputs = new float[neurons];
        float total=0f;
        int biasIndex=0;
        float output;

        for(int k=0, i=0; k<=weights.Length; k++) {
            if(k==weights.Length) {
                if(lastLayer) {
                    output = OutputActivationFunction(total, biases[biasIndex]);
                } else {
                    output = ReluActivationFunction(total, biases[biasIndex]);
                }
                layerOutputs[biasIndex] = output;
                //Debug.Log("lastlayer?: "+lastLayer+"   "+layerOutputs[biasIndex]+"  bidx"+biasIndex);
                total=0f;
                i=0;
                biasIndex++;
                return layerOutputs;
            }

            if(i>=inputs.Length) {
                if(lastLayer) {
                    output = OutputActivationFunction(total, biases[biasIndex]);
                } else {
                    output = ReluActivationFunction(total, biases[biasIndex]);
                }
                layerOutputs[biasIndex] = output;
                //Debug.Log("lastlayer?: "+lastLayer+"   "+layerOutputs[biasIndex]+"  bidx"+biasIndex);
                total=0f;
                i=0;
                biasIndex++;
            } else {
                total+=weights[k]*inputs[i];
                i++;
            }
        }

        return layerOutputs;
    }

}
