Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Modified by Sudhanshu Behera on 6/1/2010 7:10:35 PM refer developer guide no. 110
Public Module SIRToolbarFunc
	
	' ***************************************************************** '
	' Functions called by Toolbar buttons and Menus.
	'
	' Edit History: TF111298 - Created
	' ***************************************************************** '
	
	' Constant for the methods to identify which class this is.
	Private Const ACClass As String = "SIRToolbarFunc"
    Public g_oObjectManager As Object
    Public g_iUserID As Integer
	' TF031298 - Constants to identify Toolbar Buttons
	' TF291209 - Need to be Public for SIRToolbarFunc
    Public Const ACIButtonFinancial As Integer = 1
    Public Const ACIButtonCommission As Integer = 2
    Public Const ACIButtonSeparator1 As Integer = 3
    Public Const ACIButtonNotes As Integer = 4
    Public Const ACIButtonLetter As Integer = 5
    Public Const ACIButtonCD As Integer = 6 'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
    
	
	Private m_lReturn As Integer
	
	' ***************************************************************** '
	' Name: ProcessToolbar
	'
	' Description:  TF291298 - Control calls to relevant components
	' ***************************************************************** '
	'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling

    'Public Function ProcessToolbar(ByRef v_iButton As Integer, Optional ByVal v_lPartyCnt As Byte = 0, Optional ByVal v_sPartyCode As String = "", Optional ByVal v_sPartyName As String = "") As Integer
    Public Function ProcessToolbar(ByRef v_iButton As Integer, Optional ByVal v_lPartyCnt As Object = Nothing, Optional ByVal v_sPartyCode As String = "", Optional ByVal v_sPartyName As String = "") As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer = 0
        Const kMethodName As String = "ProcessToolbar" 'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If g_oObjectManager Is Nothing Then


                g_oObjectManager = CreateLateBoundObject("bObjectManager.ObjectManager")
                ' Call the initialise method.
                'lReturn = g_oObjectManager.Initialise(sCallingAppName:=MainModule.ACApp)
                lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Get Instance of bObjectManager.ObjectManager Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Store the language ID from the object manager
                ' to the public variables, to enable us to use
                ' them throughout the object.
                'With g_oObjectManager

                With g_oObjectManager
                    g_iLanguageID = .LanguageID
                    g_iSourceID = .SourceID
                    g_iUserID = .UserID
                End With


            End If




            Select Case v_iButton
                Case ACIButtonFinancial
                    ' Call Find Account component, passing Party details
                    m_lReturn = CallFindAccount()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallFindAccount().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                Case ACIButtonCommission
                    ' Call Agent Commission component, passing Party details
                    m_lReturn = CallAgentCommission()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallAgentCommission().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                Case ACIButtonNotes
                    ' Call Find Account component, passing Party details
                    m_lReturn = CallNotes(v_lPartyCnt:=v_lPartyCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallNotes().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If
                Case ACIButtonLetter
                    ' Call Find Account component, passing Party details

                    'm_lReturn = CallLetter(v_lPartyCnt:=frmInterface.PartyCnt)
                    m_lReturn = CallLetter(v_lPartyCnt:=v_lPartyCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallLetter().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                Case ACIButtonCD
                    m_lReturn = CallCashDeposit(v_lPartyCnt:=v_lPartyCnt, v_sPartyCode:=v_sPartyCode, v_sPartyName:=v_sPartyName)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "CallCashDeposit Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

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
    ' Name: CallFindAccount
    '
    ' Description:  TF031298
    '               Call Find Account component, passing Party details
    ' ***************************************************************** '
    Public Function CallFindAccount() As Integer
        Dim result As Integer = 0
        Dim oFindAccount As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create FindPolicy object
            Dim temp_oFindAccount As Object = Nothing
            'm_lReturn = g_oObjectManager.GetInstance(temp_oFindAccount, "iACTFindAccount.Interface_Renamed", gPMConstants.PMGetLocalInterface)
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindAccount, "iACTFindAccount.Interface_Renamed", gPMConstants.PMGetLocalInterface)
            oFindAccount = temp_oFindAccount

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iACTFindAccount.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindAccount", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Default Insurance Holder to Party Shortname

            'todolist
            'oFindAccount.ShortCode = frmInterface.ShortName.Trim()

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
    ' Name: CallAgentCommission
    '
    ' Description:  Call Agent Commission component, passing Party details
    '
    ' ***************************************************************** '
    Private Function CallAgentCommission() As Integer

        Dim result As Integer = 0
        
            'todolist
            'm_lReturn = frmInterface.ShowRates()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            Return m_lReturn

    End Function

    ' ***************************************************************** '
    ' Name: CallNotes
    '
    ' Description:  TF031298
    '               Call FreeformText component, passing Party details
    ' ***************************************************************** '
    Public Function CallNotes(ByVal v_lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim oFreeformText As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create FreeformText object
            Dim temp_oFreeformText As Object = Nothing
            'm_lReturn = g_oObjectManager.GetInstance(temp_oFreeformText, "iPMBFreeformText.Interface_Renamed", gPMConstants.PMGetLocalInterface)
            m_lReturn = g_oObjectManager.GetInstance(temp_oFreeformText, sClassName:="iPMBFreeFormText.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFreeformText = temp_oFreeformText

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iPMBFreeformText.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallNotes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Pass key data to FreeformText component
            'TF021203 - PN7730 - Public notes only
            'Party Bank Details
            If v_lPartyCnt > 0 Then
                With oFreeformText

                    .EntityName = gSIRLibrary.SIREntityNameParty

                    .KeyFieldValue = v_lPartyCnt

                    .Texttype = "Public"

                    .PartyCnt = v_lPartyCnt
                End With
            Else
                With oFreeformText

                    .EntityName = gSIRLibrary.SIREntityNameParty

                    'todolist
                    '.KeyFieldValue = frmInterface.PartyCnt

                    .Texttype = "Public"

                    'todolist
                    '.PartyCnt = frmInterface.PartyCnt
                End With
            End If

            ' Call Start method on Interface class

            m_lReturn = oFreeformText.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process 'iPMBFreeformText.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallNotes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse

                oFreeformText.Dispose()
                oFreeformText = Nothing
                Return result
            End If

            ' Destroy FreeformText object

            oFreeformText.Dispose()
            oFreeformText = Nothing

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
    'Public Function CallLetter() As Long
    '
    'Dim oCreateDocument As Object
    '
    '    On Error GoTo Err_CallLetter
    '
    '    CallLetter = PMTrue
    '
    '    ' Create CreateDocument object
    '    m_lReturn& = g_oObjectManager.GetInstance( _
    ''        oObject:=oCreateDocument, _
    ''        sClassName:="iPMBCreateDocument.Interface", _
    ''        vInstanceManager:=PMGetLocalInterface)
    '
    '    If (m_lReturn& <> PMTrue) Then
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
    '    ' Pass key data to CreateDocument component
    '    m_lReturn& = oCreateDocument.SetProperties( _
    ''        v_sEntityName:=SIREntityNameParty, _
    ''        v_lEntityID:=frmInterface.PartyCnt)
    '
    '    ' Call Start method on Interface class
    '    m_lReturn& = oCreateDocument.Start()
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        LogMessage _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Failed to process 'iPMBCreateDocument.Interface'.", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="CallLetter", _
    ''            vErrNo:=Err.Number, _
    ''            vErrDesc:=Err.Description
    '        CallLetter = PMFalse
    '        m_lReturn& = oCreateDocument.Terminate()
    '        Set oCreateDocument = Nothing
    '        Exit Function
    '    End If
    '
    '    ' Destroy CreateDocument object
    '    m_lReturn& = oCreateDocument.Terminate()
    '    Set oCreateDocument = Nothing
    '
    '    Exit Function
    '
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
    '        m_lReturn& = oCreateDocument.Terminate()
    '        Set oCreateDocument = Nothing
    '    End If
    '
    '    Exit Function
    '


    ' ***************************************************************** '
    ' Name: CallLetter
    '
    ' Description:  Tomo02052000
    '               Use the new
    ' ***************************************************************** '
    Public Function CallLetter(ByVal v_lPartyCnt As Integer, Optional ByRef v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_lClaimCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lDocumentTemplateId, lDocumentTypeId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetTheTemplate(r_lDocumentTemplateId:=lDocumentTemplateId, r_lDocumentTypeId:=lDocumentTypeId)

            If lDocumentTemplateId = 0 Then
                Return result
            End If

            m_lReturn = UseDocTemplate(v_lDocumentTemplateId:=lDocumentTemplateId, v_lDocumentTypeId:=lDocumentTypeId, v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lClaimCnt:=v_lClaimCnt)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Create Document.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallLetter", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function CallCashDeposit(ByVal v_lPartyCnt As Integer, ByVal v_sPartyCode As String, ByVal v_sPartyName As String) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "CallCashDeposit"
        Dim oCashDeposit As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oCashDeposit As Object = Nothing

        m_lReturn = g_oObjectManager.GetInstance(temp_oCashDeposit, sClassName:="iSIRCashDeposit.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)

        oCashDeposit = temp_oCashDeposit

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


    Private Function GetTheTemplate(ByRef r_lDocumentTemplateId As Integer, ByRef r_lDocumentTypeId As Integer) As Integer

        'Dim o As iPMBFindDocTemplate.Interface
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim o As Object = Nothing

        m_lReturn = iPMBListEvents.g_oObjectManager.GetInstance(oObject:=o, sClassName:="iPMBFindDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)

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

    Private Function UseDocTemplate(ByVal v_lDocumentTemplateId As Integer, ByVal v_lDocumentTypeId As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim temp_oObject As Object = Nothing
        Dim oObject As Object

        result = gPMConstants.PMEReturnCode.PMTrue


        'm_lReturn = g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        m_lReturn = iPMBListEvents.g_oObjectManager.GetInstance(temp_oObject, sClassName:="iPMBDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
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


        m_lReturn = oObject.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            oObject = Nothing
            Return result
        End If


		oObject.Dispose()
        Return result
    End Function
End Module