
Imports System.Configuration.ConfigurationManager
Imports System.Threading.Thread

Namespace Nexus

    Partial Class CalendarLookup
        Inherits System.Web.UI.UserControl

        ''' <summary>
        ''' Variables are defined as Protected, Because they can be accessed in Java script.
        ''' </summary>
        ''' <remarks></remarks>
        Protected m_sLinkedControl As String
        Protected m_sAdditionalLinkedControl As String
        Protected m_sMonthsDifference As String
        Protected bEnabled As Boolean = True
        Protected sFirstDate As String
        Protected sLastDate As String
        Protected m_sLinkedControlClientId As String
        Protected m_sDateFormat As String
        Protected m_sIconPath As String
        Protected dFirstDate As String
        Protected dLastDate As String

        Dim iHLevel As Integer
        Dim sHLevel As String
        Dim iFirstYear As Integer
        Dim iLastYear As Integer

        ''' <summary>
        ''' Enabled/Desabled calendar control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Enabled() As Boolean
            Set(ByVal value As Boolean)
                bEnabled = value
            End Set
            Get
                Return bEnabled
            End Get
        End Property
        ''' <summary>
        ''' First year displayed/accepted by calendar control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FirstYear() As Integer
            Get
                Return iFirstYear
            End Get
            Set(ByVal value As Integer)
                iFirstYear = value
                If iFirstYear < 2000 Then
                    dFirstDate = ""
                Else
                    dFirstDate = CType("1/1/" & iFirstYear.ToString(), DateTime).ToString(CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.Replace("yyyy", "yy"))
                End If

            End Set
        End Property
        ''' <summary>
        ''' Last year displayed/accepted by calendar control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastYear() As Integer
            Get
                Return iLastYear
            End Get
            Set(ByVal value As Integer)
                iLastYear = value
                If iLastYear < 2000 Then
                    dLastDate = ""
                Else
                    dLastDate = FormatDateTime("31/12/" & iFirstYear.ToString(), DateFormat.ShortDate)
                End If

            End Set
        End Property
        ''' <summary>
        ''' Hierarchy level of control in page
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property HLevel() As Integer
            Get
                Return iHLevel
            End Get
            Set(ByVal value As Integer)
                iHLevel = value
            End Set
        End Property
        ''' <summary>
        ''' calendar control's Linked control name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LinkedControl() As String
            Get
                Return m_sLinkedControl
            End Get
            Set(ByVal Value As String)
                m_sLinkedControl = Value

            End Set
        End Property

        ''' <summary>
        ''' Obsolete Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Obsolete("This property is obsolete")> _
        Public Property AdditionalLinkedControl() As String
            Get
                Return m_sAdditionalLinkedControl
            End Get
            Set(ByVal Value As String)
                m_sAdditionalLinkedControl = Value
            End Set
        End Property

        ''' <summary>
        ''' Obsolete Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Obsolete("This property is obsolete")> _
        Public Property MonthsDifference() As Integer
            Get
                Return m_sMonthsDifference
            End Get
            Set(ByVal Value As Integer)
                m_sMonthsDifference = Value
            End Set
        End Property

        ''' <summary>
        ''' Set different calendar propertis
        ''' 1. Calendar Image Path
        ''' 2. Linked Control Hierarchy
        ''' 3. Initializing/set First Year
        ''' 4. Initializing/Set Last Year
        ''' 5. Register StartupScript to initlize linked control associted with calender control.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            '' m_sIconPath = ResolveClientUrl(imgCalendar.ImageUrl.Replace("~", ".."))
            Enabled = bEnabled
            SetHLevel(iHLevel)
            SetDateFormat()
            m_sLinkedControlClientId = sHLevel & "_" & m_sLinkedControl
            btnCalendar.ID = m_sLinkedControl & "_" & btnCalendar.ID
            lblCalenderIcon.Attributes.Add("for", m_sLinkedControlClientId)
            ' Declare the variable to set the year range of control
            Dim sYearRange As String = String.Empty
            ' declare the variable to store the first year of year range
            Dim iYearRangeFirstYear As Integer
            ' declare the variable to store the last year of year range
            Dim iYearRangeLastYear As Integer

            If iFirstYear = 0 Then
                dFirstDate = ""
                'set the value of iYearRangeFirstYear to 100 when Ifirst year is 0 . this the default value
                iYearRangeFirstYear = 100
            Else
                If iFirstYear < 2000 Then
                    dFirstDate = ""
                Else
                    dFirstDate = CType("1/1/" & iFirstYear.ToString(), DateTime).ToString(CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.Replace("yyyy", "yy"))
                End If
                'calculate the year range first year by subtracting the first year vale from current year 
                iYearRangeFirstYear = (DateTime.Now.Year - iFirstYear)
            End If

            If iLastYear = 0 Then
                dLastDate = ""
                'set the value of iYearRangeLastYear to 10 when Last year is 0 . this the default value
                iYearRangeLastYear = 10
            Else
                If iLastYear < 2000 Then
                    dLastDate = ""
                Else
                    dLastDate = CType("31/12/" & DateTime.Now.Year.ToString(), DateTime).ToString(CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.Replace("yyyy", "yy"))
                End If
                'calculate the year range first year by subtracting the current year vale from last year
                iYearRangeLastYear = (iLastYear - DateTime.Now.Year)
            End If

            If iYearRangeFirstYear >= 0 Then
                ' if iYearRangeFirstYear is positive then append the "-"
                sYearRange = "-" + iYearRangeFirstYear.ToString
            Else
                ' if iYearRangeFirstYear is negative then append the "+"
                sYearRange = "+" + Math.Abs(iYearRangeFirstYear).ToString
            End If

            sYearRange = sYearRange + ":"

            If iYearRangeLastYear >= 0 Then
                ' if iYearRangeLastYear is positive then append the "+"
                sYearRange = sYearRange + "+" + iYearRangeLastYear.ToString
            Else
                ' if iYearRangeLastYear is negative then append nothing as its already contains the correct sign.
                sYearRange = sYearRange + iYearRangeLastYear.ToString
            End If

            Dim sLoadCalenderScript As StringBuilder = New StringBuilder
            Dim RandomClass As New Random()
            Dim RandomNumber As Integer

            RandomNumber = RandomClass.Next()
            'On page load initialize calendar linked control.
            sLoadCalenderScript.Append("$('#" & lblCalenderIcon.ClientID & "').datepicker({")
            sLoadCalenderScript.Append("changeMonth: true,")
            sLoadCalenderScript.Append("changeYear: true,")
            sLoadCalenderScript.Append("showOn: ""button"",")
            sLoadCalenderScript.Append("buttonImage: """ & m_sIconPath & """,")
            sLoadCalenderScript.Append("buttonImageOnly: true,")
            sLoadCalenderScript.Append("showOtherMonths: true,")
            sLoadCalenderScript.Append("selectOtherMonths: true,")
            sLoadCalenderScript.Append("todayHighlight: true,")
            sLoadCalenderScript.Append("format: """ & m_sDateFormat & """,")
            sLoadCalenderScript.Append("buttonText: ""Select Date"",")
            'set the year range in yearRange parameter of script
            sLoadCalenderScript.Append("yearRange: """ & sYearRange & """,")

            If dFirstDate.Trim() = "" Or iYearRangeFirstYear <> 0 Then
                sLoadCalenderScript.Append("minDate: null,")
            Else
                sLoadCalenderScript.Append("minDate: '" & dFirstDate & "',")
            End If
            If dLastDate.Trim() = "" Or iYearRangeLastYear <> 0 Then
                sLoadCalenderScript.Append("maxDate: null")
            Else
                sLoadCalenderScript.Append("maxDate: '" & dLastDate & "'")
            End If

            sLoadCalenderScript.Append("})")
            sLoadCalenderScript.Append(".on('changeDate', function(ev){  $('#" & m_sLinkedControlClientId & "').val($('#" & lblCalenderIcon.ClientID & "').data('datepicker').getFormattedDate('" + m_sDateFormat + "'));$('#" & lblCalenderIcon.ClientID & "').datepicker('hide');$('#" & m_sLinkedControlClientId & "').focus(); $('#" & m_sLinkedControlClientId & "').change();$('#" & m_sLinkedControlClientId & "').blur(); $('#" & m_sLinkedControlClientId & "').next('.pbdatepicker').first().focus(); $('#" & m_sLinkedControlClientId & "').next('.pbdatepicker').first().blur();  });")
            sLoadCalenderScript.Append("$('#" & m_sLinkedControlClientId & "').on('change',function(ev){ $('#" & lblCalenderIcon.ClientID & "').datepicker('update',$('#" & m_sLinkedControlClientId & "').val())});")
            sLoadCalenderScript.Append("$( window ).on( 'load', function() {$('#" & m_sLinkedControlClientId & "').next('.pbdatepicker').first().on('change',function(ev){$('#" & m_sLinkedControlClientId & "').change() })});")
            sLoadCalenderScript.Append("$('#" & m_sLinkedControlClientId & "').change();")

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "LoadCalendarControl" & m_sLinkedControlClientId.ToString(), sLoadCalenderScript.ToString(), True)
            'Enable/Desable calendar Linked control
            Dim sEnableDisbaleCalender As String
            If bEnabled Then
                'enable the control
                lblCalenderIcon.Attributes.Remove("style")
                sEnableDisbaleCalender = "$('#" & m_sLinkedControlClientId & "').removeAttr('readonly');"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "EnableDisableCalendarControl" & m_sLinkedControlClientId.ToString(), sEnableDisbaleCalender, True)
            Else
                'disable calender icon and make text box to be readonly
                lblCalenderIcon.Attributes.Add("style", "pointer-events:none!important")
                sEnableDisbaleCalender = "$('#" & m_sLinkedControlClientId & "').attr('readonly', true);"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "EnableDisableCalendarControl" & m_sLinkedControlClientId.ToString(), sEnableDisbaleCalender, True)
            End If

            'Dim sMaskString As String = CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToLower().Replace("d", "9").Replace("m", "9").Replace("y", "9")
            'Dim sMask As String = "$('#" & m_sLinkedControlClientId & "').mask('" & sMaskString & "');"
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "sMask" & m_sLinkedControlClientId.ToString(), sMask, True)

            'Dim sValidateDate As String = "$('#" & m_sLinkedControlClientId & "').focusout(function () {  try { $.datepicker.parseDate('" & m_sDateFormat & "', $(this).val());  } catch (e) { alert('Invalid date');$(this).val('');$(this).focus();} }); "
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "sValidateDate" & m_sLinkedControlClientId.ToString(), sValidateDate, True)

        End Sub

        ''' <summary>
        ''' Get Control Hierarchy
        ''' </summary>
        ''' <param name="i"></param>
        ''' <remarks></remarks>
        Private Sub SetHLevel(ByVal i As Integer)
            'Try to auto-resolve by finding the linked control and using its NamingContainer ClientID
            Dim current As Control = Me.Parent
            While current IsNot Nothing
                Dim resolvedControl As Control = current.FindControl(m_sLinkedControl)
                If resolvedControl IsNot Nothing Then
                    sHLevel = resolvedControl.NamingContainer.ClientID
                    Return
                End If
                current = current.Parent
            End While

            'Fallback to original HLevel logic if FindControl fails
            'HIERARCHY LEVEL OF CONTROLS
            'ADD MORE IF NEEDED - MB - 25 MAY 07
            Select Case i
                Case 0
                    sHLevel = Me.ClientID
                Case 1
                    sHLevel = Me.Parent.ClientID
                Case 2
                    sHLevel = Me.Parent.Parent.ClientID
                Case 3
                    sHLevel = Me.Parent.Parent.Parent.ClientID
                Case 4
                    sHLevel = Me.Parent.Parent.Parent.Parent.ClientID
                Case 5
                    sHLevel = Me.Parent.Parent.Parent.Parent.Parent.ClientID
                Case 6
                    sHLevel = Me.Parent.Parent.Parent.Parent.Parent.Parent.ClientID
            End Select
        End Sub

        ''' <summary>
        ''' Get current culture date format string
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetDateFormat()
            'this jquery control supports on yy for year and all should be in small case
            m_sDateFormat = CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToLower()
        End Sub

        'Protected Sub Page_SaveStateComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SaveStateComplete
        '    revCalender.ControlToValidate = m_sLinkedControlClientId
        '    Dim sRegex As String = "^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"""
        '    revCalender.ValidationExpression = sRegex
        '    revCalender.Enabled = bEnabled
        'End Sub

    End Class

End Namespace