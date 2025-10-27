🔒 SQLeet Encrypted Database Integration in C# (WinForms)

This project demonstrates how to implement full database-level encryption in a C# WinForms application using SQLeet, a lightweight encryption extension for SQLite.
The database is encrypted using the fixed key: MyStrongKey123.

It includes:
✅ A custom-compiled sqleet.dll
✅ A C# wrapper (SqleetWrapper.cs) for native SQLeet functions
✅ A managed database manager (DatabaseManager.cs)
✅ A WinForms UI to insert and view employee data
✅ A CLI binary (sqleet.exe) provided for manual testing & verification

📂 Project Structure
SqleetDBEncryption/
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

📌 Key Files & Responsibilities
File	Purpose
sqleet.dll	Native SQLite engine patched with AES-256 encryption
SqleetWrapper.cs	Bridges C# & native library (via [DllImport])
DatabaseManager.cs	Applies key, creates tables, inserts data
sqleet.exe	CLI for opening/testing encrypted DB manually
🔧 Compilation of SQLeet DLL (Already Done for You ✅)

SQLeet (C-based) was compiled into a Windows DLL using GCC/MSYS2:

gcc sqleet.c -shared -o sqleet.dll


This DLL is now ready and shipped in /Libs/.

🖥️ Running the App (WinForms UI)

✅ When the app runs for the first time:
✔ Creates employee_encrypted.db
✔ Applies encryption key (MyStrongKey123)
✔ Creates Employees table

✅ When inserting employees:
✔ Each record is encrypted transparently via engine-level encryption

🔍 Testing Encryption with CLI (Manual Check)

To confirm encryption manually:

sqleet.exe
sqlite> .open employee_encrypted.db
sqlite> PRAGMA key='MyStrongKey123';
sqlite> .tables


✅ If correct key used → Employees table becomes visible
❌ If wrong key → "file is not a database" or no tables shown

🗝️ Encryption Details
Parameter	Value
Encryption Engine	SQLeet (AES-256-CTR)
Page Size	4096 bytes
Key Length	Auto-detected from UTF-8 string
Key Used	MyStrongKey123
📜 License (MIT)

This project is licensed under the MIT License, allowing:
✅ Commercial and private use
✅ Modification and distribution
✅ Liability protections

📌 Full license text is included in LICENSE.

✨ Author

👤 Pushpasri M
💻 Passionate about secure database systems & C# development

📬 Contributions & Feedback

🛠 Open to suggestions! Feel free to:
✅ Fork ⭐
✅ Open issues
✅ Submit PRs
