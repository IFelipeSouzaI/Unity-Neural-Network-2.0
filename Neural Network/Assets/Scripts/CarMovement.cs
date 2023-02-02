using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private float currentSpeed = 0;
    private float minSpeed = 3f;
    private float maxSpeed = 5f;
    private float acceleration = 1f;
    private float raycastDistance = 15f;
    private float currentRotationSpeed = 0f;
    private float minRotationSpeed = 30f;
    private float maxRotationSpeed = 90f;

    public LayerMask obstaclesLayer;

    private DNA selfDNA = null;

    private bool canMove = false;

    private float[] inputs = new float[9] {0, 0, 0, 0, 0, 0, 0, 0, 0};
    private FMatrix outputs;

    private Vector3 originPos;

    private void Start() {
        //selfDNA = new DNA();
        //selfDNA.data = new NeuralNetwork(3, 4, 3, 2);
        originPos = transform.position;
        canMove = true;
        
    }

    void Update()
    {
        if(!canMove) {
            return;
        }
        
        RaycastHit2D hitForward = Physics2D.Raycast(transform.position, transform.right, raycastDistance, obstaclesLayer);
        CheckRaycast(hitForward, 0);
        Debug.DrawRay(transform.position, transform.right * hitForward.distance, Color.red);

        Vector2 leftDirection = Quaternion.Euler(0, 0, 45) * transform.right;
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, leftDirection, raycastDistance, obstaclesLayer);
        CheckRaycast(hitLeft, 1);
        Debug.DrawRay(transform.position, leftDirection * hitLeft.distance, Color.red);

        Vector2 rightDirection = Quaternion.Euler(0, 0, -45) * transform.right;
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, rightDirection, raycastDistance, obstaclesLayer);
        CheckRaycast(hitRight, 2);
        Debug.DrawRay(transform.position, rightDirection * hitRight.distance, Color.red);

        Vector2 midRightDirection = Quaternion.Euler(0, 0, -27) * transform.right;
        RaycastHit2D hitMidRight = Physics2D.Raycast(transform.position, midRightDirection, raycastDistance, obstaclesLayer);
        CheckRaycast(hitMidRight, 3);
        Debug.DrawRay(transform.position, midRightDirection * hitMidRight.distance, Color.red);

        Vector2 bottonDirection = Quaternion.Euler(0, 0, 90) * transform.right;
        RaycastHit2D hitBotton = Physics2D.Raycast(transform.position, bottonDirection, raycastDistance, obstaclesLayer);
        CheckRaycast(hitBotton, 4);
        Debug.DrawRay(transform.position, bottonDirection * hitBotton.distance, Color.red);

        Vector2 midLeftDirection = Quaternion.Euler(0, 0, 27) * transform.right;
        RaycastHit2D hitMidLeft = Physics2D.Raycast(transform.position, midLeftDirection, raycastDistance, obstaclesLayer);
        CheckRaycast(hitMidLeft, 5);
        Debug.DrawRay(transform.position, midLeftDirection * hitMidLeft.distance, Color.red);

        Vector2 topDirection = Quaternion.Euler(0, 0, -90) * transform.right;
        RaycastHit2D hitTop = Physics2D.Raycast(transform.position, topDirection, raycastDistance, obstaclesLayer);
        CheckRaycast(hitTop, 6);
        Debug.DrawRay(transform.position, topDirection * hitTop.distance, Color.red);

        inputs[7] = currentSpeed;
        inputs[8] = currentRotationSpeed;
        //inputs[9] = currentRotationSpeedL;

        outputs = selfDNA.data.FeedFoward(inputs);
        //Debug.Log(outputs.Print());

        if(outputs.data[0,0] > 0.5f || outputs.data[1,0] > 0.5f || outputs.data[2,0] > 0.5f) {
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        }

        if(outputs.data[0,0] > 0.25f) {
            currentSpeed += acceleration * Time.deltaTime;
            if(currentSpeed > maxSpeed) {
                currentSpeed = maxSpeed;
            }
        }
        if(outputs.data[1,0] > 0.25f) {
            currentSpeed -= acceleration * Time.deltaTime;
            if(currentSpeed < minSpeed) {
                currentSpeed = minSpeed;
            }
        }
        if(outputs.data[2,0] > 0.25f) {
            currentRotationSpeed += (acceleration*minRotationSpeed) * Time.deltaTime;
            if(currentRotationSpeed > maxRotationSpeed) {
                currentRotationSpeed = maxRotationSpeed;
            }
        }

        if(outputs.data[2,0] <= 0.25f) {
            currentRotationSpeed -= (acceleration*minRotationSpeed) * Time.deltaTime;
            if(currentRotationSpeed < maxRotationSpeed*-1) {
                currentRotationSpeed = maxRotationSpeed*-1;
            }
        }

        transform.Rotate(Vector3.forward, currentRotationSpeed * Time.deltaTime);

        transform.position += transform.right * currentSpeed * Time.deltaTime;
        
    }

    public DNA GetDNA() {
        if(selfDNA == null) {
            selfDNA = new DNA(0.02f);
            selfDNA.data = new NeuralNetwork(9, 4, 3, 2);
        }
        return selfDNA;
    }

    public void SetDNA(DNA newDNA) {
        selfDNA = newDNA;
        StartToMove();
    }

    public void StartToMove() {
        canMove = true;
    }

    private void CheckRaycast(RaycastHit2D hit, int index) {
        if (hit){
           /* if(hitBotton.transform.gameObject.tag == "power"){
                if(hitBotton.distance > 0.1f)
                    inputs[13] = (1 - hitBotton.distance/raycastDistance);
            } else {*/
            //inputs[index] = (1f - hit.distance/raycastDistance);
            inputs[index] = hit.distance;
            //}
        } else {
            inputs[index] = raycastDistance;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(!canMove) {
            return;
        }
        if(collision.gameObject.tag == "wall") {
            selfDNA.AddFitness(Vector2.Distance(transform.position, originPos));
            transform.parent.GetComponent<CarSpawnerController>().currentCount += 1;
            //selfDNA.AddFitness(Vector2.Distance(transform.position, originPos) * 5f);
            canMove = false;
        }
        /*if(collision.gameObject.tag == "power") {
            selfDNA.AddFitness(15f);
        }
        if(collision.gameObject.tag == "win") {
            Debug.Log("CHEGOU");
            selfDNA.AddFitness(75f);
            transform.parent.GetComponent<CarSpawnerController>().currentCount += 1;
            canMove = false;
        }*/
    }

    public void ResetToStart() {
        canMove = false;
        transform.position = originPos;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

}
