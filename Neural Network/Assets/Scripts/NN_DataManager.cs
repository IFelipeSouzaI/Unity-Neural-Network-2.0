using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NN_DataManager : MonoBehaviour
{
    
    public string fileName = "defaultNN";
    public TextAsset namesFile;
    public void SaveNeuralNetwork(int inputsNodesAmount, int hiddenNodesAmount, int outputNodesAmount, int hiddenLayersAmount, FMatrix[] bias, FMatrix[] weights) {
        string path = Application.dataPath + "/" + fileName + ".txt";
        if(File.Exists(path)) {
            Debug.Log("Arquivo com mesmo já existe");
        }
        
        File.WriteAllText(path, "");
        File.AppendAllText(path, (inputsNodesAmount + "\n"));
        File.AppendAllText(path, (hiddenNodesAmount + "\n"));
        File.AppendAllText(path, (outputNodesAmount + "\n"));
        File.AppendAllText(path, (hiddenLayersAmount + "\n"));
        File.AppendAllText(path, "#BIA\n");
        File.AppendAllText(path, bias.Length + "\n");
        for(int i = 0; i < bias.Length; i++){
            WriteMatrixOnFile(path, bias[i]);
        }
        File.AppendAllText(path, "#WEI\n");
        File.AppendAllText(path, weights.Length + "\n");
        for(int i = 0; i < weights.Length; i++){
            WriteMatrixOnFile(path, weights[i]);
        }
    }

    private void WriteMatrixOnFile(string path, FMatrix matrix) {
        File.AppendAllText(path, "" + matrix.rows + "\n");
        File.AppendAllText(path, "" + matrix.columns + "\n");
        for(int i = 0; i < matrix.rows; i++) {
            string rowString = "";
            for(int j = 0; j < matrix.columns; j++) {
                rowString += matrix.data[i,j];
                if(j == matrix.columns - 1) {
                    rowString += "&\n";
                } else {
                    rowString += "&";
                }
            }
            File.AppendAllText(path, rowString);
        }
        File.AppendAllText(path, "#End\n");
    }

    public NeuralNetwork ReadFromFile() {
        //string filePath = Application.dataPath + "/" + fileName + ".txt";;
        StringReader read = new StringReader(namesFile.text);
        int inputs = int.Parse(read.ReadLine());
        int hiddenNodes = int.Parse(read.ReadLine());
        int outputs = int.Parse(read.ReadLine());
        int hiddenLayers = int.Parse(read.ReadLine());
        NeuralNetwork newNN = new NeuralNetwork(inputs, hiddenNodes, outputs, hiddenLayers);
		string line = read.ReadLine();
		while(line != null){
			if(line == "#BIA") {
                int biasAmount = int.Parse(read.ReadLine());
                FMatrix[] bias = new FMatrix[biasAmount];
                for(int i = 0; i < biasAmount; i++){
                    int rows = int.Parse(read.ReadLine());
                    int columns = int.Parse(read.ReadLine());
                    FMatrix biasIn = new FMatrix(rows, columns);
                    string toAdd = "";
                    string currentRow = read.ReadLine();
                    for(int m = 0; m < rows; m++){
                        int rowIndex = 0;
                        int n = 0;
                        while(rowIndex < currentRow.Length) {
                            if(currentRow[rowIndex] == '&' || currentRow[rowIndex] == '\n') {
                                biasIn.data[m, n] = float.Parse(toAdd);
                                n += 1;
                                toAdd = "";
                            } else {
                                toAdd += currentRow[rowIndex];
                            }
                            rowIndex += 1;
                        }
                        currentRow = read.ReadLine();
                    }
                    bias[i] = biasIn;
                }
                newNN.SetBias(bias);
                line = read.ReadLine();
            }else if(line == "#WEI") {
                int weightsAmount = int.Parse(read.ReadLine());
                FMatrix[] weights = new FMatrix[weightsAmount];
                for(int i = 0; i < weightsAmount; i++){
                    int rows = int.Parse(read.ReadLine());
                    int columns = int.Parse(read.ReadLine());
                    FMatrix weightsIn = new FMatrix(rows, columns);
                    string toAdd = "";
                    string currentRow = read.ReadLine();
                    for(int m = 0; m < rows; m++){
                        int n = 0;
                        int rowIndex = 0;
                        while(rowIndex < currentRow.Length) {
                            if(currentRow[rowIndex] == '&' || currentRow[rowIndex] == '\n') {
                                weightsIn.data[m, n] = float.Parse(toAdd);
                                n += 1;
                                toAdd = "";
                            } else {
                                toAdd += currentRow[rowIndex];
                            }
                            rowIndex += 1;
                        }
                        currentRow = read.ReadLine();
                    }
                    weights[i] = weightsIn;
                }
                newNN.SetWeights(weights);
                line = read.ReadLine();
            }
		}
        read.Close();
        return newNN;
    }

}
