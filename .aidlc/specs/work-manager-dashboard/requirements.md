# Requirements — Work Manager Dashboard

**Epic**: ADO #39678 — AIDLC: Work Manager Dashboard  
**Source Feature**: ADO #22330 — 6.4 CP0 Estimating Feature - Work Manager Dashboard  
**Status**: Delivered  
**Last Updated**: 2026-05-19

---

## 1. Overview

A Work Manager Dashboard accessible as a **modal popup** from the existing Work Manager page (`workmanager.aspx`). It provides managers and team leads with a consolidated view of task workload distribution, performance metrics, and trend analysis across branches and user groups.

The dashboard opens via a "Dashboard" button placed next to the Clear button on the Work Manager page, using the same `tb_show()` modal pattern as the existing `CtrlDashboard.ascx`.

---

## 2. Business Objectives

- Enable managers to monitor team workload at a glance
- Identify overdue tasks and bottlenecks by user group and branch
- Track performance trends (weekly, monthly)
- Provide quick access from existing Work Manager page without navigation

---

## 3. Functional Requirements

### 3.1 Dashboard Access

**FR-01**: A "Dashboard" button on `workmanager.aspx` (next to Clear button) opens the dashboard in a modal iframe (1200×800px).

**FR-02**: The dashboard is a standalone page (`Modal/WorkManagerDashboard.aspx`) that does not use a master page.

### 3.2 Global Filters

**FR-03**: The dashboard header includes three filters:
- **Branch** — multi-select dropdown with checkboxes, populated with branches the logged-in user has access to
- **Team/User Group** — single-select dropdown, populated with active user groups
- **Refresh** button — applies filter selections to all dashboard sections

**FR-04**: Branch dropdown behaviour:
- Displays as a dropdown button with checkbox list inside
- Supports multiple selection (Ctrl/Shift or checkboxes)
- No selection = All Branches
- Button text updates: "All Branches" / branch names (1-2) / "N selected" (3+)
- Selected state preserved across postback (Refresh)

**FR-05**: Team/User Group dropdown behaviour:
- First value: "All Groups"
- Single selection
- If user is member of only one group, default it and disable

### 3.3 Task Summary Section (ROW 1 - Left)

**FR-06**: Display a "Task Summary" card with:
- **RadialBar chart** (left) — shows completion percentage (Completed ÷ Assigned × 100)
- **Progress bars** (right) — three metrics:
  - Tasks Assigned — count with blue 100% bar
  - Tasks Completed — count with green bar (width = % of assigned)
  - Tasks In Progress — count with yellow bar (width = % of assigned)

**FR-07**: A Date Range dropdown filter within the card (default: first item/AllDates). On change, Task Summary refreshes via full postback.

**FR-08**: Data definitions:
- Tasks Assigned = all due tasks (status 0,1,2,5) within selected date range
- Tasks Completed = tasks with status 3, completed within date range
- Tasks In Progress = tasks with status 1 within date range

### 3.4 Completed Tasks vs Total Tasks (ROW 1 - Right)

**FR-09**: Display a bar chart showing month-wise comparison (last 18 months):
- X-axis: Months (MMM yyyy format)
- Y-axis: Number of tasks
- Light grey bar: Total Tasks
- Dark blue bar: Completed Tasks

**FR-10**: Tooltips on hover showing exact values.

### 3.5 Weekly Trend of Due Tasks (ROW 2 - Left)

**FR-11**: Display metrics section:
- Current Week total and Previous Week total (Mon–Sun)

**FR-12**: Area chart comparing current vs previous week:
- X-axis: Days (Mon–Sun abbreviated)
- Y-axis: Due task count
- Blue area: Current Week
- Green area: Previous Week

**FR-13**: Display "Today's Due Tasks: [count]" overlay.

### 3.6 Tasks per User Group (ROW 2 - Right)

**FR-14**: Donut chart showing due task counts grouped by User Group.

**FR-15**: Groups with zero tasks are excluded.

**FR-16**: Legend list below chart with colour indicators and counts.

**FR-17**: Legend clears properly when no data is available (no stale data display).

---

## 4. Non-Functional Requirements

**NFR-01**: Dashboard must load in ≤ 5 seconds.

**NFR-02**: API rate limiting mitigated with 2-minute server-side cache per SP call + parameters.

**NFR-03**: UI responsive — Bootstrap 5 grid layout adapts to modal size.

**NFR-04**: All data is read-only.

---

## 5. Security Requirements

**SEC-01**: Branch filter populated only with user's accessible branches (`oUserDetails.ListOfBranches`).

**SEC-02**: User Group filter populated only with active groups from user's memberships (`oUserDetails.AvailableUsergroups`).

**SEC-03**: Stored procedures filter by `source_id` (branch) and `pmuser_group_id` using comma-separated ID lists.

---

## 6. Data Definitions

| Term | Definition |
|------|-----------|
| Due/Assigned Tasks | task_status IN (0=New, 1=InProgress, 2=Incomplete, 5=NotComplete), is_visible=1 |
| Completed Tasks | task_status = 3 |
| In Progress Tasks | task_status = 1 |
| Current Week | Monday to Sunday of the current calendar week |
| Previous Week | Monday to Sunday of the prior calendar week |

---

## 7. Out of Scope (Removed from Original Requirements)

- **Tasks Due grid** (FR-24 to FR-28) — removed to simplify modal view
- **Tasks by User grid** (FR-29 to FR-33) — removed to simplify modal view
- **RibbonMenu links** — dashboard is not a standalone navigation item; accessed only via modal button
- **NexusProvider abstract methods** — not used; page calls `CallNamedStoredProcedure` directly
- **Model classes in NexusProvider** — not used; XML parsed directly with XDocument/LINQ

---

## 8. Related ADO Work Items

| ID | Title | Type |
|----|-------|------|
| #22330 | 6.4 CP0 Estimating Feature - Work Manager Dashboard | Feature |
| #39678 | AIDLC: Work Manager Dashboard | Epic |

---

## Approval

- [x] Requirements delivered and functional
