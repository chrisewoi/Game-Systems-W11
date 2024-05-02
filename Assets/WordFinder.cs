using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordFinder : MonoBehaviour
{
    // Initialise a new list of strings
    public List<string> possibleWords = new List<string>();

    public Text wordsDisplay;

    // Start is called before the first frame update
    void Start()
    {
        // Load a .txt file from the Resources folder
        TextAsset allWords = Resources.Load("words") as TextAsset;

        // Split up the string from the file and store the results in an array
        string[] words = allWords.text.Split(',');

        foreach (string currentWord in words)
        {
            if(currentWord.Length > 3)
            {
                possibleWords.Add(currentWord);
            }
        }

        //possibleWords = new List<string>(words);
    }

    public void SearchString(string inputString)
    {
        // Contain all the woords we find that have the input string inside
        string wordsFound = "";
        wordsDisplay.text = "";

        foreach(string currentWord in possibleWords)
        {
            if (currentWord.Contains(inputString))
            {
                wordsFound += currentWord + ", ";
            }
        }

        wordsFound = wordsFound.Remove(wordsFound.LastIndexOf(','));
        // Display the results to the user
        wordsDisplay.text = wordsFound;
    } 


    public void SearchCharacters(string inputCharacters)
    {
        string wordsFound = "";

        char[] inputCharArray = inputCharacters.ToCharArray();

        foreach(string currentWord in possibleWords)
        {
            bool containsAllSoFar = true;

            foreach (char character in inputCharArray)
            {
                if (!currentWord.Contains(character))
                {
                    containsAllSoFar = false;
                    break; // <--- end the iteration early
                }
            }

            if(!containsAllSoFar)
            {
                continue; // <---- stop this iteration and continue with the next iteration
            }

            wordsFound += currentWord + ", ";
        }
        // Update wordsFound to remove evertything including and after the last comma
        wordsFound = wordsFound.Remove(wordsFound.LastIndexOf(','));
        wordsDisplay.text = wordsFound;
    }
}
