using System.Collections.Generic;
using System.IO;
using HybridCLR.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor.Tools
{
    [InitializeOnLoad]
    public class HybridCLRHelper
    {
        private static string[] _hotUpdateDllNames =
        {
            "HotUpdate.dll",
        };

        private static List<string> _patchMetaDataDllNames;

        private const string YooCodeHotUpdateDir = "Assets/YooDownloadRes/Code";
        
        static HybridCLRHelper()
        {
            _patchMetaDataDllNames = new(SettingsUtil.AOTAssemblyNames);
            for (var i = 0; i < _patchMetaDataDllNames.Count; i++)
            {
                var dllName = _patchMetaDataDllNames[i];
                if (dllName.EndsWith(".dll"))
                {
                    dllName = dllName.Substring(0, dllName.Length - 4);
                }
                _patchMetaDataDllNames[i] = dllName + ".dll";
            }
        }
        
        [MenuItem("HybridCLR/Custom/Quick copy dll")]
        public static void CopyHybridCLRGeneratedDllToHotUpdateFolder()
        {
            var genDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(EditorUserBuildSettings.activeBuildTarget);
            var metaDllDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(EditorUserBuildSettings.activeBuildTarget);
            if (!Directory.Exists(genDir))
            {
                Debug.LogError($"Directory {genDir} does not exist. Please compile Dll first.");
                return;
            }
            if (!Directory.Exists(metaDllDir))
            {
                Debug.LogError($"Directory {metaDllDir} does not exist. Please compile Dll first.");
                return;
            }

            CopyDllToDestWithBytesPostfix(_hotUpdateDllNames, genDir, YooCodeHotUpdateDir);
            CopyDllToDestWithBytesPostfix(_patchMetaDataDllNames, metaDllDir, YooCodeHotUpdateDir);
        }

        private static void CopyDllToDestWithBytesPostfix(IList<string> dllNames, string srcDir, string dstDir)
        {
            foreach (var dllName in dllNames)
            {
                string srcFile = Path.Combine(srcDir, Path.GetFileName(dllName));
                string destFile = Path.Combine(dstDir, Path.GetFileName(dllName) + ".bytes");
                File.Copy(srcFile, destFile, true);
                Debug.Log($"Copied {dllName} to {destFile}");
            }
            AssetDatabase.Refresh();
        }
    }
}