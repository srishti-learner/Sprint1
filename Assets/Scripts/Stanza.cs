using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stanza : MonoBehaviour {
    public StanzaManager stanzaManager;
    public List<TinkerText> tinkerTexts;

    // Time delay at end of stanza during autoplay
    public float endDelay;

    private TinkerText mouseDragBeginTinkerText;
    private TinkerText mouseDragEndTinkerText;

	private TinkerText mouseDownTinkerText;
	private TinkerText mouseUpTinkerText;
    // Use this for initialization
    void Start () {
		
	}

	// Auto play the word audio in this stanza
	public IEnumerator AutoPlay(TinkerText startingTinkerText = null)
	{
		int startingTinkerTextIndex = 0;

		if (startingTinkerText != null)
		{
			startingTinkerTextIndex = tinkerTexts.IndexOf(startingTinkerText);
		}

		for (int i = startingTinkerTextIndex; i < tinkerTexts.Count; i++)
		{
			// delay according to timing data
			//animation not integrated
			//yield return new WaitForSeconds(tinkerTexts[i].GetAnimationDelay());

			// If we aren't on last word, delay before playing next word
			if (i < tinkerTexts.Count - 1)
			{
				float pauseDelay = tinkerTexts[i + 1].GetStartTime() - tinkerTexts[i].GetEndTime();

				yield return new WaitForSeconds(pauseDelay);
			}
			else // Delay before next stanza
			{
				yield return new WaitForSeconds(endDelay);
			}

			// Abort early?
			if (stanzaManager.CancelAutoPlay())
			{
				yield break;
			}
		}

		// Stop the coroutine
		yield break;
	}



	public void OnDrag(TinkerText tinkerText){
		
		// and reassign it to currently down 
		int currentTextIndex = tinkerTexts.IndexOf (tinkerText);

		stanzaManager.RequestAutoPlay(this, tinkerTexts[currentTextIndex]);
	}


    public void OnMouseUp(TinkerText tinkerText)
    {
		// Assign this new one
		mouseDownTinkerText = tinkerText;
		// And signal the tinkerTextt
		if (!stanzaManager.IsDrag ()) {
			tinkerText.OnMouseUp ();
		}
    }
    public void OnMouseDown(TinkerText tinkerText)
    {
        
    }



}
