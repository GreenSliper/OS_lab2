using System;
using System.Runtime.InteropServices;
using OS_lab2.WinApi;

namespace OS_lab2
{
	public interface IMemoryWriter
	{
		void WriteMemoryCells();
	}

	public class MemoryWriter : MemoryRegionManager, IMemoryWriter
	{
		//CopyMemory is actually an alias for RtlCopyMemory
		[DllImport("kernel32.dll", EntryPoint = "RtlCopyMemory", SetLastError = false)]
		public static extern void CopyMemory(ulong dest, IntPtr src, uint count);

		/// <summary>
		/// Print some RAM bytes interpreted as chars
		/// </summary>
		/// <param name="addr">memory start address</param>
		/// <param name="length">character count</param>
		protected static void PrintMemoryChars(ulong addr, int length)
		{
			for (ulong i = 0; i < (ulong)length; i++)
			{
				IntPtr ptr = (IntPtr)(addr + i * sizeof(char));
				Console.Write((char)Marshal.ReadByte(ptr));
			}
			Console.WriteLine();
		}

		protected unsafe bool TryCopyMemoryString(ulong addr, string s)
		{
			char[] chars = s.ToCharArray();
			fixed (char* c = chars)
			{
				CopyMemory(addr, new IntPtr(c), (uint)s.Length * sizeof(char));
				uint err = GetLastError();
				if (err == 0)
				{
					Console.WriteLine("Memory written successfully!");
					return true;
				}
				else
				{
					Console.WriteLine($"Error writing memory. Error code: {err}");
					return false;
				}
			}
		}

		public unsafe void WriteMemoryCells()
		{
			ulong addr = ConsoleReadHex();
			Console.WriteLine("Input data to write:");
			string data = Console.ReadLine();
			uint length = (uint)data.Length * sizeof(char);
			MEMORY_BASIC_INFORMATION info = new MEMORY_BASIC_INFORMATION();
			if ((uint)VirtualQuery(addr, out info, (IntPtr)Marshal.SizeOf(info)) != 0)
			{
				uint accessibleFlags = (uint)(MEM_ALLOCATION_PROTECT.PAGE_READWRITE |
											  MEM_ALLOCATION_PROTECT.PAGE_WRITECOPY |
											  MEM_ALLOCATION_PROTECT.PAGE_EXECUTE_READWRITE |
											  MEM_ALLOCATION_PROTECT.PAGE_EXECUTE_WRITECOPY);
				//if memory is writable
				if ((info.allocationProtect & accessibleFlags) != 0)
				{
					TryCopyMemoryString(addr, data);
					PrintMemoryChars(addr, data.Length);
				}
				else
					Console.WriteLine("Memory protection level does not allow writing bytes!");
			}
			else
				Console.WriteLine($"Error occured. Error code: {GetLastError()}");
		}
	}
}
