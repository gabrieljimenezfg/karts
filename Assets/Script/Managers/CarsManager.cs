using System;
using System.Collections.Generic;
using UnityEngine;

public class CarsManager : MonoBehaviour
{
    private enum State
    {
        WaitingToStart,
        GamePlaying,
    }

    [SerializeField] private State state;

    public static CarsManager Instance;
    [SerializeField] public List<CarInfo> cars;

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public void StartGame()
    {
        state = State.GamePlaying;
    }
}