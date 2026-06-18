Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
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
	
	
	 ' {* USER DEFINED CODE (Begin) *}
	 ' {* USER DEFINED CODE (End) *}
	
	
	 ' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	Private m_oKeyword As Object
	
	 ' Stores the return value for the a
	 ' function call.
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	 ' Whether Cancel or OK has been clicked
	Public Canceled As Boolean
	
	
	Private Sub cmdAttach_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAttach.Click
		
		For lLoop1 As Integer = 1 To lvwKeywords.Items.Count
			If lvwKeywords.Items.Item(lLoop1 - 1).Selected Then
				ListViewHelper.GetListViewSubItem(lvwKeywords.Items.Item(lLoop1 - 1), 1).Text = "Yes"
			End If
		Next lLoop1
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Canceled = True
		
		 ' hide the form
		Me.Hide()
		
	End Sub
	
	Private Sub cmdDetach_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetach.Click
		
		
		 ' If its tagged as "yes" then untag it
		For lLoop1 As Integer = 1 To lvwKeywords.Items.Count
			If lvwKeywords.Items.Item(lLoop1 - 1).Selected Then
				If ListViewHelper.GetListViewSubItem(lvwKeywords.Items.Item(lLoop1 - 1), 1).Text = "Yes" Then
					ListViewHelper.GetListViewSubItem(lvwKeywords.Items.Item(lLoop1 - 1), 1).Text = ""
				End If
			End If
		Next lLoop1
		
	End Sub
	
	Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click
		
		Dim vSaveState As Object
		
		 ' Save the state of the "Yes"s otherwise they will be lost
		If Not bAdminMode Then

			m_lReturn = CType(SaveAttachState(vSave:=vSaveState), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
		End If
		
		 ' Get the new keyword
		m_lReturn = CType(NewKeyword(), gPMConstants.PMEReturnCode)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If
		
		 ' Restore the "Yes"s
		If Not bAdminMode Then

			m_lReturn = CType(RestoreAttachState(vRestore:=vSaveState), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
		End If
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Canceled = False
		
		If bHasDocNum Then
			
			 ' if theres a document number, then attach the keywords
			 ' otherwise an array will just be returned from the class
			If lDocumentNum > 0 Then
				
				 ' Set the mouse pointer to busy, as we could be doing
				 ' loads of processing
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                 ' attach those keywords
                m_lReturn = CType(AttachKeywords(), gPMConstants.PMEReturnCode)

                 ' back to normal
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
            End If

        End If

        Me.Hide()

    End Sub

    Private Sub cmdRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemove.Click

        m_lReturn = CType(RemoveKeyword(), gPMConstants.PMEReturnCode)

    End Sub

    Private Function RemoveKeyword() As Integer

        Dim result As Integer = 0
        Dim lKeyNum As Integer
        Dim sKey As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            For lLoop1 As Integer = 0 To lvwKeywords.Items.Count - 1
                If lvwKeywords.Items.Item(lLoop1).Selected Then
                    sKey = lvwKeywords.Items.Item(lLoop1).Name
                    sKey = sKey.Substring(sKey.Length - (sKey.Length - 1))
                    lKeyNum = CInt(sKey)

                    m_lReturn = m_oKeyword.DeleteKeyword(lKeywordID:=lKeyNum)
                End If
            Next lLoop1

             ' Update the view
            m_lReturn = CType(UpdateView(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not bAdminMode Then
                If bHasDocNum Then
                     ' Mark attached keywords
                    m_lReturn = CType(GetAttachedKeywords(lDocNum:=lDocumentNum), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

             ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to remove keyword.", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveKeyword", excep:=excep)

            Return result

        End Try
    End Function

    Private Function NewKeyword() As Integer

        Dim result As Integer = 0
        Dim sKeyword, sCheckString, sMsg As String


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

             'Get the document name
            sKeyword = Interaction.InputBox(NEWKEYWORD_PROMPT, NEWKEYWORD_TITLE)

            If sKeyword = "" Then
                Return result
            End If

             'Make sure it isn't too long
            While sKeyword.Length > DOCKeyword_Max

                sKeyword = Interaction.InputBox(NEWKEYWORD_PROMPT & Strings.Chr(10).ToString() & Strings.Chr(10).ToString() & _
                           "Maximum " & CStr(DOCKeyword_Max) & " characters.", NEWKEYWORD_TITLE, sKeyword.Substring(0, DOCKeyword_Max))

                If sKeyword = "" Then
                    Return result
                End If

            End While

            sKeyword = sKeyword.Trim()

             ' Check its not already in the list
            For iLoop1 As Integer = 1 To lvwKeywords.Items.Count
                sCheckString = lvwKeywords.Items.Item(iLoop1 - 1).Text
                If sCheckString.Trim() = sKeyword Then
                    sMsg = "The keyword '" & sKeyword & "' is already on the list."
                    MessageBox.Show(sMsg, "New Keyword", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return result
                End If
            Next iLoop1


             ' Add the keyword

            m_lReturn = m_oKeyword.AddKeyword(sKeyword:=sKeyword)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

             ' Update the view
            m_lReturn = CType(UpdateView(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If bHasDocNum Then
                 ' Mark attached keywords
                m_lReturn = CType(GetAttachedKeywords(lDocNum:=lDocumentNum), gPMConstants.PMEReturnCode)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

             ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add new keyword.", vApp:=ACApp, vClass:=ACClass, vMethod:="NewKeyword", excep:=excep)

            Return result
        End Try
    End Function

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

         ' Forms initialise event.

        Try

             ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

             ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

             ' Get an instance of the business object via
             ' the public object manager.
            Dim temp_m_oKeyword As Object
            m_lReturn = CType(g_oObjectManager.GetInstance(temp_m_oKeyword, "bDOCKeywordAdmin.Form", vInstanceManager:="ClientManager"), gPMConstants.PMEReturnCode)
            m_oKeyword = temp_m_oKeyword

             ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                 ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                 ' Display error stating the problem.

                 ' Get description from the resource file.
                 'sTitle$ = GetResData( _
                 'iLangID:=g_iLanguageID%, _
                 'lID:=ACBusinessFailTitle, _
                 'iDataType:=PMResString)

                 'sMessage$ = GetResData( _
                 'iLangID:=g_iLanguageID%, _
                 'lID:=ACBusinessFail, _
                 'iDataType:=PMResString)

                 ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

             ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            m_lReturn = CType(UpdateView(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

             '    ' Mark attached keywords
            If Not bAdminMode Then
                If bHasDocNum Then
                    m_lReturn = CType(GetAttachedKeywords(lDocNum:=lDocumentNum), gPMConstants.PMEReturnCode)
                End If
            End If

        Catch excep As System.Exception



             ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

             ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


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

            m_lReturn = CType(UpdateView(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

             ' Mark attached keywords
            If Not bAdminMode Then
                If bHasDocNum Then
                    m_lReturn = CType(GetAttachedKeywords(lDocNum:=lDocumentNum), gPMConstants.PMEReturnCode)
                End If
            End If

        Catch excep As System.Exception



             ' Error Section

             ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwKeywords_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwKeywords.DoubleClick

         ' Toggle!
         ' only in attach mode!
        If Not bAdminMode Then
            If ListViewHelper.GetListViewSubItem(lvwKeywords.FocusedItem, 1).Text = "Yes" Then
                ListViewHelper.GetListViewSubItem(lvwKeywords.FocusedItem, 1).Text = ""
            Else
                ListViewHelper.GetListViewSubItem(lvwKeywords.FocusedItem, 1).Text = "Yes"
            End If
        End If

    End Sub


    Public Sub mnuFileNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileNew.Click

        cmdNew_Click(cmdNew, New EventArgs())

    End Sub

    Public Sub mnuFileRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileRemove.Click

        cmdRemove_Click(cmdRemove, New EventArgs())

    End Sub

    Private Function UpdateView() As Integer

        Dim result As Integer = 0
        Dim vKeywords As Object
        Dim sKey, sText As String

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

             ' GetKeywordList

            m_lReturn = m_oKeyword.GetKeywordList(vKeywords:=vKeywords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

             ' Clear the view
            lvwKeywords.Items.Clear()

             ' Update view
            If Information.IsArray(vKeywords) Then


                For lLoop1 As Integer = 0 To vKeywords.GetUpperBound(1)
                     ' Only display it if its not been deleted

                    If CStr(vKeywords(2, lLoop1)) = "N" Then

                        sKey = "K" & CStr(vKeywords(0, lLoop1))

                        sText = CStr(vKeywords(1, lLoop1))
                        lvwKeywords.Items.Add(sKey, sText, "")
                    End If
                Next lLoop1

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

             ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to obtain update the keyword list.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateView", excep:=excep)

            Return result
        End Try
    End Function

    Public Sub mnuViewRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewRefresh.Click

        m_lReturn = CType(UpdateView(), gPMConstants.PMEReturnCode)

    End Sub

     'UPGRADE_NOTE: (7001) The following declaration (GetSelectedKeywords) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
     'Private Function GetSelectedKeywords(ByRef vKeywordID() As Object) As gPMConstants.PMEReturnCode
     '
     'Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
     'Dim lCount, lIDCount As Integer
     'Dim sKey As String = ""
     '
     'Dim vIDs As Object
     '
     'Try 
     '
     'result = gPMConstants.PMEReturnCode.PMTrue
     '
     'lIDCount = 0
     '
     ''ReDim vIDs(0)
     '
     'lCount = lvwKeywords.Items.Count
     '
     'For 'lLoop1 As Integer = 0 To lCount - 1
     '
     'If ListViewHelper.GetListViewSubItem(lvwKeywords.Items.Item(lLoop1), 1).Text = "Yes" Then
     '
     ''ReDim Preserve vIDs(lIDCount)
     '
     'sKey = lvwKeywords.Items.Item(lLoop1).Name
     'sKey = sKey.Substring(sKey.Length - (sKey.Length - 1))

    'vIDs.SetValue(sKey, lIDCount)     
     'lIDCount += 1
     '
     'End If
     '
     'Next lLoop1
     '

     'vKeywordID = vIDs
     '
     'Return result
     '
     'Catch excep As System.Exception
     '
     '
     '
     'result = gPMConstants.PMEReturnCode.PMFalse
     '
     ' Log Error.
     'LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to obtain selected keywords.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSelectedKeywords", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
     '
     'Return result
     '
     'End Try
     'End Function




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
            iPMFunc.CenterForm(Me)

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
             '        iPMFunc.LogMessage _
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

    Private Function GetAttachedKeywords(ByRef lDocNum As Integer) As Integer

        Dim result As Integer = 0
        Dim lKey As Integer
        Dim sKey As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vKeywordIDs As Object

        Try

             ' Get from database

            m_lReturn = m_oKeyword.GetDocKeywordIDs(lDocNum:=lDocNum, vKeywordIDs:=vKeywordIDs)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vKeywordIDs) Then
                Return result
            End If

             ' Set

            For lLoop1 As Integer = 0 To vKeywordIDs.GetUpperBound(1)

                For lLoop2 As Integer = 1 To lvwKeywords.Items.Count

                    sKey = lvwKeywords.Items.Item(lLoop2 - 1).Name
                    lKey = CInt(Conversion.Val(sKey.Substring(sKey.Length - (sKey.Length - 1))))


                    If lKey = CDbl(vKeywordIDs(2, lLoop1)) Then
                         ' Set to yes, and tag with the doc_keyword_id
                        ListViewHelper.GetListViewSubItem(lvwKeywords.Items.Item(lLoop2 - 1), 1).Text = "Yes"

                        lvwKeywords.Items.Item(lLoop2 - 1).Tag = CStr(vKeywordIDs(1, lLoop1))
                        Exit For
                    End If

                Next lLoop2

            Next lLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

             ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get attached keywords", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAttachedKeywords", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function AttachKeywords() As Integer

        Dim result As Integer = 0
        Dim bAttached As Boolean
        Dim iKey As Integer
        Dim sKey As String = ""
        Dim lDocKeywordID As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            For lLoop1 As Integer = 1 To lvwKeywords.Items.Count

                 ' is selected?
                If ListViewHelper.GetListViewSubItem(lvwKeywords.Items.Item(lLoop1 - 1), 1).Text = "Yes" Then
                     ' yes then
                    sKey = lvwKeywords.Items.Item(lLoop1 - 1).Name
                    iKey = Conversion.Val(sKey.Substring(sKey.Length - (sKey.Length - 1)))

                    m_lReturn = m_oKeyword.IsAttached(lDocNum:=lDocumentNum, iKeyword:=iKey, bAttached:=bAttached)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Not bAttached Then

                        m_lReturn = m_oKeyword.AttachKeyword(lDocNum:=lDocumentNum, iKeyword:=iKey)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                Else
                     ' no then
                     ' check if tag is key
                    If Convert.ToString(lvwKeywords.Items.Item(lLoop1 - 1).Tag) <> "" Then
                         ' yes then detach using misc class
                        lDocKeywordID = CInt(Conversion.Val(Convert.ToString(lvwKeywords.Items.Item(lLoop1 - 1).Tag)))

                        m_lReturn = m_oKeyword.DeleteDocKeyword(lDocKeywordID:=lDocKeywordID)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                End If

            Next lLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

             ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to attach keywords.", vApp:=ACApp, vClass:=ACClass, vMethod:="AttachKeywords", excep:=excep)

            Return result

        End Try
    End Function

    Private Function SaveAttachState(ByRef vSave() As Object) As Integer

         ' Saves a list of keyword ID's that have YES as attached

        Dim result As Integer = 0
        Dim lCurrent As Integer
        Dim vSaveTemp As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            lCurrent = 0

            ReDim vSaveTemp(0)

            For lLoop1 As Integer = 1 To lvwKeywords.Items.Count

                If ListViewHelper.GetListViewSubItem(lvwKeywords.Items.Item(lLoop1 - 1), 1).Text = "Yes" Then
                    ReDim Preserve vSaveTemp(lCurrent)

                    vSaveTemp(lCurrent) = lvwKeywords.Items.Item(lLoop1 - 1).Name
                    lCurrent += 1
                End If

            Next lLoop1


            vSave = vSaveTemp

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

             ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save state of attaches.", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveAttachState", excep:=excep)

            Return result

        End Try
    End Function

    Private Function RestoreAttachState(ByRef vRestore() As Object) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            For lLoop1 As Integer = 1 To lvwKeywords.Items.Count
                ListViewHelper.GetListViewSubItem(lvwKeywords.Items.Item(lLoop1 - 1), 1).Text = ""
            Next lLoop1


            If vRestore Is Nothing Then
                Return result
            End If

            For lLoop1 As Integer = 0 To vRestore.GetUpperBound(0)

                For lLoop2 As Integer = 1 To lvwKeywords.Items.Count


                    If CStr(vRestore(lLoop1)) = lvwKeywords.Items.Item(lLoop2 - 1).Name Then
                        ListViewHelper.GetListViewSubItem(lvwKeywords.Items.Item(lLoop2 - 1), 1).Text = "Yes"
                        Exit For
                    End If

                Next lLoop2

            Next lLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

             ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to restore state of attaches.", vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAttachState", excep:=excep)

            Return result

        End Try
    End Function
End Class