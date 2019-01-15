using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WanzyeeStudio;

public class UpdateManager : BaseSingleton<UpdateManager>
{
    [SerializeField]
    private bool pauseUpdating = false;

    private List<IUpdatable> updatables = new List<IUpdatable>();
    private List<Rigidbody> updatableRigidbodies = new List<Rigidbody>();
    private List<ManualGravity> updatableGravityComponents = new List<ManualGravity>();
    
    // can also be arrays
    private List<Vector3> updatableVelocities = new List<Vector3>();
    private List<Vector3> updatableAngularVelocities = new List<Vector3>();

    public void SubscribeUpdatable(IUpdatable updatableEntity)
    {
        if (!updatables.Contains(updatableEntity))
        {
            updatables.Add(updatableEntity);
            if (updatableEntity.myRigidBody)
            {
                updatableRigidbodies.Add(updatableEntity.myRigidBody);
            }

            if (updatableEntity.myGravity)
            {
                updatableGravityComponents.Add(updatableEntity.myGravity);
            }
        }
    }

    public void UnsubscribeUpdatable(IUpdatable updatableEntity)
    {
        if (updatables.Contains(updatableEntity))
        {
            updatables.Remove(updatableEntity);
            if (updatableEntity.myRigidBody && updatableRigidbodies.Contains(updatableEntity.myRigidBody))
            {
                updatableRigidbodies.Remove(updatableEntity.myRigidBody);
            }
            
            if (updatableEntity.myGravity)
            {
                updatableGravityComponents.Remove(updatableEntity.myGravity);
            }
        }
    }

    // stop updating objects and save physics velocity
    public void PauseUpdates()
    {
        pauseUpdating = true;
        
        // pause rigid bodies
        for (int i = 0; i < updatableRigidbodies.Count; i++)
        {
            updatableVelocities.Add(updatableRigidbodies[i].velocity);
            updatableAngularVelocities.Add(updatableRigidbodies[i].angularVelocity);
            updatableRigidbodies[i].isKinematic = true;
        }

        // pause manual gravities
        for (int i = 0; i < updatableGravityComponents.Count; i++)
        {
            updatableGravityComponents[i].gravityPaused = true;
        }
    }

    public void ResumeUpdates()
    {
        pauseUpdating = false;
        
        // re apply saved velocities to bodies
        for (int i = 0; i < updatableRigidbodies.Count; i++)
        {
            updatableRigidbodies[i].isKinematic = false;
            updatableRigidbodies[i].velocity = updatableVelocities[i];
            updatableRigidbodies[i].angularVelocity = updatableAngularVelocities[i];
        }
        
        // restart gravity
        for (int i = 0; i < updatableGravityComponents.Count; i++)
        {
            updatableGravityComponents[i].gravityPaused = false;
        }
        
        updatableVelocities.Clear();
        updatableAngularVelocities.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseUpdating) return;
        
        for (int i = 0; i < updatables.Count; i++)
        {
            updatables[i].MyUpdate();
        }
    }
}
