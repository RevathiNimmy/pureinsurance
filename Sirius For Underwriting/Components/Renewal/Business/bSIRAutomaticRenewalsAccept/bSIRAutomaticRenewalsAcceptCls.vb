Option Strict Off
Option Explicit On
Imports SSP.Shared
Imports SharedQuoteEngine

<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 02/09/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRRenSelection.
    '
    ' Edit History:
    ' ***************************************************************** '


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


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)


    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Calling Application Name

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_dtSelectionDate As Date

    Private m_lRenewalPreDebitDays As Integer
    'Private m_bRenewalAcceptancePrinting As Boolean    take this out and introduce three system option instead

    Private m_sRenSchedulePrinting As String = "" 'system option 1036
    Private m_sRenCertificatePrinting As String = "" 'system option 1037
    Private m_sRenDebitNotePrinting As String = "" 'system option 1038

    Private m_oBusiness As bSIRRenewal.Business
    'developer guide no. 108
    Private m_oDocManagerWrapper As bSIRDocManagerWrapper.Interface_Renamed
    Private m_oControlTrans As bControlTrans.Automated
    Private m_oAccumulationValues As bSIRAccumulationValues.Business

#If IN_DEBUG > 0 Then

	Private m_oDebugTimings As Object
#End If
    Private lPMAuthorityLevel As Integer

    'sj 21/01/2003 - start
    'PS104
    ' InsuranceFileCnt
    Private m_lInsuranceFileCnt As Integer
    Private m_bCalledFromMTA As Boolean
    'sj 21/01/2003 - end

    'sj 21/01/2003 - start
    'PS104
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    'sj 21/01/2003 - end


    Public WriteOnly Property dtSelectionDate() As Date
        Set(ByVal Value As Date)
            m_dtSelectionDate = Value
        End Set
    End Property

    ' Primary Keys to work with
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

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


            Dim iOptionNumber As Integer
            Dim sValue As String = ""


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = "RENEWAL"
            m_dtEffectiveDate = DateTime.Now

            'Renewal business
            m_oBusiness = New bSIRRenewal.Business()
            m_lReturn = m_oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oBusiness.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Control Trans (Business)
            m_oControlTrans = New bControlTrans.Automated()
            m_lReturn = m_oControlTrans.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bControlTrans.Automated.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Accumulation Values (Business)
            m_oAccumulationValues = New bSIRAccumulationValues.Business()
            m_lReturn = m_oAccumulationValues.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bSirAccumulationValues.Business.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Doc Manager Wrapper
            'developer guide no. 108
            m_oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed()
            m_lReturn = CType(m_oDocManagerWrapper.InitialiseBusiness(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bSIRDocManagerWrapper.Interface.InitialiseBusuiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get Renewal Pre Debit Days
            iOptionNumber = 1010
            m_lReturn = CType(GetRenewalGroupSystemOption(v_iOptionNumber:=iOptionNumber, r_sValue:=sValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetRenewalGroupSystemOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If
            m_lRenewalPreDebitDays = CInt(Val(sValue))

            If bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=1036, r_sOptionValue:=m_sRenSchedulePrinting) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to get system option number 1036", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

            If bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=1037, r_sOptionValue:=m_sRenCertificatePrinting) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to get system option number 1037", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

            If bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=1038, r_sOptionValue:=m_sRenCertificatePrinting) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to get system option number 1038", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

            ' remove this and add three system option instead (1036, 1037, 1038)
            '    'Get renewal acceptance printing
            '    iOptionNumber = 1011
            '    m_lReturn = GetRenewalGroupSystemOption( _
            ''        v_iOptionNumber:=iOptionNumber, _
            ''        r_sValue:=sValue _
            ''        )
            '    If m_lReturn <> PMTrue Then
            '        Initialise = PMFalse
            '        LogMessage m_sUsername, _
            ''            iType:=PMError, _
            ''            sMsg:="GetRenewalGroupSystemOption Failed", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Initialise"
            '        Exit Function
            '    End If
            '    If Val(sValue) = 1 Then
            '        m_bRenewalAcceptancePrinting = True
            '    Else
            '        m_bRenewalAcceptancePrinting = False
            '    End If


#If IN_DEBUG > 0 Then

			'Debug Timings
			Set m_oDebugTimings = CreateLateBoundObject("bSIRDebugTimings.Interface")
			m_oDebugTimings.CallingAppName = ACApp
