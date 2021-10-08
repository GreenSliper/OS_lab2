using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using OS_lab2.WinApi;

namespace OS_lab2
{
	public interface IMemoryInfoManager 
	{
		void PrintSystemInfo();
		void PrintGlobalMemoryStatus();
	}
	public class MemoryInfoManager : IMemoryInfoManager
	{
		[DllImport("kernel32")]
		protected static extern uint GetLastError();

		[DllImport("kernel32", SetLastError = true)]
		protected static extern void GetSystemInfo(out SYSTEM_INFO_WCE50 lpSystemInfo);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		protected static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);
		public void PrintSystemInfo()
		{
			GetSystemInfo(out SYSTEM_INFO_WCE50 sysInfo);
			uint err;
			if ((err = GetLastError()) != 0)
			{
				Console.WriteLine($"Error occured. Error code: {err}");
				return;
			}
			Console.WriteLine($"Processor architecture: {(ProcessorArchitecture)sysInfo.wProcessorArchitecture}");
			Console.WriteLine($"Processor count: {sysInfo.dwNumberOfProcessors}");
			Console.WriteLine($"Active processor mask: {Convert.ToString(sysInfo.dwActiveProcessorMask.ToInt32(), 2)}");
			Console.WriteLine($"Processor level: { sysInfo.wProcessorLevel }");
			Console.WriteLine($"Processor revision: { sysInfo.wProcessorRevision:X}");

			Console.WriteLine($"Memory page size: {sysInfo.dwPageSize}");
			Console.WriteLine($"Minimum accessible memory address: {sysInfo.lpMinimumApplicationAddress:X}");
			Console.WriteLine($"Maximum accessible memory address: {sysInfo.lpMaximumApplicationAddress:X}");
			Console.WriteLine($"Allocation granularity: { sysInfo.dwAllocationGranularity }");

		}

		public void PrintGlobalMemoryStatus()
		{
			MEMORYSTATUSEX ms = new MEMORYSTATUSEX();
			if (!GlobalMemoryStatusEx(ms))
			{
				uint err;
				if ((err = GetLastError()) != 0)
				{
					Console.WriteLine($"Error occured. Error code: {err}");
					return;
				}
				return;
			}
			
			Console.WriteLine($"Memory Load: {ms.dwMemoryLoad} %");
			Console.WriteLine($"Total Physical: {ms.ullTotalPhys / 1024} Kb");
			Console.WriteLine($"Available Physical: {ms.ullAvailPhys / 1024} Kb");
			Console.WriteLine($"Total Page File: {ms.ullTotalPageFile / 1024} Kb");
			Console.WriteLine($"Available Page File: {ms.ullAvailPageFile / 1024} Kb");
			Console.WriteLine($"Total Virtual: {ms.ullTotalVirtual / 1024} Kb");
			Console.WriteLine($"Available Virtual: {ms.ullAvailVirtual / 1024} Kb");
		}
	}
}
