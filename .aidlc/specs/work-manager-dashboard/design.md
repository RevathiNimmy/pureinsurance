# Design — Work Manager Dashboard

**Epic**: ADO #39678 — AIDLC: Work Manager Dashboard  
**Status**: Delivered  
**Last Updated**: 2026-05-19

---

## 1. Architecture Overview

The Work Manager Dashboard is a standalone ASP.NET WebForms modal page opened from the existing Work Manager page via an iframe modal (`tb_show()`). It calls stored procedures directly via the REST API's `CallNamedStoredProcedure` method and renders charts client-side with ApexCharts.

```
┌─────────────────────────────────────────────────────────┐
│  secure/workmanager.aspx                                │
│  └─ CtrlWorkManagerDashboard.ascx (Dashboard button)    │
│      └─ Opens modal via tb_show()                       │
├─────────────────────────────────────────────────────────┤
│  Modal/WorkManagerDashboard.aspx (standalone, no master)│
│  ├─ Bootstrap 5 + MDI icons (CDN)                       │
│  ├─ ApexCharts (CDN)                                    │
│  └─ workManagerDashboard.js (chart rendering)           │
├─────────────────────────────────────────────────────────┤
│  WorkManagerDashboard.aspx.vb (Code-behind)             │
│  ├─ LoadFilters() — branches (Repeater), user groups    │
│  ├─ BindDashboard() — calls 4 wrapper SPs              │
│  └─ CallSPWithCache() — 2-min ASP.NET Cache            │
├─────────────────────────────────────────────────────────┤
│  REST API (localhost:7246) → dPMDAO                      │
│  └─ CallNamedStoredProcedure(params, spName)            │
├─────────────────────────────────────────────────────────┤
│  Database: RDCOMQA8 on RDCOMDEV1                         │
│  ├─ spu_SAM_WorkManagerDashboardSummary (wrapper)       │
│  │   └─ spu_SAM_WorkManagerSummary (inner)              │
│  ├─ spu_SAM_WorkManagerDashboardTrend (wrapper)         │
│  │   └─ spu_SAM_WorkManagerTrend (inner)                │
│  ├─ spu_SAM_WorkManagerDashboardTasksByGroup (wrapper)  │
│  │   └─ spu_SAM_WorkManagerTasksByGroup (inner)         │
│  └─ spu_SAM_WorkManagerDashboardCompletedVsTotal (wrap) │
│       └─ spu_SAM_WorkManagerCompletedVsTotal (inner)    │
└─────────────────────────────────────────────────────────┘
```

---

## 2. File Locations

| File | Path | Purpose |
|------|------|---------|
| Modal page | `Pure.Portals/Modal/WorkManagerDashboard.aspx` | Dashboard UI (standalone HTML, no master) |
| Code-behind | `Pure.Portals/Modal/WorkManagerDashboard.aspx.vb` | Server-side data binding |
| Button control | `Pure.Portals/Controls/CtrlWorkManagerDashboard.ascx` | LinkButton to open modal |
| Button code-behind | `Pure.Portals/Controls/CtrlWorkManagerDashboard.ascx.vb` | Creates button with `tb_show()` |
| JS (charts) | `Pure.Portals/dashboard/assets/js/pages/workManagerDashboard.js` | ApexCharts init |
| Host page | `Pure.Portals/secure/workmanager.aspx` | Hosts the Dashboard button control |
| DB scripts | `Databases/Pure/Procedures/WorkManagerDashboard/` | All SP SQL files |

---

## 3. Page Layout

