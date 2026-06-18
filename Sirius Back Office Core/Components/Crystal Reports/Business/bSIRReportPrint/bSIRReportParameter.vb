Option Strict Off
Option Explicit On
Imports SSP.Shared

Friend NotInheritable Class SIRReportParameter
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRReportParameter
    '
    ' Date: 23/02/1999
    '
    ' Description: Describes the SIRReportParameter attributes.
    '
    ' Edit History: TF230299 - created
    '               TF130600 - Functions added to get defaults
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "SIRReportParameter"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Error Code
    Private m_lReturn As Integer

    Private m_oBusiness As Business
    Private m_iPrintJob As Integer
    Private m_iParamIndex As Integer
    Private m_sName As String = ""
    Private m_sPrompt As String = ""
    'eck190602
    Private m_sPartySearch As String = ""
    Private m_iValueType As Integer
    Private m_vDefaultValue As Object
    Private m_vCurrentValue As Object
    Private m_vCurrentIDValue As String = ""
    Private m_vDefaultValues() As Object
    Private m_vIDValues As Object
    Private m_iDefaultValueSet As Object
    Private m_iCurrentValueSet As Object
    ' Report Name
    Private m_sReportName As String = ""
    ' AkashS 14-Oct-2010    ReportDocument Object
    Private m_oReportDocument As Report

    Private m_sParentParamName As String
    Private m_bRemoveAll As Boolean
    Private m_bMultiSelect As Boolean
    Private m_sTableName As String
    Private m_sIDFieldName As String
    Private m_sDescriptionFieldName As String
    Private m_bIsDBTable As Boolean
    Private m_bSetReadOnly As Boolean
    Private m_sReadOnlyCriteria As String
    Private m_sReadOnlyValue As String
    Private m_bHasNoneOption As Boolean
    Private m_sCustomStoredProcedure As String
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property ParentParamName() As String
        Get
            Return m_sParentParamName
        End Get
        Set(ByVal value As String)
            m_sParentParamName = value
        End Set
    End Property

    Public ReadOnly Property TableName() As String
        Get
            Return m_sTableName
        End Get
    End Property

    Public ReadOnly Property IDFieldName() As String
        Get
            Return m_sIDFieldName
        End Get
    End Property

    Public ReadOnly Property DescriptionFieldName() As String
        Get
            Return m_sDescriptionFieldName
        End Get
    End Property

    Public ReadOnly Property RemoveAll() As Boolean
        Get
            Return m_bRemoveAll
        End Get
    End Property

    Public ReadOnly Property IsDBTable() As Boolean
        Get
            Return m_bIsDBTable
        End Get
    End Property

    Public ReadOnly Property SetReadOnly() As Boolean
        Get
            Return m_bSetReadOnly
        End Get
    End Property

    Public ReadOnly Property ReadOnlyCriteria() As String
        Get
            Return m_sReadOnlyCriteria
        End Get
    End Property

    Public ReadOnly Property ReadOnlyValue() As String
        Get
            Return m_sReadOnlyValue
        End Get
    End Property

    Public ReadOnly Property HasNoneOption() As Boolean
        Get
            Return m_bHasNoneOption
        End Get
    End Property

    Public ReadOnly Property IsMultiSelect() As Boolean
        Get
            Return m_bMultiSelect
        End Get
    End Property

    Public ReadOnly Property CustomStoredProcedure() As String
        Get
            Return m_sCustomStoredProcedure
        End Get
    End Property

    Public WriteOnly Property Business() As Business
        Set(ByVal Value As Business)

            m_oBusiness = Value

        End Set
    End Property

    Public WriteOnly Property Report() As Report
        Set(ByVal Value As Report)

            m_oReportDocument = Value

        End Set
    End Property

    Public Property printJob() As Integer
        Get

            Return m_iPrintJob

        End Get
        Set(ByVal Value As Integer)

            m_iPrintJob = Value

        End Set
    End Property

    Public Property ParamIndex() As Integer
        Get

            Return m_iParamIndex

        End Get
        Set(ByVal Value As Integer)

            m_iParamIndex = Value

        End Set
    End Property
    'eck190602
    Public Property PartySearch() As String
        Get

            Return m_sPartySearch

        End Get
        Set(ByVal Value As String)

            m_sPartySearch = Value

        End Set
    End Property

    Public ReadOnly Property Name() As String
        Get

            Return m_sName

        End Get
    End Property

    Public ReadOnly Property Prompt() As String
        Get

            Return m_sPrompt

        End Get
    End Property

    Public ReadOnly Property valueType() As Integer
        Get

            Return m_iValueType

        End Get
    End Property

    Public Property currentValue() As Object
        Get

            Return m_vCurrentValue

        End Get
        Set(ByVal Value As Object)

            m_vCurrentValue = Value

        End Set
    End Property

    Public Property CurrentIDValue() As String
        Get

            Return m_vCurrentIDValue

        End Get
        Set(ByVal Value As String)

            m_vCurrentIDValue = CStr(Value)

        End Set
    End Property

    Public Property DefaultValues() As Object
        Get

            Return (m_vDefaultValues)

        End Get
        Set(ByVal Value As Object)

            m_vDefaultValues = Value

        End Set
    End Property

    Public ReadOnly Property IDValues() As Object
        Get

            Return m_vIDValues

        End Get
    End Property

    Public Property reportName() As String
        Get

            Return m_sReportName

        End Get
        Set(ByVal Value As String)

            m_sReportName = Value

        End Set
    End Property

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

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

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


    ' ***************************************************************** '
    ' Name: SetPropertiesFromReport
    '
    ' Description: Use Crystal API to get parameter info
    '
    ' ***************************************************************** '
    Public Function SetPropertiesFromReport() As Integer

        Dim result As Integer = 0
        Dim lNumberOfDefaults As Integer
        Dim iSeparator As Integer
        Dim sDatabaseName, sTableName, sDisplayFieldName As String
        Dim sIDFieldName As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim bAddAll, bAddNull As Boolean
        Dim lDefaultCount As Integer
        Dim bNearestDateFound As Boolean
        Dim lDDLimit As Integer
        Dim sTempDefaultValue As String
        Dim bProcessBranch As Boolean = False
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oReportDocument.ReportParameters.ReportParameter(m_iParamIndex)
                '.StructSize = PE_SIZEOF_PARAMETER_FIELD_INFO
                '.CurrentValueSet = False
                '.DefaultValueSet = False

                ' m_lReturn = PEGetNthParameterField(printJob:=CShort(m_iPrintJob), varN:=CShort(m_iParamIndex), varInfo:=vParamFieldInfo)

                ' If m_lReturn = 0 Then
                'Return gPMConstants.PMEReturnCode.PMFalse
                ' End If

                ' Fields returned with null terminators
                m_sName = .Name
                ' Remove leading @ from front of database param names
                If m_sName.StartsWith("@") Then
                    m_sName = Microsoft.VisualBasic.Mid(m_sName, 2)
                End If

                ' Suppress prompt for special parameters
                Select Case m_sName.ToLower()
                    'DC270303 -ISS1911
                    Case "operator", "branch_id", "session_id"
                        m_sPrompt = ""

                        '12/03/2003 - PWC - Issue (ref:2896)
                    Case "period_id"
                        Select Case m_sReportName.Trim().ToUpper()
                            Case ACRptName_AccountsEarnedPremium, ACRptName_AccountsUnearnedPremium, ACRptName_ClaimsOSClaims, ACRptName_ClaimsOSClaimsGrossToNet

                                m_sPrompt = ""
                        End Select

                    Case Else
                        m_sPrompt = Strip(.Prompt) 'Strip(.Prompt)
                        If m_sPrompt.StartsWith("@") Then
                            m_sPrompt = Microsoft.VisualBasic.Mid(m_sPrompt, 2)
                        End If
                End Select

                ' Set parameter type
                Select Case .DataType
                    Case "Float"
                        m_iValueType = VariantType.Double
                    Case "Currency"
                        m_iValueType = VariantType.Decimal
                    Case "Boolean"
                        m_iValueType = VariantType.Boolean
                    Case "Date", "DateTime", "Time"
                        m_iValueType = VariantType.Date
                    Case "String"
                        m_iValueType = VariantType.String
                    Case "Integer"
                        m_iValueType = VariantType.Short
                        'Case PE_VI_COLOR
                        '    m_iValueType% = vbInteger
                    Case "Char"
                        m_iValueType = VariantType.Byte
                    Case "Long"
                        m_iValueType = VariantType.Integer
                    Case ""
                        m_iValueType = VariantType.Empty
                End Select

                ' Get number of default values
                If (m_sName.ToLower() = "branch_id") Then
                    lNumberOfDefaults = 0
                    bProcessBranch = True
                ElseIf (.DefaultValue IsNot Nothing) Then
                    lNumberOfDefaults = 1
                ElseIf .ValidValues IsNot Nothing Then
                    If .ValidValues.ParameterValues IsNot Nothing Then
                        lNumberOfDefaults = .ValidValues.ParameterValues.ParameterValue.Count
                    ElseIf .ValidValues.ParameterValues IsNot Nothing Then
                        lNumberOfDefaults = 1
                    End If
                End If
            End With

            If lNumberOfDefaults = -1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lNumberOfDefaults = 0 Then
                Return result
            End If

            For lCount As Integer = 0 To lNumberOfDefaults - 1

                'DN 10/12/02
                If Not Information.IsArray(m_vDefaultValues) Then
                    ReDim m_vDefaultValues(0)
                Else
                    ReDim Preserve m_vDefaultValues(m_vDefaultValues.GetUpperBound(0) + 1)
                End If
                lDefaultCount = m_vDefaultValues.GetUpperBound(0)

                With m_oReportDocument.ReportParameters.ReportParameter(m_iParamIndex)

                    '.StructSize = PE_SIZEOF_VALUE_INFO

                    ' m_lReturn = PEGetNthParameterDefaultValue(printJob:=CShort(m_iPrintJob), parameterFieldName:=vParamFieldInfo.Name.Value, reportName:=vParamFieldInfo.reportName.Value, Index:=CShort(lCount), valueInfo:=vParamValueInfo)

                    'If m_lReturn = 0 Then
                    'Return gPMConstants.PMEReturnCode.PMFalse
                    'End If

                    Select Case .DataType
                        Case "Float"
                            m_iValueType = VariantType.Double
                        Case "Currency"
                            m_iValueType = VariantType.Decimal
                        Case "Boolean"
                            m_iValueType = VariantType.Boolean
                        Case "Date"
                            m_iValueType = VariantType.Date
                        Case "DateTime"
                            m_iValueType = VariantType.Date
                        Case "Time"
                            m_iValueType = VariantType.Date
                        Case "String"
                            m_iValueType = VariantType.String
                        Case "Integer"
                            m_iValueType = VariantType.Short
                        Case "Char"
                            m_iValueType = VariantType.Byte
                        Case "Long"
                            m_iValueType = VariantType.Integer
                        Case ""
                            m_vDefaultValues(lDefaultCount) = DBNull.Value
                            m_iValueType = VariantType.Empty
                    End Select
                    If m_iValueType <> VariantType.Empty Then
                        If .ValidValues IsNot Nothing AndAlso .ValidValues.ParameterValues IsNot Nothing AndAlso .ValidValues.ParameterValues.ParameterValue.Count > 0 Then
                            m_vDefaultValues(lDefaultCount) = .ValidValues.ParameterValues.ParameterValue(lDefaultCount).Value

                        Else
                            Dim sDataSetName As String = ""
                            Dim sIDValue As String = ""

                            If .DefaultValue IsNot Nothing Then
                                sDataSetName = .Name
                                m_vDefaultValues(lDefaultCount) = .DefaultValue.Values.Value
                                If sDataSetName = "Branch" Then
                                    m_vDefaultValues(lDefaultCount) = .DefaultValue.Values.Value '"DB-" + "sirius" + "." + "Source" + ".Description"
                                ElseIf sDataSetName = "PeriodDate" Then
                                    m_vDefaultValues(lDefaultCount) = .DefaultValue.Values.Value
                                End If
                            ElseIf .DefaultValue IsNot Nothing AndAlso .DefaultValue.Values IsNot Nothing AndAlso Not String.IsNullOrEmpty(.DefaultValue.Values.Value) Then
                                m_vDefaultValues(lDefaultCount) = .DefaultValue.Values.Value
                            ElseIf .ValidValues IsNot Nothing AndAlso .ValidValues.ParameterValues.ParameterValue IsNot Nothing Then
                                m_vDefaultValues(lDefaultCount) = "DB-" + "Orion" + ".Period.Period_end_date"

                                'm_vDefaultValues(lDefaultCount) = .ValidValues.ParameterValues.ParameterValue(lDefaultCount).Value
                            End If

                        End If

                    End If

                End With

                ' Test for Populate from Database Table
                ' Default value in format :
                ' DB-DATABASE.TABLENAME.DISPLAYFIELDNAME.IDFIELDNAME.multiselectflag.removeallflag.addnoneflag.parenttablename.parentreadonlyvalue.readonlyvalue
                'DN 10/12/02
                If CStr(m_vDefaultValues(lDefaultCount)).ToUpper().StartsWith("DB-") Then
                    m_bIsDBTable = True
                    sTempDefaultValue = m_vDefaultValues(lDefaultCount)

                    ' Get Database Name
                    sDatabaseName = Strip(Microsoft.VisualBasic.Mid(CStr(m_vDefaultValues(lDefaultCount)), 4))
                    iSeparator = (sDatabaseName.IndexOf("."c) + 1)
                    sTableName = Microsoft.VisualBasic.Mid(sDatabaseName, iSeparator + 1)
                    sDatabaseName = sDatabaseName.Substring(0, iSeparator - 1)

                    ' Get Table Name
                    iSeparator = (sTableName.IndexOf("."c) + 1)
                    sDisplayFieldName = Microsoft.VisualBasic.Mid(sTableName, iSeparator + 1)
                    sTableName = sTableName.Substring(0, iSeparator - 1)

                    ' Get Display & ID field Names
                    iSeparator = (sDisplayFieldName.IndexOf("."c) + 1)
                    If iSeparator > 0 Then
                        sIDFieldName = Microsoft.VisualBasic.Mid(sDisplayFieldName, iSeparator + 1)
                        sDisplayFieldName = sDisplayFieldName.Substring(0, iSeparator - 1)
                    End If

                    'SMMI 1.15 development - additional values passed in on the default values string from the report
                    'DB-Sirius.tablename.descriptionfield.idfield.ismultiselect.removealloption.addnoneoption.parentparamname.makereadonlyparentvalue.readonlyvalue.spu_customchild

                    m_sTableName = sTableName
                    m_sDescriptionFieldName = sDisplayFieldName

                    If sIDFieldName <> "" And InStr(sIDFieldName, ".") > 0 Then

                        sIDFieldName = Left(sIDFieldName, InStr(sIDFieldName, ".") - 1)
                        m_sIDFieldName = sIDFieldName
                        iSeparator = InStr(1, sTempDefaultValue, sIDFieldName)

                        'we need to store the rest of the string so we can work with it
                        sTempDefaultValue = Mid(sTempDefaultValue, iSeparator + sIDFieldName.Length + 1)
                        sTempDefaultValue = (Replace(sTempDefaultValue, Chr(0), "")).Trim
                        sTempDefaultValue = Microsoft.VisualBasic.Mid(sTempDefaultValue, iSeparator + Microsoft.VisualBasic.Len(sIDFieldName) + 1)
                        sTempDefaultValue = Microsoft.VisualBasic.Trim(Replace(sTempDefaultValue, Chr(0), ""))

                        If sTempDefaultValue <> "" Then

                            'Get the multi select flag
                            'iSeparator = InStr(1, sTempDefaultValue, ".")

                            Dim iIterator As Integer

                            Do While iSeparator > 0
                                iSeparator = InStr(1, sTempDefaultValue, ".")

                                Select Case iIterator
                                    Case 0
                                        If iSeparator = 0 Then
                                            m_bMultiSelect = ToSafeBoolean(ToSafeLong(sTempDefaultValue))
                                        Else
                                            m_bMultiSelect = ToSafeBoolean(ToSafeLong(Left(sTempDefaultValue, iSeparator - 1)))
                                        End If

                                    Case 1
                                        'Get the remove <ALL> flag
                                        If iSeparator = 0 Then
                                            m_bRemoveAll = ToSafeBoolean(ToSafeLong(sTempDefaultValue))
                                        Else
                                            m_bRemoveAll = ToSafeBoolean(ToSafeLong(Left(sTempDefaultValue, iSeparator - 1))) '--Ritu
                                        End If

                                    Case 2
                                        'Get the add <None> flag
                                        If iSeparator = 0 Then
                                            m_bHasNoneOption = ToSafeBoolean(ToSafeLong(sTempDefaultValue))
                                        Else
                                            m_bHasNoneOption = ToSafeBoolean(ToSafeLong(Left(sTempDefaultValue, iSeparator - 1)))
                                        End If

                                    Case 3
                                        'Get the new parent table name
                                        If iSeparator = 0 Then
                                            m_sParentParamName = sTempDefaultValue
                                        Else
                                            m_sParentParamName = Left(sTempDefaultValue, iSeparator - 1)
                                        End If

                                    Case 4
                                        'Get the parent value that will make this child read-only
                                        If sTempDefaultValue <> "" Then
                                            m_bSetReadOnly = True
                                            If iSeparator = 0 Then
                                                m_sReadOnlyCriteria = sTempDefaultValue
                                            Else
                                                m_sReadOnlyCriteria = Left(sTempDefaultValue, iSeparator - 1)
                                            End If
                                        End If

                                    Case 5
                                        'set the child param's value here if there's one supplied
                                        If sTempDefaultValue <> "" Then
                                            m_sReadOnlyValue = sTempDefaultValue
                                        End If

                                    Case 6
                                        If sTempDefaultValue <> "" Then
                                            m_sCustomStoredProcedure = sTempDefaultValue
                                        End If

                                End Select

                                iIterator = iIterator + 1
                                sTempDefaultValue = Microsoft.VisualBasic.Mid(sTempDefaultValue, InStr(1, sTempDefaultValue, ".") + 1)
                            Loop
                        End If
                    End If

                    ' Special for Branch (source_id)
                    ' as it's not in all reoports!
                    If sTableName.ToLower() = "source" Then
                        sIDFieldName = "source_id"
                    End If

                    ' Special for Period (period_id)
                    If sTableName.ToLower() = "period" Then
                        sIDFieldName = "period_id"
                    End If

                    'DJM 28/08/2002 : Don't allow <ALL> for the account table
                    'JMK 30/06/2003 : allow <ALL> on Accounts if required using .rpt default value
                    '               : normally          DB-Orion.Account.short_code
                    '               : to allow <ALL>    DB-Orion.Account-ALL.short_code
                    Dim lAllIndex As Long
                    If m_bHasNoneOption Then
                        ReDim m_vDefaultValues(1)
                        ReDim m_vIDValues(1)
                        m_vDefaultValues(0) = "<NONE>"
                        m_vIDValues(0) = 0
                        lAllIndex = 1
                    End If

                    If sTableName.ToUpper() <> "ACCOUNT" Then
                        If sTableName.ToUpper() = "ACCOUNT-ALL" Then ' reset the table name
                            sTableName = "Account"
                        End If

                        If Not m_bRemoveAll Then
                            m_vDefaultValues(lAllIndex) = "<ALL>"
                        End If

                        'If m_bRemoveAll Then
                        '    m_vDefaultValues(lAllIndex) = "<ALL>"
                        'End If

                    Else
                        bAddAll = False
                        m_vDefaultValues(lAllIndex) = ""
                    End If

                    If IsArray(m_vDefaultValues) And UCase$(Left(m_vDefaultValues(0), 3)) = "DB-" Then
                        m_vDefaultValues(0) = ""
                    End If

                    'DC221001 -end
                    If Not Information.IsArray(m_vIDValues) Then
                        ReDim m_vIDValues(0)
                    End If

                    'eck190602
                    'Tracy Richards 21/11/03 - Gurad against "party_Type" as we do not want a party search for this
                    If Not sTableName.ToUpper().StartsWith("PARTY_TYPE") Then
                        ' SET 26112002 ISS 1317 - Policy Numbers
                        If sTableName.StartsWith("Party") Or sTableName.StartsWith("Account") Or sTableName.StartsWith("Insurance_Folder") Then
                            'eck040602 extend party type for multi tables
                            If PartySearch = "" Then
                                If m_sReportName.IndexOf("SubAgent") >= 0 Then
                                    PartySearch = "Party-UB" & "|" & sDisplayFieldName
                                Else
                                    PartySearch = sTableName & "|" & sDisplayFieldName
                                End If
                            Else
                                If m_sReportName.IndexOf("SubAgent") >= 0 Then
                                    PartySearch = PartySearch & "+" & "Party-UB" & "|" & sDisplayFieldName
                                Else
                                    PartySearch = PartySearch & "+" & sTableName & "|" & sDisplayFieldName
                                End If
                            End If
                        Else
                            PartySearch = ""
                        End If
                    Else
                        PartySearch = ""
                    End If
                    If PartySearch = "" Then
                        'eck190602end

                        ' Populate from Business object
                        With m_oBusiness
                            m_lReturn = .GetParametersFromDB(v_sDatabaseName:=sDatabaseName, v_sTableName:=sTableName, v_sDisplayFieldName:=sDisplayFieldName, v_sIDFieldName:=sIDFieldName, r_vDefaultValues:=vResultArray)

                            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                                ' OK - move on to next
                                ReDim Preserve m_vDefaultValues(lDefaultCount + lAllIndex)
                                ReDim Preserve m_vIDValues(lDefaultCount + lAllIndex)
                            ElseIf (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            Else
                                ' Populate parameter arrays

                                ReDim Preserve m_vDefaultValues(lDefaultCount + vResultArray.GetUpperBound(1) + 1 + lAllIndex)

                                ReDim Preserve m_vIDValues(lDefaultCount + vResultArray.GetUpperBound(1) + 1 + lAllIndex)
                                'TB 16/10/2002 - Exclusions for "ALL"
                                bAddAll = Not m_bRemoveAll
                                'bAddAll = m_bRemoveAll
                                bAddNull = False
                                bNearestDateFound = False
                                If m_sReportName.Trim().ToUpper() = ACRptName_UWPolicyListLong Then
                                    If m_sName.ToUpper() <> ("Policy").ToUpper() Then
                                        bAddAll = False
                                        bAddNull = True
                                    End If
                                    'JMK 18/12/2001 - do not need "ALL" for Period
                                ElseIf (sTableName.ToLower() = "period") Then

                                    '13/01/2003 - PWC - We DO require <ALL> for the following reports
                                    Select Case m_sReportName.Trim().ToUpper()
                                        Case ACRptName_AccountsEarnedPremium, ACRptName_AccountsUnearnedPremium, ACRptName_ClaimsOSClaims, ACRptName_ClaimsOSClaimsGrossToNet

                                            'Nar as display <ALL> flag is set to true by default

                                        Case Else 'As before
                                            bAddAll = False
                                            ' TB 11//11/2002: put nearest to today as default

                                            For lCount2 As Integer = 0 To vResultArray.GetUpperBound(1)

                                                If CDate(vResultArray(0, lCount2)) < DateTime.Now Then

                                                    m_vDefaultValues(0) = vResultArray(0, lCount2)
                                                    bNearestDateFound = True
                                                    Exit For
                                                End If
                                            Next lCount2
                                    End Select

                                    ' TB: Re-apply DN's no ALL for account
                                ElseIf (sTableName.ToLower() = "account") Then
                                    bAddAll = False
                                End If

                                'Don't add <all> to source list when multi-company. Only the logged in branch should show.
                                If sTableName.ToLower() = "source" And m_oBusiness.MultiCompany Then
                                    m_vDefaultValues(0) = ""
                                    bAddAll = False
                                End If

                                ' TB Check for exclusion before adding the ALL item to the list
                                If bAddAll Then
                                    m_vDefaultValues(lAllIndex) = "<ALL>"

                                    If vResultArray.GetUpperBound(0) = 1 Then

                                        m_vIDValues(lAllIndex) = lAllIndex
                                    End If
                                    ' The null parameter is replaced before display
                                ElseIf bAddNull Then
                                    m_vDefaultValues(lAllIndex) = "<NULL>"

                                    If vResultArray.GetUpperBound(0) = 1 Then

                                        m_vIDValues(lAllIndex) = lAllIndex
                                    End If
                                End If
                                ' If more items returned than allowable in the dropdown list
                                ' then limit the number

                                If vResultArray.GetUpperBound(1) > ACDropDownLimit Then
                                    lDDLimit = ACDropDownLimit
                                Else

                                    lDDLimit = vResultArray.GetUpperBound(1)
                                    If bNearestDateFound And lDDLimit > 0 Then
                                        lDDLimit -= 1
                                    End If
                                End If
                                For lCount2 As Integer = 0 To lDDLimit
                                    '                    For lCount2 = 0 To UBound(vResultArray, 2)
                                    If bNearestDateFound Then

                                        If Not m_vDefaultValues(0).Equals(vResultArray(0, lCount2)) Then

                                            m_vDefaultValues(lDefaultCount + lCount2 + 1 + lAllIndex) = vResultArray(0, lCount2)
                                        End If
                                    Else

                                        m_vDefaultValues(lDefaultCount + lCount2 + 1 + lAllIndex) = vResultArray(0, lCount2)
                                    End If

                                    If vResultArray.GetUpperBound(0) = 1 Then

                                        m_vIDValues(lDefaultCount + lCount2 + 1 + lAllIndex) = vResultArray(1, lCount2)
                                    End If
                                Next lCount2
                            End If
                        End With
                        'eck190602
                    End If

                End If

            Next lCount

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertiesFromReportFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertiesFromReport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function ProcessBranch() As Integer
        Dim lNumberOfDefaults As Integer = 1
        Dim lDefaultCount As Integer
        Dim sTempDefaultValue As String
        Dim sDatabaseName As String
        Dim iSeparator As Integer
        Dim sTableName As String
        Dim sDisplayFieldName As String
        Dim sIDFieldName As String
        Dim bAddAll As Boolean = False
        Dim bAddNull As Boolean = False
        Dim vResultArray(,) As Object = Nothing

        Dim bNearestDateFound As Boolean
        Dim lDDLimit As Integer
        For lCount As Integer = 0 To lNumberOfDefaults - 1

            'DN 10/12/02
            If Not Information.IsArray(m_vDefaultValues) Then
                ReDim m_vDefaultValues(0)
            Else
                ReDim Preserve m_vDefaultValues(m_vDefaultValues.GetUpperBound(0) + 1)
            End If
            lDefaultCount = m_vDefaultValues.GetUpperBound(0)

            m_iValueType = VariantType.String
            m_sPrompt = "Branch:"

            m_vDefaultValues(lDefaultCount) = "DB-" + "sirius" + "." + "Source" + ".Description"

            If CStr(m_vDefaultValues(lDefaultCount)).ToUpper().StartsWith("DB-") Then
                m_bIsDBTable = True
                sTempDefaultValue = m_vDefaultValues(lDefaultCount)

                ' Get Database Name
                sDatabaseName = Strip(Microsoft.VisualBasic.Mid(CStr(m_vDefaultValues(lDefaultCount)), 4))
                iSeparator = (sDatabaseName.IndexOf("."c) + 1)
                sTableName = Microsoft.VisualBasic.Mid(sDatabaseName, iSeparator + 1)
                sDatabaseName = sDatabaseName.Substring(0, iSeparator - 1)

                ' Get Table Name
                iSeparator = (sTableName.IndexOf("."c) + 1)
                sDisplayFieldName = Microsoft.VisualBasic.Mid(sTableName, iSeparator + 1)
                sTableName = sTableName.Substring(0, iSeparator - 1)

                ' Get Display & ID field Names
                iSeparator = (sDisplayFieldName.IndexOf("."c) + 1)
                If iSeparator > 0 Then
                    sIDFieldName = Microsoft.VisualBasic.Mid(sDisplayFieldName, iSeparator + 1)
                    sDisplayFieldName = sDisplayFieldName.Substring(0, iSeparator - 1)
                End If

                'SMMI 1.15 development - additional values passed in on the default values string from the report
                'DB-Sirius.tablename.descriptionfield.idfield.ismultiselect.removealloption.addnoneoption.parentparamname.makereadonlyparentvalue.readonlyvalue.spu_customchild

                m_sTableName = sTableName
                m_sDescriptionFieldName = sDisplayFieldName
                If sIDFieldName <> "" And InStr(sIDFieldName, ".") > 0 Then

                    sIDFieldName = Left(sIDFieldName, InStr(sIDFieldName, ".") - 1)
                    m_sIDFieldName = sIDFieldName
                    iSeparator = InStr(1, sTempDefaultValue, sIDFieldName)

                    'we need to store the rest of the string so we can work with it
                    sTempDefaultValue = Mid(sTempDefaultValue, iSeparator + sIDFieldName.Length + 1)
                    sTempDefaultValue = (Replace(sTempDefaultValue, Chr(0), "")).Trim
                    sTempDefaultValue = Microsoft.VisualBasic.Mid(sTempDefaultValue, iSeparator + Microsoft.VisualBasic.Len(sIDFieldName) + 1)
                    sTempDefaultValue = Microsoft.VisualBasic.Trim(Replace(sTempDefaultValue, Chr(0), ""))

                    If sTempDefaultValue <> "" Then

                        'Get the multi select flag
                        'iSeparator = InStr(1, sTempDefaultValue, ".")

                        Dim iIterator As Integer

                        Do While iSeparator > 0
                            iSeparator = InStr(1, sTempDefaultValue, ".")

                            Select Case iIterator
                                Case 0
                                    If iSeparator = 0 Then
                                        m_bMultiSelect = ToSafeBoolean(ToSafeLong(sTempDefaultValue))
                                    Else
                                        m_bMultiSelect = ToSafeBoolean(ToSafeLong(Left(sTempDefaultValue, iSeparator - 1)))
                                    End If

                                Case 1
                                    'Get the remove <ALL> flag
                                    If iSeparator = 0 Then
                                        m_bRemoveAll = ToSafeBoolean(ToSafeLong(sTempDefaultValue))
                                    Else
                                        m_bRemoveAll = ToSafeBoolean(ToSafeLong(Left(sTempDefaultValue, iSeparator - 1))) '--Ritu
                                    End If

                                Case 2
                                    'Get the add <None> flag
                                    If iSeparator = 0 Then
                                        m_bHasNoneOption = ToSafeBoolean(ToSafeLong(sTempDefaultValue))
                                    Else
                                        m_bHasNoneOption = ToSafeBoolean(ToSafeLong(Left(sTempDefaultValue, iSeparator - 1)))
                                    End If

                                Case 3
                                    'Get the new parent table name
                                    If iSeparator = 0 Then
                                        m_sParentParamName = sTempDefaultValue
                                    Else
                                        m_sParentParamName = Left(sTempDefaultValue, iSeparator - 1)
                                    End If

                                Case 4
                                    'Get the parent value that will make this child read-only
                                    If sTempDefaultValue <> "" Then
                                        m_bSetReadOnly = True
                                        If iSeparator = 0 Then
                                            m_sReadOnlyCriteria = sTempDefaultValue
                                        Else
                                            m_sReadOnlyCriteria = Left(sTempDefaultValue, iSeparator - 1)
                                        End If
                                    End If

                                Case 5
                                    'set the child param's value here if there's one supplied
                                    If sTempDefaultValue <> "" Then
                                        m_sReadOnlyValue = sTempDefaultValue
                                    End If

                                Case 6
                                    If sTempDefaultValue <> "" Then
                                        m_sCustomStoredProcedure = sTempDefaultValue
                                    End If

                            End Select

                            iIterator = iIterator + 1
                            sTempDefaultValue = Microsoft.VisualBasic.Mid(sTempDefaultValue, InStr(1, sTempDefaultValue, ".") + 1)
                        Loop
                    End If
                End If

                ' Special for Branch (source_id)
                ' as it's not in all reoports!
                If sTableName.ToLower() = "source" Then
                    sIDFieldName = "source_id"
                End If


                Dim lAllIndex As Long
                If m_bHasNoneOption Then
                    ReDim m_vDefaultValues(1)
                    ReDim m_vIDValues(1)
                    m_vDefaultValues(0) = "<NONE>"
                    m_vIDValues(0) = 0
                    lAllIndex = 1
                End If

                If sTableName.ToUpper() <> "ACCOUNT" Then
                    If sTableName.ToUpper() = "ACCOUNT-ALL" Then ' reset the table name
                        sTableName = "Account"
                    End If

                    If Not m_bRemoveAll Then
                        m_vDefaultValues(lAllIndex) = "<ALL>"
                    End If

                    'If m_bRemoveAll Then
                    '    m_vDefaultValues(lAllIndex) = "<ALL>"
                    'End If

                Else
                    bAddAll = False
                    m_vDefaultValues(lAllIndex) = ""
                End If

                If IsArray(m_vDefaultValues) And UCase$(Left(m_vDefaultValues(0), 3)) = "DB-" Then
                    m_vDefaultValues(0) = ""
                End If

                'DC221001 -end
                If Not Information.IsArray(m_vIDValues) Then
                    ReDim m_vIDValues(0)
                End If

                If PartySearch = "" Then
                    'eck190602end

                    ' Populate from Business object
                    With m_oBusiness
                        m_lReturn = .GetParametersFromDB(v_sDatabaseName:=sDatabaseName, v_sTableName:=sTableName, v_sDisplayFieldName:=sDisplayFieldName, v_sIDFieldName:=sIDFieldName, r_vDefaultValues:=vResultArray)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            ' OK - move on to next
                            ReDim Preserve m_vDefaultValues(lDefaultCount + lAllIndex)
                            ReDim Preserve m_vIDValues(lDefaultCount + lAllIndex)
                        ElseIf (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        Else
                            ' Populate parameter arrays

                            ReDim Preserve m_vDefaultValues(lDefaultCount + vResultArray.GetUpperBound(1) + 1 + lAllIndex)

                            ReDim Preserve m_vIDValues(lDefaultCount + vResultArray.GetUpperBound(1) + 1 + lAllIndex)
                            'TB 16/10/2002 - Exclusions for "ALL"
                            bAddAll = Not m_bRemoveAll
                            'bAddAll = m_bRemoveAll
                            bAddNull = False
                            bNearestDateFound = False
                            If m_sReportName.Trim().ToUpper() = ACRptName_UWPolicyListLong Then
                                If m_sName.ToUpper() <> ("Policy").ToUpper() Then
                                    bAddAll = False
                                    bAddNull = True
                                End If
                                'JMK 18/12/2001 - do not need "ALL" for Period
                            ElseIf (sTableName.ToLower() = "period") Then

                                '13/01/2003 - PWC - We DO require <ALL> for the following reports
                                Select Case m_sReportName.Trim().ToUpper()
                                    Case ACRptName_AccountsEarnedPremium, ACRptName_AccountsUnearnedPremium, ACRptName_ClaimsOSClaims, ACRptName_ClaimsOSClaimsGrossToNet

                                        'Nar as display <ALL> flag is set to true by default

                                    Case Else 'As before
                                        bAddAll = False
                                        ' TB 11//11/2002: put nearest to today as default

                                        For lCount2 As Integer = 0 To vResultArray.GetUpperBound(1)

                                            If CDate(vResultArray(0, lCount2)) < DateTime.Now Then

                                                m_vDefaultValues(0) = vResultArray(0, lCount2)
                                                bNearestDateFound = True
                                                Exit For
                                            End If
                                        Next lCount2
                                End Select

                                ' TB: Re-apply DN's no ALL for account
                            ElseIf (sTableName.ToLower() = "account") Then
                                bAddAll = False
                            End If

                            'Don't add <all> to source list when multi-company. Only the logged in branch should show.
                            If sTableName.ToLower() = "source" And m_oBusiness.MultiCompany Then
                                m_vDefaultValues(0) = ""
                                bAddAll = False
                            End If

                            ' TB Check for exclusion before adding the ALL item to the list
                            If bAddAll Then
                                m_vDefaultValues(lAllIndex) = "<ALL>"

                                If vResultArray.GetUpperBound(0) = 1 Then

                                    m_vIDValues(lAllIndex) = lAllIndex
                                End If
                                ' The null parameter is replaced before display
                            ElseIf bAddNull Then
                                m_vDefaultValues(lAllIndex) = "<NULL>"

                                If vResultArray.GetUpperBound(0) = 1 Then

                                    m_vIDValues(lAllIndex) = lAllIndex
                                End If
                            End If
                            ' If more items returned than allowable in the dropdown list
                            ' then limit the number

                            If vResultArray.GetUpperBound(1) > ACDropDownLimit Then
                                lDDLimit = ACDropDownLimit
                            Else

                                lDDLimit = vResultArray.GetUpperBound(1)
                                If bNearestDateFound And lDDLimit > 0 Then
                                    lDDLimit -= 1
                                End If
                            End If
                            For lCount2 As Integer = 0 To lDDLimit
                                '                    For lCount2 = 0 To UBound(vResultArray, 2)
                                If bNearestDateFound Then

                                    If Not m_vDefaultValues(0).Equals(vResultArray(0, lCount2)) Then

                                        m_vDefaultValues(lDefaultCount + lCount2 + 1 + lAllIndex) = vResultArray(0, lCount2)
                                    End If
                                Else

                                    m_vDefaultValues(lDefaultCount + lCount2 + 1 + lAllIndex) = vResultArray(0, lCount2)
                                End If

                                If vResultArray.GetUpperBound(0) = 1 Then

                                    m_vIDValues(lDefaultCount + lCount2 + 1 + lAllIndex) = vResultArray(1, lCount2)
                                End If
                            Next lCount2
                        End If
                    End With
                    'eck190602
                End If

            End If

        Next lCount
        Return 1
    End Function
    ' ***************************************************************** '
    ' Name: SendPropertiesToReport
    '
    ' Description:
    '
    ' ***************************************************************** '
    'Public Function SendPropertiesToReport() As Integer

    '    Dim result As Integer = 0
    '    Dim sName As String = ""

    '    Dim vParamFieldInfo As Object ' Module1.PEParameterFieldInfo = Module1.PEParameterFieldInfo.CreateInstance()
    '    Dim vParamValueInfo As Object ' Module1.PEValueInfo = Module1.PEValueInfo.CreateInstance()
    '    Dim bSubReportParameter As Boolean = False

    '    Try

    '        result = gPMConstants.PMEReturnCode.PMTrue

    '        ' Remove brackets from <ALL>

    '        If CStr(m_vCurrentValue) = "<ALL>" Then
    '            m_vCurrentValue = "ALL"
    '        ElseIf CStr(m_vCurrentValue) = "<NONE>" Then
    '            m_vCurrentValue = "NONE"
    '        End If

    '        ' Start Date - set as 00:00:00
    '        sName = m_sName.ToLower()
    '        If (sName.IndexOf("start date") >= 0) Or (sName.IndexOf("start_date") >= 0) Then
    '            If m_vCurrentValue IsNot Nothing AndAlso Not CStr(m_vCurrentValue).Contains(":") Then
    '                m_vCurrentValue = CDate(CStr(m_vCurrentValue) & " 00:00:00")
    '            End If
    '        End If

    '        ' End Date - set 1 second to midnight
    '        If (sName.IndexOf("end date") >= 0) Or (sName.IndexOf("end_date") >= 0) Then

    '            ' RAG 2001-11-27
    '            ' AAAAARGH! this is setting the end-time to midday instead of midnight !
    '            'm_vCurrentValue = CDate( _
    '            'CStr(m_vCurrentValue) & " 11:59:59")

    '            If m_sReportName.ToUpper <> ACRptName_AgencyDebitingBordereau And m_sReportName.ToUpper <> ACRptName_AgencyPaidBordereau Then
    '                If m_vCurrentValue IsNot Nothing AndAlso Not CStr(m_vCurrentValue).Contains(":") Then
    '                    m_vCurrentValue = CDate(CStr(m_vCurrentValue) & " 23:59:59")
    '                End If
    '            End If
    '        End If

    '        If (sName.IndexOf("party cnt") >= 0) Or (sName.IndexOf("party_cnt") >= 0) Then
    '            If m_vCurrentValue = "" Then
    '                m_vCurrentValue = "0"
    '            End If
    '        End If

    '        If (m_oReportDocument.ReportParameters.ReportParameter(m_iParamIndex).Name = "@Underwriting_Year" Or m_oReportDocument.ReportParameters.ReportParameter(m_iParamIndex).Name = "@AgentGroupCode") And m_vCurrentValue Is Nothing Then
    '            m_oReportDocument.ReportParameters.ReportParameter(m_iParamIndex).DefaultValue.Values.Value = "ALL"

    '            ''ElseIf m_oReportDocument.ReportParameters.ReportParameter(m_iParamIndex).ReportName.Length = 0 Or CStr(m_vCurrentValue) <> "" Then
    '            ''For i As Integer = 0 To m_iParamIndex - 1
    '            ''    If m_oReportDocument.ParameterFields(m_iParamIndex).Name = m_oReportDocument.ParameterFields(i).Name Then
    '            ''        bSubReportParameter = True
    '            ''        Exit For
    '            ''    End If
    '            ''Next
    '            ''If Not bSubReportParameter Then
    '            ''    m_oReportDocument.SetParameterValue(m_iParamIndex, m_vCurrentValue)
    '            ''End If
    '        End If

    '        Return result

    '    Catch excep As System.Exception

    '        result = gPMConstants.PMEReturnCode.PMError

    '        bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendPropertiesToReport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SendPropertiesToReport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

    '        Return result

    '    End Try
    'End Function

    Public Function FilterDefaultValues(ByRef oDatabase As dPMDAO.Database,
                                        ByVal v_sParentTableName As String,
                                        ByVal v_sIDCol As String,
                                        ByVal v_sDescriptionCol As String,
                                        ByVal v_sFilterVal As String,
                                        Optional ByVal v_sCustomStoredProcedure As String = "") As Integer

        Const kMethodName As String = "FilterDefaultValues"
        Dim sSQL As String = String.Empty
        Dim vResults(,) As Object = Nothing
        Dim vTempDefaultValues As Object = Nothing
        Dim lLoop As Long

        Dim iResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try

            'IF <ALL> Then we need all values (filter has been cleared)
            If v_sParentTableName <> "" Then

                If v_sFilterVal = "<ALL>" Then
                    sSQL = "SELECT CT." & v_sIDCol & ", CT." & v_sDescriptionCol & " FROM " & v_sParentTableName & " PT with (nolock) " &
                        "INNER JOIN " & TableName & " CT with (nolock) ON CT." & v_sParentTableName & "_id = PT." & v_sParentTableName & "_id "
                ElseIf v_sCustomStoredProcedure <> "" Then
                    sSQL = "EXEC " & v_sCustomStoredProcedure & "'" & v_sFilterVal & "'"
                Else
                    sSQL = "SELECT CT." & v_sIDCol & ", CT." & v_sDescriptionCol & " FROM " & v_sParentTableName & " PT with (nolock) " &
                        "INNER JOIN " & TableName & " CT with (nolock) ON CT." & v_sParentTableName & "_id = PT." & v_sParentTableName & "_id " &
                        "WHERE PT.description = '" & v_sFilterVal & "'"
                End If

            Else
                If v_sCustomStoredProcedure <> "" Then
                    sSQL = "EXEC " & v_sCustomStoredProcedure & "'" & v_sFilterVal & "'"
                End If
            End If

            With oDatabase

                .Parameters.Clear()
                m_lReturn = .SQLSelect(
                    sSQL:=sSQL,
                    sSQLName:=kMethodName,
                    bStoredProcedure:=False,
                    lNumberRecords:=gPMConstants.PMAllRecords,
                    vResultArray:=vResults)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed for " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                    Return iResult
                End If

            End With

            If IsArray(vResults) Then

                'we need to put the array into the class variable
                ReDim vTempDefaultValues(IIf(RemoveAll, vResults.GetUpperBound(2), vResults.GetUpperBound(2)))
                ReDim vTempDefaultValues(IIf(RemoveAll, Microsoft.VisualBasic.UBound(vResults, 2), Microsoft.VisualBasic.UBound(vResults, 2)))

                'If Not RemoveAll Then
                '------------------Ritu
                If RemoveAll Then
                    vTempDefaultValues(0) = "<ALL>"
                End If

                For lLoop = IIf(RemoveAll, 1, 0) To Microsoft.VisualBasic.UBound(vTempDefaultValues)
                    vTempDefaultValues(lLoop) = vResults(0, lLoop)
                Next lLoop
                '------------------------

            End If

            DefaultValues = vTempDefaultValues

        Catch ex As Exception
            iResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
        End Try

        Return iResult

    End Function

    Public Function HandleReadOnly(ByRef oDatabase As dPMDAO.Database,
                                   ByVal v_bReadOnly As Boolean,
                                   ByVal v_sIDCol As String,
                                   ByVal v_sDescriptionCol As String
                                   ) As Integer

        Const kMethodName As String = "HandleReadOnly"

        Dim sSQL As String
        Dim vResults(,) As Object = Nothing
        Dim vTempDefaultValues As Object = Nothing
        Dim lLoop As Long

        Dim iResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try

            If v_bReadOnly = True Then
                ReDim vResults(1, 0) '2 columns one row
                vResults(1, 0) = ReadOnlyValue
            Else
                sSQL = "SELECT CT." & v_sIDCol & ", CT." & v_sDescriptionCol & " FROM " & TableName & " CT with (nolock) "
                With oDatabase
                    .Parameters.Clear()
                    m_lReturn = .SQLSelect(
                        sSQL:=sSQL,
                        sSQLName:=kMethodName,
                        bStoredProcedure:=False,
                        lNumberRecords:=gPMConstants.PMAllRecords,
                        vResultArray:=vResults)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iResult = gPMConstants.PMEReturnCode.PMFalse
                        bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed for " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                        Return iResult
                    End If

                End With
            End If

            If IsArray(vResults) Then
                'we need to put the array into the class variable
                ReDim vTempDefaultValues(IIf(RemoveAll, Microsoft.VisualBasic.UBound(vResults, 2), Microsoft.VisualBasic.UBound(vResults, 2)))

                If RemoveAll Then
                    vTempDefaultValues(0) = "<ALL>"
                End If

                'For lLoop = IIf(RemoveAll, 0, 1) To UBound(vTempDefaultValues)
                For lLoop = IIf(RemoveAll, 1, 0) To Microsoft.VisualBasic.UBound(vTempDefaultValues) '--Ritu
                    vTempDefaultValues(lLoop) = vResults(1, lLoop)
                Next lLoop
            End If

            DefaultValues = vTempDefaultValues

        Catch ex As Exception
            iResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
        End Try

        Return iResult

    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        'bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
