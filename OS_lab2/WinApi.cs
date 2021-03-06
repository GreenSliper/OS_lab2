using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OS_lab2
{
    namespace WinApi
    {
        public enum ProcessorArchitecture : int
        {
            PROCESSOR_ARCHITECTURE_AMD64 = 9,
            PROCESSOR_ARCHITECTURE_ARM = 5,
            PROCESSOR_ARCHITECTURE_ARM64 = 12,
            PROCESSOR_ARCHITECTURE_IA64 = 6,
            PROCESSOR_ARCHITECTURE_INTEL = 0,
            PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFFFF
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO_WCE50
        {
            public ushort wProcessorArchitecture;
            public byte wReserved;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort wProcessorLevel;
            public ushort wProcessorRevision;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
            public MEMORYSTATUSEX()
            {
                dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr baseAddress;
            public IntPtr allocationBase;
            public uint allocationProtect;
            public IntPtr regionSize;
            public uint state;
            public uint protect;
            public uint lType;
        }

        public enum MEM_STATE : uint
        {
            MEM_COMMIT = 0x1000,
            MEM_FREE = 0x10000,
            MEM_RESERVE = 0x2000,
        }

        public enum MEM_ALLOCATION_PROTECT : uint
        {
            NO_ACCESS = 0,
            PAGE_EXECUTE = 0x00000010,
            PAGE_EXECUTE_READ = 0x00000020,
            PAGE_EXECUTE_READWRITE = 0x00000040,
            PAGE_EXECUTE_WRITECOPY = 0x00000080,
            PAGE_NOACCESS = 0x00000001,
            PAGE_READONLY = 0x00000002,
            PAGE_READWRITE = 0x00000004,
            PAGE_WRITECOPY = 0x00000008,
            PAGE_GUARD = 0x00000100,
            PAGE_NOCACHE = 0x00000200,
            PAGE_WRITECOMBINE = 0x00000400
        }

        public enum MEM_TYPE : uint
        {
            MEM_IMAGE = 0x1000000,
            MEM_MAPPED = 0x40000,
            MEM_PRIVATE = 0x20000
        }

        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400,
            TargetsInvalid = 0x40000000
        }

        public enum MEM_FREE_TYPE : uint
        {
            MEM_DECOMMIT = 0x00004000,
            MEM_RELEASE = 0x00008000,
            /// <summary>
            /// USE WITH MEM_RELEASE ONLY!
            /// To coalesce two adjacent placeholders, specify
            /// MEM_RELEASE | MEM_COALESCE_PLACEHOLDERS. When you coalesce placeholders,
            /// lpAddress and dwSize must exactly match those of the placeholder.
            /// </summary>
            MEM_COALESCE_PLACEHOLDERS = 0x00000001,
            /// <summary>
            /// USE WITH MEM_RELEASE ONLY!
            /// frees an allocation back to a placeholder (after you've replaced a
            /// placeholder with a private allocation using VirtualAlloc2
            /// </summary>
            MEM_PRESERVE_PLACEHOLDER = 0x00000002
        }

        [Flags]
        public enum FileMapProtection : uint
        {
            PageReadonly = 0x02,
            PageReadWrite = 0x04,
            PageWriteCopy = 0x08,
            PageExecuteRead = 0x20,
            PageExecuteReadWrite = 0x40,
            SectionCommit = 0x8000000,
            SectionImage = 0x1000000,
            SectionNoCache = 0x10000000,
            SectionReserve = 0x4000000,
        }
        public enum FileMapAccess : uint
        {
            Copy = 1,
            Write = 2,
            Read = 4,
            AllAccess = 983071,
            Execute = 32,
        }

        [StructLayout(LayoutKind.Sequential)]
        public class SECURITY_ATTRIBUTES
        {
            public uint length;
            public IntPtr securityDescriptor;
            public bool inheritHandle;
        }

        public enum DesiredAccess : uint
        {
            GENERIC_READ = 0x80000000,
            GENERIC_WRITE = 0x40000000,
            GENERIC_EXECUTE = 0x20000000,
            GENERIC_ALL = 0x10000000
        }

        [Flags]
        public enum ShareMode : uint
        {
            None = 0,
            FILE_SHARE_READ = 1,
            FILE_SHARE_WRITE = 2,
            FILE_SHARE_DELETE = 4
        }

        public enum CreationDisposition : uint
        {
            CREATE_NEW = 1,
            CREATE_ALWAYS = 2,
            OPEN_EXISTING = 3,
            OPEN_ALWAYS = 4,
            TRUNCATE_EXISTING = 5
        }

        public enum FileAttributes : uint
        {
            FILE_ATTRIBUTE_READONLY = 0x1,
            FILE_ATTRIBUTE_HIDDEN = 0x2,
            FILE_ATTRIBUTE_SYSTEM = 0x4,
            FILE_ATTRIBUTE_ARCHIVE = 0x20,
            FILE_ATTRIBUTE_NORMAL = 0x80,
            FILE_ATTRIBUTE_TEMPORARY = 0x100,
            FILE_ATTRIBUTE_OFFLINE = 0x1000,
            FILE_ATTRIBUTE_ENCRYPTED = 0x4000
        }
    }
}