```
┌──────────────────────────────────────────────────────────────┐
│ FILTERS: [Branch ▼ checkboxes] [User Group ▼] [Refresh btn]  │
├──────────────────────────────────────────────────────────────┤
│ ROW 1 (50/50):                                               │
│ ┌──────────────────────────┐ ┌──────────────────────────┐   │
│ │ Task Summary             │ │ Completed vs Total Tasks  │   │
│ │ [Date Range ▼]           │ │ (bar chart - 18 months)   │   │
│ │                          │ │                           │   │
│ │ ┌──────┐ ┌───────────┐  │ │                           │   │
│ │ │Radial│ │Assigned: N│  │ │                           │   │
│ │ │ Bar  │ │Completed:N│  │ │                           │   │
│ │ │ 26%  │ │InProgress:N│ │ │                           │   │
│ │ └──────┘ └───────────┘  │ │                           │   │
│ └──────────────────────────┘ └──────────────────────────┘   │
├──────────────────────────────────────────────────────────────┤
│ ROW 2 (66/33):                                               │
│ ┌───────────────────────────────┐ ┌────────────────────┐    │
│ │ Weekly Trend of Due Tasks     │ │ Tasks per User Group│    │
│ │ Current Week: N | Prev Week: N│ │ (donut chart)       │    │
│ │ (area chart Mon-Sun)          │ │ + legend list       │    │
│ │ Today's Due: N                │ │                     │    │
│ └───────────────────────────────┘ └────────────────────┘    │
└──────────────────────────────────────────────────────────────┘
```

---

## 4. Component Design

### 4.1 Modal Trigger (CtrlWorkManagerDashboard.ascx)

Same pattern as `CtrlDashboard.ascx`:
- Creates a `LinkButton` dynamically in `Page_Load`
- `OnClientClick` calls `tb_show()` opening `Modal/WorkManagerDashboard.aspx?modal=true&KeepThis=true&TB_iframe=true&height=800&width=1200`
- Supports `CustomSkinID` property for button styling
- Registered in `workmanager.aspx` as `uc7:WMDashboard` next to Clear button

### 4.2 Branch Multi-Select (Dropdown with Checkboxes)

- Bootstrap 5 dropdown button + `<ul>` menu with checkbox items
- `asp:Repeater` renders checkboxes from `oUserDetails.ListOfBranches`
- `asp:HiddenField` (`hdnSelectedBranches`) stores comma-separated selected BranchKey values
- Client-side JS:
  - Prevents dropdown close on click inside (`e.stopPropagation()`)
  - Updates hidden field and button text on checkbox change
  - Restores checked state from hidden field after postback

### 4.3 Data Access Pattern

All SP calls use `CallNamedStoredProcedure` directly from the page code-behind (same pattern as `Modal/Dashboard.aspx.vb`):

```vbnet
Dim params As New List(Of StoredProcedureParameterType) From {
    New StoredProcedureParameterType With {.ParamName = "suser_group_ids", .ParamValue = sGroupIds},
    New StoredProcedureParameterType With {.ParamName = "sbranch_ids", .ParamValue = sBranchIds}
}
Dim result = CallSPWithCache(oWebservice, params.ToArray(), "spu_SAM_WorkManagerDashboardTrend")
Dim xdoc As XDocument = XDocument.Parse(result.Results)
```

### 4.4 Caching

`CallSPWithCache` helper builds a cache key from SP name + all parameter values, stores results in `HttpContext.Current.Cache` with 2-minute absolute expiration. Prevents API rate limiting (429 errors) when making 4 SP calls per page load.

### 4.5 Chart Data Flow

1. Code-behind builds `sChartScripts` string with JS variable declarations
2. `litChartScripts` literal outputs inline `<script>` before chart divs
3. `workManagerDashboard.js` reads global variables (`perfData`, `trendData`, `groupData`, `monthlyData`)
4. Charts render on `DOMContentLoaded`

---

## 5. Database Design

### 5.1 Stored Procedure Naming Convention

- **Inner SPs** (`spu_SAM_WorkManager*`): Contain the business logic, accept `@user_group_ids VARCHAR(MAX)` and `@branch_ids VARCHAR(MAX)` (comma-separated IDs)
- **Wrapper SPs** (`spu_SAM_WorkManagerDashboard*`): Called by REST API, translate `s`/`l` prefixed params, convert empty strings to NULL

### 5.2 SP Parameters (API Convention)

| Prefix | SQL Type | Example |
|--------|----------|---------|
| `s` | VARCHAR(255) | `@suser_group_ids`, `@sbranch_ids` |
| `l` | INT | `@ldate_range`, `@lmonths` |

### 5.3 Active Stored Procedures

