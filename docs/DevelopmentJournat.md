# Sprint 2 – Sysmon Telemetry

## Objective
Read and normalize Sysmon Event ID 3 network connection events.

## Achievements
- Implemented SysmonReader
- Queried Windows Event Log
- Enabled Event ID 3 logging
- Parsed XML into SysmonNetworkEvent
- Successfully displayed structured telemetry
- Added .gitignore and cleaned repository

## Lessons Learned
- Windows Event Logs require administrator privileges for this log.
- Sysmon XML is preferable to FormatDescription() for structured parsing.
- Git ignores new files with .gitignore but tracked files must be removed from the index separately.
