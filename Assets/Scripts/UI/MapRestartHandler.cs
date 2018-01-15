using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Used to restart the level and bring up the difficutly selection
 * menu
 */ 
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

    // links to input and button
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

    // check every frame if the level has ended
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

    // resest the level back to it's orginal state
    private void VariableReset()
    {
        hasEnded = false;
        player.levelEnded = false;
        hasEnteredText = false;
        buttonClicked = false;
        input.text = "";
    }

    // switch from the ingame ui to the difficulty select UI
    private void SwithToPauseUI(bool pauseMenuOn)
    {
        UICanvas.SetActive(!pauseMenuOn);
        PostLevelCanvas.SetActive(pauseMenuOn);
    }

    // toggle game pause
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

    // load the new map based on the difficulty given
    private void LoadNewMap(int newDifficulty)
    {
        SetDifficultyScore(newDifficulty);
        SwithToPauseUI(false);
        player.transform.position = new Vector3(3f, 1.5f, player.transform.position.z);
        map.GenerateMap(new Vector2(24f, 10f));
        player.ResetHealth();
        player.ResetScore();
        Pause(false);
    }

    // pass the target score to the map generator
    private void SetDifficultyScore(int score)
    {
        map.mapTargetDifficulty = score;
    }

    // if the button has been clicked do the following
    private void ButtonClicked()
    {
        if(hasEnteredText)
        {
            buttonClicked = true;
        }
    }

    // when the input box is used set the input number
    private void OnInputChange(string input)
    {
        hasEnteredText = true;
        inputNum = input;
    }

    // if these are both true the level can be restarted
    public bool CanRestatLevel()
    {
        return hasEnteredText && buttonClicked;
    }

    // try and convert the input from string to int if not score is -1 to show error
    public int DiffStringToInt()
    {
        int output = -1;
        int.TryParse(inputNum, out output);
        return output;
    }

}
