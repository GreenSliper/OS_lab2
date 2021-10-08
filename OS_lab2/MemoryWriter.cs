using System;
using System.Runtime.InteropServices;
using static OS_lab2.WinApi;

namespace OS_lab2
{
	public interface IMemoryWriter
	{
		void WriteMemoryCells();
	}

	public class MemoryWriter : MemoryRegionManager, IMemoryWriter
	{
		[DllImport("kernel32.dll", EntryPoint = "RtlCopyMemory", SetLastError = false)]
		public static extern void CopyMemory(ulong dest, IntPtr src, uint count);

		protected void PrintMemoryChars(ulong addr, int length)
		{
			for (ulong i = 0; i < (ulong)length; i++)
			{
				IntPtr ptr = (IntPtr)(addr + i * sizeof(char));
				Console.Write((char)Marshal.ReadByte(ptr));
			}
			Console.WriteLine();
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

				if ((info.allocationProtect & accessibleFlags) != 0)
				{
					fixed (char *d = data.ToCharArray())
					{
						CopyMemory(addr, new IntPtr(d), length);
						uint err = GetLastError();
						if (err == 0)
							Console.WriteLine("Memory written successfully!");
						else
						{
							Console.WriteLine($"Error writing memory. Error code: {err}");
							return;
						}
						Console.WriteLine("Memory written:");
						PrintMemoryChars(addr, data.Length);
					}
				}
				else
					Console.WriteLine("Memory protection level does not allow writing bytes!");
			}
			else
				Console.WriteLine($"Error occured. Error code: {GetLastError()}");
		}
	}
}
