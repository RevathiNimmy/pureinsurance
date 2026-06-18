# Tasks — Work Manager Dashboard

**Epic**: ADO #39678 — AIDLC: Work Manager Dashboard  
**Status**: All Tasks Complete  
**Last Updated**: 2026-05-19

---

## Task List

### T1: Database — Create stored procedures ✅

**Status**: Done  
**Deliverables**:
- 6 inner SPs (`spu_SAM_WorkManager*`) containing business logic
- 6 wrapper SPs (`spu_SAM_WorkManagerDashboard*`) for REST API compatibility
- SQL files at `Databases/Pure/Procedures/WorkManagerDashboard/`

**SPs Delivered**:
| Inner SP | Wrapper SP | Purpose |
|----------|-----------|---------|
| `spu_SAM_WorkManagerSummary` | `spu_SAM_WorkManagerDashboardSummary` | KPI counts + InProgressCount |
| `spu_SAM_WorkManagerTrend` | `spu_SAM_WorkManagerDashboardTrend` | Weekly trend (Mon-Sun) + TodayDueCount |
| `spu_SAM_WorkManagerTasksByGroup` | `spu_SAM_WorkManagerDashboardTasksByGroup` | Due tasks per user group |
| `spu_SAM_WorkManagerCompletedVsTotal` | `spu_SAM_WorkManagerDashboardCompletedVsTotal` | Monthly bar chart (18 months) |
| `spu_SAM_WorkManagerTasksByUser` | `spu_SAM_WorkManagerDashboardTasksByUser` | Per-user pending count (retained for future) |
| `spu_SAM_WorkManagerTasksDue` | `spu_SAM_WorkManagerDashboardTasksDue` | Task detail grid (retained for future) |

**Key Implementation Details**:
- Wrapper SPs accept `@suser_group_ids VARCHAR(255)` and `@sbranch_ids VARCHAR(255)` (comma-separated)
- Empty string converted to NULL in wrappers (`IF @param = '' SET @param = NULL`)
- Inner SPs use `STRING_SPLIT` + `CAST(value AS INT)` for filtering
- Branch filter uses `source_id` column, User Group filter uses `pmuser_group_id` column
- Task status: Due = IN (0,1,2,5), Completed = 3, InProgress = 1

---

### T2: UI — Modal Dashboard Page ✅

**Status**: Done  
**Deliverables**:
- `Modal/WorkManagerDashboard.aspx` — standalone page (no master page)
- `Modal/WorkManagerDashboard.aspx.vb` — code-behind (class: `Modal_WorkManagerDashboard`)

**Key Implementation Details**:
- Self-contained HTML with Bootstrap 5.3.0 + MDI Icons from CDN
- ApexCharts loaded from CDN
- Inherits `System.Web.UI.Page` (not `Frontend.clsCMSPage`)
- `Page_Load` always calls `BindDashboard()` (postback or not)
- `LoadFilters()` only on first load (populates Repeater + dropdowns)
- 4 SP calls with individual Try/Catch (Summary, Trend, TasksByGroup, CompletedVsTotal)
- `CallSPWithCache()` helper with 2-minute `HttpContext.Current.Cache`
- Chart data output via `litChartScripts` Literal as inline `<script>` variables

---

### T3: UI — Global Filters ✅

**Status**: Done  
**Deliverables**:
- Branch: multi-select dropdown with checkboxes (Bootstrap dropdown + Repeater + HiddenField)
- User Group: single-select DropDownList (`DataValueField = "UserGroupKey"`)
- Date Range: single-select DropDownList (`AutoPostBack="true"`)
- Refresh button: LinkButton triggering full postback

**Key Implementation Details**:
- Branch uses `asp:Repeater` rendering checkbox items, `asp:HiddenField` stores comma-separated BranchKey values
- Client-side JS restores checkbox state from hidden field after postback
- Button text updates: "All Branches" / "Name1, Name2" / "N selected"
- User Group passes numeric `UserGroupKey` (INT) not text `Code`
- No selection on Branch = all branches (empty string → NULL in SP)

---

### T4: UI — Task Summary (RadialBar + Progress Bars) ✅

**Status**: Done  
**Deliverables**:
- Left: ApexCharts radialBar (250px, completion %)
- Right: 3 progress bars — Tasks Assigned, Tasks Completed, Tasks In Progress
- Numbers display inline with label (label left, count right, bar below)
- Date Range dropdown within card header

