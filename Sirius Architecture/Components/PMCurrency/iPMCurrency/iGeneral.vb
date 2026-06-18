Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Friend NotInheritable Class General
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: General
    '
    ' Date: {TodaysDate}
    '
    ' Description: General class to accompany the interface form.
    '
    ' Edit History:
    ' VB 14/03/2005 PN19417 : ValidCurCode module added for checking duplicate currency code
    ' ***************************************************************** '



    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "General"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Private instance of the interface form.
    'developer guide no.291
    Private m_frmInterface As Object

    ' Private instance of the business object.
    Private m_oBusiness As Object

    ' Index value used when adding new data.
    Private m_lNewIndex As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public WriteOnly Property NewIndex() As Integer
        Set(ByVal Value As Integer)

            ' Store the new index value.
            m_lNewIndex = Value

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
    Public Function Initialise(ByRef frmInterface As Form, ByRef oBusiness As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the instance of the form into the member.
            m_frmInterface = frmInterface

            ' Store the instance of the business object
            ' into the member.
            m_oBusiness = oBusiness

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
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
            If disposing Then
                m_oBusiness = Nothing
                m_frmInterface = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetInterfaceDetails
    '
    ' Description: Gets the interface details.
    '
    ' ***************************************************************** '
    Public Function GetInterfaceDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.

            If m_frmInterface.Task = gPMConstants.PMEComponentAction.PMEdit Or m_frmInterface.Task = gPMConstants.PMEComponentAction.PMView Then
                ' Get the interface details from the
                ' business object.

                m_lReturn = m_frmInterface.GetBusiness()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Assign the details from the business object
                ' to the interface.

                m_lReturn = m_frmInterface.BusinessToData()

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Display all of the lookup details.

            m_lReturn = m_frmInterface.DisplayLookupDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the grid default values.

            m_lReturn = m_frmInterface.SetGridInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Public Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String
        ' RDC 11092003

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RDC 11092003 code and description cannot be empty

            If m_frmInterface.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                m_lReturn = m_oBusiness.CheckCodesDescriptions()

                If m_lReturn = gPMConstants.PMEReturnCode.PMMandatoryMissing Then
                    MessageBox.Show("Codes and description are mandatory." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                    "Please check data and try again.", "iPMCurrencyMaintenance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    g_ToMessage = True
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Check the task.

            Select Case (m_frmInterface.Task)
                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.

                    If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Check the details havn't changed.

                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        ' Update the details using the business object.

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                    End If
            End Select

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GridRead
    '
    ' Description: Used when the grid is first shown, when the grid is
    '              scrolled, and after a record in the grid is modified
    '              and the user commits the change by moving off of the
    '              current row. The grid fetches data in "chunks", and
    '              the number of rows the grid is asking for is given
    '              by RowBuf.RowCount.
    '
    ' ***************************************************************** '
    'Public Function GridRead(ByVal RowBuf As DataGridViewRow, ByRef vStartLocation As Object, ByVal lOffset As Integer, ByRef lApproximatePosition As Integer) As Integer

    '    Dim result As Integer = 0
    '    Dim iColIndex As Integer
    '    Dim iRowsFetched As Integer
    '    Dim lNewPosition As Integer
    '    Dim vBookmark As String = ""

    '    Try

    '        result = gPMConstants.PMEReturnCode.PMTrue

    '        If lOffset = 0 Then
    '            lOffset = 1
    '        End If
    '        iRowsFetched = 0

    '        ' Step through all of the rows.
    '        For lRow As Integer = 0 To 1 - 1
    '            ' Get the bookmark of the next available row.
    '            vBookmark = GetRelativeBookmark(vBookmark:=vStartLocation, lOffset:=lOffset + lRow)

    '            ' If the next row is BOF or EOF (Null), then
    '            ' stop fetching and return any rows fetched up
    '            ' to this point.

    '            If Convert.IsDBNull(vBookmark) Or IsNothing(vBookmark) Then
    '                Exit For
    '            End If

    '            ' Step through all of the columns.
    '            For iCol As Integer = 0 To RowBuf.Cells.Count - 1
    '                ' Place the record data into the row buffer.
    '                iColIndex = RowBuf.Cells(iCol).ColumnIndex


    '                RowBuf.Cells(iCol).Value = GetUserData(vBookmark:=vBookmark, iCol:=iColIndex)
    '            Next iCol

    '            ' Set the bookmark for the row.


    '            'TODOLIST
    '            'RowBuf = vBookmark
    '            RowBuf.Tag = vBookmark

    '            ' Increment the fetched rows counter.
    '            iRowsFetched += 1
    '        Next lRow

    '        ' Tell the grid how many rows were fetched

    '        'TODOLIST
    '        'RowBuf.RowCount = iRowsFetched

    '        ' Set the approximate scroll bar position.  Only
    '        ' nonnegative values are valid.

    '        lNewPosition = IndexFromBookmark(vBookmark:=CStr(vStartLocation), lOffset:=lOffset)

    '        ' Check the new position is greater or equal to zero.
    '        If lNewPosition >= 0 Then
    '            lApproximatePosition = lNewPosition
    '        End If

    '        Return result

    '    Catch excep As System.Exception



    '        ' Error Section.

    '        result = gPMConstants.PMEReturnCode.PMError

    '        ' Log Error.
    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="GridRead", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '        Return result

    '    End Try
    'End Function

    'Public Function GridRead(ByVal RowBuf As oBJECT, ByRef vStartLocation As Object, ByVal lOffset As Integer, ByRef lApproximatePosition As Integer) As Integer

    '    Dim result As Integer = 0
    '    Dim iColIndex As Integer
    '    Dim iRowsFetched As Integer
    '    Dim lNewPosition As Integer
    '    Dim vBookmark As String = ""

    '    Try

    '        result = gPMConstants.PMEReturnCode.PMTrue

    '        If lOffset = 0 Then
    '            lOffset = 1
    '        End If
    '        iRowsFetched = 0

    '        ' Step through all of the rows.
    '        For lRow As Integer = 0 To 1 - 1
    '            ' Get the bookmark of the next available row.
    '            vBookmark = GetRelativeBookmark(vBookmark:=vStartLocation, lOffset:=lOffset + lRow)

    '            ' If the next row is BOF or EOF (Null), then
    '            ' stop fetching and return any rows fetched up
    '            ' to this point.

    '            If Convert.IsDBNull(vBookmark) Or IsNothing(vBookmark) Then
    '                Exit For
    '            End If

    '            ' Step through all of the columns.
    '            For iCol As Integer = 0 To RowBuf.Cells.Count - 1
    '                ' Place the record data into the row buffer.
    '                iColIndex = RowBuf.Cells(iCol).ColumnIndex


    '                RowBuf.Cells(iCol).Value = GetUserData(vBookmark:=vBookmark, iCol:=iColIndex)
    '            Next iCol

    '            ' Set the bookmark for the row.


    '            'TODOLIST
    '            'RowBuf = vBookmark
    '            RowBuf.Tag = vBookmark

    '            ' Increment the fetched rows counter.
    '            iRowsFetched += 1
    '        Next lRow

    '        ' Tell the grid how many rows were fetched

    '        'TODOLIST
    '        'RowBuf.RowCount = iRowsFetched

    '        ' Set the approximate scroll bar position.  Only
    '        ' nonnegative values are valid.

    '        lNewPosition = IndexFromBookmark(vBookmark:=CStr(vStartLocation), lOffset:=lOffset)

    '        ' Check the new position is greater or equal to zero.
    '        If lNewPosition >= 0 Then
    '            lApproximatePosition = lNewPosition
    '        End If

    '        Return result

    '    Catch excep As System.Exception



    '        ' Error Section.

    '        result = gPMConstants.PMEReturnCode.PMError

    '        ' Log Error.
    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="GridRead", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '        Return result

    '    End Try
    'End Function
    ' ***************************************************************** '
    ' Name: GridWrite
    '
    ' Description: Used when the user modifies an existing row and
    '              attempts to commit the changes by moving to another
    '              row.
    '
    ' ***************************************************************** '
    'Public Function GridWrite(ByVal RowBuf As DataGridViewRow, ByRef vWriteLocation As Object) As Integer

    '	Dim result As Integer = 0

    '	Try 

    '		result = gPMConstants.PMEReturnCode.PMTrue

    '		' Step through all of the columns of the row, storing
    '		' the new values.
    '		For iCol As Integer = 0 To RowBuf.Cells.Count - 1
    '			Dim auxVar As Object = RowBuf.Cells(iCol).FormattedValue


    '			If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then
    '				m_lReturn = StoreUserData(vBookmark:=vWriteLocation, iCol:=iCol, vUserVal:=RowBuf.Cells(iCol).FormattedValue)

    '				' Check for errors
    '				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    '                       'TODOLIST
    '                       'RowBuf.RowCount = 0
    '				End If
    '			End If
    '		Next iCol

    '		' Update the data in the business object.


    '		m_lReturn = m_frmInterface.DataToBusiness(lMode:=gPMConstants.PMEComponentAction.PMEdit, lDataID:=CInt(vWriteLocation))

    '		' Check for errors.
    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			result = gPMConstants.PMEReturnCode.PMFalse
    '		End If

    '		Return result

    '	Catch excep As System.Exception



    '		' Error Section.

    '		result = gPMConstants.PMEReturnCode.PMError

    '		' Log Error.
    '		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="GridWrite", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '		Return result

    '	End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GridAdd
    '
    ' Description: Used when the user adds a new row of data.
    '
    ' ***************************************************************** '
    'Public Function GridAdd(ByVal RowBuf As DataGridViewRow, ByRef vNewRowBookmark As String) As Integer

    '	Dim result As Integer = 0
    '	Dim vNewVal As Object
    '	Dim iRow As Integer

    '	Try 

    '		result = gPMConstants.PMEReturnCode.PMTrue

    '		' Get a new bookmark value.
    '		vNewRowBookmark = GetNewBookmark()

    '		'Does this one already exist?


    '		vNewVal = RowBuf.Cells(0).FormattedValue

    '		m_lReturn = ValidCurCode(RowBuf:=RowBuf, vCurrentRow:=CInt(vNewRowBookmark))

    '		' Check for errors.
    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			Return result
    '		End If

    '		' Step through all of the columns of the row, storing
    '		' the new values.
    '		For iCol As Integer = 0 To RowBuf.Cells.Count - 1


    '			vNewVal = RowBuf.Cells(iCol).FormattedValue

    '			'If (iCol = 0 And Not bOk) Then
    '			'    m_lReturn = GetAVal(vNewVal)
    '			'    MsgBox "Code already exists - renamed to " & vNewVal, vbCritical + vbOKOnly, "Currency Maintenance"
    '			'End If

    '			' Check if the new value is a null.

    '			If Convert.IsDBNull(vNewVal) Or IsNothing(vNewVal) Then
    '				' Assign a default value.


    '				vNewVal = m_frmInterface.grdMainData.Columns(iCol).DefaultValue
    '			End If

    '			' Store the new values.
    '			m_lReturn = StoreUserData(vBookmark:=vNewRowBookmark, iCol:=iCol, vUserVal:=vNewVal)

    '			' Check for errors.
    '			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '				' Failed to store the new values.
    '				DeleteRow(vNewRowBookmark)

    '                   'TODOLIST
    '                   'RowBuf.RowCount = 0

    '				Return result
    '			End If
    '		Next iCol

    '		' Update the data in the business object.

    '		m_lReturn = m_frmInterface.DataToBusiness(lMode:=gPMConstants.PMEComponentAction.PMAdd, lDataID:=CInt(vNewRowBookmark))

    '		' Check for errors.
    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			result = gPMConstants.PMEReturnCode.PMFalse
    '		End If

    '		Return result

    '	Catch excep As System.Exception



    '		' Error Section.

    '		result = gPMConstants.PMEReturnCode.PMError

    '		' Log Error.
    '		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="GridAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '		Return result

    '	End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GridDelete
    '
    ' Description: Used when the user deletes an existing row.
    '
    ' ***************************************************************** '
    Public Function GridDelete(ByRef vBookmark As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'This was the wrong way around - imagine what was happening (CCWBAW)...
            ' Delete the row using the bookmark value.
            '    If (DeleteRow(vBookmark) <> PMTrue) Then
            '        ' Failed to delete the row, so we return
            '        ' a null.
            '        vBookmark = Null
            '    Else
            '        ' Update the data in the business object.
            '        m_lReturn& = m_frmInterface.DataToBusiness( _
            ''            lMode:=PMDelete, _
            ''            lDataID:=CLng(vBookmark))
            '
            '        ' Check for errors.
            '        If (m_lReturn& <> PMTrue) Then
            '            GridDelete = PMFalse
            '        End If
            '    End If

            ' Update the data in the business object.

            m_lReturn = m_frmInterface.DataToBusiness(lMode:=gPMConstants.PMEComponentAction.PMDelete, lDataID:=CInt(vBookmark))

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If DeleteRow(vBookmark) <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to delete the row, so we return
                ' a null.

                vBookmark = Nothing
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to grid delete data", vApp:=ACApp, vClass:=ACClass, vMethod:="GridDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'PUBLIC Methods (End)


    'PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: MakeBookmark
    '
    ' Description: Converts the index value to a standard string
    '              format.
    '
    ' ***************************************************************** '
    Private Function MakeBookmark(ByRef lIndex As Integer) As String

        Dim result As String = String.Empty


        ' Convert the string.

        Return Conversion.Str(lIndex)

    End Function

    ' ***************************************************************** '
    ' Name: IndexFromBookmark
    '
    ' Description: Gets the row index value that corresponds to a row
    '              that is offset rows from the row specified by the
    '              bookmark parameter.
    '
    ' ***************************************************************** '
    Private Function IndexFromBookmark(ByRef vBookmark As Object, ByRef lOffset As Integer) As Integer

        Dim result As Integer = 0
        Dim lIndex As Integer



        ' Check if the bookmark is null.

        If Convert.IsDBNull(vBookmark) Or IsNothing(vBookmark) Then
            ' Check if the offset value is less the zero.
            If lOffset < 0 Then
                ' Use the ubound plus the offset the represent EOF.
                lIndex = g_vGridData.GetUpperBound(1) + lOffset
            Else
                ' Use the minus value plus the offset the represent BOF.
                lIndex = -1 + lOffset
            End If
        Else
            ' Convert string to a value.
            lIndex = CInt(Conversion.Val(vBookmark) + lOffset)
        End If

        ' Check to see if the row index is valid.

        If lIndex >= 0 And lIndex <= g_vGridData.GetUpperBound(1) Then
            Return lIndex
        Else
            Return -1
        End If




        ' Error Section.

        result = -1

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get index from bookmark", vApp:=ACApp, vClass:=ACClass, vMethod:="IndexFromBookmark", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetNewBookmark
    '
    ' Description: Creates a new bookmark value for a newly added row.
    '
    ' ***************************************************************** '
    Private Function GetNewBookmark() As String

        Dim result As String = String.Empty


        ' Increment the new index value.
        m_lNewIndex += 1

        ' Reserve space for the new row in the array
        ReDim Preserve g_vGridData(g_vGridData.GetUpperBound(0), m_lNewIndex - 1)

        ' Assign the new index value to the new row.

        g_vGridData(g_vGridData.GetUpperBound(0), g_vGridData.GetUpperBound(1)) = m_lNewIndex

        ' Get bookmark value.
        result = MakeBookmark(lIndex:=g_vGridData.GetUpperBound(1))

        ' Calibrate the scroll bars based on the new size.

        m_frmInterface.grdMainData.ApproxCount = g_vGridData.GetUpperBound(1)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetRelativeBookmark
    '
    ' Description: Gets a bookmark for a row that is a specified
    '              number of rows away from the given row.
    '
    ' ***************************************************************** '
    Private Function GetRelativeBookmark(ByRef vBookmark As Object, ByRef lOffset As Integer) As String

        Dim result As String = String.Empty
        Dim lIndex As Integer



        ' Get the index value of the bookmark and offset.

        lIndex = IndexFromBookmark(vBookmark:=vBookmark, lOffset:=lOffset)

        ' Check if the index value is correct.

        If lIndex < 0 Or lIndex > g_vGridData.GetUpperBound(1) Then

            Return Nothing
        Else
            Return MakeBookmark(lIndex:=lIndex)
        End If




        ' Error Section.


        result = Nothing

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get relative bookmark", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRelativeBookmark", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteBookmark
    '
    ' Description: Gets a valid value based on what was sent.
    '
    ' ***************************************************************** '
    Public Function GetAVal(ByRef vNewVal As String) As Integer

        Dim result As Integer = 0
        Dim sString, sTemp As String
        Dim iTemp As Integer
        Dim bOk As Boolean

        Try

            sString = vNewVal

            If sString.Length < 4 Then
                sString = sString & New String(" ", 4 - sString.Length)
            End If

            iTemp = 1
            bOk = False
            While Not bOk

                iTemp += 1
                sTemp = CStr(iTemp).Trim()

                sTemp = sString.Substring(0, 4 - sTemp.Length) & sTemp

                bOk = True
                For Each dr As DataRow In gridData.Rows

                    If CStr(dr(0).ToString.Trim().ToUpper() = sTemp.ToUpper()) Then
                        bOk = False
                    End If
                Next


            End While

            vNewVal = sTemp

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get a value", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAVal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserData
    '
    ' Description: Gets the data from the array using the bookmark and
    '              column values.
    '
    ' ***************************************************************** '
    Private Function GetUserData(ByRef vBookmark As Object, ByRef iCol As Integer) As String

        Dim result As String = String.Empty
        Dim lIndex As Integer



        ' Get the index value using the bookmark value.

        lIndex = IndexFromBookmark(vBookmark:=CStr(vBookmark), lOffset:=0)

        ' Check if the index value is correct.

        If lIndex < 0 Or lIndex > g_vGridData.GetUpperBound(1) Or iCol < 0 Or iCol > g_vGridData.GetUpperBound(0) Then

            Return Nothing
        Else

            Return CStr(g_vGridData(iCol, lIndex))
        End If




        ' Error Section.


        result = Nothing

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get user data", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: StoreUserData
    '
    ' Description: Updates the data array from the grid data.
    '
    ' ***************************************************************** '
    Private Function StoreUserData(ByRef vBookmark As Object, ByRef iCol As Integer, ByRef vUserVal As Object) As Integer

        Dim result As Integer = 0
        Dim lIndex As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the index value using the bookmark value.

        lIndex = IndexFromBookmark(vBookmark:=CStr(vBookmark), lOffset:=0)

        ' Check if the index value is correct.
        If lIndex < 0 Or lIndex > g_vGridData.GetUpperBound(1) Or iCol < 0 Or iCol > g_vGridData.GetUpperBound(0) Then
            result = gPMConstants.PMEReturnCode.PMFalse
        Else


            g_vGridData(iCol, lIndex) = vUserVal
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteRow
    '
    ' Description: Deletes a row from the data array.
    '
    ' ***************************************************************** '
    Private Function DeleteRow(ByRef vBookmark As String) As Integer

        Dim result As Integer = 0
        Dim lIndex As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the index value using the bookmark value.
        lIndex = IndexFromBookmark(vBookmark:=vBookmark, lOffset:=0)

        ' Check if the index value is correct.
        If lIndex < 0 Or lIndex > g_vGridData.GetUpperBound(1) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Shift the data in the data array to fill the empty space
        ' vacated by the deleted row.
        For lRow As Integer = lIndex To g_vGridData.GetUpperBound(1) - 1
            For iCol As Integer = 0 To g_vGridData.GetUpperBound(0)


                g_vGridData(iCol, lRow) = g_vGridData(iCol, lRow + 1)
            Next iCol
        Next lRow

        ' Resize array to free storage space used by
        ' deleted row.
        If g_vGridData.GetUpperBound(1) > 0 Then
            ReDim Preserve g_vGridData(g_vGridData.GetUpperBound(0), g_vGridData.GetUpperBound(1) - 1)
        Else
            ReDim Preserve g_vGridData(g_vGridData.GetUpperBound(0), 0)
        End If

        ' Calibrate scroll bars based on new size.

        m_frmInterface.grdMainData.ApproxCount = g_vGridData.GetUpperBound(1)

        Return result

    End Function

    ' ***************************************************************** '
    ' VB 14/03/2005 PN19417
    ' Name: ValidCurCode
    '
    ' Description: Validation for duplicate currency code ..
    '
    ' ***************************************************************** '
    Public Function ValidCurCode(ByRef vCurrentRow As DataGridViewRow, ByVal vRowIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim bOk As Boolean
        Dim vOldVal As String = ""
        Dim vNewVal As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If gPMFunctions.ToSafeString(vCurrentRow.Cells(0).FormattedValue) <> "" Then
                vNewVal = CStr(vCurrentRow.Cells(0).FormattedValue).Trim()
                bOk = True
                'Does this one already exist?
                For Each dr As DataRow In gridData.Rows
                    If CStr(dr(0).ToString.Trim().ToUpper() = vNewVal.Trim().ToUpper()) Then
                        bOk = False
                    End If
                Next

                ' Step through all of the columns of the row

                If Not bOk Then
                    ' Get new Currency Code
                    m_lReturn = GetAVal(vNewVal)

                    If System.Windows.Forms.DialogResult.Yes = MessageBox.Show("Code already exists?" & Strings.Chr(13) & Strings.Chr(10) & " Renamed to " & _
                                                                               vNewVal, "Currency Maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question) Then

                        'RowBuf.Cells(0).Value = vNewVal
                    Else

                        '  RowBuf.Cells(0).Value = vOldVal
                    End If

                End If
            End If
            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed Validation", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidCurCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

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
        ' Error Section.
        '
        ' Log Error Message
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface general class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

