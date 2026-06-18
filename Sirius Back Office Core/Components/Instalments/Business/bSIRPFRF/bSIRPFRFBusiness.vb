Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 23/10/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a PFRF.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 13/01/2004
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

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_bChanged As Boolean

    Private lPMAuthorityLevel As Integer

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)




    Public Property Changed() As Boolean
        Get

            Return m_bChanged

        End Get
        Set(ByVal Value As Boolean)

            m_bChanged = Value

        End Set
    End Property

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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


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
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                End If
                m_oLookup = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
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


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single PFRF directly into the database.
    '        Note: The PFRF will NOT be added to the collection.
    ' Edit History  :
    ' RAM20030401   : Added v_vExistingDaysDelay Optional Parameter
    '                   Issue 2915 Changes
    'Start (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.1.1)
    'Added the v_vFinanceNetCommission optional parameter
    'End (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.1.1)
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function DirectAdd(Optional ByRef r_lPFRF_ID As Integer = 0, Optional ByRef v_lCompanyno As Integer = 0, Optional ByVal v_lSchemeno As Integer = 0,
                              Optional ByVal v_lSchemeversion As Integer = 0,
                              Optional ByVal v_vStartdate As Object = Nothing, Optional ByVal v_vArrangementFee As Object = Nothing,
                              Optional ByVal v_vMnemonic As Object = Nothing, Optional ByVal v_vEndDate As Object = Nothing,
                              Optional ByVal v_vDaysDelay As Object = Nothing, Optional ByVal v_vDepositPC As Object = Nothing,
                              Optional ByVal v_vProductFamily As Object = Nothing, Optional ByVal v_vProtectRate As Object = Nothing,
                              Optional ByVal v_vMinInterest As Object = Nothing, Optional ByVal v_vMin1 As Object = Nothing,
                              Optional ByVal v_vMax1 As Object = Nothing, Optional ByVal v_vRate1 As Object = Nothing,
                              Optional ByVal v_vR1Com As Object = Nothing, Optional ByVal v_vMin2 As Object = Nothing,
                              Optional ByVal v_vMax2 As Object = Nothing, Optional ByVal v_vRate2 As Object = Nothing,
                              Optional ByVal v_vR2Com As Object = Nothing, Optional ByVal v_vMin3 As Object = Nothing,
                              Optional ByVal v_vMax3 As Object = Nothing, Optional ByVal v_vRate3 As Object = Nothing,
                              Optional ByVal v_vR3Com As Object = Nothing, Optional ByVal v_vMin4 As Object = Nothing,
                              Optional ByVal v_vMax4 As Object = Nothing, Optional ByVal v_vRate4 As Object = Nothing,
                              Optional ByVal v_vR4Com As Object = Nothing, Optional ByVal v_vMin5 As Object = Nothing,
                              Optional ByVal v_vMax5 As Object = Nothing, Optional ByVal v_vRate5 As Object = Nothing,
                              Optional ByVal v_vR5Com As Object = Nothing, Optional ByVal v_vMinMTA As Object = Nothing,
                              Optional ByVal v_vMinMTAInstalments As Object = Nothing, Optional ByRef v_vpffrequencyID As Object = Nothing,
                              Optional ByRef v_vTaxChargedTo As Object = Nothing, Optional ByRef v_vFeeType As Object = Nothing,
                              Optional ByRef v_vFeeChargedTo As Object = Nothing, Optional ByRef v_vProtectionType As Object = Nothing,
                              Optional ByRef v_vProtectionChargedTo As Object = Nothing, Optional ByRef v_vDepositType As Object = Nothing,
                              Optional ByRef v_vDepositChargedTo As Object = Nothing, Optional ByRef v_vBackdatedRollupTo As Object = Nothing,
                              Optional ByRef v_vAlignTo As Object = Nothing, Optional ByRef v_vStartLimit As Object = Nothing,
                              Optional ByRef v_vRecollectOnNext As Object = Nothing, Optional ByRef v_vRecollectDays As Object = Nothing,
                              Optional ByRef v_vRetryLimit As Object = Nothing, Optional ByRef v_vMTAOnNextInstalment As Object = Nothing,
                              Optional ByRef v_vDetailsArray As Object = Nothing, Optional ByVal v_vExistingDaysDelay As Object = Nothing,
                              Optional ByVal v_vStatementFrequID As Object = Nothing, Optional ByVal v_vStatementReportID As Object = Nothing,
                              Optional ByVal v_vAdvanceInstalments As Object = Nothing, Optional ByVal v_vUserGroup As Object = Nothing,
                              Optional ByVal v_vRemainderThreshhold As Object = Nothing, Optional ByVal v_vRemainderAtEnd As Object = Nothing,
                              Optional ByVal vParams() As Object = Nothing, Optional ByVal bDepositOverrideAllowed As Boolean = True,
                              Optional ByVal bApplyFeePercentagesToPolicyRisk As Boolean = True, Optional ByVal bApplyFeePercentagesToTaxes As Boolean = True,
                              Optional ByVal v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "", Optional ByVal v_vTransactionType As Object = Nothing) As Integer



        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                'Start (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.1.1)
                'Added the v_vFinanceNetCommission optional parameter
                'End (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.1.1)
                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                AddKeyParam(r_lPFRF_ID, v_vStartdate, v_vProductFamily, v_sUniqueId, v_sScreenHierarchy)
                AddInputParam(v_lCompanyno, v_lSchemeno, v_lSchemeversion, v_vArrangementFee, v_vMnemonic, v_vEndDate, v_vDaysDelay, v_vDepositPC, v_vProductFamily,
                              v_vProtectRate, v_vMinInterest, v_vMin1, v_vMax1, v_vRate1, v_vR1Com, v_vMin2, v_vMax2, v_vRate2, v_vR2Com, v_vMin3, v_vMax3, v_vRate3,
                              v_vR3Com, v_vMin4, v_vMax4, v_vRate4, v_vR4Com, v_vMin5, v_vMax5, v_vRate5, v_vR5Com, v_vMinMTA, v_vMinMTAInstalments, v_vpffrequencyID,
                              v_vTaxChargedTo, v_vFeeType, v_vFeeChargedTo, v_vProtectionType, v_vProtectionChargedTo, v_vDepositType, v_vDepositChargedTo,
                              v_vBackdatedRollupTo, v_vAlignTo, v_vStartLimit, v_vRecollectOnNext, v_vRecollectDays, v_vRetryLimit, v_vMTAOnNextInstalment,
                              v_vExistingDaysDelay, v_vStatementFrequID, v_vStatementReportID, v_vAdvanceInstalments, v_vUserGroup, v_vRemainderThreshhold,
                               v_vRemainderAtEnd, vParams(ACMaxInstalments), vParams(ACFinanceNetCommission), vParams(ACSingleInstalmentPerMonth),
                               vParams(ACFirstInstalmentAlignWithDayInMonth), bDepositOverrideAllowed, bApplyFeePercentagesToPolicyRisk:=bApplyFeePercentagesToPolicyRisk,
                               bApplyFeePercentagesToTaxes:=bApplyFeePercentagesToTaxes, v_vTransactionType:=v_vTransactionType)

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectEdit (Public)
    '
    ' Description: Updates a single PFRF directly into the database.
    '
    ' Edit History  :
    ' RAM20030401   : Added v_vExistingDaysDelay Optional Parameter
    '                   Issue 2915 Changes
    'Start (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.1.1)
    'Added the v_vFinanceNetCommission optional parameter
    'End (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.1.1)
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function DirectEdit(Optional ByRef r_lPFRF_ID As Integer = 0, Optional ByVal v_lCompanyno As Integer = 0, Optional ByVal v_lSchemeno As Integer = 0,
                               Optional ByVal v_lSchemeversion As Integer = 0, Optional ByVal v_vStartdate As Object = Nothing, Optional ByVal v_vArrangementFee As Object = Nothing,
                               Optional ByVal v_vMnemonic As Object = Nothing, Optional ByVal v_vEndDate As Object = Nothing, Optional ByVal v_vDaysDelay As Object = Nothing,
                               Optional ByVal v_vDepositPC As Object = Nothing, Optional ByVal v_vProductFamily As Object = Nothing, Optional ByVal v_vProtectRate As Object = Nothing,
                               Optional ByVal v_vMinInterest As Object = Nothing, Optional ByVal v_vMin1 As Object = Nothing, Optional ByVal v_vMax1 As Object = Nothing,
                               Optional ByVal v_vRate1 As Object = Nothing, Optional ByVal v_vR1Com As Object = Nothing, Optional ByVal v_vMin2 As Object = Nothing,
                               Optional ByVal v_vMax2 As Object = Nothing, Optional ByVal v_vRate2 As Object = Nothing, Optional ByVal v_vR2Com As Object = Nothing,
                               Optional ByVal v_vMin3 As Object = Nothing, Optional ByVal v_vMax3 As Object = Nothing, Optional ByVal v_vRate3 As Object = Nothing,
                               Optional ByVal v_vR3Com As Object = Nothing, Optional ByVal v_vMin4 As Object = Nothing, Optional ByVal v_vMax4 As Object = Nothing,
                               Optional ByVal v_vRate4 As Object = Nothing, Optional ByVal v_vR4Com As Object = Nothing, Optional ByVal v_vMin5 As Object = Nothing,
                               Optional ByVal v_vMax5 As Object = Nothing, Optional ByVal v_vRate5 As Object = Nothing, Optional ByVal v_vR5Com As Object = Nothing,
                               Optional ByVal v_vMinMTA As Object = Nothing, Optional ByVal v_vMinMTAInstalments As Object = Nothing,
                               Optional ByRef v_vpffrequencyID As Object = Nothing, Optional ByRef v_vTaxChargedTo As Object = Nothing,
                               Optional ByRef v_vFeeType As Object = Nothing, Optional ByRef v_vFeeChargedTo As Object = Nothing,
                               Optional ByRef v_vProtectionType As Object = Nothing, Optional ByRef v_vProtectionChargedTo As Object = Nothing,
                               Optional ByRef v_vDepositType As Object = Nothing, Optional ByRef v_vDepositChargedTo As Object = Nothing,
                               Optional ByRef v_vBackdatedRollupTo As Object = Nothing, Optional ByRef v_vAlignTo As Object = Nothing,
                               Optional ByRef v_vStartLimit As Object = Nothing, Optional ByRef v_vRecollectOnNext As Object = Nothing,
                               Optional ByRef v_vRecollectDays As Object = Nothing, Optional ByRef v_vRetryLimit As Object = Nothing,
                               Optional ByRef v_vMTAOnNextInstalment As Object = Nothing, Optional ByRef v_vDetailsArray As Object = Nothing,
                               Optional ByVal v_vExistingDaysDelay As Object = Nothing, Optional ByVal v_vStatementFrequID As Object = Nothing,
                               Optional ByVal v_vStatementReportID As Object = Nothing, Optional ByVal v_vAdvanceInstalments As Object = Nothing,
                               Optional ByVal v_vUserGroup As Object = Nothing, Optional ByVal v_vRemainderThreshhold As Object = Nothing,
                               Optional ByVal v_vRemainderAtEnd As Object = Nothing, Optional ByVal vParams() As Object = Nothing,
                               Optional ByVal bDepositOverrideAllowed As Boolean = True, Optional ByVal bApplyFeePercentagesToPolicyRisk As Boolean = True, Optional ByVal bApplyFeePercentagesToTaxes As Boolean = True,
                               Optional ByVal v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "", Optional ByVal v_vTransactionType As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase
                'Start (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.1.1)
                'Added the v_vFinanceNetCommission optional parameter
                'End (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.1.1)
                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                AddKeyParam(r_lPFRF_ID, v_vStartdate, v_vProductFamily, v_sUniqueId, v_sScreenHierarchy)
                AddInputParam(v_lCompanyno, v_lSchemeno, v_lSchemeversion, v_vArrangementFee, v_vMnemonic, v_vEndDate, v_vDaysDelay, v_vDepositPC, v_vProductFamily, v_vProtectRate,
                              v_vMinInterest, v_vMin1, v_vMax1, v_vRate1, v_vR1Com, v_vMin2, v_vMax2, v_vRate2, v_vR2Com, v_vMin3, v_vMax3, v_vRate3, v_vR3Com, v_vMin4, v_vMax4,
                              v_vRate4, v_vR4Com, v_vMin5, v_vMax5, v_vRate5, v_vR5Com, v_vMinMTA, v_vMinMTAInstalments, v_vpffrequencyID, v_vTaxChargedTo, v_vFeeType,
                              v_vFeeChargedTo, v_vProtectionType, v_vProtectionChargedTo, v_vDepositType, v_vDepositChargedTo, v_vBackdatedRollupTo, v_vAlignTo, v_vStartLimit,
                              v_vRecollectOnNext, v_vRecollectDays, v_vRetryLimit, v_vMTAOnNextInstalment, v_vExistingDaysDelay, v_vStatementFrequID, v_vStatementReportID,
                              v_vAdvanceInstalments, v_vUserGroup, v_vRemainderThreshhold, v_vRemainderAtEnd, vParams(ACMaxInstalments), vParams(ACFinanceNetCommission),
                              vParams(ACSingleInstalmentPerMonth), vParams(ACFirstInstalmentAlignWithDayInMonth), bDepositOverrideAllowed, bApplyFeePercentagesToPolicyRisk:=bApplyFeePercentagesToPolicyRisk,
                              bApplyFeePercentagesToTaxes:=bApplyFeePercentagesToTaxes, v_vTransactionType:=v_vTransactionType)

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectEdit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectEdit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single PFRF directly from the database.
    '        Note: The PFRF will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByVal v_lPFRF_ID As Integer = 0, Optional ByVal v_vStartdate As Object = Nothing, Optional ByVal v_vProductFamily As Object = Nothing, Optional ByVal v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer


        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                AddKeyParam(v_lPFRF_ID, v_vStartdate, v_vProductFamily, v_sUniqueId, v_sScreenHierarchy)

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If record wasn't deleted, error
                If lRecordsAffected > 0 Then
                    ' Deleted, No action required
                Else
                    result = gPMConstants.PMEReturnCode.PMFail
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = m_lReturn

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required PFRFs and populate the Collection
    ' Edit History  :
    ' RAM20030402   : Added r_vExistingDaysDelay Optional Parameter
    '                   Issue 2915 Changes
    ' ***************************************************************** '
    Public Function GetDetails(ByVal v_lPFRF_ID As Integer, ByRef r_vResultArray(,) As Object, Optional ByVal v_vLockMode As gPMConstants.PMELockMode = 0) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Information.IsNothing(v_vLockMode)) Or (Not Double.TryParse(CStr(v_vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                v_vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If
            With m_oDatabase

                ' Add parameter
                .Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="PFRF_ID", vValue:=CStr(v_lPFRF_ID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                ' Check if records returned
                If Not Information.IsArray(r_vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No record were returned.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End With
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    ' PRIVATE Methods (End)


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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds the supplied parameters to the m_oDatabase.
    ' Edit History  :
    ' RAM20030401   : Added v_vExistingDaysDelay Optional Parameter
    '                   Issue 2915 Changes
    'Start (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.1.1)
    'Added the v_vFinanceNetCommission optional parameter
    'End (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.1.1)
    ' ***************************************************************** '
    Private Sub AddInputParam(ByVal v_lCompanyno As Integer, ByVal v_lSchemeno As Integer, ByVal v_lSchemeversion As Integer, Optional ByVal v_vArrangementFee As Object = Nothing,
                              Optional ByVal v_vMnemonic As Object = Nothing, Optional ByVal v_vEndDate As Object = Nothing, Optional ByVal v_vDaysDelay As String = "",
                              Optional ByVal v_vDepositPC As String = "", Optional ByVal v_vProductFamily As Object = Nothing, Optional ByVal v_vProtectRate As String = "",
                              Optional ByVal v_vMinInterest As String = "", Optional ByVal v_vMin1 As Object = Nothing, Optional ByRef v_vMax1 As Object = Nothing,
                              Optional ByVal v_vRate1 As Object = Nothing, Optional ByVal v_vR1Com As Object = Nothing, Optional ByVal v_vMin2 As Object = Nothing,
                              Optional ByVal v_vMax2 As Object = Nothing, Optional ByVal v_vRate2 As Object = Nothing, Optional ByVal v_vR2Com As Object = Nothing,
                              Optional ByVal v_vMin3 As Object = Nothing, Optional ByVal v_vMax3 As Object = Nothing, Optional ByVal v_vRate3 As Object = Nothing,
                              Optional ByVal v_vR3Com As Object = Nothing, Optional ByVal v_vMin4 As Object = Nothing, Optional ByVal v_vMax4 As Object = Nothing,
                              Optional ByVal v_vRate4 As Object = Nothing, Optional ByVal v_vR4Com As Object = Nothing, Optional ByVal v_vMin5 As Object = Nothing,
                              Optional ByVal v_vMax5 As Object = Nothing, Optional ByVal v_vRate5 As Object = Nothing, Optional ByVal v_vR5Com As Object = Nothing,
                              Optional ByVal v_vMinMTA As Object = Nothing, Optional ByVal v_vMinMTAInstalments As Object = Nothing, Optional ByVal v_vpffrequencyID As Object = Nothing,
                              Optional ByVal v_vTaxChargedTo As Object = Nothing, Optional ByVal v_vFeeType As Object = Nothing, Optional ByVal v_vFeeChargedTo As Object = Nothing,
                              Optional ByVal v_vProtectionType As Object = Nothing, Optional ByVal v_vProtectionChargedTo As Object = Nothing,
                              Optional ByVal v_vDepositType As Object = Nothing, Optional ByVal v_vDepositChargedTo As Object = Nothing,
                              Optional ByVal v_vBackdatedRollupTo As Object = Nothing, Optional ByVal v_vAlignTo As Object = Nothing, Optional ByVal v_vStartLimit As Object = Nothing,
                              Optional ByVal v_vRecollectOnNext As Object = Nothing, Optional ByVal v_vRecollectDays As Object = Nothing,
                              Optional ByVal v_vRetryLimit As Object = Nothing, Optional ByVal v_vMTAOnNextInstalment As Object = Nothing,
                              Optional ByVal v_vExistingDaysDelay As String = "", Optional ByVal v_vStatementFrequID As Object = Nothing,
                              Optional ByVal v_vStatementReportID As Object = Nothing, Optional ByVal v_vAdvanceInstalments As Object = Nothing,
                              Optional ByVal v_vUserGroup As Object = Nothing, Optional ByVal v_vRemainderThreshhold As Object = Nothing,
                              Optional ByVal v_vRemainderAtEnd As Object = Nothing, Optional ByVal v_vMaxInstalments As Object = Nothing,
                              Optional ByVal v_vFinanceNetCommission As Object = Nothing,
                              Optional v_iSingleInstalmentPerMonth As Integer = 0, Optional ByVal v_iFirstInstalmentAlignWithDayInMonth As Integer = 0,
                              Optional ByVal bDepositOverrideAllowed As Boolean = True, Optional ByVal bApplyFeePercentagesToPolicyRisk As Boolean = True, Optional ByVal bApplyFeePercentagesToTaxes As Boolean = True,
                              Optional ByVal v_vTransactionType As Object = Nothing)

        With m_oDatabase

            'm_lReturn = .Parameters.Add(sName:="mta_on_next_instalment", vValue:=CStr(v_vMTAOnNextInstalment), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = .Parameters.Add(sName:="CompanyNo", vValue:=v_lCompanyno, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="SchemeNo", vValue:=v_lSchemeno, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="SchemeVersion", vValue:=v_lSchemeversion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="ArrangementFee", vValue:=CStr(v_vArrangementFee), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Mnemonic", vValue:=CStr(v_vMnemonic), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = .Parameters.Add(sName:="EndDate", vValue:=CStr(v_vEndDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = .Parameters.Add(sName:="DaysDelay", vValue:=v_vDaysDelay, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If v_vDepositPC = "" Then v_vDepositPC = 0
            m_lReturn = .Parameters.Add(sName:="DepositPC", vValue:=v_vDepositPC, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If v_vProtectRate = "" Then v_vProtectRate = CStr(0)
            m_lReturn = .Parameters.Add(sName:="ProtectRate", vValue:=v_vProtectRate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            If v_vMinInterest = "" Then v_vMinInterest = CStr(0)
            m_lReturn = .Parameters.Add(sName:="MinInterest", vValue:=v_vMinInterest, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Min1", vValue:=CStr(v_vMin1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Max1", vValue:=CStr(v_vMax1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Rate1", vValue:=CStr(v_vRate1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="R1Com", vValue:=CStr(v_vR1Com), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Min2", vValue:=CStr(v_vMin2), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Max2", vValue:=CStr(v_vMax2), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Rate2", vValue:=CStr(v_vRate2), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="R2Com", vValue:=CStr(v_vR2Com), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Min3", vValue:=CStr(v_vMin3), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Max3", vValue:=CStr(v_vMax3), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Rate3", vValue:=CStr(v_vRate3), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="R3Com", vValue:=CStr(v_vR3Com), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Min4", vValue:=CStr(v_vMin4), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Max4", vValue:=CStr(v_vMax4), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Rate4", vValue:=CStr(v_vRate4), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="R4Com", vValue:=CStr(v_vR4Com), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Min5", vValue:=CStr(v_vMin5), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Max5", vValue:=CStr(v_vMax5), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="Rate5", vValue:=CStr(v_vRate5), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="R5Com", vValue:=CStr(v_vR5Com), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="MinMTA", vValue:=CStr(v_vMinMTA), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_lReturn = .Parameters.Add(sName:="MinMTAInstalments", vValue:=v_vMinMTAInstalments, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="pfFrequency_id", vValue:=v_vpffrequencyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .Parameters.Add(sName:="tax_charged_to", vValue:=v_vTaxChargedTo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="fee_type", vValue:=v_vFeeType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="fee_charged_to", vValue:=v_vFeeChargedTo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="protection_type", vValue:=v_vProtectionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="protection_charged_to", vValue:=v_vProtectionChargedTo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="deposit_type", vValue:=v_vDepositType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="deposit_charged_to", vValue:=v_vDepositChargedTo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="backdated_rollup_to", vValue:=v_vBackdatedRollupTo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="align_to", vValue:=v_vAlignTo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="start_limit", vValue:=v_vStartLimit, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="recollect_on_next", vValue:=v_vRecollectOnNext, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="recollect_days", vValue:=v_vRecollectDays, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_lReturn = .Parameters.Add(sName:="retry_limit", vValue:=v_vRetryLimit, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)



            m_lReturn = .Parameters.Add(sName:="mta_on_next_instalment", vValue:=v_vMTAOnNextInstalment, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            'Ends
            'Fill in Missing Parameters
            'Developer Guide No. 173 (Latest Guide)
            If v_vDepositPC > 0 Then
                m_lReturn = .Parameters.Add(sName:="DepositReq", vValue:="Y", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="DepositReq", vValue:="N", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If
            'Developer Guide No. 173 (Latest Guide)
            If v_vProtectRate > 0 Then
                m_lReturn = .Parameters.Add(sName:="AllowProtection", vValue:="Y", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lReturn = .Parameters.Add(sName:="Protect", vValue:="Y", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="AllowProtection", vValue:="N", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lReturn = .Parameters.Add(sName:="Protect", vValue:="N", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            m_lReturn = .Parameters.Add(sName:="AllowOveride", vValue:="N", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            '''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030401 : Added the following parameter
            '               Ref. Issue 2915
            '''''''''''''''''''''''''''''''''''''''''''''''
            If v_vExistingDaysDelay = "" Then v_vExistingDaysDelay = v_vDaysDelay
            m_lReturn = .Parameters.Add(sName:="existing_days_delay", vValue:=v_vExistingDaysDelay, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            '''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030401   : END
            '''''''''''''''''''''''''''''''''''''''''''''''

            ' Alix Bergeret - 07/04/2003

            'Developer Guide No 85
            If Convert.IsDBNull(v_vStatementFrequID) Then
                m_lReturn = .Parameters.Add(sName:="statement_frequency_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="statement_frequency_id", vValue:=v_vStatementFrequID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            'Developer Guide No 85
            If Convert.IsDBNull(v_vStatementReportID) Then
                m_lReturn = .Parameters.Add(sName:="statement_report_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="statement_report_id", vValue:=v_vStatementReportID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If



            'Developer Guide No 85
            If Convert.IsDBNull(v_vAdvanceInstalments) Then
                m_lReturn = .Parameters.Add(sName:="advance_instalments", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="advance_instalments", vValue:=v_vAdvanceInstalments, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If



            'Developer Guide No 85
            If Convert.IsDBNull(v_vUserGroup) Then
                m_lReturn = .Parameters.Add(sName:="review_pmuser_group_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="review_pmuser_group_id", vValue:=v_vUserGroup, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If



            'Developer Guide No 85
            If Convert.IsDBNull(v_vRemainderThreshhold) Then
                m_lReturn = .Parameters.Add(sName:="remainder_threshhold", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="remainder_threshhold", vValue:=v_vRemainderThreshhold, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If



            'Developer Guide No 85
            If Convert.IsDBNull(v_vRemainderAtEnd) Then
                m_lReturn = .Parameters.Add(sName:="remainder_at_end", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="remainder_at_end", vValue:=v_vRemainderAtEnd, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If


            'Developer Guide No 85
            If Convert.IsDBNull(v_vMaxInstalments) Then
                m_lReturn = .Parameters.Add(sName:="maximum_instalments", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="maximum_instalments", vValue:=v_vMaxInstalments, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If


            'Developer Guide No 85
            If Convert.IsDBNull(v_vFinanceNetCommission) Then
                m_lReturn = .Parameters.Add(sName:="finance_net_commission", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="finance_net_commission", vValue:=v_vFinanceNetCommission, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            m_lReturn = .Parameters.Add(sName:="single_instalment_per_month", vValue:=v_iSingleInstalmentPerMonth, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = .Parameters.Add(sName:="first_instalment_align_with_day_in_month", vValue:=v_iFirstInstalmentAlignWithDayInMonth, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = .Parameters.Add(sName:="is_deposit_override_allowed", vValue:=bDepositOverrideAllowed, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            'Add parameter to apply_fee_percentages_to_fees and apply_fee_percentages_to_taxes for WPR04
            m_lReturn = .Parameters.Add(sName:="apply_fee_percentages_to_fees", vValue:=bApplyFeePercentagesToPolicyRisk, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            m_lReturn = .Parameters.Add(sName:="apply_fee_percentages_to_taxes", vValue:=bApplyFeePercentagesToTaxes, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            ' ADO #39993: Pass transaction_type for Claim Recovery rate configuration
            If Convert.IsDBNull(v_vTransactionType) OrElse v_vTransactionType Is Nothing Then
                m_lReturn = .Parameters.Add(sName:="transaction_type", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="transaction_type", vValue:=v_vTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

        End With

    End Sub

    Private Sub AddKeyParam(ByVal v_lPFRF_ID As Integer, ByRef v_vStartdate As Object, ByRef v_vProductFamily As Object, Optional ByVal v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "")

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="PFRF_ID", vValue:=v_lPFRF_ID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="StartDate", vValue:=CStr(v_vStartdate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = .Parameters.Add(sName:="ProductFamily", vValue:=CStr(v_vProductFamily), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="ScreenHierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        End With

    End Sub

    Public Function GetFrequencies(ByRef v_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'changed to spu SW 20/01/2003

            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_PFScheme_Frequencies_List", sSQLName:="spe_PFScheme_PrintType_List", bStoredProcedure:=True, vResultArray:=v_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If Not Information.IsArray(v_vResultArray) Then
                'There is nothing to do
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFrequencies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFrequencies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetReports(ByRef v_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'changed to spu SW 20/01/2003

            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_get_all_reports", sSQLName:="spu_get_all_reports", bStoredProcedure:=True, vResultArray:=v_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            If Not Information.IsArray(v_vResultArray) Then
                'There is nothing to do
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReports Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReports", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetUserGroups(ByRef v_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'changed to spu SW 20/01/2003

            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_get_all_pmgroups", sSQLName:="spu_get_all_pmgroups", bStoredProcedure:=True, vResultArray:=v_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If Not Information.IsArray(v_vResultArray) Then
                'There is nothing to do
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
