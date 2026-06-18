Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("bPMUListCreate_NET.bPMUListCreate")> _
Public NotInheritable Class bPMUListCreate
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: ListCreate
    '
    ' Date: 30/03/2000
    '
    ' Description: Generates a list file(.dat) and index(.idx) from a
    '              specified raw data file
    '
    ' Edit History:
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 19/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    Private Const ACClass As String = "bPMUListCreate"

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date


    Private m_sInput As String = ""
    Private m_sOutputFile As String = ""
    Private m_iRLDFFile As Integer
    Private m_lRLDFRecord As Integer
    Private m_vPropertyArray(,) As Object
    Private m_lPropertyArrayCnt As Integer
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_sOutputFileDataModel As String = ""
    Private m_sOutputFilePath As String = ""

    Private m_sOutputFileName As String = ""

    Private Const m_sOUTPUT_FILE_DESC As String = "List"

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property

    Public Property OutputFilePath() As String
        Get
            Return m_sOutputFilePath
        End Get
        Set(ByVal Value As String)
            m_sOutputFilePath = Value
            If Not m_sOutputFilePath.EndsWith("\") Then
                m_sOutputFilePath = m_sOutputFilePath & "\"
            End If
        End Set
    End Property

    Public Property OutputFileDataModel() As String
        Get
            Return m_sOutputFileDataModel
        End Get
        Set(ByVal Value As String)
            m_sOutputFileDataModel = Value
        End Set
    End Property


    Public Property InputFile() As String
        Get
            Return m_sInput
        End Get
        Set(ByVal Value As String)
            m_sInput = Value
        End Set
    End Property


    Public Property OutputFile() As String
        Get
            Return m_sOutputFile
        End Get
        Set(ByVal Value As String)
            m_sOutputFile = Value
        End Set
    End Property


    ' Public method - Create .dat and .idx files
    Public Function Create() As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sFilename As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Has an input file been specified
            If m_sInput = "" Then
                lResult = gPMConstants.PMEReturnCode.PMFail
            Else
                ' Make sure the specified input file exists
                If FileSystem.Dir(m_sInput, FileAttribute.Normal) = "" Then
                    lResult = gPMConstants.PMEReturnCode.PMFail
                Else
                    If CheckForOutputDir() <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Determine the output file name, if not already set
                    If m_sOutputFile = "" Then
                        lResult = CType(CreateOutputName(), gPMConstants.PMEReturnCode)
                        If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                    'Archive existing live files.
                    m_lReturn = ArchiveRLDF()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Build DAT and IDX files
                    lResult = CType(BuildOutputDatFile(), gPMConstants.PMEReturnCode)
                    If lResult = gPMConstants.PMEReturnCode.PMTrue Then
                        lResult = CType(BuildOutputIdxFile(), gPMConstants.PMEReturnCode)
                    End If
                End If
            End If

            Return lResult

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Create Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Create", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function





    'Private Function ExtractOutputName(ByRef sFilename As String) As String
    '
    'Dim iSuffixStart As Integer = (sFilename.IndexOf("."c) + 1)
    'If iSuffixStart > 0 Then
    'Return sFilename.Substring(0, iSuffixStart - 1)
    'Else
    'Return sFilename
    'End If
    'End Function

    Private Function BuildOutputDatFile() As Integer
        Dim result As Integer = 0
        Dim iInputFile As Integer
        Dim sInputRecord As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim m_vPropertyArray(2, 0)
            m_lRLDFRecord = 1

            m_iRLDFFile = FileSystem.FreeFile()
            ' Open the output file
            FileSystem.FileOpen(m_iRLDFFile, m_sOutputFile & ".Dat", OpenMode.Random)


            If FileSystem.Dir(m_sInput, FileAttribute.Normal) <> "" Then
                ' Open and process the input file
                iInputFile = FileSystem.FreeFile()
                FileSystem.FileOpen(iInputFile, m_sInput, OpenMode.Binary)

                lReturn = gPMConstants.PMEReturnCode.PMTrue
                While Not FileSystem.EOF(iInputFile) And lReturn = gPMConstants.PMEReturnCode.PMTrue
                    sInputRecord = FileSystem.LineInput(iInputFile)
                    If sInputRecord <> "" Then
                        lReturn = CType(ProcessInputRecord(v_sInputRecord:=sInputRecord), gPMConstants.PMEReturnCode)
                    End If
                    Application.DoEvents()
                End While
                FileSystem.FileClose(iInputFile)
            End If
        Catch excep As System.Exception


            ' Error.
            lReturn = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="BuildOutputDatFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildOutputDatFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


        Finally
            FileSystem.FileClose(m_iRLDFFile)
        End Try


        Return result
    End Function



    Private Function ProcessInputRecord(ByRef v_sInputRecord As String) As Integer
        Dim uHeaderRecord As MainModule.RLDFHeaderRecord = New MainModule.RLDFHeaderRecord
        Dim uDetailrecord As MainModule.RLDFDetailRecord = New MainModule.RLDFDetailRecord
        Dim sDescription, sABICode, sPropertyId As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Static sSavePropertyId As String = ""



        lReturn = gPMConstants.PMEReturnCode.PMTrue

        sPropertyId = Mid(v_sInputRecord, 1, 11).Trim()
        sABICode = Mid(v_sInputRecord, 84).Trim()
        sDescription = Mid(v_sInputRecord, 13, 70).Trim()

        If sSavePropertyId = "" Or sSavePropertyId <> sPropertyId Then

            ReDim Preserve m_vPropertyArray(2, m_lPropertyArrayCnt)
            m_vPropertyArray(ACPAPropertyId, m_lPropertyArrayCnt) = sPropertyId
            m_vPropertyArray(ACPARecordIndex, m_lPropertyArrayCnt) = m_lRLDFRecord
            m_vPropertyArray(ACPACustom, m_lPropertyArrayCnt) = False
            m_lPropertyArrayCnt += 1

            uHeaderRecord.PropertyId = sPropertyId & Space(10 - sPropertyId.Length)
            FileSystem.FilePut(m_iRLDFFile, uHeaderRecord.PropertyId & uHeaderRecord.Filler, m_lRLDFRecord, StringIsFixedLength:=True)

            m_lRLDFRecord += 1

            sSavePropertyId = sPropertyId

        End If

        uDetailrecord.ABICode = sABICode & Space(10 - sABICode.Length)
        uDetailrecord.PropertyId = sPropertyId & Space(10 - sPropertyId.Length)
        uDetailrecord.Description = sDescription & Space(70 - sDescription.Length)


        FileSystem.FilePut(m_iRLDFFile, uDetailrecord.PropertyId & uDetailrecord.Description & uDetailrecord.ABICode, m_lRLDFRecord, StringIsFixedLength:=True)
        m_lRLDFRecord += 1

        Return lReturn

    End Function




    Private Function BuildOutputIdxFile() As Integer
        Dim result As Integer = 0
        Dim iIndexFile As Integer
        Dim sPropertyId As String = ""
        Dim lRecordNumber As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMSucceed


            Dim uindexrecord As MainModule.RLDFIndexRecord = MainModule.RLDFIndexRecord.CreateInstance()
            Dim i As Integer

            i = 0

            iIndexFile = FileSystem.FreeFile()

            ' Open the index file
            If FileSystem.Dir(m_sOutputFile & ".Idx", FileAttribute.Normal) <> "" Then
                Try
                    File.Delete(m_sOutputFile & ".Idx")

                Catch
                End Try

            End If
            FileSystem.FileOpen(iIndexFile, m_sOutputFile & ".Idx", OpenMode.Random)

            ' Loop arround the arrray writing records
            For i = 0 To m_vPropertyArray.GetUpperBound(1)

                sPropertyId = CStr(m_vPropertyArray(ACPAPropertyId, i))
                lRecordNumber = CInt(m_vPropertyArray(ACPARecordIndex, i))

                uindexrecord.PropertyId = sPropertyId & Space(10 - sPropertyId.Length)
                uindexrecord.RecordNumber = lRecordNumber
                FileSystem.FilePut(iIndexFile, uindexrecord, i + 1)
            Next i
            result = gPMConstants.PMEReturnCode.PMTrue


            'Done_BuildOutputIdxFile:
            ' close the index file
            'FileSystem.FileClose(iIndexFile)

            'Return result
        Catch ex As Exception

            'Err_BuildOutputIdxFile:
            ' Error.
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="BuildOutputIdxFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildOutputIdxFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            Return result
        Finally
            FileSystem.FileClose(iIndexFile)
        End Try
        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDataModels
    '
    ' Description:
    '
    ' History: 28/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetDataModels(ByRef vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim oDatabase As dPMDAO.Database

        'Const sGET_GIS_DATA_MODELS As String = "{call spu_GIS_data_model_saa (?)}"
        Const sGET_GIS_DATA_MODELS As String = "spu_GIS_data_model_saa"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            '    sSQL = "SELECT gis_data_model_id, description, code " _
            ''            & "FROM GIS_data_model " _
            ''            & "WHERE is_deleted <> 1 " _
            ''            & "AND effective_date < " & Date



            m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=oDatabase), gPMConstants.PMEReturnCode)


            oDatabase.Parameters.Clear()

            m_lReturn = oDatabase.Parameters.Add("Date", DateTimeHelper.ToString(DateTime.Today), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

            ' Execute SQL Statement
            m_lReturn = oDatabase.SQLSelect(sSQL:=sGET_GIS_DATA_MODELS, sSQLName:="", bStoredProcedure:=True, vResultArray:=vResultArray)

            oDatabase = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataModels Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataModels", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateReleaseVersion
    '
    ' Description:
    '
    ' History: 25/10/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateReleaseVersion() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateReleaseVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateReleaseVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateOutputName
    '
    ' Description: Create output file name from details passed in.
    '
    ' History: 26/10/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CreateOutputName() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Start with path which has been passed in and should have been
        'retrieved from registry.
        m_sOutputFile = m_sOutputFilePath
        If Not m_sOutputFile.EndsWith("\") Then
            m_sOutputFile = m_sOutputFile & "\"
        End If

        'The file name itself.
        m_sOutputFileName = m_sOutputFileDataModel & m_sOUTPUT_FILE_DESC

        'Finally the common part of list file names.
        m_sOutputFile = m_sOutputFile & m_sOutputFileName

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckForOutputDir
    '
    ' Description:
    '
    ' History: 26/10/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CheckForOutputDir() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If Not Directory.Exists(m_sOutputFilePath) Then
            Directory.CreateDirectory(m_sOutputFilePath)
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: ArchiveRLDF (Private)
    '
    ' Description: Archives a copy of the RLDF

    ' ***************************************************************** '
    Private Function ArchiveRLDF() As Integer
        Dim result As Integer = 0
        Dim sSequenceNumber, sSaveServerListFilePath As String



        result = gPMConstants.PMEReturnCode.PMTrue

        '******************************************************************
        ' sequence number is built from the date and time
        '******************************************************************
        sSequenceNumber = DateTime.Today.ToString("yyyyMMdd") & DateTimeHelper.Time.ToString("HHmmss")

        sSaveServerListFilePath = m_sOutputFilePath & sSequenceNumber '& "\" & m_sOutputFileName & ".dat"

        '******************************************************************
        ' Rename the existing RLDF
        '******************************************************************
        If FileSystem.Dir(m_sOutputFile & ".dat", FileAttribute.Normal) <> "" Then
            Directory.CreateDirectory(sSaveServerListFilePath)
            FileSystem.FileCopy(m_sOutputFile & ".dat", sSaveServerListFilePath & "\" & m_sOutputFileName & ".dat")
            FileSystem.FreeFile()
            File.Delete(m_sOutputFile & ".dat")
        End If
        If FileSystem.Dir(m_sOutputFile & ".idx", FileAttribute.Normal) <> "" Then
            FileSystem.FileCopy(m_sOutputFile & ".idx", sSaveServerListFilePath & "\" & m_sOutputFileName & ".idx")
            FileSystem.FreeFile()
            File.Delete(m_sOutputFile & ".idx")
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub

End Class
