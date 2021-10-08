using System;
using System.Runtime.InteropServices;
using OS_lab2.WinApi;

namespace ProjectedMemoryWriter
{
    public interface IMemoryWriter
    {
        bool TryInit();
        void WriteMessage();
        void Close();
    }
    public class MemoryWriter : OS_lab2.MemoryWriter, IMemoryWriter
	{
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        protected static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPUTF8Str)] string fileName,
                                      uint desiredAccess, uint shareMode, SECURITY_ATTRIBUTES securityAttributes,
                                      uint creationDisposition, uint flagsAndAttributes, IntPtr templateFile);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFileMapping(
            IntPtr hFile,
            SECURITY_ATTRIBUTES lpFileMappingAttributes,
            FileMapProtection flProtect,
            uint dwMaximumSizeHigh,
            uint dwMaximumSizeLow,
            [MarshalAs(UnmanagedType.LPStr)] string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr MapViewOfFile(
            IntPtr hFileMappingObject,
            uint dwDesiredAccess,
            uint dwFileOffsetHigh,
            uint dwFileOffsetLow,
            UIntPtr dwNumberOfBytesToMap);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        protected static extern bool CloseHandle(IntPtr hObject);

        protected readonly static IntPtr INVALID_HANDLE_VALUE = (IntPtr)(-1);
        private IntPtr file, lpFileMap = INVALID_HANDLE_VALUE;
        readonly UIntPtr bufferSize = new UIntPtr(128);

        public bool TryInit()
		{
			Console.WriteLine("Please, input filename:");
            string targetPath = Console.ReadLine();
            file = CreateFile(targetPath, (uint)DesiredAccess.GENERIC_WRITE | (uint)DesiredAccess.GENERIC_READ,
                        0,
                        null, (uint)CreationDisposition.CREATE_ALWAYS, 0,
                        IntPtr.Zero);
            if (file != INVALID_HANDLE_VALUE)
            {
                Console.WriteLine("Input map name:");
                string mapName = Console.ReadLine();
                var fileMap = CreateFileMapping(file, null, FileMapProtection.PageReadWrite, 0, bufferSize.ToUInt32(), mapName);
                lpFileMap = MapViewOfFile(fileMap, (uint)FileMapAccess.AllAccess, 0, 0, bufferSize);
                if (lpFileMap != INVALID_HANDLE_VALUE)
                {
                    Console.WriteLine("File projected successfully!");
                    return true;
                }
                else
                {
                    Console.WriteLine($"File projection error. Error code {GetLastError()}");
                    return false;
                }
            }
            else
				Console.WriteLine($"File creation error. Error code {GetLastError()}");
            return false;
        }
       
        public void WriteMessage()
        {
            if (lpFileMap == INVALID_HANDLE_VALUE)
            {
                Console.WriteLine("Projected file uninitialized or corrupted!");
                return;
            }
			Console.WriteLine("Input message:");
            string message = Console.ReadLine();
            if(TryCopyMemoryString(Convert.ToUInt64(lpFileMap.ToInt64()), message))
				Console.WriteLine("Message passed to the projected file successfully!\n Do NOT close this window to access data from reader!");
            else
                Console.WriteLine($"Error occured when passing message to the projected file. Error code: {GetLastError()}");
        }

		public void Close()
		{
            UnmapViewOfFile(lpFileMap);
            CloseHandle(file);
		}
	}
}
