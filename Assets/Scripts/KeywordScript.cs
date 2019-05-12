using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;

public class KeywordScript : MonoBehaviour {
    [SerializeField]
    private Rigidbody rb;
    public DogScript dogscript;
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    // Use this for initialization
    void Start() {

        actions.Add("sit", Sit);
        actions.Add("shit", Sit);
        actions.Add("set", Sit);
        actions.Add("soot", Sit);
        actions.Add("seat", Sit);
        actions.Add("sid", Sit);
        actions.Add("seed", Sit);
        actions.Add("what's that smell?", Sniff);
        actions.Add("what's that?", Sniff);
        actions.Add("scout the area", Sniff);
        actions.Add("anyone there?", Sniff);

        //m_Recognizer = new KeywordRecognizer(m_Keywords);
        //m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        //m_Recognizer.Start();
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Sit();
            dogscript.sniff = false;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            dogscript.sit = false;
            Sniff();
        }
    }
    // private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    //{
    // Debug.LogFormat("{0} ({1})", args.text, args.confidence);
    // }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Sit()
    {
        dogscript.sit = true;
        Debug.Log("Sit");
    }
    private void Sniff()
    {
        dogscript.sniff = true;
        Debug.Log("Sniff");
    }
}
