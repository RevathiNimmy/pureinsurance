Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports SSP.Shared
'developer guide no.129

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 03/09/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a CashList.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 20/10/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of CashLists (Private)
    Private m_oCashLists As bACTCashList.CashLists

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Source ID
    ' Insurance File ID
    Private m_lInsuranceFileID As Integer
    ' Risk ID
    Private m_lRiskID As Integer


    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    Private Const klFirstRow As Integer = 0

    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    'These have been added for the posting to the accounts
    'for Front Office Receipting...
    Private m_oDocumentPost As Object
    Private m_oAutoNumber As Object
    Private m_oMatchPost As Object
    Private m_oExplorer As bACTExplorer.Form
    Private m_oDocument As bACTDocument.Form
    Private m_oTransdetail As Object
    Private m_oPeriod As bACTPeriod.Form
    Private m_oAllocate As bACTAllocate.Business
    'Above have been added for the posting to the accounts
    'for Front Office Receipting...

    ' KG 19/09/03 - Should use a globally available const - but i can't find one...
    Private Const ACJournalBanking As String = "Banking"
    Private Const ACJournalAdjustments As String = "Adjustments"

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oCashLists.Count()
                    m_lCurrentRecord = m_oCashLists.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oCashLists.Count()

        End Get
    End Property

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
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

    Public Property SourceID() As Integer
        Get

            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = Value

        End Set
    End Property

    Public Property InsuranceFileID() As Integer
        Get

            Return m_lInsuranceFileID

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileID = Value

        End Set
    End Property

    Public Property RiskID() As Integer
        Get

            Return m_lRiskID

        End Get
        Set(ByVal Value As Integer)

            m_lRiskID = Value

        End Set
    End Property

    Public ReadOnly Property Details() As CashLists
        Get
            Return m_oCashLists
        End Get
    End Property

    ' ***************************************************************** '
    ' Name: GetAccountDetails
    '
    ' Author : Kevin Grandison
    ' Date : 19/09/03
    ' Description: Get some Account details
    ' Copied from bACTCashListPost.Possibly should have called this by running
    ' an instance of bACTCashListPost, but time is running out...
    '
    '
    ' ***************************************************************** '
    Private Function GetAccountDetails(ByVal v_lAccountId As Integer, ByRef r_vAccountArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Construct the SQL
        sSQL = "SELECT Company_ID,Sub_Branch_ID from Account WHERE Account_ID = " & v_lAccountId

        ' Perform query
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountDetails", bStoredProcedure:=True, vResultArray:=r_vAccountArray)

        If Not Informations.IsArray(r_vAccountArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetCashDrawerDetails
    '
    ' Author : Kevin Grandison
    ' Date : 19/09/03
    ' Description: Get some Cash Drawer details
    '
    '
    ' ***************************************************************** '
    Private Function GetCashDrawerDetails(ByVal v_lCashListDrawerID As Integer, ByRef r_vCashDrawerArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Construct the SQL
        sSQL = "SELECT Company_ID,Sub_Branch_ID from CashList_Drawer WHERE CashList_Drawer_ID = " & v_lCashListDrawerID

        ' Perform query
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetCashDrawerDetails", bStoredProcedure:=True, vResultArray:=r_vCashDrawerArray)

        If Not Informations.IsArray(r_vCashDrawerArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

    End Function


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
            ' Set Username and Password
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

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Get Reference to Database
            'ECK 21/07/99 Use Services
            '    Set m_oDatabase = GetOrionDatabase( _
            ''                            lOpenStatus:=m_lReturn, _
            ''                            bCloseDatabase:=m_bCloseDatabase, _
            ''                            vDatabase:=vDatabase)



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            'ECK
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            ' Default RiskID to 0 (So we will work at Ins File Level by default)
            m_lRiskID = 0

            ' Create CashLists Collection
            m_oCashLists = New bACTCashList.CashLists()


            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

            ' Currency Convert - added for front office receipting.

            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bACTCurrencyConvert.Form", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Initialise"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Initialise"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

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
                    m_oLookup = Nothing
                End If
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                    m_oCurrencyConvert = Nothing
                End If
                m_oCashLists = Nothing
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("SetProcessModes"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a CashList.
    '
    '
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oCashList As bACTCashList.CashList = Nothing
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 0) As Object
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names
            'vTabArray(PMLookupTableName, 0) = PMLookupEventRepeatType
            'vTabArray(PMLookupTableName, 1) = PMLookupEventType

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oCashList = m_oCashLists.Item(m_lCurrentRecord - 1)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' {* USER DEFINED CODE (Begin) *}
                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""
                    'vTabArray(PMLookupKey, 1) = ""
                    ' {* USER DEFINED CODE (End) *}

                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oCashList

                        ' {* USER DEFINED CODE (Begin) *}
                        'vTabArray(PMLookupKey, 0) = .RepeatTypeID
                        'vTabArray(PMLookupKey, 1) = .EventTypeID

                        'BB dtEffectiveDate = .EffectiveDate
                        'BB Default Effective Date to current date/time when not present
                        dtEffectiveDate = DateTime.Now
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oCashList

                        ' {* USER DEFINED CODE (Begin) *}
                        'vTabArray(PMLookupKey, 0) = .RepeatTypeID
                        'vTabArray(PMLookupKey, 1) = .EventTypeID
                        ' {* USER DEFINED CODE (End) *}

                    End With
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

            End Select

            ' Release CashList reference
            oCashList = Nothing

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetLookupValues"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single CashList directly into the database.
    '        Note: The CashList will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vCashListID As Integer = 0, Optional ByRef vCashListStatusID As Object = Nothing, Optional ByRef vCashListTypeID As Object = Nothing, Optional ByRef vCashListRef As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBankAccountID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vListDate As Object = Nothing, Optional ByRef vControlTotal As Object = Nothing, Optional ByRef vItemCount As Object = Nothing, Optional ByRef vCashlist_drawer_id As Object = Nothing, Optional ByRef vBatch_id As Object = Nothing, Optional ByRef vPMUser_id As Object = Nothing, Optional ByRef vConfirm_PMUser_id As Object = Nothing, Optional ByRef vConfirm2_PMUser_id As Object = Nothing, Optional ByRef vDate_Approved As Object = Nothing, Optional ByRef vBanking_Total As Object = Nothing, Optional ByRef vCash_Float_Amount As Object = Nothing, Optional ByRef vDepositDate As Object = Nothing, Optional ByRef vSubBranchID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCashList As bACTCashList.CashList

        Try

            Dim vSubBranchArray(,) As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue

            'MKW 311003 PN8033 START
            ' Retrieve SubBranch for Selected Company If Not Supplied.

            If Informations.IsNothing(vSubBranchID) Then

                'developer guide no. 98
                m_lReturn = CType(GetSubBranches(v_lSourceID:=vCompanyID, r_vSubBranchArray:=vSubBranchArray), gPMConstants.PMEReturnCode)
                If Informations.IsArray(vSubBranchArray) Then


                    vSubBranchID = vSubBranchArray(0, 0)
                End If
            End If
            'MKW 311003 PN8033 END

            ' Create a new CashList
            oCashList = New bACTCashList.CashList()

            ' Populate CashList Attributes

            'developer guide no. 98
            m_lReturn = CType(SetProperties(oCashList, gPMConstants.PMEComponentAction.PMAdd, vCashListID:=vCashListID, vCashListStatusID:=vCashListStatusID, vCashListTypeID:=vCashListTypeID, vCashListRef:=vCashListRef, vCompanyID:=vCompanyID, vBankAccountID:=vBankAccountID, vCurrencyID:=vCurrencyID, vListDate:=vListDate, vControlTotal:=vControlTotal, vItemCount:=vItemCount, vCashlist_drawer_id:=vCashlist_drawer_id, vBatch_id:=vBatch_id, vPMUser_id:=vPMUser_id, vConfirm_PMUser_id:=vConfirm_PMUser_id, vConfirm2_PMUser_id:=vConfirm2_PMUser_id, vDate_Approved:=vDate_Approved, vBanking_Total:=vBanking_Total, vCash_Float_Amount:=vCash_Float_Amount, vDepositDate:=vDepositDate, vSubBranchID:=vSubBranchID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the CashList to the Database
            m_lReturn = CType(AddItem(oCashList), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' Return the ID of the CashList Added

            If Not Informations.IsNothing(vCashListID) Then
                vCashListID = oCashList.CashListID
            End If

            ' {* USER DEFINED CODE (End) *}

            oCashList = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("DirectAdd"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single CashList directly from the database.
    '        Note: The CashList will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vCashListID As Object = Nothing, Optional ByRef vCashListStatusID As Object = Nothing, Optional ByRef vCashListTypeID As Object = Nothing, Optional ByRef vCashListRef As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBankAccountID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vListDate As Object = Nothing, Optional ByRef vControlTotal As Object = Nothing, Optional ByRef vItemCount As Object = Nothing, Optional ByRef vCashlist_drawer_id As Object = Nothing, Optional ByRef vBatch_id As Object = Nothing, Optional ByRef vPMUser_id As Object = Nothing, Optional ByRef vConfirm_PMUser_id As Object = Nothing, Optional ByRef vConfirm2_PMUser_id As Object = Nothing, Optional ByRef vDate_Approved As Object = Nothing, Optional ByRef vBanking_Total As Object = Nothing, Optional ByRef vCash_Float_Amount As Object = Nothing, Optional ByRef vDepositDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCashList As bACTCashList.CashList

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new CashList
            oCashList = New bACTCashList.CashList()

            ' Populate CashList Attributes

            'developer guide no. 98
            m_lReturn = CType(SetProperties(oCashList, gPMConstants.PMEComponentAction.PMDelete, vCashListID:=vCashListID, vCashListStatusID:=vCashListStatusID, vCashListTypeID:=vCashListTypeID, vCashListRef:=vCashListRef, vCompanyID:=vCompanyID, vBankAccountID:=vBankAccountID, vCurrencyID:=vCurrencyID, vListDate:=vListDate, vControlTotal:=vControlTotal, vItemCount:=vItemCount, vCashlist_drawer_id:=vCashlist_drawer_id, vBatch_id:=vBatch_id, vPMUser_id:=vPMUser_id, vConfirm_PMUser_id:=vConfirm_PMUser_id, vConfirm2_PMUser_id:=vConfirm2_PMUser_id, vDate_Approved:=vDate_Approved, vBanking_Total:=vBanking_Total, vCash_Float_Amount:=vCash_Float_Amount, vDepositDate:=vDepositDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the CashList to the Database
            m_lReturn = CType(DeleteItem(oCashList), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oCashList = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("DirectDelete"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the CashList.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vCashListID As Object = Nothing, Optional ByRef vCashListStatusID As Object = Nothing, Optional ByRef vCashListTypeID As Object = Nothing, Optional ByRef vCashListRef As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBankAccountID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vListDate As Object = Nothing, Optional ByRef vControlTotal As Object = Nothing, Optional ByRef vItemCount As Object = Nothing, Optional ByRef vCashlist_drawer_id As Object = Nothing, Optional ByRef vBatch_id As Object = Nothing, Optional ByRef vPMUser_id As Object = Nothing, Optional ByRef vConfirm_PMUser_id As Object = Nothing, Optional ByRef vConfirm2_PMUser_id As Object = Nothing, Optional ByRef vDate_Approved As Object = Nothing, Optional ByRef vBanking_Total As Object = Nothing, Optional ByRef vCash_Float_Amount As Object = Nothing, Optional ByRef vDepositDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults

            'developer guide no. 98
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vCashListID:=vCashListID, vCashListStatusID:=vCashListStatusID, vCashListTypeID:=vCashListTypeID, vCashListRef:=vCashListRef, vCompanyID:=vCompanyID, vBankAccountID:=vBankAccountID, vCurrencyID:=vCurrencyID, vListDate:=vListDate, vControlTotal:=vControlTotal, vItemCount:=vItemCount, vCashlist_drawer_id:=vCashlist_drawer_id, vBatch_id:=vBatch_id, vPMUser_id:=vPMUser_id, vConfirm_PMUser_id:=vConfirm_PMUser_id, vConfirm2_PMUser_id:=vConfirm2_PMUser_id, vDate_Approved:=vDate_Approved, vBanking_Total:=vBanking_Total, vCash_Float_Amount:=vCash_Float_Amount, vDepositDate:=vDepositDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetDefaults"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

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

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("CheckID"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBankAccountDefault (Public)
    '
    ' Description: Checks to see the default bank account for the
    '              current 'branch' and type of cash list
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function GetBankAccountDefault(Optional ByRef vSourceID As Object = Nothing, Optional ByRef vCashListTypeID As Object = Nothing, Optional ByRef vDefaultAccountID As Object = Nothing, Optional ByRef vMediaTypeId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim vBankAccountDefault(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the source_id parameter (INPUT)

            'SD 06/08/2002 Multibranch coding - Provide the company id if it is missing.


            If (Convert.IsDBNull(vSourceID) Or Informations.IsNothing(vSourceID)) Or (Informations.IsNothing(vSourceID)) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else

                m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(vSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                vDefaultAccountID = 0
                Return result
            End If

            ' Add the cashlisttype_id parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlisttype_id", vValue:=CStr(vCashListTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                vDefaultAccountID = 0
                Return result
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBankDefaultSQL, sSQLName:=ACGetBankDefaultName, bStoredProcedure:=ACGetBankDefaultStored, lNumberRecords:=0, vResultArray:=vBankAccountDefault)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                vDefaultAccountID = 0
                Return result
            End If

            If Informations.IsArray(vBankAccountDefault) Then
                ' How many records were selected

                lRecordCount = vBankAccountDefault.GetUpperBound(0) + 1
            End If

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                result = gPMConstants.PMEReturnCode.PMNotFound
                vDefaultAccountID = 0
                Return result
            End If

            ' check the first record returned -
            ' there should only ever be 1 record returned

            If Informations.IsArray(vBankAccountDefault) Then

                If Not Informations.IsNothing(vDefaultAccountID) Then
                    vDefaultAccountID = gPMFunctions.ToSafeLong(vBankAccountDefault(0, 0), 0)
                End If

                If Not Informations.IsNothing(vMediaTypeId) Then
                    vMediaTypeId = gPMFunctions.ToSafeLong(vBankAccountDefault(1, 0), 0)
                End If

            Else

                If Not Informations.IsNothing(vDefaultAccountID) Then
                    vDefaultAccountID = 0
                End If

                If Not Informations.IsNothing(vMediaTypeId) Then
                    vMediaTypeId = 0
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError
            vDefaultAccountID = 0

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBankAccountDefault Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetBankAccountDefault"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllUserCashListDrawer (Public)
    '
    ' Description: Selects multiple CashList_Drawer records that a user
    '              has access to from the database.
    '
    ' ***************************************************************** '
    Public Function GetAllUserCashListDrawer(ByVal v_lUserId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the cashlisttype_id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_id", vValue:=CStr(v_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Test for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllUserCashListDrawerSQL, sSQLName:=ACGetAllUserCashListDrawerName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get all the cash list drawer records for the user", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetAllUserCashListDrawer"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserBankingAuthorisation (Public)
    '
    ' Description: Selects multiple CashList_Drawer records that a user
    '              has access to from the database.
    '
    ' ***************************************************************** '
    Public Function GetUserBankingAuthorisation(ByVal v_lUserId As Integer, ByVal v_lCashDrawerId As Integer, ByRef r_bAuthorised As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the pmuser_Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmuser_id", vValue:=CStr(v_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Test for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the cashlist_drawer_id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(v_lCashDrawerId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Test for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the authorised parameter (OUTPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="authorised", vValue:=CStr(r_bAuthorised), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            'Test for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserBankingAuthorisationSQL, sSQLName:=ACGetUserBankingAuthorisationName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Return the @authorised parameter value
            r_bAuthorised = m_oDatabase.Parameters.Item("authorised").Value

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get all the cash list drawer records for the user", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetUserBankingAuthorisation"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLocks (Public)
    '
    ' Description: Selects multiple CashList_Drawer records that a user
    '              has access to from the database.
    '
    ' ***************************************************************** '
    Public Function GetLocks(ByVal v_sLockName As String, ByVal v_lLockValue As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the @lock_name parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="lock_name", vValue:=v_sLockName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'Test for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the @lock_value parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="lock_value", vValue:=CStr(v_lLockValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Test for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLocksSQL, sSQLName:=ACGetLocksName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get all locks for a key / value", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetLocks"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCaptions (Public)
    '
    ' Description: Get the requested caption fields for a record.
    '
    ' ***************************************************************** '
    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, Optional ByRef vTable As Object = Nothing) As Integer

        Dim result As Integer = 0
        'Developer Guide No. 21
        Dim oFields As DataRow

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(vFieldArray) Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Parameter vFieldArray must be a Variant Array", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetCaptions"))

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have a Table name
            'BB If (IsMissing(vTable) = False) Then

            ' Is this our table
            '    If (Trim$(vTable) <> PMTableCashList) Then

            '        GetCaptions = PMInvalidRequest


            '       Exit Function

            '    End If

            'End If

            ' Get the Captions ourself

            ' Check that this record exists
            m_lReturn = CType(CheckID(vID:=vID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Resize the Temporary Results Array
            Dim vResults(vFieldArray.GetUpperBound(0)) As Object

            ' Get a reference to the Fields returned
            'Developer Guide No. 111
            oFields = m_oDatabase.Records.Item(0).Fields()

            With oFields

                ' For Each Field requested
                For lSub As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                    'AK 230702 - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or Informations.IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        Select Case oFields(vFieldArray(lSub)).Type
                            Case DbType.String, DbType.String, DbType.String, ADODB.DataTypeEnum.adLongVarWChar, DbType.String, DbType.String, ADODB.DataTypeEnum.adWChar

                                vResults(lSub) = ""
                            Case DbType.Date, ADODB.DataTypeEnum.adDBDate

                                vResults(lSub) = -1
                            Case Else

                                vResults(lSub) = 0
                        End Select
                    End If

                Next lSub

            End With

            ' Assign the results
            vResultArray = vResults

            ' Release the reference to the Fields
            oFields = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetCaptions"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required CashLists and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vCashListID As Object = Nothing, Optional ByRef vLockMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oCashList As bACTCashList.CashList

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oCashLists.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = 0
            End If

            ' Do we have a key

            If Not Informations.IsNothing(vCashListID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vCashListID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vCashListID =" & CStr(vCashListID), vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetDetails"))

                    Return result

                End If

                ' Add the CashListID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="CashList_id", vValue:=CStr(vCashListID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                ' No Key, Get All Records

                'BB Add the CompanyID parameter (INPUT)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound

            Else

                ' Yes, load them into the collection
                'developer guide no. 162
                For lSub As Integer = 0 To lRecordCount - 1

                    ' Create New CashList
                    oCashList = New bACTCashList.CashList()

                    m_lReturn = CType(SetPropertiesFromDB(oCashList:=oCashList, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add CashList to collection
                    
                    m_lReturn = CType(m_oCashLists.Add(oNewCashList:=oCashList), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oCashList = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetDetails"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required CashLists and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vCashListID As Object = Nothing, Optional ByRef vCashListStatusID As Object = Nothing, Optional ByRef vCashListTypeID As Object = Nothing, Optional ByRef vCashListRef As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBankAccountID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vListDate As Object = Nothing, Optional ByRef vControlTotal As Object = Nothing, Optional ByRef vItemCount As Object = Nothing, Optional ByRef vCashlist_drawer_id As Object = Nothing, Optional ByRef vBatch_id As Object = Nothing, Optional ByRef vPMUser_id As Object = Nothing, Optional ByRef vConfirm_PMUser_id As Object = Nothing, Optional ByRef vConfirm2_PMUser_id As Object = Nothing, Optional ByRef vDate_Approved As Object = Nothing, Optional ByRef vBanking_Total As Object = Nothing, Optional ByRef vCash_Float_Amount As Object = Nothing, Optional ByRef vDepositDate As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oCashList As bACTCashList.CashList
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oCashLists.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oCashList = m_oCashLists.Item(m_lCurrentRecord - 1)

            ' Get the CashList Property Values

            'developer guide no. 98
            m_lReturn = CType(GetProperties(oCashList, iStatus, vCashListID:=vCashListID, vCashListStatusID:=vCashListStatusID, vCashListTypeID:=vCashListTypeID, vCashListRef:=vCashListRef, vCompanyID:=vCompanyID, vBankAccountID:=vBankAccountID, vCurrencyID:=vCurrencyID, vListDate:=vListDate, vControlTotal:=vControlTotal, vItemCount:=vItemCount, vCashlist_drawer_id:=vCashlist_drawer_id, vBatch_id:=vBatch_id, vPMUser_id:=vPMUser_id, vConfirm_PMUser_id:=vConfirm_PMUser_id, vConfirm2_PMUser_id:=vConfirm2_PMUser_id, vDate_Approved:=vDate_Approved, vBanking_Total:=vBanking_Total, vCash_Float_Amount:=vCash_Float_Amount, vDepositDate:=vDepositDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oCashList = Nothing


            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetNext"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied CashList into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vCashListID As Object = Nothing, Optional ByRef vCashListStatusID As Object = Nothing, Optional ByRef vCashListTypeID As Object = Nothing, Optional ByRef vCashListRef As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBankAccountID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vListDate As Object = Nothing, Optional ByRef vControlTotal As Object = Nothing, Optional ByRef vItemCount As Object = Nothing, Optional ByRef vCashlist_drawer_id As Object = Nothing, Optional ByRef vBatch_id As Object = Nothing, Optional ByRef vPMUser_id As Object = Nothing, Optional ByRef vConfirm_PMUser_id As Object = Nothing, Optional ByRef vConfirm2_PMUser_id As Object = Nothing, Optional ByRef vDate_Approved As Object = Nothing, Optional ByRef vBanking_Total As Object = Nothing, Optional ByRef vCash_Float_Amount As Object = Nothing, Optional ByRef vDepositDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCashList As bACTCashList.CashList

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oCashLists.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new CashList
            oCashList = New bACTCashList.CashList()

            ' Populate CashList Attributes

            'developer guide no. 98
            m_lReturn = CType(SetProperties(oCashList, gPMConstants.PMEComponentAction.PMAdd, vCashListID:=vCashListID, vCashListStatusID:=vCashListStatusID, vCashListTypeID:=vCashListTypeID, vCashListRef:=vCashListRef, vCompanyID:=vCompanyID, vBankAccountID:=vBankAccountID, vCurrencyID:=vCurrencyID, vListDate:=vListDate, vControlTotal:=vControlTotal, vItemCount:=vItemCount, vCashlist_drawer_id:=vCashlist_drawer_id, vBatch_id:=vBatch_id, vPMUser_id:=vPMUser_id, vConfirm_PMUser_id:=vConfirm_PMUser_id, vConfirm2_PMUser_id:=vConfirm2_PMUser_id, vDate_Approved:=vDate_Approved, vBanking_Total:=vBanking_Total, vCash_Float_Amount:=vCash_Float_Amount, vDepositDate:=vDepositDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCashList = Nothing
                Return result
            End If

            ' Add CashList to collection
           
            m_lReturn = CType(m_oCashLists.Add(oNewCashList:=oCashList), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCashList = Nothing
                Return result
            End If

            oCashList = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("EditAdd"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the CashList
    '              specified and updates the CashList with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vCashListID As Object = Nothing, Optional ByRef vCashListStatusID As Object = Nothing, Optional ByRef vCashListTypeID As Object = Nothing, Optional ByRef vCashListRef As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBankAccountID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vListDate As Object = Nothing, Optional ByRef vControlTotal As Object = Nothing, Optional ByRef vItemCount As Object = Nothing, Optional ByRef vCashlist_drawer_id As Object = Nothing, Optional ByRef vBatch_id As Object = Nothing, Optional ByRef vPMUser_id As Object = Nothing, Optional ByRef vConfirm_PMUser_id As Object = Nothing, Optional ByRef vConfirm2_PMUser_id As Object = Nothing, Optional ByRef vDate_Approved As Object = Nothing, Optional ByRef vBanking_Total As Object = Nothing, Optional ByRef vCash_Float_Amount As Object = Nothing, Optional ByRef vDepositDate As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oCashList As bACTCashList.CashList
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCashLists.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oCashList = m_oCashLists.Item(lRow - 1)

            ' Check the Status of the CashList

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oCashList.DatabaseStatus
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

            ' Update CashList Attributes

            'developer guide no. 98
            m_lReturn = CType(SetProperties(oCashList, iStatus, vCashListID:=vCashListID, vCashListStatusID:=vCashListStatusID, vCashListTypeID:=vCashListTypeID, vCashListRef:=vCashListRef, vCompanyID:=vCompanyID, vBankAccountID:=vBankAccountID, vCurrencyID:=vCurrencyID, vListDate:=vListDate, vControlTotal:=vControlTotal, vItemCount:=vItemCount, vCashlist_drawer_id:=vCashlist_drawer_id, vBatch_id:=vBatch_id, vPMUser_id:=vPMUser_id, vConfirm_PMUser_id:=vConfirm_PMUser_id, vConfirm2_PMUser_id:=vConfirm2_PMUser_id, vDate_Approved:=vDate_Approved, vBanking_Total:=vBanking_Total, vCash_Float_Amount:=vCash_Float_Amount, vDepositDate:=vDepositDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCashList = Nothing
                Return result
            End If

            ' Release reference to CashList
            oCashList = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("EditUpdate"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified CashList can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oCashList As bACTCashList.CashList

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCashLists.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oCashList = m_oCashLists.Item(lRow - 1)

            ' Check the Status of the CashList

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oCashList.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oCashList.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oCashList.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to CashList
            oCashList = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("EditDelete"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

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
            For lSub As Integer = 1 To m_oCashLists.Count()
                Select Case m_oCashLists.Item(lSub - 1).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Cancel"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

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
        Dim oCashList As bACTCashList.CashList
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oCashLists.Count()
                oCashList = m_oCashLists.Item(lSub - 1)


                Select Case oCashList.DatabaseStatus
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
                        m_lReturn = CType(AddItem(oCashList), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(UpdateItem(oCashList), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(DeleteItem(oCashList), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oCashList = Nothing

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
                    Do While lSub <= m_oCashLists.Count()

                        ' With the item
                        With m_oCashLists.Item(lSub - 1)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oCashLists.Delete(lSub - 1)

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Update"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oCashList As bACTCashList.CashList) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add CashListID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="CashList_id", vValue:=CStr(oCashList.CashListID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oCashList:=oCashList), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oCashList.CashListID = m_oDatabase.Parameters.Item("CashList_id").Value

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oCashList As bACTCashList.CashList) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add CashListID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="CashList_id", vValue:=CStr(oCashList.CashListID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oCashList:=oCashList), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' BB Not required by Orion
        ' Add Timestamp as an INPUT param for an update
        'm_lReturn& = m_oDatabase.Parameters.Add( _
        'sName:="timestamp", _
        'vValue:=oCashList.Timestamp, _
        'iDirection:=PMParamInput, _
        'iDataType:=PMBinary)

        'If (m_lReturn& <> PMTrue) Then
        '    UpdateItem = PMFalse
        '    Exit Function
        'End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check to see that the record was updated OK

        If lRecordsAffected > 0 Then
            ' Updated No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Private Function DeleteItem(ByRef oCashList As bACTCashList.CashList) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the CashListID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="CashList_id", vValue:=CStr(oCashList.CashListID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If record wasn't deleted, error
        If lRecordsAffected > 0 Then
            ' Deleted, No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied CashList properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oCashList As bACTCashList.CashList, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'developer guide no.21
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oCashList

            .CashListID = oFields("CashList_id")

            If Convert.IsDBNull(oFields("CashListStatus_id")) Or Informations.IsNothing(oFields("CashListStatus_id")) Then
                .CashListStatusID = 0
            Else
                .CashListStatusID = oFields("CashListStatus_id")
            End If

            If Convert.IsDBNull(oFields("cashlisttype_id")) Or Informations.IsNothing(oFields("cashlisttype_id")) Then
                .CashListTypeID = 0
            Else
                .CashListTypeID = oFields("cashlisttype_id")
            End If
            .CashListRef = oFields("CashList_ref")
            .CompanyID = oFields("company_id")
            ' KG 29/07/03 - Get SubBranchID

            If Convert.IsDBNull(oFields("sub_branch_id")) Or Informations.IsNothing(oFields("sub_branch_id")) Then
                .SubBranchID = 0
            Else
                .SubBranchID = oFields("sub_branch_id")
            End If


            If Convert.IsDBNull(oFields("bankaccount_id")) Or Informations.IsNothing(oFields("bankaccount_id")) Then
                .BankAccountID = 0
            Else
                .BankAccountID = oFields("bankaccount_id")
            End If

            If Convert.IsDBNull(oFields("currency_id")) Or Informations.IsNothing(oFields("currency_id")) Then
                .CurrencyID = 0
            Else
                .CurrencyID = oFields("currency_id")
            End If

            If Convert.IsDBNull(oFields("list_date")) Or Informations.IsNothing(oFields("list_date")) Then
                .ListDate = #12/30/1899#
            Else
                .ListDate = oFields("list_date")
            End If

            If Convert.IsDBNull(oFields("control_total")) Or Informations.IsNothing(oFields("control_total")) Then
                .ControlTotal = 0
            Else
                .ControlTotal = oFields("control_total")
            End If

            If Convert.IsDBNull(oFields("item_count")) Or Informations.IsNothing(oFields("item_count")) Then
                .ItemCount = 0
            Else
                .ItemCount = oFields("item_count")
            End If

            'pkh 07/10/2002 starts - Added for Front Office Receipting module

            If Convert.IsDBNull(oFields("cashlist_drawer_id")) Or Informations.IsNothing(oFields("cashlist_drawer_id")) Then
                .CashList_drawer_id = 0
            Else
                .CashList_drawer_id = oFields("cashlist_drawer_id")
            End If

            If Convert.IsDBNull(oFields("batch_id")) Or Informations.IsNothing(oFields("batch_id")) Then
                .Batch_id = 0
            Else
                .Batch_id = oFields("batch_id")
            End If

            If Convert.IsDBNull(oFields("pmuser_id")) Or Informations.IsNothing(oFields("pmuser_id")) Then
                .PMUser_id = 0
            Else
                .PMUser_id = oFields("pmuser_id")
            End If

            If Convert.IsDBNull(oFields("confirm_pmuser_id")) Or Informations.IsNothing(oFields("confirm_pmuser_id")) Then
                .Confirm_pmuser_id = 0
            Else
                .Confirm_pmuser_id = oFields("confirm_pmuser_id")
            End If

            If Convert.IsDBNull(oFields("confirm2_pmuser_id")) Or Informations.IsNothing(oFields("confirm2_pmuser_id")) Then
                .Confirm2_pmuser_id = 0
            Else
                .Confirm2_pmuser_id = oFields("confirm2_pmuser_id")
            End If

            If Convert.IsDBNull(oFields("date_approved")) Or Informations.IsNothing(oFields("date_approved")) Then
                .Date_Approved = #12/30/1899#
            Else
                .Date_Approved = oFields("date_approved")
            End If

            If Convert.IsDBNull(oFields("banking_total")) Or Informations.IsNothing(oFields("banking_total")) Then
                .Banking_Total = 0
            Else
                .Banking_Total = oFields("banking_total")
            End If

            If Convert.IsDBNull(oFields("cash_float_amount")) Or Informations.IsNothing(oFields("cash_float_amount")) Then
                .Cash_Float_Amount = 0
            Else
                .Cash_Float_Amount = oFields("cash_float_amount")
            End If
            'someone forgot this field sw front office receipting 07/01/2002

            If Convert.IsDBNull(oFields("Date_Deposited")) Or Informations.IsNothing(oFields("Date_Deposited")) Then
                .DepositDate = #12/30/1899#
            Else
                .DepositDate = oFields("Date_Deposited")
            End If

            'pkh 07/10/2002 ends   - Added for Front Office Receipting module


            If Convert.IsDBNull(oFields("is_split_receipt")) Or oFields("is_split_receipt") Is Nothing Then
                .IsSplitReceipt = False
            Else
                .IsSplitReceipt = True
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied CashList property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oCashList As bACTCashList.CashList, ByRef iStatus As Integer, Optional ByRef vCashListID As Integer = 0, Optional ByRef vCashListStatusID As Integer = 0, Optional ByRef vCashListTypeID As Integer = 0, Optional ByRef vCashListRef As String = "", Optional ByRef vCompanyID As Integer = 0, Optional ByRef vBankAccountID As Integer = 0, Optional ByRef vCurrencyID As Integer = 0, Optional ByRef vListDate As Object = Nothing, Optional ByRef vControlTotal As Decimal = 0, Optional ByRef vItemCount As Integer = 0, Optional ByRef vCashlist_drawer_id As Integer = 0, Optional ByRef vBatch_id As Integer = 0, Optional ByRef vPMUser_id As Integer = 0, Optional ByRef vConfirm_PMUser_id As Integer = 0, Optional ByRef vConfirm2_PMUser_id As Integer = 0, Optional ByRef vDate_Approved As Object = Nothing, Optional ByRef vBanking_Total As Decimal = 0, Optional ByRef vCash_Float_Amount As Decimal = 0, Optional ByRef vDepositDate As Object = Nothing, Optional ByRef vSubBranchID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CType(CheckMandatory(vCashListStatusID:=vCashListStatusID, vCashListTypeID:=vCashListTypeID, vCashListRef:=vCashListRef, vCompanyID:=vCompanyID, vBankAccountID:=vBankAccountID, vCurrencyID:=vCurrencyID, vListDate:=vListDate, vControlTotal:=vControlTotal, vItemCount:=vItemCount, vCashlist_drawer_id:=vCashlist_drawer_id, vBatch_id:=vBatch_id, vPMUser_id:=vPMUser_id, vConfirm_PMUser_id:=vConfirm_PMUser_id, vConfirm2_PMUser_id:=vConfirm2_PMUser_id, vDate_Approved:=vDate_Approved, vBanking_Total:=vBanking_Total, vCash_Float_Amount:=vCash_Float_Amount, vDepositDate:=vDepositDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters



            'developer guide no. 98
            m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vCashListID:=vCashListID, vCashListStatusID:=vCashListStatusID, vCashListTypeID:=vCashListTypeID, vCashListRef:=vCashListRef, vCompanyID:=vCompanyID, vBankAccountID:=vBankAccountID, vCurrencyID:=vCurrencyID, vListDate:=vListDate, vControlTotal:=vControlTotal, vItemCount:=vItemCount, vCashlist_drawer_id:=vCashlist_drawer_id, vBatch_id:=vBatch_id, vPMUser_id:=vPMUser_id, vConfirm_PMUser_id:=vConfirm_PMUser_id, vConfirm2_PMUser_id:=vConfirm2_PMUser_id, vDate_Approved:=vDate_Approved, vBanking_Total:=vBanking_Total, vCash_Float_Amount:=vCash_Float_Amount, vDepositDate:=vDepositDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = CType(Validate(vCashListID:=vCashListID, vCashListStatusID:=vCashListStatusID, vCashListTypeID:=vCashListTypeID, vCashListRef:=vCashListRef, vCompanyID:=vCompanyID, vBankAccountID:=vBankAccountID, vCurrencyID:=vCurrencyID, vListDate:=vListDate, vControlTotal:=vControlTotal, vItemCount:=vItemCount, vCashlist_drawer_id:=vCashlist_drawer_id, vBatch_id:=vBatch_id, vPMUser_id:=vPMUser_id, vConfirm_PMUser_id:=vConfirm_PMUser_id, vConfirm2_PMUser_id:=vConfirm2_PMUser_id, vDate_Approved:=vDate_Approved, vBanking_Total:=vBanking_Total, vCash_Float_Amount:=vCash_Float_Amount, vDepositDate:=vDepositDate), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False


        ' Set Property values.
        With oCashList


            If Not Informations.IsNothing(vCashListID) Then
                If .CashListID <> vCashListID Then
                    .CashListID = vCashListID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCashListStatusID) Then
                If .CashListStatusID <> vCashListStatusID Then
                    .CashListStatusID = vCashListStatusID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCashListTypeID) Then
                If .CashListTypeID <> vCashListTypeID Then
                    .CashListTypeID = vCashListTypeID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCashListRef) Then
                If .CashListRef.Trim() <> vCashListRef.Trim() Then
                    .CashListRef = vCashListRef
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCompanyID) Then
                If .CompanyID <> vCompanyID Then
                    .CompanyID = vCompanyID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBankAccountID) Then
                If .BankAccountID <> vBankAccountID Then
                    .BankAccountID = vBankAccountID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCurrencyID) Then
                If .CurrencyID <> vCurrencyID Then
                    .CurrencyID = vCurrencyID
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vListDate) Then

                If .ListDate <> CDate(vListDate) Then

                    .ListDate = CDate(vListDate)
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vControlTotal) Then
                If .ControlTotal <> vControlTotal Then
                    .ControlTotal = vControlTotal
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vItemCount) Then
                If .ItemCount <> vItemCount Then
                    .ItemCount = vItemCount
                    bDataChanged = True
                End If
            End If

            'pkh 07/10/2002 starts - Added for Front Office Receipting module

            If Not Informations.IsNothing(vCashlist_drawer_id) Then
                If .CashList_drawer_id <> vCashlist_drawer_id Then
                    .CashList_drawer_id = vCashlist_drawer_id
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBatch_id) Then
                If .Batch_id <> vBatch_id Then
                    .Batch_id = vBatch_id
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vPMUser_id) Then
                If .PMUser_id <> vPMUser_id Then
                    .PMUser_id = vPMUser_id
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vConfirm_PMUser_id) Then
                If .Confirm_pmuser_id <> vConfirm_PMUser_id Then
                    .Confirm_pmuser_id = vConfirm_PMUser_id
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vConfirm2_PMUser_id) Then
                If .Confirm2_pmuser_id <> vConfirm2_PMUser_id Then
                    .Confirm2_pmuser_id = vConfirm2_PMUser_id
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vDate_Approved) Then

                If .Date_Approved <> CDate(vDate_Approved) Then

                    .Date_Approved = CDate(vDate_Approved)
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vBanking_Total) Then
                If .Banking_Total <> vBanking_Total Then
                    .Banking_Total = vBanking_Total
                    bDataChanged = True
                End If
            End If


            If Not Informations.IsNothing(vCash_Float_Amount) Then
                If .Cash_Float_Amount <> vCash_Float_Amount Then
                    .Cash_Float_Amount = vCash_Float_Amount
                    bDataChanged = True
                End If
            End If
            'pkh 07/10/2002 ends   - Added for Front Office Receipting module

            'someone forgot this field
            'sw 07/01/2003

            If Not Informations.IsNothing(vDepositDate) Then

                If .DepositDate <> CDate(vDepositDate) Then

                    .DepositDate = CDate(vDepositDate)
                    bDataChanged = True
                End If
            End If

            'KG 12/06/03 - Sub Branch ID

            If Not Informations.IsNothing(vSubBranchID) Then
                If .SubBranchID <> vSubBranchID Then
                    .SubBranchID = vSubBranchID
                    bDataChanged = True
                End If
            End If


            ' If we have changed one of the properties, update the status
            If bDataChanged Then
                .DatabaseStatus = iStatus
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied CashList property values.
    '
    ' ***************************************************************** '
    Private Function GetProperties(ByRef oCashList As bACTCashList.CashList, ByRef iStatus As Integer, Optional ByRef vCashListID As Integer = 0, Optional ByRef vCashListStatusID As Integer = 0, Optional ByRef vCashListTypeID As Integer = 0, Optional ByRef vCashListRef As String = "", Optional ByRef vCompanyID As Integer = 0, Optional ByRef vBankAccountID As Integer = 0, Optional ByRef vCurrencyID As Integer = 0, Optional ByRef vListDate As Object = Nothing, Optional ByRef vControlTotal As Decimal = 0, Optional ByRef vItemCount As Integer = 0, Optional ByRef vCashlist_drawer_id As Integer = 0, Optional ByRef vBatch_id As Integer = 0, Optional ByRef vPMUser_id As Integer = 0, Optional ByRef vConfirm_PMUser_id As Integer = 0, Optional ByRef vConfirm2_PMUser_id As Integer = 0, Optional ByRef vDate_Approved As Object = Nothing, Optional ByRef vBanking_Total As Decimal = 0, Optional ByRef vCash_Float_Amount As Decimal = 0, Optional ByRef vDepositDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oCashList


            'developer guide no. 118
            vCashListID = .CashListID

            'developer guide no. 118
            vCashListStatusID = .CashListStatusID

            'developer guide no. 118
            vCashListTypeID = .CashListTypeID

            'developer guide no. 118
            vCashListRef = .CashListRef

            'developer guide no. 118
            vCompanyID = .CompanyID

            'developer guide no. 118
            vBankAccountID = .BankAccountID

            'developer guide no. 118
            vCurrencyID = .CurrencyID

            'developer guide no. 118
            vListDate = .ListDate

            'developer guide no. 118
            vControlTotal = .ControlTotal

            'developer guide no. 118
            vItemCount = .ItemCount

            'pkh 07/10/2002 starts - Added for Front Office Receipting module

            'developer guide no. 118
            vCashlist_drawer_id = .CashList_drawer_id

            'developer guide no. 118
            vBatch_id = .Batch_id

            'developer guide no. 118
            vPMUser_id = .PMUser_id

            'developer guide no. 118
            vConfirm_PMUser_id = .Confirm_pmuser_id

            'developer guide no. 118
            vConfirm2_PMUser_id = .Confirm2_pmuser_id

            'developer guide no. 118
            vDate_Approved = .Date_Approved

            'developer guide no. 118
            vBanking_Total = .Banking_Total


            'developer guide no. 118
            vCash_Float_Amount = .Cash_Float_Amount

            'pkh 07/10/2002 ends   - Added for Front Office Receipting module

            'someone forgot this property
            'sw 07/01/2003


            'developer guide no. 118
            vDepositDate = .DepositDate

            iStatus = .DatabaseStatus

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oCashList As bACTCashList.CashList) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            ' CTAF 191200 - Rearranged the order these are added.

            If oCashList.BankAccountID < 1 Then

                m_lReturn = .Parameters.Add(sName:="bankaccount_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="bankaccount_id", vValue:=CStr(oCashList.BankAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oCashList.CashListTypeID < 1 Then

                m_lReturn = .Parameters.Add(sName:="cashlisttype_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="cashlisttype_id", vValue:=CStr(oCashList.CashListTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oCashList.CashListStatusID < 1 Then

                m_lReturn = .Parameters.Add(sName:="cashliststatus_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="cashliststatus_id", vValue:=CStr(oCashList.CashListStatusID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="cashlist_ref", vValue:=oCashList.CashListRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="company_id", vValue:=CStr(oCashList.CompanyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' KG 12/06/03
            ' Sub Branch ID
            m_lReturn = .Parameters.Add(sName:="Sub_Branch_ID", vValue:=CStr(oCashList.SubBranchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If oCashList.CurrencyID < 1 Then

                m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=CStr(oCashList.CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no. 131
            If (DateTime.MinValue.Equals(oCashList.ListDate)) Then
                m_lReturn = .Parameters.Add(sName:="list_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = .Parameters.Add(sName:="list_date", vValue:=oCashList.ListDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If




            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="control_total", vValue:=CStr(oCashList.ControlTotal), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="item_count", vValue:=CStr(oCashList.ItemCount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'pkh 07/10/2002 - Add new parameters to support Front Office Receipting
            If oCashList.CashList_drawer_id < 1 Then

                m_lReturn = .Parameters.Add(sName:="cashlist_drawer_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(oCashList.CashList_drawer_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oCashList.Batch_id < 1 Then

                m_lReturn = .Parameters.Add(sName:="batch_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="batch_id", vValue:=CStr(oCashList.Batch_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oCashList.PMUser_id < 1 Then

                m_lReturn = .Parameters.Add(sName:="pmuser_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="pmuser_id", vValue:=CStr(oCashList.PMUser_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oCashList.Confirm_pmuser_id < 1 Then

                m_lReturn = .Parameters.Add(sName:="confirm_pmuser_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="confirm_pmuser_id", vValue:=CStr(oCashList.Confirm_pmuser_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oCashList.Confirm2_pmuser_id < 1 Then

                m_lReturn = .Parameters.Add(sName:="confirm2_pmuser_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="confirm2_pmuser_id", vValue:=CStr(oCashList.Confirm2_pmuser_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no. 131
            If (DateTime.MinValue.Equals(oCashList.Date_Approved)) Then
                m_lReturn = .Parameters.Add(sName:="date_approved", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = .Parameters.Add(sName:="date_approved", vValue:=oCashList.Date_Approved, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oCashList.Banking_Total < 1 Then

                m_lReturn = .Parameters.Add(sName:="banking_total", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            Else
                m_lReturn = .Parameters.Add(sName:="banking_total", vValue:=CStr(oCashList.Banking_Total), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oCashList.Cash_Float_Amount < 1 Then

                m_lReturn = .Parameters.Add(sName:="cash_float_amount", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            Else
                m_lReturn = .Parameters.Add(sName:="cash_float_amount", vValue:=CStr(oCashList.Cash_Float_Amount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'pkh 07/10/2002 - ends

            'sw 07/01/2003
            'developer guide no. 131
            If (DateTime.MinValue.Equals(oCashList.DepositDate)) Then
                m_lReturn = .Parameters.Add(sName:="deposit_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                'developer guide no. 40
                m_lReturn = .Parameters.Add(sName:="deposit_date", vValue:=oCashList.DepositDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a CashList.
    '
    ' ***************************************************************** '
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vCashListID As Object = Nothing, Optional ByRef vCashListStatusID As Object = Nothing, Optional ByRef vCashListTypeID As Object = Nothing, Optional ByRef vCashListRef As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBankAccountID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vListDate As Object = Nothing, Optional ByRef vControlTotal As Object = Nothing, Optional ByRef vItemCount As Object = Nothing, Optional ByRef vCashlist_drawer_id As Object = Nothing, Optional ByRef vBatch_id As Object = Nothing, Optional ByRef vPMUser_id As Object = Nothing, Optional ByRef vConfirm_PMUser_id As Object = Nothing, Optional ByRef vConfirm2_PMUser_id As Object = Nothing, Optional ByRef vDate_Approved As Object = Nothing, Optional ByRef vBanking_Total As Object = Nothing, Optional ByRef vCash_Float_Amount As Object = Nothing, Optional ByRef vDepositDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vCashListID)) OrElse (vCashListID.Equals(0)) Or (bDefaultAll) Then
            vCashListID = 0
        End If



        If (Informations.IsNothing(vCashListStatusID)) OrElse (vCashListStatusID.Equals(0)) Or (bDefaultAll) Then
            vCashListStatusID = 0
        End If



        If (Informations.IsNothing(vCashListTypeID)) OrElse (vCashListTypeID.Equals(0)) Or (bDefaultAll) Then
            vCashListTypeID = 0
        End If



        If (Informations.IsNothing(vCashListRef)) OrElse (String.IsNullOrEmpty(vCashListRef)) Or (bDefaultAll) Then
            vCashListRef = ""
        End If



        If (Informations.IsNothing(vCompanyID)) OrElse (vCompanyID.Equals(0)) Or (bDefaultAll) Then
            vCompanyID = 0
        End If



        If (Informations.IsNothing(vBankAccountID)) OrElse (vBankAccountID.Equals(0)) Or (bDefaultAll) Then
            vBankAccountID = 0
        End If



        If (Informations.IsNothing(vCurrencyID)) OrElse (vCurrencyID.Equals(0)) Or (bDefaultAll) Then
            vCurrencyID = 0
        End If



        If (Informations.IsNothing(vListDate)) OrElse (vListDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vListDate = DateTime.Now
        End If



        If (Informations.IsNothing(vControlTotal)) OrElse (vControlTotal.Equals(0)) Or (bDefaultAll) Then
            vControlTotal = 0
        End If



        If (Informations.IsNothing(vItemCount)) OrElse (vItemCount.Equals(0)) Or (bDefaultAll) Then
            vItemCount = 0
        End If

        'pkh 07/10/2002 starts - Added for Front Office Receipting module


        If (Informations.IsNothing(vCashlist_drawer_id)) OrElse (vCashlist_drawer_id.Equals(0)) Or (bDefaultAll) Then
            vCashlist_drawer_id = 0
        End If



        If (Informations.IsNothing(vBatch_id)) OrElse (vBatch_id.Equals(0)) Or (bDefaultAll) Then
            vBatch_id = 0
        End If



        If (Informations.IsNothing(vPMUser_id)) OrElse (vPMUser_id.Equals(0)) Or (bDefaultAll) Then
            vPMUser_id = 0
        End If



        If (Informations.IsNothing(vConfirm_PMUser_id)) OrElse (vConfirm_PMUser_id.Equals(0)) Or (bDefaultAll) Then
            vConfirm_PMUser_id = 0
        End If



        If (Informations.IsNothing(vConfirm2_PMUser_id)) OrElse (vConfirm2_PMUser_id.Equals(0)) Or (bDefaultAll) Then
            vConfirm2_PMUser_id = 0
        End If



        If (Informations.IsNothing(vDate_Approved)) OrElse (vDate_Approved.Equals(0)) Or (bDefaultAll) Then
            vDate_Approved = DateTime.MinValue
        End If

        If (Informations.IsNothing(vBanking_Total)) OrElse (vBanking_Total.Equals(0)) Or (bDefaultAll) Then
            vBanking_Total = 0
        End If



        If (Informations.IsNothing(vCash_Float_Amount)) OrElse (vCash_Float_Amount.Equals(0)) Or (bDefaultAll) Then
            vCash_Float_Amount = 0
        End If




        If (Informations.IsNothing(vDepositDate)) OrElse (vDepositDate.Equals(0)) Or (bDefaultAll) Then
            vDepositDate = DateTime.MinValue
        End If

        'pkh 07/10/2002 ends   - Added for Front Office Receipting module


        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    '*************************************************************************
    'Name:          CheckMandatory (Public)
    'Description:   Sets the Default Values for a CashList.
    'History:       TR140103 - Removed CashListId from list of mandatory
    '               fields as it's not
    '*************************************************************************
    Private Function CheckMandatory(Optional ByRef vCashListStatusID As Object = Nothing, Optional ByRef vCashListTypeID As Object = Nothing, Optional ByRef vCashListRef As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBankAccountID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vListDate As Object = Nothing, Optional ByRef vControlTotal As Object = Nothing, Optional ByRef vItemCount As Object = Nothing, Optional ByRef vCashlist_drawer_id As Object = Nothing, Optional ByRef vBatch_id As Object = Nothing, Optional ByRef vPMUser_id As Object = Nothing, Optional ByRef vConfirm_PMUser_id As Object = Nothing, Optional ByRef vConfirm2_PMUser_id As Object = Nothing, Optional ByRef vDate_Approved As Object = Nothing, Optional ByRef vBanking_Total As Object = Nothing, Optional ByRef vCash_Float_Amount As Object = Nothing, Optional ByRef vDepositDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue



        If (Informations.IsNothing(vCashListStatusID)) Or (Object.Equals(vCashListStatusID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCashListTypeID)) Or (Object.Equals(vCashListTypeID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCashListRef)) Or (Object.Equals(vCashListRef, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCompanyID)) Or (Object.Equals(vCompanyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vBankAccountID)) Or (Object.Equals(vBankAccountID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vCurrencyID)) Or (Object.Equals(vCurrencyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vListDate)) Or (Object.Equals(vListDate, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vControlTotal)) Or (Object.Equals(vControlTotal, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If



        If (Informations.IsNothing(vItemCount)) Or (Object.Equals(vItemCount, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the CashList for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vCashListID As Object = Nothing, Optional ByRef vCashListStatusID As Object = Nothing, Optional ByRef vCashListTypeID As Object = Nothing, Optional ByRef vCashListRef As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vBankAccountID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vListDate As Object = Nothing, Optional ByRef vControlTotal As Object = Nothing, Optional ByRef vItemCount As Object = Nothing, Optional ByRef vCashlist_drawer_id As Object = Nothing, Optional ByRef vBatch_id As Object = Nothing, Optional ByRef vPMUser_id As Object = Nothing, Optional ByRef vConfirm_PMUser_id As Object = Nothing, Optional ByRef vConfirm2_PMUser_id As Object = Nothing, Optional ByRef vDate_Approved As Object = Nothing, Optional ByRef vBanking_Total As Object = Nothing, Optional ByRef vCash_Float_Amount As Object = Nothing, Optional ByRef vDepositDate As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        Dim dbNumericTemp As Double
        If Not Double.TryParse(CStr(vCashListID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp2 As Double
        If Not Double.TryParse(CStr(vCashListStatusID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp3 As Double
        If Not Double.TryParse(CStr(vCashListTypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp4 As Double
        If Not Double.TryParse(CStr(vCompanyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp5 As Double
        If Not Double.TryParse(CStr(vBankAccountID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp6 As Double
        If Not Double.TryParse(CStr(vCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsDate(vListDate) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp7 As Double
        If Not Double.TryParse(CStr(vControlTotal), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim dbNumericTemp8 As Double
        If Not Double.TryParse(CStr(vItemCount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Public)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Public Function BeginTrans() As Integer

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("BeginTrans"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Public)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("CommitTrans"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Public)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

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



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("RollbackTrans"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

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
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    ' ***************************************************************** '
    ' Name: GetCurrencyDenom (Public)
    '
    ' Description: Selects the Currency Denominations list for the
    '              current Cash Drawer.
    '
    ' ***************************************************************** '
    Public Function GetCurrencyDenom(ByRef vCurrencyDenom(,) As Object, ByRef lCurrencyId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Currency ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CurrencyId", vValue:=CStr(lCurrencyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCurrencyDenominationsSQL, sSQLName:=ACGetCurrencyDenominationsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vCurrencyDenom)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return the Currency Denominations", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetCurrencyDenom"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetBankingItems (Public)
    '
    ' Description: Selects the full list of Banking Items for the
    '              current Cash Drawer.
    '
    ' ***************************************************************** '
    Public Function GetBankingItems(ByRef vBankingItems(,) As Object, ByRef lCashListID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Currency ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CashList_Id", vValue:=CStr(lCashListID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBankingItemsSQL, sSQLName:=ACGetBankingItemsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vBankingItems)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return the Banking Items", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetBankingItems"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Public Function AddAdjustment(ByVal v_lCashListID As Integer, ByVal v_lPMUserID As Integer, ByVal v_cAmount As Decimal, ByVal v_lAdjustMethod As Integer, ByVal v_sReason As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the CashList ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CashList_Id", vValue:=CStr(v_lCashListID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the adjustment_date parameter (INPUT)
            'developer guide no. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="adjustment_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the PMUser_id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="PMUser_id", vValue:=CStr(v_lPMUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Amount parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="amount", vValue:=CStr(v_cAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the cashlist_adjustment_method_id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_adjustment_method_id", vValue:=CStr(v_lAdjustMethod), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the reason parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="reason", vValue:=v_sReason, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACAddAdjustmentSQL, sSQLName:=ACAddAdjustmentName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add the Adjustment", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("AddAdjustment"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function CashFloat(ByRef blnCashFloat As Boolean, ByRef lCashListDrawerID As Integer) As Integer
        Dim result As Integer = 0
        Dim vCashFloat(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the cashList_drawer_id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashList_drawer_id", vValue:=CStr(lCashListDrawerID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckCashFloatSQL, sSQLName:=ACCheckCashFloatName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vCashFloat)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            blnCashFloat = CDbl(vCashFloat(0, 0)) = 1

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return the CashFloat Details", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("CashFloat"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetAdjustmentMethods(ByRef vAdjustMethods(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllAdjMethodsSQL, sSQLName:=ACGetAllAdjMethodsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vAdjustMethods)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return the Adjustment Methods", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetAdjustmentMethods"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function ListAdjustments(ByRef vAdjustments(,) As Object, ByRef lCashList_ID As Integer) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ''Dim sSQL As String
            ''    sSQL = "select " & _
            ''''        "c.[CashList_Adjustment_id]  ," & _
            ''''        "c.[cashlist_id]  ," & _
            ''''        "u.[username] , " & _
            ''''        "c.[amount] , " & _
            ''''        "c.[cashlist_adjustment_method_id], " & _
            ''''        "cb.[description]," & _
            ''''        "c.[adjustment_date], " & _
            ''''        "c.[reason], " & _
            ''''        "cd.[description] " & _
            ''''        "from CashList_Adjustment c ," & _
            ''''        "Cashlist_adjustment_method cb, " & _
            ''''        "Cashlist cl, " & _
            ''''        "Cashlist_drawer cd, " & _
            ''''        "PMUser u " & _
            ''''        "where c.[cashlist_id] = " & lCashList_ID
            ''    sSQL = sSQL & " And c.cashlist_adjustment_method_id = cb.cashlist_adjustment_method_id"
            ''    sSQL = sSQL & " And cl.cashlist_id = c.cashlist_id"
            ''    sSQL = sSQL & " And cd.cashlist_drawer_id = cl.cashlist_drawer_id"
            ''    sSQL = sSQL & " And (u.[user_id] = c.[PMUser_id] and u.is_deleted = 0)"

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the CashList ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CashListId", vValue:=CStr(lCashList_ID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAdjustmentsSQL, sSQLName:=ACGetAdjustmentsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vAdjustments)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return the Adjustments List", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("ListAdjustments"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetAdjustment(ByRef vAdjustments(,) As Object, ByRef lCashList_Adjustment_ID As Integer) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQL As String = ""
            ''    sSQL = "select " & _
            ''''        "c.[CashList_Adjustment_id]  ," & _
            ''''        "c.[cashlist_id]  ," & _
            ''''        "c.[PMUser_id] , " & _
            ''''        "c.[adjustment_date] , " & _
            ''''        "c.[amount] , " & _
            ''''        "c.[cashlist_adjustment_method_id], " & _
            ''''        "c.[reason], " & _
            ''''        "cd.[description] " & _
            ''''        "from CashList_Adjustment c , " & _
            ''''        "cashlist cl, " & _
            ''''        "cashlist_drawer cd " & _
            ''''        "where [CashList_Adjustment_id] = " & lCashList_Adjustment_ID & _
            ''''        " and c.[cashlist_id] = cl.[cashlist_id]" & _
            ''''        " and cl.[cashlist_drawer_id] = cd.[cashlist_drawer_id]"

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the CashList ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CashListAdjustmentID", vValue:=CStr(lCashList_Adjustment_ID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAdjustmentSQL, sSQLName:=ACGetAdjustmentName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vAdjustments)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return the Adjustment Data", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetAdjustment"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function SaveCash(ByVal v_lCashListID As Integer, ByVal v_lCurrencyDenomID As Integer, ByVal v_blnFloat As Boolean, ByVal v_lAmount As Integer) As Integer
        Dim result As Integer = 0
        Dim iFloat As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the CashList ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CashListID", vValue:=CStr(v_lCashListID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the CurrencyDenomID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CurrencyDenomID", vValue:=CStr(v_lCurrencyDenomID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_blnFloat Then
                iFloat = 1
            Else
                iFloat = 0
            End If

            ' Add the Float parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Float", vValue:=CStr(iFloat), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Amount parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Amount", vValue:=CStr(v_lAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ''Dim sSQL As String
            ''sSQL = "INSERT INTO CashList_Cash (" & _
            ''''    "[cashlist_id]  ," & _
            ''''    "[currency_denomination_id] , " & _
            ''''    "[is_float] , " & _
            ''''    "[amount] ) "
            ''sSQL = sSQL & " VALUES (" & _
            ''''    v_lCashListID & "," & _
            ''''    v_lCurrencyDenomID & "," & _
            ''''    iFloat & "," & _
            ''''    v_lAmount & ")"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACAddCashlistCashSQL, sSQLName:=ACAddCashlistCashName, bStoredProcedure:=True)
            ''    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, _
            ''''                                      sSQLName:="temporary!", _
            ''''                                      bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Save the Cash Data", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("SaveCash"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function LoadCash(ByRef vCashData(,) As Object, ByRef lCashListID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Cashlist ID parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CashlistID", vValue:=CStr(lCashListID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCashlistCashSQL, sSQLName:=ACGetCashlistCashName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vCashData)
            ''Dim sSQL As String
            ''sSQL = "SELECT currency_denomination_id,is_float,amount " & _
            ''''        "FROM CashList_Cash  " & _
            ''''        "Where (cashlist_id = " & lCashlistID & ")"
            ''
            ''    m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, _
            ''''                                      sSQLName:="Temporary", _
            ''''                                      bStoredProcedure:=True, _
            ''''                                      lNumberRecords:=PMAllRecords, _
            ''''                                      vResultArray:=vCashData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return the previous cash data", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("LoadCash"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetCashListStatus(ByRef vCashlistStatusCode As Object, ByRef lCashlistStatueID As Integer) As Integer
        Dim result As Integer = 0
        Dim vTempArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the CashlistStatusCode parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="CashlistStatusCode", vValue:=CStr(vCashlistStatusCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ''Dim sSQL As String
            ''sSQL = "select cashliststatus_id from cashliststatus where code = '" & vCashlistStatusCode & "'"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCashlistStatusSQL, sSQLName:=ACGetCashlistStatusName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vTempArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lCashlistStatueID = CInt(vTempArray(0, 0))

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return the CashList Status ID", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetCashListStatus"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetCashDrawerName(ByRef vCashListName As String, ByRef lCashDrawerID As Integer) As Integer
        Dim result As Integer = 0
        Dim vTempArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add the CashlistdrawerID parameter sw front office receipting 07/01/2003
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_drawer_id", vValue:=CStr(lCashDrawerID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectCashListDrawerSQL, sSQLName:=ACSelectCashListDrawerName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vTempArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            vCashListName = CStr(vTempArray(3, 0)) 'Return the Description
            vTempArray = Nothing 'destroy it

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return the CashDrawer Name", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetCashDrawerName"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetBankAccounts(ByVal v_lCashListID As Integer, Optional ByRef r_vCollectionAccount As Integer = 0, Optional ByRef r_vBankAccount As Integer = 0, Optional ByRef r_vAdjustmentAccount As Integer = 0, Optional ByRef r_vSuspenseAccount As Integer = 0) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetBankAccounts
        ' PURPOSE: Get account ids for a cashlist (bank account, suspense account, etc)
        ' AUTHOR: Paul Cunningham
        ' DATE: 13 December 2002, 09:45:40
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "GetBankAccounts"

        Dim vTempArray(,) As Object = Nothing

        'constants representing columns in vTempArray
        Const klBankAccountId As Integer = 0
        Const klCollectionAccountId As Integer = 1
        Const klAdjustmentAccountId As Integer = 2
        Const klSuspenseAccountId As Integer = 3


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Cashlist ID parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="lCashlistID", vValue:=CStr(v_lCashListID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to add parameter: @lCashlistID")
            End If

            'Go get those details
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGet_BankAccountIdsSQL, sSQLName:=ACGet_BankAccountIdsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vTempArray)

            'Handle the return
            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    If Informations.IsArray(vTempArray) Then
                        'Populate the paramters if passed

                        If Not Informations.IsNothing(r_vCollectionAccount) Then

                            r_vCollectionAccount = CInt(vTempArray(klCollectionAccountId, 0))
                        End If

                        If Not Informations.IsNothing(r_vBankAccount) Then

                            r_vBankAccount = CInt(vTempArray(klBankAccountId, 0))
                        End If

                        If Not Informations.IsNothing(r_vAdjustmentAccount) Then
                            'sw CQ 958 type error, we were previously reading collection account into adjustment account variable
                            'changed to adjustmentaccount SW 07/05/2003

                            r_vAdjustmentAccount = CInt(vTempArray(klAdjustmentAccountId, 0))
                        End If

                        If Not Informations.IsNothing(r_vSuspenseAccount) Then

                            r_vSuspenseAccount = CInt(vTempArray(klSuspenseAccountId, 0))
                        End If

                        result = gPMConstants.PMEReturnCode.PMTrue
                    Else
                        result = gPMConstants.PMEReturnCode.PMNotFound
                    End If

                Case Else
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get bank account details")

            End Select


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Informations.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=CInt(Informations.Err().Source), excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result

                Case Else
                    ' Log internal error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt(ACMethod), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

            End Select

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessBinder
    '
    ' Description: Function to create Transaction
    '              Copied from bACTInsurerPayment. pkh - 15/11/2002
    '
    '
    ' Steve Watton 13/01/2003 , added cashlistid and companyid as
    ' optional parameters for document creation
    '
    ' ***************************************************************** '
    Public Function ProcessBinder(ByVal v_lCreditAccountId As Integer, ByVal v_lDebitAccountId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cPayment As Decimal, Optional ByRef r_vCreditTransDetailId As Integer = 0, Optional ByRef r_vDebitTransDetailId As Integer = 0, Optional ByVal v_lCashListID As Integer = 0, Optional ByVal v_lCompanyID As Integer = 0, Optional ByVal v_lSubBranchID As Integer = 0, Optional ByVal v_bSecondApprove As Boolean = False, Optional ByVal v_vJournalType As String = "", Optional ByVal v_vCashListDrawerID As Integer = 0) As Integer
        ' KG 19/09/03 - Specify Journal & Cashlist Drawer

        Dim result As Integer = 0
        Try

            Dim sGroupCode As String = String.Empty, sRangeCode As String = String.Empty
            Dim lPeriodID, lDocumentId As Integer
            Dim sDocumentRef As String = ""
            Dim cBaseAmount, cCurrencyAmount As Decimal
            Dim dCurrencyBaseXRate As Double
            Dim lTransDetailID As Integer
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            Dim sReference As String = ""

            ' KG 19/09/03
            Dim iDebitBranchID, iDebitSubBranchID, iCreditBranchID, iCreditSubBranchID, iCashDrawerBranchID, iCashDrawerSubBranchID As Integer
            Dim vAccountResultArray(,) As Object = Nothing
            Dim vCashDrawerResultArray(,) As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oPeriod Is Nothing Then
                ' Get a new instance of component services

                ' Get an instance of match post

                m_oPeriod = New bACTPeriod.Form
                m_lReturn = m_oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_oExplorer Is Nothing Then
                ' Get a new instance of component services

                ' Get an instance of match post

                m_oExplorer = New bACTExplorer.Form
                m_lReturn = m_oExplorer.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_oDocument Is Nothing Then
                ' Get a new instance of component services

                ' Get an instance of match post

                m_oDocument = New bACTDocument.Form
                m_lReturn = m_oDocument.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End If


            If m_oTransdetail Is Nothing Then
                ' Get a new instance of component services

                ' Get an instance of match post

                m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oTransdetail, v_sClassName:="bACTTransDetail.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            If m_oAllocate Is Nothing Then
                ' Get a new instance of component services

                ' Get an instance of match post

                m_oAllocate = New bACTAllocate.Business
                m_lReturn = m_oAllocate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            'Get the Period


            m_lReturn = m_oPeriod.GetPostingPeriodForDate(dtDateInPeriod:=DateTime.Today, lPeriodID:=lPeriodID, lLedgerID:=gACTLibrary.ACTLedgerTypeCreditor)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return False
            End If

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = False
                Throw New Exception()
                Return result
            End If

            m_lReturn = CType(GetAutoNumValues(iDocumenttypeID:=gACTLibrary.ACTDocTypeJournal, sGroupCode:=sGroupCode, sRangeCode:=sRangeCode), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = False
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Throw New Exception()
            End If
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

            m_lReturn = m_oDocument.GenerateDocumentReferenceNumber(v_sGroupCode:=sGroupCode, v_sRangeCode:=sRangeCode, v_iUserID:=m_iUserID, v_iCompanyID:=If(False, 0, v_lCompanyID), r_sDocumentRef:=sReference)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = False
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Throw New Exception()
                Return result
            End If
            'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            If sReference <> "" Then
                sDocumentRef = sRangeCode & sReference
            End If
            'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)

            m_lReturn = m_oDocument.DirectAdd(vDocumentID:=lDocumentId, vCompanyID:=If(False, 0, v_lCompanyID), vPostingstatusID:=gACTLibrary.ACTPostStatusPosted, vDocumenttypeID:=gACTLibrary.ACTDocTypeJournal, vDocumentRef:=sDocumentRef, vDocumentDate:=DateTime.Today, vCreatedDate:=DateTime.Today, vAuthorisedDate:=DateTime.Today, vComment:=If(False, "", v_lCashListID))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = False
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Throw New Exception()
                Return result
            End If

            cCurrencyAmount = v_cPayment
            dCurrencyBaseXRate = 0


            m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=v_iCurrencyID, lCompanyID:=v_lCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cCurrencyAmount, vConversionDate:=DateTime.Today, vConversionRate:=dCurrencyBaseXRate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = False
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Throw New Exception()
                Return result
            End If

            ' CQ1030 - Set branch/sub Branch for the journals
            ' KG 19/09/03 - Get Account Details for Credit account

            m_lReturn = CType(GetAccountDetails(v_lCreditAccountId, vAccountResultArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFail
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            ' KG 19/09/03 - Get Branch,Sub Branch

            iCreditBranchID = CInt(vAccountResultArray(0, 0))

            iCreditSubBranchID = CInt(vAccountResultArray(1, 0))


            ' KG 19/09/03 - Get Account Details for Debit account

            m_lReturn = CType(GetAccountDetails(v_lDebitAccountId, vAccountResultArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFail
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            ' KG 19/09/03 - Get Branch,Sub Branch

            iDebitBranchID = CInt(vAccountResultArray(0, 0))

            iDebitSubBranchID = CInt(vAccountResultArray(1, 0))

            ' KG 19/09/03 - Get CashList Drawer Branch, Sub Branch

            m_lReturn = CType(GetCashDrawerDetails(v_vCashListDrawerID, vCashDrawerResultArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' KG 19/09/03 - Get Branch,Sub Branch

            iCashDrawerBranchID = CInt(vCashDrawerResultArray(0, 0))

            iCashDrawerSubBranchID = CInt(vCashDrawerResultArray(1, 0))


            ' KG 19/09/03
            ' Get credit/debit branches  sub branches
            ' Set default to Account Branch/Sub Branch - my aribritary choice..

            If Informations.IsNothing(v_vJournalType) Then
                ' Use account assigned values
            Else
                Select Case v_vJournalType
                    Case ACJournalBanking ' Banking
                        ' Use account assigned values for Debit
                        ' iDebitBranchID
                        ' iDebitSubBranchID

                        ' Use Cash Drawer values for Credit
                        iCreditBranchID = iCashDrawerBranchID
                        iCreditSubBranchID = iCashDrawerSubBranchID
                    Case ACJournalAdjustments ' Adjustments
                        ' Use Cash Drawer values for both Credit & Debit
                        iDebitBranchID = iCashDrawerBranchID
                        iDebitSubBranchID = iCashDrawerSubBranchID
                        iCreditBranchID = iCashDrawerBranchID
                        iCreditSubBranchID = iCashDrawerSubBranchID

                End Select
            End If

            'Generate Journal transaction to the Account to be Debitted (Added to!!)

            m_lReturn = m_oTransdetail.DirectAdd(vTransdetailID:=ToSafeInteger(lTransDetailID), vAccountID:=ToSafeInteger(v_lDebitAccountId), vPostingstatusID:=ToSafeInteger(gACTLibrary.ACTPostStatusPosted), vCompanyID:=ToSafeInteger(iDebitBranchID), vCurrencyID:=ToSafeInteger(v_iCurrencyID), vPeriodID:=ToSafeInteger(lPeriodID), vDocumentID:=ToSafeInteger(lDocumentId), vDocumentSequence:=2, vAccountingDate:=DateTime.Today, vAmount:=ToSafeDecimal(cBaseAmount), vBaseAmountUnrounded:=ToSafeDecimal(cBaseAmount), vFullyMatched:=-1, vCurrencyAmount:=ToSafeDecimal(cCurrencyAmount), vCurrencyAmountUnrounded:=ToSafeDecimal(cCurrencyAmount), vCurrencyBaseXrate:=ToSafeDouble(dCurrencyBaseXRate), vOperatorID:=ToSafeInteger(m_iUserID), vSubBranchID:=ToSafeInteger(iDebitSubBranchID))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = False
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Throw New Exception()
                Return result
            End If

            'Return debit transdetail id if required

            If Not Informations.IsNothing(r_vDebitTransDetailId) Then
                r_vDebitTransDetailId = lTransDetailID
            End If

            'eck290502 save suspense transaction
            '    lSuspTransDetailId = lTransDetailID


            'Generate Journal transaction to the Account to be Creditted (Taken Off!)

            m_lReturn = m_oTransdetail.DirectAdd(vTransdetailID:=ToSafeInteger(lTransDetailID), vAccountID:=ToSafeInteger(v_lCreditAccountId), vPostingstatusID:=ToSafeInteger(gACTLibrary.ACTPostStatusPosted), vCompanyID:=ToSafeInteger(iCreditBranchID), vCurrencyID:=ToSafeInteger(v_iCurrencyID), vPeriodID:=ToSafeInteger(lPeriodID), vDocumentID:=ToSafeInteger(lDocumentId), vDocumentSequence:=1, vAccountingDate:=DateTime.Today, vAmount:=cBaseAmount * -1, vBaseAmountUnrounded:=ToSafeDecimal(cBaseAmount * -1), vFullyMatched:=-1, vCurrencyAmount:=cCurrencyAmount * -1, vCurrencyAmountUnrounded:=cCurrencyAmount * -1, vCurrencyBaseXrate:=ToSafeDouble(dCurrencyBaseXRate), vOperatorID:=ToSafeInteger(m_iUserID), vSubBranchID:=ToSafeInteger(iCreditSubBranchID))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = False
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Throw New Exception()
                Return result
            End If

            'Return credit transdetail id if required

            If Not Informations.IsNothing(r_vCreditTransDetailId) Then
                r_vCreditTransDetailId = lTransDetailID
            End If

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = False
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Throw New Exception()
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessBinder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("ProcessBinder"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCashListAutoBankTotal
    '
    ' Description: get the auto bank total for the cashlist
    '
    ' Steve Watton 11/07/2003
    '
    '***************************************************************** '

    Public Function GetCashListAutoBankTotal(ByRef r_cAutoBankTotal As Decimal, ByRef v_lCashListID As Integer) As Integer
        Dim result As Integer = 0
        Dim vTempArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'add cash list id as param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_id", vValue:=CStr(v_lCashListID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCashListAutoBankTotalSQL, sSQLName:=ACGetCashListAutoBankTotalName, bStoredProcedure:=True, vResultArray:=vTempArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SMJB: CQ1951 28/07/03 Test for 0 records returned

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vTempArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                r_cAutoBankTotal = 0
            Else

                r_cAutoBankTotal = CDec(vTempArray(0, 0))
            End If
            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the auto bank total for the cashlist", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetCashListAutoBankTotal"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    Private Function GetAutoNumValues(ByVal iDocumenttypeID As Integer, ByRef sGroupCode As String, ByRef sRangeCode As String) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Select Case iDocumenttypeID
            Case 1
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeJn
            Case 2
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef2
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSdn
            Case 3
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef3
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeScn
            Case 4
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef4
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSnd
            Case 5
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef5
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSnc
            Case 6
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef6
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeCcr
            Case 7
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef7
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeCdr
            Case 8
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef8
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeACc
            Case 9
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef9
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodePpt
            Case 10
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef10
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeRvj
            Case 11
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef11
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDpj
            Case 12
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef12
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeRcj
            Case 13
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef13
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodePin
            Case 14
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef14
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSwd
            Case 15
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef15
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSrd
            Case 16
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef16
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSrc
            Case 17
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef17
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSed
            Case 18
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef18
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSec
            Case 19
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef19
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSat
            Case 20
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef20
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSaj
            Case 21
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef21
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSbd
            Case 22
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef22
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSrp
            Case 23
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef23
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSpy
            Case 24
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef24
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSin
            Case 25
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef25
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodePcn
            Case 26
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef26
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDia
            Case 27
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef27
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDir
            Case 28
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef28
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeClp
            Case 29
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef29
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeClr
            Case 30
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef30
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeFee
            Case 31
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef31
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeShd
            Case 32
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef32
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeShc
            Case 33
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef33
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDid
            Case 34
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef34
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeDic
            Case 35
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef35
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeTrd
            Case 36
                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef36
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeTrc
        End Select
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: GetBatchStatusDetails
    '
    ' Description: gets the batch status details
    '
    'steve watton front office receipting 26-11-2002
    '
    ' ***************************************************************** '

    Public Function GetBatchStatusDetails(ByVal v_lBatchID As Integer, ByRef r_sBatchStatusDescription As String) As Integer

        Dim result As Integer = 0
        Dim vTempArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the CashlistStatusCode parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=CStr(v_lBatchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBatchStatusDetailsSQL, sSQLName:=ACGetBatchStatusDetailsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vTempArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            r_sBatchStatusDescription = CStr(vTempArray(0, 0))

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return the batch details", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetBatchStatusDetails"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function





    ' ***************************************************************** '
    '
    ' Name: GetCashListStatusCode
    '
    ' Description: gets the cash list status code
    '
    'steve watton front office receipting 05-11-2002
    '
    ' ***************************************************************** '

    Public Function GetCashListStatusCode(ByVal v_lCashListID As Integer, ByRef r_sStatusCode As String) As Integer

        Dim result As Integer = 0
        Dim vTempArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the CashlistStatusCode parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistid", vValue:=CStr(v_lCashListID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCashListStatusCodeSQL, sSQLName:=ACGetCashListStatusCodeName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vTempArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vTempArray) Then

                r_sStatusCode = CStr(vTempArray(0, 0)).Trim()
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return the batch details", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetCashListStatusCode"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetMatchingDebits(ByVal v_lCashListID As Integer, ByRef r_vMatchingDebits(,) As Object, Optional ByVal v_vIsBanking As Object = Nothing) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetMatchingDebits
        ' PURPOSE: Gets the TransDetailIds and amounts of the matching debits for a cashlist
        ' AUTHOR: Paul Cunningham
        ' DATE: 16 December 2002, 09:38:39
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "GetMatchingDebits"


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Cashlist ID parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="lCashlistID", vValue:=CStr(v_lCashListID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to add parameter: @lCashlistID")
            End If


            If Not Informations.IsNothing(v_vIsBanking) Then
                ' Add the Cashlist ID parameter (INPUT)

                If m_oDatabase.Parameters.Add(sName:="bIsBanking", vValue:=CStr(v_vIsBanking), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to add parameter: @bIsBanking")
                End If
            End If

            'Go get those details
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetMatchingCashListDebitsSQL, sSQLName:=ACGetMatchingCashListDebitsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vMatchingDebits)

            'Handle the return
            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    If Informations.IsArray(r_vMatchingDebits) Then
                        result = gPMConstants.PMEReturnCode.PMTrue
                    Else
                        result = gPMConstants.PMEReturnCode.PMNotFound
                    End If

                Case Else
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get matching transaction debits")

            End Select


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Informations.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=CInt(Informations.Err().Source), excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt(ACMethod), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError


            End Select

        Finally


        End Try
        Return result
    End Function

    '*************************************************************************
    'Name:          GetPaymentMediaTypeIDs
    'Description:   Gets all the Media Types that support Payments
    'History:       25/11/2002 - TR - Created
    '*************************************************************************
    Public Function GetPaymentMediaTypeIDs(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Clear the Database Parameters Collection
            m_lReturn = CType(GetFilteredMediaTypeIDs(True, r_vResultArray), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception


            bPMFunc.LogMessage("", CStr(gPMConstants.PMELogLevel.PMLogOnError), CInt("GetPaymentMediaTypeIDs Failed"), ACApp, ACClass, "GetPaymentMediaTypeIDs", Informations.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '*************************************************************************
    'Name:          GetReceiptMediaTypeIDs
    'Description:   Gets all the Media Types that support Payments
    'History:       25/11/2002 - TR - Created
    '*************************************************************************
    Public Function GetReceiptMediaTypeIDs(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Clear the Database Parameters Collection
            m_lReturn = CType(GetFilteredMediaTypeIDs(False, r_vResultArray), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception


            bPMFunc.LogMessage("", CStr(gPMConstants.PMELogLevel.PMLogOnError), CInt("GetReceiptMediaTypeIDs Failed"), ACApp, ACClass, "GetReceiptMediaTypeIDs", Informations.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function


    '*************************************************************************
    'Name:          GetFilteredMediaTypeIDs
    'Description:   Gets all the Media Types that support Payments OR Receipts
    '               Could be extended to support other filters in the future
    'History:       05/02/2003 - TR - Created
    '*************************************************************************
    Private Function GetFilteredMediaTypeIDs(ByVal v_bGetPaymentTypes As Boolean, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0


        'TR - Assume success
        result = gPMConstants.PMEReturnCode.PMTrue

        'TR - Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        'TR - Are we searching for MediaTypes that support Payments or Receipts?
        If v_bGetPaymentTypes Then
            'TR - Add parameters to filter in Payment Types
            m_lReturn = m_oDatabase.Parameters.Add("PaymentsOnly", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
        Else
            'TR - Add parameters to filter in Receipt Types
            m_lReturn = m_oDatabase.Parameters.Add("ReceiptsOnly", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
        End If
        'TR - Make sure that this worked
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'TR - Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(ACSelectFilteredMediaTypesSQL, ACSelectFilteredMediaTypesName, ACSelectFilteredMediaTypesStored, vResultArray:=r_vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GETSUBBRANCHES
    '
    ' Description:
    '
    ' History:
    '
    ' ***************************************************************** '
    Private Function GetSubBranches(ByVal v_lSourceID As Integer, ByRef r_vSubBranchArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        Return SiriusCoreFunc.GetSubBranches(v_oDatabase:=m_oDatabase, v_lSourceID:=v_lSourceID, r_vSubBranchArray:=r_vSubBranchArray)

    End Function
    ' ***************************************************************** '
    '
    ' Name: GetAccountFromParty
    '
    ' Description: Gets an account id, for a party
    '
    ' History: 19/05/2004 Steve WAtton - Created.
    '                     virtual cut and paste from bSIRRoadMap
    '
    ' ***************************************************************** '

    Public Function GetAccountFromParty(ByVal v_lPartyCnt As Integer, ByVal v_lSourceID As Integer, ByRef r_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim vMultiCompany As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, 1, vMultiCompany)


            sSQL = "SELECT account_id FROM Account WHERE account_key = {account_key} "

            If ToSafeDouble(vMultiCompany) = 1 Then
                sSQL = sSQL & " AND company_id = {company_id}"
            End If

            ' Clear the paramters
            m_oDatabase.Parameters.Clear()

            ' Add the Account_ID and Company_id paramaters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_key", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If ToSafeDouble(vMultiCompany) = 1 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Execute the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountFromParty", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                ' Get the result (should only return 1 record now we are filtering by company_id

                r_lAccountID = CInt(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountFromParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetAccountFromParty"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetOSCashForDebit
    '
    ' Description: Gets Outstanding cas for the Debit just posted
    '
    ' History: 19/05/2004 Steve WAtton - Created.
    '                     virtual cut and paste from bSIRRoadMap
    '
    ' ***************************************************************** '
    Public Function GetOSCashForDebit(ByVal v_lAccountId As Integer, ByVal v_sDocumentRef As String, ByVal v_lSourceID As Integer, ByRef r_iCurrencyID As Integer, ByRef r_cCash As Decimal) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Construct the SQL
            'DC040305 PN19191 added new parameter for company id
            sSQL = "{call spu_ACT_Do_Get_OS_Cash_For_Client_Debit(?,?,?)}"

            ' Clear the paramters
            With m_oDatabase

                .Parameters.Clear()

                ' Add paramaters
                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="document_ref", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Parameters.Add(sName:="source_id", vValue:=CStr(v_lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute the SQL
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetOSCashForDebit", bStoredProcedure:=True, lNumberRecords:=2, vResultArray:=vResultArray)
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                ' Get the result

                r_iCurrencyID = CInt(vResultArray(0, 0))

                r_cCash = CDec(vResultArray(1, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOSCashForDebit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetOSCashForDebit"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetBranchBaseCurrency
    '
    ' Description:
    '
    ' History:
    '
    ' ***************************************************************** '

    Public Function GetBranchBaseCurrency(ByVal v_lSourceID As Integer, ByRef r_iCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return bPMFunc.GetBranchBaseCurrency(v_oDatabase:=m_oDatabase, v_lSourceID:=v_lSourceID, r_iCurrencyID:=r_iCurrencyID)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBranchBaseCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetBranchBaseCurrency"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ConvertPaymentAmount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-02-2006 : Claims Document Production
    ' ***************************************************************** '
    Public Function ConvertPaymentAmount(ByVal v_iCurrencyTo As Integer, ByVal v_vDocumentIds() As Object, ByVal v_lAccountId As Integer, ByRef r_crPaymentAmount As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ConvertPaymentAmount"

        Const kDocumentDetailPaymentAmount As Integer = 0
        Const kDocumentDetailCurrencyId As Integer = 1
        Const kThisDocument As Integer = 0

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lDocumentId, llBound, lUBound As Integer
        Dim crTotalPaymentAmount As Decimal
        Dim vDocumentDetails As Object = Nothing
        Dim iPaymentCurrency As Integer
        Dim crPaymentAmount, crConvertedAmount As Decimal

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if there are documents provided in the array
            If Informations.IsArray(v_vDocumentIds) Then

                ' get the arrays boundaries
                llBound = v_vDocumentIds.GetLowerBound(0)
                lUBound = v_vDocumentIds.GetUpperBound(0)

                ' for each document in the array
                For lDocument As Integer = llBound To lUBound

                    ' get the document id

                    lDocumentId = CInt(v_vDocumentIds(lDocument))

                    ' get the details for the document / account
                    lReturn = CType(GetDocumentDetails(v_lDocumentId:=lDocumentId, v_lAccountId:=v_lAccountId, r_vResults:=vDocumentDetails), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "GetDocumentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If Not Informations.IsArray(vDocumentDetails) Then
                        gPMFunctions.RaiseError(kMethodName, "GetDocumentDetails Failed to return any data", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' get the payment details from the document

                    crPaymentAmount = CDec(vDocumentDetails(kDocumentDetailPaymentAmount, kThisDocument))

                    iPaymentCurrency = CInt(vDocumentDetails(kDocumentDetailCurrencyId, kThisDocument))

                    ' convert document amount to new currency if required
                    If iPaymentCurrency <> v_iCurrencyTo Then

                        ' convert payment amount to new currency amount

                        lReturn = m_oCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=iPaymentCurrency, v_crCurrencyAmountFrom:=crPaymentAmount, v_lCompanyId:=m_iSourceID, v_lCurrencyIdTo:=v_iCurrencyTo, r_crCurrencyAmountTo:=crConvertedAmount)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CurrencyToCurrencyConversion Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' calculate the total payment amount in the case of multiple documents
                        crTotalPaymentAmount += CDbl(StringsHelper.Format(crConvertedAmount, "0.00"))

                    Else
                        crTotalPaymentAmount += CDbl(StringsHelper.Format(crPaymentAmount, "0.00"))
                    End If

                Next

                ' return the total payment amount in the specified currency
                r_crPaymentAmount = crTotalPaymentAmount

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetDocumentDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-02-2006 : Claims Document Production
    ' ***************************************************************** '
    Private Function GetDocumentDetails(ByVal v_lAccountId As Integer, ByVal v_lDocumentId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDocumentDetails"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = CType(AddInputParameter(v_sName:="account_id", v_vValue:=v_lAccountId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        m_lReturn = CType(AddInputParameter(v_sName:="document_id", v_vValue:=v_lDocumentId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute selection Query
        lReturn = m_oDatabase.SQLSelect(sSQL:=kGetDocumentDetailsSQL, sSQLName:=kGetDocumentDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kGetDocumentDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If



        Return result


    End Function

    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object

            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                        ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetCurrencyRateExist
    ' PURPOSE: Check Wheather currency Rate are defined or not.
    ' AUTHOR: Shankhdhar Dubey
    ' DATE: 27 June 2008
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Public Function CheckCurrencyRate(ByVal v_lCurrencyID As Integer, ByRef r_bCurrencyRateExist As Boolean) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "CheckCurrencyRate"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Currency_ID parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="CURRENCY_ID", vValue:=CStr(v_lCurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to add parameter: @CURRENCY_ID")
            End If

            If m_oDatabase.Parameters.Add(sName:="CurrencyExist", vValue:=CStr(r_bCurrencyRateExist), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to add parameter: @CurrencyExist")
            End If

            'Go get those details
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheck_CurrencyRatesSQL, sSQLName:=ACCheck_CurrencyRates, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords)

            r_bCurrencyRateExist = m_oDatabase.Parameters.Item("CurrencyExist").Value


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Informations.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=CInt(Informations.Err().Source), excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result

                Case Else
                    ' Log internal error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt(ACMethod), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

            End Select

        Finally

        End Try
        Return result

    End Function
    Public Function CheckClaimLink(ByVal v_lClaimPaymentId As Integer, ByRef r_bResults As Boolean) As Integer

        Const kMethodName As String = "CheckClaimLink"

        Dim lReturn As Integer
        Dim vResultArray(,) As Object = Nothing
        Try



            CheckClaimLink = gPMConstants.PMEReturnCode.PMTrue

            r_bResults = False

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="claimpayment_id", v_vValue:=v_lClaimPaymentId, v_iType:=gPMConstants.PMEDataType.PMLong)


            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckClaimLinkSQL, sSQLName:=ACCheckClaimLinkName, bStoredProcedure:=ACCheckClaimLinkStored, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Informations.IsArray(vResultArray) Then
                r_bResults = True
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CheckClaimLink, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return m_lReturn
    End Function

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
