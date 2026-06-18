<%@ Page Language="VB" AutoEventWireup="false"
    CodeFile="WorkManagerDashboard.aspx.vb" Inherits="Nexus.Modal_WorkManagerDashboard" %>

<!DOCTYPE html>
<html>
<head>
    <title>Work Manager Dashboard</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@mdi/font@7.2.96/css/materialdesignicons.min.css" />
    <style>
        body { background: #f5f6f8; font-family: 'Nunito', sans-serif; padding: 15px; }
        .card-h-100 { height: 100%; }
        .widget-icon { font-size: 24px; color: #727cf5; opacity: 0.5; }
        .grid-pager td { padding: 8px 0; }
        .grid-pager td table { margin-left: auto; }
        .grid-pager td table td { padding: 2px; }
        .grid-pager td table td a, .grid-pager td table td span {
            display: inline-block; padding: 4px 10px; margin: 0 2px;
            border: 1px solid #dee2e6; border-radius: 4px;
            text-decoration: none; color: #727cf5; font-size: 13px;
        }
        .grid-pager td table td span {
            background-color: #727cf5; color: #fff; border-color: #727cf5;
        }
        .grid-pager td table td a:hover { background-color: #f1f3fa; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"></asp:ScriptManager>

    <div class="container-fluid">
        <!-- Page Title -->
        <div class="row">
            <div class="col-12">
                <div class="page-title-box">
                    <h4 class="page-title">Work Manager Dashboard</h4>
                </div>
            </div>
        </div>

        <!-- Global Filters -->
        <div class="row mb-3">
            <div class="col-12">
                <div class="card">
                    <div class="card-body py-2">
                        <div class="row align-items-end">
                            <div class="col-md-4">
                                <label class="form-label fw-semibold mb-1">Branch</label>
                                <div class="dropdown" id="branchDropdown">
                                    <button class="form-select form-select-sm text-start" type="button" data-bs-toggle="dropdown" aria-expanded="false" id="btnBranchSelect">
                                        All Branches
                                    </button>
                                    <ul class="dropdown-menu w-100 p-2" style="max-height:200px;overflow-y:auto;" id="branchMenu">
                                        <asp:Repeater ID="rptBranches" runat="server">
                                            <ItemTemplate>
                                                <li>
                                                    <label class="dropdown-item py-1 px-2 d-flex align-items-center">
                                                        <input type="checkbox" class="form-check-input me-2 branch-chk" name="chkBranch" value='<%# Eval("BranchKey") %>' />
                                                        <%# Eval("Description") %>
                                                    </label>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                                <asp:HiddenField ID="hdnSelectedBranches" runat="server" Value="" />
                            </div>
                            <div class="col-md-4">
                                <label class="form-label fw-semibold mb-1">Team / User Group</label>
                                <asp:DropDownList ID="ddlUserGroup" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn btn-primary btn-sm w-100">
                                    <i class="mdi mdi-refresh me-1"></i>Refresh
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Chart data scripts -->
        <asp:Literal ID="litChartScripts" runat="server" />

        <!-- ROW 1: Task Summary (radialBar + progress bars) | Completed vs Total -->
        <div class="row mb-3">
            <div class="col-xl-6 col-lg-6">
                <div class="card card-h-100">
                    <div class="card-body">
                        <div class="d-flex align-items-center justify-content-between mb-3">
                            <h4 class="header-title">Task Summary</h4>
                            <div class="d-flex align-items-center">
                                <label class="form-label fw-semibold mb-0 me-2">Date Range</label>
                                <asp:DropDownList ID="ddlDateRange" runat="server" CssClass="form-select form-select-sm" style="width:auto;" AutoPostBack="true"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="row align-items-center">
                            <div class="col-md-6">
                                <div id="performance-chart"></div>
                            </div>
                            <div class="col-md-6">
                                <h5 class="mb-1 mt-0 fw-normal">Tasks Assigned <span class="fw-bold float-end"><asp:Literal ID="litAssignedCount" runat="server" Text="0" /></span></h5>
                                <div class="progress progress-sm mb-3">
                                    <div class="progress-bar" role="progressbar" style="width: 100%;" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100"></div>
                                </div>
                                <h5 class="mb-1 mt-0 fw-normal">Tasks Completed <span class="fw-bold float-end"><asp:Literal ID="litCompletedCount" runat="server" Text="0" /></span></h5>
                                <div class="progress progress-sm mb-3">
                                    <div class="progress-bar bg-success" role="progressbar" id="progCompleted" runat="server" style="width: 0%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
                                </div>
                                <h5 class="mb-1 mt-0 fw-normal">Tasks In Progress <span class="fw-bold float-end"><asp:Literal ID="litInProgressCount" runat="server" Text="0" /></span></h5>
                                <div class="progress progress-sm">
                                    <div class="progress-bar bg-warning" role="progressbar" id="progInProgress" runat="server" style="width: 0%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xl-6 col-lg-6">
                <div class="card card-h-100">
                    <div class="card-body">
                        <h4 class="header-title">Completed Tasks vs Total Tasks</h4>
                        <div id="completed-vs-total-chart" class="mt-3"></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- ROW 2: Weekly Trend + Tasks per User Group -->
        <div class="row mb-3">
            <div class="col-lg-8">
                <div class="card">
                    <div class="card-body">
                        <h4 class="header-title">Weekly Trend of Due Tasks</h4>
                        <div class="chart-content-bg">
                            <div class="row text-center">
                                <div class="col-sm-6">
                                    <p class="text-muted mb-0 mt-3">Current Week</p>
                                    <h2 class="fw-normal mb-3">
                                        <small class="mdi mdi-checkbox-blank-circle text-primary align-middle me-1"></small>
                                        <asp:Literal ID="litCurrentWeekTotal" runat="server" Text="0" />
                                    </h2>
                                </div>
                                <div class="col-sm-6">
                                    <p class="text-muted mb-0 mt-3">Previous Week</p>
                                    <h2 class="fw-normal mb-3">
                                        <small class="mdi mdi-checkbox-blank-circle text-success align-middle me-1"></small>
                                        <asp:Literal ID="litPreviousWeekTotal" runat="server" Text="0" />
                                    </h2>
                                </div>
                            </div>
                        </div>
                        <div class="dash-item-overlay d-none d-md-block">
                            <h5>Today's Due Tasks: <asp:Literal ID="litTodayDue" runat="server" Text="0" /></h5>
                        </div>
                        <div id="weekly-trend-chart" class="mt-3"></div>
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="card card-h-100">
                    <div class="card-body">
                        <h4 class="header-title">Tasks per User Group</h4>
                        <div id="tasks-by-group-chart" class="mb-3 mt-3"></div>
                        <div id="tasks-by-group-legend" class="chart-widget-list" style="max-height:120px;overflow-y:auto;">
                            <asp:Literal ID="litGroupLegend" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Bootstrap JS for dropdown -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <!-- ApexCharts CDN -->
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/dashboard/assets/js/pages/workManagerDashboard.js") %>'></script>
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            var menu = document.getElementById('branchMenu');
            if (!menu) return;
            var chks = menu.querySelectorAll('.branch-chk');
            var hdn = document.getElementById('<%= hdnSelectedBranches.ClientID %>');
            var btn = document.getElementById('btnBranchSelect');

            // Restore checked state from hidden field after postback
            var saved = hdn.value ? hdn.value.split(',') : [];
            if (saved.length > 0) {
                chks.forEach(function (c) {
                    if (saved.indexOf(c.value) >= 0) c.checked = true;
                });
                updateLabel();
            }

            // Prevent dropdown from closing on click inside
            menu.addEventListener('click', function (e) { e.stopPropagation(); });

            // Update hidden field and label on change
            chks.forEach(function (chk) {
                chk.addEventListener('change', function () {
                    updateLabel();
                });
            });

            function updateLabel() {
                var selected = [];
                var labels = [];
                chks.forEach(function (c) {
                    if (c.checked) { selected.push(c.value); labels.push(c.parentElement.textContent.trim()); }
                });
                hdn.value = selected.join(',');
                btn.textContent = labels.length === 0 ? 'All Branches' : (labels.length <= 2 ? labels.join(', ') : labels.length + ' selected');
            }
        });
    </script>
    </form>
</body>
</html>
