using System;
using OS_lab2;

namespace ProjectedMemoryWriter
{
	class MemoryWriterConsole
	{
		static IMemoryWriter memoryWriter = new MemoryWriter();

		static IMenu mainMenu = new Menu("MemoryWriter", 
			new IMenuItem[] { 
				new MenuItem("Init projected file", ()=>
				{ 
					if(!memoryWriter.TryInit())
						Console.WriteLine("Initialization failed. Please, retry.");
				}),
				new MenuItem("Write message", memoryWriter.WriteMessage),
				new MenuItem("Close handles", memoryWriter.Close)
			});
		static void Main(string[] args)
		{
			mainMenu.Select();
		}
	}
}
