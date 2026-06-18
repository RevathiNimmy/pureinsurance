Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Modified by Sumeet Singh on 5/11/2010 6:48:39 PM refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 21/09/00
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' RAW 13/11/2003 : CQ1765 : pass RunMode to AutoMTA object
    ' RAW 24/11/2003 : CQ685 : allow NilPremiumRefund to be set for multiple insurance file cnts
    ' ***************************************************************** '

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPMAuthorityLevel As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_iMode As Integer

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    Private m_oAutoMTA As AutoMta


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public ReadOnly Property ReferReasons() As Object
        Get
            Return m_oAutoMTA.ReferReasons
        End Get
    End Property
    Public ReadOnly Property DeclineReasons() As Object
        Get
            Return m_oAutoMTA.DeclineReasons
        End Get
    End Property
    ' RAW 13/11/2003 : CQ1765 : added
    Public WriteOnly Property RunMode() As Integer
        Set(ByVal Value As Integer)
            If Not (m_oAutoMTA Is Nothing) Then
                m_oAutoMTA.RunMode = Value
            End If
        End Set
    End Property

    '*************************************************************************
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    '******************************************************************
    Public Function Initialise() As Integer





        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            m_oAutoMTA = New AutoMta()

            m_lReturn = m_oAutoMTA.CreateBusinessObjectsServer()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.CreateBusinessObjectsServer Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                m_oAutoMTA = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            '    For lRow& = LBound(vKeyArray, 2) To UBound(vKeyArray, 2)
            '        ' Assign the parameter member with the
            '        ' correct key array item.
            '
            '        ' {* USER DEFINED CODE (Begin) *}
            '
            '        Select Case Trim$(CStr(vKeyArray(PMKeyName, lRow&)))
            '            Case PMKeyNameInsFileCnt
            '                m_lInsuranceFileCnt = CLng(vKeyArray(PMKeyValue, lRow&))
            '
            '        End Select
            '
            '        ' {* USER DEFINED CODE (End) *}
            '    Next lRow&
            '
            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try


            ' {* USER DEFINED CODE (Begin) *}
            '    ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.


            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0

        Try



            ' {* USER DEFINED CODE (Begin) *}

            'ReDim vSummaryArray(2, 0)

            ' Assign the key array with the parameter members.

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default status to OK
            m_lStatus = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: QuoteCancellation
    '
    ' Description:
    '
    ' History: 10/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function QuoteCancellation(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_lNewInsuranceFileCnt As Integer, Optional ByVal v_vStatusBar As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sErrorText As String = ""

            m_oAutoMTA.PartyCnt = v_lPartyCnt
            m_oAutoMTA.InsuranceFolderCnt = v_lInsuranceFolderCnt
            m_oAutoMTA.TransactionType = "MTC"
            m_oAutoMTA.EffectiveDate = v_dtEffectiveDate
            m_oAutoMTA.UpdateStats = False
            m_oAutoMTA.NewInsuranceFileCnt = v_lNewInsuranceFileCnt

            If Not Information.IsNothing(v_vStatusBar) Then


                'Modified by Sumeet Singh on 5/11/2010 6:57:12 PM refer developer guide no. 24
                'm_oAutoMTA.set_StatusBar(v_vStatusBar)
                m_oAutoMTA.StatusBar = v_vStatusBar
            End If

            m_lReturn = CType(m_oAutoMTA.AutoCancelMTA(r_sErrorText:=sErrorText), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If sErrorText <> "" Then
                    MessageBox.Show(sErrorText, "AutoCancelMTA", MessageBoxButtons.OK)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.AutoCancelMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteCancellation")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QuoteCancellation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteCancellation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: QuoteMTA
    '
    ' Description:
    '
    ' History: 10/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function QuoteMTA(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_lNewInsuranceFileCnt As Integer, Optional ByVal v_vListPolicies As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sErrorText As String = ""

            m_oAutoMTA.PartyCnt = v_lPartyCnt
            m_oAutoMTA.InsuranceFolderCnt = v_lInsuranceFolderCnt
            m_oAutoMTA.TransactionType = "MTA"
            m_oAutoMTA.EffectiveDate = v_dtEffectiveDate
            m_oAutoMTA.UpdateStats = False
            m_oAutoMTA.MergeRisks = True
            m_oAutoMTA.NewInsuranceFileCnt = v_lNewInsuranceFileCnt

            If Not Information.IsNothing(v_vListPolicies) Then


                'Modified by Sumeet Singh on 5/11/2010 6:58:06 PM refer developer guide no. 24
                'm_oAutoMTA.set_ListPolicies(v_vListPolicies)
                m_oAutoMTA.ListPolicies = v_vListPolicies
            End If

            m_lReturn = CType(m_oAutoMTA.AutoBackdatedMTA(r_sErrorText:=sErrorText), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If sErrorText <> "" Then
                    MessageBox.Show(sErrorText, "AutoCancelMTA", MessageBoxButtons.OK)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.AutoBackdatedMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteMTA")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QuoteMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteMTA", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name:  QuoteReinstatement
    '
    ' Description:
    '
    ' History: 10/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function QuoteReinstatement(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer, Optional ByVal v_vStatusBar As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sErrorText As String = ""

            m_oAutoMTA.PartyCnt = v_lPartyCnt
            m_oAutoMTA.InsuranceFolderCnt = v_lInsuranceFolderCnt
            m_oAutoMTA.TransactionType = "MTR"
            m_oAutoMTA.EffectiveDate = DateTime.Now
            m_oAutoMTA.UpdateStats = False
            m_oAutoMTA.NewInsuranceFileCnt = v_lNewInsuranceFileCnt

            If Not Information.IsNothing(v_vStatusBar) Then


                'Modified by Sumeet Singh on 5/11/2010 6:58:37 PM refer developer guide no. 24
                'm_oAutoMTA.set_StatusBar(v_vStatusBar)
                m_oAutoMTA.StatusBar = v_vStatusBar
            End If

            m_lReturn = CType(m_oAutoMTA.AutoReinstateMTA(r_sErrorText:=sErrorText), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If sErrorText <> "" Then
                    MessageBox.Show(sErrorText, "AutoCancelMTA", MessageBoxButtons.OK)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.AutoCancelMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteCancellation")
                End If
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QuoteCancellation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteCancellation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: QuoteReinstateRisk
    '
    ' Description:
    '
    ' History: 08/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function QuoteReinstateRisk(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lDeletedRiskInsuranceFileCnt As Integer, ByVal v_lDeletedRiskCnt As Integer, ByVal v_sReinstatementReason As String, ByRef r_lNewInsuranceFileCnt As Integer, Optional ByVal v_vStatusBar As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sErrorText As String = ""

            m_oAutoMTA.PartyCnt = v_lPartyCnt
            m_oAutoMTA.InsuranceFolderCnt = v_lInsuranceFolderCnt
            m_oAutoMTA.DeletedRiskInsuranceFileCnt = v_lDeletedRiskInsuranceFileCnt
            m_oAutoMTA.DeletedRiskCnt = v_lDeletedRiskCnt
            m_oAutoMTA.IsReinstateRisk = True
            m_oAutoMTA.TransactionType = "MTA"
            m_oAutoMTA.UpdateStats = False
            m_oAutoMTA.ReinstatementReason = v_sReinstatementReason


            If Not Information.IsNothing(v_vStatusBar) Then


                'Modified by Sumeet Singh on 5/11/2010 6:58:56 PM refer developer guide no. 24
                'm_oAutoMTA.set_StatusBar(v_vStatusBar)
                m_oAutoMTA.StatusBar = v_vStatusBar
            End If

            m_lReturn = CType(m_oAutoMTA.AutoReinstateRisk(r_sErrorText:=sErrorText), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If sErrorText <> "" Then
                    MessageBox.Show(sErrorText, "QuoteReinstateRisk", MessageBoxButtons.OK)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.QuoteReinstateRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteReinstateRisk")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result
            End If

            r_lNewInsuranceFileCnt = m_oAutoMTA.NewInsuranceFileCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QuoteReinstateRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteReinstateRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: TransactPolicyVersions
    '
    ' Description:
    '
    ' History: 17/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function TransactPolicyVersions(ByVal v_lNewInsuranceFileCnt As Integer, Optional ByVal v_bIsReinstateRisk As Boolean = False, Optional ByVal v_lDeletedRiskCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oAutoMTA.NewInsuranceFileCnt = v_lNewInsuranceFileCnt
            m_oAutoMTA.DeletedRiskCnt = v_lDeletedRiskCnt
            m_oAutoMTA.IsReinstateRisk = v_bIsReinstateRisk

            m_lReturn = CType(m_oAutoMTA.TransactPolicyVersions(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.TransactPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: RestoreAutoRunMTA
    '
    ' Description:
    '
    ' History: 17/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function RestoreAutoRunMTA(ByVal v_lNewInsuranceFileCnt As Integer, Optional ByVal v_bKeepQuote As Boolean = True) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oAutoMTA.NewInsuranceFileCnt = v_lNewInsuranceFileCnt

            m_lReturn = CType(m_oAutoMTA.RestoreAutoRunMTA(v_bKeepQuote:=v_bKeepQuote), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.RestoreAutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteCancellation")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestoreAutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAutoRunMTA", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AutoCancelMTA
    '
    ' Description:
    '
    ' History: 08/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function AutoCancelMTA(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtEffectiveDate As Date) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sErrorText As String = ""

            m_oAutoMTA.PartyCnt = v_lPartyCnt
            m_oAutoMTA.InsuranceFolderCnt = v_lInsuranceFolderCnt
            m_oAutoMTA.TransactionType = "MTC"
            m_oAutoMTA.EffectiveDate = v_dtEffectiveDate
            m_oAutoMTA.UpdateStats = True

            m_lReturn = CType(m_oAutoMTA.AutoCancelMTA(r_sErrorText:=sErrorText), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If sErrorText <> "" Then
                    MessageBox.Show(sErrorText, "AutoCancelMTA", MessageBoxButtons.OK)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.AutoCancelMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelMTA")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoCancelMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelMTA", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: AutoRiskChangeMTA
    '
    ' Description:
    '
    ' History: 08/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    'Modified by Sumeet Singh on 5/11/2010 7:01:06 PM refer developer guide no. 33
    'Public Function AutoRiskChangeMTA(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_vObjectPropertyArray( ,  ) As Object) As Integer
    Public Function AutoRiskChangeMTA(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_vObjectPropertyArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sErrorText As String = ""

            m_oAutoMTA.PartyCnt = v_lPartyCnt
            m_oAutoMTA.InsuranceFolderCnt = v_lInsuranceFolderCnt
            m_oAutoMTA.TransactionType = "MTA"
            m_oAutoMTA.EffectiveDate = v_dtEffectiveDate
            m_oAutoMTA.UpdateStats = True
            'Modified by Sumeet Singh on 5/11/2010 6:59:39 PM refer developer guide no. 24
            'm_oAutoMTA.set_ObjectPropertyArray(v_vObjectPropertyArray)
            m_oAutoMTA.ObjectPropertyArray = v_vObjectPropertyArray
            m_oAutoMTA.MergeRisks = True

            m_lReturn = CType(m_oAutoMTA.AutoRiskChangeMTA(r_sErrorText:=sErrorText), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If sErrorText <> "" Then
                    MessageBox.Show(sErrorText, "AutoRiskChangeMTA", MessageBoxButtons.OK)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.AutoRiskChangeMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRiskChangeMTA")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoRiskChangeMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoRiskChangeMTA", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AutoNCDChangeMTA
    '
    ' Description:
    '
    ' History: 08/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function AutoNCDChangeMTA(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_sNCDReason As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sErrorText As String = ""

            m_oAutoMTA.PartyCnt = v_lPartyCnt
            m_oAutoMTA.InsuranceFolderCnt = v_lInsuranceFolderCnt
            m_oAutoMTA.TransactionType = "MTA"
            m_oAutoMTA.EffectiveDate = v_dtEffectiveDate
            m_oAutoMTA.UpdateStats = True
            m_oAutoMTA.NCDReason = v_sNCDReason
            m_oAutoMTA.IsNCDChange = True
            m_oAutoMTA.MergeRisks = False

            m_lReturn = CType(m_oAutoMTA.AutoNCDChangeMTA(r_sErrorText:=sErrorText), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If sErrorText <> "" Then
                    MessageBox.Show(sErrorText, "AutoNCDChangeMTA", MessageBoxButtons.OK)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.AutoNCDChangeMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoNCDChangeMTA")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoNCDChangeMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoNCDChangeMTA", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: AutoReinstateRisk
    '
    ' Description:
    '
    ' History: 08/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function AutoReinstateRisk(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lDeletedRiskInsuranceFileCnt As Integer, ByVal v_lDeletedRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sErrorText As String = ""

            m_oAutoMTA.PartyCnt = v_lPartyCnt
            m_oAutoMTA.InsuranceFolderCnt = v_lInsuranceFolderCnt
            m_oAutoMTA.DeletedRiskInsuranceFileCnt = v_lDeletedRiskInsuranceFileCnt
            m_oAutoMTA.DeletedRiskCnt = v_lDeletedRiskCnt
            m_oAutoMTA.IsReinstateRisk = True
            m_oAutoMTA.TransactionType = "MTA"
            m_oAutoMTA.UpdateStats = True

            m_lReturn = CType(m_oAutoMTA.AutoReinstateRisk(r_sErrorText:=sErrorText), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If sErrorText <> "" Then
                    MessageBox.Show(sErrorText, "AutoReinstateRisk", MessageBoxButtons.OK)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.AutoReinstateRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoReinstateRisk")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoReinstateRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoReinstateRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name:  AutoReinstateMTA
    '
    ' Description:
    '
    ' History: 10/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function AutoReinstateMTA(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtCurrentDate As Date) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sErrorText As String = ""

            m_oAutoMTA.PartyCnt = v_lPartyCnt
            m_oAutoMTA.InsuranceFolderCnt = v_lInsuranceFolderCnt
            m_oAutoMTA.TransactionType = "MTR"
            m_oAutoMTA.EffectiveDate = DateTime.Now
            m_oAutoMTA.UpdateStats = True
            m_oAutoMTA.CurrentDate = v_dtCurrentDate

            m_lReturn = CType(m_oAutoMTA.AutoReinstateMTA(r_sErrorText:=sErrorText), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If sErrorText <> "" Then
                    MessageBox.Show(sErrorText, "AutoCancelMTA", MessageBoxButtons.OK)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.AutoCancelMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteCancellation")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QuoteCancellation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteCancellation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetNillPremiumRefund
    '
    ' Description:
    '
    ' History: 31/01/2003 sj - Created.
    ' RAW 24/11/2003 : CQ685 : added v_vAffectedInsuranceFileCnts param
    ' ***************************************************************** '
    Public Function SetNillPremiumRefund(Optional ByVal v_vAffectedInsuranceFileCnts As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            ' RAW 24/11/2003 : CQ685 : added v_vAffectedInsuranceFileCnts argument


            Return m_oAutoMTA.SetNillPremiumRefund(v_vAffectedInsuranceFileCnts:=v_vAffectedInsuranceFileCnts)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetNillPremiumRefund Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNillPremiumRefund", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function IsBackdatedMTARequired(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_lNewInsuranceFileCnt As Integer) As Boolean

        Dim result As Boolean = False
        Try

            m_oAutoMTA.NewInsuranceFileCnt = v_lNewInsuranceFileCnt
            m_oAutoMTA.EffectiveDate = v_dtEffectiveDate
            m_oAutoMTA.InsuranceFolderCnt = v_lInsuranceFolderCnt


            Return m_oAutoMTA.IsBackdatedMTARequired()

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsBackdatedMTARequired Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsBackdatedMTARequired", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function MultipleVersionsExist(Optional ByVal v_lNewInsuranceFileCnt As Integer = 0) As Boolean

        If v_lNewInsuranceFileCnt <> 0 Then
            m_oAutoMTA.NewInsuranceFileCnt = v_lNewInsuranceFileCnt
        End If
        Return m_oAutoMTA.MultipleVersionsExist

    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)


    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
