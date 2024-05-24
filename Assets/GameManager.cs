using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    string secretWord;
    int secretWordLength;
    bool[] secretCharVisible;
    char[] secretChars;
    int wrongGuesses = 0;
    int maxGuesses;
    bool alreadyGuessedLetter;
    [SerializeField] public WordFinder wordFinder;
    [SerializeField] public GameObject GOpanel;
    [SerializeField] public Text GOText;
    [SerializeField] public Text GuessedText;

    public List<char> guessedLetters = new List<char>();

    public Text secretWordText;
    public Text infoText;

    // Start is called before the first frame update
    void Start()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        // Clears the secret word's text box
        secretWordText.text = "";

        // Gets a new random word
        secretWord = wordFinder.GetRandomWord();
        Debug.Log("Secret word is " +  secretWord);

        // Gets the word length
        secretWordLength = wordFinder.GetWordLength();
        Debug.Log("Word length is " + secretWordLength);

        // Sets the max guesses to word length + 4
        maxGuesses = secretWordLength + 4;

        // Creates a bool array for setting the visibility of each character of the word
        secretCharVisible = new bool[secretWordLength];

        // Adds the secret word into the char array
       secretChars = secretWord.ToCharArray();

        //Resets the info text
        infoText.text = "Guess the word...";

        //Resets incorrect guess count
        wrongGuesses = 0;

        //sets each character to invisible in the secret word
        for (int i = 0; i < secretCharVisible.Length; i++)
        {
            secretCharVisible[i] = false;
        }

        // Sets the correct visibility for each char of the secret word
        UpdateText();

        //secretWordText.text = secretWord;
        Debug.Log("Text should show " + secretWord);

        GOpanel.SetActive(false);
        alreadyGuessedLetter = false;

        guessedLetters.Clear();
    }

    public void UpdateText()
    {
        secretWordText.text = "";
        int j = 0;
        bool solved = true;
        foreach (bool vis in secretCharVisible)
        {
            if (vis)
            {
                secretWordText.text += secretChars[j];
            }
            else
            {
                secretWordText.text += " _ ";
                solved = false;
            }
            j++;
        }
        if (solved)
        {
            Debug.Log("SOLVED!");
            GOpanel.SetActive(true);
            GOText.text = "You win!\r\nPlay again?";
            infoText.text = "Congrats! " + (maxGuesses - wrongGuesses) + " guesses unused";
        } else if (wrongGuesses >= maxGuesses)
        {
            wrongGuesses = maxGuesses;
            GOpanel.SetActive(true);
            GOText.text = "You lose!\r\nPlay again?";
        }
        string guessedString = "";
        foreach(char c in guessedLetters)
        {
            guessedString += c.ToString() + ", ";
        }
        GuessedText.text = "Guessed letters:\r\n" + guessedString;
    }

    // Takes a guessed letter and returns if it's a correct guess or not
    // Also sets visibility for correctly guessed letters
    public bool GuessLetter(char letter)
    {
        int i = 0;
        bool correctGuess = false;
        foreach(char c in secretChars)
        {
            // If letter is not yet discovered and it's correct
            if (!secretCharVisible[i] && c == letter)
            {
                secretCharVisible[i] = true;
                correctGuess = true;
            } else if (secretCharVisible[i] && letter == c)
            {
                alreadyGuessedLetter = true;
            }
            i++;
        }

        // bool for tracking if a letter has already been added to the guessed list
        bool guessed = false;
        // Add letter to guessed letters if not already added
        foreach (char c in guessedLetters)
        {
            if (c == letter)
            {
                guessed = true;
                alreadyGuessedLetter = true;
            }
        }
        if (!guessed)
        {
            guessedLetters.Add(letter);
            
        }
        return correctGuess;
    }

    public void submitGuess(string guess)
    {
        Debug.Log("Guessed word " + guess);
        if (guess.Length == 1)
        {
            if (GuessLetter(guess[0]))
            {
                // Correct guess
                infoText.text = "Correct!";
                alreadyGuessedLetter = false;
            } else if (!alreadyGuessedLetter)
            {
                // Wrong guess
                wrongGuesses++;
                infoText.text = "Nope! " + (maxGuesses - wrongGuesses) + " guesses remaining!";
                if(wrongGuesses >= maxGuesses)
                {
                    infoText.text = "No Guesses remaining. Man hanged!";
                }
            }
            if (alreadyGuessedLetter)
            {
                infoText.text = "Already guessed letter!";
                alreadyGuessedLetter = false;
            }
        }
        UpdateText();
    }
}

/******************************************************************************

TODO:
Main menu
Popup when you win DONE
Feedback for guesses text box DONE

******************************************************************************/