using System;
using System.Runtime.InteropServices;

namespace EmployeeMangementForm
{
    public static class SqleetWrapper
    {
        private const string DLL = "sqleet.dll";

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqlite3_open_v2")]
        public static extern int sqlite3_open_v2([MarshalAs(UnmanagedType.LPUTF8Str)] string filename, out IntPtr db, int flags, IntPtr zvfs);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqlite3_close")]
        public static extern int sqlite3_close(IntPtr db);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqlite3_key")]
        public static extern int sqlite3_key(IntPtr db, byte[] key, int keyLength);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqlite3_rekey")]
        public static extern int sqlite3_rekey(IntPtr db, byte[] key, int keyLength);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqlite3_prepare_v2")]
        public static extern int sqlite3_prepare_v2(IntPtr db, [MarshalAs(UnmanagedType.LPUTF8Str)] string sql, int nByte, out IntPtr stmt, IntPtr pzTail);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqlite3_step")]
        public static extern int sqlite3_step(IntPtr stmt);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqlite3_finalize")]
        public static extern int sqlite3_finalize(IntPtr stmt);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqlite3_column_text")]
        public static extern IntPtr sqlite3_column_text(IntPtr stmt, int col);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqlite3_column_int")]
        public static extern int sqlite3_column_int(IntPtr stmt, int col);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqlite3_bind_text")]
        public static extern int sqlite3_bind_text(IntPtr stmt, int index, [MarshalAs(UnmanagedType.LPUTF8Str)] string value, int n, IntPtr destructor);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqlite3_bind_int")]
        public static extern int sqlite3_bind_int(IntPtr stmt, int index, int val);

        [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqlite3_errmsg")]
        public static extern IntPtr sqlite3_errmsg(IntPtr db);
    }
}
