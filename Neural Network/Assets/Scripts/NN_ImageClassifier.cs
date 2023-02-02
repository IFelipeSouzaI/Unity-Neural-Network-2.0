using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NN_ImageClassifier : MonoBehaviour
{

    [Header("NN Classifier")]
    public Texture2D imageToClassify;

    [Header("Images To Train")]
    public string imagesPath;
    public List<Texture2D> images;
    private int imageDimension;
    private List<string> namesToClassifier = new List<string>();
    
    [Header("NN Options")]
    public int hiddenLayers = 1;
    public int hiddenNodes = 5;
    private int outputs = 3;
    public int maxInterationPerFrame = 50;
    public int maxInteration = 50000;
    
    // Debug
    private int countAmount = 0;

    // Things
    private NeuralNetwork nn;
    private List<NNInput> inputs = new List<NNInput>();
    private NN_DataManager dataManager;
    private bool process = false;

    private void Start() {
        dataManager = this.gameObject.GetComponent<NN_DataManager>();
        //NNInitialize();
    }

    private void Update() {
        if(process) {
            Process();
        }
    }

    [ContextMenu("NN Start")]
    private void NNInitialize() {
        object[] loadedImages = Resources.LoadAll(imagesPath,typeof(Texture2D)) ;
        for(int i = 0; i < loadedImages.Length; i++){
            images.Add((Texture2D)loadedImages[i]);
            string imageName = images[i].name.Substring(0,3);
            if(!CheckIfNameExists(imageName)) {
                namesToClassifier.Add(imageName);
            }
        }
        outputs = namesToClassifier.Count;
        imageDimension = images[0].width;
        SetDataToUse();
        nn = new NeuralNetwork((imageDimension*imageDimension), hiddenNodes, outputs, hiddenLayers);
    }

    private bool CheckIfNameExists(string nameToChek) {
        for(int i = 0; i < namesToClassifier.Count; i++) {
            if(nameToChek == namesToClassifier[i]) {
                return true;
            }
        }
        return false;
    }

    [ContextMenu("NN Process")]
    private void Train() {
        Debug.Log("Started");
        process = true;
    }

    private void Process() {
        if(countAmount >= (maxInteration/maxInterationPerFrame)){
            Debug.Log("Finished");
            dataManager.SaveNeuralNetwork(nn.GetInputLength(), nn.GetNodesAmount(), nn.GetOutputLength(), nn.GetHiddenSize(), nn.GetBias(), nn.GetWeights());
            process = false;
            return;
        }
        for (int i = 0; i < maxInterationPerFrame; i++){
            int randomIndex = Random.Range(0, inputs.Count);
            nn.Train(inputs[randomIndex].data, inputs[randomIndex].target);
        }
        countAmount += 1;
        //Debug.Log("Training Percentage: " + (((float)(countAmount*maxInterationPerFrame)/(maxInteration + 0.0f)) * 100f) + "%");
    }

    [ContextMenu("NN Classify")]
    private void Classify() {
        Debug.Log(nn.FeedFoward(NNInput.ImageToArray(imageToClassify)).Print());
    }

    [ContextMenu("NN Load")]
    public void LoadNN() {
        nn = dataManager.ReadFromFile();
    }

    public void SetDataToUse(){
        for(int i = 0; i < images.Count; i++){
            float[] target = new float[outputs];
            for(int j = 0; j < outputs; j++){
                if(images[i].name.Substring(0,3) == namesToClassifier[j]){
                    target[j] = 1;
                }else{
                    target[j] = 0;
                }
            }
            inputs.Add(new NNInput(NNInput.ImageToArray(images[i]), target));
        }
    }

}
