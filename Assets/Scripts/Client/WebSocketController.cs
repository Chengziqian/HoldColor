using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class WebSocketController : MonoBehaviour {
    [DllImport("__Internal")]
    private static extern void SocketConnect(string url);
    [DllImport("__Internal")]
    private static extern void SocketSendMessage(string msg);
    [DllImport("__Internal")]
    private static extern void SocketClose();

    private MessageBox.MessageBase MessageBase;
    private MessageBox.InitializeMessage InitializeMessage;
    private MessageBox.OtherInitializeMessage OtherInitializeMessage;
    private MessageBox.PlayerPosition PlayerPosition;
    private MessageBox.BulletMessage BulletMessage;
    private MessageBox.ChangeStateBar ChangeStateBar;
    private MessageBox.BuildMessage BuildMessage;
    private Collector Collector;
    private Initialize Initialize;

    private GameObject Bullet;
    // Use this for initialization
   void Awake () {
        MessageBase = new MessageBox.MessageBase();
        InitializeMessage = new MessageBox.InitializeMessage();
        OtherInitializeMessage = new MessageBox.OtherInitializeMessage();
        PlayerPosition = new MessageBox.PlayerPosition();
        BulletMessage = new MessageBox.BulletMessage();
        ChangeStateBar = new MessageBox.ChangeStateBar();
        BuildMessage = new MessageBox.BuildMessage();
        Collector = GameObject.Find("Collector").GetComponent<Collector>();
        Initialize = GameObject.Find("InitializeController").GetComponent<Initialize>();
        Debug.Log("start connect");
        SocketConnect("ws://192.168.0.106:2222");
        Debug.Log("connect done");

        Bullet = Resources.Load<GameObject>("Prefabs/OtherBullet");
    }

    public void Send(string msg)
    {
        SocketSendMessage(msg);
    }

    void Open()
    {
        Debug.Log("connected!");
    }

    void Close()
    {
        Debug.Log("closed!");
    }

    void OnMessage(string data)
    {
        JsonUtility.FromJsonOverwrite(data, MessageBase);
        switch (MessageBase.Type)
        {
            case "InitializeMessage":
                JsonUtility.FromJsonOverwrite(MessageBase.Message, InitializeMessage);
                Initialize.SendMessage("initialize", InitializeMessage);
                break;
            case "PlayerPosition":
                JsonUtility.FromJsonOverwrite(MessageBase.Message, PlayerPosition);
                GameObject Player = new GameObject();
                foreach (Collector.KeyValuePair p in Collector.Others)
                {
                    if (p.key == PlayerPosition.id)
                    {
                        Player = p.value;
                    }
                }
                Player.transform.position = new Vector3(PlayerPosition.x, PlayerPosition.y, 0);
                break;
            case "OtherInitializeMessage":
                JsonUtility.FromJsonOverwrite(MessageBase.Message, OtherInitializeMessage);
                Initialize.SendMessage("initializeOthers", OtherInitializeMessage);
                break;
            case "BulletMessage":
                JsonUtility.FromJsonOverwrite(MessageBase.Message, BulletMessage);
                MessageBox.Position SP = new MessageBox.Position();
                JsonUtility.FromJsonOverwrite(BulletMessage.StartPosition, SP);
                Debug.Log("SP: x:" + SP.x + "SP:y:" + SP.y);
                GameObject bullet = Instantiate(Bullet, new Vector3(SP.x, SP.y, 0), new Quaternion());
                foreach (Collector.KeyValuePair p in Collector.Others)
                {
                    if (p.key == BulletMessage.TargetID)
                    {
                        bullet.GetComponent<OtherBulletController>().target = p.value;
                    }
                }
                bullet.GetComponent<OtherBulletController>().Camp = Initialize.GetOtherCamp(BulletMessage.Camp);
                bullet.GetComponent<OtherBulletController>().isShoot = true;
                break;
            case "ChangeStateBar":
                Debug.Log(data);
                JsonUtility.FromJsonOverwrite(MessageBase.Message, ChangeStateBar);
                GameObject Object = new GameObject();
                foreach (Collector.KeyValuePair p in Collector.Others)
                {
                    if (p.key == ChangeStateBar.id)
                    {
                        Object = p.value;
                        Debug.Log("get Object");
                    }
                }
                Object.GetComponentInChildren<StateBar>().ChangeInfoByMessage(ChangeStateBar);
                break;
            case "BuildMessage":
                Debug.Log("BuildMessage: " + data);
                JsonUtility.FromJsonOverwrite(MessageBase.Message, BuildMessage);
                MessageBox.Position P = JsonUtility.FromJson<MessageBox.Position>(BuildMessage.Position);
                switch (BuildMessage.Type)
                {
                    case "Field":
                        GameObject Field = Resources.Load<GameObject>("Prefabs/Field");
                        GameObject field = Instantiate(Field, new Vector3 (P.x, P.y, 0), new Quaternion());
                        field.GetComponent<FieldController>().id = BuildMessage.id;
                        field.GetComponentInChildren<StateBar>().id = BuildMessage.id;
                        Collector.Others.Add(new Collector.KeyValuePair
                        {
                            key = BuildMessage.id,
                            value = field
                        });
                        break;
                    case "Reserve":
                        GameObject Reserve = Resources.Load<GameObject>("Prefabs/Reserve");
                        GameObject reserve = Instantiate(Reserve, new Vector3(P.x, P.y, 0), new Quaternion());
                        reserve.GetComponent<FieldController>().id = BuildMessage.id;
                        reserve.GetComponentInChildren<StateBar>().id = BuildMessage.id;
                        Collector.Others.Add(new Collector.KeyValuePair
                        {
                            key = BuildMessage.id,
                            value = reserve
                        });
                        break;
                    case "Turret":
                        GameObject Turret = Resources.Load<GameObject>("Prefabs/Turret");
                        GameObject turret = Instantiate(Turret, new Vector3(P.x, P.y, 0), new Quaternion());
                        turret.GetComponent<FieldController>().id = BuildMessage.id;
                        turret.GetComponentInChildren<StateBar>().id = BuildMessage.id;
                        Collector.Others.Add(new Collector.KeyValuePair
                        {
                            key = BuildMessage.id,
                            value = turret
                        });
                        break;
                }
                break;
            case "OtherBuildMessage":
                Debug.Log("OtherBuildMessage: " + data);
                JsonUtility.FromJsonOverwrite(MessageBase.Message, BuildMessage);
                MessageBox.Position PP = JsonUtility.FromJson<MessageBox.Position>(BuildMessage.Position);
                switch (BuildMessage.Type)
                {
                    case "Field":
                        GameObject Field = Resources.Load<GameObject>("Prefabs/OtherField");
                        GameObject field = Instantiate(Field, new Vector3(PP.x, PP.y, 0), new Quaternion());
                        field.GetComponent<OtherObjectController>().id = BuildMessage.id;
                        field.GetComponent<OtherObjectController>().SendMessage("Init", Initialize.GetOtherCamp(BuildMessage.Camp));
                        Collector.Others.Add(new Collector.KeyValuePair
                        {
                            key = BuildMessage.id,
                            value = field
                        });
                        break;
                    case "Reserve":
                        GameObject Reserve = Resources.Load<GameObject>("Prefabs/OtherReserve");
                        GameObject reserve = Instantiate(Reserve, new Vector3(PP.x, PP.y, 0), new Quaternion());
                        reserve.GetComponent<OtherObjectController>().id = BuildMessage.id;
                        reserve.GetComponent<OtherObjectController>().SendMessage("Init", Initialize.GetOtherCamp(BuildMessage.Camp));
                        Collector.Others.Add(new Collector.KeyValuePair
                        {
                            key = BuildMessage.id,
                            value = reserve
                        });
                        break;
                    case "Turret":
                        GameObject Turret = Resources.Load<GameObject>("Prefabs/OtherTurret");
                        GameObject turret = Instantiate(Turret, new Vector3(PP.x, PP.y, 0), new Quaternion());
                        turret.GetComponent<OtherObjectController>().id = BuildMessage.id;
                        turret.GetComponent<OtherObjectController>().SendMessage("Init", Initialize.GetOtherCamp(BuildMessage.Camp));
                        Collector.Others.Add(new Collector.KeyValuePair
                        {
                            key = BuildMessage.id,
                            value = turret
                        });
                        break;
                }
                break;
        }
    }
}
