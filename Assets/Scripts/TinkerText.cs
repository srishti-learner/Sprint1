using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
    
public class TinkerText : MonoBehaviour {
   
	// Auto play timing related
	private float startTime;
	private float endTime;
	private float delayTime;
	// public Text tinkerText;
    public Stanza stanza;
    
    public Animator a;
    public Animation anim;


    // Use this for initialization
    void Start () {
        a = gameObject.GetComponent<Animator>();
    }


	// Takes an xml word element and reads and sets the timing data
	public void SetupWordTiming(XmlNode wordNode)
	{
		startTime = float.Parse(wordNode.Attributes["msStart"].Value) / 1000.0f;
		endTime = float.Parse(wordNode.Attributes["msEnd"].Value) / 1000.0f;
		delayTime = endTime - startTime;
	}

	// Returns the absolute start time
	public float GetStartTime()
	{
		return startTime;
	}

	// Returns the absolute end time
	public float GetEndTime()
	{
		return endTime;
	}

	// Mouse Down Event
	public void OnMouseDown()
	{
		clipPlay();
	}

	public void OnMouseUp()
	{
		a.speed = 1/delayTime;
		clipPlay();
	}


    void clipPlay()
    {
        AudioSource source = gameObject.GetComponent<AudioSource>();
        float delaytime = 0.21f;
        a.speed = 1 / delaytime;
        //play sound
        source.Play();
        //play animation
        a.Play("textzoomout");
        a.speed = 0;
    }

}
