using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO sceneToGo;
    public Vector3 posToGo;
    public bool needKey;

    public void TriggerAction()
    {
        if(!needKey)
        {
            loadEventSO.RaiseLoadRequestEvent(sceneToGo, posToGo, true);
        } else {
            bool hasKey = InventoryManager.Instance.UseItem("Boss Key");
            if(hasKey){
                loadEventSO.RaiseLoadRequestEvent(sceneToGo, posToGo, true); // caller 
            } else {
                Debug.Log("Need Boss Key!");
            }
        }
    }
    
}
