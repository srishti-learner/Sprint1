using UnityEngine.EventSystems;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IPointerUpHandler, IPointerClickHandler,
    IPointerExitHandler, IPointerEnterHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler{
    
    
    // Use this for initialization

    public SManager sceneManager;                       // Reference to SManager for each scene
    public static bool mousepressed = false;
    [HideInInspector]
    public enum Scenes                                      // Place all scene names here in order
    {
        Init,
        Intro,
        Scene01,
        Scene02,
        Scene03,
		END
    }
    public Scenes currentScene;
    public static GameManager Instance
    {
        get { return GameManager.instance; }
    }
    // access to the singleton
    private static GameManager instance;
    // this is called after Awake() OR after the script is recompiled (Recompile > Disable > Enable)
    private void Init()
    {
        // Assign our current scene on one-time init so we can support starting game from any scene during testing
        currentScene = (Scenes)Enum.Parse(typeof(Scenes), SceneManager.GetActiveScene().name);
	}
		
    protected virtual void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (instance == null)
        {
            instance = this;

            Init();
        }
        else if (instance != this)
        {
            Debug.LogWarning("GAME MANAGER: WARNING - THERE IS ALREADY AN INSTANCE OF GAME MANAGER RUNNING - DESTROYING THIS ONE.");
            Destroy(this.gameObject);
            return;
        }
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called each time a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        Debug.Log("LEVEL WAS LOADED: " + SceneManager.GetActiveScene().name);
        //AndroidBroadcastIntentHandler.BroadcastJSONData("scene", SceneManager.GetActiveScene().name);
        LoadSceneManager();
    }

    private void LoadSceneManager()
    {
        // Grab the current SManager GameObject (if it exists)
        GameObject sceneManagerGO = GameObject.Find("SceneManager");
	
        if (sceneManagerGO != null)
        {
            sceneManager = sceneManagerGO.GetComponent<SManager>();

            if (sceneManager != null)
            {
                sceneManager.Init(this);

            }
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnBeginDrag(PointerEventData eventData)
    {
		GameObject go = GameObject.Find(eventData.pointerCurrentRaycast.gameObject.name);
		if (go != null) {
			sceneManager.OnDragBegin (go);
		}
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
		sceneManager.OnDragEnd ();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject go=eventData.pointerCurrentRaycast.gameObject;
        if (go != null)
        {
            sceneManager.OnMouseUp(eventData.pointerCurrentRaycast.gameObject);
        }
      
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    
}
