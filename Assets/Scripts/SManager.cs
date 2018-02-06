using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SManager : MonoBehaviour {
    [HideInInspector]
    public GameManager gameManager;
	public List<Stanza> stanzas;

    // Manager for all things TinkerText
    public StanzaManager stanzaManager;
	public bool disableAutoplay;
    // Use this for initialization
    void Start () {
		
	}

	public virtual void Init(GameManager _gameManager)
	{
		gameManager = _gameManager;
		disableAutoplay = false;

		// If we have a stanza manager
	    if (stanzaManager != null){
			// And it has an audio clip and xml defined already in the scene
			if (stanzaManager.xmlStanzaData != null && stanzaManager.GetComponent<AudioSource>().clip != null)
			{
				// Then have it set the xml up
				stanzaManager.LoadStanzaXML();
			}
	    }
   }

	public virtual void OnDragBegin(GameObject go)
	{
		
		if (go.tag == "text")
		{
			stanzaManager.LoadStanzaXML();
			if (stanzaManager == null) {
				stanzaManager = GameObject.Find ("StanzaManager").GetComponent<StanzaManager>();
			}
			if (stanzaManager != null)
			{Debug.Log ("begin  again2");
				stanzaManager.OnDragBegin(go.GetComponent<TinkerText>());
				Debug.Log ("begin  again");
			}

		}

	}

	public virtual void OnDragEnd()
	{
		if (stanzaManager != null)
		{
			stanzaManager.OnDragEnd();
		}

	}

    public virtual void OnMouseUp(GameObject go)
    {
        if (go.tag == "text") {
			if (stanzaManager == null) {
				stanzaManager = GameObject.Find ("StanzaManager").GetComponent<StanzaManager>();
			}
           if (stanzaManager != null)
                {
                   stanzaManager.OnMouseUp(go.GetComponent<TinkerText>());
                }
            


        }
        
    }
    
}
