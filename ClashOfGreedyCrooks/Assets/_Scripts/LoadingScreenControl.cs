using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenControl : MonoBehaviour {
    public GameObject loadingScreen;

    AsyncOperation operation;

    public Slider slider;
    int loadSceen = 3;

    bool AddLoadingTimeisLoaded;

    private void Start()
    {
        Invoke("StartGame", 1);
    }

    void StartGame()
    {
        StartCoroutine(LoadAsynchronously(loadSceen));

        
    }

    //public void LoadLevel (int sceneIndex = 3)
    //{
    //    StartCoroutine(LoadAsynchronously(sceneIndex));
    //}

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        loadingScreen.SetActive(true);
        
        operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            
            Debug.Log(slider.value);
                slider.value = 50f;
  
            if (operation.progress == 0.9f)
            {
                slider.value = 100f;
                Invoke("LoadArena", 0.1f);
            }
            yield return null;
        }

    }

    void LoadArena()
    {
        operation.allowSceneActivation = true;
		//GameStateManager.GetInstance.SetState(GameState.Arena);
	}
    
   
}
//public GameObject loadingScreenObj;

    //AsyncOperation async;

    //private void Start()
    //{
    //    Invoke("LoadingScreenLoad", 1);
    //}

    //void LoadingScreenLoad()
    //{
    //    StartCoroutine(LoadingScreen());
    //}

    ////public void LoadingScreenExample()
    ////{
    ////    StartCoroutine(LoadingScreen());
    ////}

    //IEnumerator LoadingScreen()
    //{
    //    loadingScreenObj.SetActive(true);
    //    async = SceneManager.LoadSceneAsync(3);
    //    async.allowSceneActivation = false;

    //    while(async.isDone == false)
    //    {
    //        slider.value = async.progress;
    //        Debug.Log(async.progress);
    //        if(async.progress == 0.9f)
    //        {
    //            slider.value = 1f;
    //            async.allowSceneActivation = true;
    //        }
    //        yield return null;
    //    }
    //}
	

