using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoldColor.Config;

public class OtherBuildingBar : MonoBehaviour {

    public float BuildTime
    {
        set
        {
            _time = value;
            InvokeRepeating("StartBuilding", 0, _cycle);
        }
    }
    public float TotalHealth
    {
        set
        {
            _totalHealth = value;
        }
    }
    public GameObject Info;
    private float _time;
    private float _totalHealth;
    private float _totalValue;
    private float _currentValue;
    private float _cycle;
    private Slider Bar;
    public Image Fill;
    private MonoBehaviour OwnController;
    // Use this for initialization
    private void Awake()
    {
        Bar = gameObject.transform.Find("bar").GetComponent<Slider>();
        _totalValue = 100;
        _currentValue = 100;
        Bar.value = 1f;
        _cycle = 0.1f;
        Fill = gameObject.transform.Find("bar/Fill Area/Fill").GetComponent<Image>();
    }

    private void StartBuilding()
    {
        float ProcessByCycle = _totalValue / _time * _cycle;
        Processing(ProcessByCycle);
        if (_currentValue <= 0)
        {
            Info.GetComponent<StateBar>().CurrentHealth = _totalHealth;
            gameObject.GetComponentInParent<OtherObjectController>().gameObject.GetComponent<SpriteRenderer>().color = Fill.color;
            CancelInvoke();
        }
    }

    private void Processing(float ByCycle)
    {
        _currentValue -= ByCycle;
        Bar.value = ((float)_currentValue / (float)_totalValue);
    }
}
