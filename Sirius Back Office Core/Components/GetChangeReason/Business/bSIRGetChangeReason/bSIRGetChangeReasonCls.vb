Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    ' ************************************************
    ' Added to replace global variables 10/12/2003
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
    ' Class Name: FindAccount
    '
    ' Date: 06 May 1997
    '
    ' Description: Creatable class used by the FindAccount interface.
    '
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    Private Const ACIFullKey As Integer = 0
    Private Const ACIShortCode As Integer = 1
    Private Const ACIAccountName As Integer = 2
    Private Const ACILedgerID As Integer = 3
    Private Const ACIAccountType As Integer = 4
    Private Const ACIAccountID As Integer = 5

    ' Constants for use in SQL
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lError As gPMConstants.PMEReturnCode

    ' Task
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As New FixedLengthString(10)
    Private m_dtEffectiveDate As Date

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Component Sub Type
    Private m_sSubType As New FixedLengthString(20)
    ' Variable Data Business Component (Private)

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    ' Account Explorer for LongKeys
    Private m_oExplorer As Object

    ' NavigatorV3 variables
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFOrion

        End Get
    End Property

    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    ' PUBLIC Property Procedures (End)

    ' PUBLIC Methods (Begin)

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
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "SetProcessModes Failed", MainModule.ACApp, ACClass, "SetProcessModes", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values as defined by vTableArray.
    '
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'developer guide no. 12
            vResultArray = Nothing

            ' Get the Lookup items from the Business Component

            m_lReturn = m_oLookup.GetLookupValues(iLookupType, vTableArray, iLanguageID, m_dtEffectiveDate, vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "GetLookupValues Failed", MainModule.ACApp, ACClass, "GetLookupValues", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetKeys
    '
    ' Description: Navigator SetKeys function.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "SetKeysFailed", MainModule.ACApp, ACClass, "SetKeys", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys
    '
    ' Description: Navigator GetKeys function.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no. 12
            vKeyArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "GetKeysFailed", MainModule.ACApp, ACClass, "GetKeys", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Navigator Start function. Entry point into Navigator.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "StartFailed", MainModule.ACApp, ACClass, "Start", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function


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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, PMProductFamily, m_bCloseDatabase, m_oDatabase, vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password
            m_iUserID = iUserID

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType.Value = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business Object passing our Database Reference.
            'developer guide no.  Replace m_oDatabase by vDatabase:=m_oDatabase
            m_lReturn = m_oLookup.Initialise(sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, MainModule.ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

            m_lReturn = CType(gPMComponentServices.CreateBusinessObject(m_oExplorer, "bACTExplorer.Form", MainModule.ACApp, sUsername, sPassword, iUserID, iSourceID, iLanguageID, iCurrencyID, iLogLevel, m_oDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "Initialise Failed", MainModule.ACApp, ACClass, "Initialise", Information.Err().Number, excep.Message, excep:=excep)

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
                m_oLookup = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    'Public Function SelectBankQuery(ByRef lNumberOfRecords As Long, vResultArray As Variant, _
    ''    Optional ByVal vShortCode As Variant, Optional ByVal vBankName As Variant)
    '
    '     Call SP "spu_SIR_Select" & sTransactionType & "EventDescription"
    '    Returning data into r_vReasonData
    '


    '****************************************************************** '
    '* Name: GetReasonData (Public)
    '*
    '* Description: Get the data from the appropriate Event Description table
    '*
    '****************************************************************** '

    Public Function GetReasonData(ByVal lproductID As Integer, ByVal sTransactionType As String, ByRef r_vReasonData As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("product_id", CStr(lproductID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement
            Select Case sTransactionType.Trim().ToUpper()
                Case "MTA"

                    m_lError = m_oDatabase.SQLSelect(ACMTAEventDescriptionSQL, ACMTAEventDescriptionName, ACMTAEventDescriptionStored, gPMConstants.PMAllRecords, r_vReasonData)
                Case "CLAIM"

                    m_lError = m_oDatabase.SQLSelect(ACClaimEventDescriptionSQL, ACClaimEventDescriptionName, ACClaimEventDescriptionStored, gPMConstants.PMAllRecords, r_vReasonData)
            End Select

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogError, "m_oDatabase.SQLSelect failed", MainModule.ACApp, ACClass, "GetReasonData")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' If NO records were found return PMNotFound
            If Not Information.IsArray(r_vReasonData) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "GetReasonData Failed", MainModule.ACApp, ACClass, "GetReasonData", Information.Err().Number, excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)


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
        'gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogOnError, "Class_Initialize Failed", MainModule.ACApp, ACClass, "Class_Initialize", CStr(Information.Err().Number), excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
