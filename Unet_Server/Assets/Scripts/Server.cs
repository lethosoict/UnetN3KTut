using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private const int MAX_USER = 100;
    private const int PORT = 26000;
    private const int WEB_PORT = 26001;

    private byte reliableChannel;
    private int hostId;
    private int webHostId;
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

        hostId = NetworkTransport.AddHost(topo, PORT, null);
        webHostId = NetworkTransport.AddWebsocketHost(topo, WEB_PORT, null);

        Debug.Log(string.Format("Openning connection on port {0} and web_port {1}", PORT, WEB_PORT));
        isStarted = true;
    }
}
