using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public float MaxTimeToReachNextCheckpoint = 30f;
    public float TimeLeft = 30f;
    public KartAgent kartAgent;
    public Checkpointtt nextCheckPointToReach;
    private int CurrentCheckpointIndex;
    private List<Checkpointtt> Checkpoints;
    private Checkpointtt lastCheckpoint;

    public event Action<Checkpointtt> reachedCheckpoint;

    void Start()
    {
        Checkpoints = FindAnyObjectByType<Checkpoints>().checkPoints;
        ResetCheckpoints();
    }

    public void ResetCheckpoints()
    {
        CurrentCheckpointIndex = 0;
        TimeLeft = MaxTimeToReachNextCheckpoint;
        SetNextCheckpoint();
    }

    private void Update()
    {
        TimeLeft -= Time.deltaTime;

        if (TimeLeft < 0f)
        {
            kartAgent.AddReward(-1f);
            kartAgent.EndEpisode();
        }
    }

    public void CheckPointReached(Checkpointtt checkpoint)
    {
        if (nextCheckPointToReach != checkpoint) return;
        lastCheckpoint = Checkpoints[CurrentCheckpointIndex];
        reachedCheckpoint?.Invoke(checkpoint);
        CurrentCheckpointIndex++;

        if (CurrentCheckpointIndex >= Checkpoints.Count)
        {
            kartAgent.AddReward(0.5f);
            kartAgent.EndEpisode();
        }
        else
        {
            kartAgent.AddReward((0.5f) / Checkpoints.Count);
            SetNextCheckpoint();
        }
    }

    private void SetNextCheckpoint()
    {
        if (Checkpoints.Count > 0)
        {
            TimeLeft = MaxTimeToReachNextCheckpoint;
            nextCheckPointToReach = Checkpoints[CurrentCheckpointIndex];
        }
    }
}