
---

# # HangFire Background Job Processing System

ASP.NET MVC Documentation (.NET 9)

---

## 📌 Project Overview

**HangFireDemo** is a background automation system built using **ASP.NET Core MVC (.NET 9)**.

It automates backend tasks such as daily email reports, automatic expiration of records, and database cleanup. The system uses **Hangfire** to schedule, monitor, and run background jobs, and provides a dashboard to track job status in real time.

The project also includes:

* SMTP email integration
* Manual job trigger interface
* EF Core SQL Server storage

---

## 🚀 Features

### ✔ Background Job Processing

* Fire-and-forget jobs
* Scheduled recurring jobs
* Automatic retries
* Fully managed execution

### ✔ Automated Jobs Included

* **Daily Summary Email Job**
* **Auto-Expire Records Job**
* **Database Cleanup Job**

### ✔ Email Notification System

* SMTP-based email sending
* Supports Gmail App Passwords
* Customizable email content

### ✔ Hangfire Dashboard

* View job history
* Monitor failed/successful jobs
* Track scheduled jobs
* Real-time job execution monitoring

### ✔ Manual Job Trigger UI

* Buttons to trigger all jobs
* Success confirmation message
* Useful for admin or testing

### ✔ Database Support

* SQL Server + Entity Framework Core
* Table for storing application records
* Integrated with Hangfire storage tables

---

## 🧱 Project Structure

```
HangFireDemo/
│
├── Controllers/
│     ├── RecordsController.cs
│     └── JobsController.cs
│
├── Services/
│     ├── IEmailService.cs
│     └── EmailService.cs
│
├── Jobs/
│     ├── DailySummaryJob.cs
│     ├── RecordExpiryJob.cs
│     └── DbCleanupJob.cs
│
├── Data/
│     └── AppDbContext.cs
│
├── Models/
│     ├── AppRecord.cs
│     └── EmailSettings.cs
│
├── Views/
│     ├── Records/
│     └── Jobs/
│          └── Index.cshtml
│
├── appsettings.json
└── Program.cs
```

---

## ⚙️ Configuration (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=HangfireDemoDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "EnableSSL": true,
    "UserName": "yourgmail@gmail.com",
    "Password": "your-app-password",
    "FromEmail": "yourgmail@gmail.com",
    "FromName": "Hangfire System"
  }
}
```

---

## 🗄 Database Schema (AppRecord)

| Column     | Type     | Description               |
| ---------- | -------- | ------------------------- |
| Id         | int      | Primary key               |
| Title      | string   | Record description        |
| CreatedAt  | DateTime | Time record was created   |
| ExpiryDate | DateTime | When record should expire |
| IsExpired  | bool     | Expiration status         |

---

## 📤 Background Job Execution Flow

```
Hangfire Server Starts → Register Recurring Jobs → Execute Daily Tasks → Mark Expired Data → Cleanup Database → Log Results in Dashboard
```

### Background execution logic (step-by-step)

1. Hangfire server starts when the application runs
2. Scheduler loads recurring jobs
3. Daily summary email job sends statistics
4. Auto-expire job marks outdated records
5. Cleanup job removes expired data older than 30 days
6. Admin can manually trigger jobs via UI
7. Dashboard logs job execution history

---

## 🔐 Email Notification System

Email notifications are sent via SMTP configured in `EmailSettings`.

### SMTP Logic

```csharp
await smtp.SendMailAsync(message);
```

### Supported Providers

* Gmail (App Password required)
* Outlook
* Zoho
* Custom SMTP servers

If email sending fails, Hangfire will **automatically retry the job**.

---

## 🧩 Background Jobs

### ✔ Daily Summary Email Job

Sends system summary including:

* Total records
* Newly created records (last 24 hours)
* Newly expired records

### ✔ Auto-Expire Records Job

Runs **hourly** and sets:

```
IsExpired = true
```

when:

```
ExpiryDate <= DateTime.UtcNow
```

### ✔ Database Cleanup Job

Runs **daily at 2 AM** and removes records that:

```
IsExpired == true
AND ExpiryDate <= Now - 30 days
```

---

## 🧩 Controllers

### ✔ RecordsController

Handles:

* Displaying records
* Creating new records
* Passing data to views

### ✔ JobsController

Handles:

* Manual triggering of background jobs
* Displays admin job panel with buttons

---

## 🖥 Views (Razor)

### ✔ Records Pages

* List all stored records
* Create new record
* Shows expiry status

### ✔ Jobs Admin Page

Buttons:

* Run Daily Summary Email
* Run Auto Expire Job
* Run Cleanup Job

Useful for manual testing or admin access.

---

## 📊 Hangfire Dashboard (Job Monitoring)

Access:

```
/hangfire
```

Dashboard includes:

✔ Recurring jobs
✔ Scheduled jobs
✔ Failed jobs
✔ Succeeded jobs
✔ Retry logs
✔ Processing jobs
✔ Server health

---

## 🧪 Testing Email System

### Steps:

1. Enable Gmail 2FA
2. Create **App Password**
3. Replace SMTP password in `appsettings.json`
4. Trigger email from:

```
/jobs
```

You should receive an email instantly.

---

## ▶️ Running the Project

### 1. Install EF Tools

```bash
dotnet tool install --global dotnet-ef
```

### 2. Apply Migrations

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 3. Run the Application

```bash
dotnet run
```

Navigate to:

```
https://localhost:5001/records
https://localhost:5001/jobs
https://localhost:5001/hangfire
```

---

## 🔧 Future Enhancements

* Add email templates (HTML)
* Add execution analytics (charts)
* Role-based dashboard authorization
* Slack/Webhook/Teams notifications
* Store job execution logs in DB

---

## 📄 Conclusion

This ASP.NET MVC project provides a **production-ready Hangfire background job system**, including:

* Automatic daily tasks
* Scheduled maintenance
* Email alert system
* Manual job control panel
* Realtime dashboard monitoring


---


