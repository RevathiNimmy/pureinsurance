Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared
Friend NotInheritable Class SIRPartyPCs

    Implements IDisposable
    ' ************************************************
    ' Added to replace global variables 27/11/2003
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
    ' Class Name: SIRPartyPCs
    '
    ' Date: 12/10/1998
    '
    ' Description: Maintains the SIRPartyPCs Collection.
    '
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "SIRPartyPCs"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the SIRPartyPC Collection
    '    Private m_SIRPartyPCs As Collection
    Private m_SIRPartyPCs As New List(Of Object)


    ' Error Code
    Private m_lReturn As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single SIRPartyPC into the SIRPartyPCs Collection
    '
    '
    ' ***************************************************************** '
    Public Function Add(ByRef oNewSIRPartyPC As bSIRPartyPC.SIRPartyPC) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the supplied SIRPartyPC into the SIRPartyPCs Collection
            ' Do not specify a key as we do not know the SIRPartyPC ID for
            ' new ones entered until they are added to the DB.
            m_SIRPartyPCs.Add(oNewSIRPartyPC)

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add SIRPartyPC to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of SIRPartyPCs in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try

            If m_SIRPartyPCs.Count > 0 AndAlso m_SIRPartyPCs.Item(0) Is Nothing Then
                Return m_SIRPartyPCs.Count - 1
            Else
                Return m_SIRPartyPCs.Count
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
    ' Description: Delete a SIRPartyPC from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As gPMConstants.PMEReturnCode)

        Try

            m_SIRPartyPCs.Remove(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected SIRPartyPC from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As gPMConstants.PMEReturnCode) As bSIRPartyPC.SIRPartyPC

        Dim result As bSIRPartyPC.SIRPartyPC = Nothing
        Try


            Return m_SIRPartyPCs(vKey)

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all SIRPartyPCs from the Collection
    '
    '
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim lFieldCount As Integer

        Try

            ' Determine the number of SIRPartyPC in the collection
            lFieldCount = Count()

            ' Delete all the items
            For lSub As Integer = 1 To lFieldCount
                Delete(gPMConstants.PMEReturnCode.PMTrue)
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
    ' Description: Clear the SIRPartyPC Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set SIRPartyPC Collection to Nothing
            m_SIRPartyPCs = Nothing


            ' Added by Scalability Update Program - 30/07/2002
            'm_SIRPartyPCs = New Collection()
            m_SIRPartyPCs = New List(Of Object)


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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String) As Integer

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


    Public Sub New()
        MyBase.New()

        Try

            ' Added by Scalability Update Program - 30/07/2002
            ' m_SIRPartyPCs = New Collection()
            m_SIRPartyPCs = New List(Of Object)

            ' Class Initialise

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the SIRPartyPC class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
