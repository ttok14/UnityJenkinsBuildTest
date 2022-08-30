using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public static class JenkinsTest
{
    public static void DoBuild()
    {
        UnityEngine.Debug.Log("Start!");

        string shPath = $"{Directory.GetParent(Application.dataPath).FullName}/Test.sh";

        UnityEngine.Debug.Log("ShPath : " + shPath);

        ProcessStartInfo info = new ProcessStartInfo($"{shPath}");
        var process = System.Diagnostics.Process.Start(info);
        process.WaitForExit();

        UnityEngine.Debug.Log("End!");
    }
}
