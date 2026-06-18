Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no 129. 
Imports SharedFiles


<System.Runtime.InteropServices.ProgId("FormFields_NET.FormFields")> _
Public NotInheritable Class FormFields
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: FormFields
    '
    ' Date: 29-07-1997
    '
    ' Description: Class to validate screen controls
    '
    ' Edit History:
    ' SP140998 - return pmtrue not vvalue in unformatcontrol
    'RFC151098 - Reverse the above amendment and change it back to how it was.
    '            Return vValue as this behaviour is used by existing live components.
    ' RAW 30/06/2003 : CQ1465 : improve handling of dattime fields - and other bits and pieces notived along the way
    ' CLG 17/11/2003 : CQ2807 : Date Field Cleared
    ' VB 14/02/2005  : PN18426 : PMAccountLookup added for 'AccountLookup' UserControl
    ' ***************************************************************** '
    ''developer guide no. Replaced iPMFunc.GetResData with GetResData in the whole document

    Private Const ACClass As String = "FormFields"

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_FormFields As Collection
    Private m_iLanguageID As Integer


    Public Property LanguageID() As Integer
        Get
            Return m_iLanguageID
        End Get
        Set(ByVal Value As Integer)
            m_iLanguageID = Value
            g_iLanguageID = m_iLanguageID
        End Set
    End Property

    '*******************************************************************
    ' GeneralMessage : Display a message from the res file
    '                  Assumes title & message are consecutive
    '*******************************************************************
    Public Function GeneralMessage(ByRef lMessageCode As Integer) As Integer
        Dim result As Integer = 0
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            sTitle = CStr(iPMFunc.GetResData(iLangID:=LanguageID, lId:=lMessageCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=LanguageID, lId:=lMessageCode + 1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If sMessage.Trim() = "" Then
                sMessage = "Message with code " & Conversion.Str(lMessageCode)
            End If

            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GeneralMessage", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneralMessage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single FormField into the FormFields Collection
    '
    ' ***************************************************************** '
    Public Function Add(ByRef oNewFormField As iPMFormControl.FormField) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Grid Columns are indexed by their column no

            'Tomo020800
            'Always include the gridcolumn
            '    If (oNewFormField.ControlType = PMGrid) Or (oNewFormField.ControlType = ACSpread) Then
            'Developer Guide No. 85
            If m_FormFields.Contains(oNewFormField.FormControl.Name & "-" & oNewFormField.GridColumn) = False Then
                m_FormFields.Add(oNewFormField, oNewFormField.FormControl.Name & "-" & oNewFormField.GridColumn)
            End If

            '    Else
            '      m_FormFields.Add oNewFormField, Key:=oNewFormField.FormControl.Name
            '    End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add FormField to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of FormFields in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try


            Return m_FormFields.Count

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a FormField from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As String)

        Try

            m_FormFields.Remove(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected FormField from the Collection.
    '
    '
    ' ***************************************************************** '
    'developer guide no.101 (As per VB Code)
    Public Function Item(ByRef vKey As Object) As iPMFormControl.FormField

        Dim result As iPMFormControl.FormField = Nothing
        Try


            Return m_FormFields(vKey)

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all FormFields from the Collection
    '
    '
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim lFieldCount As Integer

        Try

            ' Determine the number of FormField in the collection
            lFieldCount = Count()

            ' Delete all the items
            For lSub As Integer = 1 To lFieldCount
                Delete(CStr(1))
            Next lSub

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete All Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAll", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the FormField Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set FormField Collection to Nothing
            m_FormFields = Nothing
            m_FormFields = New Collection()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try


            ' Initialisation Code.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Sub New()
        MyBase.New()

        Try

            ' Class Initialise

            m_FormFields = New Collection()

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the FormControl class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: AddNewFormField
    '
    ' Description: Add a new FormField to the collection
    '
    ' ***************************************************************** '
    'developer guide no. 165(Guide)
    Public Function AddNewFormField(ByVal ctlControl As Control, Optional ByVal lFormat As Integer = -1, Optional ByVal lFieldType As Integer = -1, Optional ByVal lGridColumn As Integer = -1, Optional ByVal lMandatory As Integer = -1, Optional ByVal lCurrencyID As Integer = -1, Optional ByVal lDecimalPlaces As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim oFormField As FormField
        Dim ctlLabel As Control
        'Dim ctlCont As Control

        Try

            oFormField = New FormField()

            With oFormField

                .FormControl = ctlControl

                ' Format type for field eg PMFormatStringCase
                'developer guide no. 164(Guide)
                If lFormat = -1 Then
                    .FieldFormat = gPMConstants.PMEFormatStyle.PMFormatString
                Else
                    .FieldFormat = lFormat
                End If

                ' Base data type eg PMString
                'developer guide no. 164(Guide)
                If lFieldType = -1 Then
                    .FieldType = gPMConstants.PMEDataType.PMString
                Else
                    .FieldType = lFieldType
                End If

                ' To determine formatting rules
                'developer guide no. 164(Guide)
                If lCurrencyID = -1 Then
                    .CurrencyID = 0
                Else
                    .CurrencyID = lCurrencyID
                End If

                ' For doubles - decimals for formatting
                'developer guide no. 164(Guide)
                If lDecimalPlaces = -1 Then
                    .DecimalPlaces = 0
                Else
                    .DecimalPlaces = lDecimalPlaces
                End If

                ' Field must be entered if PMMandatory
                ' Fields are hidden if PMNonVisible
                'developer guide no. 164(Guide)
                If lMandatory = -1 Then
                    .IsMandatory = False
                    .IsVisible = False
                Else
                    .IsMandatory = lMandatory = gPMConstants.PMEMandatoryStatus.PMMandatory
                    .IsVisible = Not (lMandatory = gPMConstants.PMEMandatoryStatus.PMNonVisible)
                End If

                ' Gridcolumn number for grid columns
                'developer guide no. 164(Guide)
                If lGridColumn = -1 Then
                    '.GridColumn = -1 
                    .GridColumn = 0
                Else
                    'Tomo020800
                    'Always use the grid column
                    '        If (TypeOf ctlControl Is TDBGrid) Or _
                    '(TypeOf ctlControl Is vaSpread) Or _
                    '(TypeOf ctlControl Is uctPMGridControl) Then 'JK280898 Added to recognise Spread control
                    .GridColumn = lGridColumn
                    '        Else
                    '          .GridColumn = -1
                    '        End If
                End If

                m_lReturn = CType(Add(oFormField), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If it's mandatory then embolden it's label

                If .IsMandatory Then
                    ctlLabel = GetLabelForControl(.FormControl)
                    If Not (ctlLabel Is Nothing) Then
                        ctlLabel.Font = VB6.FontChangeBold(ctlLabel.Font, True)
                    End If
                End If

                ' If it's not visible then hide the control & it's label

                If Not .IsVisible Then
                    ctlLabel = GetLabelForControl(.FormControl)
                    .FormControl.Visible = False
                    If Not (ctlLabel Is Nothing) Then
                        ControlHelper.SetVisible(ctlLabel, False)
                    End If
                End If

            End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to AddNewFormField", vApp:=ACApp, vClass:=ACClass, vMethod:="AddNewFormField", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LostFocus
    '
    ' Description: To be called when a form loses focus on a FormField
    '              the control is the actual control that focus was lost from
    '              formatting and validation is carried out on the field
    'JAS(CMG) - 26/9/02 (ref:867) Added MessageThenFocus() calls for the
    '           following messages:
    '           ACInvalidNumber
    '           ACInvalidPercentage
    '           ACInvalidYear
    '           ACInvalidCurrency
    '
    ' Kevin Renshaw (CMG) 4/3/2003 - Issue 1673 - Val function not working with
    '                ',' separator - use CDbl instead.
    ' ***************************************************************** '
    Public Function LostFocus(ByRef ctlControl As Object, Optional ByVal vRow As Object = Nothing, Optional ByVal vCol As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sDate As String = ""
        Dim dtDate As Date
        Dim sNewValue As String = ""
        Dim oItem As FormField
        Dim vIndex As Integer
        Dim lFieldFormat As gPMConstants.PMEFormatStyle

        Try

            ' Optionals are for grid control

            If Not Information.IsNothing(vCol) Then
                vIndex = vCol
                'Tomo020800
            Else
                vIndex = 0
            End If

            ' The FormField that relates to this control
            'Tomo020800
            '  Set oItem = Item(ctlControl.Name & vIndex)

            oItem = Item(ctlControl.Name & "-" & CStr(vIndex))
            If oItem Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With oItem
                lFieldFormat = .FieldFormat

                Select Case .ControlType
                    Case gPMConstants.PMEControlType.PMTextBox, gPMConstants.PMEControlType.PMGrid, gPMConstants.PMEControlType.PMAccountLookup

                        ' Remember the current row

                        If .ControlType = gPMConstants.PMEControlType.PMGrid And Not Information.IsNothing(vRow) Then

                            'developer guide no. 24(Guide)
                            .BookMark = vRow
                        End If

                        Select Case lFieldFormat
                            Case gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMConstants.PMEFormatStyle.PMFormatDateMedium, gPMConstants.PMEFormatStyle.PMFormatDateShort, gPMConstants.PMEFormatStyle.PMFormatDateTimeLong, gPMConstants.PMEFormatStyle.PMFormatDateTimeMedium, gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, gPMConstants.PMEFormatStyle.PMFormatTimeLong, gPMConstants.PMEFormatStyle.PMFormatTimeMedium, gPMConstants.PMEFormatStyle.PMFormatTimeShort, gPMConstants.PMEFormatStyle.PMFormatMonthOnlyLong, gPMConstants.PMEFormatStyle.PMFormatMonthOnlyMedium, gPMConstants.PMEFormatStyle.PMFormatMonthOnlyShort, gPMConstants.PMEFormatStyle.PMFormatDayOnlyLong, gPMConstants.PMEFormatStyle.PMFormatDayOnlyMedium, gPMConstants.PMEFormatStyle.PMFormatDayOnlyShort
                                Dim isInvalidEntryForDate As Boolean = False
                                If Strings.Len(.Text) > 0 Then

                                    ' Unformat the field so it is safe to process as a date

                                    Try
                                        dtDate = CDate(gPMFunctions.UnFormatField(lFieldFormat, gPMConstants.PMEDataType.PMDate, .Text))
                                    Catch ex As Exception
                                        isInvalidEntryForDate = True
                                    End Try
                                End If

                                If (Strings.Len(.Text) > 0 And ToSafeInteger(dtDate.ToOADate) = -1) OrElse isInvalidEntryForDate Then
                                    m_lReturn = CType(MessageThenFocus(ACInvalidDateRangeMsg, oItem, gPMFunctions.FormatField(lFieldFormat, DateTime.Now)), gPMConstants.PMEReturnCode)
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                                If Strings.Len(.Text) > 0 Then

                                    ' Format the date nicely now
                                    .Text = gPMFunctions.FormatField(lFieldFormat, dtDate)
                                End If

                                ''JK280898-Format the date to it's year only form
                            Case gPMConstants.PMEFormatStyle.PMFormatDateYearOnly
                                Dim dbNumericTemp As Double
                                If Information.IsDate(.Text) Then
                                    '--> to return say 1999 after 1/2/99 is entered
                                    ' RAW 30/06/2003 : CQ1465 : replaced hard-coded lFieldFormat control's own format property
                                    .Text = gPMFunctions.FormatField(lFieldFormat, .Text)
                                ElseIf (Double.TryParse(.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                                    ' add 1/ to year entered and then format to return
                                    ' just the year.
                                    ' RAW 30/06/2003 : CQ1465 : replaced hard-coded lFieldFormat control's own format property
                                    .Text = gPMFunctions.FormatField(lFieldFormat, "1/" & .Text)
                                Else
                                    If (.Text) <> "" Then
                                        ' RAW 30/06/2003 : CQ1465 : initialise control contents before setting focus
                                        m_lReturn = CType(MessageThenFocus(ACInvalidYear, oItem, ""), gPMConstants.PMEReturnCode)
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                End If


                                ' 'JK280898-Format the number to it's correct form
                            Case gPMConstants.PMEFormatStyle.PMFormatLong, gPMConstants.PMEFormatStyle.PMFormatInteger
                                Dim dbNumericTemp2 As Double
                                If Not Double.TryParse(.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                                    If (.Text) <> "" Then
                                        ' RAW 30/06/2003 : CQ1465 : initialise control contents before setting focus
                                        m_lReturn = CType(MessageThenFocus(ACInvalidNumber, oItem, ""), gPMConstants.PMEReturnCode)
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                ElseIf (.Text > "") Then
                                    'MKW270404 PN9392 Remove Commas from Integer.
                                    .Text = CStr(Conversion.Val(.Text.Replace(",", "")))
                                End If

                                'JK280898-Format the number to it's percentage form
                            Case gPMConstants.PMEFormatStyle.PMFormatPercent
                                'RFC151098 Remove Percent before IsNumeric test
                                If .Text.EndsWith("%") Then
                                    .Text = .Text.Substring(0, Strings.Len(.Text) - 1)
                                End If

                                ' Check for valid numeric
                                Dim dbNumericTemp3 As Double
                                If Double.TryParse(.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then

                                    .Text = .FormattedText
                                Else
                                    ' Only raise "Invalid" error if string is not empty
                                    If Strings.Len(.Text) Then
                                        ' RAW 30/06/2003 : CQ1465 : initialise control contents before setting focus
                                        m_lReturn = CType(MessageThenFocus(ACInvalidPercentage, oItem, ""), gPMConstants.PMEReturnCode)
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                End If

                                'RDC 03072001 - pecentage with 4 d.p.
                            Case gPMConstants.PMEFormatStyle.PMFormatPercentFourDecimal
                                'RFC151098 Remove Percent before IsNumeric test
                                If .Text.EndsWith("%") Then
                                    .Text = .Text.Substring(0, Strings.Len(.Text) - 1)
                                End If
                                Dim dbNumericTemp4 As Double
                                If Not Double.TryParse(.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                                    If (.Text) <> "" Then
                                        ' RAW 30/06/2003 : CQ1465 : initialise control contents before setting focus
                                        m_lReturn = CType(MessageThenFocus(ACInvalidPercentage, oItem, ""), gPMConstants.PMEReturnCode)
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                ElseIf (.Text > "") Then
                                    .Text = CStr(Conversion.Val(.Text))
                                    ' RAW 30/06/2003 : CQ1465 : replaced hard-coded lFieldFormat control's own format property
                                    .Text = gPMFunctions.FormatField(lFieldFormat, .Text)
                                ElseIf .Text = "0.0000%" Then
                                    .Text = ""
                                End If

                                'JK280898 - Format to decimal
                            Case gPMConstants.PMEFormatStyle.PMFormatDecimal
                                Dim dbNumericTemp5 As Double
                                If Not Double.TryParse(.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                                    If (.Text) <> "" Then
                                        ' RAW 30/06/2003 : CQ1465 : initialise control contents before setting focus
                                        m_lReturn = CType(MessageThenFocus(ACInvalidNumber, oItem, ""), gPMConstants.PMEReturnCode)
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                ElseIf (.Text > "") Then
                                    .Text = CStr(CDbl(.Text))
                                    ' RAW 30/06/2003 : CQ1465 : replaced hard-coded lFieldFormat control's own format property
                                    .Text = gPMFunctions.FormatField(lFieldFormat, .Text)
                                Else
                                    .Text = ""
                                End If

                                ' RAW 30/06/2003 : CQ1465 : added
                            Case gPMConstants.PMEFormatStyle.PMFormatDouble
                                Dim dbNumericTemp6 As Double
                                If Not Double.TryParse(.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                                    If (.Text) <> "" Then
                                        ' RAW 30/06/2003 : CQ1465 : initialise control contents before setting focus
                                        m_lReturn = CType(MessageThenFocus(ACInvalidNumber, oItem, ""), gPMConstants.PMEReturnCode)
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                ElseIf (.Text > "") Then
                                    .Text = CStr(CDbl(.Text))
                                    ' no need to format it
                                Else
                                    .Text = ""
                                End If
                                ' RAW 30/06/2003 : CQ1465 : end

                                ' RAW 30/06/2003 : CQ1465 : added PMFormatStringCase , PMFormatStringUpper
                            Case gPMConstants.PMEFormatStyle.PMFormatString, gPMConstants.PMEFormatStyle.PMFormatStringMultiLine, gPMConstants.PMEFormatStyle.PMFormatStringCase, gPMConstants.PMEFormatStyle.PMFormatStringUpper
                                If .Text > "" Then
                                    .Text = .Text
                                    .Text = gPMFunctions.FormatField(lFieldFormat, .Text)
                                Else
                                    .Text = ""
                                End If

                            Case gPMConstants.PMEFormatStyle.PMFormatCurrency
                                ' Format value to a currency
                                Dim dbNumericTemp7 As Double
                                If Not Double.TryParse(.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
                                    If (.Text) <> "" Then
                                        ' RAW 30/06/2003 : CQ1465 : initialise control contents before setting focus
                                        m_lReturn = CType(MessageThenFocus(ACInvalidCurrency, oItem, ""), gPMConstants.PMEReturnCode)
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                Else
                                    .Text = gPMFunctions.FormatField(lFieldFormat, .Text)
                                End If


                                'Shouldn't there be validation here for Money and WholeMoney?


                            Case Else

                                .Text = .FormattedText
                        End Select

                    Case ACSpread

                        .GridColumn = vCol


                        For iRowIndex As Integer = 1 To .FormControl.MaxRows

                            .GridRow = iRowIndex


                            Select Case lFieldFormat
                                Case gPMConstants.PMEFormatStyle.PMFormatDateLong
                                    If .Text.Trim() <> "" Then
                                        sDate = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, .Text)
                                        If sDate = "" Then
                                            m_lReturn = CType(MessageThenFocus(ACInvalidDateMsg, oItem), gPMConstants.PMEReturnCode)
                                            .Text = DateTimeHelper.ToString(DateTime.Now)
                                            .Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, .Text)
                                            result = gPMConstants.PMEReturnCode.PMFalse
                                            .FormControl.Focus()

                                            .FormControl.Action = 0
                                            Return result
                                        Else
                                            .Text = sDate
                                        End If
                                    End If
                                Case gPMConstants.PMEFormatStyle.PMFormatDateShort
                                    If .Text.Trim() <> "" Then
                                        sDate = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, .Text)
                                        If sDate = "" Then
                                            m_lReturn = CType(MessageThenFocus(ACInvalidDateMsg, oItem), gPMConstants.PMEReturnCode)
                                            .Text = DateTimeHelper.ToString(DateTime.Now)
                                            .Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, .Text)
                                            result = gPMConstants.PMEReturnCode.PMFalse
                                            .FormControl.Focus()

                                            .FormControl.Action = 0
                                            Return result
                                        Else
                                            .Text = sDate
                                        End If
                                    End If
                                Case Else

                                    .Text = .FormattedText
                            End Select

                        Next iRowIndex

                End Select
            End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process lost focus", vApp:=ACApp, vClass:=ACClass, vMethod:="LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GotFocus
    '
    ' Description: To be called when a form gains focus on a FormField
    '              the control is the actual control that gained focus
    '              formatting and is carried out on the field
    ' ***************************************************************** '
    Public Function GotFocus(ByRef ctlControl As Control, Optional ByVal vRow As Integer = 0, Optional ByVal vCol As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sDate As String = ""
        Dim oItem As FormField
        Dim vIndex As Integer

        Try

            ' Optionals are for the grid control

            If Not Information.IsNothing(vCol) Then
                vIndex = vCol
                'Tomo020800
            Else
                vIndex = 0
            End If

            ' The FormField that it refers to
            'Tomo020800
            '  Set oItem = Item(ctlControl.Name & vIndex)
            oItem = Item(ctlControl.Name & "-" & CStr(vIndex))
            If oItem Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With oItem

                Select Case .ControlType
                    Case gPMConstants.PMEControlType.PMTextBox, gPMConstants.PMEControlType.PMGrid, gPMConstants.PMEControlType.PMAccountLookup

                        Select Case oItem.FieldFormat
                            ' RAW 30/06/2003 : CQ1465 : added PMFormatDateMedium
                            Case gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMConstants.PMEFormatStyle.PMFormatDateMedium
                                If .Text.Trim() <> "" Then
                                    ' switch to short format for data entry
                                    sDate = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, .Text)
                                    If sDate <> "" Then
                                        .Text = sDate
                                    End If
                                End If

                                ' RAW 30/06/2003 : CQ1465 : added
                            Case gPMConstants.PMEFormatStyle.PMFormatDateTimeMedium
                                ' do not alter PMFormatDateTimeLong format - otherwise will lose seconds !!!!

                                If .Text.Trim() <> "" Then
                                    ' switch to short format for data entry
                                    sDate = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, .Text)
                                    If sDate <> "" Then
                                        .Text = sDate
                                    End If
                                End If
                                ' RAW 30/06/2003 : CQ1465 : end

                                ' RAW 30/06/2003 : CQ1465 : added
                            Case gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatPercentFourDecimal
                                ' remove '%' for data entry
                                If Strings.Len(.Text) > 0 Then
                                    If Mid(.Text, Strings.Len(.Text), 1) = "%" Then
                                        .Text = Mid(.Text, 1, Strings.Len(.Text) - 1)
                                    End If
                                End If
                                ' RAW 30/06/2003 : CQ1465 : end

                                ' RAW 30/06/2003 : CQ1465 : added
                            Case gPMConstants.PMEFormatStyle.PMFormatMoney, gPMConstants.PMEFormatStyle.PMFormatWholeMoney
                                ' remove '£' for data entry
                                If Strings.Len(.Text) > 0 Then
                                    If .Text.StartsWith("£") Then
                                        .Text = Mid(.Text, 2, Strings.Len(.Text) - 1)
                                    End If
                                End If
                                ' RAW 30/06/2003 : CQ1465 : end

                            Case Else
                                ' no action for other formats
                        End Select

                        If .ControlType = gPMConstants.PMEControlType.PMTextBox Then
                            iPMFunc.SelectText(ctlControl)
                        End If
                        If .ControlType = gPMConstants.PMEControlType.PMAccountLookup Then
                            iPMFunc.SelectText(ctlControl)
                        End If
                    Case ACSpread


                        For vRow = 1 To .FormControl.MaxRows

                            .GridRow = vRow
                            .GridColumn = vCol

                            Select Case oItem.FieldFormat
                                Case gPMConstants.PMEFormatStyle.PMFormatDateLong
                                    If .Text.Trim() <> "" Then
                                        ' switch to short format
                                        sDate = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, .Text)
                                        If sDate <> "" Then
                                            .Text = sDate
                                        End If
                                    End If
                                Case gPMConstants.PMEFormatStyle.PMFormatPercent
                                    If Strings.Len(.Text) > 0 Then
                                        If Mid(.Text, Strings.Len(.Text), 1) = "%" Then
                                            .Text = Mid(.Text, 1, Strings.Len(.Text) - 1)
                                        End If
                                    End If
                                Case Else
                                    ' no action for other formats
                            End Select

                        Next vRow


                    Case Else
                        ' no gotfocus processing for non textbox
                End Select
            End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process got focus", vApp:=ACApp, vClass:=ACClass, vMethod:="GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    ' Name: AddFormatGridText
    '
    ' Description: Called for Adding and Formatting of Grid Text
    '              (Only for Text be loaded with Grid at run-time)
    '              JK070898
    ' ***************************************************************** '
    Public Function AddFormatGridText(ByRef ctlControl As Control, Optional ByRef lFormat As Integer = 0, Optional ByRef lFieldType As Integer = 0, Optional ByRef lCurrencyID As Integer = 0, Optional ByRef lDecimalPlaces As Integer = 0, Optional ByRef vlCol As Integer = 0, Optional ByRef vlRow As Integer = 0, Optional ByRef sText As String = "") As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oFormField As FormField

        Try

            oFormField = New FormField()

            With oFormField

                .FormControl = ctlControl

                ' Format type for field eg PMFormatStringCase
                'developer guide no. 164(Guide)
                If lFormat = Nothing Or lFormat = 0 Then
                    .FieldFormat = gPMConstants.PMEFormatStyle.PMFormatString
                Else
                    .FieldFormat = lFormat
                End If

                ' Base data type eg PMString
                'developer guide no. 164(Guide)
                If lFieldType = Nothing Or lFieldType = 0 Then
                    .FieldType = gPMConstants.PMEDataType.PMString
                Else
                    .FieldType = lFieldType
                End If


                ' Gridcolumn number for grid columns
                'developer guide no. 164(Guide)
                If vlCol = Nothing Or vlCol = 0 Then
                    .GridColumn = -1
                Else

                    Select Case ctlControl.GetType().Name
                        Case "TDBGrid", "ITrueDBGridCtrl", "uctPMGridControl", "vaSpread"
                            .GridColumn = vlCol
                        Case Else
                            .GridColumn = -1
                    End Select
                End If

                ' Gridrow number for grid row
                'developer guide no. 164(Guide)
                If vlRow = Nothing Or vlRow = 0 Then
                    .GridRow = -1
                Else

                    Select Case ctlControl.GetType().Name
                        Case "TDBGrid", "ITrueDBGridCtrl", "uctPMGridControl", "vaSpread"
                            .GridRow = vlRow
                        Case Else
                            .GridRow = -1
                    End Select
                End If

                ' Gridtext for grid
                'developer guide no. 164(Guide)
                If sText = Nothing Or sText = "" Then
                    .Text = ""
                Else

                    Select Case ctlControl.GetType().Name
                        Case "TDBGrid", "ITrueDBGridCtrl", "uctPMGridControl", "vaSpread"
                            .Text = sText
                        Case Else
                            .Text = ""
                    End Select
                End If

                ' To determine formatting rules
                'developer guide no. 164(Guide)
                If lCurrencyID = Nothing Or lCurrencyID = 0 Then
                    .CurrencyID = 0
                Else
                    .CurrencyID = lCurrencyID
                End If

                ' For doubles - decimals for formatting
                'developer guide no. 164(Guide)
                If lDecimalPlaces = Nothing Or lDecimalPlaces = 0 Then
                    .DecimalPlaces = 0
                Else
                    .DecimalPlaces = lDecimalPlaces
                End If


                Select Case .ControlType
                    Case gPMConstants.PMEControlType.PMGrid

                        .Text = .FormattedText
                    Case Else
                        ' nothing to do

                End Select

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to AddFormatGridText", vApp:=ACApp, vClass:=ACClass, vMethod:="AddFormatGridText", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FormatControl
    '
    ' Description: Called for initial formatting of a FormField
    ' ***************************************************************** '
    Public Function FormatControl(ByRef ctlControl As Object, ByRef vControlValue As Object) As Integer

        Dim result As Integer = 0
        Dim oItem As FormField

        Try

            'Tomo020800
            '  Set oItem = Item(ctlControl.Name)

            oItem = Item(ctlControl.Name & "-0")
            If oItem Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With oItem
                Select Case .ControlType
                    Case gPMConstants.PMEControlType.PMTextBox, gPMConstants.PMEControlType.PMAccountLookup
                        If ctlControl.GetType().Name = "uctRichTextBox" Then
                            ReflectionHelper.SetMember(.FormControl, "TextRTF", CStr(vControlValue))
                        Else

                            .Text = CStr(vControlValue)

                            .Text = .FormattedText
                        End If
                    Case gPMConstants.PMEControlType.PMCombo, gPMConstants.PMEControlType.PMListBox

                        m_lReturn = CType(SetListIndex(.FormControl, CInt(vControlValue)), gPMConstants.PMEReturnCode)

                    Case gPMConstants.PMEControlType.PMCheckBox, gPMConstants.PMEControlType.PMOptionButton


                        Select Case CInt(vControlValue)
                            Case gPMConstants.PMEReturnCode.PMTrue, True

                                'Developer Guide No 226
                                ReflectionHelper.SetMember(.FormControl, "CheckState", CheckState.Checked)

                            Case gPMConstants.PMEReturnCode.PMFalse, False

                                'Developer Guide No 226
                                ReflectionHelper.SetMember(.FormControl, "CheckState", CheckState.Unchecked)
                            Case Else

                                'Developer Guide No 226
                                ReflectionHelper.SetMember(.FormControl, "CheckState", CheckState.Indeterminate)
                        End Select


                    Case Else
                        ' nothing to do
                End Select

            End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to FormatControl", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnFormatControl
    '
    ' Description: Called for initial formatting of a FormField
    ' ***************************************************************** '
    Public Function UnformatControl(ByRef ctlControl As Object) As Object

        Dim result As Object = Nothing
        Dim oItem As FormField
        Dim vValue As Object

        Try

            'Tomo020800
            '  Set oItem = Item(ctlControl.Name)

            oItem = Item(ctlControl.Name & "-0")

            If oItem Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With oItem


                vValue = .FieldValue
            End With

            'RFC151098 - Return vValue as this behaviour is used by existing live components.
            'SP140998 - return pmtrue not vvalue

            Return vValue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to UnformatControl", vApp:=ACApp, vClass:=ACClass, vMethod:="UnformatControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FormatGridControl
    '
    ' Description: Called for initial formatting of a FormField
    'JK240898
    ' ***************************************************************** '
    Public Function FormatGridControl(ByRef ctlControl As Object, ByRef vlCol As Integer, ByRef vlRow As Integer, ByRef vControlValue As Object) As Integer

        Dim result As Integer = 0
        Dim oItem As FormField
        Dim vIndex As Integer

        Try

            ' Optionals are for grid control
            If Not False Then
                vIndex = vlCol
            End If


            oItem = Item(ctlControl.Name & CStr(vIndex))

            If oItem Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oItem.ControlType = gPMConstants.PMEControlType.PMGrid Then

                oItem.GridRow = vlRow
                oItem.GridColumn = vlCol

            End If

            With oItem


                Select Case .ControlType
                    Case gPMConstants.PMEControlType.PMGrid

                        .Text = .FormattedText
                    Case Else
                        ' nothing to do
                End Select

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to FormatGridControl", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatGridControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnFormatGridControl
    '
    ' Description: Called for unformatting of a FormField
    'JK240898
    ' ***************************************************************** '
    Public Function UnformatGridControl(ByRef ctlControl As Object, ByRef vlRow As Integer, ByRef vlCol As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oItem As FormField
        Dim vValue As Object
        Dim vIndex As Integer

        Try

            ' Optionals are for grid control
            If Not False Then
                vIndex = vlCol
            End If


            oItem = Item(ctlControl.Name & CStr(vIndex))

            If oItem.ControlType = gPMConstants.PMEControlType.PMGrid Then

                oItem.GridRow = vlRow
                oItem.GridColumn = vlCol

            End If

            If oItem Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With oItem


                vValue = .FieldValue
            End With

            result = gPMConstants.PMEReturnCode.PMTrue


            oItem.Text = CStr(vValue)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to UnformatGridControl", vApp:=ACApp, vClass:=ACClass, vMethod:="UnformatGridControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FormatControlArray
    '
    ' Description: Called for initial formatting of a FormField
    ' ***************************************************************** '
    'Public Function FormatControlArray(ByRef ctlControl As Object, ByRef vControlValue As Object) As Integer
    Public Function FormatControlArray(ByRef ctlControl As Object, ByRef vControlValue As Object, Optional ByVal ValueEditedForIndex As Integer = -1) As Integer


        'Tomo020800
        'New function cloned from FormatControl, only difference is use of ctlControl.Index

        Dim result As Integer = 0
        Dim oItem As FormField

        Try

            'Like this
            '  Set oItem = Item(ctlControl.Name)
            If ValueEditedForIndex <> -1 Then
                oItem = Item(ctlControl.Name & "-" & ValueEditedForIndex)
            Else
                oItem = Item(ctlControl.Name & "-" & ctlControl.Index)
            End If

            If oItem Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With oItem


                Select Case .ControlType
                    Case gPMConstants.PMEControlType.PMTextBox, gPMConstants.PMEControlType.PMAccountLookup

                        If ctlControl.GetType().Name = "uctRichTextBox" Then
                            ReflectionHelper.SetMember(.FormControl, "TextRTF", CStr(vControlValue))
                        Else
                            .Text = CStr(vControlValue)

                            .Text = .FormattedText
                        End If

                    Case gPMConstants.PMEControlType.PMCombo, gPMConstants.PMEControlType.PMListBox

                        m_lReturn = CType(SetListIndex(.FormControl, CInt(vControlValue)), gPMConstants.PMEReturnCode)

                    Case gPMConstants.PMEControlType.PMCheckBox, gPMConstants.PMEControlType.PMOptionButton


                        Select Case CInt(vControlValue)
                            Case gPMConstants.PMEReturnCode.PMTrue, True

                                'Developer Guide No 226
                                ReflectionHelper.SetMember(.FormControl, "CheckState", CheckState.Checked)
                            Case gPMConstants.PMEReturnCode.PMFalse, False

                                'Developer Guide No 226
                                ReflectionHelper.SetMember(.FormControl, "CheckState", CheckState.Unchecked)
                            Case Else

                                'Developer Guide No 226
                                ReflectionHelper.SetMember(.FormControl, "CheckState", CheckState.Indeterminate)
                        End Select


                    Case Else
                        ' nothing to do
                End Select

            End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to FormatControlArray", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatControlArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnformatControlArray
    '
    ' Description: Called for initial formatting of a FormField
    ' ***************************************************************** '
    'Public Function UnformatControlArray(ByRef ctlControl As Object) As Object
    Public Function UnformatControlArray(ByRef ctlControl As Object, Optional ByVal ValueEditedForIndex As Integer = -1) As Object

        'Tomo020800
        'New function cloned from UnformatControl, only difference is use of ctlControl.Index
        Dim result As Object = Nothing
        Dim oItem As FormField
        Dim vValue As Object

        Try

            'Like this 
            '  Set oItem = Item(ctlControl.Name)

            If ValueEditedForIndex <> -1 Then
                oItem = Item(ctlControl.Name & "-" & ValueEditedForIndex)
            Else
                oItem = Item(ctlControl.Name & "-" & ctlControl.Index)
            End If

            If oItem Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With oItem


                vValue = .FieldValue

            End With

            'RFC151098 - Return vValue as this behaviour is used by existing live components.
            'SP140998 - return pmtrue not vvalue

            Return vValue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to UnformatControlArray", vApp:=ACApp, vClass:=ACClass, vMethod:="UnformatControlArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatoryControls
    '
    ' Description: Loop around form ensuring that all mandatory fields
    '              have been entered. If not set focus to them and stop
    '
    ' ***************************************************************** '
    Public Function CheckMandatoryControls() As Integer

        Dim result As Integer = 0

        Try

            For Each oItem As FormField In m_FormFields
                With oItem
                    Select Case .ControlType
                        Case ACSpread

                            For iColIndex As Integer = 1 To .FormControl.MaxCols
                                .GridColumn = iColIndex

                                For iRowIndex As Integer = 1 To .FormControl.MaxRows
                                    .GridRow = iRowIndex
                                    If .IsMandatory And .IsBlank Then

                                        .FormControl.Row = .GridRow

                                        .FormControl.Col = .GridColumn
                                        .FormControl.Focus()

                                        .FormControl.Action = 0
                                        m_lReturn = CType(MessageThenFocus(ACMandyMsg, oItem), gPMConstants.PMEReturnCode)

                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                Next iRowIndex
                            Next iColIndex
                        Case Else
                            If .IsMandatory And .IsBlank Then
                                m_lReturn = CType(MessageThenFocus(ACMandyMsg, oItem), gPMConstants.PMEReturnCode)
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                    End Select
                End With
            Next oItem

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to CheckMandatoryControls", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatoryControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'MJS 03/02/98 ValidateControls Function to validate
    'data according to type and format
    Public Function ValidateControls(ByRef oItem As FormField) As Boolean

        Dim result As Boolean = False
        Try

            result = True
            'Check for different types...
            With oItem
                Select Case .FieldType
                    'String - PMString
                    Case gPMConstants.PMEDataType.PMString
                        'Integer - PMInteger
                        'Long - PMLong
                    Case gPMConstants.PMEDataType.PMInteger, gPMConstants.PMEDataType.PMLong
                        'Check numeric
                        Dim dbNumericTemp As Double
                        If Not Double.TryParse(.FieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                            result = False
                        Else
                            'Check for no decimal places
                            If CDbl(.FieldValue - .FieldValue) <> 0 Then
                                result = False
                            End If
                        End If

                        If gPMConstants.PMEDataType.PMLong Then
                            'Check >= -2147483648 and <= 2147483648
                            If .FieldValue < -2147483648.0# Or .FieldValue > 2147483648.0# Then
                                result = False
                            End If
                        End If

                        'Double - PMDouble
                    Case gPMConstants.PMEDataType.PMDouble
                        'Check numeric
                        Dim dbNumericTemp2 As Double
                        If Not Double.TryParse(.FieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                            result = False
                        End If

                        'Date - PMDate
                    Case gPMConstants.PMEDataType.PMDate
                        'Check if date
                        If Not Information.IsDate(.FormattedText) Then
                            result = False
                        End If

                        ' RDC07062001 extra date validation
                        If .FormattedText.Year < 1753 Or .FormattedText.Year > 9999 Then
                            result = False
                        End If

                        'Currency - PMCurrency
                    Case gPMConstants.PMEDataType.PMCurrency
                        'Check numeric
                        Dim dbNumericTemp3 As Double
                        If Not Double.TryParse(.FieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                            result = False
                        Else

                            'Check for 2 decimal places
                            If CDbl(.FieldValue - .FieldValue) <> 0 Then
                                result = False
                            End If
                        End If

                        'Decimal
                    Case gPMConstants.PMEDataType.PMDecimal
                        'Check numeric
                        Dim dbNumericTemp4 As Double
                        If Not Double.TryParse(.FieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                            result = False
                        End If

                End Select
            End With

            Return result

        Catch


            Return result
        End Try
    End Function

    Function ValidDataMsg(ByRef oItem As FormField) As Integer

        Try


            Select Case oItem.FieldFormat
                Case gPMConstants.PMEFormatStyle.PMFormatDateShort, gPMConstants.PMEFormatStyle.PMFormatDateMedium, gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, gPMConstants.PMEFormatStyle.PMFormatDateTimeMedium, gPMConstants.PMEFormatStyle.PMFormatDateTimeLong
                    Return ACInvalidDateMsg
                Case gPMConstants.PMEFormatStyle.PMFormatTimeShort, gPMConstants.PMEFormatStyle.PMFormatTimeMedium, gPMConstants.PMEFormatStyle.PMFormatTimeLong
                    Return ACInvalidTimeMsg
                Case Else
                    Return ACInvalidGeneralMsg
            End Select

        Catch
        End Try


        Exit Function
    End Function
    'Sumeet- Overloading function added to put values to the dynamically created control.
    ' ***************************************************************** '
    ' Name: FormatControlArray
    '
    ' Description: Called for initial formatting of a FormField
    ' ***************************************************************** '
    Public Function FormatControlArray(ByRef ctlControl As Object, ByRef vControlIndex As Integer, ByRef vControlValue As Object) As Integer

        'Tomo020800
        'New function cloned from FormatControl, only difference is use of ctlControl.Index

        Dim result As Integer = 0
        Dim oItem As FormField

        Try

            'Like this
            '  Set oItem = Item(ctlControl.Name)


            'oItem = Item(ctlControl.Name & "-" & ctlControl.Index)
            oItem = Item(ctlControl.Name & "-" & vControlIndex)
            If oItem Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With oItem


                Select Case .ControlType
                    Case gPMConstants.PMEControlType.PMTextBox, gPMConstants.PMEControlType.PMAccountLookup
                        If ctlControl.GetType().Name = "uctRichTextBox" Then
                            Try
                                .FormControl.TextRTF = CStr(vControlValue)
                            Catch ex As Exception
                                Dim rtfText As String = String.Empty
                                Using rtf As New RichTextBox()
                                    rtf.Text = vControlValue
                                    rtfText = rtf.Rtf
                                End Using
                                .FormControl.TextRTF = CStr(rtfText)
                            End Try
                        Else
                            .Text = CStr(vControlValue)
                            .Text = .FormattedText
                        End If
                    Case gPMConstants.PMEControlType.PMCombo, gPMConstants.PMEControlType.PMListBox

                        m_lReturn = CType(SetListIndex(.FormControl, CInt(vControlValue)), gPMConstants.PMEReturnCode)

                    Case gPMConstants.PMEControlType.PMCheckBox, gPMConstants.PMEControlType.PMOptionButton


                        Select Case CInt(vControlValue)
                            Case gPMConstants.PMEReturnCode.PMTrue, True

                                'Developer Guide No 226
                                ReflectionHelper.SetMember(.FormControl, "CheckState", CheckState.Checked)
                            Case gPMConstants.PMEReturnCode.PMFalse, False

                                'Developer Guide No 226
                                ReflectionHelper.SetMember(.FormControl, "CheckState", CheckState.Unchecked)
                            Case Else

                                'Developer Guide No 226
                                ReflectionHelper.SetMember(.FormControl, "CheckState", CheckState.Indeterminate)
                        End Select


                    Case Else
                        ' nothing to do
                End Select

            End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to FormatControlArray", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatControlArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
