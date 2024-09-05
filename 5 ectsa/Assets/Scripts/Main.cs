using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public bool jede = false, mobitel = false, pije = false;
    public bool onCamera = false;
    public int score = 0;
    private int highscore = 100; // dobiva int iz filea, stavit u  start da dobiva iz filea
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
    void Start()
    {
        //tekst koji oznacava game over je uvijek hidden na pocetku
        kraj.SetActive(false);
        //na pocetku igre hunger je na 100%(nisi gladan)
        Hunger = 100f;
        Thirst = 100f;
        San = 100f;
    }
    void Update()//svaki frame
    {
        float lossHunger = Random.Range(4, maxLoss);
        float lossThirst = Random.Range(3, maxLoss);
        float lossSan = Random.Range(4, maxLoss);

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
        if(Hunger>100f)
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
            Thirst += 0.05f;
            jede = false;
            mobitel = false;
        }
        else if (mobitel)
        {
            San += 0.05f;
            jede = false;
            pije = false;
        }


        if(Hunger<=0 || Thirst<=0 || San <= 0)
        {
            gameOver(highscore, score);
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

                gameOver(highscore, score);
            }


        }

        timer += Time.deltaTime;
        if (timer >= kolikoTrebaProc)
        {
            timer = 0f;
            score++;

        }

        void gameOver(int highscore, int score)//dobiva spremljeni high score iz filea
        {
            //kada je jede/pije/spava aktivan krug postaje unhiden
            kraj.SetActive(true);

            if (score > highscore)
            {
                highscore = score;
                //spremi score na memoriju highscorea
                // print NEW HIGH SCORE
                // UI ZA GAME OVER
                // UI ZA HIGH SCORE (best score:highscore)
            }
            else
            {
                // UI ZA GAME OVER
                //print score: best score: highscore
                //             current run: score
            }

            //ENDAJ PROGRAM
        }

    }

}
