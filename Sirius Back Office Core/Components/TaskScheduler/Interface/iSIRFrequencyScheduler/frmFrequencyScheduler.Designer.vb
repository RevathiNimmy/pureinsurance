<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFrequencyScheduler
    Inherits System.Windows.Forms.Form
    Public Sub New()
        MyBase.New()
        InitializeComponent()
        InitializeoptOccurs()
        InitializeSecurityOptions()
        Form_Initialize_Renamed()

    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.fraPolicyFilter = New System.Windows.Forms.GroupBox()
        Me.chkExpireDate = New System.Windows.Forms.CheckBox()
        Me.dateTimePickerExpireTime = New System.Windows.Forms.DateTimePicker()
        Me.lblExpiresDate = New System.Windows.Forms.Label()
        Me.dateTimePickerExpireDate = New System.Windows.Forms.DateTimePicker()
        Me.chkEnabled = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkListBoxMonthlyWeekDay = New System.Windows.Forms.CheckedListBox()
        Me.rdMonthDaysOfWeek = New System.Windows.Forms.RadioButton()
        Me.rdMonthsDays = New System.Windows.Forms.RadioButton()
        Me.chkListBoxMonthlyWeekNumber = New System.Windows.Forms.CheckedListBox()
        Me.lblMonths = New System.Windows.Forms.Label()
        Me.checkedListBoxMonthlyDays = New System.Windows.Forms.CheckedListBox()
        Me.checkedListBoxMonthlyMonths = New System.Windows.Forms.CheckedListBox()
        Me.lblRecurWeeksOn = New System.Windows.Forms.Label()
        Me.chkListBoxWeeklyDays = New System.Windows.Forms.CheckedListBox()
        Me.lblDays = New System.Windows.Forms.Label()
        Me.lblDailyRecurDays = New System.Windows.Forms.Label()
        Me.txtDaysDaily = New System.Windows.Forms.NumericUpDown()
        Me.PnlSecurity = New System.Windows.Forms.Panel()
        Me.rdLoggedOnSecurityOption = New System.Windows.Forms.RadioButton()
        Me.rdLoggedOnorNotSecurityOption = New System.Windows.Forms.RadioButton()
        Me.dateTimePickerTriggerTime = New System.Windows.Forms.DateTimePicker()
        Me.labelStartDate = New System.Windows.Forms.Label()
        Me.dateTimePickerStartDate = New System.Windows.Forms.DateTimePicker()
        Me.rdMonthlyFrequency = New System.Windows.Forms.RadioButton()
        Me.rdWeeklyFrequency = New System.Windows.Forms.RadioButton()
        Me.rdDailyFrequency = New System.Windows.Forms.RadioButton()
        Me.rdOneTimeFrequency = New System.Windows.Forms.RadioButton()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.fraPolicyFilter.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.txtDaysDaily, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlSecurity.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraPolicyFilter
        '
        Me.fraPolicyFilter.BackColor = System.Drawing.SystemColors.Control
        Me.fraPolicyFilter.Controls.Add(Me.chkExpireDate)
        Me.fraPolicyFilter.Controls.Add(Me.dateTimePickerExpireTime)
        Me.fraPolicyFilter.Controls.Add(Me.lblExpiresDate)
        Me.fraPolicyFilter.Controls.Add(Me.dateTimePickerExpireDate)
        Me.fraPolicyFilter.Controls.Add(Me.chkEnabled)
        Me.fraPolicyFilter.Controls.Add(Me.Panel1)
        Me.fraPolicyFilter.Controls.Add(Me.PnlSecurity)
        Me.fraPolicyFilter.Controls.Add(Me.dateTimePickerTriggerTime)
        Me.fraPolicyFilter.Controls.Add(Me.labelStartDate)
        Me.fraPolicyFilter.Controls.Add(Me.dateTimePickerStartDate)
        Me.fraPolicyFilter.Controls.Add(Me.rdMonthlyFrequency)
        Me.fraPolicyFilter.Controls.Add(Me.rdWeeklyFrequency)
        Me.fraPolicyFilter.Controls.Add(Me.rdDailyFrequency)
        Me.fraPolicyFilter.Controls.Add(Me.rdOneTimeFrequency)
        Me.fraPolicyFilter.Controls.Add(Me.Splitter1)
        Me.fraPolicyFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraPolicyFilter.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraPolicyFilter.Location = New System.Drawing.Point(12, 12)
        Me.fraPolicyFilter.Name = "fraPolicyFilter"
        Me.fraPolicyFilter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraPolicyFilter.Size = New System.Drawing.Size(807, 480)
        Me.fraPolicyFilter.TabIndex = 7
        Me.fraPolicyFilter.TabStop = False
        Me.fraPolicyFilter.Text = "Settings"
        '
        'chkExpireDate
        '
        Me.chkExpireDate.AutoSize = True
        Me.chkExpireDate.Location = New System.Drawing.Point(164, 410)
        Me.chkExpireDate.Name = "chkExpireDate"
        Me.chkExpireDate.Size = New System.Drawing.Size(15, 14)
        Me.chkExpireDate.TabIndex = 44
        Me.chkExpireDate.UseVisualStyleBackColor = True
        '
        'dateTimePickerExpireTime
        '
        Me.dateTimePickerExpireTime.CustomFormat = ""
        Me.dateTimePickerExpireTime.Enabled = False
        Me.dateTimePickerExpireTime.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dateTimePickerExpireTime.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dateTimePickerExpireTime.Location = New System.Drawing.Point(470, 410)
        Me.dateTimePickerExpireTime.Name = "dateTimePickerExpireTime"
        Me.dateTimePickerExpireTime.ShowUpDown = True
        Me.dateTimePickerExpireTime.Size = New System.Drawing.Size(96, 20)
        Me.dateTimePickerExpireTime.TabIndex = 43
        '
        'lblExpiresDate
        '
        Me.lblExpiresDate.AutoSize = True
        Me.lblExpiresDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpiresDate.Location = New System.Drawing.Point(178, 410)
        Me.lblExpiresDate.Name = "lblExpiresDate"
        Me.lblExpiresDate.Size = New System.Drawing.Size(54, 13)
        Me.lblExpiresDate.TabIndex = 42
        Me.lblExpiresDate.Text = "Expires:"
        '
        'dateTimePickerExpireDate
        '
        Me.dateTimePickerExpireDate.Enabled = False
        Me.dateTimePickerExpireDate.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dateTimePickerExpireDate.Location = New System.Drawing.Point(235, 410)
        Me.dateTimePickerExpireDate.Name = "dateTimePickerExpireDate"
        Me.dateTimePickerExpireDate.Size = New System.Drawing.Size(230, 20)
        Me.dateTimePickerExpireDate.TabIndex = 41
        '
        'chkEnabled
        '
        Me.chkEnabled.AutoSize = True
        Me.chkEnabled.Checked = True
        Me.chkEnabled.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkEnabled.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.chkEnabled.Location = New System.Drawing.Point(562, 36)
        Me.chkEnabled.Name = "chkEnabled"
        Me.chkEnabled.Size = New System.Drawing.Size(71, 17)
        Me.chkEnabled.TabIndex = 40
        Me.chkEnabled.Text = "Enabled"
        Me.chkEnabled.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.chkListBoxMonthlyWeekDay)
        Me.Panel1.Controls.Add(Me.rdMonthDaysOfWeek)
        Me.Panel1.Controls.Add(Me.rdMonthsDays)
        Me.Panel1.Controls.Add(Me.chkListBoxMonthlyWeekNumber)
        Me.Panel1.Controls.Add(Me.lblMonths)
        Me.Panel1.Controls.Add(Me.checkedListBoxMonthlyDays)
        Me.Panel1.Controls.Add(Me.checkedListBoxMonthlyMonths)
        Me.Panel1.Controls.Add(Me.lblRecurWeeksOn)
        Me.Panel1.Controls.Add(Me.chkListBoxWeeklyDays)
        Me.Panel1.Controls.Add(Me.lblDays)
        Me.Panel1.Controls.Add(Me.lblDailyRecurDays)
        Me.Panel1.Controls.Add(Me.txtDaysDaily)
        Me.Panel1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Panel1.Location = New System.Drawing.Point(164, 66)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(623, 338)
        Me.Panel1.TabIndex = 39
        '
        'chkListBoxMonthlyWeekDay
        '
        Me.chkListBoxMonthlyWeekDay.CheckOnClick = True
        Me.chkListBoxMonthlyWeekDay.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.chkListBoxMonthlyWeekDay.FormattingEnabled = True
        Me.chkListBoxMonthlyWeekDay.HorizontalExtent = 2
        Me.chkListBoxMonthlyWeekDay.Items.AddRange(New Object() {"ALL", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"})
        Me.chkListBoxMonthlyWeekDay.Location = New System.Drawing.Point(204, 252)
        Me.chkListBoxMonthlyWeekDay.MultiColumn = True
        Me.chkListBoxMonthlyWeekDay.Name = "chkListBoxMonthlyWeekDay"
        Me.chkListBoxMonthlyWeekDay.Size = New System.Drawing.Size(360, 68)
        Me.chkListBoxMonthlyWeekDay.TabIndex = 34
        '
        'rdMonthDaysOfWeek
        '
        Me.rdMonthDaysOfWeek.AutoSize = True
        Me.rdMonthDaysOfWeek.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.rdMonthDaysOfWeek.Location = New System.Drawing.Point(6, 253)
        Me.rdMonthDaysOfWeek.Name = "rdMonthDaysOfWeek"
        Me.rdMonthDaysOfWeek.Size = New System.Drawing.Size(46, 17)
        Me.rdMonthDaysOfWeek.TabIndex = 53
        Me.rdMonthDaysOfWeek.TabStop = True
        Me.rdMonthDaysOfWeek.Text = "On:"
        Me.rdMonthDaysOfWeek.UseVisualStyleBackColor = True
        '
        'rdMonthsDays
        '
        Me.rdMonthsDays.AutoSize = True
        Me.rdMonthsDays.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.rdMonthsDays.Location = New System.Drawing.Point(6, 110)
        Me.rdMonthsDays.Name = "rdMonthsDays"
        Me.rdMonthsDays.Size = New System.Drawing.Size(59, 17)
        Me.rdMonthsDays.TabIndex = 52
        Me.rdMonthsDays.TabStop = True
        Me.rdMonthsDays.Text = "Days:"
        Me.rdMonthsDays.UseVisualStyleBackColor = True
        '
        'chkListBoxMonthlyWeekNumber
        '
        Me.chkListBoxMonthlyWeekNumber.CheckOnClick = True
        Me.chkListBoxMonthlyWeekNumber.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.chkListBoxMonthlyWeekNumber.FormattingEnabled = True
        Me.chkListBoxMonthlyWeekNumber.HorizontalExtent = 2
        Me.chkListBoxMonthlyWeekNumber.Items.AddRange(New Object() {"First", "Second", "Third", "Fourth", "Last"})
        Me.chkListBoxMonthlyWeekNumber.Location = New System.Drawing.Point(64, 250)
        Me.chkListBoxMonthlyWeekNumber.Name = "chkListBoxMonthlyWeekNumber"
        Me.chkListBoxMonthlyWeekNumber.Size = New System.Drawing.Size(125, 84)
        Me.chkListBoxMonthlyWeekNumber.TabIndex = 33
        '
        'lblMonths
        '
        Me.lblMonths.AutoSize = True
        Me.lblMonths.Font = New System.Drawing.Font("Verdana", 8.26!)
        Me.lblMonths.Location = New System.Drawing.Point(3, 17)
        Me.lblMonths.Name = "lblMonths"
        Me.lblMonths.Size = New System.Drawing.Size(58, 14)
        Me.lblMonths.TabIndex = 50
        Me.lblMonths.Text = "Months:"
        '
        'checkedListBoxMonthlyDays
        '
        Me.checkedListBoxMonthlyDays.CheckOnClick = True
        Me.checkedListBoxMonthlyDays.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.checkedListBoxMonthlyDays.FormattingEnabled = True
        Me.checkedListBoxMonthlyDays.HorizontalScrollbar = True
        Me.checkedListBoxMonthlyDays.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "Last Day"})
        Me.checkedListBoxMonthlyDays.Location = New System.Drawing.Point(64, 92)
        Me.checkedListBoxMonthlyDays.MultiColumn = True
        Me.checkedListBoxMonthlyDays.Name = "checkedListBoxMonthlyDays"
        Me.checkedListBoxMonthlyDays.Size = New System.Drawing.Size(499, 157)
        Me.checkedListBoxMonthlyDays.TabIndex = 29
        '
        'checkedListBoxMonthlyMonths
        '
        Me.checkedListBoxMonthlyMonths.CheckOnClick = True
        Me.checkedListBoxMonthlyMonths.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.checkedListBoxMonthlyMonths.FormattingEnabled = True
        Me.checkedListBoxMonthlyMonths.HorizontalScrollbar = True
        Me.checkedListBoxMonthlyMonths.Items.AddRange(New Object() {"ALL", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"})
        Me.checkedListBoxMonthlyMonths.Location = New System.Drawing.Point(65, 16)
        Me.checkedListBoxMonthlyMonths.MultiColumn = True
        Me.checkedListBoxMonthlyMonths.Name = "checkedListBoxMonthlyMonths"
        Me.checkedListBoxMonthlyMonths.Size = New System.Drawing.Size(499, 68)
        Me.checkedListBoxMonthlyMonths.TabIndex = 49
        '
        'lblRecurWeeksOn
        '
        Me.lblRecurWeeksOn.AutoSize = True
        Me.lblRecurWeeksOn.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecurWeeksOn.Location = New System.Drawing.Point(164, 17)
        Me.lblRecurWeeksOn.Name = "lblRecurWeeksOn"
        Me.lblRecurWeeksOn.Size = New System.Drawing.Size(66, 13)
        Me.lblRecurWeeksOn.TabIndex = 46
        Me.lblRecurWeeksOn.Text = "weeks on:"
        '
        'chkListBoxWeeklyDays
        '
        Me.chkListBoxWeeklyDays.CheckOnClick = True
        Me.chkListBoxWeeklyDays.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkListBoxWeeklyDays.FormattingEnabled = True
        Me.chkListBoxWeeklyDays.Items.AddRange(New Object() {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"})
        Me.chkListBoxWeeklyDays.Location = New System.Drawing.Point(35, 54)
        Me.chkListBoxWeeklyDays.MultiColumn = True
        Me.chkListBoxWeeklyDays.Name = "chkListBoxWeeklyDays"
        Me.chkListBoxWeeklyDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkListBoxWeeklyDays.Size = New System.Drawing.Size(412, 116)
        Me.chkListBoxWeeklyDays.TabIndex = 45
        '
        'lblDays
        '
        Me.lblDays.AutoSize = True
        Me.lblDays.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDays.Location = New System.Drawing.Point(163, 17)
        Me.lblDays.Name = "lblDays"
        Me.lblDays.Size = New System.Drawing.Size(36, 13)
        Me.lblDays.TabIndex = 44
        Me.lblDays.Text = "Days"
        '
        'lblDailyRecurDays
        '
        Me.lblDailyRecurDays.AutoSize = True
        Me.lblDailyRecurDays.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDailyRecurDays.Location = New System.Drawing.Point(32, 17)
        Me.lblDailyRecurDays.Name = "lblDailyRecurDays"
        Me.lblDailyRecurDays.Size = New System.Drawing.Size(82, 13)
        Me.lblDailyRecurDays.TabIndex = 43
        Me.lblDailyRecurDays.Text = "Recur every:"
        '
        'txtDaysDaily
        '
        Me.txtDaysDaily.Location = New System.Drawing.Point(110, 17)
        Me.txtDaysDaily.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.txtDaysDaily.Name = "txtDaysDaily"
        Me.txtDaysDaily.Size = New System.Drawing.Size(49, 21)
        Me.txtDaysDaily.TabIndex = 1
        Me.txtDaysDaily.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PnlSecurity
        '
        Me.PnlSecurity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PnlSecurity.Controls.Add(Me.rdLoggedOnSecurityOption)
        Me.PnlSecurity.Controls.Add(Me.rdLoggedOnorNotSecurityOption)
        Me.PnlSecurity.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PnlSecurity.ForeColor = System.Drawing.SystemColors.ControlText
        Me.PnlSecurity.Location = New System.Drawing.Point(164, 436)
        Me.PnlSecurity.Name = "PnlSecurity"
        Me.PnlSecurity.Size = New System.Drawing.Size(623, 40)
        Me.PnlSecurity.TabIndex = 39
        '
        'rdLoggedOnSecurityOption
        '
        Me.rdLoggedOnSecurityOption.AutoSize = True
        Me.rdLoggedOnSecurityOption.Checked = True
        Me.rdLoggedOnSecurityOption.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.rdLoggedOnSecurityOption.Location = New System.Drawing.Point(5, 6)
        Me.rdLoggedOnSecurityOption.Name = "rdLoggedOnSecurityOption"
        Me.rdLoggedOnSecurityOption.Size = New System.Drawing.Size(199, 17)
        Me.rdLoggedOnSecurityOption.TabIndex = 53
        Me.rdLoggedOnSecurityOption.TabStop = True
        Me.rdLoggedOnSecurityOption.Text = "Run whether user is logged on"
        Me.rdLoggedOnSecurityOption.UseVisualStyleBackColor = True
        '
        'rdLoggedOnorNotSecurityOption
        '
        Me.rdLoggedOnorNotSecurityOption.AutoSize = True
        Me.rdLoggedOnorNotSecurityOption.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.rdLoggedOnorNotSecurityOption.Location = New System.Drawing.Point(238, 7)
        Me.rdLoggedOnorNotSecurityOption.Name = "rdLoggedOnorNotSecurityOption"
        Me.rdLoggedOnorNotSecurityOption.Size = New System.Drawing.Size(237, 17)
        Me.rdLoggedOnorNotSecurityOption.TabIndex = 54
        Me.rdLoggedOnorNotSecurityOption.Text = "Run whether user is logged on or not"
        '
        'dateTimePickerTriggerTime
        '
        Me.dateTimePickerTriggerTime.CustomFormat = ""
        Me.dateTimePickerTriggerTime.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dateTimePickerTriggerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dateTimePickerTriggerTime.Location = New System.Drawing.Point(451, 34)
        Me.dateTimePickerTriggerTime.Name = "dateTimePickerTriggerTime"
        Me.dateTimePickerTriggerTime.ShowUpDown = True
        Me.dateTimePickerTriggerTime.Size = New System.Drawing.Size(100, 20)
        Me.dateTimePickerTriggerTime.TabIndex = 38
        '
        'labelStartDate
        '
        Me.labelStartDate.AutoSize = True
        Me.labelStartDate.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.labelStartDate.Location = New System.Drawing.Point(175, 38)
        Me.labelStartDate.Name = "labelStartDate"
        Me.labelStartDate.Size = New System.Drawing.Size(40, 13)
        Me.labelStartDate.TabIndex = 37
        Me.labelStartDate.Text = "Start:"
        '
        'dateTimePickerStartDate
        '
        Me.dateTimePickerStartDate.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dateTimePickerStartDate.Location = New System.Drawing.Point(215, 34)
        Me.dateTimePickerStartDate.Name = "dateTimePickerStartDate"
        Me.dateTimePickerStartDate.Size = New System.Drawing.Size(230, 20)
        Me.dateTimePickerStartDate.TabIndex = 5
        '
        'rdMonthlyFrequency
        '
        Me.rdMonthlyFrequency.AutoSize = True
        Me.rdMonthlyFrequency.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.rdMonthlyFrequency.Location = New System.Drawing.Point(16, 104)
        Me.rdMonthlyFrequency.Name = "rdMonthlyFrequency"
        Me.rdMonthlyFrequency.Size = New System.Drawing.Size(74, 18)
        Me.rdMonthlyFrequency.TabIndex = 4
        Me.rdMonthlyFrequency.TabStop = True
        Me.rdMonthlyFrequency.Text = "Monthly"
        Me.rdMonthlyFrequency.UseVisualStyleBackColor = True
        '
        'rdWeeklyFrequency
        '
        Me.rdWeeklyFrequency.AutoSize = True
        Me.rdWeeklyFrequency.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.rdWeeklyFrequency.Location = New System.Drawing.Point(16, 83)
        Me.rdWeeklyFrequency.Name = "rdWeeklyFrequency"
        Me.rdWeeklyFrequency.Size = New System.Drawing.Size(70, 18)
        Me.rdWeeklyFrequency.TabIndex = 3
        Me.rdWeeklyFrequency.TabStop = True
        Me.rdWeeklyFrequency.Text = "Weekly"
        Me.rdWeeklyFrequency.UseVisualStyleBackColor = True
        '
        'rdDailyFrequency
        '
        Me.rdDailyFrequency.AutoSize = True
        Me.rdDailyFrequency.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.rdDailyFrequency.Location = New System.Drawing.Point(16, 61)
        Me.rdDailyFrequency.Name = "rdDailyFrequency"
        Me.rdDailyFrequency.Size = New System.Drawing.Size(55, 18)
        Me.rdDailyFrequency.TabIndex = 2
        Me.rdDailyFrequency.TabStop = True
        Me.rdDailyFrequency.Text = "Daily"
        Me.rdDailyFrequency.UseVisualStyleBackColor = True
        '
        'rdOneTimeFrequency
        '
        Me.rdOneTimeFrequency.AutoSize = True
        Me.rdOneTimeFrequency.Checked = True
        Me.rdOneTimeFrequency.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.rdOneTimeFrequency.Location = New System.Drawing.Point(16, 38)
        Me.rdOneTimeFrequency.Name = "rdOneTimeFrequency"
        Me.rdOneTimeFrequency.Size = New System.Drawing.Size(84, 18)
        Me.rdOneTimeFrequency.TabIndex = 1
        Me.rdOneTimeFrequency.TabStop = True
        Me.rdOneTimeFrequency.Text = "One Time"
        Me.rdOneTimeFrequency.UseVisualStyleBackColor = True
        '
        'Splitter1
        '
        Me.Splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Splitter1.Location = New System.Drawing.Point(3, 16)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(142, 461)
        Me.Splitter1.TabIndex = 0
        Me.Splitter1.TabStop = False
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(678, 498)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(73, 22)
        Me.cmdCancel.TabIndex = 42
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOK
        '
        Me.cmdOK.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOK.Font = New System.Drawing.Font("Verdana", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOK.Location = New System.Drawing.Point(578, 498)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOK.Size = New System.Drawing.Size(73, 22)
        Me.cmdOK.TabIndex = 41
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'frmFrequencyScheduler
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(831, 529)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.fraPolicyFilter)
        Me.Controls.Add(Me.cmdOK)
        Me.MaximizeBox = False
        Me.Name = "frmFrequencyScheduler"
        Me.Text = "BatchJob Scheduler Configuration"
        Me.fraPolicyFilter.ResumeLayout(False)
        Me.fraPolicyFilter.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.txtDaysDaily, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlSecurity.ResumeLayout(False)
        Me.PnlSecurity.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Sub InitializeoptOccurs()
        Me.rdFrequency(0) = rdOneTimeFrequency
        Me.rdFrequency(1) = rdDailyFrequency
        Me.rdFrequency(2) = rdWeeklyFrequency
        Me.rdFrequency(3) = rdMonthlyFrequency
    End Sub


    Sub InitializeSecurityOptions()
        Me.rdSecurityOptions(0) = rdLoggedOnSecurityOption
        Me.rdSecurityOptions(1) = rdLoggedOnorNotSecurityOption

    End Sub
    Public WithEvents fraPolicyFilter As GroupBox
    Friend WithEvents rdMonthlyFrequency As RadioButton
    Friend WithEvents rdWeeklyFrequency As RadioButton
    Friend WithEvents rdDailyFrequency As RadioButton
    Friend WithEvents rdOneTimeFrequency As RadioButton
    Public rdFrequency(3) As System.Windows.Forms.RadioButton
    Public rdSecurityOptions(1) As System.Windows.Forms.RadioButton
    Friend WithEvents Splitter1 As Splitter
    Friend WithEvents chkEnabled As CheckBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents PnlSecurity As Panel
    Private WithEvents dateTimePickerTriggerTime As DateTimePicker
    Private WithEvents labelStartDate As Label
    Private WithEvents dateTimePickerStartDate As DateTimePicker
    Public WithEvents cmdCancel As Button
    Public WithEvents cmdOK As Button
    Private WithEvents lblDays As Label
    Private WithEvents lblDailyRecurDays As Label
    Private WithEvents txtDaysDaily As NumericUpDown
    Friend WithEvents chkListBoxWeeklyDays As CheckedListBox
    Private WithEvents lblRecurWeeksOn As Label
    Private WithEvents checkedListBoxMonthlyDays As CheckedListBox
    Friend WithEvents chkExpireDate As CheckBox
    Private WithEvents dateTimePickerExpireTime As DateTimePicker
    Private WithEvents lblExpiresDate As Label
    Private WithEvents dateTimePickerExpireDate As DateTimePicker
    Private WithEvents checkedListBoxMonthlyMonths As CheckedListBox
    Private WithEvents lblMonths As Label
    Private WithEvents chkListBoxMonthlyWeekDay As CheckedListBox
    Friend WithEvents rdMonthDaysOfWeek As RadioButton
    Friend WithEvents rdMonthsDays As RadioButton
    Private WithEvents chkListBoxMonthlyWeekNumber As CheckedListBox
    Friend WithEvents rdLoggedOnSecurityOption As RadioButton
    Friend WithEvents rdLoggedOnorNotSecurityOption As RadioButton
End Class
