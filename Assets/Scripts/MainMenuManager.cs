using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    [SerializeField]
    Button m_ButtonStart, m_ButtonExit, m_ButtonConnect;
    [SerializeField]
    InputField m_InputFieldAddress;
    [SerializeField]
    NetworkManager m_NetworkManager;
    string IpAddress;
	// Use this for initialization
	void Start () {
       // m_NetworkManager = GetComponent<NetworkManager>();
        m_InputFieldAddress.onEndEdit.AddListener(onEndEdit);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void m_ButtonStartClicked()
    {
        m_NetworkManager.StartHost();
    }
    public void m_ButtonExitClicked()
    {
        Application.Quit();
    }
    public void m_ButtonConnectClicked()
    {
        m_InputFieldAddress.gameObject.SetActive(true);
        if(m_InputFieldAddress.gameObject.activeSelf)
        {
            Connect(IpAddress);
        }
    }

    private void Connect(string ipAddress)
    {
        var address = ipAddress.Split(':');
        m_NetworkManager.networkAddress = address[0];
        m_NetworkManager.networkPort = int.Parse(address[1]);
        m_NetworkManager.StartClient();
    }

    public void onEndEdit(string ipAddress)
    {
        IpAddress = ipAddress;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Connect(ipAddress);
        }
    }
}
