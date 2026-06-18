// Work Manager Dashboard - ApexCharts initialisation
var WorkManagerDashboard = {
    colors: ['#727cf5', '#0acf97', '#fa5c7c', '#ffbc00', '#39afd1', '#e3eaef', '#6c757d', '#fd7e14'],
    charts: {},

    init: function () {
        if (typeof perfData !== 'undefined') this.renderPerformanceChart();
        if (typeof trendData !== 'undefined') this.renderWeeklyTrendChart();
        if (typeof groupData !== 'undefined') this.renderTasksByGroupChart();
        if (typeof monthlyData !== 'undefined') this.renderCompletedVsTotalChart();
    },

    destroyChart: function (key) {
        if (this.charts[key]) {
            try { this.charts[key].destroy(); } catch (e) { }
            this.charts[key] = null;
        }
    },

    renderPerformanceChart: function () {
        var el = document.querySelector('#performance-chart');
        if (!el || typeof perfData === 'undefined') return;
        this.destroyChart('perf');
        el.innerHTML = '';
        var percent = perfData.percent;
        if (percent === 0) percent = 0.1;
        var options = {
            chart: { height: 250, type: 'radialBar' },
            plotOptions: {
                radialBar: {
                    hollow: { size: '65%' },
                    dataLabels: {
                        name: { fontSize: '16px' },
                        value: { fontSize: '22px', formatter: function () { return perfData.percent + '%'; } }
                    }
                }
            },
            colors: ['#727cf5'],
            series: [percent],
            labels: ['Completed']
        };
        this.charts['perf'] = new ApexCharts(el, options);
        this.charts['perf'].render();
    },

    renderWeeklyTrendChart: function () {
        var el = document.querySelector('#weekly-trend-chart');
        if (!el || typeof trendData === 'undefined') return;
        this.destroyChart('trend');
        el.innerHTML = '';
        if (!trendData.categories || trendData.categories.length === 0) {
            el.innerHTML = '<p class="text-muted text-center mt-5">No data to display</p>';
            return;
        }
        var options = {
            chart: { height: 280, type: 'area', toolbar: { show: false } },
            dataLabels: { enabled: false },
            stroke: { width: 2, curve: 'smooth' },
            series: [
                { name: 'Current Week', data: trendData.currentWeek },
                { name: 'Previous Week', data: trendData.previousWeek }
            ],
            colors: ['#727cf5', '#0acf97'],
            xaxis: { categories: trendData.categories },
            tooltip: { shared: true },
            fill: { type: 'gradient', gradient: { shadeIntensity: 1, opacityFrom: 0.5, opacityTo: 0.1 } }
        };
        this.charts['trend'] = new ApexCharts(el, options);
        this.charts['trend'].render();
    },

    renderTasksByGroupChart: function () {
        var el = document.querySelector('#tasks-by-group-chart');
        if (!el || typeof groupData === 'undefined') return;
        this.destroyChart('group');
        el.innerHTML = '';
        if (!groupData.series || groupData.series.length === 0) {
            el.innerHTML = '<p class="text-muted text-center mt-5">No data to display</p>';
            return;
        }
        var options = {
            chart: { height: 260, type: 'donut' },
            series: groupData.series,
            labels: groupData.labels,
            colors: this.colors.slice(0, groupData.labels.length),
            legend: { show: false },
            plotOptions: { pie: { donut: { size: '70%' } } },
            tooltip: {
                y: { formatter: function (val) { return val + ' tasks'; } }
            }
        };
        this.charts['group'] = new ApexCharts(el, options);
        this.charts['group'].render();
    },

    renderCompletedVsTotalChart: function () {
        var el = document.querySelector('#completed-vs-total-chart');
        if (!el || typeof monthlyData === 'undefined') return;
        this.destroyChart('monthly');
        el.innerHTML = '';
        if (!monthlyData.categories || monthlyData.categories.length === 0) {
            el.innerHTML = '<p class="text-muted text-center mt-5">No data to display</p>';
            return;
        }
        var options = {
            chart: { height: 300, type: 'bar', toolbar: { show: false } },
            plotOptions: { bar: { horizontal: false, columnWidth: '50%', borderRadius: 4 } },
            dataLabels: { enabled: false },
            series: [
                { name: 'Total Tasks', data: monthlyData.totalTasks },
                { name: 'Completed Tasks', data: monthlyData.completedTasks }
            ],
            colors: ['#e3eaef', '#727cf5'],
            xaxis: { categories: monthlyData.categories },
            legend: { position: 'top' },
            tooltip: { shared: true, intersect: false }
        };
        this.charts['monthly'] = new ApexCharts(el, options);
        this.charts['monthly'].render();
    }
};

// Initialise on page load and after async postback (UpdatePanel refresh)
document.addEventListener('DOMContentLoaded', function () { WorkManagerDashboard.init(); });
if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { WorkManagerDashboard.init(); });
}
