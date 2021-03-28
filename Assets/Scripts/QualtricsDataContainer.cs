using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Container for data relevant to the qualtrics survey
/// Also contains methods to send and alter this data
/// Author: Trenton Plager
/// </summary>
public class QualtricsDataContainer : MonoBehaviour
{
    #region Fields
    private Dictionary<string, bool> accessibilityFeatures = new Dictionary<string, bool>();
    private Dictionary<string, float> weaponUseTimes = new Dictionary<string, float>();
    #endregion

    #region Methods
    void Start()
    {
        // Add new features here and also on the qualtrics survey under Survey Flow 
        accessibilityFeatures.Add("AimAssist", false);
        //accessibilityFeatures.Add("AccessibilityFeatureY", false);

        // Add new weapons here and also on the qualtrics survey under Survey Flow 
        // Time will be stored in minutes, but added in seconds
        weaponUseTimes.Add("BallLauncher", 0);
        weaponUseTimes.Add("DiscPistol", 0);
    }

    /// <summary>
    /// Set whether an individual Accessibility Feature is being used or not
    /// </summary>
    /// <param name="featureName">The name of the feature</param>
    /// <param name="offOrOn">Whether the feature is off or on</param>
    public void SetAccessibilityFeature(string featureName, bool offOrOn)
    {
        if (accessibilityFeatures.ContainsKey(featureName))
        {
            accessibilityFeatures[featureName] = offOrOn;
        }
        else
        {
            Debug.Log("Accessibiliy Feature name not found. Add it in QualtricsDataContainer.Start()");
        }
    }

    /// <summary>
    /// Adds a value to the current use time for a weapon
    /// </summary>
    /// <param name="weaponName">The name of the weapon</param>
    /// <param name="secondsToAdd">The time to add to the weapon's use time value in seconds</param>
    public void AddWeaponUseTime(string weaponName, float secondsToAdd)
    {
        if (weaponUseTimes.ContainsKey(weaponName))
        {
            float minutesToAdd = secondsToAdd / 60;
            weaponUseTimes[weaponName] += minutesToAdd;
        }
        else
        {
            Debug.Log("Weapon name not found. Add it in QualtricsDataContainer.Start()"); 
        }
    }

    /// <summary>
    /// Send relevant data to the qualtrics survey
    /// Intended to be used only at the end of a test
    /// </summary>
    public void SendData()
    {
        string queryString = "https://rit.az1.qualtrics.com/jfe/form/SV_diIHkjs847UtUPk/?";

        foreach (KeyValuePair<string, bool> feature in accessibilityFeatures)
        {
            queryString += $"Using{feature.Key}={feature.Value}&";
        }

        foreach(KeyValuePair<string, float> weapon in weaponUseTimes)
        {
            queryString += $"{weapon.Key}UseTime={Math.Round(weapon.Value, 2)}&";
        }

        queryString = queryString.Remove(queryString.Length - 1, 1);

        UnityWebRequest www = UnityWebRequest.Get(queryString);

        www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            Debug.Log("Data Sent Successfully");
        }
    }

    /// <summary>
    /// Test method for reference
    /// </summary>
    public void Test()
    {
        WWWForm form = new WWWForm();
        UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get("https://rit.az1.qualtrics.com/jfe/form/SV_diIHkjs847UtUPk/?test=blah");
        //UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get("https://rit.az1.qualtrics.com/jfe/form/SV_afPgmwJySIesQxU/?action1=blah&action2=blah2");

        www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            Debug.Log("successful!");
        }
    }
    #endregion
}
