using UnityEngine;
using UnityEditor;
using System.Collections;

public class RunGitShell 
{	
	private const string shell = "PowerShell";
	private const string args = "-ExecutionPolicy RemoteSigned";
	
	[MenuItem("Utilities/Version Control/Open Git Shell %&g")]
	public static void OpenShell()
	{
		System.Diagnostics.Process.Start(shell, args);
	}	
}
