Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'Developer Guide No.129
Imports System.Text
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    '*******************************************************************************
    ' Class Name: Business
    '
    ' Date: 07/05/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRDocSpooler.
    '
    ' Edit History:
    '
    '   SJP14062002 - getUnderWritingOrAgency use new product options methods
    '   PW140105 - PN18078 - changes required for new Document Template ID field
    '              being added to the document_spooler table.
    '   CJB31032005 - PN19682 Changed GetUsers to get list of users that have a spooler entry.
    '                 Do not link to event_log as was being done previously for some reason.
    '*******************************************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' ************************************************
    ' Added to replace global variables 09/01/2004
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************
    Private m_oSIRDocSpoolers As bSIRDocSpooler.SIRDocSpoolers

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    ' Current Record Pointer
    Private m_lCurrentRecord As Integer
    ' Error Code (Private)
    'Developer Guide No. 15
    Private m_lReturn As Long
    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private lPMAuthorityLevel As Integer
    ' Primary Keys to work with
    Private m_lDocumentSpoolerId As Integer
    Private m_lDocumentTypeId As Integer
    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business
    ' PM System Option Business Component (Private)
    Private m_oSystemOption As bSIROptions.Business
    ' PM Event Business Component (Private)
    Private m_oEvent As bSIREvent.Business

    ' CTAF 240700 - Instance of bSIRParty
    Private m_oParty As bSIRParty.Business
    Private m_sUnderwritingOrAgency As String = ""

    Public ReadOnly Property UnderwritingOrAgency() As String
        Get
            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If
            Return m_sUnderwritingOrAgency
        End Get
    End Property
    Public ReadOnly Property Party() As Object
        Get
            Return m_oParty
        End Get
    End Property
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property
    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            Value = Value
        End Set
    End Property
    Public Property CurrentRecord() As Integer
        Get
            Return m_lCurrentRecord
        End Get
        Set(ByVal Value As Integer)
            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oSIRDocSpoolers.Count()
                    m_lCurrentRecord = m_oSIRDocSpoolers.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select
        End Set
    End Property
    Public ReadOnly Property RecordCount() As Integer
        Get
            ' Return Number in Collection
            Return m_oSIRDocSpoolers.Count()
        End Get
    End Property
    Public Property DocumentSpoolerId() As Integer
        Get
            Return m_lDocumentSpoolerId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentSpoolerId = Value
        End Set
    End Property
    Public Property DocumentTypeId() As Integer
        Get
            Return m_lDocumentTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lDocumentTypeId = Value
        End Set
    End Property

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


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create SIRDocSpoolers Collection
            m_oSIRDocSpoolers = New bSIRDocSpooler.SIRDocSpoolers()
            'Developer Guide No. 97
            m_lReturn = m_oSIRDocSpoolers.Initialise(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, sCallingAppName)

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            ' CTAF 240700 - Create an instance of bSIRParty
            'LateBinding
            m_oParty = New bSIRParty.Business
            m_lReturn = m_oParty.Initialise(sUsername:=ToSafeString(sUsername), sPassword:=ToSafeString(sPassword), iUserID:=ToSafeInteger(iUserID), iSourceID:=ToSafeInteger(iSourceID), iLanguageID:=ToSafeInteger(iLanguageID), iCurrencyID:=ToSafeInteger(iCurrencyID), iLogLevel:=ToSafeInteger(iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=DirectCast(m_oDatabase, dPMDAO.Database))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get an instance of bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Remove the instance of component services

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oParty IsNot Nothing Then
                    m_oParty.Dispose()
                    m_oParty = Nothing
                End If
                If m_oEvent IsNot Nothing Then
                    m_oEvent.Dispose()
                    m_oEvent = Nothing
                End If
                If m_oSystemOption IsNot Nothing Then
                    m_oSystemOption.Dispose()
                    m_oSystemOption = Nothing
                End If
                m_oSIRDocSpoolers = Nothing
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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRDocSpooler.
    '
    '
    ' ***************************************************************** '
    'Developer Guide No.71
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oSIRDocSpooler As bSIRDocSpooler.SIRDocSpooler = Nothing
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 0) As Object
        Dim vDocumentTypeId As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vResultArray = Nothing

            vTableArray = Nothing
            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "document_type"


            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oSIRDocSpooler = m_oSIRDocSpoolers.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key
                    'But here we are now passing the type...
                    If DocumentTypeId = 0 Then

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""
                    Else

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = DocumentTypeId
                    End If

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oSIRDocSpooler

                        m_lReturn = CType(.GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vDocumentTypeId:=vDocumentTypeId), gPMConstants.PMEReturnCode)

                        If Convert.IsDBNull(vDocumentTypeId) Or Informations.IsNothing(vDocumentTypeId) Then

                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""
                        Else


                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vDocumentTypeId
                        End If

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oSIRDocSpooler

                        m_lReturn = CType(.GetProperties(iStatus:=gPMConstants.PMEComponentAction.PMView, vDocumentTypeId:=vDocumentTypeId), gPMConstants.PMEReturnCode)


                        If Convert.IsDBNull(vDocumentTypeId) Or Informations.IsNothing(vDocumentTypeId) Then

                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""
                        Else


                            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vDocumentTypeId
                        End If

                    End With

            End Select

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release SIRDocSpooler reference
            oSIRDocSpooler = Nothing

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
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIRDocSpooler directly into the database.
    '        Note: The SIRDocSpooler will NOT be added to the collection.
    '
    ' Hist : TN20010730 - add printer name
    '      : sj 15/10/2002 - Add spool level ind
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function DirectAdd(Optional ByRef vDocumentSpoolerId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vIsEditable As Object = Nothing, Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vTimesPrinted As Object = Nothing, Optional ByRef vTimesArchived As Object = Nothing, Optional ByRef vPrinter As Object = Nothing, Optional ByRef vSpoolLevelInd As Object = Nothing, Optional ByRef vDocumentTemplateID As Object = Nothing, Optional ByVal v_iIsClient As Object = Nothing, Optional ByVal v_iIsAgent As Object = Nothing, Optional ByVal v_iIsOffice As Object = Nothing, Optional ByVal v_iOrderByProductionOrder As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRDocSpooler As bSIRDocSpooler.SIRDocSpooler

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRDocSpooler
            oSIRDocSpooler = New bSIRDocSpooler.SIRDocSpooler()
            m_lReturn = CType(oSIRDocSpooler.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate SIRDocSpooler Attributes

            'Developer Guide No 98
            m_lReturn = CType(oSIRDocSpooler.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vDocumentSpoolerId:=vDocumentSpoolerId, vDocumentTypeId:=vDocumentTypeId, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDescription:=vDescription, vIsDeletable:=vIsDeletable, vIsEditable:=vIsEditable, vCreatedById:=vCreatedById, vDateCreated:=vDateCreated, vModifiedById:=vModifiedById, vLastModified:=vLastModified, vTimesPrinted:=vTimesPrinted, vTimesArchived:=vTimesArchived, vPrinter:=vPrinter, vSpoolLevelInd:=vSpoolLevelInd, vSourceId:=m_iSourceID, vDocumentTemplateID:=vDocumentTemplateID, v_iIsClient:=v_iIsClient, v_iIsAgent:=v_iIsAgent, v_iIsOffice:=v_iIsOffice, v_iOrderByProductionOrder:=v_iOrderByProductionOrder), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRDocSpooler = Nothing
                Return result
            End If

            ' Add the SIRDocSpooler to the Database
            m_lReturn = CType(oSIRDocSpooler.AddItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRDocSpooler = Nothing
                Return result
            End If

            ' Retain the Primary Key of the SIRDocSpooler Added
            With oSIRDocSpooler
                DocumentSpoolerId = .DocumentSpoolerId
            End With

            vDocumentSpoolerId = DocumentSpoolerId

            oSIRDocSpooler = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single SIRDocSpooler directly from the database.
    '        Note: The SIRDocSpooler will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vDocumentSpoolerId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRDocSpooler As bSIRDocSpooler.SIRDocSpooler

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRDocSpooler
            oSIRDocSpooler = New bSIRDocSpooler.SIRDocSpooler()
            m_lReturn = CType(oSIRDocSpooler.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Set SIRDocSpooler Primary Key

            m_lReturn = CType(oSIRDocSpooler.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vDocumentSpoolerId:=CInt(vDocumentSpoolerId)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRDocSpooler = Nothing
                Return result
            End If

            ' Delete the SIRDocSpooler from the Database
            m_lReturn = CType(oSIRDocSpooler.DeleteItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRDocSpooler = Nothing
                Return result
            End If

            oSIRDocSpooler = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            'Developer Guide No 98
            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=vID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckCode (Public)
    '
    ' Description: Checks to see if the supplied code already exists.
    '
    ' ***************************************************************** '
    Public Function CheckCode(ByRef vCode As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(vCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckCodeSQL, sSQLName:=ACCheckCodeName, bStoredProcedure:=ACCheckCodeStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required SIRDocSpoolers and populate the Collection
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function GetDetails(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0, Optional ByRef vDocumentSpoolerId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'Developer Guide No 22
        Dim oFields As DataRow
        Dim oSIRDocSpooler As bSIRDocSpooler.SIRDocSpooler
        Dim sSQL As String = ""
        Dim bDone As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSIRDocSpoolers.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Check for Valid Primary Key
            Dim dbNumericTemp2 As Double

            If (Not Informations.IsNothing(vDocumentSpoolerId)) And (Not Double.TryParse(CStr(vDocumentSpoolerId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vDocumentSpoolerId=" & vDocumentSpoolerId, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Informations.IsNothing(vDocumentSpoolerId) Then

                ' Create New SIRDocSpooler
                oSIRDocSpooler = New bSIRDocSpooler.SIRDocSpooler()
                m_lReturn = CType(oSIRDocSpooler.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                ' Set component primary keys
                With oSIRDocSpooler
                    .DocumentSpoolerId = vDocumentSpoolerId

                    m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                ' Add SIRDocSpooler to collection
                If m_oSIRDocSpoolers.Count = 0 Then
                    m_oSIRDocSpoolers.Add(Nothing)
                End If
                m_lReturn = CType(m_oSIRDocSpoolers.Add(oNewSIRDocSpooler:=oSIRDocSpooler), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSIRDocSpooler = Nothing

            Else

                ' No Key, Get All Records for the parameters passed
                sSQL = "SELECT document_template_id FROM document_template"
                bDone = False

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = m_oDatabase.Records.Count()

                ' Do we have any records ?
                If lRecordCount < 1 Then
                    ' No Records, return PMFalse
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Yes, load them into the collection
                For lSub As Integer = 1 To lRecordCount

                    ' Create New
                    oSIRDocSpooler = New bSIRDocSpooler.SIRDocSpooler()
                    'Developer Guide No 9
                    m_lReturn = oSIRDocSpooler.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

                    ' Set oFields to refer to one Record
                    'Developer Guide No 162
                    oFields = m_oDatabase.Records.Item(lSub - 1).Fields()

                    ' Set component primary keys from current record
                    With oSIRDocSpooler
                        .DocumentSpoolerId = gPMFunctions.NullToLong(oFields("document_template_id"))

                        m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End With

                    ' Add SIRDocSpooler to collection
                    If m_oSIRDocSpoolers.Count = 0 Then
                        m_oSIRDocSpoolers.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oSIRDocSpoolers.Add(oNewSIRDocSpooler:=oSIRDocSpooler), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oSIRDocSpooler = Nothing
                Next lSub
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetOtherDetails (Public)
    '
    ' Description: Gets the required information about slots...
    '
    ' ***************************************************************** '
    'Developer Guide No 17
    Public Function GetOtherDetails(ByRef vClientArray(,) As Object, ByRef vPolicyArray(,) As Object, ByRef vDocumentTypeArray(,) As Object, ByRef vSourceArray(,) As Object, ByRef vRiskBySource As Object, ByRef vRiskGroupBySource As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim vArray(,) As Object = Nothing
        Dim lSourceId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = New StringBuilder("SELECT tfd.slot_number, " &
                   "tfd.description " &
                   "FROM text_file_description tfd, " &
                   "entity_type et " &
                   "WHERE source_id = " & CStr(m_iSourceID) & " " &
                   "AND tfd.entity_type_id = et.entity_type_id " &
                   "AND et.code = 'CLIENT'")

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="GetClientTexts", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vClientArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = New StringBuilder("SELECT tfd.slot_number, " &
                   "tfd.description " &
                   "FROM text_file_description tfd, " &
                   "entity_type et " &
                   "WHERE source_id = " & CStr(m_iSourceID) & " " &
                   "AND tfd.entity_type_id = et.entity_type_id " &
                   "AND et.code = 'POLICY'")

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="GetPolicyTexts", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vPolicyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = New StringBuilder("SELECT document_type_id, is_editable_after_merging " &
                   "FROM document_type ")

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="GetEditable", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vDocumentTypeArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get sources
            sSQL = New StringBuilder("SELECT source_id, code, description" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "FROM source" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "WHERE is_deleted = 0" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "UNION" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "SELECT 0, '(All)', 'All branches'" & Strings.ChrW(13) & Strings.ChrW(10) &
                   "ORDER BY 2" & Strings.ChrW(13) & Strings.ChrW(10))

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="GetSources", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vSourceArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ReDim vRiskBySource(vSourceArray.GetUpperBound(1))
            ReDim vRiskGroupBySource(vSourceArray.GetUpperBound(1))

            For lTemp As Integer = vSourceArray.GetLowerBound(1) To vSourceArray.GetUpperBound(1)


                lSourceId = CInt(vSourceArray(0, lTemp))

                'Get risks for this source

                sSQL = New StringBuilder("SELECT DISTINCT rc.risk_code_id, rc.code, rc.description" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "FROM risk_code rc, risk_by_source rbs" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "WHERE rc.is_deleted = 0" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "AND rbs.risk_group_id = rc.risk_group_id" & Strings.ChrW(13) & Strings.ChrW(10))


                '        If (lSourceId <> 0) Then
                sSQL.Append("AND rbs.source_id IN (0," & lSourceId & ")" & Strings.ChrW(13) & Strings.ChrW(10))
                '        End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="GetRisks", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



                vRiskBySource(lTemp) = vArray

                'Get risk groups for this source

                sSQL = New StringBuilder("SELECT DISTINCT rg.risk_group_id, rg.code, rg.description" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "FROM risk_group rg, risk_by_source rbs" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "WHERE rg.is_deleted = 0" & Strings.ChrW(13) & Strings.ChrW(10) &
                       "AND rbs.risk_group_id = rg.risk_group_id" & Strings.ChrW(13) & Strings.ChrW(10))


                '        If (lSourceId <> 0) Then
                sSQL.Append("AND rbs.source_id IN (0," & lSourceId & ")" & Strings.ChrW(13) & Strings.ChrW(10))
                '        End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="GetRiskGroups", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



                vRiskGroupBySource(lTemp) = vArray

            Next lTemp

            vArray = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOtherDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDescription (Public)
    '
    ' Description: Gets the description for the template...
    '
    ' ***************************************************************** '
    Public Function GetDescription(ByRef lDocumentSpoolerId As Integer, ByRef sDocumentSpoolerDescription As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT description " &
                   "FROM document_template " &
                   "WHERE document_template_id = " & CStr(lDocumentSpoolerId)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sDocumentSpoolerDescription = ""

            If Informations.IsArray(vArray) Then

                'Developer Guide No 149
                sDocumentSpoolerDescription = Convert.ToString(vArray(0, 0)).Trim()
                vArray = Nothing
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDescription Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDescription", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required SIRDocSpoolers and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vDocumentSpoolerId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vIsEditable As Object = Nothing, Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vTimesPrinted As Object = Nothing, Optional ByRef vTimesArchived As Object = Nothing) As Integer


        Dim result As Integer = 0

        Dim oSIRDocSpooler As bSIRDocSpooler.SIRDocSpooler
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSIRDocSpoolers.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oSIRDocSpooler = m_oSIRDocSpoolers.Item(m_lCurrentRecord)

            ' Get the SIRDocSpooler Property Value

            'Developer Guide No 98
            m_lReturn = CType(oSIRDocSpooler.GetProperties(iStatus, vDocumentSpoolerId:=vDocumentSpoolerId, vDocumentTypeId:=vDocumentTypeId, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDescription:=vDescription, vIsDeletable:=vIsDeletable, vIsEditable:=vIsEditable, vCreatedById:=vCreatedById, vDateCreated:=vDateCreated, vModifiedById:=vModifiedById, vLastModified:=vLastModified, vTimesPrinted:=vTimesPrinted, vTimesArchived:=vTimesArchived), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRDocSpooler = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied SIRDocSpooler into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vDocumentSpoolerId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vIsEditable As Object = Nothing, Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vTimesPrinted As Object = Nothing, Optional ByRef vTimesArchived As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRDocSpooler As bSIRDocSpooler.SIRDocSpooler

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oSIRDocSpoolers.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new SIRDocSpooler
            oSIRDocSpooler = New bSIRDocSpooler.SIRDocSpooler()
            m_lReturn = CType(oSIRDocSpooler.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            ' Populate SIRDocSpooler Attributes










            'Developer Guide No 98
            m_lReturn = CType(oSIRDocSpooler.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vDocumentSpoolerId:=vDocumentSpoolerId, vDocumentTypeId:=vDocumentTypeId, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDescription:=vDescription, vIsDeletable:=vIsDeletable, vIsEditable:=vIsEditable, vCreatedById:=vCreatedById, vDateCreated:=vDateCreated, vModifiedById:=vModifiedById, vLastModified:=vLastModified, vTimesPrinted:=vTimesPrinted, vTimesArchived:=vTimesArchived), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRDocSpooler = Nothing
                Return result
            End If

            ' Add SIRDocSpooler to collection
            If m_oSIRDocSpoolers.Count = 0 Then
                m_oSIRDocSpoolers.Add(Nothing)
            End If
            m_lReturn = CType(m_oSIRDocSpoolers.Add(oNewSIRDocSpooler:=oSIRDocSpooler), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRDocSpooler = Nothing
                Return result
            End If

            oSIRDocSpooler = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the SIRDocSpooler
    '              specified and updates the SIRDocSpooler with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vDocumentSpoolerId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vIsEditable As Object = Nothing, Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vTimesPrinted As Object = Nothing, Optional ByRef vTimesArchived As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oSIRDocSpooler As bSIRDocSpooler.SIRDocSpooler
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRDocSpoolers.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSIRDocSpooler = m_oSIRDocSpoolers.Item(lRow)

            ' Check the Status of the SIRDocSpooler

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSIRDocSpooler.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update SIRDocSpooler Attributes










            'Developer Guide No 98
            m_lReturn = CType(oSIRDocSpooler.SetProperties(iStatus:=iStatus, vDocumentSpoolerId:=vDocumentSpoolerId, vDocumentTypeId:=vDocumentTypeId, vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFileCnt:=vInsuranceFileCnt, vClaimCnt:=vClaimCnt, vDescription:=vDescription, vIsDeletable:=vIsDeletable, vIsEditable:=vIsEditable, vCreatedById:=vCreatedById, vDateCreated:=vDateCreated, vModifiedById:=vModifiedById, vLastModified:=vLastModified, vTimesPrinted:=vTimesPrinted, vTimesArchived:=vTimesArchived), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRDocSpooler = Nothing
                Return result
            End If

            ' Release reference to SIRDocSpooler
            oSIRDocSpooler = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified SIRDocSpooler can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oSIRDocSpooler As bSIRDocSpooler.SIRDocSpooler

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRDocSpoolers.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSIRDocSpooler = m_oSIRDocSpoolers.Item(lRow)

            ' Check the Status of the SIRDocSpooler

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oSIRDocSpooler.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oSIRDocSpooler.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oSIRDocSpooler.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to SIRDocSpooler
            oSIRDocSpooler = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oSIRDocSpoolers.Count()
                'Developer Guide No 162
                Select Case m_oSIRDocSpoolers.Item(lSub - 1).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oSIRDocSpooler As bSIRDocSpooler.SIRDocSpooler = Nothing
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oSIRDocSpoolers.Count()
                'Developer Guide No 162
                oSIRDocSpooler = m_oSIRDocSpoolers.Item(lSub - 1)


                Select Case oSIRDocSpooler.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = CType(oSIRDocSpooler.AddItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = CType(oSIRDocSpooler.UpdateItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = CType(oSIRDocSpooler.DeleteItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the SIRDocSpooler
            With oSIRDocSpooler
                DocumentSpoolerId = .DocumentSpoolerId
            End With

            ' Release last reference
            oSIRDocSpooler = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oSIRDocSpoolers.Count()

                        ' With the item
                        With m_oSIRDocSpoolers.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oSIRDocSpoolers.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception




            'KN (CMG) 04/11/2002
            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdatePrinted
    '
    ' Description:
    '
    ' History: 08/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UpdatePrinted(ByRef lDocumentSpoolerId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)
            'Developer Guide No 98
            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_spooler_id", vValue:=lDocumentSpoolerId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdatePrintedSQL, sSQLName:=ACUpdatePrintedName, bStoredProcedure:=ACUpdatePrintedStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePrinted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePrinted", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateArchived
    '
    ' Description:
    '
    ' History: 08/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateArchived(ByRef lDocumentSpoolerId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_spooler_id", vValue:=CStr(lDocumentSpoolerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateArchivedSQL, sSQLName:=ACUpdateArchivedName, bStoredProcedure:=ACUpdateArchivedStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateArchived Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateArchived", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateModified
    '
    ' Description:
    '
    ' History: 08/05/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateModified(ByRef lDocumentSpoolerId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_spooler_id", vValue:=CStr(lDocumentSpoolerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="modified_by_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the ID parameter (INPUT)
            'developer guide no.40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="date_modified", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateModifiedSQL, sSQLName:=ACUpdateModifiedName, bStoredProcedure:=ACUpdateModifiedStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateModified Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateModified", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' Gets everything as an array
    ''' </summary>
    ''' <param name="r_vResultArray"></param>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lClaimCnt"></param>
    ''' <param name="v_sUser"></param>
    ''' <param name="v_lSourceID"></param>
    ''' <param name="v_lAccountHandlerCnt"></param>
    ''' <param name="v_iIsClient"></param>
    ''' <param name="v_iIsAgent"></param>
    ''' <param name="v_iIsOffice"></param>
    ''' <param name="v_iOrderProductionOrder"></param>
    ''' <param name="sYearSelected"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SearchAll(ByRef r_vResultArray(,) As Object, ByVal v_lPartyCnt As Integer,
                              ByVal v_lInsuranceFileCnt As Integer,
                              ByVal v_lClaimCnt As Integer,
                              Optional ByVal v_sUser As String = "",
                              Optional ByVal v_lSourceID As Integer = 0,
                              Optional ByVal v_lAccountHandlerCnt As Integer = 0,
                              Optional ByVal v_iIsClient As Integer = 0,
                              Optional ByVal v_iIsAgent As Integer = 0,
                              Optional ByVal v_iIsOffice As Integer = 0,
                              Optional ByVal v_iOrderProductionOrder As Integer = 0,
                              Optional ByVal sYearSelected As String = "") As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sSQL As String = String.Empty
        Dim sTempSQL As String = String.Empty

        Try

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'We need to get the claim number.  Although an underwriting site will always have
            'a claim table, a broking site may not.  Hence the test for the table and the use of
            'a second query just in case...

            sSQL = ""
            If v_sUser <> "(All)" Then
                ' TB 311002: Get userID for use later
                sSQL = "DECLARE @UID int" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "SELECT @UID = ( SELECT user_id FROM PMUser " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE username = '" & v_sUser & "')" & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            sSQL = sSQL & "SELECT DISTINCT ds.document_spooler_id," & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "ds.document_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "dt.description," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.party_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "p.shortname," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ifi.insurance_folder_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ifi.Insurance_Ref," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.claim_cnt," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "cm.claim_number," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.description," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.is_deletable," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.is_editable," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.created_by_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "u1.username," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.date_created," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.modified_by_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "u2.username," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.date_modified," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.times_printed," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.times_archived," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "dt.Code," & Strings.ChrW(13) & Strings.ChrW(10)
            'PN 28681 Add Agent Column
            sSQL = sSQL & "py.resolved_name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.printer," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.spool_level_ind," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "dtp.code," & Strings.ChrW(13) & Strings.ChrW(10)
            'DC060505 PN20770 show full name not forename
            sSQL = sSQL & "ph.forename + ' ' + php.name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ds.production_order," & Strings.ChrW(13) & Strings.ChrW(10)
            'PN:73866
            sSQL = sSQL & "dtp.document_template_id," & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "dtp.document_template_group_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "dtp.document_template_sub_group_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "dtp.is_portal_internal_only" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM document_spooler ds" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN document_type dt      ON ds.document_type_id = dt.document_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN document_template dtp ON ds.document_template_id = dtp.document_template_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN party p               ON ds.party_cnt = p.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN claim cm              ON ds.claim_cnt = cm.claim_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN pmuser u1             ON ds.created_by_id = u1.user_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN pmuser u2             ON ds.modified_by_id = u2.user_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN insurance_file ifi    ON ds.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN party_handler ph      ON ph.party_cnt = ifi.account_handler_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "LEFT OUTER JOIN party php             ON php.party_cnt = ph.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10)
            'PN 28681 Add Agent Column
            sSQL = sSQL & "LEFT OUTER JOIN  Party py             ON py.party_cnt=ifi.lead_agent_cnt" & Strings.ChrW(13) & Strings.ChrW(10)

            Dim sWhereClause As String = ""
            Dim bConditionFound As Boolean = False

            If v_sUser <> "(All)" Then
                If bConditionFound Then
                    sWhereClause = sWhereClause + " AND ( u1.user_id = @UID OR u2.user_id = @UID )"
                Else
                    sWhereClause = sWhereClause + " ( u1.user_id = @UID OR u2.user_id = @UID )"
                    bConditionFound = True
                End If
            End If

            If v_lPartyCnt <> 0 Then
                If bConditionFound Then
                    sWhereClause = sWhereClause + " AND ds.party_cnt = " & CStr(v_lPartyCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                Else
                    sWhereClause = sWhereClause + " ds.party_cnt = " & CStr(v_lPartyCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                    bConditionFound = True
                End If
            End If

            If v_lInsuranceFileCnt <> 0 Then
                If bConditionFound Then
                    sWhereClause = sWhereClause + " AND ds.insurance_file_cnt = " & CStr(v_lInsuranceFileCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                Else
                    sWhereClause = sWhereClause + " ds.insurance_file_cnt = " & CStr(v_lInsuranceFileCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                    bConditionFound = True
                End If
            End If

            If v_lClaimCnt <> 0 Then
                If bConditionFound Then
                    sWhereClause = sWhereClause + " AND ds.claim_cnt = " & CStr(v_lClaimCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                Else
                    sWhereClause = sWhereClause + " ds.claim_cnt = " & CStr(v_lClaimCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                    bConditionFound = True
                End If
            End If

            If v_lSourceID <> 0 Then
                If bConditionFound Then
                    sWhereClause = sWhereClause + " AND ds.source_id = " & CStr(v_lSourceID) & Strings.ChrW(13) & Strings.ChrW(10)
                Else
                    sWhereClause = sWhereClause + " ds.source_id = " & CStr(v_lSourceID) & Strings.ChrW(13) & Strings.ChrW(10)
                    bConditionFound = True
                End If
            End If


            If sYearSelected <> "" Then
                sWhereClause = sWhereClause + "AND Year(ds.date_created) ='" & sYearSelected & "'" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            If v_lAccountHandlerCnt <> 0 Then
                If bConditionFound Then
                    sWhereClause = sWhereClause + " AND ifi.account_handler_cnt=" & CStr(v_lAccountHandlerCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                Else
                    sWhereClause = sWhereClause + " ifi.account_handler_cnt=" & CStr(v_lAccountHandlerCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                    bConditionFound = True
                End If
            End If

            If sWhereClause.Length > 0 Then
                sSQL = sSQL + "Where " + sWhereClause
            End If
            sTempSQL = ""
            If v_iIsClient = 1 Then
                sTempSQL = " ds.is_client = " & v_iIsClient & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            If v_iIsAgent = 1 Then
                If sTempSQL.Trim().Length > 0 Then sTempSQL = sTempSQL & " OR "
                sTempSQL = sTempSQL & " ds.is_agent = " & CStr(v_iIsAgent) & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            If v_iIsOffice = 1 Then
                If sTempSQL.Trim().Length > 0 Then sTempSQL = sTempSQL & " OR "
                sTempSQL = sTempSQL & " ds.is_office = " & CStr(v_iIsOffice) & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            If sTempSQL.Trim().Length > 0 Then
                sSQL = sSQL & " And (" & sTempSQL & " ) "
            End If


            'add the order by clause
            If v_iOrderProductionOrder = 1 Then
                sSQL = sSQL & "ORDER BY ds.date_modified DESC" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            'Order by Party then production order
            If v_iOrderProductionOrder = 2 Then
                sSQL = sSQL & "ORDER BY p.shortname,ds.production_order" & Strings.ChrW(13) & Strings.ChrW(10)
                'Order by Agent,Party then Production Order
            ElseIf v_iOrderProductionOrder = 3 Then
                sSQL = sSQL & "ORDER BY py.resolved_name,p.shortname,ds.production_order" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateEvent (Private)
    '
    ' Description: Create an event record.
    '
    ' ***************************************************************** '
    Public Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_vInsuranceFolderCnt As Object, ByVal v_vInsuranceFileCnt As Object, ByVal v_vClaimCnt As Object, ByVal v_vDocumentCnt As Object, ByVal v_vOldAddressCnt As Object, ByVal v_vNewAddressCnt As Object, ByVal v_vCampaignId As Object, ByVal v_vDocumentTypeId As Object, ByVal v_vReportTypeId As Object, ByVal v_lEventTypeId As Integer, ByVal v_dtEventDate As Date, Optional ByVal v_vDescription As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oEvent Is Nothing Then
                m_oEvent = New bSIREvent.Business()

                m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the event object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            If r_lEventCnt = 0 Then
                m_lReturn = m_oEvent.DirectAdd(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=v_dtEventDate, vDescription:=v_vDescription)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            Else
                m_lReturn = m_oEvent.DirectUpdate(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=v_dtEventDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oSystemOption Is Nothing Then
                m_oSystemOption = New bSIROptions.Business()

                m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckDuplicates
    '
    ' Description:
    '
    ' History: 22/12/1999 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function CheckDuplicates(ByVal v_lSourceID As Integer, ByVal v_lDocumentId As Integer, ByVal v_lDocumentTypeId As Integer, ByVal v_lSlotNumber As Integer, ByVal v_vRiskCodeId As String, ByVal v_vRiskGroupId As String, ByRef r_bDuplicates As Boolean) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            sSQL = ""
            sSQL = sSQL & "SELECT document_template_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM document_template" & Strings.ChrW(13) & Strings.ChrW(10)

            sSQL = sSQL & "WHERE document_type_id = " & CStr(v_lDocumentTypeId) & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND source_id = " & CStr(v_lSourceID) & Strings.ChrW(13) & Strings.ChrW(10)

            If v_lDocumentId <> 0 Then
                sSQL = sSQL & "AND document_template_id <> " & CStr(v_lDocumentId) & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            sSQL = sSQL & "AND slot_number = " & CStr(v_lSlotNumber) & Strings.ChrW(13) & Strings.ChrW(10)


            If Not (Convert.IsDBNull(v_vRiskCodeId) Or Informations.IsNothing(v_vRiskCodeId)) Then
                sSQL = sSQL & "AND risk_code_id = " & v_vRiskCodeId & Strings.ChrW(13) & Strings.ChrW(10)
            End If


            If Not (Convert.IsDBNull(v_vRiskGroupId) Or Informations.IsNothing(v_vRiskGroupId)) Then
                sSQL = sSQL & "AND risk_group_id = " & v_vRiskGroupId & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckDuplicates", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_bDuplicates = Informations.IsArray(vArray)

            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckDuplicates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDuplicates", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetUsers
    '
    ' Description:
    '
    ' History: 26/11/2001 DavidN - Created.
    '
    ' ***************************************************************** '
    Public Function GetUsers(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing
        Dim bFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Only show users (deleted or not) if they have assigned documents
            sSQL = ""
            sSQL = sSQL & "SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "   DISTINCT u.username" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM pmuser u" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "INNER JOIN" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "document_spooler ds" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " on (ds.created_by_id = u.user_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & " OR ds. modified_by_id = u.user_id)" & Strings.ChrW(13) & Strings.ChrW(10)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetUsers", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then
                bFound = False

                For lLoop As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    If CStr(vArray(0, lLoop)) = m_sUsername Then
                        bFound = True
                    End If
                Next
                If Not bFound Then

                    ReDim Preserve vArray(0, vArray.GetUpperBound(1) + 1)


                    vArray(0, vArray.GetUpperBound(1)) = m_sUsername
                End If
            End If


            r_vResultArray = vArray

            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUsers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUsers", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory(Optional ByRef vDocumentSpoolerId As Object = Nothing, Optional ByRef vDocumentTypeId As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vClaimCnt As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIsDeletable As Object = Nothing, Optional ByRef vIsEditable As Object = Nothing, Optional ByRef vCreatedById As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedById As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vTimesPrinted As Object = Nothing, Optional ByRef vTimesArchived As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    '
    ' {* USER DEFINED CODE (End) *}
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetUnderwritingOrAgency
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
    ' Name: GetAccountHandlers
    '
    ' Description:
    '
    ' History: 26/11/2001 DavidN - Created.
    '
    ' ***************************************************************** '
    Public Function GetAccountHandlers(ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'DC050505 PN20735/PN20740 get acc handlers & exec handlers & get name not forename
            'sSQL = "select * from party_account_handler order by forename"
            sSQL = "Select p.party_cnt, pah.forename + ' ' + p.name " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "from party p " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "join party_type pt " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "on pt.party_type_id = p.party_type_id " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "join party_handler pah " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "on pah.party_cnt = p.party_cnt " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "where pt.code in ( 'AH', 'HC') " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "order by pah.forename"

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountHandlers", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            r_vResultArray = vArray

            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountHandlers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountHandlers", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

