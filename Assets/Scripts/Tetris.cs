using System;
using TMPro;
using UnityEngine;

public class Tetris : MonoBehaviour
{
    public GameObject square0;
    public GameObject long0;
    public GameObject zigzag0;
    public GameObject square1;
    public GameObject long1;
    public GameObject zigzag1;
    public GameObject square2;
    public GameObject long2;
    public GameObject zigzag2;
    public GameObject square3;
    public GameObject long3;
    public GameObject zigzag3;
    public GameObject square4;
    public GameObject long4;
    public GameObject zigzag4;
    public GameObject square0Sentence;
    public GameObject long0Sentence;
    public GameObject zigzag0Sentence;
    public GameObject zigzag0Sentence2;
    public GameObject square1Sentence;
    public GameObject long1Sentence;
    public GameObject zigzag1Sentence;
    public GameObject zigzag1Sentence2;
    public GameObject square2Sentence;
    public GameObject long2Sentence;
    public GameObject zigzag2Sentence;
    public GameObject zigzag2Sentence2;
    public GameObject square3Sentence;
    public GameObject long3Sentence;
    public GameObject zigzag3Sentence;
    public GameObject zigzag3Sentence2;
    public GameObject square4Sentence;
    public GameObject long4Sentence;
    public GameObject zigzag4Sentence;
    public GameObject zigzag4Sentence2;

    public GameObject background2;

    
    //saved start position for shapes
    public Vector2 startPos; 
    //saved number for random piece choosing
    public int choiceNumber = 0;

    //saved time for checking how much time has gone by
    public float lastTime;

    //saved yes/no true/false for when to keep making more pieces
    public bool keepSpawning;

    //to count how many times a random choice has been tried
    public int tryNumber;

    //list of shapes so we can call them by number instead of by name
    public GameObject[] shapes;
    //list of sentences so we can call them by number instead of by name
    public GameObject[] sentences;

    //to save the position controller for shapes
    //so our code lines are shorter and easier to read
    public RectTransform rect;

    public float xPositionCheck;

    public int shapeNumber = 0;
    
