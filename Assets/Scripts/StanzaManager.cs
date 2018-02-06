using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class StanzaManager : MonoBehaviour {

    public SManager sceneManager;
    public List<Stanza> stanzas;


	private bool autoPlaying = false;
	private bool cancelAutoPlay = false;

	public volatile bool drag = false;
	public TextAsset xmlStanzaData;

	void Start() {
		GetComponent<AudioSource>().GetComponent<AudioSource>().volume = 0.25f;
	}
	// Load up the scene defined xml timing data
	public void LoadStanzaXML()
	{
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(xmlStanzaData.text);
		XmlNodeList wordsList = xmlDoc.GetElementsByTagName("word");

		Debug.Log("xml loaded");
		SetupWordTimings(wordsList);
	}

	// Loads up a custom stanza audio mp3 and xml timing data
	public void LoadStanzaAudioAndXML(string audioFilename, string xmlFilename)
	{
		// Load our audio
		AudioClip stanzaClip = (AudioClip)Resources.Load(audioFilename) as AudioClip;
		GetComponent<AudioSource>().clip = stanzaClip;

		// Load our xml
		xmlStanzaData = (TextAsset)Resources.Load(xmlFilename);
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(xmlStanzaData.text);
		XmlNodeList wordsList = xmlDoc.GetElementsByTagName("word");

		SetupWordTimings(wordsList);
	}

	// Goes through all stanzas and tinkertexts assigning the word timings
	private void SetupWordTimings(XmlNodeList wordsList)
	{
		int stanzaIndex = 0;
		int wordIndex = 0;
		int relativeWordIndex = 0;

		Debug.Log("timing");
		while (wordIndex < wordsList.Count)
		{
			stanzas[stanzaIndex].tinkerTexts[relativeWordIndex].SetupWordTiming(wordsList[wordIndex]);

			wordIndex++;
			relativeWordIndex++;

			// Hit end of stanza yet?
			if (relativeWordIndex > stanzas[stanzaIndex].tinkerTexts.Count - 1)
			{
				relativeWordIndex = 0;
				stanzaIndex++;

				// If we are in a new valid stanza
				if (stanzaIndex < stanzas.Count)
				{
					// Calculate and set our end delay based on when last word in stanza ends and when first word of next stanza begins
					float firstWordStartTime = float.Parse(wordsList[wordIndex].Attributes["msStart"].Value) / 1000.0f;
					float lastWordEndTime = float.Parse(wordsList[wordIndex - 1].Attributes["msEnd"].Value) / 1000.0f;
					stanzas[stanzaIndex - 1].endDelay = firstWordStartTime - lastWordEndTime;
					Debug.Log("stanza time"+stanzas[stanzaIndex-1].endDelay);
				}
			}
		}
	}
	


	// Whether we are currently autoplaying stanzas
	public bool IsAutoPlaying()
	{
		return autoPlaying;
	}

	public bool IsDrag(){
		return drag;
	}

	// Method to request an auto play starting w/ a stanza
	public void RequestAutoPlay(Stanza startingStanza, TinkerText startingTinkerText = null)
	{
		Debug.Log ("request"+autoPlaying);
		if (!autoPlaying)  // && !sceneManager.disableAutoplay)
		{
			autoPlaying = true;
			cancelAutoPlay = false; // reset our cancel flag
			StartCoroutine(StartAutoPlay(startingStanza, startingTinkerText));
		}
	}

	// Method to request cancelling an autoplay
	public void RequestCancelAutoPlay()
	{
		if (autoPlaying)
		{
			cancelAutoPlay = true;
		}
	}

	// Whether there is a cancel request in progress
	public bool CancelAutoPlay()
	{
		return cancelAutoPlay;
	}


	// Begins an auto play starting w/ a stanza
	private IEnumerator StartAutoPlay(Stanza startingStanza, TinkerText startingTinkerText)
	{
		// If we aren't starting from the beginning, read the audio progress from the startingTinkerText
		GetComponent<AudioSource>().time = startingTinkerText.GetStartTime();
		Debug.Log ("start time:"+startingTinkerText+startingTinkerText.GetStartTime ());
		// Start playing the full stanza audio
		GetComponent<AudioSource>().Play();

		int startingStanzaIndex = stanzas.IndexOf(startingStanza);
		Debug.Log ("time:"+startingTinkerText.GetStartTime ()+"index:"+startingStanzaIndex);
		for (int i = startingStanzaIndex; i < stanzas.Count; i++)
		{
			if (i == startingStanzaIndex)
			{
				yield return StartCoroutine(stanzas[i].AutoPlay(startingTinkerText));
			}
			else
			{
				yield return StartCoroutine(stanzas[i].AutoPlay());
			}

			// Abort early?
			if (CancelAutoPlay())
			{
				autoPlaying = false;
				GetComponent<AudioSource>().Stop();
				yield break;
			}
		}

		autoPlaying = false;
		yield break;
	}
	public void OnDragBegin(TinkerText tinkerText) {

		drag = true;
		if (tinkerText.stanza != null && stanzas.Contains(tinkerText.stanza))
		{
			tinkerText.stanza.OnDrag(tinkerText);
		}

	}

	public void OnDragEnd(){
		drag = false;
	}

    public void OnMouseUp(TinkerText tinkerText) {
        if (tinkerText.stanza != null && stanzas.Contains(tinkerText.stanza))
        {
			GetComponent<AudioSource>().Stop();
            tinkerText.stanza.OnMouseUp(tinkerText);
        }

    }


}
