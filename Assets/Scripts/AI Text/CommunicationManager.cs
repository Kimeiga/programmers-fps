using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Word
{
    public string actualWord;
    public string partOfSpeech;
    public string verbType = null;


    public override bool Equals(System.Object obj)
    {
        // If parameter is null return false.
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to Point return false.
        Word w = obj as Word;
        if ((System.Object)w == null)
        {
            return false;
        }

        // Return true if the fields match:
        return (actualWord == w.actualWord);
    }


    public override int GetHashCode()
    {
        return actualWord.GetHashCode();
    }
}


public class CommunicationManager : MonoBehaviour {

    public GameObject[] players;


    public string receivedMessage;



    static List<Word> vocabulary = new List<Word>
    {
        new Word {actualWord = "lo",partOfSpeech = "verb", verbType = "subjobj" },
        new Word {actualWord = "i",partOfSpeech = "noun" },
        new Word {actualWord = "u",partOfSpeech = "noun" },
        new Word {actualWord = "e",partOfSpeech = "noun" },
        new Word {actualWord = "a",partOfSpeech = "noun" },
        new Word {actualWord = "o",partOfSpeech = "noun" },

    };



    void ReceiveTextMessage(string message) {
        receivedMessage = message;

        string[] words = message.Split(' ');

        List<Word> sentence = new List<Word> { };
  

        foreach(string word in words)
        {
            
            if(vocabulary.Contains(new Word { actualWord = word}))
            {
                sentence.Add(vocabulary.Find(x => x.actualWord.Contains(word)));

            }
        }
        

        if (sentence.Find(x => x.partOfSpeech.Contains("verb")) != null)
        {
            
            Word verb = sentence.Find(x => x.partOfSpeech.Equals("verb"));
            int verbIndex = sentence.IndexOf(verb);

            Word subjectOfVerb = null;
            Word objectOfVerb = null;

            if (verb.verbType == "subjobj")
            {
                subjectOfVerb = sentence[verbIndex - 1];
                objectOfVerb = sentence[verbIndex + 1];

                print(subjectOfVerb.actualWord.ToString());
                print(objectOfVerb.actualWord);
                

            }

            foreach(GameObject player in players)
            {
                if(player.name == subjectOfVerb.actualWord)
                {
                    TextBrain textBrainScript = player.GetComponent<TextBrain>();
                    textBrainScript.TakeCommand(verb, objectOfVerb, null);
                }
            }
            
        }




    }



}
