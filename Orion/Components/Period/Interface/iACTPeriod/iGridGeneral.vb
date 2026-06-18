Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide no 129
Imports SharedFiles
Friend NotInheritable Class GridGeneral
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: General
    '
    ' Date: {TodaysDate}
    '
    ' Description: General class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '



    ' Constant for the functions to identify
    ' which class this is.
    ''Developer Guide no 50
    'Dim frmDetails As frmDetails
    Private Const ACClass As String = "General"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Private instance of the interface form.
    Private m_frmInterface As frmDetails

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
            'm_frmInterface = frmDetails
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
            'm_frmInterface = New frmDetails
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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            Select Case (m_frmInterface.Task)
                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.
                    If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Check the details havn't changed.

                        m_lReturn = m_oBusiness.Cancel()

                        If m_lReturn = gPMConstants.PMEReturnCode.PMDataChanged Then
                            ' Get string messages


                            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                            ' Check message result.
                            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                                ' Set return to false, meaning
                                ' don't cancel.
                                result = gPMConstants.PMEReturnCode.PMFalse
                            End If
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
    'Modified the values as per datatable
    'Public Function GridRead(ByVal RowBuf As TrueDBGrid.RowBuffer, ByRef vStartLocation As Object, ByVal lOffset As Integer, ByRef lApproximatePosition As Integer) As Integer
    Public Function GridRead(ByVal RowBuf As DataTable, ByRef vStartLocation As Object, ByVal lOffset As Integer, ByRef lApproximatePosition As Integer) As Integer

        Dim result As Integer = 0
        Dim iColIndex As Integer
        Dim iRowsFetched As Integer
        Dim lNewPosition As Integer
        Dim vBookmark As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iRowsFetched = 0

            ' Step through all of the rows.
            'Modified as per Datatable
            'For lRow As Integer = 0 To RowBuf.RowCount - 1
            For lRow As Integer = 0 To RowBuf.Rows.Count - 1
                ' Get the bookmark of the next available row.
                vBookmark = GetRelativeBookmark(vBookmark:=vStartLocation, lOffset:=lOffset + lRow)

                ' If the next row is BOF or EOF (Null), then
                ' stop fetching and return any rows fetched up
                ' to this point.

                If Convert.IsDBNull(vBookmark) Or IsNothing(vBookmark) Then
                    Exit For
                End If

                ' Step through all of the columns.
                'Modified as per Datatable 
                'For iCol As Integer = 0 To RowBuf.ColumnCount - 1
                For iCol As Integer = 0 To RowBuf.Columns.Count - 1
                    ' Place the record data into the row buffer.

                    'Modified as per Datatable,Todolist
                    'iColIndex = CInt(RowBuf.ColumnIndex(lRow, CShort(iCol)))
                    iColIndex = CInt(RowBuf.Columns.IndexOf(iCol))




                    'Modified as per Datatable
                    'RowBuf.Value(lRow, CShort(iCol)) = GetUserData(vBookmark:=vBookmark, iCol:=iColIndex)
                    RowBuf.Rows(lRow)(iCol) = GetUserData(vBookmark:=vBookmark, iCol:=iColIndex)
                Next iCol

                ' Set the bookmark for the row.

                'Modified as per Document,Todolist
                'RowBuf.Bookmark(lRow) = vBookmark

                ' Increment the fetched rows counter.
                iRowsFetched += 1
            Next lRow

            ' Tell the grid how many rows were fetched
            'Modified as per Datatable,Todolist
            'RowBuf.RowCount = iRowsFetched

            ' Set the approximate scroll bar position.  Only
            ' nonnegative values are valid.

            lNewPosition = IndexFromBookmark(vBookmark:=CStr(vStartLocation), lOffset:=lOffset)

            ' Check the new position is greater or equal to zero.
            If lNewPosition >= 0 Then
                lApproximatePosition = lNewPosition
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="GridRead", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GridWrite
    '
    ' Description: Used when the user modifies an existing row and
    '              attempts to commit the changes by moving to another
    '              row.
    '
    ' ***************************************************************** '
    'Modified TrueDBGrid.RowBuffer by DataTable
    'Public Function GridWrite(ByVal RowBuf As TrueDBGrid.RowBuffer, ByRef vWriteLocation As Object) As Integer
    Public Function GridWrite(ByVal RowBuf As Artinsoft.Windows.Forms.ExtendedDataGridView, ByRef vWriteLocation As Object, ByRef vWriteColumn As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Step through all of the columns of the row, storing
            ' the new values.
            'Modified the values as per datatable
            'For iCol As Integer = 0 To RowBuf.ColumnCount - 1
            'comment
            For iCol As Integer = 0 To RowBuf.Columns.Count - 1
                'Modified the values as per datatable
                'Dim auxVar As Object = RowBuf.Value(0, CShort(iCol))
                'comment
                Dim auxVar As Object = RowBuf.Rows(vWriteLocation).Cells(iCol)
                ' Dim auxVar As Object = RowBuf.CurrentCell
                'If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then
                'Modified the values as per datatable
                'Dim auxVar As Object = RowBuf.Value(0, CShort(iCol))
                'Dim auxVar As Object = RowBuf.Rows(0).Cells(iCol)
                If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then
                    'Modified the values as per datatable
                    'm_lReturn = StoreUserData(vBookmark:=vWriteLocation, iCol:=iCol, vUserVal:=RowBuf.Value(0, CShort(iCol)))
                    'm_lReturn = StoreUserData(vBookmark:=vWriteLocation, iCol:=iCol, vUserVal:=RowBuf.Rows(0).Cells(iCol))
                    '''''''
                    If vWriteLocation < 0 Or vWriteLocation > g_vGridData.GetUpperBound(1) Or iCol < 0 Or iCol > g_vGridData.GetUpperBound(0) Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    Else
                        auxVar.value = auxVar.editedformattedvalue
                        g_vGridData(iCol, vWriteLocation) = auxVar.value

                    End If
                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'comment the line as per datatable
                        'RowBuf.RowCount = 0
                    End If
                End If
                'comment
            Next iCol

            ' Update the data in the business object.

            m_lReturn = m_frmInterface.DataToBusiness(lMode:=gPMConstants.PMEComponentAction.PMEdit, lDataID:=CInt(vWriteLocation))

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="GridWrite", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GridAdd
    '
    ' Description: Used when the user adds a new row of data.
    '
    ' ***************************************************************** '
    'Public Function GridAdd(ByVal RowBuf As TrueDBGrid.RowBuffer, ByRef vNewRowBookmark As String) As Integer
    ' Public Function GridAdd(ByVal RowBuf As DataTable, ByRef vNewRowBookmark As Object) As Integer
    Public Function GridAdd(ByVal RowBuf As Artinsoft.Windows.Forms.ExtendedDataGridView, ByRef vWriteLocation As Object, ByRef vWriteColumn As Object) As Integer
        Dim result As Integer = 0
        Dim vNewVal As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a new bookmark value.
            ' vNewRowBookmark = GetNewBookmark()

            ' Step through all of the columns of the row, storing
            ' the new values.

            'For iCol As Integer = 0 To RowBuf.ColumnCount - 1
            For iCol As Integer = 0 To RowBuf.Columns.Count - 1
                'vNewVal = RowBuf.Value(0, CShort(iCol))
                'vNewVal = RowBuf.Rows(0)(iCol) '(0, CShort(iCol))
                vNewVal = RowBuf.Rows(vWriteLocation).Cells(iCol)
                ' Check if the new value is a null.
                If vWriteColumn = 0 Then
                    If String.IsNullOrEmpty(vNewVal.editedFormattedValue) And iCol = 1 Then

                        'vNewVal = m_frmInterface.grdMainData.Columns(iCol).HeaderText
                        vNewVal = m_frmInterface.lastperioddate
                    End If
                Else
                    If String.IsNullOrEmpty(vNewVal.editedFormattedValue) And iCol = 0 Then
                        'vNewVal = m_frmInterface.grdMainData.Columns(iCol).HeaderText
                        vNewVal = m_frmInterface.lastperiodname
                    End If
                End If
                'If Convert.IsDBNull(vNewVal) Or IsNothing(vNewVal) Then
                '    ' Assign a default value.


                '    'TODOLIST: 
                '    'vNewVal = m_frmInterface.grdMainData.get_Columns(iCol).DefaultValue

                '    vNewVal = m_frmInterface.grdMainData.Columns(iCol).HeaderText
                '    vNewVal = m_frmInterface.grdMainData(0, vWriteLocation)
                'End If

                ' Store the new values.
                m_lReturn = StoreUserData(vBookmark:=vWriteLocation, iCol:=iCol, vUserVal:=vNewVal)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to store the new values.
                    DeleteRow(vWriteLocation)

                    Return result
                End If
            Next iCol

            ' Update the data in the business object.
            m_lReturn = m_frmInterface.DataToBusiness(lMode:=gPMConstants.PMEComponentAction.PMAdd, lDataID:=CInt(vWriteLocation))

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            ' m_lReturn = m_frmInterface.SetGridInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add grid data", vApp:=ACApp, vClass:=ACClass, vMethod:="GridAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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

            'PN12720 - RDT 22/07/2004 - Changed the order in which the row is removed from the
            '          array/datagrid and the business object as the wrong row was being deleted.

            ' Update the data in the business object.
            m_lReturn = m_frmInterface.DataToBusiness(lMode:=gPMConstants.PMEComponentAction.PMDelete, lDataID:=CInt(vBookmark))

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Delete the row using the bookmark value.
            m_lReturn = DeleteRow(vBookmark)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                vBookmark = Nothing
                result = gPMConstants.PMEReturnCode.PMFalse
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
    Private Function IndexFromBookmark(ByRef vBookmark As String, ByRef lOffset As Integer) As Integer

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

        'TODOLIST: As per R&D
        'm_frmInterface.grdMainData.ApproxCount = g_vGridData.GetUpperBound(1)
        m_frmInterface.grdMainData.RowsCount = g_vGridData.GetUpperBound(1)

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

        lIndex = IndexFromBookmark(vBookmark:=CStr(vBookmark), lOffset:=lOffset)

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
    ' Name: GetUserData
    '
    ' Description: Gets the data from the array using the bookmark and
    '              column values.
    '
    ' ***************************************************************** '
    Private Function GetUserData(ByRef vBookmark As Object, ByRef iCol As Integer) As Object

        Dim result As Object = Nothing
        Dim lIndex As Integer



        ' Get the index value using the bookmark value.

        lIndex = IndexFromBookmark(vBookmark:=CStr(vBookmark), lOffset:=0)

        ' Check if the index value is correct.


        If lIndex < 0 Or lIndex > g_vGridData.GetUpperBound(1) Or iCol < 0 Or iCol > g_vGridData.GetUpperBound(0) Then

            Return DBNull.Value
        Else
            Return g_vGridData(iCol, lIndex)
        End If




        ' Error Section.


        result = DBNull.Value

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

            If vUserVal.GetType().Name = "String" Or vUserVal.GetType().Name = "DateTime" Then
                g_vGridData(iCol, lIndex) = vUserVal
            Else
                g_vGridData(iCol, lIndex) = vUserVal.EditedFormattedValue
            End If
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
        m_frmInterface.grdMainData.RowsCount = g_vGridData.GetUpperBound(1)

        Return result

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

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

