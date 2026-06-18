Option Strict Off
Option Explicit On
Imports System.IO
Imports System.Runtime.InteropServices
'Developer Guide no 129
Imports SSP.Shared


<System.Runtime.InteropServices.ProgId("Maintain_NET.Maintain")>
Public NotInheritable Class Maintain

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

    ' Maintain.cls
    ' ------------
    ' This class module is used for maintenance purposes for creating the data file

    Private m_IndexRecord As MainModule.Lookup_Header_Index = MainModule.Lookup_Header_Index.CreateInstance()
    Private m_HeaderRecord As MainModule.Lookup_Header = MainModule.Lookup_Header.CreateInstance()
    Private m_DataRecord As MainModule.Lookup_Data = MainModule.Lookup_Data.CreateInstance()
    Private m_oDatabase As Object = Nothing
    'Private oDatabaseObject As dPMDAO.Database
    Private m_vLookup As Object
    Private m_lReturn As Integer
    Private m_sRequiredBusinessType As String = ""
    Private m_sModelCode As String = ""
    Private lFileHandleI As Integer
    Private lFileHandleH As Integer
    Private lFileHandleD As Integer
    Private lNumIndexRecords As Integer


    Public Function initialise(ByRef oDatabase As Object) As Integer

        'set database to use
        m_oDatabase = oDatabase

        If m_oDatabase Is Nothing Then
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            Return gPMConstants.PMEReturnCode.PMTrue
        End If

    End Function





    Public Function BuildLookupCacheFiles(ByRef sModelCode As String, ByRef sOpenFileBusinessType As String, ByRef lLimitToSpecifiedBusinessType As Integer) As Integer

        Dim result As Integer = 0
        Dim vLookupNames As Object = Nothing
        Dim vHeaderDetails As Object = Nothing
        Dim vData(,) As Object = Nothing


        Dim sLookupName, sEffectiveDate, sModifiedDate As String
        Dim lStatus As Integer
        Dim sDefinition, sValidConstants, sDefaultValue As String
        Dim lInsurer_id As Integer
        Dim sBusinessType As String = ""
        Dim lHeaderKey As Integer
        Dim sLevel, sLevelValue, sType As String


        Dim lIndexFilePtr As Integer = 1
        Dim lHeaderFilePtr As Integer = 1
        Dim lDataFilePtr As Integer = 1

        result = gPMConstants.PMEReturnCode.PMTrue

        'open temp cache files for output
        m_lReturn = OpenTempFiles(sModelCode, sOpenFileBusinessType)

        'get index file details (primarily lookup names)

        m_lReturn = GetDistinctLookupNames(vLookupNames, sOpenFileBusinessType, lLimitToSpecifiedBusinessType)

        'if we didn't get any names then exit
        If Informations.IsArray(vLookupNames) = gPMConstants.PMEReturnCode.PMFalse Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        'loop around each lookup and get headers

        For lLookupNamePtr As Integer = vLookupNames.GetLowerBound(1) To vLookupNames.GetUpperBound(1)
            'get lookup name

            sLookupName = Convert.ToString(vLookupNames(2, lLookupNamePtr)).ToUpper()
            sBusinessType = vLookupNames(1, lLookupNamePtr)
            lInsurer_id = CInt(vLookupNames(0, lLookupNamePtr))
            'get header file details
            'populate index record buffer

            'm_IndexRecord.BusinessType = New String(" "c, m_IndexRecord.BusinessType.Length - sBusinessType.Length) & sBusinessType
            'm_IndexRecord.InsurerID = New String(" "c, m_IndexRecord.InsurerID.Length - CStr(lInsurer_id).Length) & CStr(lInsurer_id)
            'm_IndexRecord.TableName = New String(" "c, m_IndexRecord.TableName.Length - sLookupName.Length) & sLookupName.ToUpper()
            'm_IndexRecord.Header_Start_ptr = CStr(lHeaderFilePtr)

            m_IndexRecord.BusinessType = Space((m_IndexRecord.BusinessType).Length - (sBusinessType).Length) & sBusinessType
            m_IndexRecord.InsurerID = Space((m_IndexRecord.InsurerID).Length - (CStr(lInsurer_id)).Length) & CStr(lInsurer_id)
            m_IndexRecord.TableName = Space((m_IndexRecord.TableName).Length - (sLookupName).Length) & (sLookupName).ToUpper
            m_IndexRecord.Header_Start_ptr = lHeaderFilePtr

            m_lReturn = GetHeaderDetails(lInsurer_id, sBusinessType, sLookupName, vHeaderDetails)
            'note there must be at least one otherwise we couldn't have got this far!

            For lHeaderPtr As Integer = vHeaderDetails.GetLowerBound(1) To vHeaderDetails.GetUpperBound(1)

                sEffectiveDate = CStr(vHeaderDetails(0, lHeaderPtr))

                sModifiedDate = CStr(vHeaderDetails(1, lHeaderPtr))

                lStatus = CInt(vHeaderDetails(2, lHeaderPtr))

                'FileSystem.FilePut need fixed length string if file is open in Random access, not supporting long strings
                sDefinition = Left(CStr(vHeaderDetails(3, lHeaderPtr)), 70)

                sValidConstants = CStr(vHeaderDetails(4, lHeaderPtr))

                sDefaultValue = CStr(vHeaderDetails(5, lHeaderPtr))

                lHeaderKey = CInt(vHeaderDetails(6, lHeaderPtr))

                'populate header record buffer
                m_HeaderRecord.EffectiveDateTime = CDate(sEffectiveDate).ToString("yyyyMMddHHmmss")
                m_HeaderRecord.ModifiedDateTime = CDate(sModifiedDate).ToString("yyyyMMddHHmmss")
                m_HeaderRecord.Status = Strings.ChrW(lStatus).ToString()
                m_HeaderRecord.Definition = sDefinition
                m_HeaderRecord.ValidConstants = sValidConstants
                m_HeaderRecord.DefaultValue = sDefaultValue
                m_HeaderRecord.Data_Start_ptr = CStr(0)
                m_HeaderRecord.Data_End_ptr = CStr(0)

                'now lets get data for this particular table entry

                m_lReturn = GetData(lHeaderKey, vData)
                If Informations.IsArray(vData) Then
                    m_HeaderRecord.Data_Start_ptr = CStr(lDataFilePtr)

                    For lDataPtr As Integer = vData.GetLowerBound(1) To vData.GetUpperBound(1)

                        sLevel = CStr(vData(0, lDataPtr))

                        sLevelValue = CStr(vData(1, lDataPtr))

                        sType = CStr(vData(2, lDataPtr))
                        m_DataRecord.Level = sLevel
                        m_DataRecord.LevelValue = sLevelValue
                        m_DataRecord.TypeOfLevel = sType
                        'write data record

                        'Modified as checked runtime
                        'FileSystem.FilePutObject(lFileHandleD, m_DataRecord, lDataFilePtr)
                        FileSystem.FilePut(lFileHandleD, m_DataRecord, lDataFilePtr)
                        'increment datafileptr
                        lDataFilePtr += 1
                    Next lDataPtr
                    m_HeaderRecord.Data_End_ptr = CStr(lDataFilePtr - 1)
                End If

                'finish off header record details
                'write header record

                'Modified as checked runtime
                'FileSystem.FilePutObject(lFileHandleH, m_HeaderRecord, lHeaderFilePtr)
                FileSystem.FilePut(lFileHandleH, m_HeaderRecord, lHeaderFilePtr)

                'increment header file ptr
                lHeaderFilePtr += 1

            Next lHeaderPtr

            'finish off index record details
            m_IndexRecord.Header_End_ptr = CStr(lHeaderFilePtr - 1)
            'write indexrecord

            'Modified as checked runtime
            'FileSystem.FilePutObject(lFileHandleI, m_IndexRecord, lIndexFilePtr)
            FileSystem.FilePut(lFileHandleI, m_IndexRecord, lIndexFilePtr)
            'increment index file ptr
            lIndexFilePtr += 1

        Next lLookupNamePtr

        m_lReturn = CloseTempFiles()

        Return result
    End Function

    Private Function GetData(ByRef lHeaderKey As Integer, ByRef vData As Object) As Integer


        Dim sSQL As String = ""
        sSQL = sSQL & " SELECT      Key_Level,value,type"
        sSQL = sSQL & " FROM        gis_lookup_data"
        sSQL = sSQL & " WHERE       lookup_key = " & CStr(lHeaderKey)
        sSQL = sSQL & " ORDER BY    line_key"

        ' Call the SQL

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString, sSQLName:="GetData", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vData)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMTrue
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


    End Function


    Private Function GetHeaderDetails(ByRef lInsurer_id As Integer, ByRef sBusinessType As String, ByRef sLookupName As String, ByRef vHeaderDetails As Object) As Integer


        Dim sSQL As String = ""
        sSQL = sSQL & " SELECT      Effective_Date,"
        sSQL = sSQL & "             Modified_Date,"
        sSQL = sSQL & "             Status,"
        sSQL = sSQL & "             Definition,"
        sSQL = sSQL & "             Valid_Constants,"
        sSQL = sSQL & "             default_value,"
        sSQL = sSQL & "             lookup_key"
        sSQL = sSQL & " FROM        gis_lookup_header"
        sSQL = sSQL & " WHERE       Insurer_Panel_member_Key = " & CStr(lInsurer_id)
        sSQL = sSQL & " AND         Scheme_Number=" & sBusinessType
        sSQL = sSQL & " AND         UPPER(Lookup_Name)='" & sLookupName & "'"
        sSQL = sSQL & " ORDER BY    Effective_Date,Modified_Date,Status"


        ' Call the SQL

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString, sSQLName:="GetHeaderDetails", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vHeaderDetails)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMTrue
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

    End Function

    Private Function GetDistinctLookupNames(ByRef vresultarray As Object, ByRef sBusinessType As String, ByRef lLimitToSpecifiedBusinessType As Integer) As Integer


        Dim sSQL As String = ""
        sSQL = sSQL & " SELECT insurer_panel_member_key,scheme_number, lookup_name"
        sSQL = sSQL & " FROM gis_lookup_header"
        If lLimitToSpecifiedBusinessType = gPMConstants.PMEReturnCode.PMTrue Then
            sSQL = sSQL & " WHERE scheme_number=" & sBusinessType
        End If
        sSQL = sSQL & " ORDER BY insurer_panel_member_key,scheme_number,"
        sSQL = sSQL & " (SPACE(50 - Len(Ltrim(RTrim(lookup_name))))+ LTrim(RTrim(lookup_name)))"

        ' Call the SQL
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString, sSQLName:="GetDistinctLookupNames", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vresultarray)
        'm_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDistinctLookupNames", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vresultarray:=vresultarray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMTrue
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

    End Function



    Public Sub New()
        MyBase.New()
    End Sub


    ' *********************************************************************************
    ' OpenFiles
    ' Opens index, header, and data files for random access
    ' *********************************************************************************
    Public Function OpenTempFiles(ByRef sModelCode As String, ByRef sBusinessType As String) As Integer

        Try

            Dim sFileI As String = ""
            Dim sFileH As String = ""
            Dim sFileD As String = ""
            Dim sFilesLocation As String = ""

            'set and format business type and modelcode e.g. DNO/MAR
            m_sRequiredBusinessType = StringsHelper.Format(sBusinessType.Trim().ToUpper(), "@@@@")
            m_sModelCode = sModelCode

            m_lReturn = FullPathFileNames(sFileI, sFileH, sFileD)


            lFileHandleI = FileSystem.FreeFile()

            'Modified as Marshal.SizeOf is not working in .net
            'FileSystem.FileOpen(lFileHandleI, sFileI, OpenMode.Random, , , Marshal.SizeOf(m_IndexRecord))
            FileSystem.FileOpen(lFileHandleI, sFileI, OpenMode.Random, Access:=OpenAccess.Default, Share:=OpenShare.Shared)

            lFileHandleH = FileSystem.FreeFile()

            'Developer Guide no. 132 and Marshal.SizeOf is not working in .net

            FileSystem.FileOpen(lFileHandleH, sFileH, OpenMode.Random, Access:=OpenAccess.Default, Share:=OpenShare.Shared)
            lFileHandleD = FileSystem.FreeFile()

            'Developer Guide no. 132 and Marshal.SizeOf is not working in .net

            FileSystem.FileOpen(lFileHandleD, sFileD, OpenMode.Random, Access:=OpenAccess.Default, Share:=OpenShare.Shared)


            'Modified as lNumIndexRecords is not using and showing runtime error
            lNumIndexRecords = CInt(CInt((New FileInfo(sFileI)).Length) / Marshal.SizeOf(m_IndexRecord))

        Catch excep As System.Exception


            'Developer Guide no 216

            excep.Message.Replace(Nothing, "Can not open/create Lookup cache files")
            Throw New System.Exception("10002")
            Exit Function

        End Try

        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    Function CloseTempFiles() As Integer

        Try

            FileSystem.FileClose(lFileHandleI)
            FileSystem.FileClose(lFileHandleH)
            FileSystem.FileClose(lFileHandleD)

        Catch excep As System.Exception

            'Developer Guide no 216

            excep.Message.Replace(Nothing, "Unable to close Lookup cache files")

            Throw New System.Exception("10007")
            Exit Function

        End Try

        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    Private Function FullPathFileNames(ByRef sFileI As String, ByRef sFileH As String, ByRef sFileD As String) As Integer

        Dim sFilesLocation As String = ""

        'get directory location from registry

        'm_lReturn = GetLookupsPath(m_sRequiredBusinessType, sFilesLocation)
        '
        ' Ram 29-06-2000
        m_lReturn = GISSharedConstants.GetLookupsPath(m_sModelCode, sFilesLocation)


        'get file names
        m_lReturn = FileNames(sFileI, sFileH, sFileD)
        'concat
        'file names are made up of location code and model type
        sFileI = sFilesLocation & sFileI
        sFileH = sFilesLocation & sFileH
        sFileD = sFilesLocation & sFileD

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    Private Function FileNames(ByRef sFileI As String, ByRef sFileH As String, ByRef sFileD As String) As Integer

        sFileI = m_sModelCode & "_LookupIndex.dat"
        sFileH = m_sModelCode & "_LookupHeader.dat"
        sFileD = m_sModelCode & "_LookupData.dat"

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function
End Class