#End If

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
            Me.disposedValue = True
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If m_oControlTrans IsNot Nothing Then
                    m_oControlTrans.Dispose()
                    m_oControlTrans = Nothing
                End If
                If m_oDocManagerWrapper IsNot Nothing Then
                    m_oDocManagerWrapper.Dispose()
                    m_oDocManagerWrapper = Nothing
                End If
                If m_oAccumulationValues IsNot Nothing Then
                    m_oAccumulationValues.Dispose()
                    m_oAccumulationValues = Nothing
                End If
#If IN_DEBUG > 0 Then

			Set m_oDebugTimings = Nothing
#End If

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetRenewalGroupSystemOption(ByRef v_iOptionNumber As Integer, ByRef r_sValue As String) As Integer

        Dim result As Integer = 0
        Dim oOptions As bSIROptions.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oOptions = New bSIROptions.Business
            m_lReturn = oOptions.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the value for this option

            m_lReturn = oOptions.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

                oOptions = Nothing

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise oRenEdiAudit", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalGroupSystemOption")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            oOptions.Dispose()
            oOptions = Nothing


            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                r_sValue = "0"
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRenewalGroupSystemOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalGroupSystemOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 09/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing
            Dim dtSelectionDate As Date
            Dim lOldPolicyCnt, lRenewalStatusCnt, lNewPolicyCnt, lInsuranceFolder, lPartyCnt As Integer
            Dim sInsuranceRef As String = ""
            Dim lProductID As Integer

            ' Get a list of the renewals which are ON the supplied date
            dtSelectionDate = m_dtSelectionDate.AddDays(m_lRenewalPreDebitDays)


#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "Start"
			m_oDebugTimings.StartTiming "ClearAcceptFailures"
