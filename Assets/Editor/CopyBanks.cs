using UnityEngine;
using UnityEditor;
using System.IO;

public class CopyFmodBanks : EditorWindow
{
    private static string configPath = "Assets/Editor/FMODBankPath.asset";
    private static string targetBankPath = "Assets/FMOD_Banks";

    [MenuItem("Tools/FMOD/Copy Bank Files")]
    public static void CopyBankFiles()
    {
        var config = AssetDatabase.LoadAssetAtPath<FMODBankPathConfig>(configPath);
        if (config == null)
        {
            Debug.LogError("FMODBankPath not found. Please create one in Assets/Editor.");
            return;
        }

        string sourceBankPath = config.sourceBankPath;
        if (!Directory.Exists(sourceBankPath))
        {
            Debug.LogError($"FMOD bank path does not exist: {sourceBankPath}");
            return;
        }

        if (!Directory.Exists(targetBankPath))
            Directory.CreateDirectory(targetBankPath);

        var bankFiles = Directory.GetFiles(sourceBankPath, "*.bank", SearchOption.TopDirectoryOnly);

        foreach (var file in bankFiles)
        {
            var fileName = Path.GetFileName(file);
            var dest = Path.Combine(targetBankPath, fileName);
            File.Copy(file, dest, true);
            Debug.Log($"Copied: {fileName}");
        }

        AssetDatabase.Refresh();
        Debug.Log("FMOD bank copy complete.");
    }
}
