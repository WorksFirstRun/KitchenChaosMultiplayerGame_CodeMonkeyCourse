using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{

     private static Scene targetScene;
     public static void LoadSceneMode(Scene s)
     {
          targetScene = s;
          SceneManager.LoadScene(Scene.LoadingScene.ToString());
     }

     public static void LoaderCallBack()
     {
          SceneManager.LoadScene(targetScene.ToString());
     }

     public static void Network_LoadSceneMode(Scene s)
     {
          NetworkManager.Singleton.SceneManager.LoadScene(s.ToString(),UnityEngine.SceneManagement.LoadSceneMode.Single);
     }
     
     
     public enum Scene
     {
          MainMenu,
          LoadingScene,
          GameScene,
          LobbyScene,
          CharacterSelectScene
     }
     
}
