using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{

    public int amountOfOutputs = 3;
    public int imageDimension = 10;
    public List<Texture2D> images;
    private List<NNInput> inputs = new List<NNInput>();

    public int hiddenLayers = 1;
    public int hiddenNodes = 5;
    private int countAmount = 0;
    public int maxInterationPerFrame = 50;
    public int maxInteration = 50000;

    private NeuralNetwork nn;

    public int infoWanted = 0;

    void Start(){
        SetDataToUse();
        Debug.Log("Inputs Size: " + inputs.Count);
        nn = new NeuralNetwork((imageDimension*imageDimension), hiddenNodes, amountOfOutputs, hiddenLayers);
        //nn.LoadInfo(trainedData);
    }

    private void Update(){
        //Debug.Log(nn.FeedFoward(inputs[infoWanted].data).Print());
        if(countAmount >= (maxInteration/maxInterationPerFrame)){
            return;
        }
        for (int i = 0; i < maxInterationPerFrame; i++){
            int randomIndex = Random.Range(0, inputs.Count);
            nn.Train(inputs[randomIndex].data, inputs[randomIndex].target);
        }
        countAmount += 1;
        Debug.Log("Training Percentage: " + (((float)(countAmount*maxInterationPerFrame)/(maxInteration + 0.0f)) * 100f) + "%");
        if(countAmount == (maxInteration/maxInterationPerFrame)){
            /*trainedData.BIAS = nn.GetBias();
            trainedData.WEIGHTS = nn.GetWeights();*/
            Debug.Log(nn.FeedFoward(inputs[0].data).Print());
            Debug.Log(nn.FeedFoward(inputs[10].data).Print());
            Debug.Log(nn.FeedFoward(inputs[20].data).Print());
            Debug.Log(nn.FeedFoward(inputs[30].data).Print());
            Debug.Log(nn.FeedFoward(inputs[40].data).Print());
            Debug.Log(nn.FeedFoward(inputs[50].data).Print());
            Debug.Log(nn.FeedFoward(inputs[60].data).Print());
            Debug.Log(nn.FeedFoward(inputs[70].data).Print());
            Debug.Log(nn.FeedFoward(inputs[80].data).Print());
            Debug.Log(nn.FeedFoward(inputs[90].data).Print());
            /*for (int i = 0; i < amountOfOutputs; i++)
            {
                Debug.Log(nn.FeedFoward(inputs[i*amountOfOutputs].data).Print());
            }*/
        }
    }
    
    public void SetDataToUse(){
        //Debug.Log("HI<<<<<<<<<<<<<<<<<<<<<");
        for(int i = 0; i < images.Count; i++){
            float[] target = new float[amountOfOutputs];
            //string s = "";
            for(int j = 0; j < amountOfOutputs; j++){
                if(j == ((int)(i / (images.Count / amountOfOutputs)))){
                    target[j] = 1;
                }else{
                    target[j] = 0;
                }
                //s += target[j];
            }
            //Debug.Log(s);
            inputs.Add(new NNInput(NNInput.ImageToArray(images[i]), target));
        }
        //Debug.Log("Bye<<<<<<<<<<<<<<<<<<<<<");
    }


}


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{

    private float[] i0 = new float[] { 0, 1 };
    private float[] i1 = new float[] { 1, 0 };
    private float[] i2 = new float[] { 1, 1 };
    private float[] i3 = new float[] { 0, 0 };
    private float[] target0 = new float[] { 1 };
    private float[] target1 = new float[] { 1 };
    private float[] target2 = new float[] { 0 };
    private float[] target3 = new float[] { 0 };

    void Start()
    {
        int hidden_layers = 2;
        NeuralNetwork nn = new NeuralNetwork(2, 5, 1, hidden_layers);

        for (int i = 0; i < 50000; i++)
        {
            int r = Random.Range(0, 10) % 4;
            if (r == 0)
            {
                nn.Train(i0, target0);
            }
            else if (r == 1)
            {
                nn.Train(i1, target1);
            }
            else if (r == 2)
            {
                nn.Train(i2, target2);
            }
            else if (r == 3)
            {
                nn.Train(i3, target3);
            }
        }

        Debug.Log(nn.FeedFoward(i0).Print());
        Debug.Log(nn.FeedFoward(i1).Print());
        Debug.Log(nn.FeedFoward(i2).Print());
        Debug.Log(nn.FeedFoward(i3).Print());
    }
}
*/