    // Everything in Start() happens one time at the beginning
    void Start()
    {

        // These set our shapes into a numbered list that we can easily use
        // to randomly call different shapes
        shapes = new GameObject[15];
        sentences = new GameObject[20];

        shapes[0] = square0;
        shapes[1] = long0;
        shapes[2] = zigzag0;
        shapes[3] = square1;
        shapes[4] = long1;
        shapes[5] = zigzag1;
        shapes[6] = square2;
        shapes[7] = long2;
        shapes[8] = zigzag2;
        shapes[9] = square3;
        shapes[10] = long3;
        shapes[11] = zigzag3;
        shapes[12] = square4;
        shapes[13] = long4;
        shapes[14] = zigzag4;

        // find all shape gameobjects
        shapeNumber = 0;
        foreach (Transform child in background2.transform)
        {     
            if(child.CompareTag("Piece"))
            {
                Debug.Log(child.gameObject.name);
                shapes[shapeNumber] = child.gameObject;
                Debug.Log(shapes[shapeNumber].name);
                shapeNumber += 1;    
            }        
        }    
        
        square0     =   shapes[0];
        long0       =   shapes[1];
        zigzag0     =   shapes[2];
        square1     =   shapes[3];
        long1       =   shapes[4];
        zigzag1     =   shapes[5];
        square2     =   shapes[6];
        long2       =   shapes[7];
        zigzag2     =   shapes[8];
        square3     =   shapes[9];
        long3       =   shapes[10];
        zigzag3     =   shapes[11];
        square4     =   shapes[12];
        long4       =   shapes[13];
        zigzag4     =   shapes[14]; 

        // find all sentence gameobjects
        shapeNumber = 0;
        foreach (GameObject shape in shapes)
        {     
            foreach (Transform child in shape.transform)
            {
                if(child.CompareTag("Sentence"))
                {
                    Debug.Log(child.gameObject.name);
                    sentences[shapeNumber] = child.gameObject;
                    Debug.Log(sentences[shapeNumber].name);
                    shapeNumber += 1;    
                }    
            }                
        }    

        square0Sentence    = sentences[0];
        long0Sentence      = sentences[1];
        zigzag0Sentence    = sentences[2];
        zigzag0Sentence2   = sentences[3];
        square1Sentence    = sentences[4];
        long1Sentence      = sentences[5];
        zigzag1Sentence    = sentences[6];
        zigzag1Sentence2   = sentences[7];
        square2Sentence    = sentences[8];
        long2Sentence      = sentences[9];
        zigzag2Sentence    = sentences[10];
        zigzag2Sentence2   = sentences[11];
        square3Sentence    = sentences[12];
        long3Sentence      = sentences[13];
        zigzag3Sentence    = sentences[14];
        zigzag3Sentence2   = sentences[15];
        square4Sentence    = sentences[16];
        long4Sentence      = sentences[17];
        zigzag4Sentence    = sentences[18];
        zigzag4Sentence2   = sentences[19];       

        //this is the text that needs to change
        //The text here will appear in the pieces        
        square0Sentence.GetComponent<TextMeshProUGUI>().text    =   "1"        ;   //  Sentence 1
        long0Sentence.GetComponent<TextMeshProUGUI>().text      =   "2"        ;   //  Sentence 2
        zigzag0Sentence.GetComponent<TextMeshProUGUI>().text    =   "3"        ;   //  Sentence 3 first half
        zigzag0Sentence2.GetComponent<TextMeshProUGUI>().text   =   "4"        ;   //  Sentence 3 second hal
        square1Sentence.GetComponent<TextMeshProUGUI>().text    =   "5"        ;   //  Sentence 4
        long1Sentence.GetComponent<TextMeshProUGUI>().text      =   "6"        ;   //  Sentence 5        
        zigzag1Sentence.GetComponent<TextMeshProUGUI>().text    =   "7"        ;   //  Sentence 6 first half        
        zigzag1Sentence2.GetComponent<TextMeshProUGUI>().text   =   "8"        ;   //  Sentence 6 second half        
        square2Sentence.GetComponent<TextMeshProUGUI>().text    =   "9"        ;   //  Sentence 7
        long2Sentence.GetComponent<TextMeshProUGUI>().text      =   "10"       ;   //  Sentence 8        
        zigzag2Sentence.GetComponent<TextMeshProUGUI>().text    =   "11"       ;   //  Sentence 9 first half
        zigzag2Sentence2.GetComponent<TextMeshProUGUI>().text   =   "12"       ;   //  Sentence 9 second half        
        square3Sentence.GetComponent<TextMeshProUGUI>().text    =   "13"       ;   //  Sentence 10        
        long3Sentence.GetComponent<TextMeshProUGUI>().text      =   "14"       ;   //  Sentence 11        
        zigzag3Sentence.GetComponent<TextMeshProUGUI>().text    =   "15"       ;   //  Sentence 12 first half        
        zigzag3Sentence2.GetComponent<TextMeshProUGUI>().text   =   "16"       ;   //  Sentence 12 second half        
        square4Sentence.GetComponent<TextMeshProUGUI>().text    =   "17"       ;   //  Sentence 13
        long4Sentence.GetComponent<TextMeshProUGUI>().text      =   "18"       ;   //  Sentence 14        
        zigzag4Sentence.GetComponent<TextMeshProUGUI>().text    =   "19"       ;   //  Sentence 15 first half        
        zigzag4Sentence2.GetComponent<TextMeshProUGUI>().text   =   "20"       ;   //  Sentence 15 second half 

        // this makes all the pieces turned off at the beginning
        square0.SetActive(false);
        long0.SetActive(false);
        zigzag0.SetActive(false);
        square1.SetActive(false);
        long1.SetActive(false);
        zigzag1.SetActive(false);
        square2.SetActive(false);
        long2.SetActive(false);
        zigzag2.SetActive(false);
        square3.SetActive(false);
        long3.SetActive(false);
        zigzag3.SetActive(false);
        square4.SetActive(false);
        long4.SetActive(false);
        zigzag4.SetActive(false);

        //this is the start position of the falling blocks
        //lets try it
        //Let's movea piece and see what number we get
        // a height of 364
        startPos = new Vector2(0, 0); 

        // this sets teh location of all the pieces to the starting location
        // so they can fall down when they appear 
        square0.GetComponent<RectTransform>().anchoredPosition = startPos;
        long0.GetComponent<RectTransform>().anchoredPosition = startPos;
        zigzag0.GetComponent<RectTransform>().anchoredPosition = startPos;
        square1.GetComponent<RectTransform>().anchoredPosition = startPos;
        long1.GetComponent<RectTransform>().anchoredPosition = startPos;
        zigzag1.GetComponent<RectTransform>().anchoredPosition = startPos;
        square2.GetComponent<RectTransform>().anchoredPosition = startPos;
        long2.GetComponent<RectTransform>().anchoredPosition = startPos;
        zigzag2.GetComponent<RectTransform>().anchoredPosition = startPos;
        square3.GetComponent<RectTransform>().anchoredPosition = startPos;
        long3.GetComponent<RectTransform>().anchoredPosition = startPos;
        zigzag3.GetComponent<RectTransform>().anchoredPosition = startPos;
        square4.GetComponent<RectTransform>().anchoredPosition = startPos;
        long4.GetComponent<RectTransform>().anchoredPosition = startPos;
        zigzag4.GetComponent<RectTransform>().anchoredPosition = startPos;        
        
        //sets a start time to the current time, so we can measure seconds passed
        lastTime = Time.time;

        // sets the number of random piece trys to 0
        // If there are to many tries, then we will skip
        // generating random numbers, because we want to avoid
        // the program running forever trying to pick a random number
        tryNumber = 0;
        keepSpawning = true;
        
    }

    // Everything in update happens about 30 times a second
    void Update()
    {
        // change from getkeydown to getkey
        // this moves pieces every time a key is pressed
        // let's change it to keep moving when you hold down the key
        // that's it
        //we need an if statement here
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //add if statement to stop it going out left side
            // what number are we checking?
            // -798          
            
            rect = shapes[choiceNumber].GetComponent<RectTransform>();
            //change it to 5 to make it move slower
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x - 10,rect.anchoredPosition.y); 
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            //add if statement to stop it going out right side
            //What number do we need for the right side?
            //let's check
            //less than 795
            
