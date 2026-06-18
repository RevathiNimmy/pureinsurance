Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no.129
<System.Runtime.InteropServices.ProgId("BankAccounts_NET.BankAccounts")> _
Public NotInheritable Class BankAccounts

    Implements IDisposable
    ' ************************************************
    ' Added to replace global variables 07/10/2003
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

    ' ***************************************************************** '
    ' Class Name: BankAccounts
    '
    ' Date: 09/09/1997
    '
    ' Description: Maintains the BankAccounts Collection.
    '
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "BankAccounts"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the BankAccount Collection
    Private m_BankAccounts As ArrayList
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single BankAccount into the BankAccounts Collection
    '
    '
    ' ***************************************************************** '
    Public Function Add(ByRef oNewBankAccount As bACTBankAccount.BankAccount) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the supplied BankAccount into the BankAccounts Collection
            ' Do not specify a key as we do not know the BankAccount ID for
            ' new ones entered until they are added to the DB.
            m_BankAccounts.Add(oNewBankAccount)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add BankAccount to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of BankAccounts in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try

            If m_BankAccounts.Count > 0 AndAlso m_BankAccounts.Item(0) Is Nothing Then
                Return m_BankAccounts.Count - 1
            Else
                Return m_BankAccounts.Count
            End If

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a BankAccount from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As Integer)

        Try

            m_BankAccounts.RemoveAt(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected BankAccount from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As Integer) As bACTBankAccount.BankAccount

        Dim result As bACTBankAccount.BankAccount = Nothing
        Try


            Return m_BankAccounts(vKey)

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all BankAccounts from the Collection
    '
    '
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim lFieldCount As Integer

        Try

            ' Determine the number of BankAccount in the collection
            lFieldCount = Count()

            ' Delete all the items
            For lSub As Integer = 1 To lFieldCount
                Delete(0)
            Next lSub

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete All Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the BankAccount Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set BankAccount Collection to Nothing
            m_BankAccounts = Nothing


            ' Added by Scalability Update Program - 22/07/2002
            m_BankAccounts = New ArrayList()

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

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

            Return result

        Catch excep As System.Exception



            ' Error.

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
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


    Friend Sub New()
        MyBase.New()

        Try

            ' Added by Scalability Update Program - 22/07/2002
            m_BankAccounts = New ArrayList()

            ' Class Initialise

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the BankAccount class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class