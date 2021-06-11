using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private Text textDisplay;
    [SerializeField] private string[] sentences;
    [SerializeField] private float typingSpeed;
    [SerializeField] private GameObject continueBtn;
    [SerializeField] private bool spaceKeyEvent;
    [SerializeField] private bool loadNextDirector = true;
    private bool canLoadNextDirector = true;
    private bool fastSpeed;
    private int index;
    private float startTypingSpeed;

    void Start()
    {
        StartCoroutine(Type());

        startTypingSpeed = typingSpeed;
    }
    void Update()
    {
        // if(textDisplay.text == sentences[index])
        // {
        //     continueBtn.SetActive(true);

        //     if(Input.GetKeyDown(KeyCode.Space))
        //         NextSentence();
        // }

        fastSpeed = Input.GetKey(KeyCode.Space);

        if(fastSpeed)
            typingSpeed = 0f;
        else
            typingSpeed = startTypingSpeed;

        if(Input.GetKeyDown(KeyCode.B))
            StartCoroutine(Type());

        if(textDisplay.text == sentences[index])
        {
            if(continueBtn != null)
            {
                continueBtn.SetActive(true);
            }
            else
            {
                // if(!spaceKeyEvent && !isTyping)
                //     StartCoroutine(TimeToNextSentence());
            }

            if(Input.GetKeyDown(KeyCode.Space) && spaceKeyEvent)
                NextSentence();
        }

        // if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey("return"))
        // {
        //     if(CutsceneHandler.instance != null)
        //     {
        //         if(canLoadNextDirector)
        //         {
        //             CutsceneHandler.instance.LoadNextDirector();
        //             canLoadNextDirector = false;
        //         }
        //     }
            
        //     Destroy(this.gameObject);
        // }
    }

    IEnumerator Type()
    {
        foreach(char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        if(continueBtn != null)
            continueBtn.SetActive(false);

        if(index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";

            if(continueBtn != null)
                continueBtn.SetActive(false);

            if(Time.timeScale < 1f)
                Time.timeScale = 1f;


            if(CutsceneHandler.instance != null)
            {
                if(canLoadNextDirector && loadNextDirector) //&& loadNextDirector
                {
                    CutsceneHandler.instance.LoadNextDirector();
                    canLoadNextDirector = false;
                }
            }
            
            Destroy(this.gameObject);
        }
    }

    IEnumerator TimeToNextSentence()
    {
        yield return new WaitForSeconds(0.8f);
        NextSentence();
    }
}
