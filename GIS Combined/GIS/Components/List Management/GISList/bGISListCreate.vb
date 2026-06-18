Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("bGISListCreate_NET.bGISListCreate")> _
Public NotInheritable Class bGISListCreate 
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
	Private m_sPassword As String = ""
	Private m_iUserID As Integer
	Private m_sCallingAppName As String = ""
	Private m_iSourceID As Integer
	Private m_iLanguageID As Integer
	Private m_iCurrencyID As Integer
	Private m_iLogLevel As Integer
	Private Const ACClass As String = "bGISListCreate"
	
	Private m_sInput As String = ""
	Private m_sOutput As String = ""
	Private m_iRLDFFile As Integer
	Private m_lRLDFRecord As Integer
	Private m_vPropertyArray( ,  ) As Object
	Private m_lPropertyArrayCnt As Integer

    Private m_oDatabase As dPMDAO.Database
	
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
            Return m_sOutput
        End Get
        Set(ByVal Value As String)
            m_sOutput = Value
        End Set
    End Property
	
    Public Function Create() As Integer

        Dim lResult As gPMConstants.PMEReturnCode
        Dim sFilename As String = ""

        ' Has an input file been specified
        If m_sInput = "" Then
            lResult = gPMConstants.PMEReturnCode.PMFail
        Else
            ' Make sure the specified input file exists
            If FileSystem.Dir(m_sInput, FileAttribute.Normal) = "" Then
                lResult = gPMConstants.PMEReturnCode.PMFail
            Else
                ' Determine the output file name, if not already set
                If m_sOutput = "" Then
                    m_sOutput = ExtractOutputName(m_sInput)
                End If

                ' Build DAT and IDX files
                lResult = CType(BuildOutputDatFile(), gPMConstants.PMEReturnCode)
                If lResult = gPMConstants.PMEReturnCode.PMTrue Then
                    lResult = CType(BuildOutputIdxFile(), gPMConstants.PMEReturnCode)
                End If
            End If

        End If
        Return lResult
    End Function
	
    Private Function ExtractOutputName(ByRef sFilename As String) As String

        Dim iSuffixStart As Integer = (sFilename.IndexOf("."c) + 1)
        If iSuffixStart > 0 Then
            Return sFilename.Substring(0, iSuffixStart - 1)
        Else
            Return sFilename
        End If
    End Function
	
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
            FileSystem.FileOpen(m_iRLDFFile, m_sOutput & ".Dat", OpenMode.Random)


            If FileSystem.Dir(m_sInput, FileAttribute.Normal) <> "" Then
                ' Open and process the input file
                iInputFile = FileSystem.FreeFile()
                FileSystem.FileOpen(iInputFile, m_sInput, OpenMode.Binary)

                ' CTAF 100103 - Connect to the database
                m_oDatabase = New dPMDAO.Database()

                lReturn = CType(m_oDatabase.OpenDatabase("sirius", 1, 1, CStr(1), "", "", gPMConstants.PMSiriusDSN, ""), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_oDatabase = Nothing
                End If

                lReturn = gPMConstants.PMEReturnCode.PMTrue
                While Not FileSystem.EOF(iInputFile) And lReturn = gPMConstants.PMEReturnCode.PMTrue
                    sInputRecord = FileSystem.LineInput(iInputFile)
                    If sInputRecord <> "" Then
                        lReturn = CType(ProcessInputRecord(v_sInputRecord:=sInputRecord), gPMConstants.PMEReturnCode)
                    End If
                    Application.DoEvents()
                End While
                FileSystem.FileClose(iInputFile)

                If Not (m_oDatabase Is Nothing) Then
                    lReturn = m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If

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
        Dim lTemp As Integer 

        Static sSavePropertyId As String = ""

        

            lReturn = gPMConstants.PMEReturnCode.PMTrue

            sPropertyId = Mid(v_sInputRecord, 1, 11).Trim()
            sABICode = Mid(v_sInputRecord, 83).Trim()

        lTemp = v_sInputRecord.Length - Mid(v_sInputRecord, 83).Length - 12
        sDescription = Mid(v_sInputRecord, 12, lTemp).Trim()

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

            Dim sSQL As String = "" 
            Dim vTemp(,) As Object 
            Dim lResult As gPMConstants.PMEReturnCode 
            Dim lCaptionID, lID As Integer 

            If Not (m_oDatabase Is Nothing) Then

                sDescription = sDescription.Trim()

                sSQL = "SELECT gis_user_def_header_id FROM gis_user_def_header WHERE code = {code}"

                m_oDatabase.Parameters.Clear()

                lResult = m_oDatabase.Parameters.Add(sName:="code", vValue:=sPropertyId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                lResult = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="getheader", bStoredProcedure:=False, vResultArray:=vTemp)

                If Not Information.IsArray(vTemp) Then

                    ' Get the caption
                    sSQL = "spu_pm_caption_id_return"
                    m_oDatabase.Parameters.Clear()
                    lResult = m_oDatabase.Parameters.Add("language_id", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    lResult = m_oDatabase.Parameters.Add("caption", sPropertyId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    lResult = m_oDatabase.Parameters.Add("caption_id", CStr(0), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                    lResult = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="getcap", bStoredProcedure:=True, vResultArray:=vTemp)
                    lCaptionID = m_oDatabase.Parameters.Item("caption_id").Value

                    ' New Header
                    sSQL = "INSERT INTO gis_user_def_header ( " & _
                           "caption_id, code, description, is_deleted, effective_date, Parent ) VALUES (" & _
                           lCaptionID & ", '" & sPropertyId & "', '" & sPropertyId & "', 0, GetDate(), -1 )"

                    lResult = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="putheader", bStoredProcedure:=False)

                    sSQL = "SELECT gis_user_def_header_id FROM gis_user_def_header WHERE code = {code}"

                    m_oDatabase.Parameters.Clear()

                    lResult = m_oDatabase.Parameters.Add(sName:="code", vValue:=sPropertyId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    lResult = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="getheader", bStoredProcedure:=False, vResultArray:=vTemp)

                End If

                lID = CInt(vTemp(0, 0))

                sSQL = "SELECT * FROM gis_user_def_detail gd " & _
                       "INNER JOIN gis_user_def_header gh ON gh.gis_user_def_header_id = gd.gis_user_def_header_id " & _
                       "WHERE gd.code = '" & sABICode.Trim() & "' " & _
                       "AND gh.code = '" & sPropertyId.Trim() & "'"

                lResult = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckExists", bStoredProcedure:=False, vResultArray:=vTemp)

                If Not Information.IsArray(vTemp) Then

                    sSQL = "spu_pm_caption_id_return"
                    m_oDatabase.Parameters.Clear()
                    lResult = m_oDatabase.Parameters.Add("language_id", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    lResult = m_oDatabase.Parameters.Add("caption", sDescription, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                    lResult = m_oDatabase.Parameters.Add("caption_id", CStr(0), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                    lResult = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="getcap", bStoredProcedure:=True, vResultArray:=vTemp)
                    lCaptionID = m_oDatabase.Parameters.Item("caption_id").Value

                    ' Add the detail
                    sSQL = "INSERT INTO gis_user_def_detail ( " & _
                           "gis_user_def_header_id, caption_id, code, description, is_deleted , effective_date, Parent, GIS_user_def_header_inds_id) VALUES ( " & _
                           lID & ", " & CStr(lCaptionID) & ", '" & sABICode.Trim() & "', '" & sDescription.Replace("'", "''").Trim() & "', 0, GetDate(), NULL, NULL )"

                    lResult = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="adddetail", bStoredProcedure:=False)

                ElseIf Not vTemp(4, 0).ToString().Equals(sDescription) Then

                    sSQL = "UPDATE PMCaption " & _
                       "SET caption = '" & sDescription.Trim() & "' " & _
                       "WHERE caption_id = '" & vTemp(2, 0) & "'"

                    lResult = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateDescription", bStoredProcedure:=False)

                    sSQL = "UPDATE gis_user_def_detail " & _
                       "SET description = '" & sDescription.Trim() & "' " & _
                       "WHERE code = '" & sABICode.Trim() & "' " & _
                       "AND caption_id = '" & vTemp(2, 0) & "'"

                    lResult = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateDescription", bStoredProcedure:=False)

                End If

            End If


        Return lReturn
    End Function

    Private Function BuildOutputIdxFile() As Integer
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMSucceed

        Dim iIndexFile As Integer
        Dim sPropertyId As String = ""
        Dim lRecordNumber As Integer
        Dim uindexrecord As MainModule.RLDFIndexRecord = MainModule.RLDFIndexRecord.CreateInstance()
        Dim i As Integer

        i = 0

        iIndexFile = FileSystem.FreeFile()

        ' Open the index file
        If FileSystem.Dir(m_sOutput & ".Idx", FileAttribute.Normal) <> "" Then
            Try
                File.Delete(m_sOutput & ".Idx")

            Catch
            End Try
        End If
        FileSystem.FileOpen(iIndexFile, m_sOutput & ".Idx", OpenMode.Random)

        ' Loop arround the arrray writing records
        For i = 0 To m_vPropertyArray.GetUpperBound(1)

            sPropertyId = CStr(m_vPropertyArray(ACPAPropertyId, i))
            lRecordNumber = CInt(m_vPropertyArray(ACPARecordIndex, i))

            uindexrecord.PropertyId = sPropertyId & Space(10 - sPropertyId.Length)
            uindexrecord.RecordNumber = lRecordNumber
            FileSystem.FilePut(iIndexFile, uindexrecord, i + 1)

        Next i
        result = gPMConstants.PMEReturnCode.PMTrue

Done_BuildOutputIdxFile:
        ' close the index file
        FileSystem.FileClose(iIndexFile)

        Return result

Err_BuildOutputIdxFile:
        ' Error.
        result = gPMConstants.PMEReturnCode.PMError
        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="BuildOutputIdxFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildOutputIdxFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function
End Class

