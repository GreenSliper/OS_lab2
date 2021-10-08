using System;
using OS_lab2;

namespace ProjectedMemoryReader
{
	class MemoryReaderConsole
	{
		static IMemoryReader memoryReader = new MemoryReader();
		static IMenu mainMenu = new Menu("MemoryReader",
			new IMenuItem[] {
				new MenuItem("Read message", memoryReader.PrintReadFromMap)
			});
		static void Main(string[] args)
		{
			mainMenu.Select();
		}
	}
}
