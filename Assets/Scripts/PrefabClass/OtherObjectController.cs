using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoldColor.Config;

public class OtherObjectController : MonoBehaviour {
    public enum Type
    {
        Player,
        Hinge,
        Field,
        Reserve,
        Turret
    }
    public Type type;
    public GameObject Info;
    public Color Camp;
    public string id;
    private Collector Collector;
    private void Awake () {
        Collector = GameObject.Find("Collector").GetComponent<Collector>();
    }

    public void Init(Color camp)
    {
        gameObject.GetComponentInChildren<StateBar>().id = id;
        Camp = camp;
        gameObject.GetComponent<SpriteRenderer>().color = Camp;
        gameObject.GetComponent<ColliderController>().Camp = Camp;
        StateBar statebar = Info.GetComponent<StateBar>();
        switch (type)
        {
            case Type.Player:
                float _health = PlayerConfig._TotalHealth;
                float _energy = PlayerConfig._TotalEnergy;
                statebar.TotalHealth = _health;
                statebar.TotalEnergy = _energy;
                statebar.CurrentEnergy = _energy;
                statebar.CurrentHealth = _health;
                break;
            case Type.Hinge:
                _health = HingeConfig._TotalHealth;
                _energy = HingeConfig._TotalEnergy;
                statebar.TotalHealth = _health;
                statebar.TotalEnergy = _energy;
                statebar.CurrentEnergy = _energy;
                statebar.CurrentHealth = _health;
                break;
            case Type.Field:
                _health = Field._TotalHealth;
                _energy = Field._TotalEnergy;
                statebar.TotalHealth = _health;
                statebar.TotalEnergy = _energy;
                statebar.CurrentEnergy = 0;
                statebar.CurrentHealth = 0;
                gameObject.GetComponent<SpriteRenderer>().color = CampDefine.Campless;
                gameObject.GetComponentInChildren<OtherBuildingBar>().Fill.color = Camp;
                gameObject.GetComponentInChildren<OtherBuildingBar>().TotalHealth = _health;
                gameObject.GetComponentInChildren<OtherBuildingBar>().BuildTime = Field._BuildingTime;
                break;
            case Type.Reserve:
                _health = ReserveConfig._TotalHealth;
                _energy = ReserveConfig._TotalEnergy;
                statebar.TotalHealth = _health;
                statebar.TotalEnergy = _energy;
                statebar.CurrentEnergy = 0;
                statebar.CurrentHealth = 0;
                gameObject.GetComponent<SpriteRenderer>().color = CampDefine.Campless;
                gameObject.GetComponentInChildren<OtherBuildingBar>().Fill.color = Camp;
                gameObject.GetComponentInChildren<OtherBuildingBar>().TotalHealth = _health;
                gameObject.GetComponentInChildren<OtherBuildingBar>().BuildTime = ReserveConfig._BuildingTime;
                break;
            case Type.Turret:
                _health = TurretConfig._TotalHealth;
                _energy = TurretConfig._TotalEnergy;
                statebar.TotalHealth = _health;
                statebar.TotalEnergy = _energy;
                statebar.CurrentEnergy = 0;
                statebar.CurrentHealth = 0;
                gameObject.GetComponent<SpriteRenderer>().color = CampDefine.Campless;
                gameObject.GetComponentInChildren<OtherBuildingBar>().Fill.color = Camp;
                gameObject.GetComponentInChildren<OtherBuildingBar>().TotalHealth = _health;
                gameObject.GetComponentInChildren<OtherBuildingBar>().BuildTime = TurretConfig._BuildingTime;
                break;
        }
        Collector.KeyValuePair Pair = new Collector.KeyValuePair
        {
            key = id,
            value = gameObject
        };
        Collector.Others.Add(Pair);
    }
}
