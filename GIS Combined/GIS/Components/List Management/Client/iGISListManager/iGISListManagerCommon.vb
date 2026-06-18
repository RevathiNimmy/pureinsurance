Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
'developer guide no. 129
Imports SharedFiles
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
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    Structure RLDFDetailRecord
        'developer guide no. 130(Guide)	
        <VBFixedString(10)> Public PropertyId As String
        'developer guide no. 130(Guide)	
        <VBFixedString(70)> Public Description As String
        'developer guide no. 130(Guide)	
        <VBFixedString(10)> Public ABICode As String
        Public Shared Function CreateInstance() As RLDFDetailRecord
            Dim result As New RLDFDetailRecord
            'developer guide no. 130(Guide)	
            Return result
        End Function
    End Structure
    'RLDF Index record 
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    Structure RLDFIndexRecord
        'developer guide no. 130 (Guide)
        <VBFixedString(10)> Public PropertyId As String
        Dim RecordNumber As Integer
        Public Shared Function CreateInstance() As RLDFIndexRecord
            Dim result As New RLDFIndexRecord
            'developer guide no. 130 (Guide)
            Return result
        End Function
    End Structure
    Private _cFiles As Collection = Nothing
    Public Property cFiles() As Collection
        Get
            If _cFiles Is Nothing Then
                _cFiles = New Collection()
            End If
            Return _cFiles
        End Get
        Set(ByVal Value As Collection)
            _cFiles = Value
        End Set
    End Property
    'END'

    Private Const ACClass As String = "Common"

    'PN20870 SRF187-262 Insurer Maintenance ABI List erroring
    'These have been moved from Common as the files can be opened by both Interface and InterfaceNoLogin
    'NOTE There could be a problem if different lists are opened by the same process using Interface and InterfaceNologin
    Public m_iFileOpenCounter As Integer 'indicates how many times OpenFiles has been called
    Public m_iRLDFFile As Integer
    Public m_iRLDFFileB As Integer
    Public m_iRLDFIndex As Integer
    Public m_cIndex As Collection
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
        <VBFixedString(1)> _
        Public LeftBracket As String
        <VBFixedString(4)> _
        Public YearStart As String
        <VBFixedString(1)> _
        Public YearDivider As String
        <VBFixedString(4)> _
        Public YearEnd As String
        <VBFixedString(1)> _
        Public Divider1 As String
        <VBFixedString(4)> _
        Public Make As String
        <VBFixedString(1)> _
        Public Divider2 As String
        <VBFixedString(1)> _
        Public Doors As String
        <VBFixedString(1)> _
        Public StyleType As String
        <VBFixedString(1)> _
        Public Divider3 As String
        <VBFixedString(1)> _
        Public FuelType As String
        <VBFixedString(1)> _
        Public Transmission As String
        <VBFixedString(1)> _
        Public Divider4 As String
        <VBFixedString(4)> _
        Public EngineSize As String
        <VBFixedString(1)> _
        Public RightBracket As String
        <VBFixedString(1)> _
        Public Divider5 As String
        <VBFixedString(41)> _
        Public ModelName As String
        <VBFixedString(1)> _
        Public Divider6 As String
        <VBFixedString(20)> _
        Public Identifier As String
        Public Shared Function CreateInstance() As udtVehicleRecord
            Dim result As New udtVehicleRecord
            Return result
        End Function
    End Structure

    Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByVal hpvDest As Integer, ByVal hpvSource As Integer, ByVal cbCopy As Integer)

    Public WriteOnly Property ClassOfBusiness() As String
        Set(ByVal Value As String)
            m_sClassOfBusiness = Value
        End Set
    End Property
    Public WriteOnly Property FileOpenCounter() As Integer
        Set(ByVal Value As Integer)
            m_iFileOpenCounter = Value
        End Set
    End Property
    Public WriteOnly Property RLDFFile() As Integer
        Set(ByVal Value As Integer)
            m_iRLDFFile = Value
        End Set
    End Property
    Public WriteOnly Property RLDFFileB() As Integer
        Set(ByVal Value As Integer)
            m_iRLDFFileB = Value
        End Set
    End Property
    Public WriteOnly Property RLDFIndexx() As Integer
        Set(ByVal Value As Integer)
            m_iRLDFIndex = Value
        End Set
    End Property
    Public WriteOnly Property RLDFFileLength() As Integer
        Set(ByVal Value As Integer)
            m_lRLDFFileLength = Value
        End Set
    End Property

    Public WriteOnly Property Index() As Collection
        Set(ByVal Value As Collection)
            m_cIndex = Value
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
            If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is String) Then
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
            If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is String) Then
                m_oBusiness = Value
            Else
                m_oBusiness = Value
            End If
        End Set
    End Property



    ' ***************************************************************** '
    ' Name: GetListIdsAndNamesC
    '
    ' Description:
    '
    ' History: 19/01/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GetListIdsAndNamesC(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no. 107(Guide)	
            Dim uDetailRecord As RLDFDetailRecord = RLDFDetailRecord.CreateInstance()
            Dim lCnt As Integer

            ReDim r_vResultArray(1, 0)
            lCnt = 0

            For Each vIndex As String In m_cIndex

                FileSystem.FileGet(m_iRLDFFile, uDetailRecord, CInt(Conversion.Val(vIndex)))
                ReDim Preserve r_vResultArray(1, lCnt)

                'developer guide no. 130(Guide)	
                r_vResultArray(0, lCnt) = uDetailRecord.PropertyId.Trim

                'developer guide no. 130(Guide)
                r_vResultArray(1, lCnt) = uDetailRecord.Description.Trim

                lCnt += 1
            Next vIndex

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListIdsAndNamesC Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListIdsAndNamesC", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (Begin)
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

                FileSystem.FileGet(m_iRLDFFileB, sRecord, lByteStart)

                ' search the string for the abi code
                lCnt = 1
                While lCnt < lBufferSize

                    lStart = Strings.InStr(lCnt, sRecord, v_sABICode)

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
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListC failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDescriptionFromABICodeC", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
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

                    lStart = Strings.InStr(lCnt, sRecord, v_sDescription)

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
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListC failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetABICodeFromDescriptionC", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Developer Guide No 101
    Public Function GetVehicleList(ByRef r_vListData As Object, Optional ByVal v_vMake As String = "Missing", Optional ByVal v_vModelChoosen As Object = Nothing, Optional ByVal v_vYear As Object = Nothing, Optional ByVal v_vCC As Object = Nothing, Optional ByVal v_vDoors As Object = Nothing, Optional ByVal v_vFuelType As Object = Nothing, Optional ByVal v_vTransType As Object = Nothing) As Integer
        '(DB 25/11/99 Added 3 extra filter parameters)

        Dim result As Integer = 0
        Try

            Dim lByteStart, lRecordNumber As Integer
            Dim sRecord As String = ""
            Dim lStart, lCnt, lBufferSize, lNumberOfReads, lRemainder, lRow As Integer
            Dim sResult As String = ""
            Dim bMatch As Boolean
            Dim iYearFrom, iYearTo, iYearChoosen, iDoors As Integer 'DB 25/11/99
            Dim iActCC, iCC As Integer
            Dim sMake, sModelChoosen, sFuelType As String 'DB 25/11/99
            Dim sTransType As String = "" 'DB 25/11/99
            Dim bInStrMake, bCheckModelChoosen, bCheckYear, bCheckCC, bCheckDoors As Boolean 'DB 25/11/99
            Dim bCheckFuel As Boolean 'DB 25/11/99
            Dim bCheckTrans As Boolean 'DB 25/11/99
            Dim iPropertyOffset, iDescriptionOffset As Integer
            Dim bCVRestrictionRequired As Boolean
            Dim lVehicleAbiCode As Integer

            'Const m_lNumberOfVehicles = 28000

            'sj 02/02/2001 - start
            If m_lNumberOfVehicles = 0 Then
                m_lNumberOfVehicles = 35000
            End If

            If m_sVehicleListId = "" Then
                m_sVehicleListId = CStr(393220)
            End If

            If m_sClassOfBusiness = m_kClassOfBusinessCV Then
                bCVRestrictionRequired = True
            End If

            'Change constants NUMBER_OF_VEHICLES and m_sVehicleListId to m_lNumberOfVehicles
            ' and m_sVehicleListId respectively
            'sj 02/02/2001 - end

            'Const m_sVehicleListId = 393220

            result = gPMConstants.PMEReturnCode.PMTrue

            'RT 16/03/2000
            ' Changed the If statements splitting the condition into two
            ' seperate If's, as the <> "" condition causes type mismatch if
            ' the parameter IsMissing.
            ' This applies to all but Make which is a string, and so does not work
            ' with IsMissing so instead set parameter default to "Missing" instead

            ' Check that parameter has been passed in by
            ' checking for default
            If v_vMake <> "Missing" And v_vMake <> "" Then
                sMake = v_vMake
                bInStrMake = True
                iPropertyOffset = 21
                iDescriptionOffset = 11
            Else
                iPropertyOffset = 38
                iDescriptionOffset = 28
                sMake = ""
            End If


            If Not Information.IsNothing(v_vModelChoosen) Then
                If v_vModelChoosen <> "" Then
                    sModelChoosen = v_vModelChoosen
                    If bInStrMake Then
                        bCheckModelChoosen = True
                    End If
                End If
            Else
                sModelChoosen = ""
            End If

            ' Must have a make or model
            If sMake = "" And sModelChoosen = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Information.IsNothing(v_vYear) Then
                If v_vYear <> "" Then
                    bCheckYear = True
                    iYearChoosen = CInt(v_vYear)
                End If
            End If


            If Not Information.IsNothing(v_vCC) Then
                If v_vCC <> "" Then
                    bCheckCC = True
                    iCC = CInt(v_vCC)
                End If
            End If

            'DB 25/11/99 Start
            'Check for the extra 3 filters
            '(doors, fuel & transmission)



            If Not Information.IsNothing(v_vDoors) Then
                If v_vDoors <> "" Then
                    bCheckDoors = True
                    iDoors = CInt(v_vDoors)
                End If
            End If


            If Not Information.IsNothing(v_vFuelType) Then
                If v_vFuelType <> "" Then
                    bCheckFuel = True
                    sFuelType = v_vFuelType
                End If
            End If


            If Not Information.IsNothing(v_vTransType) Then
                If v_vTransType <> "" Then
                    bCheckTrans = True
                    sTransType = v_vTransType
                End If
            End If

            'RT 16/03/2000 End

            'DB 25/11/99 End

            ReDim r_vListData(m_lMaxListItems)

            ' *****************************************************************
            ' Get the record number from the index
            ' *****************************************************************
            m_lReturn = CType(GetListIndex(v_sPropertyId:=m_sVehicleListId, r_lRecordNumber:=lRecordNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'Find out the number of reads we need to do
            lNumberOfReads = CInt(m_lNumberOfVehicles / RECORDS_PER_READ)
            If (lNumberOfReads * RECORDS_PER_READ) > m_lNumberOfVehicles Then
                lRemainder = m_lNumberOfVehicles - ((lNumberOfReads - 1) * RECORDS_PER_READ)
            Else
                lRemainder = m_lNumberOfVehicles - (lNumberOfReads * RECORDS_PER_READ)
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

                ' search the string for the make/model
                lCnt = 1
                While lCnt < lBufferSize

                    If bInStrMake Then
                        lStart = Strings.InStr(lCnt, sRecord, sMake)
                    Else
                        lStart = Strings.InStr(lCnt, sRecord, sModelChoosen)
                    End If

                    If lStart > 0 And lStart > iPropertyOffset Then
                        ' check we have the correct property id
                        If m_sVehicleListId = sRecord.Substring(lStart - iPropertyOffset - 1, Math.Min(sRecord.Length, 10)).Trim() Then

                            ' Assign the vehicle
                            bMatch = True
                            sResult = sRecord.Substring(lStart - iDescriptionOffset - 1, Math.Min(sRecord.Length, 90)).Trim()

                            'Only certain ranges required for CV
                            If bCVRestrictionRequired Then
                                lVehicleAbiCode = CInt(Conversion.Val(sResult.Substring(70, Math.Min(sResult.Length, 8))))
                                bMatch = (lVehicleAbiCode > 90300000 And lVehicleAbiCode < 90302999) Or (lVehicleAbiCode > 90309301 And lVehicleAbiCode < 90399999)
                            End If

                            ' Filter for model
                            If bMatch Then
                                If bCheckModelChoosen Then
                                    If (Mid(sResult, 28, 40).IndexOf(sModelChoosen, StringComparison.CurrentCultureIgnoreCase) + 1) = 0 Then
                                        bMatch = False
                                    End If
                                End If
                            End If

                            ' filter for year
                            If bMatch Then
                                If bCheckYear Then
                                    iYearFrom = Conversion.Val(sResult.Substring(1, Math.Min(sResult.Length, 4)))
                                    iYearTo = Conversion.Val(sResult.Substring(6, Math.Min(sResult.Length, 4)))
                                    If ((iYearChoosen >= iYearFrom And iYearChoosen <= iYearTo) Or (iYearFrom = 0 And iYearChoosen <= iYearTo) Or (iYearChoosen >= iYearFrom And iYearTo = 0) Or (iYearFrom = 0 And iYearTo = 0)) Or ((iYearChoosen + 1 >= iYearFrom And iYearChoosen + 1 <= iYearTo) Or (iYearFrom = 0 And iYearChoosen + 1 <= iYearTo) Or (iYearChoosen + 1 >= iYearFrom And iYearTo = 0) Or (iYearFrom = 0 And iYearTo = 0)) Or ((iYearChoosen - 1 >= iYearFrom And iYearChoosen - 1 <= iYearTo) Or (iYearFrom = 0 And iYearChoosen - 1 <= iYearTo) Or (iYearChoosen - 1 >= iYearFrom And iYearTo = 0) Or (iYearFrom = 0 And iYearTo = 0)) Then
                                        '-- RWJ 31/03/00 - Ammended the above filter
                                        '-- to allow selection of year of manufacture
                                        '-- to be one year before or one year after the
                                        '-- choosen date -- RWJ End
                                    Else
                                        bMatch = False
                                    End If
                                End If
                            End If

                            'filter for engine cc
                            If bMatch Then
                                If bCheckCC Then
                                    iActCC = Conversion.Val(sResult.Substring(22, Math.Min(sResult.Length, 4)))
                                    If iCC < (iActCC + 50) And iCC > (iActCC - 50) Then
                                    Else
                                        bMatch = False
                                    End If
                                End If
                            End If
                            'DB 25/11/99 Start

                            'Add extra filters for number of doors, fuel & transmission types

                            'Filter for number of doors
                            If bMatch Then
                                If bCheckDoors Then
                                    iDoors = Conversion.Val(sResult.Substring(16, 1))
                                    If iDoors = CInt(v_vDoors) Then
                                    Else
                                        bMatch = False
                                    End If
                                End If
                            End If

                            'Filter for fuel type
                            If bMatch Then
                                If bCheckFuel Then
                                    sFuelType = sResult.Substring(19, 1)
                                    If sFuelType = v_vFuelType Then
                                    Else
                                        bMatch = False
                                    End If
                                End If
                            End If

                            'Filter for transmission type
                            If bMatch Then
                                If bCheckTrans Then
                                    sTransType = sResult.Substring(20, 1)
                                    If sTransType = v_vTransType Then
                                    Else
                                        bMatch = False
                                    End If
                                End If
                            End If

                            'DB 25/11/99 End

                            If bMatch Then


                                r_vListData(lRow) = sRecord.Substring(lStart - iDescriptionOffset - 1, Math.Min(sRecord.Length, 90))
                                lRow += 1

                                If lRow > m_lMaxListItems Then
                                    ReDim Preserve r_vListData(lRow - 1)
                                    Return result
                                End If

                            End If

                            lCnt = lStart + RLDF_RECORD_LENGTH


                        Else
                            lCnt = lStart + 1
                        End If
                    Else
                        lCnt = lBufferSize + 1
                    End If
                End While

                sRecord = ""

                lRecordNumber += RECORDS_PER_READ

            Next lRecordCnt
            If lRow > 0 Then
                ReDim Preserve r_vListData(lRow - 1)
            Else

                r_vListData = ""
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListC failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            '    Resume

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetVehicleModels
    '
    ' Description: Adaptation of GetVehicleList,
    ' which gets the vehicle models ONLY from the given make.
    '
    ' 'DB 31/1/2000
    ' ***************************************************************** '
    Public Function GetVehicleModels(ByRef r_vListData As Object, ByVal v_vMake As String) As Integer

        Dim result As Integer = 0
        Try

            Dim lByteStart, lRecordNumber As Integer
            Dim sRecord As String = ""
            Dim lStart, lCnt, lBufferSize, lNumberOfReads, lRemainder, lRow As Integer
            Dim sResult As String = ""
            Dim bMatch As Boolean
            Dim iYearFrom, iYearTo As Integer
            Dim sMake As String
            Dim bInStrMake As Boolean
            Dim iPropertyOffset, iDescriptionOffset As Integer

            Dim sModel As String = ""
            Dim iPos As Integer

            'DB 18/2/2000 Start
            'Dim colModels As Collection
            Dim vModelsArray() As Object
            Dim bFirstTime As Boolean
            Dim iNewArraySize As Integer
            'DB 18/2/2000 End

            Dim sMakeDescription As String = ""
            Dim bCVRestrictionRequired As Boolean
            Dim lVehicleAbiCode As Integer

            Const NUMBER_OF_VEHICLES As Integer = 50000
            Const PROPERTY_ID As Integer = 393220

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sClassOfBusiness = m_kClassOfBusinessCV Then
                bCVRestrictionRequired = True
            End If

            'DB 18/2/2000 Start

            'Remove the collection (colModels) & turn it into an array
            'It seems to be causing us problems again,
            'when called from the web pages.

            If m_sVehicleListId = "" Then
                m_sVehicleListId = CStr(PROPERTY_ID)
            End If

            'Set colModels = New Collection
            bFirstTime = True

            If Not False And v_vMake <> "" Then
                sMake = v_vMake
                bInStrMake = True
                iPropertyOffset = 21
                iDescriptionOffset = 11
            Else
                iPropertyOffset = 38
                iDescriptionOffset = 28
                sMake = ""
            End If

            ' Must have a make
            If sMake = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(GetDescriptionFromABICodeC(v_sPropertyId:=CStr(393219), v_sABICode:=sMake, r_sDescription:=sMakeDescription), gPMConstants.PMEReturnCode)

            ReDim r_vListData(m_lMaxListItems)

            ' *****************************************************************
            ' Get the record number from the index
            ' *****************************************************************
            m_lReturn = CType(GetListIndex(v_sPropertyId:=m_sVehicleListId, r_lRecordNumber:=lRecordNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'Find out the number of reads we need to do
            lNumberOfReads = CInt(NUMBER_OF_VEHICLES / RECORDS_PER_READ)
            If (lNumberOfReads * RECORDS_PER_READ) > NUMBER_OF_VEHICLES Then
                lRemainder = NUMBER_OF_VEHICLES - ((lNumberOfReads - 1) * RECORDS_PER_READ)
            Else
                lRemainder = NUMBER_OF_VEHICLES - (lNumberOfReads * RECORDS_PER_READ)
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
                    If lRow > 0 Then
                        ReDim Preserve r_vListData(lRow - 1)
                    Else

                        r_vListData = ""
                    End If
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

                ' search the string for the make/model
                lCnt = 1
                While lCnt < lBufferSize

                    If bInStrMake Then
                        lStart = Strings.InStr(lCnt, sRecord, sMake)
                    End If

                    If lStart > iPropertyOffset Then
                        ' check we have the correct property id
                        If m_sVehicleListId = sRecord.Substring(lStart - iPropertyOffset - 1, Math.Min(sRecord.Length, 10)).Trim() Then

                            ' Assign the vehicle
                            bMatch = True
                            sResult = sRecord.Substring(lStart - iDescriptionOffset - 1, Math.Min(sRecord.Length, 90)).Trim()

                            'Only certain ranges required for CV
                            If bCVRestrictionRequired Then
                                lVehicleAbiCode = CInt(Conversion.Val(sResult.Substring(70, Math.Min(sResult.Length, 8))))
                                bMatch = (lVehicleAbiCode > 90300000 And lVehicleAbiCode < 90302999) Or (lVehicleAbiCode > 90309301 And lVehicleAbiCode < 90309999)
                            End If

                            'Get the model
                            sModel = Mid(sResult, 29, 42).Trim()
                            'Check that the year range is valid
                            iYearFrom = Conversion.Val(sResult.Substring(1, Math.Min(sResult.Length, 4)))
                            iYearTo = Conversion.Val(sResult.Substring(6, Math.Min(sResult.Length, 4)))

                            'If year range is 0000 - 0000 then extract the
                            'second word onwards if the first is the manufacturer
                            If iYearFrom = 0 And iYearTo = 0 Then
                                If sMakeDescription.Trim().ToUpper() = sModel.Substring(0, 4).Trim().ToUpper() Then
                                    iPos = (sModel.IndexOf(" "c) + 1)
                                    If iPos <> 0 Then
                                        sModel = sModel.Substring(iPos - 1, Math.Min(sModel.Length, sModel.Length - (iPos - 1))).Trim().ToUpper()
                                    Else
                                        'Only one word, so return false
                                        bMatch = False
                                    End If
                                End If

                            End If

                            'BD - 9/10/02 - If make is in the model name then remove it, causes problems with MG
                            If (sModel.IndexOf(sMakeDescription.Trim() & " ", StringComparison.CurrentCultureIgnoreCase) + 1) = 1 Then
                                sModel = sModel.Substring(sModel.Length - (sModel.Length - v_vMake.Trim().Length - 1))
                            End If

                            iPos = (sModel.IndexOf(" "c) + 1)

                            'Trim to the first word (simple model description)
                            'if more than one word.
                            If iPos <> 0 Then
                                sModel = sModel.Substring(0, Math.Min(sModel.Length, iPos)).Trim().ToUpper()
                            End If

                            'Check that this model hasn't already been found
                            'by searching in the collection of models already found.

                            If bMatch Then
                                If Not bFirstTime Then
                                    For Each vModelsArray_item As Object In vModelsArray

                                        If sModel = CStr(vModelsArray_item) Or sModel = "" Then
                                            bMatch = False
                                            Exit For
                                        End If
                                    Next vModelsArray_item
                                End If
                            End If

                            'If it's a model match that hasn't been found then
                            'add it to the models found array, and add
                            'the full record to the output array.

                            If bMatch Then

                                If bFirstTime Then
                                    ReDim vModelsArray(0) 'First Time
                                    bFirstTime = False
                                Else
                                    iNewArraySize = vModelsArray.GetUpperBound(0) + 1
                                    ReDim Preserve vModelsArray(iNewArraySize)
                                End If


                                vModelsArray(vModelsArray.GetUpperBound(0)) = sModel.Trim()

                                If sRecord.Substring(lStart - iDescriptionOffset - 1, Math.Min(sRecord.Length, 90)).Trim() = "" Then
                                    sModel = ""
                                End If


                                r_vListData(lRow) = sRecord.Substring(lStart - iDescriptionOffset - 1, Math.Min(sRecord.Length, 90))

                                lRow += 1

                                If lRow > m_lMaxListItems Then
                                    ReDim Preserve r_vListData(lRow - 1)
                                    Return result
                                End If

                            End If

                            lCnt = lStart + RLDF_RECORD_LENGTH

                        Else
                            lCnt = lStart + 1
                        End If
                    Else
                        lCnt = lBufferSize + 1
                    End If
                End While

                sRecord = ""

                lRecordNumber += RECORDS_PER_READ

            Next lRecordCnt
            If lRow > 0 Then
                ReDim Preserve r_vListData(lRow - 1)
            Else

                r_vListData = ""
            End If

            'Set colModels = Nothing

            'DB 18/2/2000 End

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListC failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleModels", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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


            bCodeListWanted = Information.IsNothing(r_vListDataCode)

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

                    sPropID = uDetailRecord.PropertyId.Trim()
                    sDesc = uDetailRecord.Description.Trim()
                    sABICode = uDetailRecord.ABICode.Trim()

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
                        sDesc = uDetailRecord.Description.Trim()
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
                    sPropID = uDetailRecord.PropertyId.Trim()
                    sDesc = uDetailRecord.Description.Trim()
                    sABICode = uDetailRecord.ABICode.Trim()

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
                        sDesc = uDetailRecord.Description.Trim()
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
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListC failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListC", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        'DB 6/1/2000 Start

        ' The previous code in this method was causing problems with the interface/IIS
        ' It has been rewritten using arrays instead of collections and hopefully with
        ' a slightly more efficient searching method ...


        '    Dim colRecordColl As Collection
        '    Dim colSearchStringColl As Collection
        Dim result As Integer = 0
        '    Dim myStr As Variant
        '    Dim sReturnString As String

        Dim vPos As Integer
        Dim startPos As Integer
        Dim vRecordArray() As Object
        Dim iRecArraySize As Integer
        Dim vSearchArray() As Object
        Dim iSearchArraySize, iFirstNoChars As Integer 'DB 16/2/2000

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Split the record up into separate words and put them in an array

            startPos = 1
            vPos = 10000
            iRecArraySize = 0

            v_sRecord = v_sRecord.Trim()


            Do
                vPos = Strings.InStr(startPos, v_sRecord, " ", 1)

                If Not (Convert.IsDBNull(vPos) Or IsNothing(vPos)) Then
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
            Loop Until vPos = 0 Or Convert.IsDBNull(vPos) Or IsNothing(vPos)

            ' Split the search string up into separate words and put them in an array

            startPos = 1
            vPos = 10000
            iSearchArraySize = 0

            v_sSearchString = v_sSearchString.Trim()


            Do
                vPos = Strings.InStr(startPos, v_sSearchString, " ", 1)

                If Not (Convert.IsDBNull(vPos) Or IsNothing(vPos)) Then
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
            Loop Until vPos = 0 Or Convert.IsDBNull(vPos) Or IsNothing(vPos)

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

            '    ' Tokenize the two strings and shove them in a couple of collections.
            '
            '    sReturnString = ""
            '    jIndex = 0


            '    ' Loop round each character in the record searching for spaces
            '    ' & chop the words up into sub strings, then put the first 5 characters
            '    ' of each word into the collection ...

            '    For iIndex = 1 To Len(v_sRecord)
            '
            '        ' Final character in the record ...
            '        If iIndex = Len(v_sRecord) Then
            '            sReturnString = sReturnString & Mid(v_sRecord, iIndex, 1)
            '
            '            colRecordColl.Add Item:=Mid$(sReturnString, 1, 5), Key:=CStr(jIndex)
            '            jIndex = jIndex + 1
            '            sReturnString = ""
            '
            '        ' Character in the middle of a word ...
            '        ElseIf InStr(iIndex, v_sRecord, " ", vbTextCompare) <> iIndex And _
            ''               iIndex <> Len(v_sRecord) Then
            '
            '            sReturnString = sReturnString & Mid(v_sRecord, iIndex, 1)
            '
            '        ' Character at the end of a word (but not the end of the record) ...
            '        Else
            '            colRecordColl.Add Item:=Mid$(sReturnString, 1, 5), Key:=CStr(jIndex)
            '            jIndex = jIndex + 1
            '            sReturnString = ""
            '        End If
            '
            '    Next
            '
            '    ' Loop round each character in the search string searching for spaces
            '    ' & chop the words up into sub strings ...
            '
            '    For iIndex = 1 To Len(v_sSearchString)
            '
            '        ' Final character in the search string ...
            '        If iIndex = Len(v_sSearchString) Then
            '            sReturnString = sReturnString & Mid(v_sSearchString, iIndex, 1)
            '
            '            colSearchStringColl.Add Item:=Mid$(sReturnString, 1, 5), Key:=CStr(jIndex)
            '            jIndex = jIndex + 1
            '            sReturnString = ""
            '
            '        ' Character in the middle of a word ...
            '        ElseIf InStr(iIndex, v_sSearchString, " ", vbTextCompare) <> iIndex And _
            ''               iIndex <> Len(v_sSearchString) Then
            '
            '            sReturnString = sReturnString & Mid(v_sSearchString, iIndex, 1)
            '
            '        ' Character at the end of a word (but not the end of the record) ...
            '        Else
            '            colSearchStringColl.Add Item:=Mid$(sReturnString, 1, 5), Key:=CStr(jIndex)
            '            jIndex = jIndex + 1
            '            sReturnString = ""
            '        End If
            '
            '    Next
            '
            '    ' Loop round each substring in the record collection &
            '    ' see if there is match with any substring in the search string collection.
            '
            '    Dim vSubRecordString As Variant
            '    Dim vSubSearchString As Variant
            '
            '    MultiSearch = PMFalse
            '    For Each vSubRecordString In colRecordColl
            '
            '        For Each vSubSearchString In colSearchStringColl
            '            If LCase(vSubRecordString) = LCase(vSubSearchString) Then
            '                MultiSearch = PMTrue
            '                Exit Function
            '            End If
            '        Next
            '    Next

            'DB 6/1/2000 End

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MultiSearchFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="MultiSearch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RemoveDoubleQuotes
    '
    ' Description: Removes double quotes from a string
    '
    ' DB (16/04/1999)
    ' ***************************************************************** '
    Public Function RemoveDoubleQuotes(ByVal v_sStringToStrip As String) As String

        Dim result As String = String.Empty
        Dim sReturnString As New StringBuilder

        Try

            result = CStr(gPMConstants.PMEReturnCode.PMTrue)

            sReturnString = New StringBuilder("")

            For lIndex As Integer = 1 To v_sStringToStrip.Length
                If Strings.InStr(lIndex, v_sStringToStrip, """", CompareMethod.Text) <> lIndex Then
                    sReturnString.Append(Mid(v_sStringToStrip, lIndex, 1))
                End If
            Next


            Return sReturnString.ToString()

        Catch excep As System.Exception



            result = CStr(gPMConstants.PMEReturnCode.PMError)

            'Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveDoubleQuotesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveDoubleQuotes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: PopulateListControlC (Standard Method)
    '
    ' Description: Returns a list in a variant array for a given
    '              property id.
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function PopulateListControlC(ByVal v_sPropertyId As String, ByRef r_oControl As Object, Optional ByVal v_vSearchString As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordNumber As Integer
        'developer guide no. 107(Guide)	
        Dim uDetailRecord As RLDFDetailRecord = RLDFDetailRecord.CreateInstance()
        Dim lPos As Integer
        Dim sSearchString, sRecord As String
        Dim i As Integer
        Dim bEOF, bFirst As Boolean
        Dim sABICode, sPropID, sDesc As String
        Dim sFullDesc As New StringBuilder
        Dim iPosPlusPlusPlus As Integer
        Dim bSearchDone As Boolean
        Dim sSearchString2 As String = ""
        Dim lPos2 As Integer
        Dim k As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            i = 0


            r_oControl.Clear()

            ' *****************************************************************
            ' Get the record number from the index
            ' *****************************************************************
            m_lReturn = CType(GetListIndex(v_sPropertyId:=v_sPropertyId, r_lRecordNumber:=lRecordNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                'MsgBox "Unable to load list for " & v_sPropertyId, _
                'vbOKOnly, _
                '"ListManager"
                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            'sj 18/05/99 - start
            ' Pick up last record of last list

            bEOF = False
            bFirst = True


            If Information.IsNothing(v_vSearchString) Then

                While Not bEOF And i < m_lMaxListItems

                    If bFirst Then

                        FileSystem.FileGet(m_iRLDFFile, uDetailRecord, lRecordNumber + 1)
                        bFirst = False
                    Else

                        FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)
                    End If
                    ' developer guide no. 130(Guide)
                    sPropID = uDetailRecord.PropertyId.Trim()
                    sDesc = uDetailRecord.Description.Trim()
                    sABICode = uDetailRecord.ABICode.Trim()

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
                        sDesc = uDetailRecord.Description.Trim()
                        iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)
                    Loop


                    sFullDesc.Append(sDesc)

                    ' <-- END CL160699

                    If v_sPropertyId <> sPropID Then
                        bEOF = True
                    Else

                        r_oControl.AddItem(sFullDesc)
                        i += 1
                        bEOF = FileSystem.EOF(m_iRLDFFile)
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
                    sPropID = uDetailRecord.PropertyId.Trim()
                    sDesc = uDetailRecord.Description.Trim()
                    sABICode = uDetailRecord.ABICode.Trim()

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
                        sDesc = uDetailRecord.Description.Trim()
                        iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)
                    Loop


                    sFullDesc.Append(sDesc)

                    ' <-- END CL160699

                    If v_sPropertyId <> sPropID Then
                        bEOF = True
                    Else

                        sRecord = sFullDesc.ToString()

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
                            ' do the default search
                            lPos = (sRecord.IndexOf(sSearchString, StringComparison.CurrentCultureIgnoreCase) + 1)
                        End If

                        ' <--END CL280699


                        If lPos > 0 Then

                            r_oControl.AddItem(sRecord)
                            i += 1
                        End If

                        bEOF = FileSystem.EOF(m_iRLDFFile)
                    End If


                End While
            End If

            'sj 18/05/99 - end


            If r_oControl.ListCount > 0 Then

                r_oControl.ListIndex = 0
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Populate List Control", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateListControlC", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                sPropID = uDetailRecord.PropertyId.Trim()
                sDesc = uDetailRecord.Description.Trim()
                sABICode = uDetailRecord.ABICode.Trim()

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
                    sDesc = uDetailRecord.Description.Trim()
                    iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)
                Loop


                sFullDesc.Append(sDesc)

                ' <-- END CL160699


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

        m_cIndex = New Collection()

        

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim iCtr As Integer = 1
            'sj 18/05/99 - start
            ' For random files EOF seems to be set on the last record NOT!! after it
            bEOF = False

            While Not bEOF

                Try
                    FileSystem.FileGet(m_iRLDFIndex, uIndexRecord, iCtr)
                    'developer guide no. 130(Guide)
                    m_cIndex.Add(uIndexRecord.RecordNumber, uIndexRecord.PropertyId.ToString.Trim)

                    bEOF = FileSystem.EOF(m_iRLDFIndex)

                    iCtr += 1

                Catch ex As EndOfStreamException
                    bEOF = True
                End Try
            End While
            'sj 18/05/88 - end

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetListIndex (Standard Method)
    '
    ' Description: Gets the record number from the collection
    '
    ' ***************************************************************** '
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
    Public Function CheckListVersionsC(ByVal v_sGisDataModelCode As String, Optional ByVal v_sSellerCode As String = "") As Integer

        Dim result As Integer = 0
        Dim sExistingClientRLDF As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetServerSettings(v_sGisDataModelCode, v_sSellerCode), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error getting server registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckListVersions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' *****************************************************************
            ' Open the RLDF files
            ' *****************************************************************

            Return OpenFiles()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckListVersionsC failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckListVersionsC", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

                m_sServerListFilePathIdx = m_sServerListFilePath & _
                                           v_sGisDataModelCode & _
                                           v_sSellerCode & _
                                           "List.idx"

                m_sServerListFilePathDat = m_sServerListFilePath & _
                                           v_sGisDataModelCode & _
                                           v_sSellerCode & _
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
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Server Settings failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServerSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    'IJM 18/07/2003 Added this method to get filter model list by style (i.e. number of doors)
    Public Function GetVehicleStyleList(ByRef r_vListData As Object, ByVal v_vMake As String, ByVal v_sModel As String, ByVal v_lYear As Integer) As Integer

        Dim result As Integer = 0
        Try

            Dim lByteStart, lRecordNumber As Integer
            Dim sRecord As String = ""
            Dim lStart, lCnt, lBufferSize, lNumberOfReads, lRemainder, lRow As Integer
            Dim sResult As String = ""
            Dim bMatch As Boolean
            Dim iYearFrom, iYearTo, iYearChoosen As Integer 'DB 25/11/99
            Dim iActCC, iCC As Integer
            Dim sMake, sModelChoosen As String 'DB 25/11/99
            Dim sTransType As String = "" 'DB 25/11/99
            Dim bInStrMake, bCheckModelChoosen, bCheckYear, bCheckCC As Boolean 'DB 25/11/99
            Dim iPropertyOffset, iDescriptionOffset As Integer

            Dim m_lNumberOfVehicles As Integer
            Dim m_sVehicleListId As String = ""

            'sj 02/02/2001 - start
            If m_lNumberOfVehicles = 0 Then
                m_lNumberOfVehicles = 35000
            End If

            If m_sVehicleListId.Length = 0 Then
                m_sVehicleListId = CStr(393220)
            End If
            'Change constants NUMBER_OF_VEHICLES and m_sVehicleListId to m_lNumberOfVehicles
            ' and m_sVehicleListId respectively
            'sj 02/02/2001 - end

            'Const m_sVehicleListId = 393220

            result = gPMConstants.PMEReturnCode.PMTrue

            'RT 16/03/2000
            ' Changed the If statements splitting the condition into two
            ' seperate If's, as the <> "" condition causes type mismatch if
            ' the parameter IsMissing.
            ' This applies to all but Make which is a string, and so does not work
            ' with IsMissing so instead set parameter default to "Missing" instead

            ' Check that parameter has been passed in by
            ' checking for default
            If v_vMake <> "Missing" And v_vMake <> "" Then
                sMake = v_vMake
                bInStrMake = True
                iPropertyOffset = 21
                iDescriptionOffset = 11
            Else
                iPropertyOffset = 38
                iDescriptionOffset = 28
                sMake = ""
            End If

            If Not False Then
                If v_sModel.Length <> 0 Then
                    sModelChoosen = v_sModel
                    If bInStrMake Then
                        bCheckModelChoosen = True
                    End If
                End If
            Else
                sModelChoosen = ""
            End If

            ' Must have a make or model
            If sMake.Length = 0 And sModelChoosen.Length = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not False Then
                If v_lYear <> StringsHelper.ToDoubleSafe("") Then
                    bCheckYear = True
                    iYearChoosen = v_lYear
                End If
            End If

            'RT 16/03/2000 End

            'DB 25/11/99 End

            ReDim r_vListData(m_lMaxListItems)



            ' *****************************************************************
            ' Get the record number from the index
            ' *****************************************************************
            m_lReturn = CType(GetListIndex(v_sPropertyId:=m_sVehicleListId, r_lRecordNumber:=lRecordNumber), gPMConstants.PMEReturnCode)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Find out the number of reads we need to do
            lNumberOfReads = CInt(m_lNumberOfVehicles / RECORDS_PER_READ)
            If (lNumberOfReads * RECORDS_PER_READ) > m_lNumberOfVehicles Then
                lRemainder = m_lNumberOfVehicles - ((lNumberOfReads - 1) * RECORDS_PER_READ)
            Else
                lRemainder = m_lNumberOfVehicles - (lNumberOfReads * RECORDS_PER_READ)
                If lRemainder > 0 Then
                    lNumberOfReads += 1
                End If
            End If

            ' Convert remainder to bytes
            lRemainder *= RLDF_RECORD_LENGTH

            Dim oYearRange As iRange
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

                ' search the string for the make/model
                lCnt = 1
                While lCnt < lBufferSize

                    If bInStrMake Then
                        lStart = Strings.InStr(lCnt, sRecord, sMake)
                    Else
                        lStart = Strings.InStr(lCnt, sRecord, sModelChoosen)
                    End If

                    If lStart > 0 And lStart > iPropertyOffset Then
                        ' check we have the correct property id
                        If m_sVehicleListId = sRecord.Substring(lStart - iPropertyOffset - 1, Math.Min(sRecord.Length, 10)).Trim() Then












                            ' Assign the vehicle
                            bMatch = True
                            sResult = sRecord.Substring(lStart - iDescriptionOffset - 1, Math.Min(sRecord.Length, 90)).Trim()

                            ' Filter for model
                            If bCheckModelChoosen Then
                                If (Mid(sResult, 28, 40).IndexOf(sModelChoosen, StringComparison.CurrentCultureIgnoreCase) + 1) = 0 Then
                                    bMatch = False
                                End If
                            End If

                            ' filter for year
                            If bCheckYear Then
                                iYearFrom = Conversion.Val(sResult.Substring(1, Math.Min(sResult.Length, 4)))
                                iYearTo = Conversion.Val(sResult.Substring(6, Math.Min(sResult.Length, 4)))


                                oYearRange = New cYearManufactureRange()

                                oYearRange.StartValue = iYearFrom
                                oYearRange.EndValue = iYearTo

                                If Not oYearRange.Includes(iYearChoosen) Then
                                    bMatch = False
                                End If

                                oYearRange = Nothing

                                If ((iYearChoosen >= iYearFrom And iYearChoosen <= iYearTo) Or (iYearFrom = 0 And iYearChoosen <= iYearTo) Or (iYearChoosen >= iYearFrom And iYearTo = 0) Or (iYearFrom = 0 And iYearTo = 0)) Or ((iYearChoosen + 1 >= iYearFrom And iYearChoosen + 1 <= iYearTo) Or (iYearFrom = 0 And iYearChoosen + 1 <= iYearTo) Or (iYearChoosen + 1 >= iYearFrom And iYearTo = 0) Or (iYearFrom = 0 And iYearTo = 0)) Or ((iYearChoosen - 1 >= iYearFrom And iYearChoosen - 1 <= iYearTo) Or (iYearFrom = 0 And iYearChoosen - 1 <= iYearTo) Or (iYearChoosen - 1 >= iYearFrom And iYearTo = 0) Or (iYearFrom = 0 And iYearTo = 0)) Then
                                    '-- RWJ 31/03/00 - Ammended the above filter
                                    '-- to allow selection of year of manufacture
                                    '-- to be one year before or one year after the
                                    '-- choosen date -- RWJ End
                                Else
                                    bMatch = False
                                End If
                            End If

                            'filter for engine cc
                            If bCheckCC Then
                                iActCC = Conversion.Val(sResult.Substring(22, Math.Min(sResult.Length, 4)))
                                If iCC < (iActCC + 50) And iCC > (iActCC - 50) Then
                                Else
                                    bMatch = False
                                End If
                            End If

                            If bMatch Then


                                r_vListData(lRow) = sRecord.Substring(lStart - iDescriptionOffset - 1, Math.Min(sRecord.Length, 90))
                                lRow += 1

                                If lRow > m_lMaxListItems Then
                                    ReDim Preserve r_vListData(lRow - 1)
                                    Return result
                                End If

                            End If

                            lCnt = lStart + RLDF_RECORD_LENGTH
                        Else
                            lCnt = lStart + 1
                        End If
                    Else
                        lCnt = lBufferSize + 1
                    End If
                End While

                sRecord = ""

                lRecordNumber += RECORDS_PER_READ


            Next lRecordCnt

            If lRow > 0 Then
                ReDim Preserve r_vListData(lRow - 1)
            Else

                r_vListData = ""



            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListC failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Public Function GetVehicleListI4M(ByRef r_vListData() As Object, Optional ByVal v_vMake As String = "Missing", Optional ByVal v_vModelChoosen As String = "", Optional ByVal v_vYear As Integer = 0, Optional ByVal v_vCC As Integer = 0, Optional ByVal v_vDoors As Integer = 0, Optional ByVal v_vFuelType As String = "", Optional ByVal v_vTransType As String = "", Optional ByVal v_vModelID As Integer = 0) As Integer
        '(DB 25/11/99 Added 3 extra filter parameters)

        Dim result As Integer = 0
        Try

            Dim lByteStart, lRecordNumber As Integer
            Dim sRecord As String = ""
            Dim lStart, lCnt, lBufferSize, lNumberOfReads, lRemainder, lRow As Integer
            Dim sResult As String = ""
            Dim bMatch As Boolean
            Dim iYearFrom, iYearTo, iYearChoosen, iDoors As Integer 'DB 25/11/99
            Dim iActCC, iCC As Integer
            Dim sMake, sModelChoosen, sFuelType As String 'DB 25/11/99
            Dim sTransType As String = "" 'DB 25/11/99
            Dim bInStrMake, bCheckModelChoosen, bCheckYear, bCheckCC, bCheckDoors As Boolean 'DB 25/11/99
            Dim bCheckFuel As Boolean 'DB 25/11/99
            Dim bCheckTrans As Boolean 'DB 25/11/99
            Dim bCheckModelID As Boolean
            Dim iPropertyOffset, iDescriptionOffset As Integer


            Dim udtRecordLine As udtVehicleRecord = udtVehicleRecord.CreateInstance()
            Dim oYearRange As iRange

            oYearRange = New cYearManufactureRange()

            'Const m_lNumberOfVehicles = 28000



            result = gPMConstants.PMEReturnCode.PMTrue

            'sj 02/02/2001 - start
            If m_lNumberOfVehicles = 0 Then
                m_lNumberOfVehicles = 35000
            End If

            If m_sVehicleListId.Length = 0 Then
                m_sVehicleListId = CStr(393220)
            End If
            'Change constants NUMBER_OF_VEHICLES and m_sVehicleListId to m_lNumberOfVehicles
            ' and m_sVehicleListId respectively
            'sj 02/02/2001 - end

            'Const m_sVehicleListId = 393220

            'RT 16/03/2000
            ' Changed the If statements splitting the condition into two
            ' seperate If's, as the <> "" condition causes type mismatch if
            ' the parameter IsMissing.
            ' This applies to all but Make which is a string, and so does not work
            ' with IsMissing so instead set parameter default to "Missing" instead

            ' Check that parameter has been passed in by
            ' checking for default
            If v_vMake <> "Missing" And v_vMake.Length <> 0 Then
                sMake = v_vMake
                bInStrMake = True
                iPropertyOffset = 21
                iDescriptionOffset = 11
            Else
                iPropertyOffset = 38
                iDescriptionOffset = 28
                sMake = ""
            End If


            If Not Information.IsNothing(v_vModelChoosen) Then
                If v_vModelChoosen.Length <> 0 Then
                    sModelChoosen = v_vModelChoosen
                    If bInStrMake Then
                        bCheckModelChoosen = True
                    End If
                End If
            Else
                sModelChoosen = ""
            End If

            ' Must have a make or model
            If (sMake.Length = 0 And sModelChoosen.Length = 0) And v_vModelID = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Information.IsNothing(v_vYear) Then

                If Marshal.SizeOf(v_vYear) <> 0 Then
                    bCheckYear = True
                    iYearChoosen = v_vYear
                End If
            End If


            If Not Information.IsNothing(v_vCC) Then

                If Marshal.SizeOf(v_vCC) <> 0 Then
                    bCheckCC = True
                    iCC = v_vCC
                End If
            End If

            'DB 25/11/99 Start
            'Check for the extra 3 filters
            '(doors, fuel & transmission)

            If Not Information.IsNothing(v_vDoors) Then

                If Marshal.SizeOf(v_vDoors) <> 0 Then
                    bCheckDoors = True
                    iDoors = v_vDoors
                End If
            End If


            If Not Information.IsNothing(v_vFuelType) Then
                If v_vFuelType.Length <> 0 Then
                    bCheckFuel = True
                    sFuelType = v_vFuelType
                End If
            End If


            If Not Information.IsNothing(v_vTransType) Then
                If v_vTransType.Length <> 0 Then
                    bCheckTrans = True
                    sTransType = v_vTransType
                End If
            End If

            'RT 16/03/2000 End

            'DB 25/11/99 End

            If Not False Then
                If v_vModelID <> 0 Then
                    bCheckModelID = True
                End If
            End If

            ReDim r_vListData(m_lMaxListItems)

            ' *****************************************************************
            ' Get the record number from the index
            ' *****************************************************************
            m_lReturn = CType(GetListIndex(v_sPropertyId:=m_sVehicleListId, r_lRecordNumber:=lRecordNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Find out the number of reads we need to do
            lNumberOfReads = CInt(m_lNumberOfVehicles / RECORDS_PER_READ)
            If (lNumberOfReads * RECORDS_PER_READ) > m_lNumberOfVehicles Then
                lRemainder = m_lNumberOfVehicles - ((lNumberOfReads - 1) * RECORDS_PER_READ)
            Else
                lRemainder = m_lNumberOfVehicles - (lNumberOfReads * RECORDS_PER_READ)
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

                ' search the string for the make/model
                lCnt = 1
                While lCnt < lBufferSize

                    If bInStrMake Then
                        lStart = Strings.InStr(lCnt, sRecord, sMake)
                    Else
                        lStart = Strings.InStr(lCnt, sRecord, sModelChoosen)
                    End If

                    If lStart > 0 And lStart > iPropertyOffset Then
                        ' check we have the correct property id
                        If m_sVehicleListId = sRecord.Substring(lStart - iPropertyOffset - 1, Math.Min(sRecord.Length, 10)).Trim() Then

                            ' Assign the vehicle
                            bMatch = True
                            sResult = sRecord.Substring(lStart - iDescriptionOffset - 1, Math.Min(sRecord.Length, 90)).Trim()

                            'Modified by Archana Tokas on 4/27/2010 10:31:58 AM changes to be checked at run time
                            'Dim handle As GCHandle = GCHandle.Alloc(VarPtr(udtRecordLine), GCHandleType.Pinned)
                            'Dim handle2 As GCHandle = GCHandle.Alloc(StrPtr(sResult), GCHandleType.Pinned)

                            Dim handle As GCHandle = GCHandle.Alloc(udtRecordLine, GCHandleType.Pinned)
                            Dim handle2 As GCHandle = GCHandle.Alloc(sResult, GCHandleType.Pinned)
                            Try

                                Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()

                                Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()

                                CopyMemory(tmpPtr, tmpPtr2, Encoding.Unicode.GetByteCount(sResult))
                            Finally
                                handle.Free()
                                handle2.Free()
                            End Try

                            If bCheckModelID Then
                                If CInt(udtRecordLine.Identifier) <> v_vModelID Then
                                    bMatch = False
                                End If
                            End If

                            ' Filter for model
                            If bCheckModelChoosen Then
                                If (udtRecordLine.ModelName.Trim().IndexOf(sModelChoosen, StringComparison.CurrentCultureIgnoreCase) + 1) = 0 Then
                                    bMatch = False
                                End If
                            End If

                            ' filter for year
                            If bMatch Then
                                If bCheckYear Then
                                    iYearFrom = CInt(udtRecordLine.YearStart)
                                    iYearTo = CInt(udtRecordLine.YearEnd)
                                    With oYearRange
                                        .StartValue = CInt(udtRecordLine.YearStart)
                                        .EndValue = CInt(udtRecordLine.YearEnd)
                                        bMatch = .Includes(iYearChoosen)
                                    End With
                                End If
                            End If

                            'filter for engine cc
                            If bMatch Then
                                If bCheckCC Then
                                    iActCC = Conversion.Val(udtRecordLine.EngineSize.Trim())
                                    If iCC < (iActCC + 50) And iCC > (iActCC - 50) Then
                                    Else
                                        bMatch = False
                                    End If
                                End If
                            End If

                            'DB 25/11/99 Start

                            'Add extra filters for number of doors, fuel & transmission types

                            'Filter for number of doors
                            If bMatch Then
                                If bCheckDoors Then
                                    iDoors = Conversion.Val(udtRecordLine.Doors)
                                    If iDoors = v_vDoors Then
                                    Else
                                        bMatch = False
                                    End If
                                End If
                            End If

                            'Filter for fuel type
                            If bMatch Then
                                If bCheckFuel Then
                                    'sFuelType = udtRecordLine.FuelType
                                    If udtRecordLine.FuelType = v_vFuelType Then
                                    Else
                                        bMatch = False
                                    End If
                                End If
                            End If

                            'Filter for transmission type
                            If bMatch Then
                                If bCheckTrans Then
                                    'sTransType = Mid$(sResult, 21, 1)
                                    If udtRecordLine.Transmission.Trim() = v_vTransType Then
                                    Else
                                        bMatch = False
                                    End If
                                End If
                            End If

                            'DB 25/11/99 End

                            If bMatch Then

                                '--- TODO: IM Convert back to array using copy memory

                                r_vListData(lRow) = sResult
                                lRow += 1

                                If lRow > m_lMaxListItems Then
                                    ReDim Preserve r_vListData(lRow - 1)
                                    Return result
                                End If

                            End If

                            lCnt = lStart + RLDF_RECORD_LENGTH
                        Else
                            lCnt = lStart + 1
                        End If
                    Else
                        lCnt = lBufferSize + 1
                    End If
                End While

                sRecord = String.Empty

                lRecordNumber += RECORDS_PER_READ

            Next lRecordCnt

            If lRow > 0 Then
                ReDim Preserve r_vListData(lRow - 1)
            Else
                r_vListData = VB6.CopyArray(Nothing)
            End If

            oYearRange = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListC failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetVehicleListI4M", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            '    Resume

        End Try
    End Function





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
            Dim aArray() As Object
            ' developer guide no. 131(Guide)
            If cFiles.Count > 0 Then
                aArray = cFiles(m_sClientListFilePathDat)
            End If
            Try

                If Information.IsArray(aArray) Then
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
                    cFiles.Add(aArray, m_sClientListFilePathDat)
                Else
                    'open file
                    ReDim aArray(5)

                    m_lRLDFFileLength = CInt((New FileInfo(m_sClientListFilePathDat)).Length)
                    If m_lRLDFFileLength <> 0 Then


                        aArray(0) = m_lRLDFFileLength

                        m_iRLDFFile = FileSystem.FreeFile()
                        'developer guide no. 132(Guide)
                        FileSystem.FileOpen(m_iRLDFFile, m_sClientListFilePathDat, OpenMode.Random, Access:=OpenAccess.Default, Share:=OpenShare.Shared)
                        aArray(1) = m_iRLDFFile

                        m_iRLDFIndex = FileSystem.FreeFile()
                        'developer guide no. 132(Guide)
                        FileSystem.FileOpen(m_iRLDFIndex, m_sClientListFilePathIdx, OpenMode.Random, Access:=OpenAccess.Default, Share:=OpenShare.Shared)
                        aArray(2) = m_iRLDFIndex

                        m_iRLDFFileB = FileSystem.FreeFile()
                        'developer guide no. 132(Guide)
                        FileSystem.FileOpen(m_iRLDFFileB, m_sClientListFilePathDat, OpenMode.Binary, Access:=OpenAccess.Default, Share:=OpenShare.Shared)
                        aArray(3) = m_iRLDFFileB

                        m_lReturn = CType(BuildIndex(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error building index", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckListVersionsC", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Return gPMConstants.PMEReturnCode.PMError
                        End If
                        aArray(4) = m_cIndex


                        aArray(5) = 1 'file open count

                        'set current values into collection
                        cFiles.Add(aArray, m_sClientListFilePathDat)

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
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
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
            Dim aArray() As Object
            ' developer guide no. 131(Guide)
            If (cFiles.Count > 0) Then
                aArray = cFiles.Item(m_sClientListFilePathDat)
            End If

            If Information.IsArray(aArray) Then
                'if last or forced then close file

                If CDbl(aArray(5)) = 1 Or bNoCountProcessing Then

                    FileSystem.FileClose(CInt(aArray(1))) 'm_iRLDFFile%

                    FileSystem.FileClose(CInt(aArray(2))) 'm_iRLDFIndex%

                    FileSystem.FileClose(CInt(aArray(3))) 'm_iRLDFFileB%
                    cFiles.Remove(m_sClientListFilePathDat)
                    Exit Sub
                End If

                'don't close yet but update count and save


                aArray(5) = CDbl(aArray(5)) - 1
                cFiles.Remove(m_sClientListFilePathDat)
                cFiles.Add(aArray, m_sClientListFilePathDat)

            End If

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub

End Class

