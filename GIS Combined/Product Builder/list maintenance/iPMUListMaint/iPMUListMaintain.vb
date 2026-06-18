Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Public Class frmMaintain
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    Private m_sMsgTitle As String = ""

    Private m_lActiveListNumber As Integer
    Private m_sLookupFile As String = ""
    Private arrRecords() As LookupRecord = Nothing

    Private udtIndexRecord As IndexRecord = IndexRecord.CreateInstance()

    Private udtHeaderRecord As HeaderRecord = HeaderRecord.CreateInstance()

    Public Structure LookupRecord
        Public Number As String
        Public Space1 As String
        Public Description As String
        Public Space2 As String
        Public Code As String
        Public EndOfLine As String
        Public DeletedFlag As String
        Public Shared Function CreateInstance() As LookupRecord
            Dim result As New LookupRecord
            ' developer guide no. 128
            'result.Number = New FixedLengthString(11)
            'result.Space1 = New FixedLengthString(1)
            'result.Description = New FixedLengthString(70)
            'result.Space2 = New FixedLengthString(1)
            'result.Code = New FixedLengthString(10)
            'result.EndOfLine = New FixedLengthString(2)
            'result.DeletedFlag = New FixedLengthString(1)
            Return result
        End Function
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
 _
    Private Structure LookupRecordBuffer
        Public EntireRecord As String
        Public Shared Function CreateInstance() As LookupRecordBuffer
            Dim result As New LookupRecordBuffer
            'result.EntireRecord = New FixedLengthString(95)
            Return result
        End Function
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
 _
    Private Structure HeaderRecord
        <VBFixedString(80), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=80)> _
        Public Filler As FixedLengthString
        <VBFixedString(10), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=10)> _
        Public PropertyId As FixedLengthString
        Public Shared Function CreateInstance() As HeaderRecord
            Dim result As New HeaderRecord
            result.Filler = New FixedLengthString(80)
            result.PropertyId = New FixedLengthString(10)
            Return result
        End Function
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
 _
    Private Structure IndexRecord
        <VBFixedString(10), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=10)> _
        Public PropertyId As FixedLengthString
        Dim RecordNumber As Integer
        Public Shared Function CreateInstance() As IndexRecord
            Dim result As New IndexRecord
            result.PropertyId = New FixedLengthString(10)
            Return result
        End Function
    End Structure

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iLine As Integer
    Private m_sStepStatus As String = ""
    Private m_lNewFile As Integer

    'Private m_sServerListFilePathIdx As String
    'Private m_sServerListFilePathDat As String
    'Private m_sReleasePath As String

    Private m_sServerListFilePath As String = ""
    Private m_sServerListFilePathAndFile As String = ""
    Private m_sServerListFilePathIdx As String = ""
    Private m_sServerListFilePathDat As String = ""
    Private m_sServerListVersion As String = ""
    Private m_sServerListPrefVersion As String = ""
    Private m_bServerListFileCompressed As Boolean

    Public Property NewFile() As Integer
        Get
            Return m_lNewFile
        End Get
        Set(ByVal Value As Integer)
            m_lNewFile = Value
        End Set
    End Property


    Public Property LookupFile() As String
        Get
            Return m_sLookupFile
        End Get
        Set(ByVal Value As String)
            m_sLookupFile = Value
        End Set
    End Property


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

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus

        End Get
    End Property

    ' Sort the items in array values() with bounds min and max.

    'Private Sub Quicksort(ByRef values() As LookupRecord, ByVal min As Integer, ByVal max As Integer)
    'RWH(18/07/2000) Picked up this routine from MSDN. Modified it to deal with my
    'array of udt's. Also the random number generator bit at the beginning has been
    'modified as that from MSDN didn't seem to work!
    '

    'Dim udtWholeRecord As LookupRecord = LookupRecord.CreateInstance()
    'Dim med_value As String = ""
    'Dim hi, lo, i As Integer
    '
    'Try 
    '
    ' If the list has only 1 item, it's sorted.
    'If min >= max Then 'Exit Sub
    '
    ' Pick a dividing item randomly.
    'VBMath.Randomize()
    'i = CInt(min + CSng(Math.Floor((VBMath.Rnd() * (max - min)) + 1)))
    'RWH '''''''
    'med_value = values(i).Number.Value
    'udtWholeRecord = values(i)
    '''''''''''''''''''''
    '
    ' Swap the dividing item to the front of the list.
    'values(i) = values(min)
    '
    ' Separate the list into sublists.
    'lo = min
    'hi = max
    'Do 
    ' Look down from hi for a value < med_value.
    'Do While values(hi).Number.Value >= med_value
    'hi -= 1
    'If hi <= lo Then 'Exit Do
    'Loop 
    '
    'If hi <= lo Then
    ' The list is separated.
    '            values(lo).Number = med_value
    'RWH '''''''
    'values(lo) = udtWholeRecord
    '''''''''''''''''''''
    'Exit Do
    'End If
    '
    ' Swap the lo and hi values.
    'values(lo) = values(hi)
    '
    ' Look up from lo for a value >= med_value.
    'lo += 1
    'Do While values(lo).Number.Value < med_value
    'lo += 1
    'If lo >= hi Then 'Exit Do
    'Loop 
    '
    'If lo >= hi Then
    ' The list is separated.
    'lo = hi
    '            values(hi).Number = med_value
    'RWH '''''''
    'values(hi) = udtWholeRecord
    '''''''''''''''''''''
    'Exit Do
    'End If
    '
    ' Swap the lo and hi values.
    'values(hi) = values(lo)
    '
    'Loop  ' Loop until the list is separated.
    '
    ' Recursively sort the sublists.
    'Quicksort(values, min, lo - 1)
    'Quicksort(values, lo + 1, max)
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Log Error.
    'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort array", vApp:=ACApp, vClass:=ACClass, vMethod:="Quicksort", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    Private Sub NotSoQuicksort(ByRef values() As LookupRecord)
        'QuickSort seems to be going a little awry
        'Let's try a not so quick version


        Dim udtWholeRecord As LookupRecord = LookupRecord.CreateInstance()
        Dim sTemp1, sTemp2 As String

        Try

            For lLoop1 As Integer = values.GetLowerBound(0) To values.GetUpperBound(0)
                For lLoop2 As Integer = lLoop1 + 1 To values.GetUpperBound(0)
                    sTemp1 = values(lLoop1).Number & values(lLoop1).Description
                    sTemp2 = values(lLoop2).Number & values(lLoop2).Description
                    If sTemp1 > sTemp2 Then
                        udtWholeRecord = values(lLoop1)
                        values(lLoop1) = values(lLoop2)
                        values(lLoop2) = udtWholeRecord
                    End If
                Next lLoop2
            Next lLoop1

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort array", vApp:=ACApp, vClass:=ACClass, vMethod:="NotSoQuicksort", excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Function StoreDetails(ByVal sFilenameDat As String) As Integer
        'Writes contents of array to file.

        Dim result As Integer = 0
        Dim iFileNumDat As Integer

        Dim udtBuffer As LookupRecordBuffer = LookupRecordBuffer.CreateInstance()
        'Dim lStart As Long
        'Dim lDuration As Long
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    lStart = Timer

            iFileNumDat = FileSystem.FreeFile()

            FileSystem.FileOpen(iFileNumDat, sFilenameDat, OpenMode.Output, , , Marshal.SizeOf(udtBuffer))

            'Loop thru' our sorted array of lookup entries, writing each one, in turn,
            'to file.
            For Each arrRecords_item As LookupRecord In arrRecords

                If arrRecords_item.DeletedFlag <> "Y" Then
                    lRecordCount += 1

                    udtBuffer.EntireRecord = arrRecords_item.Number & arrRecords_item.Space1 & arrRecords_item.Description & arrRecords_item.Space2 & arrRecords_item.Code
                    FileSystem.PrintLine(iFileNumDat, udtBuffer.EntireRecord)
                End If

            Next arrRecords_item

            FileSystem.FileClose(iFileNumDat)
            '    lDuration = Timer - lStart
            '    Debug.Print lDuration & " seconds to write file."

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get store details", vApp:=ACApp, vClass:=ACClass, vMethod:="StoreDetails", excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetLookupTypeValues() As Integer
        'Retrieve all entries from file of type selected in combo and store in array.

        Dim result As Integer = 0
        Dim sKeyRequired As String = ""
        Dim iFileNum As Integer
        Dim lPosition, lStart, lDuration As Integer

        Dim udtBuffer As LookupRecordBuffer = LookupRecordBuffer.CreateInstance()

        Dim udtRecord As LookupRecord = LookupRecord.CreateInstance()
        Dim sPreviousRecord As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iFileNum = FileSystem.FreeFile()
            lPosition = 0

            lStart = CInt(DateTime.Now.TimeOfDay.TotalSeconds)
            '    Open m_sLookupFile For Random As #iFileNum
            FileSystem.FileOpen(iFileNum, m_sLookupFile, OpenMode.Input)

            sPreviousRecord = ""
            Do While Not FileSystem.EOF(iFileNum)

                'Read next line into string.
                udtBuffer.EntireRecord = FileSystem.LineInput(iFileNum)

                'Copy string into field sectioned udt.
                If udtBuffer.EntireRecord.Length < 95 Then
                    udtBuffer.EntireRecord = udtBuffer.EntireRecord & Space(95 - (udtBuffer.EntireRecord.Length - 1))
                End If
                'udtRecord = LSet(udtBuffer)
                udtRecord.Number = udtBuffer.EntireRecord.Substring(0, 11)
                udtRecord.Space1 = udtBuffer.EntireRecord.Substring(11, 1)
                'udtRecord.Description = udtBuffer.EntireRecord.Substring(12, 70)
                udtRecord.Description = udtBuffer.EntireRecord.Substring(12, 70)
                udtRecord.Space2 = udtBuffer.EntireRecord.Substring(82, 1)
                udtRecord.Code = udtBuffer.EntireRecord.Substring(84, 10)
                udtRecord.EndOfLine = udtBuffer.EntireRecord.Substring(92, 2)
                udtRecord.DeletedFlag = udtBuffer.EntireRecord.Substring(94, 1)


                'Increase size of storage array and add in new record.
                ReDim Preserve arrRecords(lPosition)
                'arrRecords(lPosition) = udtRecord

                arrRecords(lPosition) = udtRecord
                lPosition += 1

                sPreviousRecord = udtRecord.Number.Trim()
                '        Debug.Print udtRecord.Number & " " & udtRecord.Description

                Application.DoEvents()
            Loop

            ' FileSystem.FileClose(iFileNum)

            lDuration = CInt(DateTime.Now.TimeOfDay.TotalSeconds - lStart)
            '    Debug.Print lDuration & " seconds to read in file." & Chr(13) & Chr(10) & lPosition & " records read."
            '    MsgBox lDuration & " seconds to read in file." & Chr(13) & Chr(10) & lPosition & " records read."

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get list values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupTypeValues", excep:=excep)

            Return result

        Finally
            FileSystem.FileClose(iFileNum)

        End Try
        Return result
    End Function

    Private Function DisplayLookupsOfType() As Integer
        'Populates list box with lookups of type selected in combo.
        Dim result As Integer = 0
        Dim lLookupNumber, lNextRef As Integer
        Dim lStart As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If ArrayExists(arrRecords) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            lStart = CInt(DateTime.Now.TimeOfDay.TotalSeconds)

            lstLookupValues.Items.Clear()

            'Get Ref. Number of Lookup Type we are addding items to.
            lLookupNumber = VB6.GetItemData(cboLookupTypes, cboLookupTypes.SelectedIndex)

            '    'Find first record of this type
            '    For lRecordCount = 0 To UBound(arrRecords)
            '        lNextRef = Val(arrRecords(lRecordCount).Number)
            '        If lNextRef = lLookupNumber Then
            '            bTypeFound = True
            '            Exit For
            '        End If
            '    Next lRecordCount

            '    If bTypeFound Then
            '        'Display all records of this type.
            '        Do While (lRecordCount <= UBound(arrRecords))
            '            lNextRef = Val(arrRecords(lRecordCount).Number)
            '            If lNextRef = lLookupNumber Then
            '                lRecordCount = lRecordCount + 1
            '                lstLookupValues.AddItem Trim$(arrRecords(lRecordCount).Description)
            ''                lstLookupValues.ItemData(lstLookupValues.NewIndex) = lLookupNumber
            '                lstLookupValues.ItemData(lstLookupValues.NewIndex) = lRecordCount
            '            Else
            '                Exit Do
            '            End If
            '        Loop
            '        lDuration = Timer - lStart
            '
            ''        Debug.Print lDuration & " seconds to display records of this type."
            ''    Else
            ''        MsgBox "Lookup Type not found."
            '    End If

            'Why not just loop through and display them...
            For lRecordCount As Integer = 0 To arrRecords.GetUpperBound(0)
                lNextRef = CInt(Conversion.Val(arrRecords(lRecordCount).Number))
                If lNextRef = lLookupNumber Then
                    If arrRecords(lRecordCount).DeletedFlag <> "Y" Then
                        Dim lstLookupValues_NewIndex As Integer = -1
                        lstLookupValues_NewIndex = lstLookupValues.Items.Add(arrRecords(lRecordCount).Description.Trim())
                        '                lstLookupValues.ItemData(lstLookupValues.NewIndex) = lLookupNumber
                        VB6.SetItemData(lstLookupValues, lstLookupValues_NewIndex, lRecordCount)
                    End If
                End If
            Next lRecordCount

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display list", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupsOfType", excep:=excep)

            Return result

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    Private Function UpdateArrayAndDisplay() As Integer
        'Updates array of udt's with new values not yet saved and move to
        'list of existing values. Values are NOT saved to file at this point.

        Dim result As Integer = 0

        Dim udtNewRecord As LookupRecord = LookupRecord.CreateInstance()

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iCount As Integer = 0 To lstNewValues.Items.Count - 1
                'Populate udt with new details.
                'udtNewRecord.Number = RSet(CStr(m_lActiveListNumber), udtNewRecord.Number.Length)
                'udtNewRecord.Space1 = " "
                'udtNewRecord.Description = LSet(VB6.GetItemString(lstNewValues, iCount).Trim(), Len(udtNewRecord.Description))
                'udtNewRecord.Space2 = " "
                'udtNewRecord.Code = LSet(CStr(lstLookupValues.Items.Count + 1), Len(udtNewRecord.Code))


                udtNewRecord.Number = Space(11 - m_lActiveListNumber.ToString.Length) & CStr(m_lActiveListNumber)
                udtNewRecord.Space1 = " "
                udtNewRecord.Description = VB6.GetItemString(lstNewValues, iCount).Trim() & Space(70 - VB6.GetItemString(lstNewValues, iCount).Trim().Length)
                udtNewRecord.Space2 = " "
                udtNewRecord.Code = CStr(lstLookupValues.Items.Count + 1) & Space(10 - Len(CStr(lstLookupValues.Items.Count + 1)))

                'Add new record to array.
                If ArrayExists(arrRecords) <> gPMConstants.PMEReturnCode.PMTrue Then
                    ReDim arrRecords(0)
                Else
                    ReDim Preserve arrRecords(arrRecords.GetUpperBound(0) + 1)
                End If
                arrRecords(arrRecords.GetUpperBound(0)) = udtNewRecord

                'Update view of Lookup values of this type.
                lstLookupValues.Items.Add(udtNewRecord.Description.Trim())
            Next iCount
            lstNewValues.Items.Clear()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update array", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateArrayAndDisplay", excep:=excep)

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
        ' Display all language specific captions

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            If m_sLookupFile.Trim() <> "" Then
                SSTabHelper.SetTabCaption(tabMainTab, 0, m_sLookupFile)
            Else

                SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' KB 010801 reinstate help button


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLookupTypeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblNewValue.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewValueCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraNewValues.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACValuesToAddCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraStoredValues.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStoredValuesCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdRemove.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRemoveCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdUpdate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUpdateCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdRemoveStored.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRemoveStoredCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    cmdSaveAs.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACSaveAsCaption, _
            'iDataType:=PMResString)
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    '            cmdNavigate.Visible = True
                    '            cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    '            cmdNavigate.Visible = True
                    '            cmdNavigate.Enabled = False

                Case Else
                    '            cmdNavigate.Visible = False
            End Select

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    'Private Function SaveFileAs(ByVal sFilename As String) As Integer
    'Check's selected filename does not already exist and calls procedure
    'to actually store values.
    'Dim result As Integer = 0
    'Dim sMsg As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    '    'Check file does not already exist.
    '    If (Dir(sFilename) <> "") Then
    '        If (sFilename = m_sReleasePath & ".dat") Then
    '            sMsg = iPMFunc.GetResData( _
    ''                  iLangID:=g_iLanguageID%, _
    ''                  lID:=ACLiveFileWarning, _
    ''                  iDataType:=PMResString)
    '        Else
    '            sMsg = iPMFunc.GetResData( _
    ''                  iLangID:=g_iLanguageID%, _
    ''                  lID:=ACWarnFileExistsMsg, _
    ''                  iDataType:=PMResString)
    '        End If
    '        'Warn user that file already exists.
    '        If (MsgBox(sMsg, vbExclamation + vbYesNo + vbDefaultButton2, g_sTitle) = vbNo) Then
    '            SaveFileAs = PMFalse
    '            Exit Function
    '        End If
    '    End If
    '
    'If StoreDetails(sFilename) <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'Else
    'm_sLookupFile = sFilename
    'tabMainTab.SelectedTab.Text = m_sLookupFile
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Save File", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveFileAs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Function ArrayExists(ByRef arrCheck() As LookupRecord) As Integer
        'Little routine to check array exists using error raised on boundary check.
        Dim result As Integer = 0
        Dim lUbound As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lUbound = arrCheck.GetUpperBound(0)

            Return result

        Catch


            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    Private Function ValueAlreadyExists(ByVal sUserEntry As String) As Integer
        'Checks newly entered value does not already exist. If it does, user is warned
        'and existing value highlighted in list.

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sUserEntry = sUserEntry.Trim().ToUpper()

            For lExistingCount As Integer = 0 To lstLookupValues.Items.Count
                If sUserEntry = VB6.GetItemString(lstLookupValues, lExistingCount).Trim().ToUpper() Then
                    ListBoxHelper.SetSelectedIndex(lstLookupValues, lExistingCount)
                    result = gPMConstants.PMEReturnCode.PMTrue
                    Exit For
                End If
            Next lExistingCount

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Save File", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveFileAs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ArchiveRLDF (Private)
    '
    ' Description: Archives a copy of the RLDF

    ' ***************************************************************** '

    'Private Function ArchiveRLDF() As Integer
    'Dim result As Integer = 0
    'Dim sSequenceNumber, sSaveServerListFilePathDat, sSaveServerListFilePathDatIdx As String
    '
    'Try 
    '
    '
    '******************************************************************
    ' sequence number is built from the date and time
    '******************************************************************
    '    sSequenceNumber = _
    'Format(Date, "yyyymmdd") & Format(Time, "hhmmss")
    '
    '    sSaveServerListFilePathDat = _
    'm_sReleasePath & sSequenceNumber & ".dat"
    '    sSaveServerListFilePathDatIdx = _
    'm_sReleasePath & sSequenceNumber & ".idx"
    '******************************************************************
    ' Rename the existing RLDF
    '******************************************************************
    '    If (Dir(m_sReleasePath & ".dat") <> "") Then
    '        Name m_sReleasePath & ".dat" As sSaveServerListFilePathDat
    '    End If
    '    If (Dir(m_sReleasePath & ".idx") <> "") Then
    '        Name m_sReleasePath & ".idx" As sSaveServerListFilePathDatIdx
    '    End If
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="ArchiveRLDF Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ArchiveRLDF", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Function ReleaseIt() As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        Dim oPMUList As bPMUList.bPMUListCreate
        Dim oGISList As New bGISList.bGISListCreate
        Dim oGISListManager As bGISListManager.Form
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sMsg As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReleaseWarning, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Warn user what they are about to do and give them the opportunity to abort.
            If MessageBox.Show(sMsg, m_sMsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then
                If g_sServerListFilePath.EndsWith("\") Then
                    File.Delete(g_sServerListFilePath & g_sGISDataModelCode & "List" & g_sServerListVersion & ".txt")
                Else
                    File.Delete(g_sServerListFilePath & "\" & g_sGISDataModelCode & "List" & g_sServerListVersion & ".txt")
                End If

                Return result
            End If

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_oPMUList As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMUList, "bPMUList.bPMUListCreate", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMUList = temp_oPMUList

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACErrorAccessingBusObject, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                'Inform user process has failed.
                MessageBox.Show(sMsg, m_sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get an instance of the List Mananger business object via
            ' the public object manager.
            Dim temp_oGISListManager As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oGISListManager, "bGISListManager.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oGISListManager = temp_oGISListManager

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListManFailed, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                'Inform user process has failed.
                MessageBox.Show(sMsg, m_sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                oPMUList = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If g_sServerListFilePath.EndsWith("\") Then
                oPMUList.InputFile = g_sServerListFilePath & g_sGISDataModelCode & "List" & g_sServerListVersion & ".txt"
            Else
                oPMUList.InputFile = g_sServerListFilePath & "\" & g_sGISDataModelCode & "List" & g_sServerListVersion & ".txt"
            End If

            oPMUList.OutputFileDataModel = g_sGISDataModelCode

            oPMUList.OutputFilePath = g_sServerListFilePath


            lReturn = oPMUList.Create

            If lReturn = gPMConstants.PMEReturnCode.PMTrue Then


                lReturn = oGISListManager.SetServerSettings(r_sServerListFilePath:=g_sServerListFilePath, r_sServerListVersion:=g_sServerListVersion, r_sServerListPrefVersion:=g_sServerListPrefVersion, r_bServerListFileCompressed:=g_bServerListFileCompressed, v_sGISDataModelCode:=g_sGISDataModelCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFailedRegistrySet, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    'Inform user process has failed.
                    MessageBox.Show(sMsg, m_sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    oGISListManager = Nothing

                    oPMUList = Nothing

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oGISList.InputFile = oPMUList.InputFile
            lReturn = oGISList.Create()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACUMRatesTable, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                'Inform user process has failed.
                MessageBox.Show(sMsg, m_sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReleaseSuccess, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Inform user process has completed successfully.
            MessageBox.Show(sMsg, m_sMsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)

            Return result

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Release files", vApp:=ACApp, vClass:=ACClass, vMethod:="ReleaseIt", excep:=excep)

            Return result
        Finally
            oGISListManager = Nothing
            oGISList = Nothing
            oPMUList = Nothing

        End Try
    End Function

    Private Sub cboLookupTypes_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboLookupTypes.SelectedIndexChanged
        Dim sMsg As String = ""

        Try

            If lstNewValues.Items.Count <> 0 Then
                'Warn user that New Values have been entered but not saved.

                sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIncludeNewValuesMsg, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                If MessageBox.Show(sMsg, g_sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then
                    'Update file with unsaved entries.
                    cmdUpdate_Click(cmdUpdate, New EventArgs())
                End If
            End If

            If cboLookupTypes.SelectedIndex > -1 Then
                'Store new List Number. This is used in cmdUpdate_Click above to update
                'the correct list type.
                m_lActiveListNumber = VB6.GetItemData(cboLookupTypes, cboLookupTypes.SelectedIndex)
                'Get required list.
                DisplayLookupsOfType()

                txtNewValue.Enabled = True
            End If

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to select List Type", vApp:=ACApp, vClass:=ACClass, vMethod:="cboLookupTypes_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Dim sMsg As String = ""

        Try

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                Me.Close()
                Exit Sub
            End If

            If lstNewValues.Items.Count <> 0 Then

                sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelMsg, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                If MessageBox.Show(sMsg, g_sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If

            End If

            If g_sServerListFilePath.EndsWith("\") Then
                File.Delete(g_sServerListFilePath & g_sGISDataModelCode & "List" & g_sServerListVersion & ".txt")
            Else
                File.Delete(g_sServerListFilePath & "\" & g_sGISDataModelCode & "List" & g_sServerListVersion & ".txt")
            End If

            Me.Close()

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to cancel form", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' call help text
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        End If

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim sMsg As String = ""
        Dim iMsgReply As DialogResult

        Try

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                Me.Close()
                Exit Sub
            End If

            If lstNewValues.Items.Count <> 0 Then

                sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACWarnNotUpdatedMsg, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                iMsgReply = MessageBox.Show(sMsg, g_sTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)
                Select Case iMsgReply
                    Case System.Windows.Forms.DialogResult.Yes
                        cmdUpdate_Click(cmdUpdate, New EventArgs())
                    Case System.Windows.Forms.DialogResult.No
                        'do nothing
                        cmdCancel_Click(cmdCancel, New EventArgs())
                        Exit Sub
                    Case System.Windows.Forms.DialogResult.Cancel
                        Exit Sub
                End Select

            End If

            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                pnlStatus.Visible = True
                pnlStatus.BringToFront()
                lblStatus.Text = "Sorting..."
                Application.DoEvents()
                NotSoQuicksort(arrRecords)

                lblStatus.Text = "Done"
                Application.DoEvents()
                pnlStatus.Visible = False

                StoreDetails(m_sLookupFile)

                ReleaseIt()
            End If

            Me.Close()

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to OK form", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemove.Click

        Try

            'Remove selected item from New Values list.
            lstNewValues.Items.RemoveAt(CShort(ListBoxHelper.GetSelectedIndex(lstNewValues)))
            cmdRemove.Enabled = False
            If lstNewValues.Items.Count = 0 Then
                cmdUpdate.Enabled = False
            End If

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to remove new value", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRemove_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdRemoveStored_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemoveStored.Click


        'Got to remove this from the array as well...
        For lTemp As Integer = lstLookupValues.Items.Count To 1 Step -1
            If ListBoxHelper.GetSelected(lstLookupValues, lTemp - 1) Then
                arrRecords(VB6.GetItemData(lstLookupValues, lTemp - 1)).DeletedFlag = "Y"
                lstLookupValues.Items.RemoveAt(CShort(lTemp - 1))
            End If
        Next lTemp

    End Sub

    Private Sub cmdSort_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSort.Click
        'Debug button
        NotSoQuicksort(arrRecords)
        MessageBox.Show("Sorted", Application.ProductName)

    End Sub

    Private Sub cmdUpdate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdUpdate.Click
        Dim sFilename As String = ""
        Dim sMsg As String = ""

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            cmdUpdate.Enabled = False

            lblStatus.Text = "Updating..."
            lblStatus.Refresh()
            pnlStatus.Visible = True

            Application.DoEvents()

            UpdateArrayAndDisplay()

            '    Quicksort arrRecords(), 0, UBound(arrRecords)
            '    lblStatus.Caption = "Sorting..."
            '    DoEvents
            '    NotSoQuicksort arrRecords()

            '    StoreDetails m_sLookupFile

            lblStatus.Text = "Done"
            lblStatus.Refresh()
            Application.DoEvents()
            pnlStatus.Visible = False

            'Now that we have successfully updated we will always have
            'something to save so enable SaveAs button.
            '    cmdSaveAs.Enabled = True

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display list", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupsOfType", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdWrite_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdWrite.Click
        'Debug button.

        StoreDetails(m_sLookupFile)

    End Sub


    'Private Sub Command1_Click()
    ' call help text
    'm_lReturn = CType(PMHelpFunc.ShowHelp(dlgHelp:=dlgHelp, lContextID:=ScreenHelpID), gPMConstants.PMEReturnCode)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'End If
    'End Sub


    Private Sub frmMaintain_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            Dim cboLookupTypes_NewIndex As Integer = -1
            cboLookupTypes_NewIndex = cboLookupTypes.Items.Add(PMBConst.PMBListGenderText)
            VB6.SetItemData(cboLookupTypes, cboLookupTypes_NewIndex, PMBConst.PMBListGenderCode)
            cboLookupTypes_NewIndex = cboLookupTypes.Items.Add(PMBConst.PMBListMaritalStatusText)
            VB6.SetItemData(cboLookupTypes, cboLookupTypes_NewIndex, PMBConst.PMBListMaritalStatusCode)
            cboLookupTypes_NewIndex = cboLookupTypes.Items.Add(PMBConst.PMBListEmploymentText)
            VB6.SetItemData(cboLookupTypes, cboLookupTypes_NewIndex, PMBConst.PMBListEmploymentCode)
            cboLookupTypes_NewIndex = cboLookupTypes.Items.Add(PMBConst.PMBListTitleText)
            VB6.SetItemData(cboLookupTypes, cboLookupTypes_NewIndex, PMBConst.PMBListTitleCode)
            cboLookupTypes_NewIndex = cboLookupTypes.Items.Add(PMBConst.PMBListBusinessText)
            VB6.SetItemData(cboLookupTypes, cboLookupTypes_NewIndex, PMBConst.PMBListBusinessCode)
            cboLookupTypes_NewIndex = cboLookupTypes.Items.Add(PMBConst.PMBListOccupationText)
            VB6.SetItemData(cboLookupTypes, cboLookupTypes_NewIndex, PMBConst.PMBListOccupationCode)
            cboLookupTypes_NewIndex = cboLookupTypes.Items.Add(PMBConst.PMBListPaymentMethodText)
            VB6.SetItemData(cboLookupTypes, cboLookupTypes_NewIndex, PMBConst.PMBListPaymentMethodCode)
            cboLookupTypes_NewIndex = cboLookupTypes.Items.Add(PMBConst.PMBListAlcoholText)
            VB6.SetItemData(cboLookupTypes, cboLookupTypes_NewIndex, PMBConst.PMBListAlcoholCode)
            cboLookupTypes_NewIndex = cboLookupTypes.Items.Add(PMBConst.PMBListConvTypeText)
            VB6.SetItemData(cboLookupTypes, cboLookupTypes_NewIndex, PMBConst.PMBListConvTypeCode)
            cboLookupTypes_NewIndex = cboLookupTypes.Items.Add(PMBConst.PMBListConvStatusText)
            VB6.SetItemData(cboLookupTypes, cboLookupTypes_NewIndex, PMBConst.PMBListConvStatusCode)
            cboLookupTypes_NewIndex = cboLookupTypes.Items.Add(PMBConst.PMBListConvSentenceText)
            VB6.SetItemData(cboLookupTypes, cboLookupTypes_NewIndex, PMBConst.PMBListConvSentenceCode)
            cboLookupTypes_NewIndex = cboLookupTypes.Items.Add(PMBConst.PMBListTimeUnitText)
            VB6.SetItemData(cboLookupTypes, cboLookupTypes_NewIndex, PMBConst.PMBListTimeUnitCode)


            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'Reset array.
            ReDim arrRecords(0)

            If m_sLookupFile <> "" Then
                GetLookupTypeValues()
            End If

            'Check to see if there is anything to save before enabling Save As button.
            '    cmdSaveAs.Enabled = (ArrayExists(arrRecords) = PMTrue)

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                txtNewValue.Enabled = False
                lblNewValue.Enabled = False
                cmdRemove.Enabled = False
                '        cmdSaveAs.Enabled = False
            End If


            m_sMsgTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Load form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lstLookupValues_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstLookupValues.SelectedIndexChanged
        '    lstLookupValues.ToolTipText = lstLookupValues.List(lstLookupValues.ListIndex)
        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            cmdRemoveStored.Enabled = True
        End If
    End Sub

    Private Sub lstNewValues_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstNewValues.SelectedIndexChanged
        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            cmdRemove.Enabled = True
        End If
    End Sub

    Private Sub lstNewValues_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lstNewValues.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        'MsgBox KeyAscii
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtNewValue_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtNewValue.Enter
        txtNewValue.SelectionStart = 0
        txtNewValue.SelectionLength = Strings.Len(txtNewValue.Text)
    End Sub

    Private Sub txtNewValue_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtNewValue.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        Dim sMsg As String = ""

        Try

            If (KeyAscii = 13) And (txtNewValue.Text.Trim() <> "") Then
                If ValueAlreadyExists(txtNewValue.Text) = gPMConstants.PMEReturnCode.PMTrue Then


                    sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACValueAlreadyExistsMsg, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    MessageBox.Show(sMsg, g_sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    If KeyAscii = 0 Then
                        eventArgs.Handled = True
                    End If
                    Exit Sub
                End If

                lstNewValues.Items.Insert(lstNewValues.Items.Count, txtNewValue.Text)
                txtNewValue.Text = ""
                cmdUpdate.Enabled = True
            End If

            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub

        Catch
        End Try



        ' Log Error.
        gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process KeyPress", vApp:=ACApp, vClass:=ACClass, vMethod:="txtNewValue_KeyPress", excep:=New Exception(Information.Err().Description))

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        Exit Sub

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub


End Class