**Data Mapping**:
- Tasks Assigned = `DueTaskCount` from SP (respects date range)
- Tasks Completed = `CompletedTaskCount` from SP
- Tasks In Progress = `InProgressCount` from SP
- RadialBar % = Completed / Assigned × 100
- Progress bar widths calculated server-side via `progCompleted.Style("width")`

---

### T5: UI — Weekly Trend Area Chart ✅

**Status**: Done  
**Deliverables**:
- Header: Current Week total | Previous Week total
- ApexCharts area chart (280px, smooth curve, gradient fill)
- "Today's Due Tasks" overlay
- Two series: Current Week (blue), Previous Week (green)

---

### T6: UI — Tasks per User Group Donut Chart ✅

**Status**: Done  
**Deliverables**:
- ApexCharts donut chart (260px)
- Legend list below with colour indicators and counts
- Zero-count groups excluded
- Legend cleared (`litGroupLegend.Text = ""`) at start of bind to prevent stale data

---

### T7: UI — Completed vs Total Bar Chart ✅

**Status**: Done  
**Deliverables**:
- ApexCharts bar chart (300px, 18 months history)
- Two series: Total Tasks (grey `#e3eaef`), Completed Tasks (blue `#727cf5`)
- Column width 50%, border radius 4

---

### T8: UI — Dashboard Button Control ✅

**Status**: Done  
**Deliverables**:
- `Controls/CtrlWorkManagerDashboard.ascx` + `.vb`
- Same pattern as `CtrlDashboard.ascx`
- Creates LinkButton dynamically with `tb_show()` OnClientClick
- Opens `Modal/WorkManagerDashboard.aspx?modal=true&KeepThis=true&TB_iframe=true&height=800&width=1200`
- `CustomSkinID` property (default "btnSM", used as "btnPrimary" in workmanager.aspx)
- Registered as `uc7:WMDashboard` in workmanager.aspx, placed next to Clear button

---

### T9: UI — ApexCharts JavaScript ✅

**Status**: Done  
**Deliverables**:
- `dashboard/assets/js/pages/workManagerDashboard.js`
- `WorkManagerDashboard` object with `init()`, `destroyChart()`, and 4 render methods
- Destroy/recreate pattern prevents memory leaks on postback
- "No data to display" message for empty datasets
- Init on `DOMContentLoaded` + `Sys.WebForms.PageRequestManager.add_endRequest`

---

### T10: Cleanup & Configuration ✅

**Status**: Done  
**Deliverables**:
- Removed `secure/workmanagerNew.aspx` + `.vb` (replaced by Modal page)
- Removed `NexusProvider/Objects/WorkManagerDashboard.vb` (unused model classes)
- Removed 6 abstract methods from `ProviderBase.vb` (unused)
- Removed `WorkManagerDashboard.vb` from `NexusProvider.vbproj`
- Reverted RibbonMenu.config (Internal, External, TPA) — removed dashboard nav link
- Renamed all SPs from `usp_work_manager_dashboard_*` to `spu_SAM_WorkManager*`
- Dropped `spu_SAM_WorkManagerDashboardAll` and `spu_SAM_WorkManagerDashboardTest`
- Updated SP SQL files in `Databases/Pure/Procedures/WorkManagerDashboard/`

---

## Summary

| Task | Title | Status |
|------|-------|--------|
| T1 | Database — Stored procedures | ✅ Done |
| T2 | UI — Modal Dashboard Page | ✅ Done |
| T3 | UI — Global Filters (Branch multi-select, User Group, Date Range) | ✅ Done |
| T4 | UI — Task Summary (RadialBar + Progress Bars) | ✅ Done |
| T5 | UI — Weekly Trend Area Chart | ✅ Done |
| T6 | UI — Tasks per User Group Donut Chart | ✅ Done |
| T7 | UI — Completed vs Total Bar Chart | ✅ Done |
| T8 | UI — Dashboard Button Control | ✅ Done |
| T9 | UI — ApexCharts JavaScript | ✅ Done |
| T10 | Cleanup & Configuration | ✅ Done |

---

## Final File Inventory

| File | Purpose |
|------|---------|
| `Modal/WorkManagerDashboard.aspx` | Dashboard modal page |
| `Modal/WorkManagerDashboard.aspx.vb` | Code-behind |
| `Controls/CtrlWorkManagerDashboard.ascx` | Button control |
| `Controls/CtrlWorkManagerDashboard.ascx.vb` | Button code-behind |
| `dashboard/assets/js/pages/workManagerDashboard.js` | Chart JS |
| `secure/workmanager.aspx` | Host page (Dashboard button added) |
| `Databases/Pure/Procedures/WorkManagerDashboard/*.sql` | 7 SP files |
