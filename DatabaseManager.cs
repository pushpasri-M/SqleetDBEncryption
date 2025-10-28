using System;
using System.Data;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace EmployeeMangementForm
{
    public static class DatabaseManager
    {
        private static IntPtr _db = IntPtr.Zero;
        private const string DbPath = "employee_encrypted.db";
        private const string DbKey = "MyStrongKey123";

        // Database page size for SQLeet
        private const int PageSize = 4096;

        private const int SQLITE_OPEN_READWRITE = 0x00000002;
        private const int SQLITE_OPEN_CREATE = 0x00000004;

        private static string PtrToStringUtf8(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) return string.Empty;
            int len = 0;
            while (Marshal.ReadByte(ptr, len) != 0) len++;
            byte[] buf = new byte[len];
            Marshal.Copy(ptr, buf, 0, len);
            return Encoding.UTF8.GetString(buf);
        }

        public static void InitializeDatabase()
        {
            if (_db != IntPtr.Zero) return; // Already open

            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DbPath);
            bool isNew = !File.Exists(fullPath);

            // 1️⃣ Open database
            int rc = SqleetWrapper.sqlite3_open_v2(fullPath, out _db,
                SQLITE_OPEN_READWRITE | SQLITE_OPEN_CREATE, IntPtr.Zero);
            if (rc != 0 || _db == IntPtr.Zero)
                throw new Exception("Failed to open DB. rc=" + rc);

            // 2️⃣ Apply encryption key immediately after open
            byte[] keyBytes = Encoding.UTF8.GetBytes(DbKey);
            rc = SqleetWrapper.sqlite3_key(_db, keyBytes, keyBytes.Length);
            if (rc != 0)
                throw new Exception("Failed to apply key. rc=" + rc);

            // 3️⃣ Encrypt new DB if needed
            if (isNew)
            {
                rc = SqleetWrapper.sqlite3_rekey(_db, keyBytes, keyBytes.Length);
                if (rc != 0)
                    throw new Exception("Failed to encrypt new DB. rc=" + rc);
            }

            // 4️⃣ Set page size AFTER key applied
            ExecuteNonQuery($"PRAGMA page_size = {PageSize};");

            // 5️⃣ Create table if missing
            string createTable = @"
                CREATE TABLE IF NOT EXISTS Employees (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Salary INTEGER NOT NULL,
                    Address TEXT
                );";
            ExecuteNonQuery(createTable);
        }

        public static void InsertEmployee(string name, int salary, string address)
        {
            EnsureOpen();
            string sql = "INSERT INTO Employees (Name, Salary, Address) VALUES (?, ?, ?);";
            ExecuteNonQuery(sql, name, salary, address);
        }

        public static DataTable GetEmployees()
        {
            EnsureOpen();
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Salary", typeof(int));
            dt.Columns.Add("Address", typeof(string));

            string sql = "SELECT ID, Name, Salary, Address FROM Employees;";
            IntPtr stmt;
            int rc = SqleetWrapper.sqlite3_prepare_v2(_db, sql, -1, out stmt, IntPtr.Zero);
            if (rc != 0 || stmt == IntPtr.Zero)
                throw new Exception("Failed to prepare SELECT. rc=" + rc);
             
            try
            {
                const int SQLITE_ROW = 100;
                const int SQLITE_DONE = 101;

                while ((rc = SqleetWrapper.sqlite3_step(stmt)) == SQLITE_ROW)
                {
                    int id = SqleetWrapper.sqlite3_column_int(stmt, 0);
                    string name = PtrToStringUtf8(SqleetWrapper.sqlite3_column_text(stmt, 1));
                    int salary = SqleetWrapper.sqlite3_column_int(stmt, 2);
                    string addr = PtrToStringUtf8(SqleetWrapper.sqlite3_column_text(stmt, 3));

                    dt.Rows.Add(id, name, salary, addr);
                }

                if (rc != SQLITE_DONE)
                    throw new Exception("Error reading rows. rc=" + rc);
            }
            finally
            {
                SqleetWrapper.sqlite3_finalize(stmt);
            }

            return dt;
        }

        private static void ExecuteNonQuery(string sql, params object[] parameters)
        {
            EnsureOpen();
            IntPtr stmt;
            int rc = SqleetWrapper.sqlite3_prepare_v2(_db, sql, -1, out stmt, IntPtr.Zero);
            if (rc != 0 || stmt == IntPtr.Zero)
                throw new Exception("Prepare failed. rc=" + rc);

            try
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    int index = i + 1;
                    object param = parameters[i];

                    if (param is int iv)
                        rc = SqleetWrapper.sqlite3_bind_int(stmt, index, iv);
                    else if (param is string s)
                        rc = SqleetWrapper.sqlite3_bind_text(stmt, index, s, -1, new IntPtr(-1));
                    else
                        throw new Exception("Unsupported param type at index " + index);

                    if (rc != 0)
                        throw new Exception("Bind failed at index " + index + ". rc=" + rc);
                }

                rc = SqleetWrapper.sqlite3_step(stmt);
                const int SQLITE_DONE = 101;
                if (rc != SQLITE_DONE)
                    throw new Exception("ExecuteNonQuery failed. rc=" + rc);
            }
            finally
            {
                SqleetWrapper.sqlite3_finalize(stmt);
            }
        }

        private static void EnsureOpen()
        {
            if (_db == IntPtr.Zero)
                InitializeDatabase();
        }

        public static void CloseDatabase()
        {
            if (_db != IntPtr.Zero)
            {
                SqleetWrapper.sqlite3_close(_db);
                _db = IntPtr.Zero;
            }
        }
    }
}
