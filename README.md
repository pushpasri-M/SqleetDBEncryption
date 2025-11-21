# 🔒 SQLeet Database Encryption in C# WinForms

This project provides a **secure, encrypted SQLite database solution** for C# WinForms applications using **SQLeet**. It demonstrates how to integrate a custom-compiled native SQLite library (`sqleet.dll`) with a C# application using P/Invoke, ensuring that data is encrypted at rest.

## 🎯 Technical Overview

This application does **not** use the standard `System.Data.SQLite` NuGet package for encryption. Instead, it uses a **native C-compiled DLL (`sqleet.dll`)** that extends SQLite with encryption capabilities (typically ChaCha20-Poly1305).

### Key Technologies
*   **Language**: C# (.NET Framework 4.7.2+)
*   **Architecture**: **x86 (32-bit)** (Strict requirement due to the native DLL)
*   **Database Engine**: Custom SQLite3 with SQLeet extension
*   **Interop**: P/Invoke (`System.Runtime.InteropServices`)

### 🛡️ Encryption Workflow
1.  **Load Native Library**: The app loads `sqleet.dll` directly from the execution directory.
2.  **Open Database**: Uses `sqlite3_open_v2` with `ReadWrite | Create` flags.
3.  **Key Derivation**:
    *   **Existing DB**: Calls `sqlite3_key` immediately after opening to decrypt the header.
    *   **New DB**: Calls `sqlite3_rekey` to set the initial encryption key.
4.  **Page Size**: Enforces `PRAGMA page_size = 4096` for compatibility with the encryption algorithms.

---

## � How to Recreate This Project (Step-by-Step)

If you want to build this from scratch, follow these steps carefully.

### Phase 1: Project Setup
1.  **Create Project**:
    *   Open Visual Studio.
    *   Create a new **Windows Forms App (.NET Framework)**.
    *   Name it `EmployeeManagementForm`.
2.  **Set Architecture**:
    *   Go to **Build** > **Configuration Manager**.
    *   Under "Active solution platform", select **New...**.
    *   Choose **x86**. (Crucial! The provided `sqleet.dll` is likely 32-bit).
    *   Ensure the project runs in x86 mode.

### Phase 2: Native Library Integration
1.  **Obtain `sqleet.dll`**:
    *   You need a compiled version of sqleet. (Included in this repo under `Libs/` or `bin/`).
    *   **Action**: Copy `sqleet.dll` into your project's root folder.
2.  **Configure Build Action**:
    *   Right-click `sqleet.dll` in Solution Explorer -> **Properties**.
    *   **Copy to Output Directory**: Set to `Copy if newer` or `Always`.
    *   *Why?* The DLL must be in the same folder as the `.exe` at runtime.

### Phase 3: C# Wrapper (P/Invoke)
Create a class `SqleetWrapper.cs`. This acts as the bridge between C# and the C library.

```csharp
using System;
using System.Runtime.InteropServices;

public static class SqleetWrapper
{
    private const string DLL = "sqleet.dll"; // Must match filename exactly

    // Use CallingConvention.Cdecl for standard C libraries
    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqlite3_open_v2")]
    public static extern int sqlite3_open_v2(string filename, out IntPtr db, int flags, IntPtr zvfs);

    [DllImport(DLL, CallingConvention = CallingConvention.Cdecl, EntryPoint = "sqlite3_key")]
    public static extern int sqlite3_key(IntPtr db, byte[] key, int keyLength);

    // ... (See source code for full list of imports)
}
```

### Phase 4: Database Logic
Create `DatabaseManager.cs` to handle the logic.

1.  **Initialize**:
    ```csharp
    int rc = SqleetWrapper.sqlite3_open_v2("my_encrypted.db", out _db, Flags, IntPtr.Zero);
    ```
2.  **Encrypt**:
    ```csharp
    byte[] key = Encoding.UTF8.GetBytes("MyStrongKey123");
    SqleetWrapper.sqlite3_key(_db, key, key.Length);
    ```
3.  **Execute Queries**:
    *   Use `sqlite3_prepare_v2` to compile SQL.
    *   Use `sqlite3_bind_*` to prevent SQL injection.
    *   Use `sqlite3_step` to execute.

---

## 📂 Directory Structure

```text
ProjectRoot/
│
├── bin/x86/Debug/          # Output folder
│   ├── EmployeeManagementForm.exe
│   └── sqleet.dll          # <--- MUST BE HERE
│
├── DatabaseManager.cs      # High-level DB abstraction
├── SqleetWrapper.cs        # Low-level Native API definitions
├── Form1.cs                # UI Logic
└── sqleet.dll              # Original native library file
```

---

## 🔧 Troubleshooting & Common Errors

### ❌ `DllNotFoundException: Unable to load DLL 'sqleet.dll'`
*   **Cause**: The DLL is not in the `bin/x86/Debug` folder.
*   **Fix**: In Visual Studio, select `sqleet.dll`, go to Properties, and set **Copy to Output Directory** to **Copy Always**.

### ❌ `BadImageFormatException`
*   **Cause**: Architecture mismatch. You are running a 64-bit (Any CPU/x64) app trying to load a 32-bit DLL.
*   **Fix**: Change the Solution Platform to **x86**.

### ❌ Database is created but not encrypted
*   **Cause**: `sqlite3_key` was called *after* writing data, or `sqlite3_rekey` was not called for a new file.
*   **Fix**: Ensure `sqlite3_key` (or `rekey` for new files) is the **very first command** after opening the database.

---

## 🧪 Verification (Manual)

To prove the database is encrypted, try opening `employee_encrypted.db` in a standard tool like **DB Browser for SQLite**.
*   **Result**: It will fail to open or ask for a password (which standard tools might not support if the encryption scheme differs).
*   **Success**: The file is unreadable without the specific `sqleet` engine and key.

## � License
MIT License.
