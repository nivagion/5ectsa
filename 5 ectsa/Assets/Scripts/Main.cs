using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;//za text od textmashproa
using System.IO;//za fileove
using System;

public class Main : MonoBehaviour
{
    public bool jede = false, mobitel = false, pije = false;
    public bool onCamera = false;
    public int score = 0;
    private int highscore = 0; // dobiva int iz filea, stavit u  start da dobiva iz filea
    public float maxLoss = 10f;
    private float timer = 0f;
    public float kolikoTrebaProc = 1f;

    public GameObject rotirajuciNino;

    public GameObject kraj;

    public Slider HungerSlider;
    private float Hunger;
    public Slider ThirstSlider;
    private float Thirst;
    public Slider SanSlider;
    private float San;

    public Animator animacija;

    public GameObject lik;
    public GameObject burger;
    public GameObject voda;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI GameOverText;


    private string dataPath;

    private bool isGameOver = false;


    void Start()
    {

        //na pocetku igre hunger je na 100%(nisi gladan)
        Hunger = 100f;
        Thirst = 100f;
        San = 100f;

        lik.SetActive(false);  

        //score
        //datapath za highscore i ostalo
        dataPath = Path.Combine(Application.persistentDataPath, "highscore.txt");
        //Debug.Log(dataPath);
        animacija.SetBool("pije", false);
        burger.SetActive(false);
        voda.SetActive(false);


        LoadHighScore();
        UpdateHighScoreText();
        UpdateScoreText();

    }

    void Update()//svaki frame
    {
        if (isGameOver)
        {
            return;
        }

        float lossHunger = UnityEngine.Random.Range(4f, maxLoss);
        float lossThirst = UnityEngine.Random.Range(3f, maxLoss);
        float lossSan = UnityEngine.Random.Range(4f, maxLoss);

        //povezuje slider sa vrijednosti
        ThirstSlider.value = Thirst;
        HungerSlider.value = Hunger;
        SanSlider.value = San;

        //random koliko ce hrane, vode ili sna izgubit
        Hunger -= lossHunger * Time.deltaTime;
        Thirst -= lossThirst * Time.deltaTime;
        San -= lossSan * Time.deltaTime;

        jede = Input.GetKey(KeyCode.A);
        pije = Input.GetKey(KeyCode.S);
        mobitel = Input.GetKey(KeyCode.D);

        //eliminira drzanje buttona i dizanje slidera preko 100
        if (Hunger > 100f)
        {
            Hunger = 100f;
        }
        else if (Thirst > 100f)
        {
            Thirst = 100f;
        }
        else if (San > 100f)
        {
            San = 100f;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            animacija.SetBool("pije", true);
            lik.SetActive(true);
            voda.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            animacija.SetBool("pije", false);
            lik.SetActive(false);
            voda.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            animacija.SetBool("pije", true);
            lik.SetActive(true);
            burger.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            animacija.SetBool("pije", false);
            lik.SetActive(false);
            burger.SetActive(false);
        }


        //eliminira double input, ako je jedan aktivan ostali ne mogu bit
        //kada se drzi button odredena radnja postaje true, i dodaje mu se vrijednost na value slidera
        if (jede)
        {
            Hunger += 0.1f;
            pije = false;
            mobitel = false;
        }
        else if (pije)
        {
            Thirst += 0.07f;
            jede = false;
            mobitel = false;
        }
        else if (mobitel)
        {
            San += 0.07f;
            jede = false;
            pije = false;
        }

        //GAME OVER
        if (Hunger <= 0 || Thirst <= 0 || San <= 0)
        {
            gameOver();
        }

        float capsuleYDegree = rotirajuciNino.transform.eulerAngles.y;

        if (capsuleYDegree < 45 && capsuleYDegree > -1)
        {
            onCamera = true;
        }
        else
        {
            onCamera = false;
        }

        if (onCamera)
        {
            if (jede || pije || mobitel)
            {

                gameOver();
            }


        }

        //SCORE
        timer += Time.deltaTime;
        if (timer >= kolikoTrebaProc)
        {
            timer = 0f;
            score++;
            UpdateScoreText();

        }



        void gameOver()//dobiva spremljeni high score iz filea
        {
            lik.SetActive(false);
            isGameOver = true;
            GameOverText.text = "Game Over";
            //kada je jede/pije/spava aktivan krug postaje unhiden

            if (score > highscore)
            {
                highscore = score;
                SaveHighScore(); //spremi score na memoriju highscorea
                //Debug.Log("New highscore achieved!");
                UpdateHighScoreText();
            }
            else
            {
                //Debug.Log("Game over. Score: " + score + " | Highscore remains: " + highscore);

            }

            //ENDAJ PROGRAM
        }

    }
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateHighScoreText()
    {
        highscoreText.text = "Highscore: " + highscore;
    }

    void LoadHighScore()
    {

        if (File.Exists(dataPath))
        {
            //Debug.Log("Loading highscore from: " + dataPath); // Debug: Check if the file is found
            string fileContents = File.ReadAllText(dataPath);
            //Debug.Log("File contents: " + fileContents); // Debug: Check what's inside the file
            if (int.TryParse(fileContents, out int loadedHighScore))//int highscorea
            {
                highscore = loadedHighScore;
                //Debug.Log("Highscore loaded: " + highscore); // Debug: Check if parsing was successful

            }
        }
        else//ako ne postoji 
        {
            SaveHighScore();
        }
    }
    void SaveHighScore()
    {
        try
        {
            // Write the high score to the file
            File.WriteAllText(dataPath, highscore.ToString());
            Debug.Log("Highscore saved successfully: " + highscore);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to save highscore: " + ex.Message);
        }
    }


}