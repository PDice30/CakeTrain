using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.GetComponent<Button>().onClick.AddListener(startOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void startOnClick() {
        SceneManager.LoadScene("MainScene");
    }
}
