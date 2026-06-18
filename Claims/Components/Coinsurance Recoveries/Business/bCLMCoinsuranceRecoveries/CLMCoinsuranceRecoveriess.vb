Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developer Guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Recoveriess_NET.Recoveriess")> _
Public NotInheritable Class Recoveriess
    Implements IDisposable
    ' ************************************************
    ' Added to replace global variables 11/12/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private m_oDatabase As dPMDAO.Database
    ' ************************************************

    ' ***************************************************************** '
    ' Class Name: CLMCoinsuranceRecoveriess
    '
    ' Date: {TodaysDate}
    '
    ' Description: Maintains the CLMCoinsuranceRecoveriess Collection.
    '
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "CLMCoinsuranceRecoveriess"


    ' Define the CLMCoinsuranceRecoveries Collection
    Private m_CLMCoinsuranceRecoveriess As Collection

    ' Error Code
    Private m_lReturn As Integer


    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single CLMCoinsuranceRecoveries into the CLMCoinsuranceRecoveriess Collection
    '
    '
    ' ***************************************************************** '
    Public Function Add(ByRef oNewCLMCoinsuranceRecoveries As bCLMCoinsuranceRecoveries.Recoveries) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the supplied CLMCoinsuranceRecoveries into the CLMCoinsuranceRecoveriess Collection
            ' Do not specify a key as we do not know the CLMCoinsuranceRecoveries ID for
            ' new ones entered until they are added to the DB.
            m_CLMCoinsuranceRecoveriess.Add(oNewCLMCoinsuranceRecoveries)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add CLMCoinsuranceRecoveries to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of CLMCoinsuranceRecoveriess in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try


            Return m_CLMCoinsuranceRecoveriess.Count

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a CLMCoinsuranceRecoveries from the Collection.
    '
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Sub Delete(ByRef vKey As Object)

        Try

            m_CLMCoinsuranceRecoveriess.Remove(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected CLMCoinsuranceRecoveries from the Collection.
    '
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function Item(ByRef vKey As Object) As bCLMCoinsuranceRecoveries.Recoveries

        Dim result As bCLMCoinsuranceRecoveries.Recoveries = Nothing
        Try


            Return m_CLMCoinsuranceRecoveriess(vKey)

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all CLMCoinsuranceRecoveriess from the Collection
    '
    '
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim lFieldCount As Integer

        Try

            ' Determine the number of CLMCoinsuranceRecoveries in the collection
            lFieldCount = Count()

            ' Delete all the items
            For lSub As Integer = 1 To lFieldCount
                Delete(gPMConstants.PMEReturnCode.PMTrue)
            Next lSub

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete All Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAll", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the CLMCoinsuranceRecoveries Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set CLMCoinsuranceRecoveries Collection to Nothing
            m_CLMCoinsuranceRecoveriess = Nothing


            ' Added by Scalability Update Program - 31/07/2002
            m_CLMCoinsuranceRecoveriess = New Collection()

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function Initialise(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, Optional ByVal v_vDatabase As dPMDAO.Database = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = v_sUsername
            m_sPassword = v_sPassword
            m_iUserID = v_iUserID
            m_sCallingAppName = v_sCallingAppName
            m_iLanguageID = v_iLanguageID
            m_iSourceID = v_iSourceID
            m_iCurrencyID = v_iCurrencyID
            m_iLogLevel = v_iLogLevel
            m_oDatabase = v_vDatabase

            Return result

        Catch excep As System.Exception



            ' Error.

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
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Sub New()
        MyBase.New()

        Try

            ' Added by Scalability Update Program - 31/07/2002
            m_CLMCoinsuranceRecoveriess = New Collection()

            ' Class Initialise

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the CLMCoinsuranceRecoveries class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class