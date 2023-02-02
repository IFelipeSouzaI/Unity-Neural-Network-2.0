using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawInScreen : MonoBehaviour
{

    public int gridSize = 10;
    public GameObject spritePrefab;
    private GameObject[,] sprites;
    private float[] values;
    private NeuralNetwork nn;
    private NN_DataManager dataManager;
    private bool onTouch = false;
    private void Start()
    {
        dataManager = this.gameObject.GetComponent<NN_DataManager>();
        nn = dataManager.ReadFromFile();
        sprites = new GameObject[gridSize, gridSize];
        values = new float[gridSize * gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                int index = i * gridSize + j;
                sprites[i, j] = Instantiate(spritePrefab, new Vector3(i, j, 0), Quaternion.identity);
                sprites[i, j].GetComponent<SpriteRenderer>().color = Color.black;
                values[index] = 0;
            }
        }
    }

    private void Update()
    {
        // Check for mouse click on a sprite
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            int x = Mathf.FloorToInt(worldPos.x);
            int y = Mathf.FloorToInt(worldPos.y);
            DrawIn(x, y, Color.white, 1);
            onTouch = true;
        }else if (Input.GetMouseButton(1))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            int x = Mathf.FloorToInt(worldPos.x);
            int y = Mathf.FloorToInt(worldPos.y);
            DrawIn(x, y, Color.black, 0);
            onTouch = true;
        } else {
            if(onTouch) {
                /*for(int i = 0; i < values.Length; i++){
                    Debug.Log(values[i]);
                }*/
                Debug.Log(nn.FeedFoward(values).Print());
                onTouch = false;
            }
        }
    }

    private void DrawIn(int x, int y, Color newColor, int value = 0) {
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
        {
            int index = y * gridSize + x;
            sprites[x, y].GetComponent<SpriteRenderer>().color = newColor;
            values[index] = value;
        }
    }


    /*
    public GameObject sprite;
    public Vector2 worldPosition;

    public int imageDimension = 10;
    private float imageScale = 0;
    private List<GameObject> sprites = new List<GameObject>();

    private bool onTouch = false;
    
    private float[] inputs = new float[100];
    void Start()
    {
        
        imageScale = 100f / imageDimension;
        for (int i = 0; i < (imageDimension * imageDimension); i++){
            var spr = Instantiate(sprite, transform.position, Quaternion.identity);
            spr.transform.localScale = new Vector3(imageScale, imageScale, imageScale);
            spr.transform.position = new Vector2((i%imageDimension) * imageScale/10f, (int)(i/imageDimension) * imageScale/10f);
            spr.transform.parent = transform;
            spr.GetComponent<SpriteRenderer>().color = Color.black;
            sprites.Add(spr);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        if (Input.GetMouseButton(0)){
            onTouch = true;
            var spriteIndex = new Vector2((int)(worldPosition.y/(imageScale/10f)), (int)(worldPosition.x/ (imageScale / 10f)));
            //Debug.Log((int)(spriteIndex.x * imageDimension + spriteIndex.y));
            sprites[(int)(spriteIndex.x * imageDimension + spriteIndex.y)].GetComponent<SpriteRenderer>().color = Color.white;
        }else if (Input.GetMouseButton(1)){
            onTouch = true;
            var spriteIndex = new Vector2((int)(worldPosition.y/(imageScale/10f)), (int)(worldPosition.x/ (imageScale / 10f)));
            sprites[(int)(spriteIndex.x * imageDimension + spriteIndex.y)].GetComponent<SpriteRenderer>().color = Color.black;
        } else {
            if(onTouch) {
                int n = 0;
                for(int i = 0; i < 10; i++) {
                    for(int j = 0; j < 100; j += 10) {
                        if(sprites[i + j].GetComponent<SpriteRenderer>().color == Color.white){
                            inputs[n] = 1;
                        } else {
                            inputs[n] = 0;
                        }
                        Debug.Log(n);
                        n += 1;
                    }
                }
                Debug.Log(nn.FeedFoward(inputs).Print());
                onTouch = false;
            }
        }
    }

    private void InputsUpdate() {
        /*for(int i = 0; i < 100; i++){
            if(sprites[i].GetComponent<SpriteRenderer>().color == Color.white){
                inputs[i] = 1f;
            } else {
                inputs[i] = 0f;
            }
        }*/
    //}

}
