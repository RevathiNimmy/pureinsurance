Option Strict Off
Option Explicit On
'developer guide no.129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("CashLists_NET.CashLists")>
Public NotInheritable Class CashLists

    Implements IDisposable
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

    ' ***************************************************************** '
    ' Class Name: CashLists
    '
    ' Date: 03/09/1997
    '
    ' Description: Maintains the CashLists Collection.
    '
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "CashLists"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the CashList Collection
    Private m_CashLists As ArrayList
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single CashList into the CashLists Collection
    '
    '
    ' ***************************************************************** '
    Public Function Add(ByRef oNewCashList As bACTCashList.CashList) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the supplied CashList into the CashLists Collection
            ' Do not specify a key as we do not know the CashList ID for
            ' new ones entered until they are added to the DB.
            m_CashLists.Add(oNewCashList)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add CashList to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Add"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of CashLists in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try

            If m_CashLists.Count > 0 AndAlso m_CashLists.Item(0) Is Nothing Then
                Return m_CashLists.Count - 1
            Else
                Return m_CashLists.Count
            End If

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Count"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a CashList from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As Integer)

        Try

            m_CashLists.Remove(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Delete"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected CashList from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As Integer) As bACTCashList.CashList

        Dim result As bACTCashList.CashList = Nothing
        Try


            Return m_CashLists(vKey)

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Item"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all CashLists from the Collection
    '
    '
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim lFieldCount As Integer

        Try

            ' Determine the number of CashList in the collection
            lFieldCount = Count()

            ' Delete all the items
            For lSub As Integer = 1 To lFieldCount
                Delete(0)
            Next lSub

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete All Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("DeleteAll"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the CashList Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set CashList Collection to Nothing
            m_CashLists = Nothing


            ' Added by Scalability Update Program - 22/07/2002
            m_CashLists = New ArrayList()

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Clear"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

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

            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            '  m_sUsername$ = sUsername$
            '   m_sPassword$ = sPassword$
            '   m_iUserID% = iUserID%
            '   m_sCallingAppName$ = sCallingAppName$
            '   m_iLanguageID% = iLanguageID%
            '   m_iSourceID% = iSourceID%
            '   m_iCurrencyID% = iCurrencyID%
            '   m_iLogLevel% = iLogLevel%


            ' Initialisation Code.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

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
            m_CashLists = New ArrayList()

            ' Class Initialise

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the CashList class", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Class_Initialise"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

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