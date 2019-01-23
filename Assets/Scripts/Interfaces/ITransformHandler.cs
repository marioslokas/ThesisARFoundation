using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITransformHandler
{
   void Initialize(Vector3[] centralGamePositions, 
      Transform[] objectTransforms,
      GameObject[] lineRendererGameObjects,
      LineRenderer[] forceLineRenderers,
      Rigidbody[] projectileRigidbodies,
      bool messageOnFire);
}
