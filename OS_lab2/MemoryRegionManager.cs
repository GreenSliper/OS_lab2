using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OS_lab2.WinApi;

namespace OS_lab2
{
	public interface IMemoryRegionManager
	{
		void DefineSegmentState();
		void ReserveVirtualAuto();
		void ReserveVirtualManual();
		void ReservePhysicalAuto();
		void ReservePhysicalManual();
		void ProtectRegion(MemoryProtection memoryProtectionLevel);
		void FreeRegion();
	}
	public class MemoryRegionManager : MemoryInfoManager, IMemoryRegionManager
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		protected static extern IntPtr VirtualQuery(ulong lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, IntPtr dwLength);

		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		protected static extern IntPtr VirtualAlloc(ulong lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
		
		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		protected static extern bool VirtualFree(ulong lpAddress, uint dwSize, uint dwFreeType);

		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		protected static extern bool VirtualProtect(ulong lpAddress, uint dwSize, uint newProtect, [Out] out uint oldProtect);

		private static bool TryParseHex(string hex, out ulong result)
		{
			result = 0;
			if (hex == null)
				return false;
			try
			{
				if (hex.StartsWith("0x"))
					hex = hex[2..];
				result = Convert.ToUInt64(hex, 16);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		protected ulong ConsoleReadHex()
		{
			Console.WriteLine("Input hex address:");
			ulong addr;
			while (!TryParseHex(Console.ReadLine(), out addr))
				Console.WriteLine("Incorrect hex number! Try again");
			return addr;
		}

		[STAThread]
		public unsafe void DefineSegmentState()
		{
			ulong addr = ConsoleReadHex();
			MEMORY_BASIC_INFORMATION buf = new MEMORY_BASIC_INFORMATION();
			if ((uint)VirtualQuery(addr, out buf, (IntPtr)Marshal.SizeOf(buf)) != 0)
			{
				Console.WriteLine($"Allocation base: {buf.allocationBase}");
				Console.WriteLine($"Base address: {buf.baseAddress}");
				Console.WriteLine($"Region size: {buf.regionSize}");
				Console.WriteLine($"State: {(MEM_STATE)buf.state}");
				Console.WriteLine($"Allocation protect: {(MEM_ALLOCATION_PROTECT)buf.allocationProtect}");
				Console.WriteLine($"Type: {(MEM_TYPE)buf.lType}");
			}
			else
				Console.WriteLine($"Error occured. Error code: {GetLastError()}");
		}

		public void FreeRegion()
		{
			ulong addr = ConsoleReadHex();
			if (VirtualFree(addr, 0, (uint)MEM_FREE_TYPE.MEM_RELEASE))
				Console.WriteLine("Memory freed successfully!");
			else
			{
				uint err = GetLastError();
				Console.WriteLine("Error occured." + (err!=0?$" Error code : {err}":""));
			}
		}

		public void ProtectRegion(MemoryProtection memoryProtectionLevel)
		{
			ulong addr = ConsoleReadHex();
			uint oldProtect = 0;
			if (VirtualProtect(addr, 4, (uint)memoryProtectionLevel, out oldProtect))
				Console.WriteLine($"Memory protection level changed successfully! Old protection level: {(MemoryProtection)oldProtect}");
			else
			{
				uint err = GetLastError();
				Console.WriteLine("Error occured." + (err != 0 ? $" Error code : {err}" : ""));
			}
		}

		bool AllocRegion(out IntPtr basicAddr, bool automatic = true, bool physical = false)
		{
			GetSystemInfo(out SYSTEM_INFO_WCE50 info);
			ulong addr = 0;
			if (!automatic)
				addr = ConsoleReadHex();
			uint memst = (uint)MEM_STATE.MEM_RESERVE;
			if (physical)
				memst |= (uint)MEM_STATE.MEM_COMMIT;

			basicAddr = VirtualAlloc(addr, info.dwPageSize,
				memst, (uint)MEM_ALLOCATION_PROTECT.PAGE_EXECUTE_READWRITE);
			uint err;
			if ((err = GetLastError()) != 0)
			{
				Console.WriteLine($"Error occured. Error code: {err}");
				return false;
			}
			return true;
		}

		public void ReservePhysicalAuto()
		{
			if (AllocRegion(out IntPtr basicAddr, physical: true))
				Console.WriteLine($"Automatic physical allocation successful! Base address: 0x{basicAddr.ToInt64():X}");
		}

		public void ReservePhysicalManual()
		{
			if (AllocRegion(out IntPtr basicAddr, automatic: false, physical: true))
				Console.WriteLine($"Manual physical allocation successful! Base address: 0x{basicAddr.ToInt64():X}");
		}

		public void ReserveVirtualAuto()
		{
			if (AllocRegion(out IntPtr basicAddr))
				Console.WriteLine($"Automatic virtual allocation successful! Base address: 0x{basicAddr.ToInt64():X}");
		}

		public void ReserveVirtualManual()
		{
			if (AllocRegion(out IntPtr basicAddr, automatic: false))
				Console.WriteLine($"Manual virtual allocation successful! Base address: 0x{basicAddr.ToInt64():X}");
		}
	}
}
