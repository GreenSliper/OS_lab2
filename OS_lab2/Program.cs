using System;

namespace OS_lab2
{
	class Program
	{
		static IMemoryInfoManager memoryInfoManager = new MemoryInfoManager();
		static IMemoryRegionManager regionManager = new MemoryRegionManager();
		static IMemoryWriter memoryWriter = new MemoryWriter();

		static Action foo = () => Console.WriteLine("foo");

		static IMenu mainMenu = new Menu("LAB 2: Memory Management", 
			new IMenuItem[] { 
				new Menu("Get memory info", new IMenuItem[]{ 
					new MenuItem("System info", memoryInfoManager.PrintSystemInfo),
					new MenuItem("Global memory status",  memoryInfoManager.PrintGlobalMemoryStatus)
				}),

				new Menu("Memory region management", new IMenuItem[]{ 
					new MenuItem("Define memory segment state", regionManager.DefineSegmentState),
				
					new Menu("Reserve region", new IMenuItem[]{ 
						new MenuItem("Virtual (auto)", regionManager.ReserveVirtualAuto),
						new MenuItem("Virtual (define start point)", regionManager.ReserveVirtualManual),
						new MenuItem("Physical (auto)", regionManager.ReservePhysicalAuto),
						new MenuItem("Physical (define start point)", regionManager.ReservePhysicalManual)
					}),
					
					Menu.CreateFromEnum<WinApi.MemoryProtection>("Protect region (w-check)", regionManager.ProtectRegion),
					new MenuItem("Free region", regionManager.FreeRegion)
				}),

				new MenuItem("Write memory cells", memoryWriter.WriteMemoryCells)
			});

		static void Main(string[] args)
		{
			mainMenu.Select();
		}
	}
}
