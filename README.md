# Leave Management System

## Project Overview

Leave Management System is an ASP.NET Core Razor Pages based web application developed using N-Tier Architecture.

The system allows employees to:
- Apply for Leave
- Apply for Work From Home (WFH)
- Apply for OnDuty requests
- Upload supporting documents

The admin can:
- Manage Employees
- Manage Departments
- Manage Designations
- Manage Leave Categories
- Approve or Reject Leave Requests
- Track WFH and OnDuty requests

---

# Technologies Used

- ASP.NET Core Razor Pages
- C#
- MySQL
- Enterprise Library Data
- Bootstrap 5
- NLog
- HTML Helpers
- N-Tier Architecture

---

# Project Architecture

The project follows N-Tier Architecture:

```text
UI Layer
↓
Service Layer
↓
Data Layer
↓
Database
