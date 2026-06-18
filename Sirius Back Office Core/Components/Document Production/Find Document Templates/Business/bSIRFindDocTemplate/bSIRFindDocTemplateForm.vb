Option Strict Off
Option Explicit On
'developer guide no. 129 (guide)
Imports System.Text
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 07/10/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, business rules required for the
    '              SIRFindInsurance summary form.
    '
    ' Edit History:
    ' TF071098 -    Created from bFindInsurance
    ' SJP14062002 moved to uniform Product Options scheme and gSIRLibrary.bas
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 12/01/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' ************************************************
    ' Added to replace global variables 26/11/2003
    ' Username.

    ' Password.

    ' User ID

    ' Calling Application
    ' Source ID
    ' Language ID
    ' Currency ID
    ' LogLevel
    ' ************************************************



    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lError As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Document Template ID
    Private m_lDocumentTemplateId As Integer

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    Private m_lSourceId As Integer

    Private m_sUnderwritingOrAgency As String = ""

    Private m_sCoverToDate As String = ""

    Private m_bViaClientManager As Boolean

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

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

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property


    Public Property DocumentTemplateId() As Integer
        Get

            Return m_lDocumentTemplateId

        End Get
        Set(ByVal Value As Integer)

            m_lDocumentTemplateId = Value

        End Set
    End Property

    Public Property SourceId() As Integer
        Get

            Return m_lSourceId

        End Get
        Set(ByVal Value As Integer)

            m_lSourceId = Value

        End Set
    End Property

    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property

    Public WriteOnly Property ViaClientManager() As Boolean
        Set(ByVal Value As Boolean)

            m_bViaClientManager = Value

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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


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


            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Check that we have the right Database for our
            ' product Family

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            Me.disposedValue = True
            If disposing Then
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                End If
                m_oLookup = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


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


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchByQuery
    '
    ' Description: SQL Query to Select Document Template details
    '
    ' ***************************************************************** '
    'MKW190903 PN6943 Added Valid Source Array for current User
    'MKW060204 PN10162 Added parameters/functionality to exclude deleted document templates.
    'developer guide no.101
    Public Function SearchByQuery(ByRef r_vResultArray(,) As Object, Optional ByVal v_vDocumentCode As Object = Nothing, Optional ByVal v_vDocumentTypeId As Object = Nothing, Optional ByVal v_vValidSourceArray As Object = Nothing, Optional ByVal v_bIncludeDeleted As Boolean = True, Optional ByVal v_bIncludeSubDocuments As Boolean = True, Optional ByVal v_lExcludeCopyTempalte As Integer = 0, Optional ByVal v_dtEffectiveDate As Date = #12/30/1899#, Optional ByVal v_lNumberOfRecords As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder


        ' Developer Guide No 21


        Dim nLower As Integer 'MKW190903 PN6943 
        Dim nUpper As Integer 'MKW190903 PN6943 
        'MKW190903 PN6943

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build the SQL select statement according to the parameters passed
            ' Select statement to select all details relating to values entered
            sSQL = New StringBuilder("")
            sSQL.Append("SELECT dte.document_template_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("dte.code," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("dte.description," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("dty.document_type_id," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("dty.code," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("dte.is_deleted," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("c.caption," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("dte.effective_date" & Strings.ChrW(13) & Strings.ChrW(10))

            sSQL.Append("FROM document_template dte," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("document_type dty," & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("PMCaption c" & Strings.ChrW(13) & Strings.ChrW(10))

            sSQL.Append("WHERE dte.document_type_id = dty.document_type_id" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("AND dty.caption_id = c.caption_id" & Strings.ChrW(13) & Strings.ChrW(10))
            sSQL.Append("AND c.language_id = " & m_iLanguageID & Strings.ChrW(13) & Strings.ChrW(10))
            'DJM 03/02/2004 : Dummy record should be suppressed as we don't want it to be used or deleted.
            sSQL.Append("AND dte.document_template_id <> 100000 " & Strings.ChrW(13) & Strings.ChrW(10))

            'append the parameters to the where clause

            'DJM 04/02/2004 : If multi-company then only show templates for the logged in branch or for all.
            'MKW190903 PN6943 START
            If Informations.IsArray(v_vValidSourceArray) Then
                nLower = v_vValidSourceArray.GetLowerBound(1)
                nUpper = v_vValidSourceArray.GetUpperBound(1)

                sSQL.Append("AND" & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" ((dte.source_id IN (" & m_iSourceID & ",0)" & Strings.ChrW(13) & Strings.ChrW(10))
                sSQL.Append(" AND EXISTS(SELECT NULL FROM hidden_options WHERE option_number = 16))")
                sSQL.Append("OR" & Strings.ChrW(13) & Strings.ChrW(10))


                For iLoop As Integer = nLower To nUpper
                    If iLoop = nLower Then
                        sSQL.Append(" (dte.source_id IN (0,")
                    End If

                    'developer guide no.111
                    sSQL.Append(CStr(Val(CStr(v_vValidSourceArray(0, iLoop)))))

                    If iLoop = nUpper Then
                        sSQL.Append(")" & Strings.ChrW(13) & Strings.ChrW(10))
                        sSQL.Append(" AND NOT EXISTS(SELECT NULL FROM hidden_options WHERE option_number = 16)))")
                    Else
                        sSQL.Append(",")
                    End If
                Next
            Else
                sSQL.Append("AND dte.source_id IN (" & m_iSourceID & ",0)" & Strings.ChrW(13) & Strings.ChrW(10))
            End If
            'MKW190903 PN6943 END


            If Not Informations.IsNothing(v_vDocumentCode) Then
                If (v_vDocumentCode <> "") And (v_vDocumentCode <> "%") Then
                    'document type is present
                    'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (Not mentioned in the tech spec)
                    'removed the concatination of "%" at the end of the shortname
                    'sSql = sSql & "AND dte.code LIKE '" & CStr(v_vDocumentCode) & "%'" & vbCrLf
                    sSQL.Append("AND dte.code LIKE '" & v_vDocumentCode & "'" & Strings.ChrW(13) & Strings.ChrW(10))
                    'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (Not mentioned in the tech spec)
                End If
            End If


            If Not Informations.IsNothing(v_vDocumentTypeId) Then
                'Developer Guide No 260
                If (CStr(v_vDocumentTypeId) <> "") AndAlso (CStr(v_vDocumentTypeId) <> "0") AndAlso (CStr(v_vDocumentTypeId) <> "%") Then
                    'document type is present
                    sSQL.Append("AND dty.document_type_id = " & CInt(v_vDocumentTypeId) & Strings.ChrW(13) & Strings.ChrW(10))
                End If
            End If

            'MKW060204 PN10162 Exclude deleted Doc Templates START
            If Not False Then
                If Not v_bIncludeDeleted Then
                    sSQL.Append("AND dte.is_deleted <> 1" & Strings.ChrW(13) & Strings.ChrW(10))
                End If
            End If
            'MKW060204 PN10162 Exclude deleted Doc Templates END

            ' RAM20050107 : Code added to support SubDocument
            If Not False Then
                If Not v_bIncludeSubDocuments Then
                    sSQL.Append("AND dty.code <> 'SUBDOC'" & Strings.ChrW(13) & Strings.ChrW(10))
                End If
            End If

            If v_lExcludeCopyTempalte = 1 Then
                sSQL.Append("AND IsNull(dte.copy_of_original,0) = 0" & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            If Not (v_dtEffectiveDate = #12/30/1899#) Then
                sSQL.Append("AND dte.effective_date >= '" & v_dtEffectiveDate.ToString("yyyy/MM/dd") & "'" & Strings.ChrW(13) & Strings.ChrW(10))
            End If

            If m_bViaClientManager Then
                sSQL.Append("AND dte.is_visible_from_client_manager = 1" & Strings.ChrW(13) & Strings.ChrW(10))
            End If


            'add the order by clause
            sSQL.Append("ORDER BY dte.code")

            ' Execute SQL Statement - use array for speed
            With m_oDatabase

                .Parameters.Clear()
                'If v_lNumberOfRecords <> -1 Then
                m_lError = .SQLSelect(sSQL:=sSQL.ToString(), sSQLName:=ACDocTemplateFromQueryName, bStoredProcedure:=ACDocTemplateFromQueryStored, lNumberRecords:=v_lNumberOfRecords, vResultArray:=r_vResultArray)
                'Else
                'm_lError = .SQLSelect(sSQL:=sSQL.ToString(), sSQLName:=ACDocTemplateFromQueryName, bStoredProcedure:=ACDocTemplateFromQueryStored, vResultArray:=r_vResultArray)
                'End If

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If NO records were found return PMFalse
                If Not Informations.IsArray(r_vResultArray) Then
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchByQuery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByQuery", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchByRiskType
    '
    ' Description: SQL Query to Select Document Template details
    '
    ' History :
    '               ??/??/????  ??? Created
    '               06/03/2003  APS Amended to accept a parameter of code
    '               20030624 - CTAF Fixed loads of junk in here
    '               31/05/2005 RKS Added lGISObjectID optional parameter
    ' ***************************************************************** '
    Public Function SearchByRiskType(ByRef r_vResultArray(,) As Object, ByVal v_lRiskTypeId As Integer, ByVal v_dtEffectiveDate As Date, Optional ByVal v_vDocumentCode As Object = Nothing, Optional ByVal lGISPropertyID As Integer = 0, Optional ByVal lGISObjectID As Integer = 0, Optional ByVal v_lNumberOfRecords As Integer = -1,
                                 Optional ByVal v_lBranchID As Integer = 0,
                                 Optional ByVal v_sPropertyName As String = "",
                                 Optional ByVal v_sColumnName As String = "") As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' CTAF 20030624 - This is an optional parameter, so we should check
            '                 if it's missing

            If Informations.IsNothing(v_vDocumentCode) Then

                'developer guide no. 85(guide)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else

                m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(v_vDocumentCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If lGISPropertyID = 0 Then

                'developer guide no. 85(guide)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_property_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_property_id", vValue:=CStr(lGISPropertyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If lGISObjectID = 0 Then

                'developer guide no. 85(guide)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_object_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_object_id", vValue:=CStr(lGISObjectID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_dtEffectiveDate = #12/30/1899# Then

                'developer guide no. 85(guide)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                'Developer Guide No 40
                m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'developer guide no. 85(guide)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_visible_from_client_manager", vValue:=If(m_bViaClientManager, CStr(1), DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_lBranchID = 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="branch_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="branch_id", vValue:=v_lBranchID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Len(Trim$(v_sPropertyName)) > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="PropertyName", vValue:=v_sPropertyName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Len(Trim$(v_sColumnName)) > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="ColumnName", vValue:=v_sColumnName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If v_lNumberOfRecords = -1 Then
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACDocTemplateFromRiskTypeSQL186, sSQLName:=ACDocTemplateFromRiskTypeName186, bStoredProcedure:=ACDocTemplateFromRiskTypeStored186, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)
            'Else
            '    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACDocTemplateFromRiskTypeSQL186, sSQLName:=ACDocTemplateFromRiskTypeName186, bStoredProcedure:=ACDocTemplateFromRiskTypeStored186, lNumberRecords:=v_lNumberOfRecords, vResultArray:=r_vResultArray)
            'End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchByRiskType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByRiskType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchByProduct
    '
    ' Description: SQL Query to Select Document Template details
    '
    ' ***************************************************************** '
    Public Function SearchByProduct(ByRef r_vResultArray(,) As Object, ByVal v_lProductId As Integer, Optional ByVal v_lBranchId As Integer = 0, Optional ByVal v_lNumberOfRecords As Integer = -1) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'developer guide no. 85(guide)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Branch_Id", vValue:=If(v_lBranchId, CStr(v_lBranchId), DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'developer guide no. 85 (guide)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_visible_from_client_manager", vValue:=If(m_bViaClientManager, CStr(1), DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If v_lNumberOfRecords = -1 Then
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACDocTemplateFromProductSQL, sSQLName:=ACDocTemplateFromProductName, bStoredProcedure:=ACDocTemplateFromProductStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)
            'Else
            '    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACDocTemplateFromProductSQL, sSQLName:=ACDocTemplateFromProductName, bStoredProcedure:=ACDocTemplateFromProductStored, lNumberRecords:=v_lNumberOfRecords, vResultArray:=r_vResultArray)
            'End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchByProduct Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchByProduct", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRDocTemplate.
    '
    '
    ' ***************************************************************** '

    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim dtEffectiveDate As Date
        Dim vTabArray(3, 0) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array

            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "document_type"

            ' {* USER DEFINED CODE (End) *}

            iLookupType = gPMConstants.PMELookupType.PMLookupAll

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.

                Case gPMConstants.PMELookupType.PMLookupSingle

            End Select

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetProcessTypesLookupValues (Public)
    '
    ' Description: Gets the Lookup values for process types docs.
    ' PW160702 - created
    '
    ' ***************************************************************** '
    Public Function GetProcessTypesLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTableArray, iLanguageID:=iLanguageID, dtEffectiveDate:=DateTime.Now, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProcessTypesLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProcessTypesLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProcessTypesDocsSplitStatus (Public)
    '
    ' ***************************************************************** '
    Public Function GetProcessTypesDocsSplitStatus(ByRef lProcessTypesDocsId As Integer, ByRef bDocumentSplit As Boolean) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "select allow_split_documents from process_types_docs where process_types_docs_id={process_types_docs_id}"

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="process_types_docs_id", vValue:=CStr(lProcessTypesDocsId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProcessTypesDocsSplitStatus")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get product_id.
            If Informations.IsArray(vResultArray) Then

                bDocumentSplit = CStr(vResultArray(0, 0)) = "1"
            Else
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProcessTypesDocsSplitStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProcessTypesDocsSplitStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


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
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetReportPointer
    '
    ' Description:
    '
    ' History: 26/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetReportPointer(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lReportPointer As Integer) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Dim vResultArray(,) As Object = Nothing
        Dim lProductId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Construct SQL string to retrieve associated product_id.
            sSQL = "SELECT product_id" & " FROM insurance_file" & " WHERE insurance_file_cnt = " & CStr(v_lInsuranceFileCnt)

            m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportPointer")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get product_id.
            If Informations.IsArray(vResultArray) Then

                lProductId = CInt(vResultArray(0, 0))
            Else
                Return result
            End If

            'Construct SQL string to retrieve report_pointer.
            sSQL = "SELECT report_pointer" & " FROM product" & " WHERE product_id = " & CStr(lProductId)

            m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportPointer")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get report_pointer.
            If Informations.IsArray(vResultArray) Then

                r_lReportPointer = CInt(Val(CStr(vResultArray(0, 0))))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReportPointer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportPointer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetBusinessType
    '
    ' Description:
    '
    ' History: 06/08/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function GetBusinessType(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sBusinessType As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Construct SQL string to retrieve associated business type code.
            sSQL = "SELECT bt.code" & Strings.ChrW(13) & Strings.ChrW(10) & "FROM insurance_file ifi, business_type bt" & Strings.ChrW(13) & Strings.ChrW(10) & "WHERE ifi.insurance_file_cnt = " & CStr(v_lInsuranceFileCnt) & Strings.ChrW(13) & Strings.ChrW(10) & "AND ifi.business_type_id = bt.business_type_id"

            m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetBusinessType", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusinessType")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get business type.
            If Informations.IsArray(vResultArray) Then

                r_sBusinessType = CStr(vResultArray(0, 0)).Trim()
            End If


            'developer guide no. 17
            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusinessType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusinessType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAvailableTemplate
    '
    ' Description: Query's database for particular template by code.
    '               If code not found then business rules are applied
    '               to search for different combinations of the code
    '               components.
    '
    ' History: 26/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetAvailableTemplate(ByVal v_sTemplateCode As String, ByRef r_lTemplateId As Integer, ByRef r_lTemplateTypeId As Integer, ByRef r_sDocDescription As String, Optional ByVal v_dtEffectiveDate As Date = #12/30/1899#) As Integer

        Dim result As Integer = 0
        Dim sDocTypeCode, sTransactionCode, sReportPointer As String
        Dim lTemplateId, lTemplateTypeId As Integer
        Dim sSearchTemplateCode As String = ""
        Dim iSearchCount As Integer
        Dim sDocDescription As String = ""

        Const iMAX_TEMPLATE_SEARCHES As Integer = 5

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            v_sTemplateCode = v_sTemplateCode.Trim()

            sDocTypeCode = v_sTemplateCode.Substring(0, 3)

            'If this is a renewal then no Transaction Code is present.
            If sDocTypeCode = "RNC" Then
                sTransactionCode = ""
                sReportPointer = v_sTemplateCode.Substring(3)
            Else
                sTransactionCode = v_sTemplateCode.Substring(3, Math.Min(v_sTemplateCode.Length, 2))
                sReportPointer = v_sTemplateCode.Substring(5)
            End If

            lTemplateId = 0

            'Apply rules until template is found.
            Do While (lTemplateId = 0) And (iSearchCount < iMAX_TEMPLATE_SEARCHES)
                iSearchCount += 1
                Select Case (iSearchCount)
                    Case 1
                        sSearchTemplateCode = v_sTemplateCode
                    Case 2
                        sSearchTemplateCode = sDocTypeCode & sTransactionCode
                    Case 3
                        sSearchTemplateCode = sDocTypeCode & sReportPointer & sTransactionCode
                    Case 4
                        sSearchTemplateCode = sDocTypeCode & sReportPointer
                    Case 5
                        sSearchTemplateCode = sDocTypeCode
                End Select


                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=sSearchTemplateCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="document_template_id", vValue:=CStr(lTemplateId), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="document_type_id", vValue:=CStr(lTemplateTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'AJM 8/3/01 - get document description
                m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=sDocDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

                'Developer Guide No.40
                'm_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTimeHelper.ToString(v_dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetDocumentTemplateSQL, sSQLName:=ACGetDocumentTemplateName, bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If Convert.IsDBNull(m_oDatabase.Parameters.Item("document_template_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("document_template_id").Value) Then
                    lTemplateId = 0
                Else
                    lTemplateId = m_oDatabase.Parameters.Item("document_template_id").Value
                End If

            Loop

            'Check that a template has been found.
            If lTemplateId = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set return values.
            r_lTemplateId = lTemplateId

            If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("document_type_id").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("document_type_id").Value)) Then
                r_lTemplateTypeId = m_oDatabase.Parameters.Item("document_type_id").Value
            End If

            'AJM 08/03/01 get description

            If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("description").Value) Or Informations.IsNothing(m_oDatabase.Parameters.Item("description").Value)) Then
                r_sDocDescription = m_oDatabase.Parameters.Item("description").Value
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAvailableTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAvailableTemplate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' History:
    ' 14/06/2002 SP - moved to uniform Product Options scheme
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0


        Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetFutureDatedTemplate
    '
    ' Description: Check that future dated template has been found by code and effectivedate.
    '
    ' History: 10/05/2007 VB - Created.
    '
    ' ***************************************************************** '
    Public Function GetFutureDatedTemplate(ByVal v_sTemplateCode As String, ByVal v_dtEffectiveDate As Date) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sTemplateCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'Developer Guide No.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetFutureDatedTemplateSQL, sSQLName:=ACGetFutureDatedTemplateName, bStoredProcedure:=ACGetFutureDatedTemplateStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Check that future dated template has been found.
            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFutureDatedTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFutureDatedTemplate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'Start-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)-(5.2.1)
    'This method will fetch all the clauses that are attached to the particular ProductType or RiskType
    'Start Arul -Bug Fixing PN 55217
    'Note:Two optional parameters(v_sPropertyName,v_sColumnName) are added

    Private Function GetClauses(ByVal v_lClauseType As Integer, ByVal v_lRiskType As Integer, ByVal v_lProduct_id As Integer, ByVal v_lBranch_Id As Integer, ByRef r_vResultArray(,) As Object, Optional ByVal v_sPropertyName As String = "", Optional ByVal v_sColumnName As String = "", Optional ByVal v_sCode As String = "") As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "LoadClauses"
        ' Dim vBranches As Object
        'Dim lCount As Integer
        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()
            If v_lClauseType = MainModule.ENClauseType.RiskType Then

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Type_ID", vValue:=CStr(v_lRiskType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Adding the parameter(Risk_Type_ID) failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Branch_ID", vValue:=CStr(v_lBranch_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Adding the parameter(Branch_ID) failed", gPMConstants.PMELogLevel.PMLogError)
                End If



                If Not String.IsNullOrEmpty(v_sPropertyName) And Not (Convert.IsDBNull(v_sPropertyName) Or Informations.IsNothing(v_sPropertyName)) And v_sPropertyName.Trim() <> "" Then
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="PropertyName", vValue:=v_sPropertyName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Adding the parameter(PropertyName) failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="ColumnName", vValue:=v_sColumnName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Adding the parameter(ColumnName) failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If


                If gPMFunctions.ToSafeString(v_sCode).Trim() <> "" Then
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Adding the parameter(PropertyName) failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELRisktypeTypeLinkedClausesSQL, sSQLName:=ACSELRisktypeTypeLinkedClausesSQLName, bStoredProcedure:=AcSELRiskORProductStored, vResultArray:=r_vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACSELRisktypeTypeLinkedClausesSQL & " is Failed to fetch the clauses", gPMConstants.PMELogLevel.PMLogError)
                End If

            ElseIf v_lClauseType = MainModule.ENClauseType.ProductType Then

                'Add Required Stored Procedure Parameters

                m_lReturn = m_oDatabase.Parameters.Add(sName:="product_ID", vValue:=CStr(v_lProduct_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Adding the parameter(Product_Id) failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Branch_ID", vValue:=CStr(v_lBranch_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Adding the parameter (Branch_ID) failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Not (m_sCoverToDate = "") Then
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="CoverToDate", vValue:=CStr(m_sCoverToDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Adding the parameter (CoverToDate) failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                If Trim(ToSafeString(v_sCode)) <> "" Then
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="SearchCode", _
                                                    vValue:=v_sCode, _
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                    iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Adding the parameter(SearchCode) failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If


                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELProductTypeLinkedClausesSQL, sSQLName:=ACSELProductTypeLinkedClausesSQLName, bStoredProcedure:=AcSELRiskORProductStored, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACSELProductTypeLinkedClausesSQL & " is Failed to fetch the clauses", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function

    ''' <summary>
    ''' To get list of linked clauses on the basis of product and branch
    ''' </summary>
    ''' <param name="v_lClauseType">To find come for Risk type or product type</param>
    ''' <param name="v_lRiskType">Id of the risk</param>
    ''' <param name="v_lProduct_id">Id of the product</param>
    ''' <param name="v_lBranch_Id">Id of the branch</param>
    ''' <param name="r_vResultArray">Array to hold resulted data</param>
    ''' <param name="v_sPropertyName">Name of the property</param>
    ''' <param name="v_sColumnName">Name of the column to get specifically</param>
    ''' <param name="v_sCode">Default code</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LoadClauses(ByVal v_lClauseType As Integer, ByVal v_lRiskType As Integer, ByVal v_lProduct_id As Integer, ByVal v_lBranch_Id As Integer, ByRef r_vResultArray(,) As Object, Optional ByVal v_sPropertyName As String = "", Optional ByVal v_sColumnName As String = "", Optional ByVal v_sCode As String = "") As Integer

        Return GetClauses(v_lClauseType, v_lRiskType, v_lProduct_id, v_lBranch_Id, r_vResultArray, v_sPropertyName, v_sColumnName, v_sCode)

    End Function

    ''' <summary>
    ''' Overloaded methd to get list of linked clauses on the basis of product, branch and effective dates
    ''' </summary>
    ''' <param name="v_lClauseType">To find come for Risk type or product type</param>
    ''' <param name="v_lRiskType">Id of the risk</param>
    ''' <param name="v_lProduct_id">Id of the product</param>
    ''' <param name="v_lBranch_Id">Id of the branch</param>
    ''' <param name="r_vResultArray">Array to hold resulted data</param>
    ''' <param name="v_sCoverToDate">Cover End Date to get data on the basis of effective date</param>
    ''' <param name="v_sPropertyName">Name of the property</param>
    ''' <param name="v_sColumnName">Name of the column to get specifically</param>
    ''' <param name="v_sCode">Default code</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LoadClauses(ByVal v_lClauseType As Integer, ByVal v_lRiskType As Integer, ByVal v_lProduct_id As Integer, ByVal v_lBranch_Id As Integer, ByRef r_vResultArray(,) As Object, ByVal v_sCoverToDate As String, Optional ByVal v_sPropertyName As String = "", Optional ByVal v_sColumnName As String = "", Optional ByVal v_sCode As String = "") As Integer

        m_sCoverToDate = v_sCoverToDate
        Return GetClauses(v_lClauseType, v_lRiskType, v_lProduct_id, v_lBranch_Id, r_vResultArray, v_sPropertyName, v_sColumnName, v_sCode)

    End Function

    'End-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)-(5.2.1)
End Class

