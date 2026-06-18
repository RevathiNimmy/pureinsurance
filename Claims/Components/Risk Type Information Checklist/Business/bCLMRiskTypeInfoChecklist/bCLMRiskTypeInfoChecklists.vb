Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developer Guide no. 129
Imports SharedFiles
Friend NotInheritable Class CLMRTInfoChklsts
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name:   CLMRTInfoChklsts
    ' Description:  Maintains the CLMRTInfoChklsts Collection.
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "CLMRTInfoChklsts"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the CLMRTInfoChklst Collection
    Private m_CLMRTInfoChklsts As Collection

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

    ' Error Code
    Private m_lReturn As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name:         Add
    ' Description:  Adds a single CLMRTInfoChklst into the
    '               CLMRiskTypeInfo-Checklists Collection
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function Add(ByRef oNewCLMRiskTypeInfoChecklist As bCLMRTInfoChkLst.CLMRTInfoChklst) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the supplied CLMRTInfoChklst into the CLMRTInfoChklsts Collection
            ' Do not specify a key as we do not know the CLMRTInfoChklst ID for
            ' new ones entered until they are added to the DB.
            m_CLMRTInfoChklsts.Add(oNewCLMRiskTypeInfoChecklist)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add CLMRTInfoChklst to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         Count
    ' Description:  Returns the number of CLMRTInfoChklsts in the
    '               collection.
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try
            'returns the no. of items in the collection

            Return m_CLMRTInfoChklsts.Count

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
    ' Description: Delete a CLMRTInfoChklst from the Collection.
    '
    '
    ' ***************************************************************** '
    '##ModelId=39629EDF02DE
    Public Sub Delete(ByRef vKey As Integer)

        Try

            m_CLMRTInfoChklsts.Remove(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name:         Item
    ' Description:  Returns the selected CLMRTInfoChklst from the
    '               Collection.
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As Integer) As bCLMRTInfoChkLst.CLMRTInfoChklst

        Dim result As bCLMRTInfoChkLst.CLMRTInfoChklst = Nothing
        Try


            Return m_CLMRTInfoChklsts(vKey)

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all CLMRTInfoChklsts from the Collection
    '
    '
    ' ***************************************************************** '
    '##ModelId=39629EDF02F1
    Public Sub DeleteAll()

        Dim lFieldCount As Integer

        Try

            ' Determine the number of CLMRTInfoChklst in the collection
            lFieldCount = Count()

            ' Delete all the items
            For lSub As Integer = 1 To lFieldCount
                Delete(1)
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
    ' Description: Clear the CLMRTInfoChklst Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set CLMRTInfoChklst Collection to Nothing
            m_CLMRTInfoChklsts = Nothing


            ' Added by Scalability Update Program - 31/07/2002
            m_CLMRTInfoChklsts = New Collection()

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
    '##ModelId=39629EDF02FB
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.
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
    '##ModelId=39629EDF0305
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


    '##ModelId=39629EDF0306
    Public Sub New()
        MyBase.New()

        Try

            ' Added by Scalability Update Program - 31/07/2002
            m_CLMRTInfoChklsts = New Collection()

            ' Class Initialise

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the CLMRTInfoChklst class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    '##ModelId=39629EDF030F
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
