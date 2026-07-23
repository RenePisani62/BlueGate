ArchitectureGuide.md
Chapter 7 – Presentation Layer
Purpose

The Presentation Layer is responsible only for displaying operational information to the user.

Design

BlueGateAgent produces an AgentCycleResult after each monitoring cycle.

The dashboard consumes this object and displays it without performing any monitoring or detection logic.

Benefits
Monitoring logic remains independent of the user interface.
Additional interfaces (web dashboard, REST API, mobile client) can reuse the same monitoring data.
Presentation changes do not affect detection logic.
Future Direction

The current console dashboard is considered Version 1 of the operator interface.

Future versions will migrate this presentation to a web-based dashboard while reusing the same monitoring results.

Git Commit

I think today's commit message should reflect the architectural change rather than the cosmetic improvements:

Introduced presentation layer and live monitoring dashboard

or, a little more descriptive:

Added ConsoleDashboard, AgentCycleResult and operator monitoring dashboard

I slightly prefer the second one because it tells you exactly what changed when you're browsing the Git history months from now.

## Runtime Health Monitoring

BlueGate performs health checks against the external resources required by the
agent.

### Responsibilities

#### AlertRepository

Owns SQLite-specific access logic.

```text
TestConnection()
    attempts to open SQLite
    returns true or false

SysmonReader

Owns Windows Event Log and Sysmon-specific access logic.

IsAvailable()
    attempts to open the Sysmon Operational log
    returns true or false
HealthMonitor

Coordinates dependency health checks.

It does not directly implement SQLite or Windows Event Log access. Instead, it
calls the components that own that technical knowledge.

HealthMonitor
    ├── AlertRepository.TestConnection()
    └── SysmonReader.IsAvailable()
HealthStatus

Carries the resulting health state.

DatabaseConnected
SysmonAvailable
MonitoringActive
LastSuccessfulPoll
LastError
BlueGateAgent

Runs the monitoring cycle and transfers HealthStatus into AgentCycleResult.

Health values must be copied from HealthStatus rather than replaced with
hard-coded values.

ConsoleDashboard

Renders the health information for the operator. It does not perform health
checks itself.

Health Data Flow
SQLite
   ▲
AlertRepository
   │
   ├──────────────┐
   │              │
   ▼              │
HealthMonitor     │
   ▲              │
   │              │
SysmonReader      │
   ▲              │
   │              │
Windows Event Log│
                  │
HealthMonitor creates HealthStatus
                  │
                  ▼
             BlueGateAgent
                  │
                  ▼
           AgentCycleResult
                  │
                  ▼
          ConsoleDashboard
Design Rule

HealthMonitor is the authoritative coordinator of operational health.

Low-level components determine whether their own resources are available.
HealthStatus carries those results, and downstream components must preserve
them without substituting assumptions or hard-coded values.


A slightly cleaner diagram would be:

```text
SQLite ─────────► AlertRepository ───────┐
                                         │
Sysmon Event Log ► SysmonReader ─────────┤
                                         ▼
                                  HealthMonitor
                                         │
                                         ▼
                                   HealthStatus
                                         │
                                         ▼
                                   BlueGateAgent
                                         │
                                         ▼
                                  AgentCycleResult
                                         │
                                         ▼
                                 ConsoleDashboard

                                 ## Dependency Injection Introduced

BlueGate.Server now relies upon ASP.NET Core's built-in Dependency Injection container.

Health status retrieval is abstracted behind:

IHealthStatusProvider

Current implementation:

HealthStatusProvider

Future implementations may include:

- Local Agent Provider
- Remote REST Provider
- SignalR Provider
- Multi-Agent Provider

The PageModel now depends only upon the interface rather than a concrete implementation.

## Dashboard Health Monitoring

BlueGate.Server now performs live health checks rather than relying
upon static values.

Current live telemetry:

- SQLite connectivity
- Alert count

Health information is gathered by HealthStatusProvider.

Future work will migrate all SQL operations into AlertRepository so
that HealthStatusProvider only coordinates health information rather
than directly accessing the database.

This establishes the following architecture:

Browser
→ Razor Page
→ HealthStatusProvider
→ AlertRepository
→ SQLite