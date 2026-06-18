Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System

'Developer Guide No.: 129
Imports SharedFiles

Friend NotInheritable Class CLMResvDefn
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CLMResvDefn
    '
    ' Date: 06/10/1998
    '
    ' Description: Describes the CLMResvDefn attributes.
    '
    ' Edit History:
    ' RKS 01/12/2005 PN25979 Adding IsExcess field
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "CLMResvDefn"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' ************************************************
    ' Added to replace global variables 18/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dCLMResvDefn As dCLMResvDefn.CLMResvDefn

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lReserveTypeID As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

        End Set
    End Property

    Public Property ReserveTypeID() As Integer
        Get

            Return m_lReserveTypeID

        End Get
        Set(ByVal Value As Integer)

            m_lReserveTypeID = Value

        End Set
    End Property
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

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

            ' Create instance of data class
            m_dCLMResvDefn = New dCLMResvDefn.CLMResvDefn()

            m_lReturn = m_dCLMResvDefn.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=vDatabase)

            Return result

        Catch excep As System.Exception




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
            Me.disposedValue = True
            If disposing Then
                If m_dCLMResvDefn IsNot Nothing Then
                    m_dCLMResvDefn.Dispose()

                End If
                m_dCLMResvDefn = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the CLMResvDefn.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vReserveTypeID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIncludeInTotal As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vIntialReserve As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyClaimID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing, Optional ByRef IsExcess As Object = Nothing) As Integer

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied CLMResvDefn property values.
    '
    ' ***************************************************************** '
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vReserveTypeID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIncludeInTotal As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vIsExcess As Object = Nothing, Optional ByRef vIs_Indemnity As Object = Nothing, Optional ByRef vIs_Expense As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Or iStatus = gPMConstants.PMEComponentAction.PMEdit Then

                ' Default Any Missing Parameters
                'Developer Guide No.: 98
                m_lReturn = DefaultParameters(bDefaultAll:=True, vReserveTypeID:=vReserveTypeID, vDescription:=vDescription, vIncludeInTotal:=vIncludeInTotal, vName:=vName, vIsExcess:=vIsExcess, vIs_Indemnity:=vIs_Indemnity, vIs_Expense:=vIs_Expense)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dCLMResvDefn


                If Not Information.IsNothing(vReserveTypeID) Then
                    If .ReserveTypeID <> vReserveTypeID Then
                        .ReserveTypeID = vReserveTypeID
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vDescription) Then
                    If .Description <> vDescription Then
                        .Description = vDescription
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vIncludeInTotal) Then
                    If .IncludeInTotal <> vIncludeInTotal Then
                        .IncludeInTotal = vIncludeInTotal
                        bDataChanged = True
                    End If
                End If


                If (Not Information.IsNothing(vName)) And (Not String.IsNullOrEmpty(vName)) Then
                    .Name = vName
                    bDataChanged = True
                End If
                If Not Information.IsNothing(vIsExcess) Then
                    If .IsExcess <> vIsExcess Then
                        .IsExcess = vIsExcess
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vIs_Indemnity) Then
                    If .Is_Indemnity <> vIs_Indemnity Then
                        .Is_Indemnity = vIs_Indemnity
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vIs_Expense) Then
                    If .Is_Expense <> vIs_Expense Then
                        .Is_Expense = vIs_Expense
                        bDataChanged = True
                    End If
                End If

                ' If we have changed one of the properties, update the status
                If bDataChanged Then
                    m_iDatabaseStatus = iStatus
                End If

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied CLMResvDefn property values.
    '
    ' ***************************************************************** '
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vReserveTypeID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIncludeInTotal As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyClaimID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing, Optional ByRef vIsExcess As Object = Nothing) As Integer

     
       
    End Function


    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function AddItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dCLMResvDefn

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the CLMResvDefn Added
                ReserveTypeID = .ReserveTypeID

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Public Function DeleteItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dCLMResvDefn

                ' Set Data object primary key
                .ReserveTypeID = ReserveTypeID

                ' Update the record on the database from the object
                m_lReturn = .Delete()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function UpdateItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dCLMResvDefn

                ' Set Data object primary key
                .ReserveTypeID = ReserveTypeID


                ' Update the record on the database from the object
                m_lReturn = .Update()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a CLMResvDefn.
    '
    ' ***************************************************************** '
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vReserveTypeID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vIncludeInTotal As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vIsExcess As Object = Nothing, Optional ByRef vIs_Indemnity As Object = Nothing, Optional ByRef vIs_Expense As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If (Information.IsNothing(vReserveTypeID)) OrElse (vReserveTypeID.Equals(0)) Then 'Or (bDefaultAll = True)) Then
                vReserveTypeID = 0
            End If

            If (Information.IsNothing(vIncludeInTotal)) OrElse (vIncludeInTotal.Equals(0)) Then 'Or (bDefaultAll = True)) Then
                vIncludeInTotal = 0
            End If

            If (Information.IsNothing(vDescription)) OrElse (vDescription.Equals(0)) Then 'Or (bDefaultAll = True)) Then
                vDescription = 0
            End If

            If (Information.IsNothing(vName)) OrElse (vName.Equals(0)) Then 'Or (bDefaultAll = True)) Then
                vName = 0
            End If

            If (Information.IsNothing(vIsExcess)) OrElse (vIsExcess.Equals(0)) Then 'Or (bDefaultAll = True)) Then
                vIsExcess = 0
            End If

            If (Information.IsNothing(vIs_Indemnity)) OrElse (vIs_Indemnity.Equals(0)) Then 'Or (bDefaultAll = True)) Then
                vIs_Indemnity = 0
            End If
            Return result

            If (Information.IsNothing(vIs_Expense)) OrElse (vIs_Expense.Equals(0)) Then 'Or (bDefaultAll = True)) Then
                vIs_Expense = 0
            End If

        Catch
        End Try

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DefaultParameters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DefaultParameters", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function



    Public Sub New()
        MyBase.New()

        ' Class Initialise

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

