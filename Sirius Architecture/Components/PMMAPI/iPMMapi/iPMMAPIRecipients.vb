Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Modified by Vijay Pal on 5/19/2010 10:35:20 AM refer developer guide no. 129
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Recipients_NET.Recipients")> _
Public NotInheritable Class Recipients
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Recipients
    '
    ' Date: 23-07-1997
    '
    ' Description: Maintains the Recipients Collection.
    '
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Recipients"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_oFunctions As Functions

    ' PRIVATE Data Members (Begin)
    ' Define the Recipient Collection
    Private m_Recipients As Collection
    ' PRIVATE Data Members (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single Recipient into the Recipients Collection
    '
    '
    ' ***************************************************************** '
    Public Function Add(ByRef oNewRecipient As Recipient) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(oNewRecipient.Initialise(m_oFunctions), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' Add the supplied Recipient into the Recipients Collection
            ' Do not specify a key as we do not know the Recipient ID for
            ' new ones entered until they are added to the DB.
            m_Recipients.Add(oNewRecipient)

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Recipient to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddRecipient
    '
    ' Description: Adds a single Recipient into the Recipients Collection
    '
    '
    ' ***************************************************************** '
    Public Function AddRecipient(Optional ByVal v_vAddress As String = "", Optional ByVal v_vName As String = "", Optional ByVal v_vRecipientType As gPMConstants.PMEMapiRecipientTypes = 0, Optional ByVal v_vAddressType As String = "", Optional ByVal v_vAddressBook As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oRcp As Recipient

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oRcp = New Recipient()


            If Not Information.IsNothing(v_vAddress) Then
                oRcp.Address = v_vAddress
            End If


            If Not Information.IsNothing(v_vName) Then
                oRcp.Name = v_vName
            End If


            If Not Information.IsNothing(v_vRecipientType) Then
                oRcp.RecipientType = v_vRecipientType
            Else
                oRcp.RecipientType = gPMConstants.PMEMapiRecipientTypes.pmeMapiToList
            End If


            If Not Information.IsNothing(v_vAddressType) Then
                oRcp.AddressType = v_vAddressType
            End If


            If Not Information.IsNothing(v_vAddressBook) Then
                oRcp.AddressBook = v_vAddressBook
            End If

            m_lReturn = CType(Add(oRcp), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Recipient to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRecipient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Count
    '
    ' Description: Returns the number of Recipients in the collection.
    '
    '
    ' ***************************************************************** '
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try


            Return m_Recipients.Count

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete
    '
    ' Description: Delete a Recipient from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Delete(ByRef vKey As gPMConstants.PMEReturnCode)

        Try

            m_Recipients.Remove(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected Recipient from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function Item(ByRef vKey As gPMConstants.PMEReturnCode) As Recipient

        Dim result As Recipient = Nothing
        Try


            Return m_Recipients(vKey)

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAll
    '
    ' Description: Delete all Recipients from the Collection
    '
    '
    ' ***************************************************************** '
    Public Sub DeleteAll()

        Dim lFieldCount As Integer

        Try

            ' Determine the number of Recipient in the collection
            lFieldCount = Count()

            ' Delete all the items
            For lSub As Integer = 1 To lFieldCount
                Delete(gPMConstants.PMEReturnCode.PMTrue)
            Next lSub

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete All Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAll", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the Recipient Collection.
    '
    '
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set Recipient Collection to Nothing
            m_Recipients = Nothing
            m_Recipients = New Collection()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function Initialise(ByRef oFunctions As Object) As Integer




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.

            m_oFunctions = oFunctions

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    ' ***************************************************************** '
    ' Name: LastItem
    '
    ' Description: Returns the last Recipient from the Collection.
    '
    '
    ' ***************************************************************** '
    Public Function LastItem() As Recipient

        Dim result As Recipient = Nothing
        Try


            Return m_Recipients(m_Recipients.Count)

        Catch excep As System.Exception




            ' Log Error Recipient
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return LastItem", vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


    Friend Sub New()
        MyBase.New()

        Try

            ' Class Initialise

            m_Recipients = New Collection()

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the Recipient class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
