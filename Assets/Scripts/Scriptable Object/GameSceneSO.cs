using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets; // 需要使用addressable的方式加载

[CreateAssetMenu(menuName = "Game Scene/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    public SceneType sceneType; // indicate type of the scene
    public AssetReference sceneReference;
}
