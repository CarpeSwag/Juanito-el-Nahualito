using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {
	public string text;
	public int timer;
	public AudioClip audioClip;
	public bool isMovementLocked;
	
	public Dialogue(string diaText, int diaTimer, AudioClip diaAudio, bool movementLocked) {
		text = diaText;
		timer = diaTimer;
		audioClip = diaAudio;
		isMovementLocked = movementLocked;
	}
	
	public Dialogue(string diaText) : this(diaText, 0, null, false) {}
}

public class UIManager : MonoBehaviour
{
	public static UIManager instance = null; // Allows us to access this from other scripts
	
	// Menu Related
	public GameObject inGameMenu;
	private bool menuToggleProtect;
    private bool isMenuOpen;
	
	// Dialogue System
	private Queue dialogueQueue;
	public GameObject dialogueUI;
	private bool dialogueToggleProtect;
    private bool isDialogueOpen;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		isMenuOpen = false;
		menuToggleProtect = false;
		
		dialogueQueue = new Queue();
		dialogueToggleProtect = false;
		isDialogueOpen = false;
	}
	
    void Start()
    {
		DynamicGI.UpdateEnvironment(); // Get rid of this eventually
	}
	
	void Update()
	{
		if (Input.GetKey(KeyCode.Escape)) {
			if (!menuToggleProtect) {
				isMenuOpen = !isMenuOpen;
			}
			menuToggleProtect = true;
		} else if (menuToggleProtect) {
			menuToggleProtect = false;
		}
		
		if (Input.GetKey(KeyCode.Return)) {
			if (!dialogueToggleProtect) {
				doDialogueAction();
			}
			dialogueToggleProtect = true;
		} else if (dialogueToggleProtect) {
			dialogueToggleProtect = false;
		}
		
		inGameMenu.gameObject.SetActive(isMenuOpen);
		dialogueUI.gameObject.SetActive(isDialogueOpen);
	}
	
	public void closeMainMenu()
	{
		isMenuOpen = false;
	}
	
	
	  ////////////////////////////////
	 //  Dialogue Related Actions  //
	////////////////////////////////
	
	
	private void updateDialogueText()
	{
		isDialogueOpen = dialogueQueue.Count > 0;
		if (isDialogueOpen) {
			Dialogue dialogue = (Dialogue) dialogueQueue.Peek();
			GameManager.instance.isMovementLocked = dialogue.isMovementLocked;
			dialogueUI.transform.Find("Dialogue").GetComponent<Text>().text = dialogue.text;
		} else {
			GameManager.instance.isMovementLocked = false;
		}
	}
	
	private void doDialogueAction()
	{
		if (dialogueQueue.Count > 0) {
			dialogueQueue.Dequeue();
		}
		updateDialogueText();
	}
	
	
	public void addDialogue(Dialogue dialogue) {
		dialogueQueue.Enqueue(dialogue);
		updateDialogueText();
	}
	
	public void addDialogue(Dialogue[] dialogue) {
		for (int i = 0; i < dialogue.Length; ++i) {
			dialogueQueue.Enqueue(dialogue[i]);
		}
		updateDialogueText();
	}
	
	public void addDialogue(string[] texts, int[] timers, AudioClip[] audioClips, bool[] movementLocked) {
		for (int i = 0; i < texts.Length; ++i) {
			string text = texts[i];
			int timer = (i < timers.Length)? timers[i]: 0;
			AudioClip audioClip = (i < audioClips.Length)? audioClips[i]: null;
			bool isMovementLocked = !(i >= movementLocked.Length || !movementLocked[i]);
			dialogueQueue.Enqueue(new Dialogue(text, timer, audioClip, isMovementLocked));
		}
		updateDialogueText();
	}
	
	// Overloaded functions
	public void addDialogue(string text) {
		this.addDialogue(new Dialogue(text));
	}
	public void addDialogue(string text, int timer, AudioClip audioClip, bool movementLocked) {
		this.addDialogue(new Dialogue(text, timer, audioClip, movementLocked));
	}
	public void addDialogue(string[] texts) {
		this.addDialogue(texts, new int[0], new AudioClip[0], new bool[0]);
	}
}
