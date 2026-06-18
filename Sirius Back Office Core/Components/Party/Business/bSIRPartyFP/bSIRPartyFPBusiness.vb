Option Strict Off
Option Explicit On
'Developer Guide No 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 16/10/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRPartyFP.
    '
    ' Edit History:
    ' TF161000 - Created
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/02/2004
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
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    ' Link to Party
    Private m_oSIRParty As bSIRParty.Business

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

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



            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            m_oSIRParty = New bSIRParty.Business
            m_lReturn = m_oSIRParty.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oLookup = New BPMLOOKUP.Business
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

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
                If m_oSIRParty IsNot Nothing Then
                    m_oSIRParty.Dispose()
                End If
                m_oSIRParty = Nothing
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

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single PartyFP record directly into the database.
    '
    ' DD 24/07/2003: Rewritten without data object.
    ' ***************************************************************** '
    Public Function DirectAdd(ByRef r_lPartyCnt As Integer, ByVal vFinanceProviderNumber As Integer, ByVal vAgencyNumber As String, ByVal vMailboxNumber As String, ByVal vPFEDIDefinitionID As Integer, ByVal vDOB As Integer, ByVal vCompanyReg As Integer, ByVal vShortName As Object, ByVal vName As Object, ByVal vCurrencyID As Object, ByVal vSourceID As Object, ByVal vSubBranchID As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim lPartyTypeID As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get party type id for a Finance Provider

            m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:=gSIRLibrary.SIRLookupPartyType, v_sCode:=gSIRLibrary.SIRPartyTypeFinanceProvider, v_dtEffectiveDate:=DateTime.Now, r_lID:=lPartyTypeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Add the Party

            m_lReturn = m_oSIRParty.DirectAdd(vPartyTypeID:=lPartyTypeID, vShortname:=vShortName, vName:=vName, vResolvedName:=vName, vCurrencyId:=vCurrencyID, vSourceID:=vSourceID, vSubBranchId:=vSubBranchID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Get the new PartyCnt

            r_lPartyCnt = m_oSIRParty.PartyCnt

            'Add the Finance Provider details
            With m_oDatabase
                'Set up the parameters
                .Parameters.Clear()
                AddKeyParam(r_lPartyCnt)
                AddInputParam(vFinanceProviderNumber, vAgencyNumber, vMailboxNumber, vPFEDIDefinitionID, vDOB, vCompanyReg)

                result = .SQLAction(ACAddSQL, ACAddName, ACAddStored)
            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DirectEdit (Public)
    '
    ' Description: Edits a single PartyFP record directly in the database.
    '
    ' DD 24/07/2003: Added to work without data object.
    ' ***************************************************************** '
    Public Function DirectEdit(ByVal lPartyCnt As Integer, ByVal vFinanceProviderNumber As Integer, ByVal vAgencyNumber As String, ByVal vMailboxNumber As String, ByVal vPFEDIDefinitionID As Integer, ByVal vDOB As Integer, ByVal vCompanyReg As Integer, ByVal vShortName As Object, ByVal vName As Object, ByVal vCurrencyID As Object, ByVal vSourceID As Object, ByVal vSubBranchID As Object, ByVal vPartyID As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                'Set up the parameters
                .Parameters.Clear()
                AddKeyParam(lPartyCnt)
                AddInputParam(vFinanceProviderNumber, vAgencyNumber, vMailboxNumber, vPFEDIDefinitionID, vDOB, vCompanyReg)

                result = .SQLAction(ACEditSQL, ACEditName, ACEditStored)
            End With

            If result = gPMConstants.PMEReturnCode.PMTrue Then
                'Update the Party record

                m_lReturn = m_oSIRParty.GetDetails(vPartyCnt:=lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                'Update the Party record

                m_lReturn = m_oSIRParty.EditUpdate(lRow:=1, vShortname:=vShortName, vName:=vName, vResolvedName:=vName, vCurrencyId:=vCurrencyID, vSourceID:=vSourceID, vSubBranchId:=vSubBranchID, vPartyID:=vPartyID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

                'Update the Party record

                m_lReturn = m_oSIRParty.Update()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single SIRPartyFP directly from the database.
    '
    ' DD 24/07/2003: Rewritten without data object.
    ' ***************************************************************** '
    Public Function DirectDelete(ByRef lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                AddKeyParam(lPartyCnt)

                m_lReturn = .SQLAction(ACDeleteSQL, ACDeleteName, ACDeleteStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Empty stub for backwards compatiblity with
    ' bSIRParty.Services call
    '
    ' DD 24/07/2003: Rewritten without data object.
    ' ***************************************************************** '
    Public Function GetDetails(ByVal vPartyCnt As Object) As Integer

        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the PartyFP data from the database
    '
    ' DD 24/07/2003: Rewritten without data object.
    ' ***************************************************************** '
    Public Function GetNext(ByVal vPartyCnt As Integer, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vFinanceProviderNumber As Object = Nothing, Optional ByRef vAgencyNumber As String = "", Optional ByRef vMailboxNumber As Object = Nothing, Optional ByRef vPFEDIDefinitionID As Object = Nothing, Optional ByRef vDOB As Object = Nothing, Optional ByRef vCompanyReg As Object = Nothing, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                ' Clear the Database Parameters Collection
                .Parameters.Clear()
                AddKeyParam(vPartyCnt)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Get the returned values

                'Developer Guide No 162
                vFinanceProviderNumber = .Records.Item(0).Fields(1)
                ' CJB 220604 Cater for nulls in agency no.

                'Developer Guide No 162
                If Convert.IsDBNull(.Records.Item(0).Fields(2)) Or Informations.IsNothing(.Records.Item(0).Fields(2)) Then
                    vAgencyNumber = ""
                Else
                    'Developer Guide No 162
                    vAgencyNumber = .Records.Item(0).Fields(2)
                End If


                'Developer Guide No 162
                vMailboxNumber = .Records.Item(0).Fields(3)

                'Developer Guide No 162
                vPFEDIDefinitionID = .Records.Item(0).Fields(4)

                'Developer Guide No 162
                vDOB = .Records.Item(0).Fields(5)

                'Developer Guide No 162
                vCompanyReg = .Records.Item(0).Fields(6)
            End With

            ' Get Core details

            m_lReturn = m_oSIRParty.GetDetails(vPartyCnt:=vPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oSIRParty.GetNext(vShortname:=vShortName, vName:=vName, vCurrencyId:=vCurrencyID, vSourceID:=vSourceID, vSubBranchId:=vSubBranchID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Sub AddKeyParam(ByVal lPartyCnt As Integer)
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AddKeyParam
        ' PURPOSE: Adds the standard party cnt key for a SP call
        ' AUTHOR: Danny Davis
        ' DATE: 24 July 2003, 10:48 AM
        ' CHANGES:
        ' ---------------------------------------------------------------------------



        With m_oDatabase
            .Parameters.Add(sName:="party_cnt", vValue:=CStr(lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End With

    End Sub

    Private Sub AddInputParam(ByVal lFinanceProviderNumber As Integer, ByVal sAgencyNumber As String, ByVal sMailboxNumber As String, ByVal lPFEDIDefinitionID As Integer, ByVal iDOB As Integer, ByVal iCompanyReg As Integer)
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AddInputParam
        ' PURPOSE: Adds the standard parameters for a SP call
        ' AUTHOR: Danny Davis
        ' DATE: 24 July 2003, 10:48 AM
        ' CHANGES:
        ' ---------------------------------------------------------------------------




        With m_oDatabase
            .Parameters.Add(sName:="finance_provider_number", vValue:=CStr(lFinanceProviderNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            .Parameters.Add(sName:="agency_number", vValue:=sAgencyNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            .Parameters.Add(sName:="mailbox_number", vValue:=sMailboxNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            .Parameters.Add(sName:="pfedidefinition_id", vValue:=CStr(lPFEDIDefinitionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            .Parameters.Add(sName:="dob", vValue:=CStr(iDOB), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            .Parameters.Add(sName:="companyreg", vValue:=CStr(iCompanyReg), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        End With


    End Sub

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

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckReference (Public)
    '
    ' Description: Checks if the passed shortname (reference) already exists
    '
    ' ***************************************************************** '
    Public Function CheckReference(ByRef sReference As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckReference"

        Dim oParty As bSIRParty.Business
        Dim bExists As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create an instance of the main party business object
            oParty = New bSIRParty.Business()
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oParty.Initialise", "oParty = bSIRParty.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Pass in the account code to see if it already exists
            m_lReturn = oParty.CheckIfAccountCodeExists(v_sAccountCode:=sReference, r_bExists:=bExists)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oParty.CheckIfAccountCodeExists", "v_sAccountCode:=" & sReference, gPMConstants.PMELogLevel.PMLogError)
            End If

            'If the Reference was found then return empty string
            If bExists Then
                sReference = ""
            End If

            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '		Return result

            ' This is for debugging only
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetAddressDetails
    '
    ' Description: Pass-through call get address details for party.
    '
    ' ***************************************************************** '
    Public Function GetAddressDetails(ByRef lPartyCnt As Integer, ByRef vAddresses As Object) As Integer

        Dim result As Integer = 0
        Try



            Return m_oSIRParty.GetAddressDetails(vPartyCnt:=lPartyCnt, vAddresses:=vAddresses)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAddressDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAddressTypeLookups
    '
    ' Description: Pass-through to get address type lookups.
    '
    ' ***************************************************************** '
    Public Function GetAddressTypeLookups(ByRef vAddressTypes As Object) As Integer

        Dim result As Integer = 0
        Try



            Return m_oSIRParty.GetAddressTypeLookups(vAddressTypes:=vAddressTypes)

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAddressTypeLookups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressTypeLookups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetContactDetails
    '
    ' Description: Get contact details for party.
    '
    ' ***************************************************************** '
    Public Function GetContactDetails(ByRef lPartyCnt As Integer, ByRef vContacts As Object) As Integer

        Dim result As Integer = 0
        Try



            Return m_oSIRParty.GetContactDetails(vPartyCnt:=lPartyCnt, vContacts:=vContacts)

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetContactDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetContactDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateAddresses
    '
    ' Description: Update the party_address usage table with old
    ' and new addresses for the party.
    '
    ' ***************************************************************** '
    Public Function UpdateAddresses(ByRef lPartyCnt As Integer, Optional ByRef vAddAddresses As Object = Nothing, Optional ByRef vDeleteAddresses As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try



            Return m_oSIRParty.UpdateAddresses(vPartyCnt:=lPartyCnt, vAddAddresses:=vAddAddresses, vDeleteAddresses:=vDeleteAddresses)

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddresses", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateContacts
    '
    ' Description: Update the party_contact usage table with old
    ' and new contacts for the party.
    '
    ' ***************************************************************** '
    Public Function UpdateContacts(ByRef lPartyCnt As Integer, Optional ByRef vAddContacts As Object = Nothing, Optional ByRef vDeleteContacts As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try



            Return m_oSIRParty.UpdateContacts(vPartyCnt:=lPartyCnt, vAddContacts:=vAddContacts, vDeleteContacts:=vDeleteContacts)

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = RollbackTrans()

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContacts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    Public Function GetBranchBaseCurrency(ByRef vSourceID As Object, ByRef iCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return bPMFunc.GetBranchBaseCurrency(v_lSourceID:=CInt(vSourceID), v_oDatabase:=m_oDatabase, r_iCurrencyID:=iCurrencyID)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetBranchBaseCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchBaseCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 19-08-2005 : 360 = Taxes on Claims
    ' ***************************************************************** '
    Public Function GetPartyDetails(ByVal v_lPartyCnt As Integer, ByRef r_vResults As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue



            lReturn = m_oSIRParty.GetPartyDetails(v_lPartyCnt:=v_lPartyCnt, r_vResults:=r_vResults)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePartyDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function UpdatePartyDetails(ByVal v_lPartyCnt As Integer, ByVal v_vPartyDetails As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePartyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oSIRParty.UpdatePartyDetails(v_lPartyCnt:=v_lPartyCnt, v_vPartyDetails:=v_vPartyDetails)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function
End Class
