using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoldColor.Config;

public class ReserveBuildTipController : MonoBehaviour {
    private WebSocketController WS;
    private float Radius;
    private bool isBuildAbled;
    public bool IsBuildAbled
    {
        get
        {
            return isBuildAbled;
        }
        set
        {
            isBuildAbled = value;
        }
    }
    private bool FinalBuildAbled;
    public GameObject ReserveUIBTN;
    // Use this for initialization
    void Start()
    {
        Radius = ReserveConfig._InteractAreaRadius;
        gameObject.GetComponentInChildren<BuildAreaTip>().Radius = Radius;
        gameObject.GetComponentInChildren<BuildAreaTip>().Type = BuildAreaTip.BuildType.Reserve;
        gameObject.GetComponent<SpriteRenderer>().color = UI._BuildingDiasbled;
        FinalBuildAbled = true;
        isBuildAbled = false;
        ReserveUIBTN = GameObject.Find("UI/BuildReserve");
        WS = GameObject.Find("WebSocketController").GetComponent<WebSocketController>();
    }
    private void Update()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0;
        gameObject.transform.position = target;

        if (FinalBuildAbled && isBuildAbled)
        {
            gameObject.GetComponent<SpriteRenderer>().color = UI._BuildingAbled;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = UI._BuildingDiasbled;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (FinalBuildAbled && isBuildAbled)
            {
                //GameObject Turret = Resources.Load<GameObject>("Prefabs/Reserve");
                //Instantiate(Turret, transform.position, transform.rotation);
                WS.Send(JsonUtility.ToJson(new MessageBox.MessageBase
                {
                    Type = "BuildMessage",
                    Message = JsonUtility.ToJson(new MessageBox.BuildMessage
                    {
                        Type = "Reserve",
                        id = null,
                        Position = JsonUtility.ToJson(new MessageBox.Position
                        {
                            x = transform.position.x,
                            y = transform.position.y
                        })
                    })
                }));
                ReserveUIBTN.GetComponent<Button>().interactable = true;
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("cant");
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("cancle");
            ReserveUIBTN.GetComponent<Button>().interactable = true;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ColliderController colliderController = collision.gameObject.GetComponent<ColliderController>();
        if (colliderController.Type == ColliderController.ColliderType.BodyCollider)
        {
            FinalBuildAbled = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ColliderController colliderController = collision.gameObject.GetComponent<ColliderController>();
        if (colliderController.Type == ColliderController.ColliderType.BodyCollider)
        {
            FinalBuildAbled = true;
        }
    }
}
