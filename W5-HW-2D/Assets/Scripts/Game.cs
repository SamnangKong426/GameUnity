using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    Image imgYou;       // your selected image (rock, paper scissor)
    Image imgCom;       // computer selected image (rock, paper scissor)
    Image imgBoom;

    Text youScore;        // the score you win
    Text ComScore;        // the score computer win
    Text txtResult;     // the result

    int cntYou = 0;     // number you win
    int cntCom = 0;     // number computer win

    private void InitGame()
    {
        imgYou = GameObject.Find("ImgYou").GetComponent<Image>();
        imgCom = GameObject.Find("ImgCom").GetComponent<Image>();
        imgBoom = GameObject.Find("ImgBoom").GetComponent<Image>();

        youScore = GameObject.Find("YouScore").GetComponent<Text>();
        ComScore = GameObject.Find("ComScore").GetComponent<Text>();
        txtResult = GameObject.Find("TxtResult").GetComponent<Text>();

        //init the text before the game start
        txtResult.text = "Select the button below";
        imgBoom.enabled = false;
    }

    void Start()
    {
        // init the game
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        //exit if press escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void OnButtonClick(GameObject buttonObject)
    {
        //event when button is clicked
        int you = int.Parse(buttonObject.name.Substring(0, 1));
        CheckResult(you);
        Debug.Log("clicked: " + you);
    }

    void CheckResult(int yourResult)
    {
        // algorithm determine the result

        int comResult = UnityEngine.Random.Range(1, 4);
        int k = yourResult - comResult;
        if (k == 0)
        {
            txtResult.text = "Draw.";
        }
        else if (k == 1 || k == -2)
        {
            cntYou++;
            txtResult.text = "You win.";
        }
        else
        {
            cntCom++;
            txtResult.text = "Computer win.";
        }
        SetResult(yourResult, comResult);    // set game result to UI
    }

    void SetResult(int you, int com)
    {
        // Define the range for initial positions
        float leftX = -Screen.width;
        float rightX = Screen.width;
        float centerY = 0;
        imgBoom.enabled = false;

        // Set initial positions
        imgYou.rectTransform.anchoredPosition = new Vector3(leftX, centerY, 0);
        imgCom.rectTransform.anchoredPosition = new Vector3(rightX, centerY, 0);

        // Change image
        imgYou.sprite = Resources.Load("img_" + you, typeof(Sprite)) as Sprite;
        imgCom.sprite = Resources.Load("img_" + com, typeof(Sprite)) as Sprite;

        // Invert image com in x axis
        imgCom.transform.localScale = new Vector3(-1, 1, 1);

        // Winning score
        youScore.text = cntYou.ToString();
        ComScore.text = cntCom.ToString();

        StartCoroutine(AnimationResult());
    }

    IEnumerator AnimationResult()
    {
        Vector3 centerPosition = new Vector3(0, 290, 0);
        float duration = 1.0f; // Duration of the animation
        float elapsedTime = 0;

        Vector3 startYouPos = imgYou.rectTransform.anchoredPosition;
        Vector3 startComPos = imgCom.rectTransform.anchoredPosition;

        while (elapsedTime < duration)
        {
            imgYou.rectTransform.anchoredPosition = Vector3.Lerp(startYouPos, centerPosition, elapsedTime / duration);
            imgCom.rectTransform.anchoredPosition = Vector3.Lerp(startComPos, centerPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        imgYou.rectTransform.anchoredPosition = centerPosition;
        imgCom.rectTransform.anchoredPosition = centerPosition;

        imgBoom.enabled = true;
    }
}
