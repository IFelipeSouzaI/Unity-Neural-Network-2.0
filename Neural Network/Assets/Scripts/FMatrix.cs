using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FMatrix
{
    public int rows;
    public int columns;
    public float[,] data = new float[100,100];
    public Vector2 dimension;

    public FMatrix(int r, int c){
        rows = r;
        columns = c;
        dimension = new Vector2(r,c);
        for(int i = 0; i < r; i++){
            for(int j = 0; j < c; j++){
                data[i,j] = 0;
            }
        }
    }

    public static FMatrix Array2Matrix(float[] array){
        FMatrix m = new FMatrix(array.Length, 1);
        for(int i = 0; i < array.Length; i++){
            m.data[i,0] = array[i];
        }
        return m;
    }
    public static float[] Matrix2Array(FMatrix M){
        float[] a = new float[M.rows*M.columns];
        int index = 0;
        for(int i = 0; i < M.rows; i++){
            for(int j = 0; j < M.columns; j++){
                a[index] = M.data[i,j];
                index += 1;
            }
        }
        return a;
    }

    public static FMatrix NewByMinus(FMatrix M1, FMatrix M2){
        FMatrix m = new FMatrix(M1.rows, M1.columns);
        for(int i = 0; i < m.rows; i++){
            for(int j = 0; j < m.columns; j++){
                m.data[i,j] = M1.data[i,j] - M2.data[i,j];
            }
        }
        return m;
    }
    public static FMatrix NewByPlus(FMatrix M1, FMatrix M2){
        FMatrix m = new FMatrix(M1.rows, M1.columns);
        for(int i = 0; i < m.rows; i++){
            for(int j = 0; j < m.columns; j++){
                m.data[i,j] = M1.data[i,j] + M2.data[i,j];
            }
        }
        return m;
    }
    public static FMatrix NewByProduct(FMatrix M1, FMatrix M2, string name = ""){
        FMatrix m = new FMatrix(M1.rows, M2.columns);
        if(M1.columns != M2.rows){
            Debug.LogError("Houston We Have a Problem in: " + name);
        }
        for(int i = 0; i < m.rows; i++){
            for(int j = 0; j < m.columns; j++){
                float sum = 0;
                for(int k = 0; k < M1.columns; k++){
                    sum += M1.data[i,k] * M2.data[k,j];
                }
                m.data[i,j] = sum;
            }
        }
        return m;
    }
    
    public static FMatrix NewByTranspose(FMatrix M){
        FMatrix m = new FMatrix(M.columns, M.rows);
        for(int i = 0; i < m.rows; i++){
            for(int j = 0; j < m.columns; j++){
                m.data[i,j] = M.data[j,i];
            }
        }
        return m;
    }
    
    public static FMatrix NewGradientM(FMatrix M){
        FMatrix gradient = new FMatrix(M.rows, M.columns);
        for(int i = 0; i < gradient.rows; i++){
            for(int j = 0; j < gradient.columns; j++){
                float aux = M.data[i,j];
                gradient.data[i,j] = (aux*(1-aux));
            }
        }
        return gradient;
    }

    private float Sigmoid(float value){
        return (1f/(1f + Mathf.Exp(-value)));
    }

    public void SigmoidMap(){
        for(int i = 0; i < rows; i++){
            for(int j = 0; j < columns; j++){
                float aux = data[i,j];
                data[i,j] = Sigmoid(aux);
            }
        }
    }
/*  
    public static newBySigmoidMap(bool derivate = false, FMatrix M){
        FMatrix m = new FMatrix(M.rows, M.columns);
        for(int i = 0; i < rows; i++){
            for(int j = 0; j < columns; j++){
                float aux = M.data[i,j];
                m.data[i,j] = Sigmoid(aux, derivate);
            }
        }
        return m;
    }
*/
    public void Randomize(){
        for(int i = 0; i < rows; i++){
            for(int j = 0; j < columns; j++){
                data[i,j] = Random.Range(-1,1);
            }
        }
    }

    public void PlusM(FMatrix m){
        for(int i = 0; i < m.rows; i++){
            for(int j = 0; j < m.columns; j++){
                data[i,j] += m.data[i,j];
            }
        }
    }
    public void PlusF(float f){   
        for(int i = 0; i < rows; i++){
            for(int j = 0; j < columns; j++){
                data[i,j] += f;
            }
        }
    }
    public void MinusM(FMatrix m){
        for(int i = 0; i < m.rows; i++){
            for(int j = 0; j < m.columns; j++){
                data[i,j] -= m.data[i,j];
            }
        }
    }
    public void MinusF(float f){
        for(int i = 0; i < rows; i++){
            for(int j = 0; j < columns; j++){
                data[i,j] -= f;
            }
        }
    }
    public void ProductE(float f){
        for(int i = 0; i < rows; i++){
            for(int j = 0; j < columns; j++){
                data[i,j] *= f;
            }
        }
    }
    public void ProductM(FMatrix M){
        for(int i = 0; i < rows; i++){
            for(int j = 0; j < columns; j++){
                data[i,j] *= M.data[i,j];
            }
        }
    }
    public string Print(){
        string s = "";
        for(int i = 0; i < rows; i++){
            for(int j = 0; j < columns; j++){
                s += data[i,j]+" ";
            }
            s += "\n";
        }
        return s;
    }
}