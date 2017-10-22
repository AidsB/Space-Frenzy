using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public Text instruction;
    public Text countdown;
    public float minTime = 8f;
    
    float timeLeft = 0f;
    int completed = 0;
    Event currentEvent;

    bool gameOver = false;

    float dedtimer = 0;

	void Start () {
        NewEvent();
    }
    
    void Update () {
        if (!gameOver) {
            timeLeft -= Time.deltaTime;

            if (currentEvent.isDone()) {
                completed++;
                NewEvent();
            }

            countdown.text = ((int)(timeLeft + .5f)).ToString();

            if (timeLeft <= 0) {
                countdown.text = "0";
                instruction.text = "You ded";
                gameOver = true;
            }
        }
        else {
            dedtimer += Time.deltaTime;
            if (dedtimer > 5f)
                UnityEngine.SceneManagement.SceneManager.LoadScene("ship", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
	}

    void NewEvent() {
        timeLeft = Mathf.Exp(-.1f * completed + 2f) + minTime;
        currentEvent = EventDatabase.GetEvent();
        instruction.text = currentEvent.text;
    }
}
