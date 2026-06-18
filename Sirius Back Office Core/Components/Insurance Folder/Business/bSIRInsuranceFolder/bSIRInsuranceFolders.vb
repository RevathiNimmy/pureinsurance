Option Strict Off
Option Explicit On
'Developer Guide No. 129
Imports SSP.Shared

Friend NotInheritable Class SIRInsuranceFolders
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRInsuranceFolders
    '
    ' Date: 10/09/1998
    '
    ' Description: Maintains the SIRInsuranceFolders Collection.
    '
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "SIRInsuranceFolders"

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


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the SIRInsuranceFolder Collection
    Private m_SIRInsuranceFolders As ArrayList

    ' Error Code
    Private m_lReturn As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single SIRInsuranceFolder into the SIRInsuranceFolders Collection
    '
    '
    ' ***************************************************************** '
    Public Function Add(ByRef oNewSIRInsuranceFolder As bSIRInsuranceFolder.SIRInsuranceFolder) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the supplied SIRInsuranceFolder into the SIRInsuranceFolders Collection
            ' Do not specify a key as we do not know the SIRInsuranceFolder ID for
            ' new ones entered until they are added to the DB.
            m_SIRInsuranceFolders.Add(oNewSIRInsuranceFolder)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add SIRInsuranceFolder to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of SIRInsuranceFolders in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try

            If m_SIRInsuranceFolders.Count > 0 AndAlso m_SIRInsuranceFolders.Item(0) Is Nothing Then
                Return m_SIRInsuranceFolders.Count - 1
            Else
                Return m_SIRInsuranceFolders.Count
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
    ' Description: Delete a SIRInsuranceFolder from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As Integer)

        Try

            m_SIRInsuranceFolders.Remove(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected SIRInsuranceFolder from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As Integer) As bSIRInsuranceFolder.SIRInsuranceFolder

        Dim result As bSIRInsuranceFolder.SIRInsuranceFolder = Nothing
        Try


            Return m_SIRInsuranceFolders(vKey)

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all SIRInsuranceFolders from the Collection
    '
    '
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim lFieldCount As Integer

        Try

            ' Determine the number of SIRInsuranceFolder in the collection
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
    ' Description: Clear the SIRInsuranceFolder Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set SIRInsuranceFolder Collection to Nothing
            m_SIRInsuranceFolders = Nothing


            ' Added by Scalability Update Program - 30/07/2002
            m_SIRInsuranceFolders = New ArrayList()

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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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
            m_SIRInsuranceFolders = New ArrayList()

            ' Class Initialise

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the SIRInsuranceFolder class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
