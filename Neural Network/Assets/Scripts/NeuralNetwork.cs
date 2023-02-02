public class NeuralNetwork
{
    private int input_nodes;
    private int hidden_layers;
    private int hidden_nodes;
    private int output_nodes;

    private FMatrix[] HIDDEN;
    private FMatrix[] WEIGHTS;
    private FMatrix[] BIAS;

    private float learning_rate = 0.1f;

    public NeuralNetwork(int inputsNodesAmount, int hiddenNodesAmount, int outputNodesAmount, int hiddenLayersAmount = 1){
        input_nodes = inputsNodesAmount;
        hidden_nodes = hiddenNodesAmount;
        output_nodes = outputNodesAmount;
        hidden_layers = hiddenLayersAmount;

        WEIGHTS = new FMatrix[hidden_layers+1]; // + 1 to add the output layer
        WEIGHTS[0] = new FMatrix(hidden_nodes, input_nodes);
        for(int i = 1; i < hidden_layers; i++){
            WEIGHTS[i] = new FMatrix(hidden_nodes, hidden_nodes);    
        }
        WEIGHTS[hidden_layers] = new FMatrix(output_nodes, hidden_nodes);
        
        BIAS = new FMatrix[hidden_layers+1];
        for(int i = 0; i < hidden_layers; i++){
            BIAS[i] = new FMatrix(hidden_nodes, 1);
        }
        BIAS[hidden_layers] = new FMatrix(output_nodes, 1);

        for(int i = 0; i < hidden_layers; i++){
            WEIGHTS[i].Randomize();
            BIAS[i].Randomize();
        }
    }

    /*public void RandomizeItAll() {
        for(int i = 0; i < hidden_layers; i++){
            WEIGHTS[i].Randomize();
            BIAS[i].Randomize();
        }
    }*/

    public FMatrix FeedFoward(float[] inputsArray){
        FMatrix inputs = FMatrix.Array2Matrix(inputsArray);
        HIDDEN = new FMatrix[hidden_layers];
        HIDDEN[0] = FMatrix.NewByProduct(WEIGHTS[0], inputs, "FeedFoward Hidden[0]");
        HIDDEN[0].PlusM(BIAS[0]);
        HIDDEN[0].SigmoidMap();
        for(int i = 1; i < hidden_layers; i++){
            HIDDEN[i] = FMatrix.NewByProduct(WEIGHTS[i], HIDDEN[i-1], "FeedFoward For");
            HIDDEN[i].PlusM(BIAS[i]);
            HIDDEN[i].SigmoidMap();
        }
        FMatrix outputs = FMatrix.NewByProduct(WEIGHTS[hidden_layers], HIDDEN[hidden_layers-1], "FeedForward Output");
        outputs.PlusM(BIAS[hidden_layers-1]);
        outputs.SigmoidMap();
        return outputs;
    }

    public void Train(float[] inputsArray, float[] targetsArray){

        FMatrix inputs = FMatrix.Array2Matrix(inputsArray);
        FMatrix outputs = FeedFoward(inputsArray);
        FMatrix targets = FMatrix.Array2Matrix(targetsArray);
        
        FMatrix output_errors = FMatrix.NewByMinus(targets, outputs);
        FMatrix outputGradient = FMatrix.NewGradientM(outputs); // Output -> Gradiente
        outputGradient.ProductM(output_errors); // -
        outputGradient.ProductE(learning_rate); // -->
        FMatrix hidden_transpose = FMatrix.NewByTranspose(HIDDEN[hidden_layers-1]); 
        FMatrix weights_delta = FMatrix.NewByProduct(outputGradient, hidden_transpose, "outputGradient");
        WEIGHTS[hidden_layers].PlusM(weights_delta);
        BIAS[hidden_layers].PlusM(outputGradient);

        for(int i = hidden_layers-1; i > 0; i--){
            FMatrix weights_transpose = FMatrix.NewByTranspose(WEIGHTS[hidden_layers]);
            FMatrix h_errors = FMatrix.NewByProduct(weights_transpose, output_errors, "hidden_erros do for");
            FMatrix hiddenGradient = FMatrix.NewGradientM(HIDDEN[i]); // Hidden -> Gradient
            hiddenGradient.ProductM(h_errors); // -
            hiddenGradient.ProductE(learning_rate); // -->
            FMatrix h_transpose = FMatrix.NewByTranspose(HIDDEN[i-1]);
            FMatrix weights_deltas = FMatrix.NewByProduct(hiddenGradient, h_transpose, "weights_delta do for");
            WEIGHTS[i].PlusM(weights_deltas);
            BIAS[i].PlusM(hiddenGradient);
        }

        FMatrix weights_ho_transpose = FMatrix.NewByTranspose(WEIGHTS[hidden_layers]);
        FMatrix hidden_errors = FMatrix.NewByProduct(weights_ho_transpose, output_errors, "hidden_erros");
        FMatrix inputGradient = FMatrix.NewGradientM(HIDDEN[0]); // Inputs -> Gradiente
        inputGradient.ProductM(hidden_errors); // -
        inputGradient.ProductE(learning_rate); // -->
        FMatrix inputs_transpose = FMatrix.NewByTranspose(inputs);
        FMatrix weights_ih_deltas = FMatrix.NewByProduct(inputGradient, inputs_transpose, "weights_hi_delta");
        WEIGHTS[0].PlusM(weights_ih_deltas);
        BIAS[0].PlusM(inputGradient);
    }

    public FMatrix[] GetHidden() {
        return HIDDEN;
    }

    public FMatrix[] GetBias() {
        return BIAS;
    }

    public FMatrix[] GetWeights() {
        return WEIGHTS;
    }

    public void SetBias(FMatrix[] newBias) {
        BIAS = newBias;
    }

    public void SetWeights(FMatrix[] newWeigths) {
        WEIGHTS = newWeigths;
    }

    public int GetInputLength() {
        return input_nodes;
    }

    public int GetHiddenSize() {
        return hidden_layers;
    }

    public int GetNodesAmount() {
        return hidden_nodes;
    }

    public int GetOutputLength() {
        return output_nodes;
    }

}