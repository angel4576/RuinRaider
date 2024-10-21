using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Transform playerTrans; // player & scene loader are persistent
    public Vector3 initialPos;
    public Vector3 menuPos;

    [Header("Event Listener")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO newGameSO;

    [Header("Boardcast")]
    public VoidEventSO afterSceneLoadEvent;

    [Header("Scene")]
    public GameSceneSO menuScene;
    public GameSceneSO firstLoadScene; 
    private GameSceneSO currentLoadScene;
    private GameSceneSO sceneToLoad;


    private Vector3 posToGo;
    private bool haveEffect;
    public float waitDuration;
    public bool isLoading;

    private void Awake() 
    {
        // need to get Asset reference in GameSceneSO
        // use asyn to load the first scene
        
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive); 
        //firstLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);

        

    }

    private void Start() 
    {
        // load menu when start
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPos, true);
        // NewGame();    
    }

    // register 
    private void OnEnable() 
    {
        loadEventSO.loadRequestEvent += OnLoadRequest;
        newGameSO.OnEventRaised += NewGame;
    }

    // unregister
    private void OnDisable() 
    {
        loadEventSO.loadRequestEvent -= OnLoadRequest;
        newGameSO.OnEventRaised -= NewGame;
    }

    private void NewGame()
    {
        // sceneToLoad = firstLoadScene;
        // OnLoadRequest(firstLoadScene, initialPos, true);
        loadEventSO.RaiseLoadRequestEvent(firstLoadScene, initialPos, true);
    }

    /// <summary>
    /// Scene Load Request
    /// </summary>
    /// <param name="gameScene"></param>
    /// <param name="pos"></param>
    /// <param name="haveEffect"></param>
    private void OnLoadRequest(GameSceneSO gameScene, Vector3 pos, bool haveEffect)
    {
        if(isLoading)
            return;
        
        isLoading = true;

        sceneToLoad = gameScene;
        posToGo = pos;
        this.haveEffect = haveEffect;

        if(currentLoadScene != null)
        {
            StartCoroutine(UnLoadScene());
        }
        else // when game starts, current scene is null
        {
            LoadNewScene(); 
        }
        
    }

    private IEnumerator UnLoadScene()
    {
        if(haveEffect) 
        {

        }

        // wait for effect to finish before unload
        yield return new WaitForSeconds(waitDuration); 
            
        // wait until scene to finish unloading before load new scene
        yield return currentLoadScene.sceneReference.UnLoadScene(); // use resources itself to un/load
    
        // Close player after unload
        playerTrans.gameObject.SetActive(false);

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadOperation = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        loadOperation.Completed += OnLoadComplete; // make sure scene is completely loaded
    }

    private void OnLoadComplete(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadScene = sceneToLoad; // update current scene

        //update player position in the new scene
        playerTrans.position = posToGo;

        // if(currentLoadScene.sceneType == SceneType.Location)
        playerTrans.gameObject.SetActive(true); 


        if(haveEffect)
        {
            // fade out
        }

        isLoading = false; // load complete

        if(currentLoadScene.sceneType == SceneType.Location) // skip after scene load event if not map
            // call all after-load events
            afterSceneLoadEvent.RaiseEvent();

    }
}
