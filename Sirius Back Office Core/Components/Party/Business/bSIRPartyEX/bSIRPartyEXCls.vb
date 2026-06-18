Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared
Friend NotInheritable Class SIRPartyEX
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyEX
    '
    ' Date: 12/10/1998
    '
    ' Description: Describes the SIRPartyEX attributes.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "SIRPartyEX"

    ' ************************************************
    ' Added to replace global variables 09/02/2004
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Update Status
    Private m_iDatabaseStatus As gPMConstants.PMEComponentAction
    ' Instance of Data component
    Private m_dSIRPartyEX As dSIRPartyEX.SIRPartyEX ' was dSIRPartyEX.SIRPartyEX
    ' Instance of the Core SIRClaim object
    'Private m_bSIRParty As bSIRParty.Business
    Private m_bSIRParty As bSIRParty.Business
    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' Primary Keys to work with
    Private m_lPartyCnt As Integer
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database


    Public Property DatabaseStatus() As Integer
        Get
            Return m_iDatabaseStatus
        End Get
        Set(ByVal Value As Integer)
            m_iDatabaseStatus = Value
        End Set
    End Property


    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public ReadOnly Property bSIRParty() As Object
        Get

            Return m_bSIRParty

        End Get
    End Property
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vAgencyNumber As String = "", Optional ByRef vIsFeeCharge As Integer = 0, Optional ByRef vRiskTransferAgreement As Boolean = False, Optional ByRef vDelegatedAuthority As Boolean = False, Optional ByRef vFSAProductID As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetProperties"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            'developer guide no. 118
            vPartyCnt = m_dSIRPartyEX.PartyCnt

            vAgencyNumber = m_dSIRPartyEX.AgencyNumber

            vIsFeeCharge = m_dSIRPartyEX.IsFeeCharge

            vRiskTransferAgreement = m_dSIRPartyEX.RiskTransferAgreement

            vDelegatedAuthority = m_dSIRPartyEX.DelegatedAuthority

            vFSAProductID = m_dSIRPartyEX.FSAProductID

            iStatus = m_iDatabaseStatus

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
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
            m_dSIRPartyEX = New dSIRPartyEX.SIRPartyEX()
            '    Set m_dSIRPartyEX = New dSIRPartyEX.SIRPartyEX

            m_lReturn = m_dSIRPartyEX.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)



            m_bSIRParty = New bSIRParty.Business
            m_lReturn = m_bSIRParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'developer guide no. 101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgencyNumber As Object = Nothing, Optional ByRef vIsFeeCharge As Object = Nothing, Optional ByRef vRiskTransferAgreement As Object = Nothing, Optional ByRef vDelegatedAuthority As Object = Nothing, Optional ByRef vFSAProductID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetProperties"

        Dim bDataChanged As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                'Default Any Missing Parameters
                'developer guide no. 98
                m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vAgencyNumber:=vAgencyNumber, vIsFeeCharge:=vIsFeeCharge, vRiskTransferAgreement:=vRiskTransferAgreement, vDelegatedAuthority:=vDelegatedAuthority, vFSAProductID:=vFSAProductID), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("DefaultParameters", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            'Validate Parameters
            m_lReturn = CType(Validate(vPartyCnt:=vPartyCnt, vAgencyNumber:=vAgencyNumber, vIsFeeCharge:=vIsFeeCharge, vRiskTransferAgreement:=vRiskTransferAgreement, vDelegatedAuthority:=vDelegatedAuthority, vFSAProductID:=vFSAProductID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Validate", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.


            If Not Informations.IsNothing(vPartyCnt) And Not vPartyCnt.Equals(0) Then
                m_dSIRPartyEX.PartyCnt = vPartyCnt
            End If



            If Not Informations.IsNothing(vAgencyNumber) And Not String.IsNullOrEmpty(vAgencyNumber) Then
                m_dSIRPartyEX.AgencyNumber = vAgencyNumber
            End If



            If Not Informations.IsNothing(vIsFeeCharge) And Not vIsFeeCharge.Equals(0) Then
                m_dSIRPartyEX.IsFeeCharge = vIsFeeCharge
            End If



            If Not Informations.IsNothing(vRiskTransferAgreement) And Not vRiskTransferAgreement.Equals(False) Then
                m_dSIRPartyEX.RiskTransferAgreement = vRiskTransferAgreement
            End If



            If Not Informations.IsNothing(vDelegatedAuthority) And Not vDelegatedAuthority.Equals(False) Then
                m_dSIRPartyEX.DelegatedAuthority = vDelegatedAuthority
            End If



            If Not Informations.IsNothing(vFSAProductID) And Not vFSAProductID.Equals(0) Then
                m_dSIRPartyEX.FSAProductID = vFSAProductID
            End If

            ' If we have changed one of the properties, update the status
            m_iDatabaseStatus = iStatus


            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
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
                If m_dSIRPartyEX IsNot Nothing Then
                    m_dSIRPartyEX.Dispose()

                End If
                m_dSIRPartyEX = Nothing
                If m_bSIRParty IsNot Nothing Then
                    m_bSIRParty.Dispose()
                End If
                m_bSIRParty = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRPartyEX.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgencyNumber As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults


            'developer guide no. 98
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vPartyCnt:=vPartyCnt, vAgencyNumber:=vAgencyNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectItem (Public)
    '
    ' Description: Reads the Base Details from the Database .
    '
    ' ***************************************************************** '
    Public Function SelectItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRPartyEX

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Select a record from the database
                m_lReturn = .SelectSingle()

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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

            With m_dSIRPartyEX

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRPartyEX Added
                PartyCnt = .PartyCnt

            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            With m_dSIRPartyEX

                ' Set Data object primary key
                .PartyCnt = PartyCnt

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            With m_dSIRPartyEX

                ' Set Data object primary key
                .PartyCnt = PartyCnt

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a SIRPartyEX.
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgencyNumber As Object = Nothing, Optional ByRef vIsFeeCharge As Object = Nothing, Optional ByRef vRiskTransferAgreement As Object = Nothing, Optional ByRef vDelegatedAuthority As Object = Nothing, Optional ByRef vFSAProductID As Object = Nothing) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue



        If Informations.IsNothing(vPartyCnt) Or vPartyCnt.Equals(0) Or bDefaultAll Then
            vPartyCnt = 0
        End If



        If Informations.IsNothing(vAgencyNumber) Or vAgencyNumber.Equals(0) Or bDefaultAll Then
            vAgencyNumber = 0
        End If



        If Informations.IsNothing(vRiskTransferAgreement) Or vRiskTransferAgreement.Equals(False) Or bDefaultAll Then
            vRiskTransferAgreement = False
        End If



        If Informations.IsNothing(vDelegatedAuthority) Or vDelegatedAuthority.Equals(False) Or bDefaultAll Then
            vDelegatedAuthority = False
        End If

        ' Do any tidy up, e.g. Set x = Nothing here
        Return result
        ' This is for debugging only
    End Function


    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgencyNumber As Object = Nothing, Optional ByRef vIsFeeCharge As Object = Nothing, Optional ByRef vRiskTransferAgreement As Object = Nothing, Optional ByRef vDelegatedAuthority As Object = Nothing, Optional ByRef vFSAProductID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        If Not Informations.IsNothing(vPartyCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                gPMFunctions.RaiseError("Validate", "Invalid value. vPartyCnt:=" & CStr(vPartyCnt), gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        ' Do any tidy up, e.g. Set x = Nothing here
        Return result
        ' This is for debugging only
    End Function


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
