using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImagesManager : MonoBehaviour
{
    
    private FImageEditor editor = new FImageEditor();
    public List<Texture2D> imagesToReduce;
    
    private void Start() {
        for(int i = 0; i < imagesToReduce.Count; i++){
            Texture2D imageCrompressed = editor.ResizeImage(imagesToReduce[i], 500);
            byte[] bytes = imageCrompressed.EncodeToPNG();
            File.WriteAllBytes("COMPRESSED.png", bytes);
            FImage newImage = editor.LoadToFImage(imageCrompressed);
            editor.SaveFromFImage(editor.ApplyEdgeFilter(newImage, 3));
        }
    }


}
