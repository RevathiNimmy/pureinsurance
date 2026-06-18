Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles

Module pbImportExportBinaryFileProcessing

    'These need to move to pbImportExportCommon at some point in the future

    Public Const gSTRING As String = "str"
    Public Const gINTEGER As String = "int"
    Public Const gCHAR As String = "char"
    Public Const gVARCHAR As String = "varchar"
    Public Const gDATETIME As String = "datetime"
    Public Const gTINYINT As String = "tinyint"
    Public Const gTEXT As String = "text"
    Public Const gCAPTIONTEXT As String = "captiontext"
    Public Const gMONEY As String = "money"
    Public Const gNUMERIC As String = "numeric"
    Public Const gSMALLINT As String = "smallint"
    Public Const gBIT As String = "bit"
    Public Const gDECIMAL As String = "decimal"
    Public Const gTIMESTAMP As String = "timestamp"
    Public Const gBOOLEAN As String = "Boolean"
    Public Const gFLOAT As String = "float"

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    '
    ' Name:        OpenBinaryFile
    '
    ' Description: Routine to open a binary file.  Returns the associated
    '              file Number
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' *****************************************************************
    Public Function OpenBinaryFile(ByRef i_AccessType As String, ByRef i_sFilePath As String, ByRef i_sFileName As String, ByRef i_sFileExtension As String, ByRef o_iFileNumber As Integer) As String

        'Define the require parameters
        Dim result As String = String.Empty
        Dim sFullFileName, sFilePath As String

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".OpenBinaryFile")
        Try
            result = conEmptyString

            'Build the correct filename (could make the delimiter a constant).  If the
            'name and extension are not received, assume that the path contains the full
            'path AND filename AND extension.
            If i_sFileExtension = conEmptyString And i_sFileName = conEmptyString Then
                sFullFileName = i_sFilePath
            Else
                If i_sFilePath.Length > 0 Then
                    If i_sFilePath.Substring(i_sFilePath.Length - 1) <> conBackSlash Then i_sFilePath = i_sFilePath & conBackSlash
                    sFullFileName = i_sFilePath & _
                                    i_sFileName & _
                                    conDot & _
                                    i_sFileExtension
                End If
            End If

            'Regardless of the mode of operation, check if the directory exists.  Create it
            'if it doesn't.  Need to re-define the path to check to account for occasions
            'where the file name and extensions are already included in the path.

            sFilePath = sFullFileName.Substring(0, IIf(sFullFileName = "" And conBackSlash = "", 0, (sFullFileName.LastIndexOf(conBackSlash) + 1))).Trim()

            'Call routine to check the directory exists and create if it
            'doesn't.  Will create all levels of directory structure if required.

            'Determine if a file with the same name already exists.  If it does,
            'get rid of it befroe attempting a write operation
            If i_AccessType = WriteAccess Then
                'If not a shared path
                If (sFilePath.IndexOf("\\") + 1) <> 1 And (sFilePath.IndexOf("//") + 1) <> 1 Then
                    m_lReturn = CreateDirectory(sDirectory:=sFilePath)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return CStr(gPMConstants.PMEReturnCode.PMFalse)
                    End If
                End If

                'Carry on if the file doesn't exist
                Try
                    'Delete any existing file with the same name if it exists
                    File.Delete(sFullFileName)
                Catch

                End Try

            End If

            'Get the required file number
            o_iFileNumber = FileSystem.FreeFile()

            'Save this number to a global so that the form can use number to close file if necessary.
            g_iFileNumber = o_iFileNumber

            Select Case i_AccessType
                Case WriteAccess
                    'Open for read write and lock for our use
                    FileSystem.FileOpen(o_iFileNumber, sFullFileName, OpenMode.Binary, OpenAccess.Write, OpenShare.LockReadWrite)
                Case WriteRandom
                    'Open for read write and lock for our use
                    FileSystem.FileOpen(o_iFileNumber, sFullFileName, OpenMode.Random, , , 4)

                Case Else
                    'Open for read and lock for our use
                    If gPMFunctions.FileExists(sFullFileName) Then
                        FileSystem.FileOpen(o_iFileNumber, sFullFileName, OpenMode.Binary, OpenAccess.Read, OpenShare.LockReadWrite)
                    Else
                        result = sFullFileName
                    End If
            End Select

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".OpenBinaryFile")

            Return result

        Catch ex As Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".OpenBinaryFile")
            'don't log message, error is caught in calling function
            Return result
        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name:        CloseBinaryFile
    '
    ' Description: Routine to Close a binary file.  Receives the
    '              associated file number
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Public Function CloseBinaryFile(ByVal v_iFileNumber As Integer) As Integer

        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & ".CloseBinaryFile")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Finished writing the file details, so close the file
            FileSystem.FileClose(v_iFileNumber)

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & ".CloseBinaryFile")

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".CloseBinaryFile")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseBinaryFile Failed", vApp:=ACApp, vClass:=conEmptyString, vMethod:="CloseBinaryFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        WriteBinaryFileRow
    '
    ' Description: Routine to write a row to a binary file, element by element.
    '              Receives the associated file number, an array of data
    '              and an array of defintions describing the contents of
    '              data array.
    '
    ' History:     30/08/2002 JB  - Created.
    '              03/10/2002 SJP - Modified to be a function, so can
    '                               pass back if successful or not.  Error
    '                               handling introduced.
    '
    ' ***************************************************************** '
    Public Function WriteBinaryFileRow(ByRef i_iFileNumber As Integer, ByVal i_iObjectType As Integer, ByRef i_aDataDefinition() As Object, _
                                       ByRef i_aData(,) As Object, ByVal rowIndex As Integer, ByRef r_lTotalLinesWritten As Integer) As Integer

        Dim result As Integer = 0
        Dim sPreparedStringData As String = ""
        Dim lPreparedLongData As Integer
        Dim cPreparedCurrencyData As Decimal

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & ".WriteBinaryFileRow")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Assume(!) that object type needs no preparation
            m_lReturn = WriteIntegerData(i_iFileNumber:=i_iFileNumber, i_aData:=CInt(i_iObjectType), i_aDataDefinition:=0)

            'The following needs to be repeated for each element in the data
            'definition array
            Dim dData As Object
            For iElementCount As Integer = 0 To i_aDataDefinition.GetUpperBound(0)

                'Determine the type of data to be written to the file,
                'assuming that the type is in a fixed position of 1
                'and write the individual elements of the row to the file.
                '**usse constant for this

                Select Case i_aDataDefinition(iElementCount)(pbIeTableDefinitions_columnType)
                    'must be oe of  string /char / varchar
                    Case gSTRING, gCHAR, gVARCHAR, gTEXT, gCAPTIONTEXT

                        'Convert the data to appropriate format

                        m_lReturn = PrepareStringDataPreExport(vInputData:=CStr(i_aData(iElementCount, rowIndex)), sOutputString:=sPreparedStringData, v_vPbIeControl_Flags:=g_aIeControl(i_iObjectType)(pbIeControl_Flags))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Write the prepared data to the file
                        m_lReturn = WriteStringData(i_iFileNumber:=i_iFileNumber, i_aData:=sPreparedStringData, i_aDataDefinition:=0)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'must be one of Integer / Tiny /
                    Case gBIT, gBOOLEAN
                        'Convert the data to appropriate format

                        m_lReturn = PrepareBitDataPreExport(vInputData:=(i_aData(iElementCount, rowIndex)), lOutputLong:=lPreparedLongData)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Write the prepared data to the file
                        m_lReturn = WriteIntegerData(i_iFileNumber:=i_iFileNumber, i_aData:=lPreparedLongData, i_aDataDefinition:=0)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'must be one of Integer / Tiny /
                    Case gINTEGER, gTINYINT, gSMALLINT

                        'Convert the data to appropriate format

                        m_lReturn = PrepareLongDataPreExport(vInputData:=(i_aData(iElementCount, rowIndex)), lOutputLong:=lPreparedLongData)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Write the prepared data to the file
                        m_lReturn = WriteIntegerData(i_iFileNumber:=i_iFileNumber, i_aData:=lPreparedLongData, i_aDataDefinition:=0)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    Case gTIMESTAMP
                        'don't do anything

                        'must be one of Money / numeric /
                    Case gMONEY, gNUMERIC, gDECIMAL, gFLOAT

                        'Convert the data to appropriate format

                        m_lReturn = PrepareCurrencyDataPreExport(vInputData:=i_aData(iElementCount, rowIndex), cOutputCurrency:=cPreparedCurrencyData)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Write the prepared data to the file
                        m_lReturn = WriteFloatingPointData(i_iFileNumber:=i_iFileNumber, i_aData:=cPreparedCurrencyData, i_aDataDefinition:=0)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Case gDATETIME

                        If IsDBNull(i_aData(iElementCount, rowIndex)) Or IsNothing(i_aData(iElementCount, rowIndex)) Then
                            dData = DefaultCurrencyValue
                        Else

                            dData = CDate(i_aData(iElementCount, rowIndex)).ToOADate
                        End If

                        FileSystem.FilePutObject(i_iFileNumber, dData)

                    Case Else
                        'All the other bits end up in here

                        writeToStatusBox("Unknown data type " & CStr(g_aIeControl(i_iObjectType)(pbIeControl_objectName)) & ":" & CStr(i_aDataDefinition(iElementCount)(pbIeTableDefinitions_columnType)))
                End Select

            Next

            'Convert the data to appropriate format
            m_lReturn = PrepareLongDataPreExport(vInputData:=EndOfLineChar, lOutputLong:=lPreparedLongData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Write the end of line identifier as the last element
            m_lReturn = WriteIntegerData(i_iFileNumber:=i_iFileNumber, i_aData:=EndOfLineChar, i_aDataDefinition:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_lTotalLinesWritten += 1

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ".WriteBinaryFileRow")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ".WriteBinaryFileRow")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteBinaryFileRow Failed", vApp:=ACApp, vClass:=conEmptyString, vMethod:="WriteBinaryFileRow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        WriteStringData
    '
    ' Description: Routine to write STRING data to a binary file.
    '
    ' History:     30/08/2002 JB  - Created.
    '              03/10/2002 SJP - Changed to function, so that success
    '                               status can be passed back, error
    '                               handling introduced.
    '
    ' ***************************************************************** '
    Public Function WriteStringData(ByRef i_iFileNumber As Integer, ByVal i_aData As String, ByRef i_aDataDefinition As Object) As Integer

        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & ".WriteStringData")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Write the line to the current binary file.  Position isn't
            'specified so it will get appened at the current position of the
            'file pointer.  Length of the element must be written prior to the
            'data itself
            If Not (i_aData Is Nothing) Then
                i_aData = i_aData.Trim()
            End If

            'Write the data length to the file
            FileSystem.FilePut(i_iFileNumber, i_aData.Length)

            'Write the data to the file
            FileSystem.FilePut(i_iFileNumber, i_aData)

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ".WriteStringData")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ".WriteStringData")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteStringData Failed", vApp:=ACApp, vClass:=conEmptyString, vMethod:="WriteStringData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        WriteIntegerData
    '
    ' Description: Routine to write NUMERIC data to a binary file.
    '              We currently receive the data definition for the #
    '              field to be written, but we don't need it at this point
    '              Left in for future use!
    '
    ' History:     30/08/2002 JB  - Created.
    '              03/10/2002 SJP - Changed to function, so that success
    '                               status can be passed back, error
    '                               handling introduced.
    '
    ' ***************************************************************** '
    Public Function WriteIntegerData(ByRef i_iFileNumber As Integer, ByRef i_aData As Integer, ByRef i_aDataDefinition As Object) As Integer

        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & ".WriteIntegerData")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'NB - When writing the line to the current binary file, position
            'isn't specified so it will get appened at the current position
            'of the file pointer.

            FileSystem.FilePutObject(i_iFileNumber, i_aData)

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ".WriteIntegerData")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ".WriteIntegerData")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteIntegerData Failed", vApp:=ACApp, vClass:=conEmptyString, vMethod:="WriteIntegerData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        WriteFloatData
    '
    ' Description: Routine to write FLOATING POINT data to a binary file.
    '              We currently receive the data definition for the #
    '              field to be written, but we don't need it at this point
    '              Left in for future use!
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Public Function WriteFloatingPointData(ByRef i_iFileNumber As Integer, ByRef i_aData As Decimal, ByRef i_aDataDefinition As Object) As Integer

        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".WriteFloatingPointData")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'NB - When writing the line to the current binary file, position
            'isn't specified so it will get appened at the current position
            'of the file pointer.

            FileSystem.FilePutObject(i_iFileNumber, i_aData)

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".WriteFloatingPointData")

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".WriteFloatingPointData")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteFloatingPointData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteFloatingPointData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        StopProcessing
    '
    ' Description: Stops further export processing if user hits cancel
    '
    ' History:     02/10/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Public Function StopProcessing(ByVal v_iFileNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim sFullFileName, sFilePath, sFileName, sFileExtension As String

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".StopProcessing")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With objFrmMainForm
                sFilePath = .txtFilePath(g_iImportExport).Text
                sFileName = .txtFileName(g_iImportExport).Text
                sFileExtension = .txtFileExtension(g_iImportExport).Text

                If sFileName.Length = 0 And sFileExtension.Length = 0 Then
                    sFullFileName = sFilePath
                Else
                    sFullFileName = sFilePath & sFileName & conDot & sFileExtension
                End If
            End With

            'Close file so that it can be removed altogether.
            If Not (v_iFileNumber = 0) Then
                m_lReturn = CType(CloseBinaryFile(v_iFileNumber:=v_iFileNumber), gPMConstants.PMEReturnCode)
                writeToStatusBox("Closing export file.")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Will destroy file if it has been created, else will be caught by error trap
            ' if file has not been created yet, during this export.
            File.Delete(sFullFileName)
            If Information.Err().Number = 0 Then
                writeToStatusBox("Export file deleted.")
            Else
                Information.Err().Clear()
            End If

            Try

                writeToStatusBox("Export process has been cancelled.")

                Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".StopProcessing")

                Return result

            Catch excep As System.Exception

                result = gPMConstants.PMEReturnCode.PMError

                Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".StopProcessing")

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StopProcessing Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StopProcessing", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result

            End Try

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        GetBinaryFileRow
    '
    ' Description: Routine to retrieve a row to a binary file, element by element.
    '              Receives the associated file number, an array in which to
    '              store the data and an array of defintions describing the
    '              contents of the file row under investigation.
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Public Function GetBinaryFileRow(ByVal v_iFileNumber As Integer, ByVal v_vObjectType As Integer, ByRef r_aDataDefinition() As Object, ByRef aRetrievedData() As Object, ByRef rowIndex As Object) As Integer

        'Define the local variables
        Dim result As Integer = 0
        Dim iNumRowElements As Integer

        Dim sRetrievedStringData As String = ""
        Dim iRetrievedIntegerData As Object

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".GetBinaryFileRow")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'objFrmMainForm.ProgressBar1(1).value = 100 / ((UBound(vResults, 2) + 1) / (lResultIndex + 1))

            'Determine the number of elements in the row
            iNumRowElements = r_aDataDefinition.GetUpperBound(0)

            'Re-size the output array to be the correct size
            ReDim aRetrievedData(iNumRowElements)

            'The following needs to be repeated for each element in the data
            'definition array

            For iElementCount As Integer = 0 To iNumRowElements

                'Determine the type of data to be retrieved from file,
                'assuming that the type is in a fixed position of 1
                'and write the individual elements of the row to the file.

                Select Case r_aDataDefinition(iElementCount)(pbIeTableDefinitions_columnType)
                    'must be a string /char / varchar
                    Case gSTRING, gCHAR, gVARCHAR, gTEXT, gCAPTIONTEXT

                        'Retrieve the data from the file
                        m_lReturn = CType(GetStringData(iFileNumber:=v_iFileNumber, sDataElement:=sRetrievedStringData, aDataDefinition:=v_vObjectType), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'The data element has been retrieved, so it must be saved to the
                        'output array

                        aRetrievedData(iElementCount) = PrepareStringDataPostImport(sInputData:=sRetrievedStringData)

                        'must be an BIT
                    Case gBIT, gBOOLEAN

                        'Retrieve the data from the file
                        GetLongData(iFileNumber:=v_iFileNumber, lDataElement:=iRetrievedIntegerData, i_aDataDefinition:=v_vObjectType)

                        'The data element has been retrieved, so it must be saved to the
                        'output array

                        aRetrievedData(iElementCount) = PrepareBitDataPostImport(lInputData:=iRetrievedIntegerData)

                    Case gTIMESTAMP
                        'don't do anything

                        'must be an Integer / Tiny /
                    Case gINTEGER, gTINYINT, gSMALLINT

                        'Retrieve the data from the file
                        GetLongData(iFileNumber:=v_iFileNumber, lDataElement:=iRetrievedIntegerData, i_aDataDefinition:=v_vObjectType)

                        'The data element has been retrieved, so it must be saved to the
                        'output array

                        aRetrievedData(iElementCount) = PrepareLongDataPostImport(lInputData:=iRetrievedIntegerData)

                    Case gDATETIME

                        Dim dData As Object
                        FileSystem.FileGetObject(v_iFileNumber, dData, -1)

                        If gPMFunctions.ToSafeDouble(dData) = DefaultCurrencyValue Then

                            aRetrievedData(iElementCount) = DBNull.Value
                        Else

                            aRetrievedData(iElementCount) = gPMFunctions.ToSafeDouble(dData)
                        End If

                        'must be one of Money / numeric /
                    Case gMONEY, gNUMERIC, gDECIMAL, gFLOAT

                        Dim cData As Object
                        FileSystem.FileGetObject(v_iFileNumber, cData, -1)

                        If gPMFunctions.ToSafeDecimal(cData) = DefaultCurrencyValue Then

                            aRetrievedData(iElementCount) = DBNull.Value
                        Else

                            aRetrievedData(iElementCount) = gPMFunctions.ToSafeDecimal(cData)
                        End If

                    Case Else
                        'All the other bits end up in here
                        MessageBox.Show("Cannot detect what is going on with the supplied type", Application.ProductName)

                End Select

            Next

            'Attempt to retrieve the last element of the row
            GetLongData(iFileNumber:=v_iFileNumber, lDataElement:=iRetrievedIntegerData, i_aDataDefinition:=v_vObjectType)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                iRetrievedIntegerData = EndOfLineChar
            End If
            '** perform a check to say that the file file / line
            '** maybe corrupt as the last character is not the check
            '** character expected
            If gPMFunctions.ToSafeInteger(iRetrievedIntegerData) <> EndOfLineChar Then
                MessageBox.Show("missed the end of line character. " & iRetrievedIntegerData, Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".GetBinaryFileRow")

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".GetBinaryFileRow")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBinaryFileRow Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBinaryFileRow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        GetStringData
    '
    ' Description: Routine to retrieve STRING data from the binary file.
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Public Function GetStringData(ByRef iFileNumber As Integer, ByRef sDataElement As String, ByRef aDataDefinition As Object) As Integer

        Dim result As Integer = 0
        Dim lStringLength As Integer = 0

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".GetStringData")

            result = gPMConstants.PMEReturnCode.PMTrue

            '***************************************************************
            'Assume the pointer is in the correct position for the retreival
            '***************************************************************

            'Retrieve the data length to the file
            If FileSystem.EOF(iFileNumber) <> True Then
                FileSystem.FileGet(iFileNumber, lStringLength, -1)
            End If
            'lStringLength = FileSystem.FileLen(FileSystem.CurDir())
            'Resize the data element as appropriate
            '  If lStringLength > 0 Then
            sDataElement = New String("a", lStringLength)
            sDataElement = sDataElement.Trim()
            'Retrieve the next 'chunk' of data from the file

            'FileSystem.FileGetObject(iFileNumber, sDataElement, -1)
            FileSystem.FileGet(FileNumber:=iFileNumber, Value:=sDataElement, RecordNumber:=-1)

            '  End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".GetStringData")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".GetStringData")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStringData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStringData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        GetLongData
    '
    ' Description: Function to retrieve NUMERIC data from the binary file.
    '              We currently receive the data definition for the #
    '              field to be written, but we don't need it at this point
    '              Left in for future use!
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Public Function GetLongData(ByRef iFileNumber As Integer, ByRef lDataElement As Object, ByRef i_aDataDefinition As Object) As Integer

        Dim result As Integer = 0
        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".GetLongData")

            '***************************************************************
            'Assume the pointer is in the correct position for the retreival
            '***************************************************************

            'Attempt to read the next long

            If FileSystem.EOF(iFileNumber) <> True Then
                FileSystem.FileGetObject(iFileNumber, lDataElement, -1)
            End If

            'Check if the read has failed, if so then there is a very good
            'chance that were at the end of the file and need to stop
            If FileSystem.EOF(iFileNumber) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".GetLongData")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".GetLongData")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLongData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLongData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name:        GetFloatingPointData
    '
    ' Description: Routine to retriev FLOATING POINT data from the binary file.
    '              We currently receive the data definition for the #
    '              field to be written, but we don't need it at this point
    '              Left in for future use!
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Public Function GetFloatingPointData(ByRef iFileNumber As Integer, ByRef cDataElement As Decimal, ByRef i_aDataDefinition As Object) As Integer

        Dim result As Integer = 0
        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".GetFloatingPointData")

            result = gPMConstants.PMEReturnCode.PMTrue

            '***************************************************************
            'Assume the pointer is in the correct position for the retreival
            '***************************************************************

            If FileSystem.EOF(iFileNumber) <> True Then
                FileSystem.FileGet(iFileNumber, cDataElement, -1)
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".GetFloatingPointData")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".GetFloatingPointData")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFloatingPointData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFloatingPointData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '****************** EXPORT PREPARATION ROWS ARE BELOW HERE *******************

    ' ***************************************************************** '
    '
    ' Name:        PrepareStringDataPreExport
    '
    ' Description: Routine to receive variant data and prepare it prior
    '              to writing to the binary file
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Private Function PrepareStringDataPreExport(ByRef vInputData As Object, ByRef sOutputString As String, ByVal v_vPbIeControl_Flags As Object) As Integer

        Dim result As Integer = 0
        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".PrepareStringDataPreExport")

            result = gPMConstants.PMEReturnCode.PMTrue

            'If the received variant is null, populate with a default value

            If IsDBNull(vInputData) Or IsNothing(vInputData) Then
                vInputData = DefaultStringValue
            End If

            'Convert the variant to a string
            sOutputString = Convert.ToString(vInputData)

            'Trim the new string

            If CBool(v_vPbIeControl_Flags And pbIeControl_Flags__dontTrimStrings) Then
            Else
                If Not (sOutputString Is Nothing) Then
                    sOutputString = sOutputString.Trim()
                End If
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".PrepareStringDataPreExport")

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name:        PrepareLongDataPreExport
    '
    ' Description: Routine to receive variant data and prepare it prior
    '              to writing to the binary file
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Private Function PrepareLongDataPreExport(ByRef vInputData As Object, ByRef lOutputLong As Integer) As Integer

        Dim result As Integer = 0
        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".PrepareLongDataPreExport")

            result = gPMConstants.PMEReturnCode.PMTrue

            'If the received variant is null, populate with a default value

            If IsDBNull(vInputData) Or IsNothing(vInputData) Then
                vInputData = DefaultIntegerValue
            End If
            'Convert the variant to a string
            lOutputLong = CInt(vInputData)

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".PrepareLongDataPreExport")

            Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name:        PrepareBitDataPreExport
    '
    ' Description: Routine to receive variant data and prepare it prior
    '              to writing to the binary file
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Private Function PrepareBitDataPreExport(ByRef vInputData As Object, ByRef lOutputLong As Integer) As Integer

        Dim result As Integer = 0
        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".PrepareBitDataPreExport")

            result = gPMConstants.PMEReturnCode.PMTrue

            'If the received variant is null, populate with a default value

            If CBool(IsDBNull(vInputData) Or IsNothing(vInputData)) Then
                vInputData = DefaultIntegerValue
            End If

            If vInputData = "False" Or vInputData = "True" Then
                vInputData = Convert.ToBoolean(vInputData)
            End If

            'If Not vInputData Then
            '    vInputData = 0
            'End If
            'If vInputData Then
            '    vInputData = 1
            'End If
            'Convert the variant to a string
            lOutputLong = gPMFunctions.ToSafeInteger(vInputData)

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".PrepareBitDataPreExport")

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name:        PrepareCurrencyDataPreExport
    '
    ' Description: Routine to receive variant data and prepare it prior
    '              to writing to the binary file
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Private Function PrepareCurrencyDataPreExport(ByRef vInputData As Object, ByRef cOutputCurrency As Decimal) As Integer

        Dim result As Integer = 0
        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".PrepareCurrencyDataPreExport")

            result = gPMConstants.PMEReturnCode.PMTrue

            'If the received variant is null, populate with a default value

            If IsDBNull(vInputData) Or IsNothing(vInputData) Then
                vInputData = DefaultCurrencyValue
            End If

            'Convert the variant to a string
            cOutputCurrency = Convert.ToDecimal(vInputData)

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".PrepareCurrencyDataPreExport")

            Return result

    End Function

    '****************** IMPORT PREPARATION ROWS ARE BELOW HERE *******************

    ' ***************************************************************** '
    '
    ' Name:        PrepareStringDataPostImport
    '
    ' Description: Routine to receive variant data and Prepare it after
    '              retrieving from the binary file
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '

    'Private Function PrepareStringDataPostImport(ByRef sInputData As String) As Double
    Private Function PrepareStringDataPostImport(ByRef sInputData As Object) As Object

        Dim result As Object = ""
        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".PrepareStringDataPostImport")

            'If the received integer is set to the default value, replace with real null
            If gPMFunctions.ToSafeString(sInputData) = DefaultStringValue Then

                result = DBNull.Value
            Else

                If Not (sInputData Is Nothing) Then
                    result = sInputData.Trim()
                End If
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".PrepareStringDataPostImport")

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name:        PrepareIntegerDataPreImport
    '
    ' Description: Routine to receive variant data and prepare it after
    '              retrieving from the binary file
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Private Function PrepareLongDataPostImport(ByRef lInputData As Object) As Object

        Dim result As Object
        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".PrepareLongDataPostImport")

            'If the received integer is set to the default value, replace with real null
            If gPMFunctions.ToSafeInteger(lInputData) = DefaultIntegerValue Then

                result = DBNull.Value
            Else
                result = lInputData
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".PrepareLongDataPostImport")

            Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name:        PrepareBitDataPreImport
    '
    ' Description: Routine to receive variant data and prepare it after
    '              retrieving from the binary file
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Private Function PrepareBitDataPostImport(ByRef lInputData As Object) As Object

        Dim result As Object
        

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".PrepareBitDataPostImport")

            'If the received integer is set to the default value, replace with real null
            If gPMFunctions.ToSafeInteger(lInputData) = DefaultIntegerValue Then

                result = DBNull.Value
            Else
                result = lInputData
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".PrepareBitDataPostImport")

            Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name:        CreateDirectory
    '
    ' Description: This procedure creates the directory where the file(s)
    '              are to be installed. There is some error handling in
    '              it incase the directory already exists.
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Friend Function CreateDirectory(ByRef sDirectory As String) As Integer

        Dim result As Integer = 0
        Dim sPath As String = ""

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".CreateDirectory")

            result = gPMConstants.PMEReturnCode.PMTrue

            If Right(sDirectory, 1) <> "\" Then
                sDirectory += "\"
            End If

            Dim strOldPath As String = Directory.GetCurrentDirectory() 'Returns the CurDir to the old path(the dir 'Find the current Directory...
            Dim intAnchor As Integer = 0 'Equal to the above variable... 'Reset intAnchor...

            'Searches for the "\" to create the dirs properly...
            Dim intOffset As Integer = Strings.InStr(intAnchor + 1, sDirectory, "\") 'Searches for a "\" so it can create the dirs...
            intAnchor = intOffset 'Equal to the above...

            'Loop looking for the directory separator
            Do
                intOffset = Strings.InStr(intAnchor + 1, sDirectory, "\")
                intAnchor = intOffset

                If intAnchor > 0 Then 'If there is 1 or more "\" then...

                    'Create the directory using the text before the "\"...
                    sPath = sDirectory.Substring(0, intOffset - 1)

                    ' Determine if this directory already exists...
                    Try
                        Directory.SetCurrentDirectory(sPath) 'If it does, change to that directory...
                    Catch dnfe As DirectoryNotFoundException
                        Directory.CreateDirectory(sPath) 'Make the Directory...
                    End Try
                End If
            Loop Until intAnchor = 0 'Loop until all directories have been made            

            Directory.SetCurrentDirectory(strOldPath) 'Change back to the the 'old' current directory...

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".CreateDirectory")

            Return result

        Catch exc As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".CreateDirectory")
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDirectory", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=exc)
            Return result

        End Try

    End Function
End Module
