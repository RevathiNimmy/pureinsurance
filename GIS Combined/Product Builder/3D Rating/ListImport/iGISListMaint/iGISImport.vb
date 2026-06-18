Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Public Class FrmImport
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Module Name: frmImport
    '
    ' Date: 28/06/2002
    '
    ' Description:  This will handl the import of the file into windows
    '
    ' Edit History:
    '   28/06/2002 SJP - Tidied up after merge from Carole Nash
    '   11/04/2005 CJB - PN15582 cmdImport_Click change to prevent divide by zero when only 1 item in list!
    ' ***************************************************************** '

    Private vDataLT(,) As Object
    Private bLargeFileFlag As Boolean
    Private m_oFormFields As iPMFormControl.FormFields


    ' ***************************************************************** '
    '
    ' Name: cboListType_Click()
    '
    ' Description:  This will populate the listTypes
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Private Sub cboListType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboListType.SelectedIndexChanged

        Try

            PopListVersions()

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Click", vApp:=ACApp, vClass:=ACClass, vMethod:="cboListtype_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub






    ' ***************************************************************** '
    '
    ' Name: cmdBrowse_Click()
    '
    ' Description:  This will allow the user to browse a file
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Private Sub cmdBrowse_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBrowse.Click

        Try

            CommonDialog1Open.Title = "Import data file"
            CommonDialog1Open.ShowDialog()
            txtfile.Text = CommonDialog1Open.FileName

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to browse", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdbrowse_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: cmdCancel_Click()
    '
    ' Description:  This will unload form
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Try

            Me.Close()

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdcancel_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: cmdImport_Click()
    '
    ' Description:  This will populate the list.
    '
    ' History: 28/06/2002 SJP - tidied up
    '          07/01/2003 APS - Amended to handle large files
    ' ***************************************************************** '
    Private Sub cmdImport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdImport.Click

        Dim vData(,) As Object
        Dim sTable As String = ""
        Dim lVersion As Integer
        Dim vNewItems As Object
        Dim lNewCount As Integer
        Dim sCode As String = ""
        Dim lFileLength As Integer
        Dim strFileName As String = ""
        Dim fso As Object
        Dim file As FileInfo
        Dim lOldVersion As Long
        Dim vFields(,) As Object
        Dim sNewField As String = ""
        Dim frmColumns As New frmColumns
        Dim FrmMap As New FrmMap

        Try

            txtfile.Text = CommonDialog1Open.FileName
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If Trim(txtEffectiveDate.Text) = "" Then
                MsgBox("Please enter a date between January 1, 1900 and December 31, 9998", vbOKOnly + vbCritical, "Effective Date")
                If txtEffectiveDate.Visible Then txtEffectiveDate.Focus()
                Exit Sub
            End If

            If CheckValidDate <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            '    'Validate Combo and Text File inputs
            '    If cboListType.Text = "" Then
            '        MsgBox "A List Type is needed", vbOKOnly, "List Management"
            '        cboListType.SetFocus
            '        Exit Sub
            '    End If
            '
            '    If txtfile.Text = "" Then
            '        MsgBox "An import file is needed", vbOKOnly, "List Management"
            '        txtfile.SetFocus
            '        Exit Sub
            '    End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Reset audit trail flag for new import process
            m_oBusiness.AuditTrailCreated = False

            'work out pm lookup name
            sTable = "UDL_" & cboListType.Text

            'has this list been imported before ?

            Dim lProcessedEntries As Integer
            If m_oBusiness.ListExists(sTable) <> gPMConstants.PMEReturnCode.PMTrue Then
                'if so then show columns form ( which will do the importing stuff too )
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                frmColumns.ShowDialog(Me)
            Else
                'else update existing list

                'get the file length
                fso = New Object()
                lFileLength = GetFileLength(Me.txtfile.Text)
                bLargeFileFlag = (lFileLength > conMaxRecords)

                m_oBusiness.UniqueId = GetUniqueID()

                If bLargeFileFlag Then
                    deleteSplitFiles()
                    splitfile(Me.txtfile.Text, lFileLength)

                    'get the next file
                    strFileName = FileSystem.Dir(sTempFolder & "*.000", FileAttribute.Normal)

                    If m_bIsServer Then
                        'get the first block of data

                        m_lReturn = m_oBusiness.ImportList(sTempFolder & strFileName, vData)

                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Information.IsArray(vData)) Then
                            Throw New System.Exception("1, cmdImport, Failed to import file")
                            Exit Sub
                        End If
                    Else

                        m_lReturn = CType(ImportList(sTempFolder & strFileName, vData), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or (Not Information.IsArray(vData)) Then
                            Throw New System.Exception("1, cmdImport, Failed to import file")
                            Exit Sub
                        End If
                    End If

                    'MKR 11/08/04 PN : 13630
                    'Checking the existing list in DB and if the new list contains extra
                    'fields than add extra columns in DB after user's confirmation...
                    '********************************************************************
                    'Getting the list of columns...

                    m_lReturn = m_oBusiness.GetColumnList(sTable, vFields)

                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Information.IsArray(vFields)) Then
                        Throw New System.Exception("1, cmdImport, Failed to get Column list..")
                        Exit Sub
                    End If

                    'checking for the extra columns in new list


                    If vData.GetUpperBound(0) - 2 > vFields.GetUpperBound(1) - 6 Then



                        If MessageBox.Show("The list you are going to import contains " & _
                                           (vData.GetUpperBound(0) - 2) - (vFields.GetUpperBound(1) - 6) & _
                                           " extra column(s). Do you want to add new column(s) to list?", "List Maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then

                            'asking for the columns names


                            For i As Integer = 1 To ((vData.GetUpperBound(0) - 2) - (vFields.GetUpperBound(1) - 6))

                                Do While (True)
                                    sNewField = Interaction.InputBox("Enter the name for column (" & i & ")...", "New Column")
                                    If sNewField.Trim().Length <= 0 Then
                                        MessageBox.Show("Please enter a valid column name...", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Else
                                        Exit Do
                                    End If
                                Loop

                                If sNewField.Trim().Length > 0 Then
                                    'adding column to the database

                                    m_lReturn = m_oBusiness.AddColumn(sTable, sNewField)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Throw New System.Exception("1, cmdImport, Failed to add Column into list..")
                                        Exit Sub
                                    End If
                                End If
                                sNewField = ""
                            Next

                        End If

                    End If

                    'MKR 11/08/04 PN : 13630 -- ends
                    If optUpdateList.Checked = True Then
                        'Get the Maximum Version from udl_table
                        m_lReturn = m_oBusiness.GetMaxUDLVersion(sTable, lOldVersion)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage( _
                                    iType:=gPMConstants.PMELogLevel.PMLogOnError, _
                                    sMsg:="Failed to get GetMaxUDLVersion", _
                                    vApp:=ACApp, _
                                    vClass:=ACClass, _
                                    vMethod:="cmdImport_Click", _
                                    vErrNo:=Err.Number, _
                                    vErrDesc:=Err.Description)
                        End If
                    End If

                    ProgressBar.Visible = True
                    Do While (strFileName <> "")

                        If m_bIsServer Then

                            m_lReturn = m_oBusiness.ImportList(sTempFolder & strFileName, vData)

                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Information.IsArray(vData)) Then
                                Throw New System.Exception("1, cmdImport, Failed to import file")
                                Exit Sub
                            End If
                        Else

                            m_lReturn = CType(ImportList(sTempFolder & strFileName, vData), gPMConstants.PMEReturnCode)

                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Information.IsArray(vData)) Then
                                Throw New System.Exception("1, cmdImport, Failed to import file")
                                Exit Sub
                            End If
                        End If

                        If Information.IsArray(vData) Then
                            lVersion = CInt(cboListVersion.Text)

                            'resize newones array
                            ReDim vNewItems(UBound(vData), 0)

                            'initialise newones count
                            lNewCount = -1

                            For i As Integer = 0 To vData.GetUpperBound(1)

                                ProgressBar.Value = (100 * lProcessedEntries) \ lFileLength
                                lProcessedEntries += 1

                                'check for existing
                                If Trim(ToSafeString(vData(0, i))) <> "" Then
                                    'IF Update list option is Chosen
                                    If optUpdateList.Checked = True And lVersion > 1 Then

                                        If (m_oBusiness.ListItemExists(sTable, CStr(vData(0, i))) = 1) Then
                                            'If doesn't exist already then add one

                                            m_lReturn = m_oBusiness.UpdateListEntry(sTable, vData, i, CDate(txtEffectiveDate.Text))
                                            'add to usage table and rating structure
                                            m_lReturn = m_oBusiness.addusage(sTable, CStr(vData(0, i)), lVersion, txtEffectiveDate.Text)

                                            'check for errors
                                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                                Throw New System.Exception("1, cmdimport, Failed to add list entry")
                                            End If
                                            m_lReturn = UpdateCaption(sTable, vFields, vData, i, lVersion)
                                        Else
                                            m_lReturn = m_oBusiness.addlistentry(sTable, vData, i, CDate(txtEffectiveDate.Text), ToSafeLong(cboListVersion.Text))

                                            'increment new ones count
                                            lNewCount = lNewCount + 1
                                            'resize new item array
                                            ReDim Preserve vNewItems(UBound(vData), lNewCount)
                                            'transfer details to new item array
                                            For j As Integer = 0 To UBound(vNewItems)
                                                vNewItems(j, lNewCount) = vData(j, i)
                                            Next
                                        End If
                                    End If



                                    If optNewList.Checked = True Or lVersion = 1 Then
                                        'check for existing
                                        If Not (m_oBusiness.ListItemExists(sTable, CStr(vData(0, i)), ToSafeLong(cboListVersion.Text)) = 1) Then
                                            'if not existing, add to the pmlookup,rating and newones array
                                            m_lReturn = m_oBusiness.addlistentry(sTable, vData, i, CDate(txtEffectiveDate.Text), ToSafeLong(cboListVersion.Text))

                                            'check for errors
                                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                                Throw New System.Exception("1, cmdimport, Failed to add item to rating")
                                            End If

                                            m_lReturn = m_oBusiness.addusage(sTable, CStr(vData(0, i)), lVersion, txtEffectiveDate.Text)

                                            'check for errors
                                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                                Throw New System.Exception("1, cmdimport, Failed to addusage  ")
                                            End If


                                        Else

                                            If txtEffectiveDate.Text <> "" Then
                                                m_lReturn = m_oBusiness.UpdateListEntry(sTable, vData, i, CDate(txtEffectiveDate.Text))
                                                'add to usage table and rating structure

                                            End If
                                            m_lReturn = m_oBusiness.addusage(sTable, CStr(vData(0, i)), lVersion, txtEffectiveDate.Text)

                                            'check for errors
                                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                                Throw New System.Exception("1, cmdimport, Failed to add item to rating")
                                            End If

                                            m_lReturn = CType(UpdateCaption(sTable, vFields, vData, i, lVersion), gPMConstants.PMEReturnCode)
                                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                Throw New System.Exception("1, cmdimport, Failed to UpdateCaption ")
                                            End If

                                        End If
                                    End If
                                End If

                                'end cycle
                            Next i

                        End If

                        If System.IO.File.Exists(sTempFolder & strFileName) Then
                            file = New FileInfo(sTempFolder & strFileName)
                            file.Delete()
                        End If

                        strFileName = FileSystem.Dir(sTempFolder & "*.000", FileAttribute.Normal)

                    Loop

                    If optUpdateList.Checked = True And lVersion > 1 Then
                        'Update the previous udl_version by lversion
                        m_lReturn = m_oBusiness.UpdateUDLVersion(sTable, lOldVersion, lVersion)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception("1, cmdimport, Failed to Update UDLVersion")
                        End If
                    End If

                    MsgBox("Imported List Successfully", vbInformation, "List Management")

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    If optUpdateList.Checked = True Then
                        FrmMap.SetListType(cboListType.Text)
                        FrmMap.SetNewItems(vNewItems)
                        FrmMap.ShowDialog()
                    End If




                Else

                    If m_bIsServer Then

                        m_lReturn = m_oBusiness.ImportList(txtfile.Text, vData)

                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Information.IsArray(vData)) Then
                            Throw New System.Exception("1, cmdImport, Failed to import file")
                            Exit Sub
                        End If
                    Else

                        m_lReturn = CType(ImportList(txtfile.Text, vData), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or (Not Information.IsArray(vData)) Then
                            Throw New System.Exception("1, cmdImport, Failed to import file")
                            Exit Sub
                        End If
                    End If

                    'MKR 11/08/04 PN : 13630 -- Start
                    'Checking the existing list in DB and if the new list contains extra
                    'fields than add extra columns in DB after user's confirmation...
                    '********************************************************************
                    'Getting the list of columns...

                    m_lReturn = m_oBusiness.GetColumnList(sTable, vFields)

                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Information.IsArray(vFields)) Then
                        Throw New System.Exception("1, cmdImport, Failed to get Column list..")
                        Exit Sub
                    End If

                    'checking for the extra columns in new list


                    If vData.GetUpperBound(0) - 2 > vFields.GetUpperBound(1) - 6 Then



                        If MessageBox.Show("The list you are going to import contains " & _
                                           (vData.GetUpperBound(0) - 2) - (vFields.GetUpperBound(1) - 6) & _
                                           " extra column(s). Do you want to add new column(s) to list?", "List Maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then

                            'asking for the columns names


                            For i As Integer = 1 To ((vData.GetUpperBound(0) - 2) - (vFields.GetUpperBound(1) - 6))

                                Do While (True)
                                    sNewField = Interaction.InputBox("Enter the name for column (" & i & ")...", "New Column")
                                    If sNewField.Trim().Length <= 0 Then
                                        MessageBox.Show("Please enter a valid column name...", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Else
                                        Exit Do
                                    End If
                                Loop

                                If sNewField.Trim().Length > 0 Then
                                    'adding column to the database

                                    m_lReturn = m_oBusiness.AddColumn(sTable, sNewField)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Throw New System.Exception("1, cmdImport, Failed to add Column into list..")
                                        Exit Sub
                                    End If
                                End If
                                sNewField = ""
                            Next

                        End If

                    End If

                    'MKR 11/08/04 PN : 13630 -- ends

                    'resize newones array

                    ReDim vNewItems(vData.GetUpperBound(0), 0)

                    'initialise newones count
                    lNewCount = -1

                    'choose version number
                    lVersion = CInt(cboListVersion.Text)

                    If optUpdateList.Checked = True Then
                        'Get the Maximum Version from udl_table
                        m_lReturn = m_oBusiness.GetMaxUDLVersion(sTable, lOldVersion)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage( _
                                    iType:=gPMConstants.PMELogLevel.PMLogOnError, _
                                    sMsg:="Failed to get GetMaxUDLVersion", _
                                    vApp:=ACApp, _
                                    vClass:=ACClass, _
                                    vMethod:="cmdImport_Click", _
                                    vErrNo:=Err.Number, _
                                    vErrDesc:=Err.Description)
                        End If
                    End If


                    'cycle through new list values

                    For i As Integer = 0 To vData.GetUpperBound(1)

                        ' Prevent divide by zero when only 1 item in list!  PN15582

                        If vData.GetUpperBound(1) > 0 Then

                            ProgressBar.Value = (100 * i) \ vData.GetUpperBound(1)
                        Else
                            ProgressBar.Value = (100 * i) \ 1
                        End If

                        'check for existing


                        'IF Update list option is Chosen
                        If Trim(ToSafeString(vData(0, i))) <> "" Then
                            If optUpdateList.Checked = True And lVersion > 1 Then
                                If (m_oBusiness.ListItemExists(sTable, CStr(vData(0, i))) = 1) Then

                                    m_lReturn = m_oBusiness.UpdateListEntry(sTable, vData, i, CDate(txtEffectiveDate.Text))
                                    'add to usage table and rating structure
                                    m_lReturn = m_oBusiness.addusage(sTable, CStr(vData(0, i)), lVersion, txtEffectiveDate.Text)
                                    '
                                    '                        'check for errors
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                        Throw New System.Exception("1, cmdimport, Failed to add item to rating")
                                    End If
                                    m_lReturn = UpdateCaption(sTable, vFields, vData, i, lVersion)
                                Else

                                    m_lReturn = m_oBusiness.addlistentry(sTable, vData, i, CDate(txtEffectiveDate.Text), ToSafeLong(cboListVersion.Text))

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                        Throw New System.Exception("1, cmdimport, Failed to addlistentry  ")
                                    End If
                                    'increment new ones count
                                    lNewCount = lNewCount + 1

                                    'resize new item array
                                    ReDim Preserve vNewItems(UBound(vData), lNewCount)

                                    'transfer details to new item array
                                    For j As Integer = 0 To UBound(vNewItems)
                                        vNewItems(j, lNewCount) = vData(j, i)
                                    Next
                                End If
                            End If


                            If optNewList.Checked = True Or lVersion = 1 Then

                                'check for existing
                                If Not (m_oBusiness.ListItemExists(sTable, CStr(vData(0, i)), ToSafeLong(cboListVersion.Text)) = 1) Then

                                    'if not existing, add to the pmlookup,rating and newones array
                                    m_lReturn = m_oBusiness.addlistentry(sTable, vData, i, CDate(txtEffectiveDate.Text), ToSafeLong(cboListVersion.Text))

                                    'check for errors
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                        Throw New System.Exception("1, cmdimport, Failed to add item to rating")
                                    End If

                                    m_lReturn = m_oBusiness.addusage(sTable, CStr(vData(0, i)), lVersion, txtEffectiveDate.Text)

                                    'check for errors
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                        Throw New System.Exception("1, cmdimport, Failed to addusage  ")
                                    End If


                                Else

                                    m_lReturn = m_oBusiness.UpdateListEntry(sTable, vData, i, CDate(txtEffectiveDate.Text))
                                    'add to usage table and rating structure


                                    m_lReturn = m_oBusiness.addusage(sTable, CStr(vData(0, i)), lVersion, txtEffectiveDate.Text)

                                    'check for errors
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                                        Throw New System.Exception("1, cmdimport, Failed to add item to rating")
                                    End If

                                    m_lReturn = CType(UpdateCaption(sTable, vFields, vData, i, lVersion), gPMConstants.PMEReturnCode)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update caption", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdImport_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                                    End If
                                End If
                            End If
                        End If
                        'end cycle
                    Next i

                    If optUpdateList.Checked = True And lVersion > 1 Then
                        'Update the previous udl_version by lversion
                        m_lReturn = m_oBusiness.UpdateUDLVersion(sTable, lOldVersion, lVersion)
                    End If

                    'display mapping form
                    MsgBox("Imported List Successfully", vbInformation, "List Management")

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    If optUpdateList.Checked = True Then
                        FrmMap.SetListType(cboListType.Text)
                        FrmMap.SetNewItems(vNewItems)
                        FrmMap.ShowDialog()
                    End If

                End If

            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Me.Close()

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to import list", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdimport_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
        End Try

    End Sub
    Private Function UpdateCaption(ByRef v_sTable As String, ByRef v_vFields As Object, ByRef v_vData(,) As Object, ByRef lIndex As Integer, ByVal lVersion As Long) As Integer
        Dim result As Integer = 0
        Dim lCaptionID As Integer
        Dim sDescription, sCode As String
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetUDLData(v_sTableName:=v_sTable, v_sCode:=v_vData(0, lIndex), r_vUDLData:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lCaptionID = gPMFunctions.ToSafeLong(vArray(0, 0), 0)

            sDescription = gPMFunctions.ToSafeString(CStr(v_vData(1, lIndex)), "")

            sCode = gPMFunctions.ToSafeString(CStr(v_vData(0, lIndex)), "")


            ' Go to the business and get the caption_id

            m_lReturn = m_oLookupBusiness.GetCaptionID(v_sCaption:=v_vData(1, lIndex), r_lCaptionID:=lCaptionID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oBusiness.UpdateUDLData(v_sTableName:=v_sTable, v_sCode:=v_vData(0, lIndex), v_lCaption_id:=lCaptionID, v_sDescription:=sDescription, v_lVersion:=lversion)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update caption", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCaption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: cmdView_Click()
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click

        Dim vData(,) As Object
        Dim frmView As New frmView

        Try

            'validate input
            If cboListType.Text = "" Then
                MessageBox.Show("A List Type is needed", "List Management", MessageBoxButtons.OK)
                cboListType.Focus()
                Exit Sub
            End If

            If txtfile.Text = "" Then
                MessageBox.Show("An import file is needed", "List Management", MessageBoxButtons.OK)
                txtfile.Focus()
                Exit Sub
            End If

            If m_bIsServer Then
                'suck in list

                m_lReturn = m_oBusiness.ImportList(txtfile.Text, vData)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception("1, cmdUpdate, Failed to read from file")
                End If
            Else

                m_lReturn = CType(ImportList(txtfile.Text, vData), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception("1, cmdUpdate, Failed to read from file")
                End If
            End If

            'check for errors
            If Information.IsArray(vData) Then

                frmView.SetData(vData)
                frmView.ShowDialog()
            Else
                MessageBox.Show("No data found", "List Management", MessageBoxButtons.OK)
                Exit Sub
            End If

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to view file", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdView_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: Form_Load()
    '
    ' Description:  This will load the list
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '

    Private Sub FrmImport_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            m_oFormFields = New iPMFormControl.FormFields()

            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            PopListTypes()

            txtEffectiveDate.Text = DateTime.Today
        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: PopListVersions()
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Private Sub PopListVersions()

        Dim vData(,) As Object
        Dim cboListVersion_NewIndex As Integer = -1

        Try

            If cboListType.Text = "" Then
                cboListVersion.Items.Clear()

                cboListVersion_NewIndex = cboListVersion.Items.Add("(new)")
                cboListVersion.Text = "(new)"
                Exit Sub
            End If

            'show description
            txtDescription.Text = CStr(vDataLT(2, cboListType.SelectedIndex))

            '   Get data from business object

            m_lReturn = m_oBusiness.GetListVersions(VB6.GetItemData(cboListType, cboListType.SelectedIndex), vData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception("1, " + +", Failed to get list of list versions")
            End If

            'Clear
            cboListVersion.Items.Clear()

            'if we have a list then stick it in
            If Information.IsArray(vData) Then

                With cboListVersion
                    '

                    'ad stuff to list view

                    For i As Integer = 0 To vData.GetUpperBound(1)
                        'add description to combo

                        'Developer Guide No. 153
                        '.Items.Add(CStr(vData(0, i)))

                        Dim listIndex As Integer = cboListVersion.Items.Add(New VB6.ListBoxItem(CStr(vData(0, i))))

                        'NIIT DONE
                        If listIndex <= vData.GetUpperBound(1) Then
                            .SelectedIndex = listIndex
                        End If
                    Next i

                End With

            End If

            'add "new" item and set it as default
            If cboListVersion.Items.Count = 0 Then
                cboListVersion_NewIndex = cboListVersion.Items.Add(CStr(1))
                cboListVersion.Text = "1"
            Else
                cboListVersion_NewIndex = cboListVersion.Items.Add(CStr(CInt(cboListVersion.Text) + 1))
                cboListVersion.SelectedIndex = cboListVersion_NewIndex
            End If

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to pop list ", vApp:=ACApp, vClass:=ACClass, vMethod:="PopListVersions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: PopListTypes()
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Private Sub PopListTypes()


        Try

            '   Get data

            m_lReturn = m_oBusiness.GetListTypes(vDataLT)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception("1, " + +", Failed to get list of list types")
            End If

            'if we have a list then stick it in
            If Information.IsArray(vDataLT) Then

                With cboListType
                    'clear
                    .Items.Clear()

                    'add stuff to list view
                    For i As Integer = 0 To vDataLT.GetUpperBound(1)
                        'add description to combo
                        '.Items.Add(CStr(vDataLT(1, i)))

                        'add id to listitem data

                        'Developer Guide No. 153
                        'VB6.SetItemData(cboListType, "NewIndex", CInt(vDataLT(0, i)))
                        Dim listIndex As Integer = cboListType.Items.Add(New VB6.ListBoxItem(CStr(vDataLT(1, i)), CInt(vDataLT(0, i))))
                    Next i

                End With

            End If

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to ill lisy type list", vApp:=ACApp, vClass:=ACClass, vMethod:="PopListtypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Function SetFieldValidation() As Integer


        Dim result As Integer = 0
        Try


            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboListType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtfile, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function CheckValidDate() As Long

        CheckValidDate = gPMConstants.PMEReturnCode.PMTrue

        If IsDate(Me.txtEffectiveDate.Text) Then
            If CDate(Me.txtEffectiveDate.Text) < "01 January 1900" Or CDate(Me.txtEffectiveDate.Text) > "31 December 9998" Then
                MsgBox("Please enter a date between January 1, 1900 and December 31, 9998", vbOKOnly + vbCritical, "Effective Date")
                Me.txtEffectiveDate.Text = FormatDateTime(Now(), DateFormat.ShortDate)
                CheckValidDate = gPMConstants.PMEReturnCode.PMFalse
            Else
                Me.txtEffectiveDate.Text = FormatDateTime(Me.txtEffectiveDate.Text, DateFormat.ShortDate)
            End If
        Else
            MsgBox("Please enter a date between January 1, 1900 and December 31, 9998", vbOKOnly + vbCritical, "Effective Date")
            Me.txtEffectiveDate.Text = FormatDateTime(Now(), DateFormat.ShortDate)
            CheckValidDate = gPMConstants.PMEReturnCode.PMFalse
        End If
    End Function

    Private Sub txtEffectiveDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEffectiveDate.TextChanged

    End Sub

    Private Sub txtEffectiveDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtEffectiveDate.Validating
        If CheckValidDate() <> gPMConstants.PMEReturnCode.PMTrue Then
            If txtEffectiveDate.Visible Then txtEffectiveDate.Focus()
            e.Cancel = True
        End If
    End Sub
End Class
