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


# 10 July 2026

## Completed

- Detection Engine created.
- First PowerShell alert generated.
- Duplicate project removed.
- Git repository cleaned.
- Remote URL updated.

## Lessons Learned

Never edit repository files directly in GitHub while actively developing locally.

Duplicate projects can confuse IntelliSense while still allowing successful builds.

## Next Sprint

Introduce Alert model.

Begin Detection Rule architecture.
