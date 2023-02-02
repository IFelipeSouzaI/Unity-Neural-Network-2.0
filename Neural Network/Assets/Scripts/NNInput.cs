using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNInput
{

    public float[] data;
    public float[] target;

    public NNInput(float[] newData, float[] newTarget){
        data = newData;
        target = newTarget;
    }

    public static float[] ImageToArray(Texture2D image){
        float[] a = new float[image.width * image.height];
        int index = 0;
        for (int i = 0; i < image.width; i++){
            for (int j = 0; j < image.height; j++){
                Color pixel = image.GetPixel(i, j);
                if(pixel == Color.white){
                    a[index] = 1;
                }else{
                    a[index] = 0;
                }
                index += 1;
            }
        }
        return a;
    }

}
