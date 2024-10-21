using UnityEngine;
using UnityEngine.Events;

/*
    传递去哪个场景，坐标，是否有场景切换特效
*/

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> loadRequestEvent;
    /// <summary>
    /// scene load request
    /// </summary>    

    public void RaiseLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool haveEffect)
    {
        loadRequestEvent?.Invoke(locationToLoad, posToGo, haveEffect);
    }
}
