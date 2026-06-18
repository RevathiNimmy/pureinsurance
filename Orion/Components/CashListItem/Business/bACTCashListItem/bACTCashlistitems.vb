Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no.129
<System.Runtime.InteropServices.ProgId("Cashlistitems_NET.Cashlistitems")>
Public NotInheritable Class Cashlistitems

    Implements IDisposable
    ' ************************************************
    ' Added to replace global variables 03/04/2007
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
    ' Class Name: Cashlistitems
    '
    ' Date: 07/04/1998
    '
    ' Description: Maintains the Cashlistitems Collection.
    '
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Cashlistitems"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Define the Cashlistitem Collection
    Private m_Cashlistitems As ArrayList
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single Cashlistitem into the Cashlistitems Collection
    '
    '
    ' ***************************************************************** '
    Public Function Add(ByRef oNewCashlistitem As bACTCashlistitem.Cashlistitem) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the supplied Cashlistitem into the Cashlistitems Collection
            ' Do not specify a key as we do not know the Cashlistitem ID for
            ' new ones entered until they are added to the DB.
            m_Cashlistitems.Add(oNewCashlistitem)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Cashlistitem to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of Cashlistitems in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try

            If m_Cashlistitems.Count > 0 AndAlso m_Cashlistitems.Item(0) Is Nothing Then
                Return m_Cashlistitems.Count - 1
            Else
                Return m_Cashlistitems.Count
            End If


        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a Cashlistitem from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As Integer)

        Try

            m_Cashlistitems.Remove(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected Cashlistitem from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As Integer) As bACTCashlistitem.Cashlistitem

        Dim result As bACTCashlistitem.Cashlistitem = Nothing
        Try


            Return m_Cashlistitems(vKey)

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all Cashlistitems from the Collection
    '
    '
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim lFieldCount As Integer

        Try

            ' Determine the number of Cashlistitem in the collection
            lFieldCount = Count()

            ' Delete all the items
            For lSub As Integer = 1 To lFieldCount
                Delete(1)
            Next lSub

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete All Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAll", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the Cashlistitem Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set Cashlistitem Collection to Nothing
            m_Cashlistitems = Nothing


            ' Added by Scalability Update Program - 22/07/2002
            m_Cashlistitems = New ArrayList()

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            ' Initialisation Code.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            m_Cashlistitems = New ArrayList()

            ' Class Initialise

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the Cashlistitem class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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