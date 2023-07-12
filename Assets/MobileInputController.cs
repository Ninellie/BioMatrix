using UnityEngine;

public class MobileInputController : MonoBehaviour
{
    //This is the Text for the Label at the top of the screen
    [SerializeField] private string _mDeviceType;

    private void Awake()
    {
        Debug.Log("Device type : " + _mDeviceType);

        //Check if the device running this is a console
        if (SystemInfo.deviceType == DeviceType.Console)
        {
            //Change the text of the label
            _mDeviceType = "Console";
            //gameObject.SetActive(false);
        }

        //Check if the device running this is a desktop
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            _mDeviceType = "Desktop";
            //gameObject.SetActive(false);
        }

        //Check if the device running this is a handheld
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            _mDeviceType = "Handheld";
            //gameObject.SetActive(true);
        }

        //Check if the device running this is unknown
        if (SystemInfo.deviceType == DeviceType.Unknown)
        {
            _mDeviceType = "Unknown";
            //gameObject.SetActive(false);
        }

        if (Application.isMobilePlatform)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
