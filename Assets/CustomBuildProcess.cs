using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class CustomBuildProcess : MonoBehaviour
{
	[Serializable]
	private struct VersionInfo
	{
		public string Version;
	}

	[Serializable]
	private struct BuildNumberInfo
	{
		public int BuildNumber;
	}

	[MenuItem("FlipCoin/BuildAndRun/Windows")]
	public static void BuildGame()
	{
		// Get filename.
		string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
		string[] levels = new string[] { "Assets/Scene1.unity", "Assets/Scene2.unity" };

		// Build player.
		BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.exe", BuildTarget.StandaloneWindows, BuildOptions.None);

		// Copy a file from the project folder to the build folder, alongside the built game.
		FileUtil.CopyFileOrDirectory("Assets/Templates/Readme.txt", path + "Readme.txt");

		// Run the game (Process class from System.Diagnostics).
		Process proc = new Process();
		proc.StartInfo.FileName = path + "/BuiltGame.exe";
		proc.Start();
	}

	[MenuItem("FlipCoin/BuildAndRun/AndroidDev")]
	public static void BuildAndroidDevelopmentMenu()
	{
		Build(BuildTarget.Android);
	}

	public static BuildResult Build(BuildTarget buildTarget, bool isDevelopmenBuild = true)
	{
		var productName = Environment.GetEnvironmentVariable("ProductName");
		PlayerSettings.productName = string.IsNullOrWhiteSpace(productName) ? "Vyzn" : productName;

		PlayerSettings.WSA.packageName = PlayerSettings.productName.Replace(" ", "");
		PlayerSettings.WSA.tileShortName = PlayerSettings.productName;

		var buildPlayerOptions = new BuildPlayerOptions
		{
			target = buildTarget,
			scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray(),
			locationPathName = "build"
		};

		PlayerSettings.SplashScreen.show = false;

		if (buildTarget == BuildTarget.Android)
		{
			buildPlayerOptions.locationPathName += ".apk";

			const string AndroidSdkRoot = "AndroidSdkRoot";
			const string AndroidNdkRoot = "AndroidNdkRoot";
			const string JdkRoot = "JdkPath";
			const string AndroidKeyStore = "AndroidKeyStore";
			const string AndroidKeyStorePassword = "AndroidKeyStorePassword";

			var androidSdkRoot = System.Environment.GetEnvironmentVariable(AndroidSdkRoot);
			var androidNdkRoot = System.Environment.GetEnvironmentVariable(AndroidNdkRoot);
			var jdkRoot = System.Environment.GetEnvironmentVariable(JdkRoot);

			EditorPrefs.SetString(AndroidSdkRoot, androidSdkRoot);
			EditorPrefs.SetString(AndroidNdkRoot, androidNdkRoot);
			EditorPrefs.SetString(JdkRoot, jdkRoot);

			EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle/*Internal*/;

			var keyStoreName = System.Environment.GetEnvironmentVariable(AndroidKeyStore);

			PlayerSettings.Android.keystoreName = Path.Combine(Environment.CurrentDirectory, keyStoreName);
			PlayerSettings.Android.keyaliasName = Path.GetFileNameWithoutExtension(keyStoreName);
			PlayerSettings.Android.keystorePass =
			PlayerSettings.Android.keyaliasPass = System.Environment.GetEnvironmentVariable(AndroidKeyStorePassword);
		}

		if (buildTarget == BuildTarget.WSAPlayer)
		{
			AttachIcons("Media/StoreContent");
		}

		var versionInfo = JsonUtility.FromJson<VersionInfo>(Resources.Load<TextAsset>("version-info").text);
		var buildNumberInfo = JsonUtility.FromJson<BuildNumberInfo>(Resources.Load<TextAsset>("buildnumber-info").text);
		var version = versionInfo.Version.Split('.');

		PlayerSettings.WSA.packageVersion = new Version(int.Parse(version[0]), int.Parse(version[1]), buildNumberInfo.BuildNumber, 0);
		PlayerSettings.Android.bundleVersionCode = buildNumberInfo.BuildNumber;
		PlayerSettings.iOS.buildNumber = buildNumberInfo.BuildNumber.ToString();
		PlayerSettings.bundleVersion = versionInfo.Version;

		if (buildTarget == BuildTarget.Android)
			PlayerSettings.bundleVersion += $".{buildNumberInfo.BuildNumber}";

		EditorUserBuildSettings.development = isDevelopmenBuild;
		EditorUserBuildSettings.allowDebugging = isDevelopmenBuild;
		EditorUserBuildSettings.androidBuildType = isDevelopmenBuild ? AndroidBuildType.Debug : AndroidBuildType.Release;
		EditorUserBuildSettings.iOSBuildConfigType = isDevelopmenBuild ? iOSBuildType.Debug : iOSBuildType.Release;

		if (isDevelopmenBuild)
			buildPlayerOptions.options |= BuildOptions.Development | BuildOptions.AllowDebugging;

		if (buildTarget == BuildTarget.WSAPlayer)
		{
			PlayerSettings.WSA.Declarations.protocolName = "zengalt." + PlayerSettings.productName.ToLower();
		}

		var result = BuildPipeline.BuildPlayer(buildPlayerOptions);


		return result.summary.result;
	}

	public static void AttachIcons(string storeContentPath)
	{

		////71x71 logos
		//SetIfExists(storeContentPath + "/Icons/71x71.png",
		//    PlayerSettings.WSAImageType.UWPSquare71x71Logo, PlayerSettings.WSAImageScale._100);
		//SetIfExists(storeContentPath + "/Icons/89x89.png",
		//    PlayerSettings.WSAImageType.UWPSquare71x71Logo, PlayerSettings.WSAImageScale._125);
		//SetIfExists(storeContentPath + "/Icons/107x107.png",
		//    PlayerSettings.WSAImageType.UWPSquare71x71Logo, PlayerSettings.WSAImageScale._150);
		//SetIfExists(storeContentPath + "/Icons/142x142.png",
		//    PlayerSettings.WSAImageType.UWPSquare71x71Logo, PlayerSettings.WSAImageScale._200);
		//SetIfExists(storeContentPath + "/Icons/284x284.png",
		//    PlayerSettings.WSAImageType.UWPSquare71x71Logo, PlayerSettings.WSAImageScale._400);

		////150x150
		//SetIfExists(storeContentPath + "/Icons/150x150.png",
		//    PlayerSettings.WSAImageType.UWPSquare150x150Logo, PlayerSettings.WSAImageScale._100);
		//SetIfExists(storeContentPath + "/Icons/300x300.png",
		//    PlayerSettings.WSAImageType.UWPSquare150x150Logo, PlayerSettings.WSAImageScale._200);
		//SetIfExists(storeContentPath + "/Icons/600x600.png",
		//    PlayerSettings.WSAImageType.UWPSquare150x150Logo, PlayerSettings.WSAImageScale._400);
		//SetIfExists(storeContentPath + "/Icons/188x188.png",
		//    PlayerSettings.WSAImageType.UWPSquare150x150Logo, PlayerSettings.WSAImageScale._125);
		//SetIfExists(storeContentPath + "/Icons/225x225.png",
		//    PlayerSettings.WSAImageType.UWPSquare150x150Logo, PlayerSettings.WSAImageScale._150);

		////310x310
		//SetIfExists(storeContentPath + "/Icons/310x310.png",
		//    PlayerSettings.WSAImageType.UWPSquare310x310Logo, PlayerSettings.WSAImageScale._100);
		//SetIfExists(storeContentPath + "/Icons/620x620.png",
		//    PlayerSettings.WSAImageType.UWPSquare310x310Logo, PlayerSettings.WSAImageScale._200);
		//SetIfExists(storeContentPath + "/Icons/1240x1240.png",
		//    PlayerSettings.WSAImageType.UWPSquare310x310Logo, PlayerSettings.WSAImageScale._400);
		//SetIfExists(storeContentPath + "/Icons/465x465.png",
		//    PlayerSettings.WSAImageType.UWPSquare310x310Logo, PlayerSettings.WSAImageScale._150);
		//SetIfExists(storeContentPath + "/Icons/388x388.png",
		//    PlayerSettings.WSAImageType.UWPSquare310x310Logo, PlayerSettings.WSAImageScale._125);

		////310x150
		//SetIfExists(storeContentPath + "/Icons/310x150.png",
		//    PlayerSettings.WSAImageType.UWPWide310x150Logo, PlayerSettings.WSAImageScale._100);
		//SetIfExists(storeContentPath + "/Icons/620x300.png",
		//    PlayerSettings.WSAImageType.UWPWide310x150Logo, PlayerSettings.WSAImageScale._200);
		//SetIfExists(storeContentPath + "/Icons/1240x600.png",
		//    PlayerSettings.WSAImageType.UWPWide310x150Logo, PlayerSettings.WSAImageScale._400);
		//SetIfExists(storeContentPath + "/Icons/388x188.png",
		//    PlayerSettings.WSAImageType.UWPWide310x150Logo, PlayerSettings.WSAImageScale._125);
		//SetIfExists(storeContentPath + "/Icons/465x225.png",
		//    PlayerSettings.WSAImageType.UWPWide310x150Logo, PlayerSettings.WSAImageScale._150);

		////Splash screen
		//SetIfExists(storeContentPath + "/Splashscreen/620x300.png",
		//    PlayerSettings.WSAImageType.SplashScreenImage, PlayerSettings.WSAImageScale._100);
		//SetIfExists(storeContentPath + "/Splashscreen/1240x600.png",
		//    PlayerSettings.WSAImageType.SplashScreenImage, PlayerSettings.WSAImageScale._200);
		//SetIfExists(storeContentPath + "/Splashscreen/2480x1200.png",
		//    PlayerSettings.WSAImageType.SplashScreenImage, PlayerSettings.WSAImageScale._400);
		//SetIfExists(storeContentPath + "/Splashscreen/775x375.png",
		//    PlayerSettings.WSAImageType.SplashScreenImage, PlayerSettings.WSAImageScale._125);
		//SetIfExists(storeContentPath + "/Splashscreen/930x450.png",
		//    PlayerSettings.WSAImageType.SplashScreenImage, PlayerSettings.WSAImageScale._150);

		//Store logo
		SetIfExists(storeContentPath + "/Icons/75x75.png",
			PlayerSettings.WSAImageType.PackageLogo, PlayerSettings.WSAImageScale._150);
		SetIfExists(storeContentPath + "/Icons/100x100.png",
			PlayerSettings.WSAImageType.PackageLogo, PlayerSettings.WSAImageScale._200);
		SetIfExists(storeContentPath + "/Icons/50x50.png",
			PlayerSettings.WSAImageType.PackageLogo, PlayerSettings.WSAImageScale._100);
		SetIfExists(storeContentPath + "/Icons/63x63.png",
			PlayerSettings.WSAImageType.PackageLogo, PlayerSettings.WSAImageScale._125);
		SetIfExists(storeContentPath + "/Icons/200x200.png",
			PlayerSettings.WSAImageType.PackageLogo, PlayerSettings.WSAImageScale._400);

		////Square 44x44
		//SetIfExists(storeContentPath + "/Icons/44x44.png",
		//    PlayerSettings.WSAImageType.UWPSquare44x44Logo, PlayerSettings.WSAImageScale._100);
		//SetIfExists(storeContentPath + "/Icons/88x88.png",
		//    PlayerSettings.WSAImageType.UWPSquare44x44Logo, PlayerSettings.WSAImageScale._200);
		//SetIfExists(storeContentPath + "/Icons/176x176.png",
		//    PlayerSettings.WSAImageType.UWPSquare44x44Logo, PlayerSettings.WSAImageScale._400);
		//SetIfExists(storeContentPath + "/Icons/55x55.png",
		//    PlayerSettings.WSAImageType.UWPSquare44x44Logo, PlayerSettings.WSAImageScale._125);
		//SetIfExists(storeContentPath + "/Icons/66x66.png",
		//    PlayerSettings.WSAImageType.UWPSquare44x44Logo, PlayerSettings.WSAImageScale._150);

		//SetIfExists(storeContentPath + "/Icons/16x16.png",
		//    PlayerSettings.WSAImageType.UWPSquare44x44Logo, PlayerSettings.WSAImageScale.Target16);
		//SetIfExists(storeContentPath + "/Icons/24x24.png",
		//    PlayerSettings.WSAImageType.UWPSquare44x44Logo, PlayerSettings.WSAImageScale.Target24);
		//SetIfExists(storeContentPath + "/Icons/48x48.png",
		//    PlayerSettings.WSAImageType.UWPSquare44x44Logo, PlayerSettings.WSAImageScale.Target48);
		//SetIfExists(storeContentPath + "/Icons/256x256.png",
		//    PlayerSettings.WSAImageType.UWPSquare44x44Logo, PlayerSettings.WSAImageScale.Target256);
	}

	private static void SetIfExists(string path, PlayerSettings.WSAImageType type, PlayerSettings.WSAImageScale scale)
	{
		var fullPath = Path.Combine(Application.dataPath, path);
		if (File.Exists(fullPath))
		{
			PlayerSettings.WSA.SetVisualAssetsImage(fullPath, type, scale);
		}
		else
		{
			UnityEngine.Debug.LogWarning("Can't find icon for path " + fullPath);
			PlayerSettings.WSA.SetVisualAssetsImage(null, type, scale);
		}
	}
}