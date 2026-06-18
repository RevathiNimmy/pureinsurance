Option Strict Off
Option Explicit On
'Developer Guide No 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 13/05/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, business rules required for the
    '              MatchPost Form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 05/02/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    Private m_bAbortTrans As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As New StringsHelper.FixedLengthString(2)
    Private m_sMapStatus As New StringsHelper.FixedLengthString(2)
    Private m_sStepStatus As New StringsHelper.FixedLengthString(2)

    Private m_cTotalBase As Decimal
    Private m_cTotalCurrency As Decimal

    Private m_oMatchGroup As bACTMatchgroup.Form
    Private m_oTransMatch As bACTTransmatch.Form
    Private m_lMatchGroupId As Integer

    Private m_lPeriodId As Integer



    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property TotalBase() As Decimal
        Get
            Return m_cTotalBase
        End Get
        Set(ByVal Value As Decimal)
            m_cTotalBase = Value
        End Set
    End Property


    Public Property TotalCurrency() As Decimal
        Get
            Return m_cTotalCurrency
        End Get
        Set(ByVal Value As Decimal)
            m_cTotalCurrency = Value
        End Set
    End Property



    Public Property AbortTrans() As Boolean
        Get
            Return m_bAbortTrans
        End Get
        Set(ByVal Value As Boolean)
            m_bAbortTrans = Value
        End Set
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property


    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property
    'eck110102
    Public Property MatchGroupId() As Integer
        Get
            Return m_lMatchGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lMatchGroupId = Value
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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
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

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level


            'Set m_oDatabase = GetOrionDatabase(m_lReturn, m_bCloseDatabase, vDatabase)


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oMatchGroup = GetOrionBusiness(v_sClassName:="bACTMatchGroup.Form", v_vDatabase:=m_oDatabase)
            'developer guide no. 218

            m_oMatchGroup = New bACTMatchgroup.Form
            m_lReturn = m_oMatchGroup.Initialise(sUsername:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oTransMatch = GetOrionBusiness(v_sClassName:="bACTTransMatch.Form", v_vDatabase:=m_oDatabase)

            '    If m_oTransMatch Is Nothing Then
            '      Initialise = PMFalse
            '      Exit Function
            '    End If

            'developer guide no. 218

            m_oTransMatch = New bACTTransmatch.Form
            m_lReturn = m_oTransMatch.Initialise(sUsername:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Always assume that we will rollback

            m_bAbortTrans = True

            ' Start a transaction for the whole object
            'eck try this
            '    m_oDatabase.SQLBeginTrans

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            m_cTotalBase = 0
            m_cTotalCurrency = 0

            Return result

        Catch excep As System.Exception



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

                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
                m_oTransMatch = Nothing
                m_oMatchGroup = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck100500
    Public Function AddMatchGroup(ByVal v_dtMatchDate As Date, ByRef r_vMatchId As Integer) As Integer
        Return AddMatchGroup(v_dtMatchDate:=v_dtMatchDate, v_lSubBranchID:=0, r_vMatchId:=r_vMatchId, r_vMatchSourceId:=Nothing)
    End Function

    Public Function AddMatchGroup(ByVal v_dtMatchDate As Date, ByVal v_lSubBranchID As Integer, ByRef r_vMatchId As Integer) As Integer
        Return AddMatchGroup(v_dtMatchDate:=v_dtMatchDate, v_lSubBranchID:=v_lSubBranchID, r_vMatchId:=r_vMatchId, r_vMatchSourceId:=Nothing)
    End Function

    Public Function AddMatchGroup(ByVal v_dtMatchDate As Date, ByVal v_lSubBranchID As Integer, ByRef r_vMatchId As Integer, ByRef r_vMatchSourceId As Object) As Integer

        ' Variables for defaulting
        Dim result As Integer = 0
        Dim iCompanyID As Integer
        Dim lMatchID, lPeriodID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Defaults

            If Not Informations.IsNothing(r_vMatchSourceId) Then

                iCompanyID = CInt(r_vMatchSourceId)
            Else
                iCompanyID = m_iSourceID
            End If
            '
            ' Set the period id from the date

            m_lReturn = CType(GetPeriodIdForDate(r_lPeriodId:=lPeriodID, v_dtAccountingDate:=v_dtMatchDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do a Direct add so we can get the ID

            m_lReturn = m_oMatchGroup.DirectAdd(vMatchID:=lMatchID, vPeriodID:=lPeriodID, vCompanyID:=iCompanyID, vMatchDate:=v_dtMatchDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Read back the added record into the collection

            m_lReturn = m_oMatchGroup.GetDetails(lMatchID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lMatchGroupId = lMatchID

            m_cTotalBase = 0
            m_cTotalCurrency = 0


            If Not Informations.IsNothing(r_vMatchId) Then
                r_vMatchId = lMatchID
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddMatchGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddMatchGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function AddMatchTrans(ByVal v_lAllocationdetailID As Object, ByVal v_lTransDetailID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cBaseMatchAmount As Decimal, ByVal v_cCurrencyMatchAmount As Decimal, ByVal v_vdCurrencyMatchXRate As Object) As Integer
        Return AddMatchTrans(v_lAllocationdetailID:=v_lAllocationdetailID, v_lTransDetailID:=v_lTransDetailID, v_iCurrencyID:=v_iCurrencyID, v_cBaseMatchAmount:=v_cBaseMatchAmount, v_cCurrencyMatchAmount:=v_cCurrencyMatchAmount, v_vdCurrencyMatchXRate:=v_vdCurrencyMatchXRate, r_vTransMatchId:=0)
    End Function

    Public Function AddMatchTrans(ByVal v_lAllocationdetailID As Object, ByVal v_lTransDetailID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cBaseMatchAmount As Decimal, ByVal v_cCurrencyMatchAmount As Decimal, ByVal v_vdCurrencyMatchXRate As Object, ByRef r_vTransMatchId As Integer) As Integer

        ' Values to be defaulted
        Dim result As Integer = 0
        Dim lTransMatchId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oTransMatch.DirectAdd(vTransmatchID:=lTransMatchId, vAllocationdetailID:=v_lAllocationdetailID, vTransdetailID:=v_lTransDetailID, vMatchID:=m_lMatchGroupId, vCurrencyID:=v_iCurrencyID, vBaseMatchAmount:=v_cBaseMatchAmount, vCurrencyMatchAmount:=v_cCurrencyMatchAmount, vCurrencyMatchXrate:=v_vdCurrencyMatchXRate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            m_cTotalBase += v_cBaseMatchAmount
            m_cTotalCurrency += v_cCurrencyMatchAmount


            If Not Informations.IsNothing(r_vTransMatchId) Then
                r_vTransMatchId = lTransMatchId
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddMatchTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddMatchTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' --------------------------------------------------------------
    ' Validate the Match and its transactions
    ' If its ok then set the Abort flag to false so that the
    ' Database transactions are not rolled back
    ' --------------------------------------------------------------
    Public Function Commit() As Integer


        Dim result As Integer = 0
        Try

            ' Assume the worst

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check that the Match adds up
            ' CF200199 - Commented out
            'If m_cTotalBase <> 0 Then
            '  Exit Function
            'End If

            ' Says that it passed validation
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Will commit trans in terminate
            m_bAbortTrans = False

            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Commit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Commit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' PUBLIC Methods (End)

    Private Function GetPeriodIdForDate(ByRef r_lPeriodId As Integer, ByVal v_dtAccountingDate As Date) As Integer

        Dim result As Integer = 0
        Dim lPeriodID As Integer
        Dim oPeriod As bACTPeriod.Form

        'EK 07/12/99



        result = gPMConstants.PMEReturnCode.PMTrue

        'Set oPeriod = GetOrionBusiness(v_sClassName:="bACTPeriod.Form", v_vDatabase:=m_oDatabase)


        oPeriod = New bACTPeriod.Form
        m_lReturn = oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = oPeriod.GetPeriodForDate(dtDateInPeriod:=v_dtAccountingDate, lPeriodID:=lPeriodID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        r_lPeriodId = lPeriodID
        oPeriod = Nothing

        Return result

    End Function
    Public Sub New()
        MyBase.New()

        Try

            ' Class Initialise

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

