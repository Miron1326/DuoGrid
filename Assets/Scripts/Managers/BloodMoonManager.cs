using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BloodMoonManager : MonoBehaviour
{
    [SerializeField] private List<CellType> cellsToChange;
    private const int BLOOD_CELL_FROM_BLOOD_MOON = 3;
    [SerializeField] private int _move;
    [SerializeField] private GameObject sunGameObject;
    [SerializeField] private Light2D LightSun;
    [SerializeField] private Light2D LightGlobal;
    [SerializeField] private int awakes = 0;
    [SerializeField] private List<Transform> sunPositions = new List<Transform>();
    private BloodMagesticManager _bloodManager;
    [SerializeField] private bool Day;
    private Color sunColor;
    public static BloodMoonManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        _bloodManager = GetComponent<BloodMagesticManager>();
        _bloodManager.OnBloodMageVisit += TryToChangeRandomCellsToBlood;
        GameManager.Instance.OnSwitchTurn += MoonDayChanging;
        LightGlobal.color = Color.white;
        Day = true;
    }
    void MoonDayChanging()
    {

        _move++;
        sunColor = LightSun.color;
        if (_move >= sunPositions.Count)
        {
            _move = 0;
            sunGameObject.transform.DOMove(sunPositions[_move].position, 1);
        }
        else
        {
            sunGameObject.transform.DOMove(sunPositions[_move].position, 1);
        }
        
        awakes++;
        if(awakes % 8 == 0)
        {
            Day = !Day;
        }
        if (Day)
        {
            GameManager.Instance.currentTimeDay = TimeOfDay.Day;
            if (sunColor != Color.white)
            for(float i = 0; i < 1;i += 0.1f)
            {
               LightGlobal.color = Color.Lerp(Color.gray, Color.white, i);
            }

        }
        else
        {
            GameManager.Instance.currentTimeDay = TimeOfDay.Night;
            if (sunColor != Color.gray6)
                for (float i = 0; i < 1; i += 0.1f)
                {
                    LightGlobal.color = Color.Lerp(Color.white, Color.gray, i);
                }
        }
    }



    private void TryToChangeRandomCellsToBlood()
    {
        if (!Day)
        {
            GameManager.Instance.currentTimeDay = TimeOfDay.BloodNight;
            cellsToChange = GameManager.Instance.GetRandomCellNotWall(BLOOD_CELL_FROM_BLOOD_MOON);
            foreach (CellType cellType in cellsToChange)
            {
                cellType.ChangeType(EffectType.BloodCapsule);
            }
        }
    }

}
