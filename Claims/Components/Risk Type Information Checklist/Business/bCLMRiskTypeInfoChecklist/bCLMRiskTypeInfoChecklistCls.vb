Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developr Guide no. 129
Imports SharedFiles

Friend NotInheritable Class CLMRTInfoChklst
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name:   CLMRTInfoChklst
    ' Description:  Describes the CLMRTInfoChklst attributes.
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    '##ModelId=39629EDE0263
    Private Const ACClass As String = "CLMRTInfoChklst"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

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

    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dCLMRiskTypeInfoChecklist As dCLMRTInfoChkLst.CLMRTInfoChkLst

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lRisk_type_Exp_ser_id As Integer
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

    Public Property Risk_type_Exp_ser_id() As Integer
        Get

            Return m_lRisk_type_Exp_ser_id

        End Get
        Set(ByVal Value As Integer)

            m_lRisk_type_Exp_ser_id = Value

        End Set
    End Property
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name:         Initialise (Standard Method)
    ' Description:  Entry point for any initialisation code for this
    '               object.
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


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

            ' Create instance of data class
            m_dCLMRiskTypeInfoChecklist = New dCLMRTInfoChkLst.CLMRTInfoChkLst()

            m_lReturn = m_dCLMRiskTypeInfoChecklist.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=vDatabase)

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
    '##ModelId=39629EDE02AA
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_dCLMRiskTypeInfoChecklist IsNot Nothing Then
                    m_dCLMRiskTypeInfoChecklist.Dispose()
                    m_dCLMRiskTypeInfoChecklist = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name:         SetProperties (Public)
    ' Description:  Sets the supplied CLMRTInfoChklst property values
    '               to the SINGLE
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    'developer guide no.101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vRisk_type_Exp_ser_id As Object = Nothing, Optional ByRef vExp_ser_id As Object = Nothing, Optional ByRef vRisk_type_id As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                m_lReturn = DefaultParameters(bDefaultAll:=False, vRisk_type_Exp_ser_id:=vRisk_type_Exp_ser_id, vExp_ser_id:=vExp_ser_id, vRisk_type_id:=vRisk_type_id)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            'm_lReturn& = Validate(vExp_ser_id:=vRecoveryCnt, vRisk_type_id:=vReserveID, vPerilID:=vPerilID, vRecoveryTypeID:=vRecoveryTypeID, vCurrencyID:=vCurrencyID, vIntialReserve:=vIntialReserve)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values to the DATA Object too
            With m_dCLMRiskTypeInfoChecklist


                If Not Information.IsNothing(vRisk_type_Exp_ser_id) Then
                    If .Risk_type_Exp_ser_id <> vRisk_type_Exp_ser_id Then
                        .Risk_type_Exp_ser_id = vRisk_type_Exp_ser_id
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vExp_ser_id) Then
                    If .Exp_ser_id <> vExp_ser_id Then
                        .Exp_ser_id = vExp_ser_id
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vRisk_type_id) Then
                    If .Risk_type_id <> vRisk_type_id Then
                        .Risk_type_id = vRisk_type_id
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
    ' Name:         AddItem (Public)
    ' Description:  Adds to the Database from the Base Details.
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function AddItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dCLMRiskTypeInfoChecklist

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Retain the Primary Key of the CLMRTInfoChklst Added
                'Assign the PK value from DO SINGLE to BO SINGLE
                Risk_type_Exp_ser_id = .Risk_type_Exp_ser_id

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
    ' Name:         DeleteItem (Public)
    ' Description:  Deletes a single record from the database.
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    Public Function DeleteItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dCLMRiskTypeInfoChecklist

                ' Set Data object primary key
                .Risk_type_Exp_ser_id = Risk_type_Exp_ser_id

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

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name:         DefaultParameters (Private)
    ' Description:  Sets the Default Values for a CLMRTInfoChklst.
    ' Author:       SK
    ' Date:         06/07/2000
    ' ***************************************************************** '
    'developer guide no.101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vRisk_type_Exp_ser_id As Object = Nothing, Optional ByRef vExp_ser_id As Object = Nothing, Optional ByRef vRisk_type_id As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue



        If (Information.IsNothing(vRisk_type_Exp_ser_id)) OrElse (vRisk_type_Exp_ser_id.Equals(0)) OrElse (bDefaultAll) Then
            vRisk_type_Exp_ser_id = 0
        End If



        If (Information.IsNothing(vExp_ser_id)) OrElse (vExp_ser_id.Equals(0)) OrElse (bDefaultAll) Then
            vExp_ser_id = 0
        End If



        If (Information.IsNothing(vRisk_type_id)) OrElse (vRisk_type_id.Equals(0)) OrElse (bDefaultAll) Then
            vRisk_type_id = 0
        End If

        Return result

    End Function


    '##ModelId=39629EDE0304
    Public Sub New()
        MyBase.New()

        ' Class Initialise


        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

