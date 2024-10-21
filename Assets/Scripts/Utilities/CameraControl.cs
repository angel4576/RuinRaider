using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;

    [Header("Event Listener")]
    public VoidEventSO cameraShakeEvent; 
    public VoidEventSO afterSceneLoadEvent;

    private void Awake() 
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }
    
    // register
    private void OnEnable() 
    {
        cameraShakeEvent.OnEventRaised += OnCameraShake;
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoad;
    }

    private void OnDisable() 
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShake;
        afterSceneLoadEvent.OnEventRaised -= OnAfterSceneLoad;
    }

    // Start is called before the first frame update
    void Start()
    {
        // GetNewCameraBound();
    }

    private void OnAfterSceneLoad()
    {
        // get camera bound after scene loaded
        GetNewCameraBound();
    }

    private void OnCameraShake()
    {
        impulseSource.GenerateImpulse();
    }

    private void GetNewCameraBound()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if(obj == null)
            return;

        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();

        confiner2D.InvalidateCache(); // 获得新摄像机边界之后 清除之前边界的形状缓存
    }
}
