using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdatable
{
    void Register();
    void Unregister();
    void MyUpdate();
}