#End If

            m_lReturn = CType(ClearAcceptFailures(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="ClearAcceptFailures Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                Return result
            End If
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "ClearAcceptFailures"
			m_oDebugTimings.StartTiming "GetRenewals"
#End If

            'sj 13/12/2002 - Change v_iCompare to 0 so get all due renewals not those due just today
            'm_lReturn = m_oBusiness.GetRenewals(r_vResultArray:=vResultArray, v_bIsAmmend:=False, v_lRenewalInsFileCnt:=0, v_sRenewalDate:=gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, DateTimeHelper.ToString(dtSelectionDate)), v_lProductId:=0, v_lSourceID:=0, v_iCompare:=0)

            'TODO: Some of the arguments names mismatch. need to find out the exact correct names, will cover when renewals functionality will be converted.
            m_lReturn = m_oBusiness.GetRenewals(r_vResultArray:=vResultArray, v_lRunMode:=False, v_lRenewalInsFileCnt:=0, v_sRenewalDate:=gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, ToSafeString(dtSelectionDate)), v_lProductId:=0, v_lSourceID:=0, v_iCompare:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetRenewals Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                Return result
            End If
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "GetRenewals"
#End If

            If Not Informations.IsArray(vResultArray) Then
                'no records to process so just exit here
                Return result
            End If

            'loop thro and process each renewal invite

            For lCount As Integer = 0 To vResultArray.GetUpperBound(1)


                lOldPolicyCnt = CInt(vResultArray(ACIRenewalLivePolicyCnt, lCount))

                lRenewalStatusCnt = CInt(vResultArray(ACIRenewalStatusId, lCount))

                lNewPolicyCnt = CInt(vResultArray(ACIRenewalPolicyCnt, lCount))

                lInsuranceFolder = CInt(vResultArray(ACIRenewalInsuranceFolder, lCount))

                lPartyCnt = CInt(vResultArray(ACIRenewalInsuranceHolder, lCount))

                sInsuranceRef = CStr(vResultArray(ACIRenewalPolicy, lCount))

                lProductID = ToSafeInteger(vResultArray(ACIRenewalProductId, lCount), 0)

                'Start database transaction
                m_lReturn = CType(BeginTrans(v_vInsuranceFileCnt:=lNewPolicyCnt), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Accept the renewal
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "AcceptRenewals"
#End If
                m_lReturn = CType(AcceptRenewal(v_lOldPolicyCnt:=lOldPolicyCnt, v_lNewPolicyCnt:=lNewPolicyCnt, v_lRenewalStatusCnt:=lRenewalStatusCnt, v_lInsuranceFolder:=lInsuranceFolder, v_lPartyCnt:=lPartyCnt, v_sInsurerRef:=sInsuranceRef, v_lProductID:=lProductID), gPMConstants.PMEReturnCode)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "AcceptRenewals"
#End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AcceptRenewal failed for InsuranceFileCnt " & lNewPolicyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                    'Rollback database transaction
                    m_lReturn = CType(RollbackTrans(v_vInsuranceFileCnt:=lNewPolicyCnt), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    'Commit database transaction
                    m_lReturn = CType(CommitTrans(v_vInsuranceFileCnt:=lNewPolicyCnt), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            Next lCount

#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "Start"
			m_oDebugTimings.Report
#End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: RenewalAcceptForMTA
    '
    ' Description:
    '
    ' History: 22/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function RenewalAcceptForMTA() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing
            Dim lOldPolicyCnt, lRenewalStatusCnt, lNewPolicyCnt, lInsuranceFolder, lPartyCnt As Integer
            Dim sInsuranceRef As String = ""
            Dim lProductID As Integer

            m_bCalledFromMTA = True

            'Get the renewal record
            m_lReturn = m_oBusiness.GetRenewals(r_vResultArray:=vResultArray, v_lRunMode:=gPMConstants.ACRenewalModeAccept, v_lRenewalInsFileCnt:=m_lInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetRenewals Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalAcceptForMTA")
                Return result
            End If


            If Not Informations.IsArray(vResultArray) Then
                'no records to process so just exit here
                Return result
            End If


            lOldPolicyCnt = CInt(vResultArray(ACIRenewalLivePolicyCnt, 0))

            lRenewalStatusCnt = CInt(vResultArray(ACIRenewalStatusId, 0))

            lNewPolicyCnt = CInt(vResultArray(ACIRenewalPolicyCnt, 0))

            lInsuranceFolder = CInt(vResultArray(ACIRenewalInsuranceFolder, 0))

            lPartyCnt = CInt(vResultArray(ACIRenewalInsuranceHolder, 0))

            sInsuranceRef = CStr(vResultArray(ACIRenewalPolicy, 0))

            lProductID = ToSafeInteger(vResultArray(ACIRenewalProductId, 0), 0)

            'Start database transaction
            m_lReturn = CType(BeginTrans(v_vInsuranceFileCnt:=lNewPolicyCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Accept the renewal
            m_lReturn = CType(AcceptRenewal(v_lOldPolicyCnt:=lOldPolicyCnt, v_lNewPolicyCnt:=lNewPolicyCnt, v_lRenewalStatusCnt:=lRenewalStatusCnt, v_lInsuranceFolder:=lInsuranceFolder, v_lPartyCnt:=lPartyCnt, v_sInsurerRef:=sInsuranceRef, v_lProductID:=lProductID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AcceptRenewal failed for InsuranceFileCnt " & lNewPolicyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalAcceptForMTA")
                'Rollback database transaction
                m_lReturn = CType(RollbackTrans(v_vInsuranceFileCnt:=lNewPolicyCnt), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                'Commit database transaction
                m_lReturn = CType(CommitTrans(v_vInsuranceFileCnt:=lNewPolicyCnt), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenewalAcceptForMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalAcceptForMTA", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenewalAcceptForSinglePolicy
    '
    ' Description:
    '
    ' History: 22/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function RenewalAcceptForSinglePolicy() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing
            Dim lOldPolicyCnt, lRenewalStatusCnt, lNewPolicyCnt, lInsuranceFolder, lPartyCnt As Integer
            Dim sInsuranceRef As String = ""
            Dim lProductID As Integer

            m_bCalledFromMTA = False

            'Get the renewal record
            m_lReturn = m_oBusiness.GetRenewals(r_vResultArray:=vResultArray, v_lRunMode:=gPMConstants.ACRenewalModeAccept, v_lRenewalInsFileCnt:=m_lInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetRenewals Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalAcceptForSinglePolicy")
                Return result
            End If


            If Not Informations.IsArray(vResultArray) Then
                'no records to process so just exit here
                Return result
            End If


            lOldPolicyCnt = CInt(vResultArray(ACIRenewalLivePolicyCnt, 0))

            lRenewalStatusCnt = CInt(vResultArray(ACIRenewalStatusId, 0))

            lNewPolicyCnt = CInt(vResultArray(ACIRenewalPolicyCnt, 0))

            lInsuranceFolder = CInt(vResultArray(ACIRenewalInsuranceFolder, 0))

            lPartyCnt = CInt(vResultArray(ACIRenewalInsuranceHolder, 0))

            sInsuranceRef = CStr(vResultArray(ACIRenewalPolicy, 0))

            lProductID = ToSafeInteger(vResultArray(ACIRenewalProductId, 0), 0)
            'Start database transaction
            m_lReturn = CType(BeginTrans(v_vInsuranceFileCnt:=lNewPolicyCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Accept the renewal
            m_lReturn = CType(AcceptRenewal(v_lOldPolicyCnt:=lOldPolicyCnt, v_lNewPolicyCnt:=lNewPolicyCnt, v_lRenewalStatusCnt:=lRenewalStatusCnt, v_lInsuranceFolder:=lInsuranceFolder, v_lPartyCnt:=lPartyCnt, v_sInsurerRef:=sInsuranceRef, v_lProductID:=lProductID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AcceptRenewal failed for InsuranceFileCnt " & lNewPolicyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalAcceptForSinglePolicy")
                'Rollback database transaction
                m_lReturn = CType(RollbackTrans(v_vInsuranceFileCnt:=lNewPolicyCnt), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                'Commit database transaction
                m_lReturn = CType(CommitTrans(v_vInsuranceFileCnt:=lNewPolicyCnt), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenewalAcceptForSinglePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalAcceptForSinglePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AcceptRenewal
    '
    ' Description:
    '
    ' History: 10/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function AcceptRenewal(ByVal v_lOldPolicyCnt As Integer, ByVal v_lNewPolicyCnt As Integer, ByVal v_lRenewalStatusCnt As Integer, ByVal v_lInsuranceFolder As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sInsurerRef As String, ByVal v_lProductID As Integer) As Integer

        Dim result As Integer = 0
        Dim vPrintOptions As Object = Nothing
        Dim bProduceSchedule As Boolean
        Dim bProduceCertificate As Boolean
        Dim bProduceDebitNote As Boolean

        Dim obPMBDocLink As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lIsQuoted As Integer

        'Initialize object
        obPMBDocLink = gPMFunctions.CreateLateBoundObject("bPMBDocLink.Business")
        m_lReturn = obPMBDocLink.Initialise(
            sUsername:=CStr(m_sUsername),
            sPassword:=CStr(m_sPassword),
            iUserID:=CInt(m_iUserID),
            iSourceID:=CInt(m_iSourceID),
            iLanguageID:=CInt(m_iLanguageID),
            iCurrencyID:=CInt(m_iCurrencyID),
            iLogLevel:=CInt(m_iLogLevel),
            sCallingAppName:=CStr(ACApp),
            vDatabase:=DirectCast(m_oDatabase, dPMDAO.Database))
        If m_lReturn <> PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername,
                iType:=gPMConstants.PMELogLevel.PMLogError,
                sMsg:="bControlTrans.Automated.Initialise Failed",
                vApp:=ACApp,
                vClass:=ACClass,
                vMethod:="AcceptRenewal")
            AcceptRenewal = PMEReturnCode.PMFalse
            Exit Function
        End If

#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "IsQuoted"
#End If
        m_lReturn = m_oBusiness.IsQuoted(v_lInsuranceFileCnt:=v_lNewPolicyCnt, r_lResult:=lIsQuoted)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = CType(AddAcceptFailure(v_vInsuranceFileCnt:=v_lNewPolicyCnt, v_vFailureReason:="m_oBusiness.IsQuoted Failed", v_vMethod:="AcceptRenewal"), gPMConstants.PMEReturnCode)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "IsQuoted"
#End If

        If lIsQuoted <> 1 Then
            m_lReturn = CType(AddAcceptFailure(v_vInsuranceFileCnt:=v_lNewPolicyCnt, v_vFailureReason:="Not Quoted", v_vMethod:="AcceptRenewal"), gPMConstants.PMEReturnCode)
            Return result
        End If

#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oBusiness.AcceptRenewal"
#End If
        m_lReturn = m_oBusiness.AcceptRenewal(v_lOldPolicyCnt, v_lNewPolicyCnt, v_lRenewalStatusCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = CType(AddAcceptFailure(v_vInsuranceFileCnt:=v_lNewPolicyCnt, v_vFailureReason:="m_oBusiness.AcceptRenewal Failed", v_vMethod:="AcceptRenewal"), gPMConstants.PMEReturnCode)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oBusiness.AcceptRenewal"
#End If

#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "GetStats"
#End If
        'sj 22/01/2003 - start
        'PS104
        If Not m_bCalledFromMTA Then
            'sj 22/01/2003 - end
            m_lReturn = CType(GetStats(v_lNewPolicyCnt:=v_lNewPolicyCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(AddAcceptFailure(v_vInsuranceFileCnt:=v_lNewPolicyCnt, v_vFailureReason:="GetStats Failed", v_vMethod:="AcceptRenewal"), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "GetStats"
#End If

#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "GetAccumulations"
#End If
        m_lReturn = CType(GetAccumulations(v_lNewPolicyCnt:=v_lNewPolicyCnt), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = CType(AddAcceptFailure(v_vInsuranceFileCnt:=v_lNewPolicyCnt, v_vFailureReason:="GetAccumulations Failed", v_vMethod:="AcceptRenewal"), gPMConstants.PMEReturnCode)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "GetAccumulations"
#End If

#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "CreateEvent"
#End If
        'create renewal event
        m_lReturn = m_oBusiness.CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=v_lPartyCnt, v_vInsuranceFolderCnt:=v_lInsuranceFolder, v_vInsuranceFileCnt:=v_lNewPolicyCnt, v_vEventType:=5, v_vUserId:=m_iUserID, v_vEventDate:=DateTime.Today, v_vDescription:="Accept Renewal - " & v_sInsurerRef)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = CType(AddAcceptFailure(v_vInsuranceFileCnt:=v_lNewPolicyCnt, v_vFailureReason:="m_oBusiness.CreateEvent Failed", v_vMethod:="AcceptRenewal"), gPMConstants.PMEReturnCode)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "CreateEvent"
#End If

        m_lReturn = m_oBusiness.GetProdPrintOptions(v_lProductID, vPrintOptions)


        If (m_lReturn <> PMEReturnCode.PMTrue) Then
            AcceptRenewal = PMEReturnCode.PMFalse
            Exit Function
        End If

        If Informations.IsArray(vPrintOptions) Then
            bProduceSchedule = ToSafeBoolean(vPrintOptions(0, 0))
            bProduceCertificate = ToSafeBoolean(vPrintOptions(1, 0))
            bProduceDebitNote = ToSafeBoolean(vPrintOptions(2, 0))
        End If



        If bProduceSchedule Then
            'Generate schedule document.
            m_lReturn = m_oBusiness.GenerateDocument(ACDocTypeSchedule,
                                         ACSpoolDocMode,
                                         v_lNewPolicyCnt,
                                         v_lInsuranceFolder,
                                         v_lPartyCnt,
                                         "Accept Renewal - Schedule Document",
                                         v_sTransactionType:="RN")


            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                AcceptRenewal = PMEReturnCode.PMFalse
                Exit Function
            End If
        End If

        If bProduceCertificate Then
            'Generate certificate document.
            m_lReturn = m_oBusiness.GenerateDocument(ACDocTypeCertificate,
                                        ACSpoolDocMode,
                                        v_lNewPolicyCnt,
                                        v_lInsuranceFolder,
                                        v_lPartyCnt,
                                        "Accept Renewal -  Certificate Document",
                                        v_sTransactionType:="RN")

            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                AcceptRenewal = PMEReturnCode.PMFalse
                Exit Function
            End If
        End If

        If bProduceDebitNote Then
            'Generate debit note.
            m_lReturn = m_oBusiness.GenerateDocument(ACDOCTypeDebitNote,
                                        ACSpoolDocMode,
                                        v_lNewPolicyCnt,
                                        v_lInsuranceFolder,
                                        v_lPartyCnt,
                                        "Accept Renewal -  Debit Note Document",
                                        v_sTransactionType:="RN")

            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                AcceptRenewal = PMEReturnCode.PMFalse
                Exit Function
            End If
        End If

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: GetStats
    '
    ' Description:
    '
    ' History: 10/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function GetStats(ByVal v_lNewPolicyCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sFailureReason As String = ""
            Dim cThisPremium As Decimal

            'Set the Insurance file count
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "GetThisPremium"
#End If
            m_oControlTrans.InsuranceFileCnt = v_lNewPolicyCnt

            m_lReturn = m_oControlTrans.GetThisPremium(cThisPremium)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(AddAcceptFailure(v_vInsuranceFileCnt:=v_lNewPolicyCnt, v_vFailureReason:="GetStats - m_oControlTrans.GetThisPremium failed", v_vMethod:="GetStats"), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "GetThisPremium"
#End If

            If cThisPremium = 0 Then
                Return result
            End If

            m_lReturn = m_oControlTrans.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vTransactionType:=m_sTransactionType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(AddAcceptFailure(v_vInsuranceFileCnt:=v_lNewPolicyCnt, v_vFailureReason:="GetStats - m_oControlTrans.SetProcessModes failed", v_vMethod:="GetStats"), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oControlTrans.Start"
#End If
            m_lReturn = m_oControlTrans.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                sFailureReason = m_oControlTrans.Message
                sFailureReason = "Statistics process failed :" & Strings.ChrW(13) & Strings.ChrW(10) & Strings.ChrW(13) & Strings.ChrW(10) & sFailureReason
                m_lReturn = CType(AddAcceptFailure(v_vInsuranceFileCnt:=v_lNewPolicyCnt, v_vFailureReason:=sFailureReason, v_vMethod:="GetStats"), gPMConstants.PMEReturnCode)
                Return result
            End If
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oControlTrans.Start"
#End If

            Return result

        Catch



            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(AddAcceptFailure(v_vInsuranceFileCnt:=v_lNewPolicyCnt, v_vFailureReason:="GetStats Failed", v_vMethod:="GetStats"), gPMConstants.PMEReturnCode)

            Return result
        End Try

    End Function
    ' ***************************************************************** '
    ' Name: GetAccumulations
    '
    ' Description:
    '
    ' History: 10/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function GetAccumulations(ByVal v_lNewPolicyCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oAccumulationValues.InsuranceFileCnt = v_lNewPolicyCnt

            m_lReturn = m_oAccumulationValues.RepopulateAccumValues()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(AddAcceptFailure(v_vInsuranceFileCnt:=v_lNewPolicyCnt, v_vFailureReason:="bSirAccumulationValues.Business.RepopulateAccumValues Failed", v_vMethod:="GetAccumulations"), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '? m_oAccumulationValues.AnyFailed

            Return result

        Catch



            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(AddAcceptFailure(v_vInsuranceFileCnt:=v_lNewPolicyCnt, v_vFailureReason:="GetAccumulations Failed", v_vMethod:="GetAccumulations"), gPMConstants.PMEReturnCode)

            Return result
        End Try

    End Function

    Public Function CheckServiceLevelForRenewalAcceptance(ByVal lInsuranceFileCnt As Integer, ByVal lBatchRenewalJobID As Integer) As Boolean
        Dim bIsRenewalAcceptanceAllowed As Boolean
        Dim sMethodName As String = "start"
        Dim oVBQuoteEngine As SharedQuoteEngine.VBQuoteEngine
        Dim oSharedStorage As SharedStorage
        Dim sScript As String = String.Empty
        Dim oExtras As bGISPMUExtras.Business

        Try
            oExtras = New bGISPMUExtras.Business

            m_lReturn = oExtras.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=DirectCast(m_oDatabase, Object))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oExtras.InsuranceFileCnt = lInsuranceFileCnt

            ' Create script control object
            oVBQuoteEngine = New VBQuoteEngine()

            ' Create shared storage object, used to hold values that are
            ' read/writable from the VB script file
            oSharedStorage = New SharedStorage()

            oSharedStorage.InsuranceFileCnt = lInsuranceFileCnt
            oSharedStorage.BatchRenewalJobID = lBatchRenewalJobID

            ' Read in the script and run it
            m_lReturn = CType(GetScriptFile(v_sScript:=sScript), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                oVBQuoteEngine.ExecuteRenewalRuleScript(sScript, sMethodName, oSharedStorage, oExtras)
            End If

            ' Retrieve Service Level flag
            bIsRenewalAcceptanceAllowed = oSharedStorage.IsGovServiceLevel

            oVBQuoteEngine = Nothing

        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Check Eligibility for Renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckServiceLevelForRenewalAcceptance", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        End Try
        Return bIsRenewalAcceptanceAllowed
    End Function

    '*********************************************************************
    ' Name: GetScriptFile
    '
    ' Description : Find and read the VBScript file
    '
    ' Created: PW030103
    '*********************************************************************
    Private Function GetScriptFile(ByRef v_sScript As String) As Integer
        Dim result As Integer = 0
        Dim sFullPath As String = ""
        Dim iFile As Integer
        Dim lFileLength As Integer
        Dim sPathName As String = ""
        Dim lFileNumber As gPMConstants.PMEReturnCode
        Dim sStr, sStr2 As String

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Get the path to the validation script from the registry
            lFileNumber = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", v_sSubKey:="GIS", r_sSettingValue:=sPathName), gPMConstants.PMEReturnCode)

            ' Build the path to the script file
            sPathName = sPathName.Trim()
            If Not sPathName.EndsWith("\") And Not sPathName.EndsWith(":") Then
                sPathName = sPathName & "\"
            End If
            sFullPath = sPathName & "AUTO_RENEW_GOV_SB.rul"

            ' Ensure the file exists
            If FileSystem.Dir(sFullPath, FileAttribute.Normal) = "" Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AUTO_RENEW_GOV_SB.rul file not found", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScriptFile")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Open the VBscript file
            iFile = FileSystem.FreeFile()
            FileSystem.FileOpen(iFile, sFullPath, OpenMode.Input)
            lFileLength = FileSystem.LOF(iFile)

            ' Read the script into the string variable
            sStr2 = FileSystem.InputString(iFile, lFileLength)
            FileSystem.FileClose(iFile)

            ' Add the option explicit in case it's missing
            sStr = "Option Explicit" & Strings.ChrW(13) & Strings.ChrW(10)
            sStr = sStr & sStr2 & Strings.ChrW(13) & Strings.ChrW(10)

            ' Return the script
            v_sScript = sStr.Trim()

            Return result

        Catch excep As System.Exception
            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScriptFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Private Function AddAcceptFailure(ByVal v_vInsuranceFileCnt As Object, ByVal v_vFailureReason As Object, ByVal v_vMethod As Object) As Integer

        Dim result As Integer = 0


        'Log to the standard place
        If Informations.Err().Number > 0 Then

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=CStr(v_vFailureReason), vApp:=ACApp, vClass:=ACClass, vMethod:=v_vMethod, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        Else

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=CStr(v_vFailureReason), vApp:=ACApp, vClass:=ACClass, vMethod:=v_vMethod)
        End If

        'sj 22/01/2003 - start
        'PS104
        If m_bCalledFromMTA Then
            Return result
        End If
        'sj 22/01/2003 - end

        m_oDatabase.Parameters.Clear()


        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="Failure_Reason", vValue:=CStr(v_vFailureReason), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACRenewalAutomaticAcceptFailureAddSQL, sSQLName:=ACRenewalAutomaticAcceptFailureAddName, bStoredProcedure:=ACRenewalAutomaticAcceptFailureAddStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return result

    End Function

    Private Function ClearAcceptFailures() As Integer

        Dim result As Integer = 0


        m_oDatabase.Parameters.Clear()


        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACRenewalAutomaticAcceptFailureDelSQL, sSQLName:=ACRenewalAutomaticAcceptFailureDelName, bStoredProcedure:=ACRenewalAutomaticAcceptFailureDelStored)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return m_lReturn

    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans(Optional ByVal v_vInsuranceFileCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim sMessage As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If Not Informations.IsNothing(v_vInsuranceFileCnt) Then

                    sMessage = "Failed to Commit database transaction for " & CStr(v_vInsuranceFileCnt)
                Else
                    sMessage = "Failed to Commit database transaction"
                End If
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans(Optional ByVal v_vInsuranceFileCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim sMessage As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If Not Informations.IsNothing(v_vInsuranceFileCnt) Then

                    sMessage = "Failed to Commit database transaction for " & CStr(v_vInsuranceFileCnt)
                Else
                    sMessage = "Failed to Commit database transaction"
                End If
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans(Optional ByVal v_vInsuranceFileCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim sMessage As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If Not Informations.IsNothing(v_vInsuranceFileCnt) Then

                    sMessage = "Failed to Rollback database transaction for " & CStr(v_vInsuranceFileCnt)
                Else
                    sMessage = "Failed to Rollback database transaction"
                End If
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
