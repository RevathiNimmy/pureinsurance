Option Strict Off
Option Explicit On
Imports SSP.Shared

Friend NotInheritable Class CLMThirdPartyRecoverys
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CLMThirdPartyRecoverys
    '
    ' Date: 24/08/2000
    '
    ' Description: Maintains the CLMThirdPartyRecoverys Collection.
    '
    '
    ' Edit History:Pandu
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "CLMThirdPartyRecoverys"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the CLMThirdPartyRecovery Collection
    Private m_CLMThirdPartyRecoverys As ArrayList

    ' Error Code
    Private m_lReturn As Integer
    ' PRIVATE Data Members (End)

    ' ************************************************
    ' Added to replace global variables 19/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single CLMThirdPartyRecovery into the CLMThirdPartyRecoverys Collection
    '
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function Add(ByRef oNewCLMThirdPartyRecovery As bCLMThirdParty.CLMThirdPartyRecovery) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the supplied CLMThirdPartyRecovery into the CLMThirdPartyRecoverys Collection
            ' Do not specify a key as we do not know the CLMThirdPartyRecovery ID for
            ' new ones entered until they are added to the DB.
            m_CLMThirdPartyRecoverys.Add(oNewCLMThirdPartyRecovery)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add CLMThirdPartyRecovery to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of CLMThirdPartyRecoverys in the collection.
    '
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try

            If m_CLMThirdPartyRecoverys.Count > 0 AndAlso m_CLMThirdPartyRecoverys.Item(0) Is Nothing Then
                Return m_CLMThirdPartyRecoverys.Count - 1
            Else
                Return m_CLMThirdPartyRecoverys.Count
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
    ' Description: Delete a CLMThirdPartyRecovery from the Collection.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As Integer)

        Try

            m_CLMThirdPartyRecoverys.Remove(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected CLMThirdPartyRecovery from the Collection.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As Integer) As bCLMThirdParty.CLMThirdPartyRecovery

        Dim result As bCLMThirdParty.CLMThirdPartyRecovery = Nothing
        Try


            Return m_CLMThirdPartyRecoverys(vKey)

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all CLMThirdPartyRecoverys from the Collection
    '
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim lFieldCount As Integer

        Try

            ' Determine the number of CLMThirdPartyRecovery in the collection
            lFieldCount = Count()

            ' Delete all the items
            For lSub As Integer = 1 To lFieldCount
                Delete(1)
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
    ' Description: Clear the CLMThirdPartyRecovery Collection.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set CLMThirdPartyRecovery Collection to Nothing
            m_CLMThirdPartyRecoverys = Nothing


            ' Added by Scalability Update Program - 31/07/2002
            m_CLMThirdPartyRecoverys = New ArrayList()

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
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

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
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
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
            m_CLMThirdPartyRecoverys = New ArrayList()

            ' Class Initialise

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the CLMThirdPartyRecovery class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
