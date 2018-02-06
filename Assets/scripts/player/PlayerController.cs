﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoldColor.Config;

public class PlayerController : MonoBehaviour {
    public GameObject GameBody;
    public GameObject Path;
    public GameObject Interact;
    public GameObject Info;
    private Color _camp;
    public Color Camp {
        get {
            return _camp;
        }
        set
        {
            _camp = value;
        }
    }
    private void Awake()
    {
        _camp = CampDefine.Orange;
        GameBody.GetComponent<SpriteRenderer>().color = _camp;
    }
}
