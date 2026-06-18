Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Text
'Developer Guide no. 129
Imports SharedFiles
Friend NotInheritable Class CLMRecoverys
    Implements IDisposable
    Implements IEnumerable
    ' ***************************************************************** '
    ' Class Name: CLMRecoverys
    '
    ' Date: 24/08/2000
    '
    ' Description: Maintains the CLMRecoverys Collection.
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "CLMRecoverys"

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
    Private m_oDatabase As dPMDAO.Database
    ' ************************************************

    ' Collection
    Private m_cRecoverys As Collection


    ' Should be post taxes for this risk_type?
    Public IsPostTaxes As Boolean


    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '
    Public ReadOnly Property Comments() As String
        Get

            Dim sSeperator As String = ""
            Dim sComments As New StringBuilder

            For Each oRecovery As CLMRecovery In m_cRecoverys
                If oRecovery.ThisReceipt <> 0 Then
                    sComments.Append(sSeperator & oRecovery.RecoveryType)
                    sSeperator = ","
                End If
            Next oRecovery

            ' Return comments
            Return sComments.ToString()

        End Get
    End Property

    Public ReadOnly Property ThisReceiptTotal() As Decimal
        Get

            Dim cTotal As Decimal

            ' Add up all receipt amounts
            For Each oRecovery As CLMRecovery In m_cRecoverys
                cTotal += oRecovery.ThisReceipt
            Next oRecovery

            ' Return total
            Return cTotal

        End Get
    End Property

    Public ReadOnly Property NetReceiptTotal() As Decimal
        Get

            Dim cTotal As Decimal

            ' Add up all receipt amounts
            For Each oRecovery As CLMRecovery In m_cRecoverys
                cTotal += oRecovery.NetReceipt
            Next oRecovery

            ' Return total
            Return cTotal

        End Get
    End Property

    Public ReadOnly Property TaxTotal() As Decimal
        Get

            Dim cTotal As Decimal

            ' Add up all receipt amounts
            For Each oRecovery As CLMRecovery In m_cRecoverys
                cTotal += oRecovery.TaxAmount
            Next oRecovery

            ' Return total
            Return cTotal

        End Get
    End Property

    ' Build a collection of all tax types and totals
    ' Note: this property should be read and cached
    Public ReadOnly Property TaxTypeCollection() As CLMRecoveryTaxes
        Get


            ' Create collection
            Dim oTaxes As New CLMRecoveryTaxes

            ' parse all tax amounts
            For Each oRecovery As CLMRecovery In m_cRecoverys
                If oRecovery.TaxAmount <> 0 Then
                    oTaxes.Add(oRecovery.TaxTypeCode, oRecovery.TaxAmount)
                End If
            Next oRecovery

            ' Return collection
            Return oTaxes

        End Get
    End Property


    ' ***************************************************************** '
    ' Name: NewEnum (Posh Method :-)
    '
    ' Description: Allow this collection to be enumerated with
    '   For Each...Next
    '
    ' Notes:
    '   The return property from this call must be IUnknown!!
    '   The _NewEnum property of the collection is hidden
    '   For this to function the Procedure ID must be set to -4
    ' ***************************************************************** '

    Public Function GetEnumerator() As IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        ' Pass through to collection class
        Return m_cRecoverys.GetEnumerator
    End Function



    ' ***************************************************************** '
    ' Name: Add
    '
    ' Description: Adds a single CLMRecovery into the CLMRecoverys Collection
    ' ***************************************************************** '
    Public Function Add(ByVal oNewCLMRecovery As bCLMRecovery.CLMRecovery) As Integer
        Dim Err_NextID As Boolean = False
        Dim Err_Add As Boolean = False

        Dim result As Integer = 0
        Static iNewID As Integer
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            Err_Add = True
            Err_NextID = False

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the object as we add it
            lReturn = CType(oNewCLMRecovery.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_oDatabase), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oNewCLMRecovery.Initialise", "Failed to initialise recovery object")
            End If

            ' Add the supplied CLMRecovery into the CLMRecoverys Collection
            ' Use a generated key where we do not know the CLMRecovery ID
            If oNewCLMRecovery.UniqueId.Length = 0 Then
                ' Generate a new id
                iNewID += 1
                oNewCLMRecovery.UniqueId = "r" & iNewID
                oNewCLMRecovery.IsNew = True

                ' Set an error handler so we can skip duplicates
                Err_NextID = True
                Err_Add = False
            End If

            ' Try to add the item, if this is a new item we will increment the
            ' generated key, if it's an existing item the error will be raised.
            m_cRecoverys.Add(oNewCLMRecovery, oNewCLMRecovery.UniqueId)

            ' We may need to restore the standard error handler
            Err_Add = True
            Err_NextID = False

            Return result

        Catch excep As System.Exception
            If Not Err_NextID And Not Err_Add Then
                Throw excep
            End If

            If Err_NextID Then

                ' Increment ID and try again
                iNewID += 1
                oNewCLMRecovery.UniqueId = "r" & iNewID



            End If
            If Err_Add Or Err_NextID Then


                ' Error.
                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add CLMRecovery to Collection", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result

            End If
        End Try
    End Function


    ' Returns the number of CLMRecoverys in the collection.
    Public Function Count() As Integer

        Dim result As Integer = 0
        Try


            Return m_cRecoverys.Count

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Count Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Count", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Remove
    '
    ' Description: Delete a CLMRecovery from the Collection.
    ' ***************************************************************** '
    'developer guide no.101
    Public Sub Remove(ByVal vKey As Object)

        Try

            m_cRecoverys.Remove(vKey)

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Deleting from collection, Key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Item
    '
    ' Description: Returns the selected CLMRecovery from the Collection.
    ' ***************************************************************** '
    'developer guide no.101
    Public Function Item(ByRef vKey As Object) As bCLMRecovery.CLMRecovery

        Dim result As bCLMRecovery.CLMRecovery = Nothing
        Try


            Return m_cRecoverys(vKey)

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to return key=" & vKey, vApp:=ACApp, vClass:=ACClass, vMethod:="Item", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Clear
    '
    ' Description: Clear the CLMRecovery Collection.
    ' ***************************************************************** '
    Public Sub Clear()

        Try

            ' Set CLMRecovery Collection to Nothing
            m_cRecoverys = Nothing

            ' Added by Scalability Update Program - 31/07/2002
            m_cRecoverys = New Collection()

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Clear Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Clear", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this object.
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Integer

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
            m_oDatabase = vDatabase

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
    ' Description: Entry point for any termination code for this object.
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
            m_cRecoverys = New Collection()

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the CLMRecovery class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
