# Work Life Balance Tracker
## Purpose
The purpose of this application is to track your time on the computer. It starts tracking when you're logged in or unlock your computer, and stops logging when you lock or logoff your computer.

## Installation
- Clone the repository
- Build it in Visual Studio
- Install the service
  - PowerShell `New-Service -Name "Work Life Balance Tracker" -BinaryPathName "PATH_TO_THE_EXE"`
- Start the service
- By default, you can view your time by going to http://localhost:1234
  - You can change the port in the app.config file before compiling
