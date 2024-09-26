using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset m_actionAsset;

    void Awake()
    {
        m_actionAsset.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
