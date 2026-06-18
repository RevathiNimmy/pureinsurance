Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Public Module PMBToolbarFunc

    ' ***************************************************************** '
    ' Functions called by Toolbar buttons and Menus.
    '
    ' Edit History  :
    ' TF111298      : Created
    ' RAM20020820   : Added Toolbar Button Keys
    ' ***************************************************************** '

    ' Constant for the methods to identify which class this is.
    Private Const ACClass As String = "SIRToolbarFunc"

    'Added by Deepak Sharma on 4/21/2010 10:13:47 AM refer developer guide no. 

    Public g_oObjectManager As Object

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

    Public Const ACIButtonFinancialKey As String = "FINANCIAL"
    Public Const ACIButtonPolicyKey As String = "POLICY"
    Public Const ACIButtonClaimKey As String = "CLAIM"
    Public Const ACIButtonCommissionKey As String = "COMMISSION"
    Public Const ACIButtonNotesKey As String = "NOTE"
    Public Const ACIButtonLetterKey As String = "LETTER"
    Public Const ACIButtonEmailKey As String = "EMAIL"
    Public Const ACIButtonInternetKey As String = "WEB"

    Private m_lReturn As Integer

    ' ***************************************************************** '
    ' Name: ProcessToolbar
    '
    ' Description   : TF291298 - Control calls to relevant components
    ' Edit History  :
    '
    ' RAM20020820   : 1. Added an optional parameter to pass in party_cnt
    '                 2. Changed the Button Index Parameter to Button Key
    '                 3. Changed the function to check for the Button Key, Rather than
    '                    the button index. (Being this iPMBToolbarFunc.bas is shared
    '                    between projects, the key index may vary, not the KEY)
    ' ***************************************************************** '

    'Public Function ProcessToolbar(ByVal v_sButtonKey As String, Optional ByVal v_lPartyCnt As Byte = 0) As Integer
    Public Function ProcessToolbar(ByVal v_sButtonKey As String, Optional ByVal v_lPartyCnt As Object = 0, Optional ByVal v_ProductName As String = "SA") As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case v_sButtonKey.Trim().ToUpper()
                Case ACIButtonFinancialKey
                    ' Call Find Account component, passing Party details
                    m_lReturn = CallFindAccount()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallFindAccount().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                Case ACIButtonPolicyKey

                    ' Call Find Policy component, passing Party details
                    m_lReturn = CallFindPolicy()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallFindPolicy().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                Case ACIButtonClaimKey
                    ' Call Find Claim component, passing Party details
                    m_lReturn = CallFindClaim()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallFindClaim().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                Case ACIButtonNotesKey
                    ' Call Find Account component, passing Party details
                    m_lReturn = CallNotes(v_lPartyCnt:=v_lPartyCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallNotes().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                Case ACIButtonLetterKey
                    ' Call Find Account component, passing Party details
                    m_lReturn = CallLetter(v_lPartyCnt:=v_lPartyCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallLetter().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                Case ACIButtonEmailKey
                    m_lReturn = CallEMail(v_lPartyCnt:=v_lPartyCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process CallEMail().", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessToolbar", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                Case CStr(ACIButtonInternet)

                    '            Dim frmBrowser As New frmBrowser
                    '            With frmBrowser
                    '                .StartingAddress = "http://www.policymaster.com/"
                    '                .Show vbModal
                    '            End With
                    '            Unload frmBrowser
                    '            Set frmBrowser = Nothing
                    '            Set oBrowser = CreateObject("PMBWebBRowser.Browser")

                Case Else
                    MessageBox.Show("Component Not Yet Available", v_ProductName)
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
    Public Function CallFindPolicy() As Integer
        Dim result As Integer = 0
        Dim oFindPolicy As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create FindPolicy object
            Dim temp_oFindPolicy As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindPolicy, sClassName:="iPMBFindInsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindPolicy = temp_oFindPolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iSIRFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Default Insurance Holder to Party Shortname

            'deepak_todo : to be handeled later
            'oFindPolicy.ShortName = frmInterface.txtIDReference.Text.Trim()

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
        Dim oFindClaim As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create FindClaim object
            Dim temp_oFindClaim As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindClaim, sClassName:="iSIRFindClaim.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindClaim = temp_oFindClaim

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iSIRFindClaim.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Default Insurance Holder to Party Shortname

            'deepak_todo : to be checked later
            'oFindClaim.PolicyHolder = frmInterface.txtIDReference.Text.Trim()

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
        Dim oFindAccount As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create FindPolicy object
            Dim temp_oFindAccount As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindAccount, sClassName:="iACTFindAccount.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindAccount = temp_oFindAccount

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iACTFindAccount.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindAccount", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Default Insurance Holder to Party Shortname

            'deepak_todo : to be checked later
            'oFindAccount.ShortCode = frmInterface.ShortName

            ' Call Start method on Interface class

            m_lReturn = oFindAccount.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start 'iACTFindAccount.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallFindAccount", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
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
    ' Name: CallNotes
    '
    ' Description:  TF031298
    '               Call FreeformText component, passing Party details
    ' Edit History  :
    ' RAM20020820   : Changed the name of the Free Form Object Name
    '                   from iSIRFreeformText.Interface To iPMBFreeformText.Interface
    ' ***************************************************************** '
    Public Function CallNotes(ByVal v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oFreeformText As Object
        Dim sFreeformTextObjectName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sFreeformTextObjectName = "iPMBFreeFormText.Interface_Renamed"

            ' Create FreeformText object
            m_lReturn = g_oObjectManager.GetInstance(oObject:=oFreeformText, sClassName:=sFreeformTextObjectName, vInstanceManager:=gPMConstants.PMGetLocalInterface)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iSIRFreeformText.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallNotes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Pass key data to FreeformText component
            'TF021203 - PN7730 - Public notes only
            'Party Bank Details
            If v_lPartyCnt > 0 Then
                With oFreeformText

                    .EntityName = gSIRLibrary.SIREntityNameParty

                    .KeyFieldValue = v_lPartyCnt

                    .TextType = "Public"

                    .PartyCnt = v_lPartyCnt
                End With
            Else
                With oFreeformText

                    .EntityName = gSIRLibrary.SIREntityNameParty

                    'deepak_todo: to be handled later
                    '.KeyFieldValue = frmInterface.PartyCnt

                    .TextType = "Public"

                    'deepak_todo: to be handled later
                    '.PartyCnt = frmInterface.PartyCnt
                End With
            End If

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

    'sj 22/08/2002 - start
    '' ***************************************************************** '
    '' Name: CallLetter
    ''
    '' Description:  TF031298
    ''               Call CreateDocument component, passing Party details
    '' Edit History  :
    '' RAM20020820   : Changed the name of the Free Form Object Name
    ''                   from iSIRCreateDocument.Interface To iPMBCreateDocument.Interface
    '' ***************************************************************** '
    'Public Function CallLetter() As Long
    '
    '    Dim oCreateDocument As Object
    '    Dim sCreateDocumentObjectName As String
    '
    '    On Error GoTo Err_CallLetter
    '
    '    CallLetter = PMTrue
    '
    '    sCreateDocumentObjectName = "iPMBCreateDocument.Interface"
    '
    '    ' Create CreateDocument object
    '    m_lReturn& = g_oObjectManager.GetInstance( _
    ''        oObject:=oCreateDocument, _
    ''        sClassName:=sCreateDocumentObjectName, _
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
    ''            sMsg:="Failed to start 'iPMBCreateDocument.Interface'.", _
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

    Private Function GetTheTemplate(ByRef r_lDocumentTemplateId As Integer, ByRef r_lDocumentTypeId As Integer) As Integer

        'Dim o As iPMBFindDocTemplate.Interface
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim o As Object = Nothing

        m_lReturn = g_oObjectManager.GetInstance(oObject:=o, sClassName:="iPMBFindDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)


        'm_lReturn = CType(o, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        m_lReturn = o.Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            o = Nothing
            Return result
        End If

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
    'sj 22/08/2002 - end

    Private Function UseDocTemplate(ByVal v_lDocumentTemplateId As Integer, ByVal v_lDocumentTypeId As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimCnt As Integer) As Integer
        Dim result As Integer = 0

        Dim oObject As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oObject As Object = Nothing
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
    ' Name          : CallEMail
    '
    ' Description   :  Function to send an e-mail by using the iPMBEMail.dll
    ' Notes         : This function is based on the client Manager's
    '                   function
    ' Edit History  :
    ' RAM20020820   :  Created
    ' ***************************************************************** '
    Public Function CallEMail(ByVal v_lPartyCnt As Object) As Integer
        Dim result As Integer = 0
        Dim oEmail As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Email object
            Dim temp_oEmail As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oEmail, sClassName:="iPMBEmail.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oEmail = temp_oEmail

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create 'iPMBEmail.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallEMail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            oEmail.PartyCnt = v_lPartyCnt

            ' Call Start method on Interface class

            m_lReturn = oEmail.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start 'iPMBEmail.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="CallEMail", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
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
End Module
