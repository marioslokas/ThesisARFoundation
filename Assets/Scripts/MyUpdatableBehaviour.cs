﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyUpdatableBehaviour : MonoBehaviour, IUpdatable
{

    void Awake()
    {
        myRigidBody = this.GetComponent<Rigidbody>();
        myGravity = this.GetComponent<ManualGravity>();
    }
    
    private void OnDisable()
    {
        Unregister();
    }

    private void OnEnable()
    {
        Register();
    }

    public void Register()
    {
        UpdateManager.instance.SubscribeUpdatable(this as IUpdatable);
        Debug.Log("Object " + this.gameObject.GetInstanceID() + " registered");
    }

    public void Unregister()
    {
        UpdateManager.instance.UnsubscribeUpdatable(this as IUpdatable);
        Debug.Log("Object " + this.gameObject.GetInstanceID() + " removed");
    }

    public abstract void MyUpdate();
    public Rigidbody myRigidBody { get; private set; }
    public ManualGravity myGravity { get; private set; }
}
