using UnityEngine;
public class DNA {

    public NeuralNetwork data;
    public float fitness = 0f;
    public float mutationRate = 0.02f;
    
    public DNA(float newMutationRate) {
        mutationRate = newMutationRate;
        // CreateDNA
    }

    public void AddFitness(float value) {
        fitness += value;
        if(fitness < 0) {
            fitness = 0;
        }
    }

    public float GetFitness() {
        return fitness;
    }

    public void FitnessNormalize(float value) {
        fitness /= value;
    }

    public void FillDnaFromParents(DNA dna1, DNA dna2, int selectionType = 0) { // Type 0 = A0 A1 / B2 B3, Type 1 = A0 / B1 / A2 / B3
        data = new NeuralNetwork(dna1.data.GetInputLength(), dna1.data.GetNodesAmount(), dna1.data.GetOutputLength(), dna1.data.GetHiddenSize());
        bool canMutate = true;
        int mid = dna1.data.GetWeights().Length/2;
        /*if(selectionType == 1){
            for(int i = 0; i < dna1.data.Length; i++) {
                if(i%2 == 0){
                    if(Random.Range(0f,1f) < mutationRate && canMutate) {
                        data += System.Convert.ToChar(Random.Range(97,122));
                        canMutate = false;
                    }else{
                        data += dna1.data[i];
                    }
                }else{
                    if(Random.Range(0f,1f) < mutationRate && canMutate) {
                        data += System.Convert.ToChar(Random.Range(97,122));
                        canMutate = false;
                    }else{
                        data += dna2.data[i];
                    }
                }
            }
            return;
        }*/
        if(selectionType == 0){
            for(int i = 0; i < dna1.data.GetWeights().Length; i++) {
                if(i < mid){
                    if(Random.Range(0.00f,1.00f) < mutationRate && canMutate) {
                        data.GetWeights()[i] = new FMatrix(data.GetWeights()[i].rows, data.GetWeights()[i].columns);
                        data.GetWeights()[i].Randomize();
                        //canMutate = false;
                    }else{
                        data.GetWeights()[i] = dna1.data.GetWeights()[i];
                    }
                }else{
                    if(Random.Range(0f,1f) < mutationRate && canMutate) {
                        data.GetWeights()[i] = new FMatrix(data.GetWeights()[i].rows, data.GetWeights()[i].columns);
                        data.GetWeights()[i].Randomize();
                        //canMutate = false;
                    }else{
                        data.GetWeights()[i] = dna2.data.GetWeights()[i];
                    }
                }
            }
            for(int i = 0; i < dna1.data.GetBias().Length; i++) {
                if(i < mid){
                    if(Random.Range(0f,1f) < mutationRate && canMutate) {
                        data.GetBias()[i] = new FMatrix(data.GetBias()[i].rows, data.GetBias()[i].columns);
                        data.GetBias()[i].Randomize();
                        //canMutate = false;
                    }else{
                        data.GetBias()[i] = dna1.data.GetBias()[i];
                    }
                }else{
                    if(Random.Range(0f,1f) < mutationRate && canMutate) {
                        data.GetBias()[i] = new FMatrix(data.GetBias()[i].rows, data.GetBias()[i].columns);
                        data.GetBias()[i].Randomize();
                        //canMutate = false;
                    }else{
                        data.GetBias()[i] = dna2.data.GetBias()[i];
                    }
                }
            }
            return;
        }
        for(int i = 0; i < dna1.data.GetWeights().Length; i++) {
            if(i%2 == 0){
                if(Random.Range(0f,1f) < mutationRate && canMutate) {
                    data.GetWeights()[i] = new FMatrix(data.GetWeights()[i].rows, data.GetWeights()[i].columns);
                    data.GetWeights()[i].Randomize();
                    //canMutate = false;
                }else{
                    data.GetWeights()[i] = dna1.data.GetWeights()[i];
                }
            }else{
                if(Random.Range(0f,1f) < mutationRate && canMutate) {
                    data.GetWeights()[i] = new FMatrix(data.GetWeights()[i].rows, data.GetWeights()[i].columns);
                    data.GetWeights()[i].Randomize();
                    //canMutate = false;
                }else{
                    data.GetWeights()[i] = dna2.data.GetWeights()[i];
                }
            }
        }
        for(int i = 0; i < dna1.data.GetBias().Length; i++) {
            if(i%2 == 0){
                if(Random.Range(0f,1f) < mutationRate && canMutate) {
                    data.GetBias()[i] = new FMatrix(data.GetBias()[i].rows, data.GetBias()[i].columns);
                    data.GetBias()[i].Randomize();
                    //canMutate = false;
                }else{
                    data.GetBias()[i] = dna1.data.GetBias()[i];
                }
            }else{
                if(Random.Range(0f,1f) < mutationRate && canMutate) {
                    data.GetBias()[i] = new FMatrix(data.GetBias()[i].rows, data.GetBias()[i].columns);
                    data.GetBias()[i].Randomize();
                    //canMutate = false;
                }else{
                    data.GetBias()[i] = dna2.data.GetBias()[i];
                }
            }
        }
    }    

}