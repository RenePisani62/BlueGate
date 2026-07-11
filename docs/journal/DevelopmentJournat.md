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

# BlueGate Development Journal — 11 July 2026

## Objectives

* Complete the modular detection-rule architecture.
* Enrich alerts with MITRE ATT&CK information.
* Add persistent alert storage using SQLite.
* Read historical alerts back from the database.
* Prevent duplicate alerts when BlueGate reprocesses the same Sysmon event.

## Completed

* Introduced the `IDetectionRule` interface.
* Refactored `PowerShellDetection` from a static helper into an independent detection rule.
* Updated `DetectionEngine` to process a collection of `IDetectionRule` implementations.
* Maintained `SysmonNetworkEvent` as the canonical telemetry model.
* Added MITRE ATT&CK fields to the `Alert` model.
* Mapped PowerShell activity to:

  * Technique: `T1059.001 — PowerShell`
  * Tactic: `TA0002 — Execution`
* Added `Microsoft.Data.Sqlite` to the BlueGate Agent project.
* Created `AlertRepository` to initialise, write to and query the SQLite database.
* Successfully persisted a PowerShell outbound-connection alert.
* Added `GetAll()` support to retrieve historical alerts after BlueGate exits.
* Confirmed that BlueGate can report:

  * alerts generated during the current run;
  * alerts already stored in SQLite.
* Added the Sysmon Event Record ID to `SysmonNetworkEvent`.
* Added a database uniqueness rule using:

  * `EventRecordId`
  * `RuleName`
* Updated `Save()` to return whether SQLite inserted a new record or ignored a duplicate.
* Recreated the development database with the updated schema.
* Successfully stored and retrieved an alert using the revised database structure.

## Problems Resolved

### Nullable process image

The PowerShell rule generated a nullable-reference warning because `Image` may be null.

The detection logic was changed to use null-safe, case-insensitive matching.

### Alert display placement

The stored-alert query was initially placed inside the loop that displays newly generated alerts.

When no new alerts existed, the loop did not execute and historical alerts were not displayed. The database-query block was moved outside the loop.

### SQLite schema error

The project built successfully but failed at runtime with:

`SQLite Error 1: near "(": syntax error`

The SQL existed inside a valid C# string, so the C# compiler could not validate its SQL syntax. SQLite detected the problem only when `ExecuteNonQuery()` executed.

The `CREATE TABLE` statement was corrected by placing the composite `UNIQUE` constraint inside the table definition and fixing the preceding comma.

## Current Processing Flow

1. Read recent Sysmon Event ID 3 records.
2. Normalise records into `SysmonNetworkEvent` objects.
3. Evaluate each event using the registered detection rules.
4. Create enriched `Alert` objects for matches.
5. Attempt to save each alert to SQLite.
6. Ignore an alert when the same rule has already processed the same Sysmon Event Record ID.
7. Query historical alerts from SQLite.
8. Display current and stored alert information.

## Current Result

BlueGate can now collect telemetry, detect PowerShell outbound connections, enrich alerts with MITRE ATT&CK context, persist them in SQLite and retrieve them after the application has exited.

This represents the first complete persistence loop in the project.

## Next Steps

* Confirm duplicate suppression across repeated executions.
* Reconstruct the complete `SysmonNetworkEvent` when reading alerts from SQLite.
* Add repository queries such as:

  * latest alerts;
  * alerts by severity;
  * alerts by rule;
  * alerts by MITRE technique.
* Prepare the agent for continuous event processing.
