using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MicScript {

	//[DllImport("PluginName")]
	[DllImport("WindowsMicrophoneMuteLibrary")]
	private static extern void MuteMic();

}
