# 🔒 SQLeet Database Encryption in C# WinForms

This project demonstrates how to secure an SQLite database using **SQLeet encryption ** (https://github.com/resilar/sqleet?tab=readme-ov-file#example-source) inside a **C# WinForms application**.  
The database is fully encrypted using a **fixed passphrase:** `MyStrongKey123`.

✅ Includes a **compiled `sqleet.dll` for C# integration**  
✅ A **WinForms frontend** to add/view employees  
✅ A **C# native wrapper (P/Invoke)** for SQLeet  
✅ A **CLI tool (`sqleet.exe`) to verify encryption manually**

---

## 📂 Project Structure


---
<pre> ```SqleetDBEncryption/
├── /Libs/
│   ├── sqleet.dll          # 🔹 Custom compiled encrypted SQLite engine
│   ├── sqleet.exe          # 🔹 CLI tool to test encrypted DB manually
├── /EmployeeManagementForm/
│   ├── SqleetWrapper.cs    # 🔹 P/Invoke bridge to sqleet.dll
│   ├── DatabaseManager.cs  # 🔹 Handles encryption, CRUD ops
│   ├── UI (Forms)          # 🔹 WinForms interface
│   ├── App.config
│   ├── Program.cs
├── employee_encrypted.db   # 🔹 Auto-created/encrypted at first run
├── README.md               # 📘 You're reading this!
└── LICENSE                 # 📜 MIT License
 ``` </pre>



## ⚙️ How It Works

1️⃣ WinForms UI takes employee details  
2️⃣ `DatabaseManager.cs` opens SQLite via `sqleet.dll`  
3️⃣ Encryption key is set: `MyStrongKey123`  
4️⃣ DB is created (if not exist) and fully encrypted  
5️⃣ Data is inserted securely into `Employees` table  

✔ Without the key, the DB cannot be opened.

---

## 🧪 Test Database Encryption (Using CLI)

You can verify encryption with the bundled CLI:



⚙️ How It Works (End-to-End Flow)
[WinForms UI] 
   └▶ Collects Employee Data
      └▶ DatabaseManager.cs
         └▶ Opens or creates DB using sqlite3_open_v2
         └▶ Applies encryption key using sqlite3_key
         └▶ Rekeys new DB (for first-time encryption)
         └▶ Inserts employee using encrypted SQL ops


✅ At no point is data stored unencrypted.
✅ Without the key MyStrongKey123, the database is unreadable.

## 📌 Key Files & Responsibilities
File	Purpose
sqleet.dll	Native SQLite engine patched with AES-256 encryption
SqleetWrapper.cs	Bridges C# & native library (via [DllImport])
DatabaseManager.cs	Applies key, creates tables, inserts data
sqleet.exe	CLI for opening/testing encrypted DB manually
🔧 Compilation of SQLeet DLL (Already Done for You ✅)

SQLeet (C-based) was compiled into a Windows DLL using GCC/MSYS2:

gcc sqleet.c -shared -o sqleet.dll


This DLL is now ready and has been shipped to/Libs/.

## 🖥️ Running the App (WinForms UI)

✅ When the app runs for the first time:
✔ Creates employee_encrypted.db
✔ Applies encryption key (MyStrongKey123)
✔ Creates the Employees table

✅ When inserting employees:
✔ Each record is encrypted transparently via engine-level encryption

🔍 Testing Encryption with CLI (Manual Check)

To confirm encryption manually:

sqleet.exe
sqlite> .open employee_encrypted.db
sqlite> PRAGMA key='MyStrongKey123';
sqlite> .tables



✅ If key is correct → `Employees` table is visible  
❌ Otherwise → DB cannot be read

---

## 🗝️ Encryption Info

| Feature          | Value           |
|------------------|----------------|
| Engine           | SQLeet  |
| Key Used         | `MyStrongKey123` |
| Applies To       | Entire DB file  |
| Visible Without Key? | ❌ No        |

---

## 📜 License

Licensed under the **MIT License**, allowing free use, modification, and distribution.

---

## 👩‍💻 Author

**Pushpasri M**  
💡 Focused on secure system development & C# application design.

---

## 💬 Contributions & Suggestions

Feel free to:
✅ ⭐ Star the project  
✅ 🛠 Improve features  
✅ 🐛 Report issues  

---

🚀 *Thank you for exploring secure database development with SQLeet + C#!*
