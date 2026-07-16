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