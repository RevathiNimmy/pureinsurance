Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Text
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    ' ************************************************
    ' Added to replace global variables 06/02/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' ***************************************************************** '
    ' Class Name: FindDocument
    '
    ' Date: 18th August 1997
    '
    ' Description: Creatable FindDocument class used by the Find Document
    '              lookup.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Parameter Collection (Private)
    Private m_oParameters As dPMDAO.Parameters

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lError As Integer

    ' Task
    Private m_iTask As Integer

    ' Navigate
    Private m_lNavigate As Integer

    ' Process Mode
    Private m_lProcessMode As Integer

    ' Type of Business
    Private m_sTransactionType As New FixedLengthString(10)

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Component Sub Type
    Private m_sSubType As New FixedLengthString(20)
    ' Variable Data Business Component (Private)

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    Private m_sUnderwritingOrAgency As String = ""

    ' PM Lookup Business Component (Private)
    'Private m_oLookup As bPMLookup.Business
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType.Value

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property
    ' Product family
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property
    ' PUBLIC Property Procedures (End)

    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Property Procedures (Begin)

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType.Value = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a DocumentType.
    '
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 1) As Object
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gACTLibrary.ACTLookupDocumentType

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = gACTLibrary.ACTLookupPostingStatus


            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else

            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""

                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

                Case gPMConstants.PMELookupType.PMLookupAllEffective
                    'complete when Document object is defined

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    'With oInsuranceFile

                    ' {* USER DEFINED CODE (Begin) *}
                    '    vTabArray(PMLookupKey, 0) = .DocumentTypeID
                    '    dtEffectiveDate = .EffectiveDate
                    ' {* USER DEFINED CODE (End) *}

                    'End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    'With oInsuranceFile

                    ' {* USER DEFINED CODE (Begin) *}
                    '    vTabArray(PMLookupKey, 0) = .DocumentTypeID
                    ' {* USER DEFINED CODE (End) *}

                    'End With
                    ' Default Effective Date to current date/time
                    'dtEffectiveDate = Now

            End Select



            ' Get the Lookup items

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Property Procedures (Start)
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise





        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Get reference to database
            'EK 010300
            '    Set m_oDatabase = GetOrionDatabase(m_lReturn, m_bCloseDatabase, vDatabase)

            ' Get an instance of the database

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'EK 010300 New Object getting method
            '    ' Create PM Lookup Business Object
            '    Set m_oLookup = New bPMLookup.Business
            '
            '    ' Initialise PM Lookup Business passing our Database Reference.
            '    m_lReturn& = m_oLookup.Initialise( _
            ''        sUsername:=sUsername, _
            ''        sPassword:=sPassword, _
            ''        iUserID:=iUserID, _
            ''        iSourceID:=iSourceID, _
            ''        iLanguageID:=iLanguageID, _
            ''        iCurrencyID:=iCurrencyID, _
            ''        iLogLevel:=iLogLevel, _
            ''        sCallingAppName:=ACApp, _
            ''        vDatabase:=m_oDatabase)



            m_oLookup = New bPMLookup.Business
            m_lReturn = m_oLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
            ' Remove component services

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub





    ' ***************************************************************** '
    ' Name: FindLikeDocumentRef (Public)
    '
    ' Description: Selects Documents the DocumentRef
    '
    ' ***************************************************************** '
    Public Function SearchLikeDocumentRef(ByRef sDocumentRef As String, ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Convert any wildcards
            sDocumentRef = bPMFunc.ConvertWildCard(v_sSearchString:=sDocumentRef)

            ' Add the ShortName parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="document_ref", vValue:=CStr(sDocumentRef), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchLikeDocumentRef")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACDocumentLikeDocumentRefSQL, sSQLName:=ACDocumentLikeDocumentRefName, bStoredProcedure:=ACDocumentLikeDocumentRefStored, lNumberRecords:=lNumberOfRecords, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchLikeDocumentRef")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' If NO records were found return PMFalse
            If Not Information.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            Else
                For iCount1 As Integer = vResultArray.GetLowerBound(0) To vResultArray.GetUpperBound(0)
                    For iCount2 As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)


                        vResultArray(iCount1, iCount2) = "" & CStr(vResultArray(iCount1, iCount2))
                    Next iCount2

                Next iCount1
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchLikeDocumentRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchLikeDocumentRef", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    '****************************************************************** '
    '* Name: SearchByQuery (Public)
    '*
    '* Description: Selects documents according to the query by example
    '*               parameters
    '* VB 01/02/2005 PN18899 : New parameter added for SourceId (i.e. Company ID)
    '****************************************************************** '
    'developer guide no. 33
    Public Function SearchByQuery(ByRef lNumberOfRecords As Integer, ByRef vResultArray(,) As Object, Optional ByVal vDocumentRef As String = "", Optional ByVal vDateFrom As Integer = 0, Optional ByVal vDateTo As Integer = 0, Optional ByVal vDocumentType As Integer = 0, Optional ByVal vPostingStatus As Integer = 0, Optional ByVal vSourceArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim iParamCount As Integer
        Dim iUpper, iLower As Integer
        Dim sTempStr As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iParamCount = 0

            'build the sql select statement according to the parameters passed
            sSQL = ""
            sSQL = sSQL & "Select distinct " & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & " Document.document_ref," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & " Document.document_date," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & " Document.documenttype_Id," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & " Document.postingstatus_id," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & " Document.comment," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & " Document.document_id," & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & " Document.company_id," & Strings.Chr(13) & Strings.Chr(10)

            'TN20010702 - start
            sSQL = sSQL & " DocumentType.from_sirius" & Strings.Chr(13) & Strings.Chr(10)
            'TN20010702 - end

            sSQL = sSQL & " From Document," & Strings.Chr(13) & Strings.Chr(10)

            'TN20010702 - start
            sSQL = sSQL & " DocumentType"
            sSQL = sSQL & "  Where Document.documenttype_id = DocumentType.documenttype_id" & Strings.Chr(13) & Strings.Chr(10)

            iParamCount = 1
            'TN20010702 - end

            'append the parameters to the where clause

            If Not Information.IsNothing(vDocumentRef) Then
                If vDocumentRef <> "" Then
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sSQL = sSQL & " And"
                    End If

                    vDocumentRef = bPMFunc.ConvertWildCard(v_sSearchString:=vDocumentRef)
                    sSQL = sSQL & " document_ref Like '" & vDocumentRef & "'" & Strings.Chr(13) & Strings.Chr(10)

                End If
            End If


            If Not Information.IsNothing(vDateFrom) Then
                If vDateFrom <> -1 Then
                    'date from is present
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sSQL = sSQL & " And"
                    End If
                    sSQL = sSQL & " document_date >= '"
                    sSQL = sSQL & DateTime.FromOADate(vDateFrom).ToString("yyyy/MM/dd").Trim() & "'" & Strings.Chr(13) & Strings.Chr(10)
                End If
            End If


            If Not Information.IsNothing(vDateTo) Then
                If vDateTo <> -1 Then
                    'date to is present
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sSQL = sSQL & " And"
                    End If
                    sSQL = sSQL & " document_date <= '"
                    sSQL = sSQL & DateTime.FromOADate(vDateTo).ToString("yyyy/MM/dd").Trim() & "'" & Strings.Chr(13) & Strings.Chr(10)
                End If
            End If


            If Not Information.IsNothing(vDocumentType) Then
                If vDocumentType <> -1 Then
                    'document type is present
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sSQL = sSQL & " And"
                    End If
                    'eck310801
                    '           sSql = sSql & " documenttype_id =" & Trim$(CStr(vDocumentType)) & vbCrLf
                    sSQL = sSQL & " Document.documenttype_id =" & CStr(vDocumentType).Trim() & Strings.Chr(13) & Strings.Chr(10)
                End If
            End If


            If Not Information.IsNothing(vPostingStatus) Then
                If vPostingStatus <> -1 Then
                    'posting status is present
                    iParamCount += 1
                    If iParamCount > 1 Then
                        sSQL = sSQL & " And"
                    End If
                    sSQL = sSQL & " postingstatus_id =" & CStr(vPostingStatus).Trim() & Strings.Chr(13) & Strings.Chr(10)
                End If
            End If

            '    If (IsMissing(vDateOfBirth) = False) Then
            '        If (vDateOfBirth <> -1 = True) Then
            '            'DateOfBirth is present
            '            iParamCount = iParamCount + 1
            '            If iParamCount > 1 Then
            '                sSql = sSql & " And"
            '            End If
            '            sSql = sSql & " DateOfBirth = '" & Trim$(Format(vDateOfBirth, "YYYY/MM/DD")) & "'" & vbCrLf
            '        End If
            '    End If

            iParamCount += 1
            If iParamCount > 1 Then
                sSQL = sSQL & " And"
            End If

            'if vSourceArray is not missing override checking of company_id
            'on vSourceArray else check on m_iSourceID

            If Not Information.IsNothing(vSourceArray) Then
                If Information.IsArray(vSourceArray) Then
                    iUpper = vSourceArray.GetUpperBound(1)
                    iLower = vSourceArray.GetLowerBound(1)
                    sTempStr = New StringBuilder("")

                    For iRow As Integer = iLower To iUpper

                        sTempStr.Append(CStr(vSourceArray(0, iRow)))
                        If iRow < iUpper Then
                            sTempStr.Append(", ")
                        End If
                    Next iRow

                    sSQL = sSQL & "  company_id IN (" & sTempStr.ToString() & ")"
                Else
                    sSQL = sSQL & "  company_id = " & CStr(vSourceArray)
                End If
            Else
                sSQL = sSQL & "  company_id = " & CStr(m_iSourceID)
            End If

            If iParamCount = 0 Then
                '        'no parameters passed so query cannot be executed
                '        SearchByQuery = PMFalse
                '        Exit Function
                sSQL = sSQL & " 1 = 1 " 'dummy where clause (always true) - get all documents

            End If

            'add the order by clause
            sSQL = sSQL & "  Order by Document.document_ref" & Strings.Chr(13) & Strings.Chr(10)

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACDocumentFromQueryName, bStoredProcedure:=ACDocumentFromQueryStored, lNumberRecords:=lNumberOfRecords, vResultArray:=vResultArray)


            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' If NO records were found return PMFalse
            If Not Information.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            Else
                For iCount1 As Integer = vResultArray.GetLowerBound(0) To vResultArray.GetUpperBound(0)
                    For iCount2 As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)


                        vResultArray(iCount1, iCount2) = "" & CStr(vResultArray(iCount1, iCount2))
                    Next iCount2

                Next iCount1
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            MessageBox.Show(excep.Message, Application.ProductName)
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchByQuery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    '' ***************************************************************** '
    '' Name: GetId (Public)
    ''
    '' Description: Selects Policies by ID and populates the
    ''              Base Details.
    ''
    '' ***************************************************************** '
    'Public Function GetID(lID As Long, Optional ByVal vName As Variant, _
    ''                    Optional ByVal vShortName As Variant, Optional ByVal vSourceId As Variant) As Long
    '
    'Dim lRowsAffected As Long
    '
    '    On Error GoTo Err_GetId
    '
    '    GetID = PMTrue
    '
    '    If (IsMissing(vName) And IsMissing(vShortName)) = True Then
    '        GetID = PMFalse
    '        lID& = -1
    '        Exit Function
    '    End If
    '
    '
    '    'set the source_id from the global property if parameter is missing
    '    If (IsMissing(vSourceId) = True) Then
    '        vSourceId = m_iSourceID
    '    End If
    '
    '    ' Clear the Database Parameters Collection
    '    m_oDatabase.Parameters.Clear
    '
    '    If (IsMissing(vName) = False) Then
    '        ' Add the name parameter (INPUT)
    '        m_lError& = m_oDatabase.Parameters.Add( _
    ''                sName:="Document_Name", _
    ''                vValue:=CVar(vName), _
    ''                iDirection:=PMParamInput, _
    ''                iDataType:=PMString)
    '    ElseIf (IsMissing(vShortName) = False) Then
    '        ' Add the shortname parameter (INPUT)
    '        m_lError& = m_oDatabase.Parameters.Add( _
    ''                sName:="Document_ShortName", _
    ''                vValue:=CVar(vShortName), _
    ''                iDirection:=PMParamInput, _
    ''                iDataType:=PMString)
    '    Else
    '        'else case
    '    End If
    '
    '    If (m_lError& <> PMTrue) Then
    '
    '        ' Log Error Message
    '        LogMessage m_sUsername, _
    ''            iType:=PMLogError, _
    ''            sMsg:="oParameters.Add failed", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetId"
    '
    '        GetID = PMFalse
    '        Exit Function
    '
    '    End If
    '
    '    ' Add the source_id parameter (INPUT)
    '    m_lError& = m_oDatabase.Parameters.Add( _
    ''            sName:="Source_Id", _
    ''            vValue:=CVar(vSourceId), _
    ''            iDirection:=PMParamInput, _
    ''            iDataType:=PMInteger)
    '
    '
    '    If (m_lError& <> PMTrue) Then
    '
    '        ' Log Error Message
    '        LogMessage m_sUsername, _
    ''            iType:=PMLogError, _
    ''            sMsg:="oParameters.Add failed", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetId"
    '
    '        GetID = PMFalse
    '        Exit Function
    '
    '    End If
    '
    '    ' Add the Document cnt parameter (OUTPUT)
    '    m_lError& = m_oDatabase.Parameters.Add( _
    ''            sName:="Document_cnt", _
    ''            vValue:=lID&, _
    ''            iDirection:=PMParamOutput, _
    ''            iDataType:=PMLong)
    '
    '    If (m_lError& <> PMTrue) Then
    '
    '        ' Log Error Message
    '        LogMessage m_sUsername, _
    ''            iType:=PMLogError, _
    ''            sMsg:="oParameters.Add failed", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetId"
    '
    '        GetID = PMFalse
    '        Exit Function
    '
    '    End If
    '
    '    If (IsMissing(vName) = False) Then
    '        ' Execute SQL Statement
    '        m_lError& = m_oDatabase.SQLAction( _
    ''            sSql:=ACDocumentFromNameSQL, _
    ''            sSQLName:=ACDocumentFromNameName, _
    ''            bStoredProcedure:=ACDocumentFromNameStored, _
    ''            lrecordsAffected:=lRowsAffected)
    '    ElseIf (IsMissing(vShortName) = False) Then
    '        ' Execute SQL Statement
    '        m_lError& = m_oDatabase.SQLAction( _
    ''            sSql:=ACDocumentFromShortNameSQL, _
    ''            sSQLName:=ACDocumentFromShortNameName, _
    ''            bStoredProcedure:=ACDocumentFromShortNameStored, _
    ''            lrecordsAffected:=lRowsAffected)
    '    Else
    '
    '    End If
    '
    '    If (m_lError& <> PMTrue) Then
    '
    '        ' Log Error Message
    '        LogMessage m_sUsername, _
    ''            iType:=PMLogError, _
    ''            sMsg:="m_oDatabase.SQLSelect failed", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetId"
    '
    '        GetID = PMFalse
    '        Exit Function
    '
    '    End If
    '
    '    ' Get the Document_cnt of the record selected
    '     lID& = NullToLong(m_oDatabase.Parameters.Item("Document_cnt").Value)
    '
    '    If (lID& = -1) Then
    '        GetID = PMNotFound
    '    End If
    '
    '     Exit Function
    '
    'Err_GetId:
    '
    '    ' Error Section.
    '
    '    GetID = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="GetId Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetId", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    '' ***************************************************************** '
    '' Name: GetName (Public)
    ''
    '' Description: Selects the Document name using the Document ID.
    ''
    '' ***************************************************************** '
    'Public Function GetName( _
    ''    lDocumentCnt As Long, _
    ''    sDocumentName As String) As Long
    '
    'Dim lRowsAffected As Long
    '
    '    On Error GoTo Err_GetName
    '
    '    GetName = PMTrue
    '
    '    ' Clear the Database Parameters Collection
    '    m_oDatabase.Parameters.Clear
    '
    '    ' Add the parameter (INPUT)
    '    m_lError& = m_oDatabase.Parameters.Add( _
    ''            sName:="Document_cnt", _
    ''            vValue:=CVar(lDocumentCnt&), _
    ''            iDirection:=PMParamInput, _
    ''            iDataType:=PMLong)
    '
    '    If (m_lError& <> PMTrue) Then
    '        ' Log Error Message
    '        LogMessage m_sUsername, _
    ''            iType:=PMLogError, _
    ''            sMsg:="oParameters.Add failed", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetName"
    '
    '        GetName = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Add the parameter (OUTPUT)
    '    m_lError& = m_oDatabase.Parameters.Add( _
    ''            sName:="shortname", _
    ''            vValue:=CVar(sDocumentName$), _
    ''            iDirection:=PMParamOutput, _
    ''            iDataType:=PMString)
    '
    '    If (m_lError& <> PMTrue) Then
    '        ' Log Error Message
    '        LogMessage m_sUsername, _
    ''            iType:=PMLogError, _
    ''            sMsg:="oParameters.Add failed", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetName"
    '
    '        GetName = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Execute SQL Statement
    '    m_lError& = m_oDatabase.SQLAction( _
    ''        sSql:=ACDocumentFromCntSQL, _
    ''        sSQLName:=ACDocumentFromCntName, _
    ''        bStoredProcedure:=ACDocumentFromCntStored, _
    ''        lrecordsAffected:=lRowsAffected)
    '
    '    If (m_lError& <> PMTrue) Then
    '        ' Log Error Message
    '        LogMessage m_sUsername, _
    ''            iType:=PMLogError, _
    ''            sMsg:="m_oDatabase.SQLSelect failed", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetName"
    '
    '        GetName = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Get the Document_cnt of the record selected
    '     sDocumentName$ = Trim$(NullToString(m_oDatabase.Parameters.Item("shortname").Value))
    '
    '    If (Trim$(sDocumentName$) = "") Then
    '        GetName = PMNotFound
    '    End If
    '
    '     Exit Function
    '
    'Err_GetName:
    '
    '    ' Error Section.
    '
    '    GetName = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="GetName Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetName", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ClearParameters (Private)
    '
    ' Description: Clears the Database Parameters Collection if there
    '              are any.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ClearParameters) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub ClearParameters()
    '
    'Try 
    '
    ' Clear the Databases Parameters Collection
    'If m_oDatabase.Parameters Is Nothing Then
    ' Do Nothing
    'Else
    'm_oDatabase.Parameters.Clear()
    'End If
    '
    ' Create New Parameter Collection
    'm_oParameters = Nothing
    'm_oParameters = New dPMDAO.Parameters()
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Clear Parameters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearParameters", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim oDatabase As dPMDAO.Database



        result = gPMConstants.PMEReturnCode.PMTrue


        If gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_sUnderwritingOrAgency = "A"

        m_oDatabase.Parameters.Clear()

        sSQL = "SELECT value FROM hidden_options WHERE option_number = 1"

        m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetHiddenOption", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
            ' Carry on without default set
        End If

        If oDatabase.Records.Count() = 1 Then
            ' select first letter of the return field
            m_sUnderwritingOrAgency = gPMFunctions.NullToString(oDatabase.Records.Item(0).Fields()("value")).Substring(0, 1)
        End If

        If (m_sUnderwritingOrAgency <> "A") And (m_sUnderwritingOrAgency <> "U") Then
            m_sUnderwritingOrAgency = "A"
        End If

        m_lReturn = oDatabase.CloseDatabase()

        oDatabase = Nothing

        Return result

    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

