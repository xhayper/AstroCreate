using System;
using System.Runtime.InteropServices;
using AstroCreate.BASS.Native;
using AstroDX.BASS.Native;
using Godot;
using ManagedBass;

public partial class InitBass : Node
{
    private IntPtr? _handle;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var fileExtension = "dll";

		switch (OS.GetName())
		{
			case "Windows":
				fileExtension = "dll";
				break;
			case "macOS":
				fileExtension = "dylib";
				break;
			case "Android":
			case "Linux":
				fileExtension = "so";
				break;
		}

		var filePath = $"{OS.GetName()}/{Engine.GetArchitectureName()}/libbass.{fileExtension}";

		DirAccess.RemoveAbsolute("user://libs/");
		DirAccess.MakeDirRecursiveAbsolute("user://libs/");
		DirAccess.CopyAbsolute($"res://dependencies/BASS/{filePath}", $"user://libs/libbass.{fileExtension}");
		
		var libPath = ProjectSettings.GlobalizePath($"user://libs/libbass.{fileExtension}");
		
		if (OS.GetName() == "Windows")
			_handle = Kernel32.LoadLibrary(libPath);
		else
			_handle = libdl.dlopen(libPath,
				(int)libdl.OPEN_FLAGS.RTLD_LAZY);

		if (_handle == IntPtr.Zero)
			throw new Exception($"Failed to load \"{libPath}\" (ErrorCode: {Marshal.GetLastWin32Error()})");
		
		if (!Bass.Init())
		{
			throw new Exception("BASS fails to initialize.");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
