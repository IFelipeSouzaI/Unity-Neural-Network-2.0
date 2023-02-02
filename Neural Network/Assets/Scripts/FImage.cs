using UnityEngine;
using System.IO;

/*
using System.Threading;
private Texture2D image;
private Texture2D processedImage;

private void Start() {
    // Carregue a imagem aqui...
    
    Thread backgroundThread = new Thread(ApplyBlackAndWhiteFilter);
    backgroundThread.Start();
}*/

public class FImage {
    public string name = "";
    public int width = 0;
    public int height = 0;
    public float[,] r;
    public float[,] g;
    public float[,] b;
    public FImage(int newWidth, int newHeight, string newName) {
        name = newName;
        height = newHeight;
        width = newWidth;
        r = new float[height, width];
        g = new float[height, width];
        b = new float[height, width];
    }
}

public class FImageEditor
{
    
    public FImage LoadToFImage(Texture2D imageToLoad) {
        FImage imageLoaded = new FImage(imageToLoad.width, imageToLoad.height, imageToLoad.name);
        for(int i = 0; i < imageLoaded.height; i++) {
            for(int j = 0; j < imageLoaded.width; j++) {
                imageLoaded.r[i,j] = imageToLoad.GetPixel(j, i).r;
                imageLoaded.g[i,j] = imageToLoad.GetPixel(j, i).g;
                imageLoaded.b[i,j] = imageToLoad.GetPixel(j, i).b;
            }
        }
        return imageLoaded;
    }

    public void SaveFromFImage(FImage imageToSave) {
        Texture2D finalImage = new Texture2D(imageToSave.width, imageToSave.height, TextureFormat.RGBA32, false);
        for(int i = 0; i < imageToSave.height; i++) {
            for(int j = 0; j < imageToSave.width; j++) {
                Color newColor = new Color();
                newColor.r = imageToSave.r[i,j];
                newColor.g = imageToSave.g[i,j];
                newColor.b = imageToSave.b[i,j];
                newColor.a = 1;
                finalImage.SetPixel(j, i, newColor);
            }
        }
        finalImage.Apply();
        byte[] bytes = finalImage.EncodeToPNG();
        File.WriteAllBytes(imageToSave.name + ".png", bytes);
    }

    public Texture2D ResizeImage(Texture2D imageToResize, int defaultSize = 500) {
        bool lowHeight = true;
        int normalSize = imageToResize.width;
        if(imageToResize.height > normalSize) {
            normalSize = imageToResize.height;
            lowHeight = false;
        }
        Texture2D squareTexture = new Texture2D(normalSize, normalSize, TextureFormat.RGBA32, false);
        for(int i = 0; i < normalSize; i++) {
            for(int j = 0; j < normalSize;j++) {
                if(lowHeight) {
                    int dif = normalSize - imageToResize.height;
                    if((i < (dif/2))) {
                        squareTexture.SetPixel(j, i, Color.black/*imageToResize.GetPixel(j, 0)*/);
                    } else if((i > (normalSize - (dif/2)))){ 
                        squareTexture.SetPixel(j, i, Color.black/*imageToResize.GetPixel(j, normalSize-1)*/);
                    } else {
                        squareTexture.SetPixel(j, i, imageToResize.GetPixel(j, i-(dif/2)));
                    }
                } else {
                    int dif = normalSize - imageToResize.width;
                    if((j < (dif/2))) {
                        squareTexture.SetPixel(j, i,Color.black /*imageToResize.GetPixel(0, i)*/);
                    } else if((j > (normalSize - (dif/2)))){ 
                        squareTexture.SetPixel(j, i,Color.black /*imageToResize.GetPixel(normalSize-1, i)*/);
                    } else {
                        squareTexture.SetPixel(j, i, imageToResize.GetPixel(j-(dif/2), i));
                    }
                }
            }
        }
        squareTexture.Apply();
        /*byte[] bytes = squareTexture.EncodeToPNG();
        File.WriteAllBytes("SQUARE"+imageIndex+".png", bytes);*/
        /*if(normalSize < defaultSize) {
            return ImageUpTo(squareTexture, defaultSize);
        }*/
        squareTexture.name = imageToResize.name;
        return ImageDownTo(squareTexture, defaultSize);
    }

    /*private Texture2D ImageUpTo(Texture2D imageToUp, int defaultSize) {
        Texture2D newTexture = new Texture2D(defaultSize, defaultSize, TextureFormat.RGBA32, false);
        int pixelsToAdd = defaultSize - imageToUp.width;
        return newTexture;
    }*/
    
