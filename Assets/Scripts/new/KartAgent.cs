using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class KartAgent : Agent
{
    public CheckpointManager _checkpointManager;
    private KartController _kartController;
    
    public override void Initialize()
    {
        _kartController = GetComponent<KartController>();
    }
    //Called each time it has timed-out or has reached the goal
    public override void OnEpisodeBegin()
    {
        _checkpointManager.ResetCheckpoints();
        _kartController.Respawn();
    }

    #region 

    //Collecting extra Information that isn't picked up by the RaycastSensors
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 diff = _checkpointManager.nextCheckPointToReach.transform.position - transform.position;
        sensor.AddObservation(diff / 20f);

        AddReward(-0.001f);
    }

    //Processing the actions received
    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.ContinuousActions;
        _kartController.ApplyAcceleration(action[1]);
        _kartController.Steer(action[0]);
        _kartController.AnimateKart(action[0]);
    }
    //For manual testing with human input, the actionsOut defined here will be sent to OnActionRecieved
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var countinousactions = actionsOut.ContinuousActions;

        countinousactions[0] = Input.GetAxis("Vertical");//steering
        countinousactions[1] = Input.GetKey(KeyCode.W) ? 1f : 0f;//acceleration
    }
    #endregion
}