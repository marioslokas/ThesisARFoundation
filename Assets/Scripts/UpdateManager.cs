using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WanzyeeStudio;

public class UpdateManager : BaseSingleton<UpdateManager>
{

    private List<IUpdatable> updatables = new List<IUpdatable>();


    public void SubscribeUpdatable(IUpdatable updatableEntity)
    {
        if (!updatables.Contains(updatableEntity))
        {
            updatables.Add(updatableEntity);
        }
    }

    public void UnsubscribeUpdatable(IUpdatable updatableEntity)
    {
        if (updatables.Contains(updatableEntity))
        {
            updatables.Remove(updatableEntity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < updatables.Count; i++)
        {
            updatables[i].MyUpdate();
        }
    }
}