            rect = shapes[choiceNumber].GetComponent<RectTransform>();
            //change it to 5 to make it move slower
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + 10,rect.anchoredPosition.y); 
        }
        // this is the spawn time of the next block
        // if you set it to 3, a piece will appear every 3 seconds
        // lets set it to 2, and see what happens
        //we can make a new variable for this number called shapeSpawnWaitTime if we want it to be clearer
        
        if(Time.time > lastTime + 2)
        {
            //resets time to wait
            lastTime = Time.time;

            // choose a random shape to spawn
            choiceNumber = UnityEngine.Random.Range(0, 15);

            // check if all shapes have appeared
            //if all shapes on, then stop spawning shapes
            // otherwise, keep spawning shapes
            if  (   shapes[0].activeSelf == true &&
                    shapes[1].activeSelf == true &&
                    shapes[2].activeSelf == true &&
                    shapes[3].activeSelf == true &&
                    shapes[4].activeSelf == true &&
                    shapes[5].activeSelf == true &&
                    shapes[6].activeSelf == true &&
                    shapes[7].activeSelf == true &&
                    shapes[8].activeSelf == true &&
                    shapes[9].activeSelf == true &&
                    shapes[10].activeSelf == true &&
                    shapes[11].activeSelf == true &&
                    shapes[12].activeSelf == true &&
                    shapes[13].activeSelf == true &&
                    shapes[14].activeSelf == true
                )
            {
                keepSpawning = false;
            }
            else
            {
                keepSpawning = true;
            }
            // this while loop keeps choosing new numbers if the randomly chosen shape is already spawned
            while(shapes[choiceNumber].activeSelf == true && keepSpawning == true && tryNumber < 30)
            {
                choiceNumber = UnityEngine.Random.Range(0, 15);
            }
            // if the randomly chosen shape is not spawned yet, spawn it, turn it on
            if(shapes[choiceNumber].activeSelf == false && keepSpawning == true)
            {
                shapes[choiceNumber].SetActive(true);
            }
            
        }
        // This if statement is seperate from the 5 second timer if statement above
        // If no shapes are active, make one. This is so that we don't have to wait 5 seconds at the start of the game.
        if  (   shapes[0].activeSelf == false &&
                shapes[1].activeSelf == false &&
                shapes[2].activeSelf == false &&
                shapes[3].activeSelf == false &&
                shapes[4].activeSelf == false &&
                shapes[5].activeSelf == false &&
                shapes[6].activeSelf == false &&
                shapes[7].activeSelf == false &&
                shapes[8].activeSelf == false &&
                shapes[9].activeSelf == false &&
                shapes[10].activeSelf == false &&
                shapes[11].activeSelf == false &&
                shapes[12].activeSelf == false &&
                shapes[13].activeSelf == false &&
                shapes[14].activeSelf == false
            )
        {
            choiceNumber = UnityEngine.Random.Range(0, 15);
            shapes[choiceNumber].SetActive(true);
        }
    }

    // this functon gets activated by the restart button
    // it turns off all of the shapes and returns them back to their original position
    public void Restart()
    {
        square0.SetActive(false);
        long0.SetActive(false);
        zigzag0.SetActive(false);
        square1.SetActive(false);
        long1.SetActive(false);
        zigzag1.SetActive(false);
        square2.SetActive(false);
        long2.SetActive(false);
        zigzag2.SetActive(false);
        square3.SetActive(false);
        long3.SetActive(false);
        zigzag3.SetActive(false);
        square4.SetActive(false);
        long4.SetActive(false);
        zigzag4.SetActive(false);

        startPos =  new Vector2(0, 0);

        square0.GetComponent<RectTransform>().anchoredPosition  = startPos;
        long0.GetComponent<RectTransform>().anchoredPosition    = startPos;
        zigzag0.GetComponent<RectTransform>().anchoredPosition  = startPos;
        square1.GetComponent<RectTransform>().anchoredPosition  = startPos;
        long1.GetComponent<RectTransform>().anchoredPosition    = startPos;
        zigzag1.GetComponent<RectTransform>().anchoredPosition  = startPos;
        square2.GetComponent<RectTransform>().anchoredPosition  = startPos;
        long2.GetComponent<RectTransform>().anchoredPosition    = startPos;
        zigzag2.GetComponent<RectTransform>().anchoredPosition  = startPos;
        square3.GetComponent<RectTransform>().anchoredPosition  = startPos;
        long3.GetComponent<RectTransform>().anchoredPosition    = startPos;
        zigzag3.GetComponent<RectTransform>().anchoredPosition  = startPos;
        square4.GetComponent<RectTransform>().anchoredPosition  = startPos;
        long4.GetComponent<RectTransform>().anchoredPosition    = startPos;
        zigzag4.GetComponent<RectTransform>().anchoredPosition  = startPos;
    }
}
