using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public List<Checkpointtt> checkPoints;
    private void Awake()
    {
        checkPoints = new List<Checkpointtt>(GetComponentsInChildren<Checkpointtt>());
    }
}