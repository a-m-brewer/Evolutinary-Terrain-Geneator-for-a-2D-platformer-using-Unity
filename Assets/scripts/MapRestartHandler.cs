using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapRestartHandler : MonoBehaviour {

    public GameObject UICanvas;
    public GameObject PostLevelCanvas;
    private Button button;
    private InputField input;
    public MapGeneratorMain map;
    public Player player;
    private bool hasEnded = false;
    private bool hasEnteredText = false;
    private bool buttonClicked = false;
    private string inputNum;

    private void Awake()
    {
        button = PostLevelCanvas.transform.Find("RestartGameButton").GetComponent<Button>();
        input = PostLevelCanvas.transform.Find("InputField").GetComponent<InputField>();
    }

    private void Start()
    {
        button.onClick.AddListener(ButtonClicked);

        InputField.SubmitEvent se = new InputField.SubmitEvent();
        se.AddListener(OnInputChange);
        input.onEndEdit = se;
    }

    private void Update()
    {
        hasEnded = player.levelEnded;
        
        if (hasEnded)
        {
 
            Pause(hasEnded);
            SwithToPauseUI(true);
           
            if (CanRestatLevel())
            {
                VariableReset();
                LoadNewMap(DiffStringToInt());
            }
        }
    }

    private void VariableReset()
    {
        hasEnded = false;
        player.levelEnded = false;
        hasEnteredText = false;
        buttonClicked = false;
        input.text = "";
    }

    private void SwithToPauseUI(bool pauseMenuOn)
    {
        UICanvas.SetActive(!pauseMenuOn);
        PostLevelCanvas.SetActive(pauseMenuOn);
    }

    private void Pause(bool on)
    {
        if(on)
        {
            Time.timeScale = 0f;
        } else
        {
            Time.timeScale = 1f;
        }
    }

    private void LoadNewMap(int newDifficulty)
    {
        SetDifficultyScore(newDifficulty);
        SwithToPauseUI(false);
        player.transform.position = new Vector3(3f, 1.5f, player.transform.position.z);
        map.GenerateMap(new Vector2(24f, 10f));
        player.ResetHealth();
        Pause(false);
    }

    private void SetDifficultyScore(int score)
    {
        map.mapTargetDifficulty = score;
    }

    private void ButtonClicked()
    {
        if(hasEnteredText)
        {
            buttonClicked = true;
        }
    }

    private void OnInputChange(string input)
    {
        Debug.Log(input);
        hasEnteredText = true;
        inputNum = input;
    }

    public bool CanRestatLevel()
    {
        return hasEnteredText && buttonClicked;
    }

    public int DiffStringToInt()
    {
        int output = 0;
        int.TryParse(inputNum, out output);
        return output;
    }

}
