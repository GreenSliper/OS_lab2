using System;

namespace OS_lab2
{
	class Program
	{

		static Action foo = () => Console.WriteLine("foo");

		static IMenu mainMenu = new Menu("LAB 2: Memory Management", 
			new IMenuItem[] { 
				new Menu("Get memory info", new IMenuItem[]{ 
					new MenuItem("System info", foo),
					new MenuItem("Global memory status", foo)
				}),
				new Menu("Memory region management", new IMenuItem[]{ 
					new MenuItem("Define memory segment state", foo),
					new Menu("Reserve region", new IMenuItem[]{ 
						new MenuItem("Virtual (auto)", foo),
						new MenuItem("Virtual (define start point)", foo),
						new MenuItem("Physical (auto)", foo),
						new MenuItem("Physical (define start point)", foo)
					}),
					new MenuItem("Protect region (w-check)", foo),
					new MenuItem("Free region", foo)
				}),
				new MenuItem("Write memory cells", foo)
			});

		static void Main(string[] args)
		{
			mainMenu.Select();
		}
	}
}
