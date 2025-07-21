using UnityEngine;
using TMPro;
using System.Net;
using UnityEngine.UI;

public class IPDisplay : MonoBehaviour
{
    public TextMeshProUGUI ipText;
    public Button copyButton;

    void Start()
    {
        ipText.text = "Gebe diese IP-Adresse bei der anderen Instanz ein: " + GetLocalIPAddress();

        if (copyButton != null)
        {
            copyButton.onClick.AddListener(CopyIPToClipboard);
        }
    }

    private string GetLocalIPAddress()
    {
        string localIP = "N/A";
        try
        {
            string hostName = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = address.ToString();
                    break;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Fehler beim Abrufen der IP-Adresse: " + e.Message);
        }
        return localIP;
    }

    private void CopyIPToClipboard()
    {
        if (ipText != null && !string.IsNullOrEmpty(ipText.text))
        {
            string ipAddress = ipText.text.Replace("Gebe diese IP-Adresse bei der anderen Instanz ein: ", "").Trim();
            GUIUtility.systemCopyBuffer = ipAddress;
            Debug.Log("IP-Adresse kopiert: " + ipAddress);
        }
    }
}