    private Texture2D ImageDownTo(Texture2D imageToDown, int defaultSize) {
        Texture2D newTexture = new Texture2D(defaultSize, defaultSize, TextureFormat.RGBA32, false);
        int pixelsToRemove = imageToDown.width%defaultSize;
        Texture2D textureNormalized = new Texture2D(imageToDown.width - pixelsToRemove, imageToDown.height - pixelsToRemove, TextureFormat.RGBA32, false);
        int pixelPerAreaToRemove = 0;
        if(pixelsToRemove != 0) {
            pixelPerAreaToRemove = imageToDown.width/pixelsToRemove;
        }
        int normalRow = 0, normalColum = 0;
        int linesRemoved = 0, columnsRemoved = 0;
        for(int i = 0; i < imageToDown.height; i++) {
            if((linesRemoved < pixelsToRemove) && i%pixelPerAreaToRemove == 0) {
                linesRemoved += 1;
                continue;
            }
            normalColum = 0;
            columnsRemoved = 0;
            for(int j = 0; j < imageToDown.width; j++) {
                if((columnsRemoved < pixelsToRemove) && j%pixelPerAreaToRemove == 0) {
                    columnsRemoved += 1;
                    continue;
                }
                textureNormalized.SetPixel(normalColum, normalRow, imageToDown.GetPixel(j, i));
                normalColum += 1;
            }
            normalRow += 1;
        }
        textureNormalized.Apply();
        /*byte[] bytes = textureNormalized.EncodeToPNG();
        File.WriteAllBytes("NORMAL"+imageIndex+".png", bytes);*/
        int row = 0, column = 0;
        int timesUp = textureNormalized.height/defaultSize;
        for(int i = 0; i < textureNormalized.height; i += timesUp) {
            column = 0;
            for(int j = 0; j < textureNormalized.width;j += timesUp) {
                newTexture.SetPixel(column, row, textureNormalized.GetPixel(j, i));
                column += 1;
            }
            row += 1;
        }
        newTexture.Apply();
        newTexture.name = imageToDown.name;
        return newTexture;
    }

    // Apply Filters
    public FImage ApplyGrayFilter(FImage baseImage, int filterSize) {
        FImage finalImage = new FImage(baseImage.width - (filterSize-1), baseImage.height - (filterSize-1), baseImage.name + "_GrayScale");
        int startingPoint = (filterSize/2);
        int endPoint = baseImage.height-(filterSize/2);
        for(int i = startingPoint; i < endPoint; i++) {
            for(int j = startingPoint; j < endPoint; j++) {
                float sum = (baseImage.r[i,j] + baseImage.g[i,j] + baseImage.b[i,j])/3f;
                finalImage.r[i-startingPoint,j-startingPoint] = sum;
                finalImage.g[i-startingPoint,j-startingPoint] = sum;
                finalImage.b[i-startingPoint,j-startingPoint] = sum;
            }
        }
        return finalImage;
    }

    public FImage ApplyEdgeFilter(FImage baseImage, int filterSize) {
        float[,] filter = new float[3,3] {{0, -1, 0}, {1, -1, 1}, {0, -1, 0}};
        FImage finalImage = new FImage(baseImage.width - (filterSize-1), baseImage.height - (filterSize-1), baseImage.name + "_EdgeDetector");
        int startingPoint = (filterSize/2);
        int endPoint = baseImage.height-(filterSize/2);
        for(int i = startingPoint; i < endPoint; i++) {
            for(int j = startingPoint; j < endPoint; j++) {
                float sumR = 0f;
                float sumG = 0f;
                float sumB = 0f;
                for(int m = 0; m < filterSize; m++) {
                    for(int n = 0; n < filterSize; n++) {
                        sumR += baseImage.r[i + m - (filterSize/2), j + n - (filterSize/2)] * filter[m,n];
                        sumG += baseImage.g[i + m - (filterSize/2), j + n - (filterSize/2)] * filter[m,n];
                        sumB += baseImage.b[i + m - (filterSize/2), j + n - (filterSize/2)] * filter[m,n];
                    }
                }
                if(sumR < 0) {
                    sumR *= -1;
                }
                if(sumG < 0) {
                    sumG *= -1;
                }
                if(sumB < 0) {
                    sumB *= -1;
                }
                finalImage.r[i-startingPoint,j-startingPoint] = sumR/4f;
                finalImage.g[i-startingPoint,j-startingPoint] = sumG/4f;
                finalImage.b[i-startingPoint,j-startingPoint] = sumB/4f;
                /*if(((sumR+sumG+sumB)/3f) > 0.74f){
                    finalImage.r[i-startingPoint,j-startingPoint] = 1;
                    finalImage.g[i-startingPoint,j-startingPoint] = 1;
                    finalImage.b[i-startingPoint,j-startingPoint] = 1;
                } else {
                    finalImage.r[i-startingPoint,j-startingPoint] = 0;
                    finalImage.g[i-startingPoint,j-startingPoint] = 0;
                    finalImage.b[i-startingPoint,j-startingPoint] = 0;
                }*/
            }
        }
        return finalImage;
    }

}


