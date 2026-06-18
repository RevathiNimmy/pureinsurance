Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
'Module PMBToolbarFunc
Partial NotInheritable Class MainModule 

    ' ***************************************************************** '
    ' Functions called by Toolbar buttons and Menus.
    '
    ' Edit History: TF111298 - Created
    ' ***************************************************************** '

    ' Constant for the methods to identify which class this is.
    'Private Const ACClass As String = "SIRToolbarFunc"

    ' TF031298 - Constants to identify Toolbar Buttons
    ' TF291209 - Need to be Public for SIRToolbarFunc
    Public Const ACIButtonFinancial As Integer = 1
    Public Const ACIButtonPolicy As Integer = 2
    Public Const ACIButtonClaim As Integer = 3
    Public Const ACIButtonSeparator1 As Integer = 4
    Public Const ACIButtonNotes As Integer = 5
    Public Const ACIButtonLetter As Integer = 6
    Public Const ACIButtonSeperator2 As Integer = 7
    Public Const ACIButtonEmail As Integer = 8
    Public Const ACIButtonInternet As Integer = 9
    'DC041203
    Public Const ACIButtoniMarket As Integer = 10
    Public Const ACIButtonStickyNote As Integer = 11
    Public Const ACIButtonCashDeposit As Integer = 12 'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling

    'Private m_lReturn As Integer

    ' ***************************************************************** '
    ' Name: ProcessToolbar
    '
    ' Description:  TF291298 - Control calls to relevant components
    ' ***************************************************************** '
    'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
    'Public Function ProcessToolbar(ByRef v_iButton As Integer, Optional ByRef v_lPartyCnt As Byte = 0, Optional ByRef v_lInsuranceFolderCnt As Byte = 0, Optional ByRef v_lInsuranceFileCnt As Byte = 0, Optional ByRef v_lClaimCnt As Byte = 0, Optional ByRef v_sShortName As String = "", Optional ByRef v_lRiskCnt As Integer = 0, Optional ByRef v_vUserProp As Object = Nothing, Optional ByRef v_sResolvedName As String = "") As Integer
    Public Function ProcessToolbar(ByRef v_iButton As Integer, Optional ByRef v_lPartyCnt As Object = Nothing, Optional ByRef v_lInsuranceFolderCnt As Object = Nothing, Optional ByRef v_lInsuranceFileCnt As Object = Nothing, Optional ByRef v_lClaimCnt As Object = Nothing, Optional ByRef v_sShortName As String = "", Optional ByRef v_lRiskCnt As Integer = 0, Optional ByRef v_vUserProp As Object = Nothing, Optional ByRef v_sResolvedName As String = "") As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "ProcessToolbar" 'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
        Dim oBrowser As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case v_iButton
                Case ACIButtonPolicy
                    ' Call Find Policy component, passing Party details
                    m_lReturn = CallFindPolicy(v_sShortName:=v_sShortName)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallFindPolicy().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                Case ACIButtonClaim
                    ' Call Find Claim component, passing Party details
                    m_lReturn = CallFindClaim()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallFindClaim().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                Case ACIButtonFinancial
                    ' Call Find Account component, passing Party details
                    m_lReturn = CallFindAccount()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallFindAccount().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                Case ACIButtonNotes
                    'TF201103 - PN7730 - Dealt with at menu level

                Case ACIButtonLetter
                    ' Call Find Account component, passing Party details
                    m_lReturn = CallLetter(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lClaimCnt:=v_lClaimCnt, v_lRiskCnt:=v_lRiskCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallLetter().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                Case ACIButtonInternet
                    '            Dim frmBrowser As New frmBrowser
                    '            With frmBrowser
                    '                .StartingAddress = "http://www.policymaster.com/"
                    '                .Show vbModal
                    '            End With
                    '            Unload frmBrowser
                    '            Set frmBrowser = Nothing
                    'oBrowser = New PMBWebBrowser.Browser() 'deepak commented as currently working on PC flow only'
                    oBrowser = New Object
                Case ACIButtonEmail
                    m_lReturn = CallEMail(v_lPartyCnt:=v_lPartyCnt, v_vUserProp:=v_vUserProp)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallLetter().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                    'DC041203
                Case ACIButtoniMarket
                    m_lReturn = CalliMarket()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CalliMarket().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If
                    '2005 Roadmap Add StickyNote
                Case ACIButtonStickyNote
                    ' Call Find Note component, passing Party details
                    m_lReturn = CallAddStickyNote(v_lPartyCnt:=v_lPartyCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallAddStickyNote().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If
                    'Start - Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
                Case ACIButtonCashDeposit
                    m_lReturn = CallCashDeposit(v_lPartyCnt:=v_lPartyCnt, v_sPartyCode:=v_sShortName, v_sPartyName:=v_sResolvedName)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "CallCashDeposit Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                    'End - Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
                Case Else
                    MessageBox.Show("Component Not Yet Available", Application.ProductName)

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process Button/Menu.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CallFindPolicy
    '
    ' Description:  TF031298
    '               Call Find Policy component, passing Party details
    ' ***************************************************************** '
    Public Function CallFindPolicy(Optional ByRef v_sShortName As String = "") As Integer
        Dim result As Integer = 0
        Dim oFindPolicy As iPMBFindInsurance.Interface_Renamed

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create FindPolicy object
            Dim temp_oFindPolicy As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindPolicy, sClassName:="iPMBFindInsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindPolicy = temp_oFindPolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iSIRFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_sShortName = "" Then
                ' Set Default Insurance Holder to Party Shortname

                'Developer Guide No. 65(Guide)
                'oFindPolicy.ShortName = frmPartyPC.uctPartyPCControl1.IDReference.Trim()
                oFindPolicy.ShortName = m_sShortName.Trim
            Else
                ' Set Default Insurance Holder to Party Shortname

                oFindPolicy.ShortName = v_sShortName
            End If

            ' Call Start method on Interface class

            m_lReturn = oFindPolicy.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process 'iSIRFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse

                oFindPolicy.Dispose()
                oFindPolicy = Nothing
                Return result
            End If

            ' Destroy FindPolicy object

            oFindPolicy.Dispose()
            oFindPolicy = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Find Policy.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            If Not (oFindPolicy Is Nothing) Then

                oFindPolicy.Dispose()
                oFindPolicy = Nothing
            End If

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CallFindClaim
    '
    ' Description:  TF031298
    '               Call Find Claim component, passing Party details
    ' ***************************************************************** '
    Public Function CallFindClaim() As Integer
        Dim result As Integer = 0
        'TODO
        'Dim oFindClaim As iSIRFindClaim.Interface_Renamed
        Dim oFindClaim As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create FindClaim object
            Dim temp_oFindClaim As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindClaim, sClassName:="iSIRFindClaim.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindClaim = temp_oFindClaim

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iSIRFindClaim.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Default Insurance Holder to Party Shortname

            'Developer Guide No. 65(Guide)
            'oFindClaim.PolicyHolder = frmPartyPC.uctPartyPCControl1.IDReference.Trim()
            oFindClaim.PolicyHolder = m_sShortName.Trim

            ' Call Start method on Interface class

            m_lReturn = oFindClaim.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process 'iSIRFindClaim.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse

                oFindClaim.Dispose()
                oFindClaim = Nothing
                Return result
            End If

            ' Destroy FindClaim object

            oFindClaim.Dispose()
            oFindClaim = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Find Claim.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            If Not (oFindClaim Is Nothing) Then

                oFindClaim.Dispose()
                oFindClaim = Nothing
            End If

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CallFindAccount
    '
    ' Description:  TF031298
    '               Call Find Account component, passing Party details
    ' ***************************************************************** '
    Public Function CallFindAccount() As Integer
        Dim result As Integer = 0
        Dim oFindAccount As iACTFindAccount.Interface_Renamed


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create FindPolicy object
            Dim temp_oFindAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindAccount, sClassName:="iACTFindAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindAccount = temp_oFindAccount

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iACTFindAccount.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindAccount", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Default Insurance Holder to Party Shortname

            'NIIT - Replaced with the Migrated code 1144
            'oFindAccount.ShortCode = frmPartyPC.ShortName
            oFindAccount.ShortCode = DirectCast(Application.OpenForms("frmPartyPC"), frmPartyPC).ShortName

            ' Call Start method on Interface class

            m_lReturn = oFindAccount.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process 'iACTFindAccount.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindAccount", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse

                oFindAccount.Dispose()
                oFindAccount = Nothing
                Return result
            End If

            ' Destroy FindAccount object

            oFindAccount.Dispose()
            oFindAccount = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Find Account.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindAccount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            If Not (oFindAccount Is Nothing) Then

                oFindAccount.Dispose()
                oFindAccount = Nothing
            End If

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CalliMarket
    '
    ' Description:  DC041203
    '               Call iMarket component
    ' ***************************************************************** '
    Public Function CalliMarket() As Integer
        Dim result As Integer = 0
        Dim oiMarket As iPMBiMarket.Interface_Renamed


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create FindPolicy object
            Dim temp_oiMarket As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oiMarket, sClassName:="iPMBiMarket.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oiMarket = temp_oiMarket

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iPMBiMarket.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CalliMarket", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call Start method on Interface class

            m_lReturn = oiMarket.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process 'iPMBiMarket.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CalliMarket", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse

                oiMarket.Dispose()
                oiMarket = Nothing
                Return result
            End If

            ' Destroy FindAccount object

            oiMarket.Dispose()
            oiMarket = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process iMarket.", vApp:=ACApp, vClass:=ACClass, vMethod:="CalliMarket", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            If Not (oiMarket Is Nothing) Then

                oiMarket.Dispose()
                oiMarket = Nothing
            End If

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CallNotes
    '
    ' Description:  TF031298
    '               Call FreeformText component, passing Party details
    '               MS220601
    '               Added Note Date for highlighting and party cnt
    ' ***************************************************************** '
    Public Function CallNotes(ByRef v_sEntityType As String, ByRef v_lEntityCnt As Integer, ByRef v_sTextType As String, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_sNoteDate As Date = #12/30/1899#) As Integer
        Dim result As Integer = 0
        Dim oFreeformText As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create FreeformText object
            Dim temp_oFreeformText As Object
            'Developer Guie No 267
            'm_lReturn = g_oObjectManager.GetInstance(temp_oFreeformText, sClassName:="iPMBFreeformText.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_lReturn = g_oObjectManager.GetInstance(temp_oFreeformText, sClassName:="iPMBFreeFormText.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFreeformText = temp_oFreeformText

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iSIRFreeformText.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallNotes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Pass key data to FreeformText component
            With oFreeformText
                'ECK 06/05/99
                '        .EntityName = SIREntityNameParty
                '        .KeyFieldValue = frmPartyPC.PartyCnt

                .EntityName = v_sEntityType

                .KeyFieldValue = v_lEntityCnt

                .Texttype = v_sTextType

                ' MS220106

                .PartyCnt = v_lPartyCnt
                If Strings.Len(v_sNoteDate) > 0 Then
                    ' Pass Date for highlight

                    .CallingAppName = "EventLog"

                    .NoteDate = v_sNoteDate
                End If

            End With

            ' Call Start method on Interface class

            m_lReturn = oFreeformText.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process 'iSIRFreeformText.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallNotes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse

                oFreeformText.Dispose()
                oFreeformText = Nothing
                Return result
            End If

            ' Destroy FreeformText object

            oFreeformText.Dispose()
            oFreeformText = Nothing

            ' MSS06062001. No return state coming back from function
            ' Added one for Event notes so we know whether to refresh the grid or not.
            ' It's pointless and annoying to refresh if we cancel.
            If v_sEntityType = "Event" Then
                Me.m_frmParentMdiForm.Focus()
                result = m_lReturn
            End If

            '    If m_vfrmListEvents.Visible = True Then
            '        frmListEvents.RefreshList
            '    End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Freeform Text.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallNotes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            If Not (oFreeformText Is Nothing) Then

                oFreeformText.Dispose()
                oFreeformText = Nothing
            End If

            Return result

        End Try
    End Function

    '' ***************************************************************** '
    '' Name: CallLetter
    ''
    '' Description:  TF031298
    ''               Call CreateDocument component, passing Party details
    '' ***************************************************************** '
    'Public Function CallLetter(Optional v_lPartyCnt As Variant = 0) As Long
    '
    'Dim oCreateDocument As Object
    '
    '    On Error GoTo Err_CallLetter
    '
    '    CallLetter = PMTrue
    '
    '    ' Create CreateDocument object
    '    m_lReturn = g_oObjectManager.GetInstance( _
    ''        oObject:=oCreateDocument, _
    ''        sClassName:="iPMBCreateDocument.Interface", _
    ''        vInstanceManager:=PMGetLocalInterface)
    '
    '    If (m_lReturn <> PMTrue) Then
    '        LogMessage _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Failed to create 'iPMBCreateDocument.Interface'.", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="CallLetter", _
    ''            vErrNo:=Err.Number, _
    ''            vErrDesc:=Err.Description
    '        CallLetter = PMFalse
    '        Exit Function
    '    End If
    '
    '    If (v_lPartyCnt = 0) Then
    '        ' Pass key data to CreateDocument component
    '        m_lReturn = oCreateDocument.SetProperties( _
    ''            v_sEntityName:=SIREntityNameParty, _
    ''            v_lEntityID:=frmPartyPC.PartyCnt)
    '    Else
    '        ' Pass key data to CreateDocument component
    '        m_lReturn = oCreateDocument.SetProperties( _
    ''            v_sEntityName:=SIREntityNameParty, _
    ''            v_lEntityID:=v_lPartyCnt)
    '    End If
    '    ' Call Start method on Interface class
    '    m_lReturn = oCreateDocument.Start()
    '
    '    If (m_lReturn <> PMTrue) Then
    '        LogMessage _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Failed to process 'iPMBCreateDocument.Interface'.", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="CallLetter", _
    ''            vErrNo:=Err.Number, _
    ''            vErrDesc:=Err.Description
    '        CallLetter = PMFalse
    '        m_lReturn = oCreateDocument.Terminate()
    '        Set oCreateDocument = Nothing
    '        Exit Function
    '    End If
    '
    '    ' Destroy CreateDocument object
    '    m_lReturn = oCreateDocument.Terminate()
    '    Set oCreateDocument = Nothing
    '
    '    Exit Function
    '
    'Err_CallLetter:
    '
    '    CallLetter = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to process Create Document.", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="CallLetter", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    If (oCreateDocument Is Nothing = False) Then
    '        m_lReturn = oCreateDocument.Terminate()
    '        Set oCreateDocument = Nothing
    '    End If
    '
    '    Exit Function
    '
    'End Function

    ' ***************************************************************** '
    ' Name: CallEMail
    '
    ' Description:  TF031298
    '               Call Find Policy component, passing Party details
    ' ***************************************************************** '
    Public Function CallEMail(ByRef v_lPartyCnt As Object, Optional ByRef v_vUserProp As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim oEmail As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Email object
            Dim temp_oEmail As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oEmail, sClassName:="iPMBEmail.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oEmail = temp_oEmail

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iPMBEmail.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallEMail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oEmail.SetKeys(vKeyArray:=v_vUserProp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to StKeys in 'iPMBEmail.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallEMail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            oEmail.PartyCnt = v_lPartyCnt

            ' Call Start method on Interface class

            m_lReturn = oEmail.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process 'iPMBEmail.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallEMail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse

                oEmail.Dispose()
                oEmail = Nothing
                Return result
            End If

            ' Destroy EMAil object

            oEmail.Dispose()
            oEmail = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Email.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallEMail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            If Not (oEmail Is Nothing) Then

                oEmail.Dispose()
                oEmail = Nothing
            End If

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CallLetter
    '
    ' Description:  TO190899
    '               Use the new
    ' ***************************************************************** '
    Public Function CallLetter(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimCnt As Integer, Optional ByVal v_lRiskCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lDocumentTemplateId, lDocumentTypeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetTheTemplate(r_lDocumentTemplateId:=lDocumentTemplateId, r_lDocumentTypeId:=lDocumentTypeId)

            If lDocumentTemplateId = 0 Then
                Return result
            End If

            m_lReturn = UseDocTemplate(v_lDocumentTemplateId:=lDocumentTemplateId, v_lDocumentTypeId:=lDocumentTypeId, v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lClaimCnt:=v_lClaimCnt, v_lRiskCnt:=v_lRiskCnt)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Create Document.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallLetter", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetTheTemplate(ByRef r_lDocumentTemplateId As Integer, ByRef r_lDocumentTypeId As Integer) As Integer

        'Dim o As iPMBFindDocTemplate.Interface
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim o As New iPMBFindDocTemplate.Interface_Renamed

        m_lReturn = CType(o, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            o = Nothing
            Return result
        End If

        'CJR 28/11/2002: Required for IAG - Proof of concept
        o.CallingAppName = ACApp

        m_lReturn = o.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

        o.Mode = 1

        m_lReturn = o.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            o = Nothing
            Return result
        End If

        r_lDocumentTemplateId = o.DocumentTemplateId
        r_lDocumentTypeId = o.DocumentTypeId

		o.Dispose()


        Return result
    End Function

    Private Function UseDocTemplate(ByVal v_lDocumentTemplateId As Integer, ByVal v_lDocumentTypeId As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimCnt As Integer, Optional ByVal v_lRiskCnt As Integer = 0) As Integer
        Dim result As Integer = 0
        'Dim oObject As iPMBDocTemplate.Interface

        Dim oObject As iPMBDocTemplate.Interface_Renamed
        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oObject As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oObject = temp_oObject

        '    m_lReturn = oObject.Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            oObject = Nothing
            Return result
        End If

        'm_lReturn = oObject.SetProcessModes(vTask:=PMEdit)


        oObject.PartyCnt = v_lPartyCnt

        oObject.InsuranceFolderCnt = v_lInsuranceFolderCnt

        oObject.InsuranceFileCnt = v_lInsuranceFileCnt

        oObject.ClaimCnt = v_lClaimCnt


        oObject.DocumentTemplateId = v_lDocumentTemplateId

        oObject.DocumentTypeId = v_lDocumentTypeId

        oObject.Mode = 1

        oObject.RiskCnt = v_lRiskCnt


        m_lReturn = oObject.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            oObject = Nothing
            Return result
        End If


		oObject.Dispose()


        Return result
    End Function
    ' ***************************************************************** '
    ' Name: CallAddStickyNote
    '
    ' Description:  ECK 25052005
    '               Call Event Note component, passing Party details
    ' ***************************************************************** '
    Public Function CallAddStickyNote(ByRef v_lPartyCnt As Object) As Integer

        Dim result As Integer = 0
        Dim oNotes As iPMBNote.Interface_Renamed

        Dim fIndex As Integer
        Dim lEventCnt As Integer
        Dim sPriorityCode As String = ""
        Dim lSubjectId As Integer
        Dim sSubjectDesc, sTypeDesc, sDescription As String
        Dim lFormTopPosToSearchFrom, lFormLeftPosToSearchFrom As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Notes object
            Dim temp_oNotes As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oNotes, sClassName:="iPMBNote.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oNotes = temp_oNotes

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iSIRFreeformText.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallAddStickyNote", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Pass key data to Note component

            oNotes.PartyCnt = CInt(v_lPartyCnt)
            oNotes.AddSticky = True
            oNotes.UserName = g_oObjectManager.UserName

            m_lReturn = oNotes.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            ' Call Start method on Interface class
            m_lReturn = oNotes.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process 'iPMBNote.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallAddStickyNote", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse
                oNotes.Dispose()
                oNotes = Nothing
                Return result
            End If

            If oNotes.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                lEventCnt = oNotes.EventCnt
                lSubjectId = oNotes.EventLogSubjectId
                sSubjectDesc = oNotes.SubjectDesc
                sTypeDesc = oNotes.TypeDesc
                sPriorityCode = oNotes.PriorityCode
                sDescription = oNotes.Description

                fIndex = FindFreeIndex()

                If Information.IsArray(g_vWarnings) Then

                    ReDim Preserve g_vWarnings(0, (g_vWarnings.GetUpperBound(1)) + 1)
                Else
                    ReDim g_vWarnings(0, 0)
                End If


                g_vWarnings(0, g_vWarnings.GetUpperBound(1)) = fIndex

                Document(fIndex) = New frmWarning(m_frmParentMdiForm)

                With Document(fIndex)

                    .ModuleClass = Me

                    .EventCnt = lEventCnt

                    .EventDate = DateTime.Now

                    .Subject = sSubjectDesc

                    .PriorityCode = sPriorityCode

                    .Username = g_oObjectManager.UserName

                    .SubjectId = lSubjectId

                    .EventType = sTypeDesc

                    .Description = sDescription

                    .Tag = fIndex

                    ' Instead of just adding new notes in same pos over each other, tile them a bit in next
                    ' free location  PN23111
                    lFormTopPosToSearchFrom = 1300
                    lFormLeftPosToSearchFrom = 11100
                    m_lReturn = FindNextFreeStickyNotePosition(r_lFormTopPosToSearchFrom:=lFormTopPosToSearchFrom, r_lFormLeftPosToSearchFrom:=lFormLeftPosToSearchFrom)
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        .FormTop = lFormTopPosToSearchFrom

                        .FormLeft = lFormLeftPosToSearchFrom
                    Else

                        .FormTop = 1300

                        .FormLeft = 11100
                    End If


                    .EventCnt = lEventCnt

                    .LoadInterface()
                End With
            End If

            oNotes.Dispose()
            oNotes = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Sticky Notes.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallAddStickyNote", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result

        End Try
    End Function

    'Start - Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
    Public Function CallCashDeposit(ByVal v_lPartyCnt As Integer, ByVal v_sPartyCode As String, ByVal v_sPartyName As String) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "CallCashDeposit"



        Dim oCashDeposit As iSIRCashDeposit.Interface_Renamed

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oCashDeposit As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oCashDeposit, sClassName:="iSIRCashDeposit.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oCashDeposit = temp_oCashDeposit


        m_lReturn = CType(oCashDeposit, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetInstance Failed", gPMConstants.PMELogLevel.PMLogError)
        End If



        oCashDeposit.PartyCnt = v_lPartyCnt

        oCashDeposit.PartyCode = v_sPartyCode

        oCashDeposit.PartyName = v_sPartyName

        oCashDeposit.FromAgentOrClientMaintenance = True

        oCashDeposit.Task = gPMConstants.PMEComponentAction.PMView


        m_lReturn = oCashDeposit.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Start Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


		oCashDeposit.Dispose()



        Return result
    End Function
    'End - Sankar - (WPR85_Cash_Deposit_Process) - Paralleling

    ' ***************************************************************** '
    ' Name: FindNextFreeStickyNotePosition
    '
    ' Description:  Tries to find a free and unused Sticky Note position
    '               so that a new one is added to a unique place on the
    '               screen and not over the top of a previous one.
    '               Note this is a recursive function.
    ' History :
    ' CJB 110805 Added for PN23111
    ' ***************************************************************** '
    Function FindNextFreeStickyNotePosition(ByRef r_lFormTopPosToSearchFrom As Integer, ByRef r_lFormLeftPosToSearchFrom As Integer) As Integer

        Dim result As Integer = 0
        Dim ArrayCount As Integer
        Dim lFormTopPosToSearchFrom, lFormLeftPosToSearchFrom As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ArrayCount = Document.GetUpperBound(0)

            ' Cycle through the document array. Search for (and return) any unused top and left co-ordinates for form
            For i As Integer = 1 To ArrayCount
                ' Only inspect forms that are not hidden/deleted or else they will load
                If Not FState(i).Deleted Then
                    ' Only look thru the Sticky Notes

                    If Document(i).Name = "frmWarning" Then



                        If Document(i).FormTop = r_lFormTopPosToSearchFrom And Document(i).FormLeft = r_lFormLeftPosToSearchFrom Then

                            ' There is a form in this pos...need to check next pos down !
                            ' Call ourselves recursively
                            lFormTopPosToSearchFrom = r_lFormTopPosToSearchFrom + 350
                            lFormLeftPosToSearchFrom = r_lFormLeftPosToSearchFrom + 200
                            m_lReturn = FindNextFreeStickyNotePosition(r_lFormTopPosToSearchFrom:=lFormTopPosToSearchFrom, r_lFormLeftPosToSearchFrom:=lFormLeftPosToSearchFrom)
                            'Save our positions that have been returned and exit
                            r_lFormTopPosToSearchFrom = lFormTopPosToSearchFrom
                            r_lFormLeftPosToSearchFrom = lFormLeftPosToSearchFrom
                            Exit For

                        End If
                    End If
                End If
            Next

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to find next free Sticky Note position", vApp:=ACApp, vClass:=ACClass, vMethod:="FindNextFreeStickyNotePosition", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
End Class
