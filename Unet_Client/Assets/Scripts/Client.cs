using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    private const int MAX_USER = 100;
    private const int PORT = 26000;
    private const int WEB_PORT = 26001;
    private const string SERVER_IP = "127.0.0.1";

    private byte reliableChannel;
    private int hostId;
    private byte error;
    
    //   Dont need WebHostId in Client Side
    //   private int webHostId;
    private bool isStarted = false;

    // Start is called before the first frame update

    #region Monobehaviour
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }
    #endregion

    public void Init()
    {
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology topo = new HostTopology(cc, MAX_USER);

        // Client Only Code
        hostId = NetworkTransport.AddHost(topo, 0);

#if UNITY_WEBGL && !UNITY_EDITOR
        // Web Client
        NetworkTransport.Connect(hostId, SERVER_IP, WEB_PORT, 0, out error);
        Debug.Log("Connecting from Web");
#else
        // Standalone Client
        NetworkTransport.Connect(hostId, SERVER_IP, PORT, 0, out error);
        Debug.Log(string.Format("Connecting from standalone"));
#endif
        // webHostId = NetworkTransport.AddWebsocketHost(topo, WEB_PORT, null);

        Debug.Log(string.Format("Openning connection on port {0} ...", SERVER_IP));
        isStarted = true;
    }

    public void Shutdown()
    {
        isStarted = false;
        NetworkTransport.Shutdown();
    }
}