| Wrapper SP (API calls this) | Inner SP (logic) | Parameters |
|----------------------------|------------------|------------|
| `spu_SAM_WorkManagerDashboardSummary` | `spu_SAM_WorkManagerSummary` | suser_group_ids, sbranch_ids, ldate_range |
| `spu_SAM_WorkManagerDashboardTrend` | `spu_SAM_WorkManagerTrend` | suser_group_ids, sbranch_ids |
| `spu_SAM_WorkManagerDashboardTasksByGroup` | `spu_SAM_WorkManagerTasksByGroup` | suser_group_ids, sbranch_ids |
| `spu_SAM_WorkManagerDashboardCompletedVsTotal` | `spu_SAM_WorkManagerCompletedVsTotal` | suser_group_ids, sbranch_ids, lmonths |
| `spu_SAM_WorkManagerDashboardTasksByUser` | `spu_SAM_WorkManagerTasksByUser` | suser_group_ids, sbranch_ids |
| `spu_SAM_WorkManagerDashboardTasksDue` | `spu_SAM_WorkManagerTasksDue` | suser_group_ids, sbranch_ids, lfor_user_id, ldate_range |

Note: TasksByUser and TasksDue are not used by the current modal page but are retained for future use.

### 5.4 Key Tables

| Table | Purpose |
|-------|---------|
| `PMWrk_Task_Instance` | Main task table — `task_status`, `task_due_date`, `pmuser_group_id`, `source_id` (branch), `is_visible`, `is_urgent`, `user_id` |
| `PMUser_Group` | User group lookup — `pmuser_group_id`, `description` |
| `PMUser` | User lookup — `user_id`, `username` |
| `source` | Branch lookup — `source_id`, `description` |

### 5.5 Task Status Values

| Status | Value | Included in "Due" |
|--------|-------|-------------------|
| New | 0 | ✅ |
| In Progress | 1 | ✅ |
| Incomplete | 2 | ✅ |
| Complete | 3 | ❌ |
| Not Complete | 5 | ✅ |

---

## 6. Client-Side Architecture

### 6.1 ApexCharts Configuration

| Chart | Type | Container ID | Data Variable |
|-------|------|-------------|---------------|
| Performance | radialBar (250px) | `#performance-chart` | `perfData.percent` |
| Weekly Trend | area (280px) | `#weekly-trend-chart` | `trendData` |
| Tasks by Group | donut (260px) | `#tasks-by-group-chart` | `groupData` |
| Completed vs Total | bar (300px) | `#completed-vs-total-chart` | `monthlyData` |

### 6.2 Chart Destroy/Recreate Pattern

Each chart uses `destroyChart(key)` before re-rendering to prevent memory leaks on postback. Shows "No data to display" message when data arrays are empty.

---

## 7. Dependencies

| Dependency | Source | Notes |
|-----------|--------|-------|
| Bootstrap 5.3.0 | CDN | Modal page loads its own (no master page) |
| MDI Icons 7.2.96 | CDN | Material Design Icons |
| ApexCharts | CDN (`cdn.jsdelivr.net/npm/apexcharts`) | Chart library |
| NexusProvider | Existing | `ProviderBase`, `ProviderManager`, `UserDetails`, `StoredProcedureParameterType` |
| System.Xml.Linq | .NET Framework | XDocument for XML parsing |

---

## 8. Key Design Decisions

1. **Modal page, not standalone** — Opens via `tb_show()` iframe from workmanager.aspx, not as a navigable page
2. **No master page** — Self-contained HTML with CDN resources for isolation
3. **No NexusProvider model classes** — Calls `CallNamedStoredProcedure` directly, parses XML with XDocument (simpler, no DLL rebuild needed)
4. **No UpdatePanel** — Full postback on filter change (charts re-render via DOMContentLoaded)
5. **Wrapper + Inner SP pattern** — Wrappers handle API param convention (`s`/`l` prefix), inner SPs contain logic
6. **Branch multi-select via checkbox dropdown** — Custom Bootstrap 5 dropdown with hidden field for state
7. **UserGroupKey for filtering** — Dropdown passes numeric `pmuser_group_id` (not text Code) for SP compatibility
8. **2-minute cache** — Prevents 429 rate limiting from REST API
