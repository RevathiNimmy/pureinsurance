Option Strict Off
Option Explicit On
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
'developer guide no. 129
Imports SSP.Shared

Friend NotInheritable Class Common
    ' ***************************************************************** '
    ' Class Name: Common
    '
    ' Date: 16/09/1999
    '
    ' Description: Contains all the common routines for the public
    '              interfaces
    ' Edit History:
    ' ***************************************************************** '
    'RLDF Detail record
    'developer guide no. 107(Guide)	
    'START'
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Structure RLDFDetailRecord
        'developer guide no. 130(Guide)	
        ' <VBFixedString(10)>
        Public PropertyId As StringsHelper.FixedLengthString
        'developer guide no. 130(Guide)	
        ' <VBFixedString(70)>
        Public Description As StringsHelper.FixedLengthString
        'developer guide no. 130(Guide)	
        ' <VBFixedString(10)>
        Public ABICode As StringsHelper.FixedLengthString
        Public Shared Function CreateInstance() As RLDFDetailRecord
            Dim result As New RLDFDetailRecord
            result.PropertyId = New StringsHelper.FixedLengthString(10)
            result.Description = New StringsHelper.FixedLengthString(70)
            result.ABICode = New StringsHelper.FixedLengthString(10)
            Return result
        End Function
    End Structure
    'RLDF Index record 
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Structure RLDFIndexRecord
        'developer guide no. 130 (Guide)
        ' <VBFixedString(10)> 
        Public PropertyId As StringsHelper.FixedLengthString
        Dim RecordNumber As Integer
        Public Shared Function CreateInstance() As RLDFIndexRecord
            Dim result As New RLDFIndexRecord
            'developer guide no. 130 (Guide)
            Return result
        End Function
    End Structure
    Private _cFiles As cFilesKeyedCollection = Nothing
    Public Property cFiles() As cFilesKeyedCollection
        Get
            If _cFiles Is Nothing Then
                _cFiles = New cFilesKeyedCollection
            End If
            Return _cFiles
        End Get
        Set(ByVal Value As cFilesKeyedCollection)
            _cFiles = Value
        End Set
    End Property
    'END'

    Private Const ACClass As String = "Common"
    Public Const GEMMaxListItems As Integer = 2500
    Public Const RLDF_RECORD_LENGTH As Integer = 128
    Public Const RECORD_BUFFER As Integer = 256000
    Public Const RECORDS_PER_READ As Integer = 2000
    'PN20870 SRF187-262 Insurer Maintenance ABI List erroring
    'These have been moved from Common as the files can be opened by both Interface and InterfaceNoLogin
    'NOTE There could be a problem if different lists are opened by the same process using Interface and InterfaceNologin
    Public m_iFileOpenCounter As Integer 'indicates how many times OpenFiles has been called
    Public m_iRLDFFile As Integer
    Public m_iRLDFFileB As Integer
    Public m_iRLDFIndex As Integer
    Public m_cIndex As cIndexKeyedCollection
    Public m_lRLDFFileLength As Integer 'this also needs to be public as it is used by GII motor lists

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_sServerListFilePath As String = ""
    Private m_sServerListVersion As String = ""
    Private m_sServerListPrefVersion As String = ""
    Private m_bServerListFileCompressed As Boolean

    Private m_sClientListFilePath As String = ""
    Private m_sClientListFilePathIdx As String = ""
    Private m_sClientListFilePathDat As String = ""

    Private m_sServerListFilePathIdx As String = ""
    Private m_sServerListFilePathDat As String = ""

    Private m_sClientListVersion As String = ""
    Private m_sClientListPrefVersion As String = ""

    ' Interface object
    Private m_oInterface As Object

    Private m_oZipper As Object

    Private m_oBusiness As bGISListManager.Form
    Private m_bNoLogin As Boolean
    Private m_lMaxListItems As Integer
    Private m_bDataModelLevel As Boolean

    'sj 02/02/2001 - start
    ' VehicleListId
    Private m_sVehicleListId As String = ""
    ' NumberOfVehicles
    Private m_lNumberOfVehicles As Integer
    ' ClassOfBusiness
    Private m_sClassOfBusiness As String = ""
    Private Const m_kClassOfBusinessCV As String = "CV"

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
                                                                 _
    Private Structure udtVehicleRecord
        '<VBFixedString(1)>
        Public LeftBracket As StringsHelper.FixedLengthString
        '<VBFixedString(4)>
        Public YearStart As StringsHelper.FixedLengthString
        '<VBFixedString(1)>
        Public YearDivider As StringsHelper.FixedLengthString
        '<VBFixedString(4)>
        Public YearEnd As StringsHelper.FixedLengthString
        '<VBFixedString(1)>
        Public Divider1 As StringsHelper.FixedLengthString
        '<VBFixedString(4)>
        Public Make As StringsHelper.FixedLengthString
        '<VBFixedString(1)>
        Public Divider2 As StringsHelper.FixedLengthString
        '<VBFixedString(1)>
        Public Doors As StringsHelper.FixedLengthString
        '<VBFixedString(1)>
        Public StyleType As StringsHelper.FixedLengthString
        '<VBFixedString(1)>
        Public Divider3 As StringsHelper.FixedLengthString
        '<VBFixedString(1)>
        Public FuelType As StringsHelper.FixedLengthString
        '<VBFixedString(1)>
        Public Transmission As StringsHelper.FixedLengthString
        '<VBFixedString(1)>
        Public Divider4 As StringsHelper.FixedLengthString
        '<VBFixedString(4)>
        Public EngineSize As StringsHelper.FixedLengthString
        '<VBFixedString(1)>
        Public RightBracket As StringsHelper.FixedLengthString
        '<VBFixedString(1)>
        Public Divider5 As StringsHelper.FixedLengthString
        '<VBFixedString(41)>
        Public ModelName As StringsHelper.FixedLengthString
        '<VBFixedString(1)>
        Public Divider6 As StringsHelper.FixedLengthString
        '<VBFixedString(20)>
        Public Identifier As StringsHelper.FixedLengthString
        Public Shared Function CreateInstance() As udtVehicleRecord
            Dim result As New udtVehicleRecord
            ''<VBFixedString(1)>
            result.LeftBracket = New StringsHelper.FixedLengthString(1)
            '<VBFixedString(4)>
            result.YearStart = New StringsHelper.FixedLengthString(4)
            '<VBFixedString(1)>
            result.YearDivider = New StringsHelper.FixedLengthString(1)
            '<VBFixedString(4)>
            result.YearEnd = New StringsHelper.FixedLengthString(4)
            '<VBFixedString(1)>
            result.Divider1 = New StringsHelper.FixedLengthString(1)
            '<VBFixedString(4)>
            result.Make = New StringsHelper.FixedLengthString(4)
            '<VBFixedString(1)>
            result.Divider2 = New StringsHelper.FixedLengthString(1)
            '<VBFixedString(1)>
            result.Doors = New StringsHelper.FixedLengthString(1)
            '<VBFixedString(1)>
            result.StyleType = New StringsHelper.FixedLengthString(1)
            '<VBFixedString(1)>
            result.Divider3 = New StringsHelper.FixedLengthString(1)
            '<VBFixedString(1)>
            result.FuelType = New StringsHelper.FixedLengthString(1)
            '<VBFixedString(1)>
            result.Transmission = New StringsHelper.FixedLengthString(1)
            '<VBFixedString(1)>
            result.Divider4 = New StringsHelper.FixedLengthString(1)
            '<VBFixedString(4)>
            result.EngineSize = New StringsHelper.FixedLengthString(4)
            '<VBFixedString(1)>
            result.RightBracket = New StringsHelper.FixedLengthString(1)
            '<VBFixedString(1)>
            result.Divider5 = New StringsHelper.FixedLengthString(1)
            '<VBFixedString(41)>
            result.ModelName = New StringsHelper.FixedLengthString(41)
            '<VBFixedString(1)>
            result.Divider6 = New StringsHelper.FixedLengthString(1)
            '<VBFixedString(20)>
            result.Identifier = New StringsHelper.FixedLengthString(20)
            Return result
        End Function
    End Structure

    Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByVal hpvDest As Integer, ByVal hpvSource As Integer, ByVal cbCopy As Integer)

    Public WriteOnly Property ClassOfBusiness() As String
        Set(ByVal Value As String)
            m_sClassOfBusiness = Value
        End Set
    End Property

    Public WriteOnly Property NumberOfVehicles() As Integer
        Set(ByVal Value As Integer)
            m_lNumberOfVehicles = Value
        End Set
    End Property
    Public WriteOnly Property VehicleListId() As String
        Set(ByVal Value As String)
            m_sVehicleListId = Value
        End Set
    End Property
    'sj 02/02/2001 - end

    Public WriteOnly Property MaxListItems() As Integer
        Set(ByVal Value As Integer)
            m_lMaxListItems = Value
        End Set
    End Property

    Public Property NoLogin() As Boolean
        Get
            Return m_bNoLogin
        End Get
        Set(ByVal Value As Boolean)
            m_bNoLogin = Value
        End Set
    End Property

    Public Property Zipper() As Object
        Get
            Return m_oZipper
        End Get
        Set(ByVal Value As Object)
            If Informations.IsReference(Value) And Not (TypeOf Value Is String) Then
                m_oZipper = Value
            Else
                m_oZipper = Value
            End If
        End Set
    End Property

    Public Property Business() As Object
        Get
            Return m_oBusiness
        End Get
        Set(ByVal Value As Object)
            If Informations.IsReference(Value) And Not (TypeOf Value Is String) Then
                m_oBusiness = Value
            Else
                m_oBusiness = Value
            End If
        End Set
    End Property


    ''' <summary>
    ''' GetDescriptionFromABICodeC
    ''' </summary>
    ''' <param name="v_sPropertyId"></param>
    ''' <param name="v_sABICode"></param>
    ''' <param name="r_sDescription"></param>
    ''' <returns></returns>
    Public Function GetDescriptionFromABICodeC(ByVal v_sPropertyId As String, ByVal v_sABICode As String, ByRef r_sDescription As String) As Integer

        Dim result As Integer = 0
        Try

            Dim lByteStart, lRecordNumber As Integer
            Dim sRecord As String = ""
            Dim lStart, lCnt, lBufferSize, lNumberOfReads, lRemainder As Integer

            result = gPMConstants.PMEReturnCode.PMNotFound

            ' *****************************************************************
            ' Get the record number from the index
            ' *****************************************************************
            m_lReturn = CType(GetListIndex(v_sPropertyId:=v_sPropertyId, r_lRecordNumber:=lRecordNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'Find out the number of reads we need to do
            lNumberOfReads = CInt(m_lMaxListItems / RECORDS_PER_READ)
            If (lNumberOfReads * RECORDS_PER_READ) > m_lMaxListItems Then
                lRemainder = m_lMaxListItems - ((lNumberOfReads - 1) * RECORDS_PER_READ)
            Else
                lRemainder = m_lMaxListItems - (lNumberOfReads * RECORDS_PER_READ)
                If lRemainder > 0 Then
                    lNumberOfReads += 1
                End If
            End If


            ' Convert remainder to bytes
            lRemainder *= RLDF_RECORD_LENGTH

            For lRecordCnt As Integer = 1 To lNumberOfReads

                ' calculate the starting point
                lByteStart = (lRecordNumber * RLDF_RECORD_LENGTH) + 1

                If lByteStart > m_lRLDFFileLength Then
                    ' we have reached the end of file without finding what we are looking for
                    Return result
                End If

                ' Work out the buffer size
                If lRecordCnt = lNumberOfReads And lRemainder > 0 Then
                    lBufferSize = lRemainder
                Else
                    lBufferSize = RECORD_BUFFER
                End If

                ' Read the data into a string
                sRecord = New String(" "c, lBufferSize)

                'FileSystem.c(m_iRLDFFileB, sRecord, lByteStart)

                ' search the string for the abi code
                lCnt = 1
                While lCnt < lBufferSize

                    lStart = Informations.InStr(lCnt, sRecord, v_sABICode)

                    If lStart > 80 Then
                        ' check we have the correct property id
                        'CLG 20042409
                        'm_lMaxListItems defaults to 2500 which is too small for motor lists.
                        'If you set m_lMaxListItems to a large value motor lists work.
                        'If you set m_lMaxListItems to a large value but pass in an incorrect value for a smaller list
                        'the list manager keeps searching till m_lMaxListItems and sometimes find an incorrect  value in another list.
                        'using v_sPropertyId = RTrim instead of v_sPropertyId = Trim fixes this
                        If v_sPropertyId = sRecord.Substring(lStart - 81, Math.Min(sRecord.Length, 10)).TrimEnd() And v_sABICode = sRecord.Substring(lStart - 1, Math.Min(sRecord.Length, 10)).Trim() Then

                            ' Assign the description
                            r_sDescription = sRecord.Substring(lStart - 71, Math.Min(sRecord.Length, 70)).Trim()

                            ' Make sure we drop out of the loops
                            lCnt = lBufferSize + 1
                            lRecordCnt = lNumberOfReads + 1

                            ' return PMTrue as we have found the item we are searching for
                            result = gPMConstants.PMEReturnCode.PMTrue
                        Else
                            lCnt = lStart + 1
                        End If
                    Else
                        If lStart = 0 Then
                            lCnt = lBufferSize + 1
                        Else
                            lCnt = lStart + 1
                        End If
                    End If
                End While

                sRecord = ""

                lRecordNumber += RECORDS_PER_READ

            Next lRecordCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListC failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDescriptionFromABICodeC", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' GetABICodeFromDescriptionC
    ''' </summary>
    ''' <param name="v_sPropertyId"></param>
    ''' <param name="v_sDescription"></param>
    ''' <param name="r_sABICode"></param>
    ''' <returns></returns>
    Public Function GetABICodeFromDescriptionC(ByVal v_sPropertyId As String, ByVal v_sDescription As String, ByRef r_sABICode As String) As Integer

        Dim result As Integer = 0
        Try

            Dim lByteStart, lRecordNumber As Integer
            Dim sRecord As String = ""
            Dim lStart, lCnt, lBufferSize, lNumberOfReads, lRemainder As Integer

            result = gPMConstants.PMEReturnCode.PMNotFound

            ' *****************************************************************
            ' Get the record number from the index
            ' *****************************************************************
            m_lReturn = CType(GetListIndex(v_sPropertyId:=v_sPropertyId, r_lRecordNumber:=lRecordNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'Find out the number of reads we need to do
            lNumberOfReads = CInt(m_lMaxListItems / RECORDS_PER_READ)
            If (lNumberOfReads * RECORDS_PER_READ) > m_lMaxListItems Then
                lRemainder = m_lMaxListItems - ((lNumberOfReads - 1) * RECORDS_PER_READ)
            Else
                lRemainder = m_lMaxListItems - (lNumberOfReads * RECORDS_PER_READ)
                If lRemainder > 0 Then
                    lNumberOfReads += 1
                End If
            End If


            ' Convert remainder to bytes
            lRemainder *= RLDF_RECORD_LENGTH

            For lRecordCnt As Integer = 1 To lNumberOfReads

                ' calculate the starting point
                lByteStart = (lRecordNumber * RLDF_RECORD_LENGTH) + 1

                If lByteStart > m_lRLDFFileLength Then
                    ' we have reached the end of file without finding what we are looking for
                    Return result
                End If

                ' Work out the buffer size
                If lRecordCnt = lNumberOfReads And lRemainder > 0 Then
                    lBufferSize = lRemainder
                Else
                    lBufferSize = RECORD_BUFFER
                End If

                ' Read the data into a string
                sRecord = New String(" "c, lBufferSize)

                FileSystem.FileGet(m_iRLDFFileB, sRecord, lByteStart)

                ' search the string for the abi code
                lCnt = 1
                While lCnt < lBufferSize

                    lStart = Informations.InStr(lCnt, sRecord, v_sDescription)

                    If lStart > 10 Then
                        ' check we have the correct property id
                        If v_sPropertyId = sRecord.Substring(lStart - 11, Math.Min(sRecord.Length, 10)).Trim() And v_sDescription = sRecord.Substring(lStart - 1, Math.Min(sRecord.Length, 70)).Trim() Then

                            ' Assign the ABI code
                            r_sABICode = sRecord.Substring(lStart + 69, Math.Min(sRecord.Length, 10)).Trim()

                            ' Make sure we drop out of the loops
                            lCnt = lBufferSize + 1
                            lRecordCnt = lNumberOfReads + 1

                            ' return PMTrue as we have found the item we are searching for
                            result = gPMConstants.PMEReturnCode.PMTrue
                        Else
                            lCnt = lStart + 1
                        End If
                    Else
                        If lStart = 0 Then
                            lCnt = lBufferSize + 1
                        Else
                            lCnt = lStart + 1
                        End If
                    End If
                End While

                sRecord = ""

                lRecordNumber += RECORDS_PER_READ

            Next lRecordCnt



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListC failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetABICodeFromDescriptionC", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BuildIndex (Standard Method)
    '
    ' Description: Build the index for the RLDF data
    '
    ' ***************************************************************** '
    Private Function BuildIndex() As Integer

        Dim result As Integer = 0
        'developer guide no. 107(Guide)	
        Dim uIndexRecord As RLDFIndexRecord = RLDFIndexRecord.CreateInstance()
        Dim bEOF As Boolean

        m_cIndex = New cIndexKeyedCollection

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim iCtr As Integer = 1
        'sj 18/05/99 - start
        ' For random files EOF seems to be set on the last record NOT!! after it
        bEOF = False

        While Not bEOF

            Try
                FileSystem.FileGet(m_iRLDFIndex, uIndexRecord, iCtr)
                'developer guide no. 130(Guide)
                m_cIndex.PropertyId = uIndexRecord.PropertyId.ToString.Trim
                m_cIndex.Add(uIndexRecord.RecordNumber)

                bEOF = EOF(m_iRLDFIndex)

                iCtr += 1

            Catch ex As EndOfStreamException
                bEOF = True
            End Try
        End While
        'sj 18/05/88 - end

        Return result

    End Function

    ''' <summary>
    ''' Gets the record number from the collection
    ''' </summary>
    ''' <param name="v_sPropertyId"></param>
    ''' <param name="r_lRecordNumber"></param>
    ''' <returns></returns>
    Private Function GetListIndex(ByVal v_sPropertyId As String, ByRef r_lRecordNumber As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        r_lRecordNumber = CInt(m_cIndex(v_sPropertyId))

        Return result


    End Function

    ' ***************************************************************** '
    ' Name: CheckListVersionsC (Standard Method)
    '
    ' Description: Check the client list versions against the server
    '              values. If they are different copy the RLDF
    '              from the server and update the versions in the client
    '              registry
    ' ***************************************************************** '
    ''' <summary>
    ''' used
    ''' </summary>
    ''' <param name="v_sGisDataModelCode"></param>
    ''' <param name="v_sSellerCode"></param>
    ''' <returns></returns>
    Public Function CheckListVersionsC(ByVal v_sGisDataModelCode As String, Optional ByVal v_sSellerCode As String = "") As Integer

        Dim result As Integer = 0
        Dim sExistingClientRLDF As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetServerSettings(v_sGisDataModelCode, v_sSellerCode), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error getting server registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckListVersions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' *****************************************************************
            ' Open the RLDF files
            ' *****************************************************************

            Return OpenFiles()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckListVersionsC failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckListVersionsC", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetServerSettings (Standard Method)
    '
    ' Description: Uses the ListManager business object to get registry
    '              values from the server.
    '
    ' ***************************************************************** '
    ''' <summary>
    ''' used
    ''' </summary>
    ''' <param name="v_sGisDataModelCode"></param>
    ''' <param name="v_sSellerCode"></param>
    ''' <returns></returns>
    Private Function GetServerSettings(ByVal v_sGisDataModelCode As String, Optional ByVal v_sSellerCode As String = "") As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oBusiness Is Nothing Then
                m_oBusiness = New bGISListManager.Form()
            End If
            m_lReturn = m_oBusiness.GetServerSettings(r_sServerListFilePath:=m_sServerListFilePath, r_sServerListVersion:=m_sServerListVersion, r_sServerListPrefVersion:=m_sServerListPrefVersion, r_bServerListFileCompressed:=m_bServerListFileCompressed, v_sGISDataModelCode:=v_sGisDataModelCode, v_sSellerCode:=v_sSellerCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not m_sServerListFilePath.EndsWith("\RLDF") Then
                If Not m_sServerListFilePath.EndsWith("\") Then
                    m_sServerListFilePath = m_sServerListFilePath & "\"
                End If

                m_sServerListFilePathIdx = m_sServerListFilePath &
                                           v_sGisDataModelCode &
                                           v_sSellerCode &
                                           "List.idx"

                m_sServerListFilePathDat = m_sServerListFilePath &
                                           v_sGisDataModelCode &
                                           v_sSellerCode &
                                           "List.dat"
            Else
                m_sServerListFilePathIdx = m_sServerListFilePath & ".idx"

                m_sServerListFilePathDat = m_sServerListFilePath & ".dat"
            End If

            m_sClientListFilePathIdx = m_sServerListFilePathIdx
            m_sClientListFilePathDat = m_sServerListFilePathDat

            m_bDataModelLevel = m_oBusiness.DataModelLevel



            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Server Settings failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServerSettings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        Finally
            m_oBusiness = Nothing
        End Try
    End Function


    Protected Overrides Sub Finalize()

        '******************************************************************
        ' Close the RLDF
        '******************************************************************
        CloseFiles()

        m_oBusiness = Nothing
        m_oZipper = Nothing

    End Sub

    ' ***************************************************************** '
    '
    ' Name: OpenFiles
    '
    ' Description: Opens the files if they are not already open.
    '              FileOpenCount is incremented each time the method is called.
    '
    ' History: 11/05/2005 CLG - Created. PN20870 SRF187-262 Insurer Maintenance ABI List erroring
    '
    ' ***************************************************************** '
    Private Function OpenFiles() As Integer
        Dim result As Integer = 0
        Dim oFSO As Object



        result = gPMConstants.PMEReturnCode.PMTrue


        Try

            'Dim aArray As Object = cFiles.Item(m_sClientListFilePathDat)
            Dim aArray() As Object = Nothing
            ' developer guide no. 131(Guide)
            If cFiles.Count > 0 Then
                aArray = cFiles(m_sClientListFilePathDat)
            End If
            Try

                If Informations.IsArray(aArray) Then
                    'already open
                    'set current values into globals

                    m_lRLDFFileLength = CInt(aArray(0))

                    m_iRLDFFile = CInt(aArray(1))

                    m_iRLDFIndex = CInt(aArray(2))

                    m_iRLDFFileB = CInt(aArray(3))
                    m_cIndex = aArray(4)

                    'increment count and reset collection


                    aArray(5) = CDbl(aArray(5)) + 1 'increment file open count
                    cFiles.Remove(m_sClientListFilePathDat)
                    cFiles.ClientListFilePathDat = m_sClientListFilePathDat
                    cFiles.Add(aArray)
                Else
                    'open file
                    ReDim aArray(5)

                    m_lRLDFFileLength = CInt((New FileInfo(m_sClientListFilePathDat)).Length)
                    If m_lRLDFFileLength <> 0 Then


                        aArray(0) = m_lRLDFFileLength

                        m_iRLDFFile = FreeFile()
                        'developer guide no. 132(Guide)
                        FileSystem.FileOpen(m_iRLDFFile, m_sClientListFilePathDat, OpenMode.Random, Access:=OpenAccess.Default, Share:=OpenShare.Shared)
                        aArray(1) = m_iRLDFFile

                        m_iRLDFIndex = FreeFile()
                        'developer guide no. 132(Guide)
                        FileSystem.FileOpen(m_iRLDFIndex, m_sClientListFilePathIdx, OpenMode.Random, Access:=OpenAccess.Default, Share:=OpenShare.Shared)
                        aArray(2) = m_iRLDFIndex

                        m_iRLDFFileB = FreeFile()
                        'developer guide no. 132(Guide)
                        FileSystem.FileOpen(m_iRLDFFileB, m_sClientListFilePathDat, OpenMode.Binary, Access:=OpenAccess.Default, Share:=OpenShare.Shared)
                        aArray(3) = m_iRLDFFileB

                        m_lReturn = CType(BuildIndex(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error building index", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckListVersionsC", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return gPMConstants.PMEReturnCode.PMError
                        End If
                        aArray(4) = m_cIndex


                        aArray(5) = 1 'file open count

                        'set current values into collection
                        cFiles.ClientListFilePathDat = m_sClientListFilePathDat
                        cFiles.Add(aArray)

                    Else
                        'okay we are in real trouble now with 0 length files.
                        'all we can do is attempt to delete them, report an error and hope they get refreshed next time.
                        oFSO = New Object()

                        File.Delete(m_sClientListFilePathDat)

                        File.Delete(m_sClientListFilePathIdx)
                        result = gPMConstants.PMEReturnCode.PMError
                    End If
                End If
                Return result

            Catch

                Return gPMConstants.PMEReturnCode.PMError
            End Try

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
        Return result
    End Function
    ' ***************************************************************** '
    '
    ' Name: CloseFiles
    '
    ' Description: Closes the files if FileOpenCount = 1 or the NoCountProcessing is true.
    '              FileOpenCount is decremented each time unless NoCountProcessing is true.
    '
    ' History: 11/05/2005 CLG - Created. PN20870 SRF187-262 Insurer Maintenance ABI List erroring
    '
    ' ***************************************************************** '
    Public Sub CloseFiles(Optional ByVal bNoCountProcessing As Boolean = False)



        Try

            'find the file entry

            'Dim aArray() As Collection = cFiles.Item(m_sClientListFilePathDat)
            'TODO:to be handled later
            'Dim aArray() As Collection
            Dim aArray() As Object = Nothing
            ' developer guide no. 131(Guide)
            If (cFiles.Count > 0) Then
                aArray = cFiles.Item(m_sClientListFilePathDat)
            End If

            If Informations.IsArray(aArray) Then
                'if last or forced then close file

                If CDbl(aArray(5)) = 1 Or bNoCountProcessing Then

                    FileClose(CInt(aArray(1))) 'm_iRLDFFile%

                    FileClose(CInt(aArray(2))) 'm_iRLDFIndex%

                    FileClose(CInt(aArray(3))) 'm_iRLDFFileB%
                    cFiles.Remove(m_sClientListFilePathDat)
                    Exit Sub
                End If

                'don't close yet but update count and save


                aArray(5) = CDbl(aArray(5)) - 1
                cFiles.Remove(m_sClientListFilePathDat)
                cFiles.ClientListFilePathDat = m_sClientListFilePathDat
                cFiles.Add(aArray)

            End If

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: GetListC (Standard Method)
    '
    ' Description: Returns a list in a variant array for a given
    '              property id.
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function GetListC(ByVal v_sPropertyId As String, ByRef r_vListData As Object, Optional ByVal v_vSearchString As Object = Nothing, Optional ByRef r_vListDataCode As Object = -1, Optional ByVal v_bMultiSearch As Boolean = False, Optional ByVal v_iFirstNoChars As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordNumber As Integer
        'developer guide no. 107(Guide)	
        Dim uDetailRecord As RLDFDetailRecord = RLDFDetailRecord.CreateInstance()
        Dim i, lPos As Integer
        Dim sSearchString, sRecord As String
        Dim bEOF, bFirst, bCodeListWanted As Boolean ' CL090699
        Dim sABICode, sPropID, sDesc As String
        Dim sFullDesc As New StringBuilder
        Dim iPosPlusPlusPlus As Integer
        Dim bSearchDone As Boolean
        Dim sSearchString2 As String = ""
        Dim lPos2 As Integer
        Dim k As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            bCodeListWanted = Informations.IsNothing(r_vListDataCode)

            i = 0
            'ReDim r_vListData(i)
            ReDim r_vListData(m_lMaxListItems)
            'If bCodeListWanted Then ReDim r_vListDataCode(i) ' CL090699
            If bCodeListWanted Then
                ReDim r_vListDataCode(m_lMaxListItems)
            End If
            ' *****************************************************************
            ' Get the record number from the index
            ' *****************************************************************
            m_lReturn = CType(GetListIndex(v_sPropertyId:=v_sPropertyId, r_lRecordNumber:=lRecordNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'MsgBox "Unable to load list for " & v_sPropertyId, _
                'vbOKOnly, _
                '"ListManager"
                'Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' *****************************************************************
            ' Assemble the list
            ' *****************************************************************
            'sj 18/05/99 - start
            ' Pick up last record of last list

            bEOF = False
            bFirst = True


            'developer guide no. 118(Guide)
            If v_vSearchString = String.Empty Then

                While Not bEOF And i < m_lMaxListItems

                    If bFirst Then

                        FileSystem.FileGet(m_iRLDFFile, uDetailRecord, lRecordNumber + 1)
                        bFirst = False
                    Else

                        FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)
                    End If

                    'developer guide no. 130(Guide)

                    sPropID = ToSafeString(uDetailRecord.PropertyId).Trim()
                    sDesc = ToSafeString(uDetailRecord.Description).Trim()
                    sABICode = ToSafeString(uDetailRecord.ABICode).Trim()

                    ' CL160699 BEGIN-->

                    ' Look for the +++ char sequence. This means concatenate with
                    ' the following record (and so on...). This gives us
                    ' access to very long list item descriptions.
                    sFullDesc = New StringBuilder("")

                    iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)

                    Do While (iPosPlusPlusPlus <> 0)
                        sFullDesc.Append(sDesc.Substring(0, iPosPlusPlusPlus - 1))


                        FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)

                        ' keep on truckin' mama
                        'developer guide no. 130(Guide)	
                        sDesc = ToSafeString(uDetailRecord.Description).Trim()
                        iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)
                    Loop


                    sFullDesc.Append(sDesc)

                    ' <-- END CL160699

                    If v_sPropertyId <> sPropID Then
                        bEOF = True
                    Else
                        'ReDim Preserve r_vListData(i)

                        r_vListData(i) = sFullDesc.ToString()

                        ' CL090699 BEGIN-->
                        If bCodeListWanted Then
                            'ReDim Preserve r_vListDataCode(i)

                            r_vListDataCode(i) = sABICode
                        End If
                        ' <-- END CL090699

                        i += 1
                        bEOF = FileSystem.EOF(m_iRLDFFile)

                        'ReDim Preserve r_vListData(i)

                        ' CL090699 BEGIN-->
                        'If bCodeListWanted Then
                        '    ReDim Preserve r_vListDataCode(i)
                        'End If
                        ' <-- END CL090699

                    End If


                End While

            Else

                sSearchString = v_vSearchString

                While Not bEOF And i < m_lMaxListItems

                    If bFirst Then

                        FileSystem.FileGet(m_iRLDFFile, uDetailRecord, lRecordNumber + 1)
                        bFirst = False
                    Else

                        FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)
                    End If
                    'developer guide no. 130(Guide)
                    sPropID = ToSafeString(uDetailRecord.PropertyId).Trim()
                    sDesc = ToSafeString(uDetailRecord.Description).Trim()
                    sABICode = ToSafeString(uDetailRecord.ABICode).Trim()

                    ' CL160699 BEGIN-->

                    ' Look for the +++ char sequence. This means concatenate with
                    ' the following record (and so on...). This gives us
                    ' access to very long list item descriptions.
                    sFullDesc = New StringBuilder("")

                    iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)

                    Do While (iPosPlusPlusPlus <> 0)
                        sFullDesc.Append(sDesc.Substring(0, iPosPlusPlusPlus - 1))


                        FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)

                        ' keep on truckin' mama
                        'developer guide no. 130(Guide)
                        sDesc = ToSafeString(uDetailRecord.Description).Trim()
                        iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)
                    Loop


                    sFullDesc.Append(sDesc)

                    ' <-- END CL160699


                    If v_sPropertyId <> sPropID Then
                        bEOF = True
                    Else

                        sRecord = sFullDesc.ToString()

                        'DB 15/11/99 Start

                        ' If the record matches the search string, clear the list & code list
                        ' Then just put that record into the list & code array.

                        'DB 23/11/99 Start
                        'The search for the perfect match should be non-case sensitive
                        If sRecord.ToLower() = sSearchString.ToLower() Then 'DB 23/11/99 End
                            ReDim r_vListData(0)

                            r_vListData(0) = sRecord
                            If bCodeListWanted Then
                                ReDim r_vListDataCode(0)

                                r_vListDataCode(0) = sABICode
                            End If

                            'Exit out we've found the right record
                            Return result
                        End If

                        'DB 15/11/99 End

                        ' CL280699 BEGIN-->

                        ' Look for search string within the list item
                        ' If the search string ends with an asterisk, then
                        ' see if the list item *begins* with the search string

                        bSearchDone = False

                        If v_vSearchString.Length >= 1 And v_vSearchString.EndsWith("*") Then
                            sSearchString = v_vSearchString.Substring(0, v_vSearchString.Length - 1)
                            lPos = (sRecord.IndexOf(sSearchString, StringComparison.CurrentCultureIgnoreCase) + 1)
                            If lPos <> 1 Then lPos = 0 ' Only interested if list item starts with the search string
                            bSearchDone = True
                        End If

                        ' Look for # char. This allows a search for 2 strings, e.g. "abc#xyz"
                        ' will search for "abc" and "xyz" and only include the item if both
                        ' are present. It will also expect "abc" to be found *before* "xyz"

                        If Not bSearchDone Then

                            k = (v_vSearchString.IndexOf("#"c) + 1)

                            If k > 0 Then
                                sSearchString = v_vSearchString.Substring(0, k - 1)
                                sSearchString2 = v_vSearchString.Substring(v_vSearchString.Length - (v_vSearchString.Length - k))
                                lPos = (sRecord.IndexOf(sSearchString, StringComparison.CurrentCultureIgnoreCase) + 1)
                                lPos2 = (sRecord.IndexOf(sSearchString2, StringComparison.CurrentCultureIgnoreCase) + 1)
                                If (lPos <> 0) And (lPos2 <> 0) And (lPos + sSearchString.Length <= lPos2) Then
                                    lPos = 1 ' Flag as found iff both are found
                                Else
                                    lPos = 0 ' Flag as not found
                                End If

                                bSearchDone = True
                            End If

                        End If

                        If Not bSearchDone Then

                            'DB 12/11/99 Start

                            ' If the multi search boolean is set to true, do the multi search
                            ' & pass the first number of charcters (i.e. 3, 4 or 5) 'DB 16/2/2000

                            If v_bMultiSearch Then
                                m_lReturn = CType(MultiSearch(v_sRecord:=sRecord, v_sSearchString:=sSearchString, v_iFirstNoChars:=v_iFirstNoChars), gPMConstants.PMEReturnCode)

                                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                    lPos = 1
                                Else
                                    lPos = 0
                                End If

                            Else
                                ' Else do the default search
                                lPos = (sRecord.IndexOf(sSearchString, StringComparison.CurrentCultureIgnoreCase) + 1)

                            End If

                            'DB 12/11/99 End

                        End If

                        ' <--END CL280699

                        If lPos > 0 Then
                            'ReDim Preserve r_vListData(I)

                            r_vListData(i) = sFullDesc.ToString()

                            ' CL090699 BEGIN-->
                            If bCodeListWanted Then
                                'ReDim Preserve r_vListDataCode(I)

                                r_vListDataCode(i) = sABICode
                            End If
                            ' <-- END CL090699

                            i += 1
                        End If

                        bEOF = FileSystem.EOF(m_iRLDFFile)
                    End If


                End While
            End If

            If i > 0 Then
                i -= 1
            End If

            ReDim Preserve r_vListData(i)
            If bCodeListWanted Then
                ReDim Preserve r_vListDataCode(i)
            End If

            'sj 18/05/99 - end
            CloseFiles()
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListC failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListC", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: MultiSearch
    '
    ' Description: Performs the multi search required by certain fields
    ' in the OIS product, e.g. Occupation & Employer's Business
    '
    ' 'DB 12/11/99
    ' ***************************************************************** '
    Public Function MultiSearch(ByVal v_sRecord As String, ByVal v_sSearchString As String, ByVal v_iFirstNoChars As Integer) As Integer

        ' The search is performed by looking at the 1st 5 letters in any
        ' of the words given and looking for a match in the 1st 5 letters
        ' of any of the records.

        Dim result As Integer = 0
        Dim vPos As Integer
        Dim startPos As Integer
        Dim vRecordArray() As Object = Nothing
        Dim iRecArraySize As Integer
        Dim vSearchArray() As Object = Nothing
        Dim iSearchArraySize, iFirstNoChars As Integer 'DB 16/2/2000

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Split the record up into separate words and put them in an array

            startPos = 1
            vPos = 10000
            iRecArraySize = 0

            v_sRecord = v_sRecord.Trim()

            Do
                vPos = Informations.InStr(startPos, v_sRecord, " ")

                If Not (Convert.IsDBNull(vPos) Or Informations.IsNothing(vPos)) Then
                    If vPos <> 0 Then
                        ' If we haven't found a double blank,
                        ' put the word into the record array
                        If vPos <> startPos Then
                            iRecArraySize += 1
                            ReDim Preserve vRecordArray(iRecArraySize)

                            vRecordArray(iRecArraySize - 1) = Mid(v_sRecord, startPos, vPos - startPos)
                            'Debug.Print vRecordArray(iRecArraySize - 1)
                        End If

                        startPos = vPos + 1
                    Else
                        ' No more blanks, so it's the end of the record
                        ' Put the final word into the array
                        iRecArraySize += 1
                        ReDim Preserve vRecordArray(iRecArraySize)

                        vRecordArray(iRecArraySize - 1) = Mid(v_sRecord, startPos, (v_sRecord.Length - startPos) + 1)
                        'Debug.Print vRecordArray(iRecArraySize - 1)
                    End If
                End If
            Loop Until vPos = 0 Or Convert.IsDBNull(vPos) Or Informations.IsNothing(vPos)

            ' Split the search string up into separate words and put them in an array

            startPos = 1
            vPos = 10000
            iSearchArraySize = 0

            v_sSearchString = v_sSearchString.Trim()

            Do
                vPos = Informations.InStr(startPos, v_sSearchString, " ")

                If Not (Convert.IsDBNull(vPos) Or Informations.IsNothing(vPos)) Then
                    If vPos <> 0 Then
                        ' If we haven't found a double blank,
                        ' put the word into the record array
                        If vPos <> startPos Then
                            iSearchArraySize += 1
                            ReDim Preserve vSearchArray(iSearchArraySize)

                            vSearchArray(iSearchArraySize - 1) = Mid(v_sSearchString, startPos, vPos - startPos)
                            'Debug.Print vSearchArray(iSearchArraySize - 1)
                        End If

                        startPos = vPos + 1
                    Else
                        ' No more blanks, so it's the end of the record
                        ' Put the final word into the array
                        iSearchArraySize += 1
                        ReDim Preserve vSearchArray(iSearchArraySize)

                        vSearchArray(iSearchArraySize - 1) = Mid(v_sSearchString, startPos, (v_sSearchString.Length - startPos) + 1)
                        'Debug.Print vSearchArray(iSearchArraySize - 1)
                    End If
                End If
            Loop Until vPos = 0 Or Convert.IsDBNull(vPos) Or Informations.IsNothing(vPos)

            ' Loop round each substring in the search string array &
            ' see if there is match with any substring in the record array
            ' using the 'first number of characters' passed of any word only. ('DB 16/2/2000)

            result = gPMConstants.PMEReturnCode.PMFalse

            'If the searchstring is shorter than v_iFirstNochars,
            'set iFirstNochars to the length of the search string
            iFirstNoChars = Math.Min(v_sSearchString.Length, v_iFirstNoChars)

            'But make sure it's at least 3 characters
            If iFirstNoChars < 3 Then
                iFirstNoChars = 3
            End If

            For iIndex As Integer = vSearchArray.GetLowerBound(0) To vSearchArray.GetUpperBound(0) - 1

                For jIndex As Integer = vRecordArray.GetLowerBound(0) To vRecordArray.GetUpperBound(0) - 1


                    If CStr(vSearchArray(iIndex)).Substring(0, iFirstNoChars).ToLower() = CStr(vRecordArray(jIndex)).Substring(0, iFirstNoChars).ToLower() Then
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If
                Next

            Next

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MultiSearchFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="MultiSearch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetDescriptionC (Standard Method)
    '
    ' Description: Returns a desc for a given property idand ABI code.
    '
    ' CL090699
    '
    ' ***************************************************************** '
    Public Function GetDescriptionC(ByVal sPropertyId As String, ByVal sABICodeTarget As String, ByRef sDescription As String) As Integer

        Dim result As Integer = 0
        Dim lRecordNumber As Integer
        'developer guide no. 107(Guide)	
        Dim uDetailRecord As RLDFDetailRecord = RLDFDetailRecord.CreateInstance()
        Dim bEOF, bFirst As Boolean
        Dim sABICode, sPropID, sDesc As String
        Dim sFullDesc As New StringBuilder
        Dim iPosPlusPlusPlus As Integer

        Try

            sDescription = ""

            ' *****************************************************************
            ' Get the record number from the index
            ' *****************************************************************
            m_lReturn = CType(GetListIndex(v_sPropertyId:=sPropertyId, r_lRecordNumber:=lRecordNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'MsgBox "Unable to load list for " & sPropertyId, _
                'vbOKOnly, _
                '"ListManager"
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            bEOF = False
            bFirst = True

            While Not bEOF

                If bFirst Then

                    FileSystem.FileGet(m_iRLDFFile, uDetailRecord, lRecordNumber + 1)
                    bFirst = False
                Else

                    FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)
                End If
                'developer guide no. 130(Guide)
                sPropID = uDetailRecord.PropertyId.ToString.Trim()
                sDesc = uDetailRecord.Description.ToString.Trim()
                sABICode = uDetailRecord.ABICode.ToString.Trim()

                ' CL160699 BEGIN-->

                ' Look for the +++ char sequence. This means concatenate with
                ' the following record (and so on...). This gives us
                ' access to very long list item descriptions.
                sFullDesc = New StringBuilder("")

                iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)

                Do While (iPosPlusPlusPlus <> 0)
                    sFullDesc.Append(sDesc.Substring(0, iPosPlusPlusPlus - 1))

                    FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)

                    ' keep on truckin' mama
                    'developer guide no. 130(Guide)
                    sDesc = uDetailRecord.Description.ToString.Trim()
                    iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)
                Loop

                sFullDesc.Append(sDesc)

                If sPropertyId <> sPropID Then
                    bEOF = True
                Else
                    If sABICodeTarget = sABICode Then
                        sDescription = sFullDesc.ToString()
                        ' found it... exit!
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If

                End If

            End While

            ' If we've reached here then we haven't found a description
            Return gPMConstants.PMEReturnCode.PMError

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDescriptionC failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDescriptionC", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

End Class

