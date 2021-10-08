using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OS_lab2.WinApi;

namespace ProjectedMemoryReader
{
	public interface IMemoryReader
	{
		void PrintReadFromMap();
	}

	public class MemoryReader : OS_lab2.MemoryWriter, IMemoryReader
	{
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern IntPtr OpenFileMapping(uint dwDesiredAccess, bool bInheritHandle, [MarshalAs(UnmanagedType.LPStr)] string lpName);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr MapViewOfFile(
			IntPtr hFileMappingObject,
			uint dwDesiredAccess,
			uint dwFileOffsetHigh,
			uint dwFileOffsetLow,
			UIntPtr dwNumberOfBytesToMap);

		protected readonly static IntPtr INVALID_HANDLE_VALUE = (IntPtr)(-1);

		public void PrintReadFromMap()
		{
			Console.WriteLine("Input map name:");
			string mapName = Console.ReadLine();
			var ptr = OpenFileMapping((uint)FileMapAccess.AllAccess, false, mapName);
			if (ptr != IntPtr.Zero && ptr != INVALID_HANDLE_VALUE)
			{
				var handle = MapViewOfFile(ptr, (uint)FileMapAccess.AllAccess, 0, 0, UIntPtr.Zero);
				if (ptr != IntPtr.Zero && ptr != INVALID_HANDLE_VALUE)
				{
					Console.WriteLine("Read data:");
					PrintMemoryChars((ulong)handle, 128);
				}
				else
					Console.WriteLine($"Error creating mapview. Error code {GetLastError()}");
			}
			else
				Console.WriteLine($"Error opening mapping. Error code {GetLastError()}");
		}
	}
}
