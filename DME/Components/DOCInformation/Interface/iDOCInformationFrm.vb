Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: {17/2/98}
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	' PUBLIC Data Members (Begin)
	
	' PUBLIC Data Members (End)
	
	' PRIVATE Data Members (Begin)
	
	
	'***Insert Form Constants***
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Public vDocNamesArray( ,  ) As Object
	Public lDocNum As Integer
	
	Public sFolderName As String = ""
	Public iAccessLevel As Integer
	Public dCreateDate As Date
	Public sDocName As String = ""
	Public sScanUser As String = ""
	Public dExpiryDate As Date
	Public dDocDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	
	Private m_sOriginalDocName As String = ""
	
	' {* USER DEFINED CODE (End) *}
	
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
	
	
	' PUBLIC Property Procedures (Begin)
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Standard Property.
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PUBLIC Property Procedures (End)
	' PRIVATE Property Procedures (Begin)
	
	'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub Status(ByVal Value As Integer)
		'
		' Standard Property.
		'
		' Set the interface exit status.
		'm_lStatus = Value
		'
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	' PRIVATE Property Procedures (End)
	
	
	' PUBLIC Methods (Begin)
	
	' ***************************************************************** '
	' Name: GetBusiness
	'
	' Description: Retrieves the details from the business object.
	'
	' ***************************************************************** '
	Public Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Try 
			
			
			' Get the details from the business object.
			
			' {* USER DEFINED CODE (Begin) *}
			
			'    m_lreturn& = m_oBusiness.GetDetails()
			
			' {* USER DEFINED CODE (End) *}
			
			' Check for errors
			'    If (m_lreturn& <> PMTrue) Then
			'        ' Failed to get details.
			'        GetBusiness = PMFalse
			'
			'        ' Log Error.
			'        LogMessage _
			''            iType:=PMLogError, _
			''            sMsg:="Failed to get details from the business object", _
			''            vApp:=ACApp, _
			''            vClass:=ACClass, _
			''            vMethod:="GetBusiness"
			'
			'        Exit Function
			'    End If
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            'CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try


            ' Display all language specific captions.

            '    Me.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACInterfaceTitle, _
            ''        iDataType:=PMResString)
            '
            '    ' Check for an error.
            '    If (Me.Caption = "") Then
            '        ' Failed to get data from the resource file.
            '        DisplayCaptions = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
            ''            "Please check the file exists and the correct captions are available", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="DisplayCaptions"
            '
            '        Exit Function
            '    End If

            '    cmdOK.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACOKButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdCancel.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACCancelButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdHelp.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACHelpButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdNavigate.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACNavigateButton, _
            ''        iDataType:=PMResString)
            '
            '    tabMainTab.TabCaption(0) = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACTabTitle1, _
            ''        iDataType:=PMResString)

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            '***Insert GetRes Calls***

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)


    ' PRIVATE Events (Begin)

    'Private Sub Form_Initialize()
    '
    'Dim sMessage As String
    'Dim sTitle As String
    'Dim iLoop1 As Integer
    '
    '    ' Forms initialise event.
    '
    '    On Error GoTo Err_FormInitialise
    '
    '    m_lreturn = m_oDOCInformation.GetDocNames(vDocNamesArray:=vDocNamesArray)
    '
    '    If (m_lreturn <> PMTrue) Then
    '        Exit Sub
    '    End If
    '
    '    For iLoop1 = 0 To UBound(vDocNamesArray, 2)
    '        CmbDocName.AddItem vDocNamesArray(0, iLoop1)
    '    Next iLoop1
    '
    '    TxtAccLevel.Locked = True
    '    TxtCreateDate.Locked = True
    '    TxtCreateUser.Locked = True
    '    TxtDocDate.Locked = True
    '    TxtDocName.Locked = True
    '    TxtExpDate.Locked = True
    '
    '    ' Set the mouse pointer to busy.
    '    iPMFunc.SetMousePointer PMMouseBusy
    '
    '    ' Initialise the error number value.
    '    m_lErrorNumber& = PMTrue
    '
    '    ' Get an instance of the business object via
    '    ' the public object manager.
    '    m_lreturn& = g_oObjectManager.GetInstance( _
    ''        oObject:=m_oBusiness, _
    ''        sClassName:="bDOCFind.Form", _
    ''        vInstanceManager:="ClientManager")
    '
    '    ' Check for errors.
    '    If (m_lreturn& <> PMTrue) Then
    '        ' Failed to get an instance of the business object.
    '        m_lErrorNumber& = PMFalse
    '
    '        ' Display error stating the problem.
    '
    '        ' Get description from the resource file.
    '        'sTitle$ = GetResData( _
    ''            iLangID:=g_iLanguageID%, _
    ''            lID:=ACBusinessFailTitle, _
    ''            iDataType:=PMResString)
    '
    '        'sMessage$ = GetResData( _
    ''            iLangID:=g_iLanguageID%, _
    ''            lID:=ACBusinessFail, _
    ''            iDataType:=PMResString)
    '
    '        ' Display message.
    '        MsgBox sMessage$, vbCritical, sTitle$
    '
    '        Exit Sub
    '    End If
    '
    '
    '    ' Set the cancelled property to true. This is done
    '    ' so that any interface termination will be noted
    '    ' as cancelled except in the event of accepting
    '    ' the interface.
    '    'todo
    '    'Cancelled = True
    '
    '    ' Set the mouse pointer to normal.
    '    iPMFunc.SetMousePointer PMMouseNormal
    '
    '    Exit Sub
    '
    'Err_FormInitialise:
    '
    '    ' Error Section
    '
    '    m_lErrorNumber& = PMError
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to initialise the interface object", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="Form_Initialise", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Sub
    '
    'End Sub

    'UPGRADE_WARNING: (2074) ComboBox event CmbDocName.Change was upgraded to CmbDocName.TextChanged which has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2074.aspx
    Private isInitializingComponent As Boolean
    Private Sub CmbDocName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmbDocName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        If Strings.Len(CmbDocName.Text) > DOCDoc_Name_Max Then
            CmbDocName.Text = CmbDocName.Text.Substring(0, DOCDoc_Name_Max)
            CmbDocName.SelectionStart = DOCDoc_Name_Max
        End If

    End Sub

    Private Sub cmdAnnotations_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAnnotations.Click

        Dim sAnnText, sDocName As String

        Try
            Me.SendToBack()
            'Capture annotation
            sDocName = CmbDocName.Text.Trim()

            sAnnText = Interaction.InputBox("Please Enter Annotation", "Current Document - '" & _
                       sDocName & "'")

            If sAnnText = "" Then
                Exit Sub
            End If

            'Make sure it isn't too long
            While sAnnText.Length > 50

                sAnnText = Interaction.InputBox("Please Enter Annotation" & Strings.Chr(10).ToString() & Strings.Chr(10).ToString() & _
                           "Maximum 50 characters", "Current Document - '" & sDocName & _
                           "'", sAnnText.Substring(0, 50))

                If sAnnText = "" Then
                    Exit Sub
                End If

            End While

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'UPGRADE_TODO: (1067) Member AddAnnotation is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            m_lReturn = m_oBusiness.AddAnnotation(lDocNum:=g_lDocNum, sAnnText:=sAnnText, sUsername:=g_sUserName)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to attach annotation to document.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAnnotations_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub CmdKeywords_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdKeywords.Click

        Dim vKeywordID, oKeywordAdmin As Object


        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            Me.SendToBack()
            'get the keyword object and initialise it
            'oKeywordAdmin = System.Runtime.InteropServices.Marshal.GetActiveObject("iDOCKeywordAdmin.Interface")
            oKeywordAdmin = New iDOCKeywordAdmin.Interface_Renamed()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'UPGRADE_TODO: (1067) Member Initialise is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            'm_lReturn = CType(oKeywordAdmin, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            m_lReturn = oKeywordAdmin.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise iDOCKeywordAdmin.Interface class", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdKeywords_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            'call the attach method for this document
            'UPGRADE_TODO: (1067) Member AttachKeywords is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            m_lReturn = oKeywordAdmin.AttachKeywords(lDocNum:=g_lDocNum, vKeywordID:=vKeywordID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'notify user, but refresh list so they know which failed
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to attach all keywords.", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdKeywords_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End If

            'UPGRADE_TODO: (1067) Member Terminate is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            oKeywordAdmin.Dispose()

            oKeywordAdmin = Nothing

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="CmdKeywords_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub CmdPassword_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdPassword.Click

        Dim sPassword As String = ""
        Dim lNodeNum As Integer
        Dim iNodeLevel As Integer

        Dim oPassword As iDOCPassword.Interface_Renamed

        Try

            Me.SendToBack()
            ' Create an instance of the password object
            oPassword = New iDOCPassword.Interface_Renamed()

            ' Initialise the object
            m_lReturn = oPassword.Initialise(False, g_sUserName, g_sPassword, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=1, sCallingAppName:=ACClass)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Get the docnumber
            lNodeNum = g_lDocNum
            iNodeLevel = DOCNode_Document

            ' add the password
            m_lReturn = oPassword.AddPassword(lNodeNum:=lNodeNum, iNodeLevel:=iNodeLevel, sEncryptedPassword:=sPassword)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' terminate the object
            oPassword.Dispose()
            ' destroy it completely
            oPassword = Nothing

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add password.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPassword_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            Dim bEnabled As Boolean = SetWinPos(Me.Handle.ToInt32())

        End If
    End Sub

    'UPGRADE_WARNING: (2080) Form_Load event was upgraded to Form_Load event and has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2080.aspx
    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            'iPMFunc.SetMousePointer PMMouseBusy

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.
            'If (UnloadMode <> vbFormCode) Then
            ' Process the next set of actions depending
            ' upon the interface task etc.
            'm_lreturn& = ProcessCommand()


            ' Check the return value.
            'If (m_lreturn& <> PMTrue) Then
            ' Do not procced with the interface termination.
            ' Cancel = 1
            'Else
            ' Everything OK, so we can hide the interface.
            ' Me.Hide

            ' Set the mouse pointer to normal.
            'iPMFunc.SetMousePointer PMMouseNormal

            'Exit Sub
            'End If

            ' End If


            ' Terminate the business object
            'UPGRADE_TODO: (1067) Member Terminate is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            m_oBusiness.Dispose()
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub CmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'update the document name if it has changed
            If m_sOriginalDocName.Trim() <> CmbDocName.Text.Trim() Then

                'UPGRADE_TODO: (1067) Member RenameDoc is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                m_lReturn = m_oBusiness.RenameDoc(lDocNum:=g_lDocNum, sNewName:=CmbDocName.Text.Trim())

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to rename the document.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                    Exit Sub
                End If

                'Store the new name so it can be returned to calling app
                g_sNewName = CmbDocName.Text.Trim()

            End If

            ' Everything OK, so we can hide the interface.
            Me.Hide()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub CmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Everything OK, so we can hide the interface.
            Me.Hide()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub



    Private Sub Form_Initialize_Renamed()
        Dim bExternal As Boolean

        Try

            'get instance of business
            Dim temp_m_oBusiness As Object
            m_lReturn = CType(g_oObjectManager.GetInstance(temp_m_oBusiness, "bDOCInformation.Form", vInstanceManager:="ClientManager"), gPMConstants.PMEReturnCode)
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'call GetDocInfo in business
            'UPGRADE_TODO: (1067) Member GetDocInfo is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            m_lReturn = m_oBusiness.GetDocInfo(lDocNum:=g_lDocNum, sFolderName:=sFolderName, iAccessLevel:=iAccessLevel, bExternal:=bExternal, dCreateDate:=dCreateDate, sDocName:=sDocName, sScanUser:=sScanUser, dExpiryDate:=dExpiryDate, dDocDate:=dDocDate)

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            'Call GetDocNames in business
            'UPGRADE_TODO: (1067) Member GetDocNames is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            m_lReturn = m_oBusiness.GetDocNames(vDocNamesArray:=vDocNamesArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Loop to get all document names
            If Information.IsArray(vDocNamesArray) Then
                For iLoop1 As Integer = vDocNamesArray.GetLowerBound(1) To vDocNamesArray.GetUpperBound(1)
                    CmbDocName.Items.Add(CStr(vDocNamesArray(0, iLoop1)))
                Next iLoop1
            End If

            'format dates and show in place of text boxes
            TxtCreateDate.Text = DateTime.Parse(dCreateDate).ToString("D") & "   " & _
                                 StringsHelper.Format(dCreateDate, "h:mm AMPM")

            TxtExpDate.Text = DateTime.Parse(dExpiryDate).ToString("D")
            TxtDocDate.Text = DateTime.Parse(dDocDate).ToString("D")


            'show returned fields in text/combo box
            CmbDocName.Text = sDocName
            TxtAccLevel.Text = CStr(iAccessLevel)

            If bExternal Then
                optExtYes.Checked = True
                optExtNo.Checked = False
            Else
                optExtYes.Checked = False
                optExtNo.Checked = True
            End If

            TxtCreateUser.Text = sScanUser
            TxtFolderName.Text = sFolderName

            'store doc name in case it is changed
            m_sOriginalDocName = sDocName

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub
End Class