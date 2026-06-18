Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("BusinessOIS_NET.BusinessOIS")> _
Public NotInheritable Class BusinessOIS
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: BusinessOIS
    '
    ' Date: 05/08/1999
    '
    ' Description: Simple no-nonsense wrapper for QAS
    '
    ' Edit History: CL050799 - Created
    '               CB090200 - Changed for OISGEN errors #47 + #120
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
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

    Private m_lQASDatabaseID As Integer


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "BusinessOIS"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Collection of PickListNodes (Private)

    ' QASPro .INI file location
    Private m_sQASPath, m_sPremiseID As String
    Private m_iLineCount As Integer
    Private m_vArray(,) As Object

    ' Successful return from QAS function
    Private Const QASSuccess As Integer = 0

    ' QAS TimeOut Values (in ms)
    Private Const QASMinTimeOutValue As Integer = 1000
    Private Const QASMaxTimeOutValue As Integer = 60000

    ' PRIVATE Data Members (End)

    ' ***************************************************************** '
    ' Name: Initialise (Private)
    '
    ' ***************************************************************** '

    Private Function Initialise() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lReturn As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' open qas database

        'm_sQASPath = "C:\QADDRESS\PROAPI32.311\QAPRO.INI"


        ' Find the QAddress.ini file location from the registry
        lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="QASPro_Path", r_sSettingValue:=m_sQASPath)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Select Case m_lQASDatabaseID
            Case 3 'if QAS Names installed
                lReturn = N_QAPro_Open(m_sQASPath, "")
            Case Else 'Default to QAS Pro
                lReturn = QAPro_Open(m_sQASPath, "")
        End Select

        If lReturn <> QASSuccess Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: Dispose (Standard Method)
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
                Select Case m_lQASDatabaseID
                    Case 3 'if QAS Names installed
                        N_QAPro_Close()
                    Case Else 'Default to QAS Pro
                        QAPro_Close()
                End Select
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    ' Name: Interrogate (Public)
    '
    ' ***************************************************************** '

    Public Function Interrogate(ByVal v_sPremiseID As String, ByVal v_sPostcode As String, ByRef r_lNumMatches As Integer, ByRef r_vArray(,) As Object, Optional ByVal v_lMaxMatches As Integer = -1, Optional ByVal v_lTimeoutSecs As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim lCount As gPMConstants.PMEReturnCode
        Dim sCounty As String = ""
        Dim v_sSearchString As String =""
        Dim sTemp As String = ""
        Dim lTimeoutMillisecs As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_lNumMatches = 0
            ReDim r_vArray(5, 0) 'DB 15/11/99 (3->5)

            m_sPremiseID = v_sPremiseID

            lReturn = Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            v_sPremiseID = v_sPremiseID.Trim()
            v_sPostcode = v_sPostcode.Trim()

            Dim dbNumericTemp As Double
            If Double.TryParse(v_sPremiseID, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                v_sSearchString = (v_sPremiseID & "@P," & v_sPostcode).Trim() ' faster searching
            Else
                'CB 070200 Start - OIS Our Error #47 - Cater for search on postcode only
                'CB - Concatenate premise id and postcode, only if premise id given
                If v_sPremiseID.Trim() <> "" Then
                    v_sSearchString = (v_sPremiseID & "," & v_sPostcode).Trim()
                Else
                    v_sSearchString = v_sPostcode
                End If
            End If

            ' Make sure search strings is not empty
            If v_sPostcode = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                'DAK140700 - prevent memory leaks
                Dispose()
                Return result
            End If
            'CB 070200 End

            If v_lMaxMatches = -1 Then

                lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="QASPro_MaxMatchesAllowed", r_sSettingValue:=sTemp)

                v_lMaxMatches = ToSafeInteger(sTemp)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'DAK140700 - correct function name
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'DAK140700 - prevent memory leaks
                    Dispose()
                    Return result
                End If

            End If


            If v_lTimeoutSecs = -1 Then

                lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="QASPro_TimeoutSeconds", r_sSettingValue:=sTemp)

                v_lTimeoutSecs = ToSafeInteger(sTemp)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'DAK140700 - correct function name
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'DAK140700 - prevent memory leaks
                    Dispose()
                    Return result
                End If

            End If

            lTimeoutMillisecs = v_lTimeoutSecs * 1000 ' convert secs to ms

            Select Case m_lQASDatabaseID
                Case 3 'if QAS Names installed
                    ' Set search timeout
                    N_QAPro_SetTimeout(lTimeoutMillisecs)
                    ' go off to qas db
                    lReturn = N_QAPro_Search(v_sSearchString)
                Case Else 'Default to QAS Pro
                    ' Set search timeout
                    QAPro_SetTimeout(lTimeoutMillisecs)

                    ' go off to qas db
                    lReturn = QAPro_Search(v_sSearchString)
            End Select

            If lReturn = qaerr_TOOMANYMATCHES Then
                result = gPMConstants.PMEReturnCode.PMError_usage
                'DAK140700 - prevent memory leaks
                EndSearch()
                Dispose()
                Return result
            End If

            If lReturn = qaerr_CANCELLED Then
                result = gPMConstants.PMEReturnCode.PMError_timeout
                'DAK140700 - prevent memory leaks
                EndSearch()
                Dispose()
                Return result
            End If

            If lReturn <> QASSuccess Then
                result = gPMConstants.PMEReturnCode.PMFalse
                'DAK140700 - prevent memory leaks
                EndSearch()
                Dispose()
                Return result
            End If

            ' Get num of matches
            Select Case m_lQASDatabaseID
                Case 3 'if QAS Names installed
                    lReturn = N_QAPro_Count()
                Case Else 'Default to QAS Pro
                    lReturn = QAPro_Count()
            End Select

            lCount = lReturn

            ' If error or no matches or exceeds maxmatches then error
            If (lReturn < 1) Or (lReturn > v_lMaxMatches) Then
                result = gPMConstants.PMEReturnCode.PMError_usage
                'DAK140700 - prevent memory leaks
                EndSearch()
                Dispose()
                Return result
            End If


            m_iLineCount = 0 ' Count the number of subpremises returned

            lReturn = Recurse() ' Walk the result set recursively, dammit

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                'DAK140700 - prevent memory leaks
                EndSearch()
                Dispose()
                Return result
            End If

            ' Release resources used for the search
            EndSearch()

            Dispose()

            r_lNumMatches = m_iLineCount

            If r_lNumMatches > v_lMaxMatches Then
                Return gPMConstants.PMEReturnCode.PMError_usage
            End If

            ReDim r_vArray(5, m_iLineCount - 1) 'DB (3->5)

            For lCounter As Integer = 0 To m_iLineCount - 1



                r_vArray(0, lCounter) = m_vArray(0, lCounter) 'Sub Premises   (could be number)


                r_vArray(1, lCounter) = m_vArray(1, lCounter) 'Premise Number (could be name)


                r_vArray(2, lCounter) = m_vArray(2, lCounter) 'Thoroughfare


                r_vArray(3, lCounter) = m_vArray(3, lCounter) 'Post Town

                'DB 15/11/99 Start
                'Need to allow for the two extra fields coming back from Recurse



                r_vArray(4, lCounter) = m_vArray(4, lCounter) 'County


                r_vArray(5, lCounter) = m_vArray(5, lCounter) 'Postcode

                'DB 15/11/99 End

            Next lCounter

            'CB 070200 Start - OIS Our Error #47
            'Since possibly returning all addresses for one postcode, need to sort the results.
            'Pass 1 to sort meaning we'll sort on premise no./name in Ascending order.
            lReturn = gPMFunctions.ShellSort2DArray(r_vArray, 1, "ASCENDING")

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'CB 070200 End
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Interrogate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Interrogate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Dispose()

            Return result




            Return result
        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetAddressLines (Private)
    '
    ' ***************************************************************** '

    Private Function GetAddressLines(ByVal v_lItem As Integer, ByRef r_sSubPremises As String, ByRef r_sBuildingNumber As String, ByRef r_sThoroughfare As String, ByRef r_sPostTown As String, ByRef r_sCounty As String, ByRef r_sPostCode As String) As Integer 'DB 15/11/99 r_sPostcode added to parameters

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim sOrganisation As String = ""
        'DAK140700
        Dim sDepThoroughfare, sThoroughfare, sLocality As String




        result = gPMConstants.PMEReturnCode.PMTrue

        r_sSubPremises = New String(" "c, 100)
        r_sBuildingNumber = New String(" "c, 100)
        'DAK140700
        sDepThoroughfare = New String(" "c, 100)
        sThoroughfare = New String(" "c, 100)
        sLocality = New String(" "c, 100)
        r_sPostTown = New String(" "c, 100)
        r_sCounty = New String(" "c, 100)
        r_sPostCode = New String(" "c, 100) 'DB 15/11/99
        sOrganisation = New String(" "c, 100) 'CB 10/2/00

        lReturn = QA_AddrLine(v_lItem, qafields_SUBPREMISES, "", r_sSubPremises, 100)

        If lReturn <> QASSuccess Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = QA_AddrLine(v_lItem, qafields_BUILDINGNUMBER, "", r_sBuildingNumber, 100)

        If lReturn <> QASSuccess Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DAK140700
        lReturn = QA_AddrLine(v_lItem, qafields_DEPTHORO, "", sDepThoroughfare, 100)

        If lReturn <> QASSuccess Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = QA_AddrLine(v_lItem, qafields_THORO, "", sThoroughfare, 100)

        If lReturn <> QASSuccess Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = QA_AddrLine(v_lItem, qafields_LOCAL, "", sLocality, 100)

        If lReturn <> QASSuccess Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = QA_AddrLine(v_lItem, qafields_POSTTOWN, "", r_sPostTown, 100)

        If lReturn <> QASSuccess Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = QA_AddrLine(v_lItem, qafields_COUNTY, "", r_sCounty, 100)

        If lReturn <> QASSuccess Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DB 15/11/99 Start
        'Pick up the postcode - it could be recoded !

        lReturn = QA_AddrLine(v_lItem, qafields_POSTCODE, "", r_sPostCode, 100)

        If lReturn <> QASSuccess Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DB 15/11/99 End

        ' Tidy up strings
        r_sSubPremises = r_sSubPremises.Trim()
        r_sBuildingNumber = r_sBuildingNumber.Trim()
        'DAK140700
        sDepThoroughfare = sDepThoroughfare.Trim()
        sThoroughfare = sThoroughfare.Trim()
        sLocality = sLocality.Trim()
        r_sPostTown = r_sPostTown.Trim()
        r_sCounty = r_sCounty.Trim()
        r_sPostCode = r_sPostCode.Trim() 'DB 15/11/99

        ' Remove terminating null
        r_sSubPremises = r_sSubPremises.Substring(0, r_sSubPremises.Length - 1)
        r_sBuildingNumber = r_sBuildingNumber.Substring(0, r_sBuildingNumber.Length - 1)
        'DAK140700
        sDepThoroughfare = sDepThoroughfare.Substring(0, sDepThoroughfare.Length - 1)
        sThoroughfare = sThoroughfare.Substring(0, sThoroughfare.Length - 1)
        sLocality = sLocality.Substring(0, sLocality.Length - 1)
        r_sPostTown = r_sPostTown.Substring(0, r_sPostTown.Length - 1)
        r_sCounty = r_sCounty.Substring(0, r_sCounty.Length - 1)
        r_sPostCode = r_sPostCode.Substring(0, r_sPostCode.Length - 1) 'DB 15/11/99

        'DB 15/11/99 Start
        'Remove the blank between the first letter and its number,
        'e.g. CV1 1AA is coming back as CV 1 1AA
        'The code below corrects it to CV1 1AA

        Dim lFirstBlankPos, lMiddleBlankPos As Integer


        'DB 16/11/99 Needed to convert this to loop because some postcodes
        'have two blanks between the area code and the number, e.g. M  1 1AA
        Do
            lFirstBlankPos = (r_sPostCode.IndexOf(" "c) + 1)
            '	lMiddleBlankPos = inStr(r_sPostCode.Length - 3, r_sPostCode, " ", CompareMethod.Text)
            lMiddleBlankPos = r_sPostCode.IndexOf(" ", r_sPostCode.Length - 4, gPMConstants.CompareMethod.Text) + 1

            If lFirstBlankPos <> lMiddleBlankPos Then
                r_sPostCode = r_sPostCode.Substring(0, Math.Min(r_sPostCode.Length, lFirstBlankPos - 1)) & _
                              r_sPostCode.Substring(lFirstBlankPos)
            End If

        Loop Until lFirstBlankPos = lMiddleBlankPos
        'DB 15/11/99 End


        If r_sBuildingNumber = "" Then
            ' this premises has no number... get the building name instead
            r_sBuildingNumber = New String(" "c, 100)
            lReturn = QA_AddrLine(v_lItem, qafields_BUILDINGNAME, "", r_sBuildingNumber, 100)

            If lReturn <> QASSuccess Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_sBuildingNumber = r_sBuildingNumber.Trim()
            r_sBuildingNumber = r_sBuildingNumber.Substring(0, r_sBuildingNumber.Length - 1)

        End If

        'CB 070200 Start - OIS Our Error #120
        'Get organisation name value and store in subpremise field at the beginning (i.e.
        'as well as any subpremise) but need to delimit with * at start and end in order
        'to pick out in interface and recognise if a company name does exist.
        lReturn = QA_AddrLine(v_lItem, qafields_ORGANISATION, "", sOrganisation, 100)

        If lReturn <> QASSuccess Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Tidy up return string
        sOrganisation = sOrganisation.Trim()

        'Remove terminating null
        sOrganisation = sOrganisation.Substring(0, sOrganisation.Length - 1)

        'If Organisation name found
        If sOrganisation <> "" Then

            'Place delimiters (*) at start and end
            sOrganisation = "*" & sOrganisation & "*"

            'Store at start of Sub premise array element
            r_sSubPremises = sOrganisation & r_sSubPremises

        End If
        'CB 070200 End

        'DAK140700 - Format r_sThoroughfare
        r_sThoroughfare = ""
        If sDepThoroughfare <> "" Then
            r_sThoroughfare = sDepThoroughfare
        End If

        If sThoroughfare <> "" Then
            If r_sThoroughfare <> "" Then
                r_sThoroughfare = r_sThoroughfare & ", "
            End If
            r_sThoroughfare = r_sThoroughfare & sThoroughfare
        End If

        If sLocality <> "" Then
            If r_sThoroughfare <> "" Then
                r_sThoroughfare = r_sThoroughfare & ", "
            End If
            r_sThoroughfare = r_sThoroughfare & sLocality
        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: Recurse (Private)
    '
    ' Walk the QAS result set recursively
    '
    ' ***************************************************************** '
    Private Function Recurse() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim sThoroughfare As String = ""
        Dim sPremNumber As String = ""
        Dim sPostTown As String = ""
        Dim sSubPremises As String = ""
        Dim sPostCode As String = "" 'DB 15/11/99
        Dim sCounty As String = ""
        Dim lNumber, lCount As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        Select Case m_lQASDatabaseID
            Case 3 'if QAS Names installed
                lCount = N_QAPro_Count()
            Case Else 'Default to QAS Pro
                lCount = QAPro_Count()
        End Select

        If lCount < 1 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        For lCounter As Integer = 0 To lCount - 1

            ' How accurate a match have we got?
            lReturn = QA_GetItemInfo(lCounter, qapro_QUALITYINFO, lNumber)

            'CB 070200 Start - OIS Error #47
            'Removed "Or lNumber <> 100" from next line as now we may match just on postcode now
            'also - this statement rejected existing valid postcodes with numbers anyway e.g. 114, ST17 4AH
            If lReturn <> QASSuccess Then
                'CB 070200 End
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get more info
            lReturn = QA_GetItemInfo(lCounter, qapro_TYPEINFO, lNumber)

            'CB 070200 Start - OIS Error #47
            'Removed following lines as may now search just on postcode and when no house
            'no given, lNumber is zero
            'If (lReturn <> QASSuccess) Or lNumber = 0 Then
            'lNumber = 0 mean no premises located... e.g. non-existant house number/name
            'Recurse = PMFalse
            'Exit Function
            'End If
            'CB 070200 End

            ' so we have a single premises
            lReturn = QA_GetItemInfo(lCounter, qapro_STEPINFO, lNumber)

            If lReturn <> QASSuccess Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lNumber <> 0 Then

                ' we can step into this premises
                Select Case m_lQASDatabaseID
                    Case 3 'if QAS Names installed
                        lReturn = N_QAPro_StepIn(lCounter)
                    Case Else 'Default to QAS Pro
                        lReturn = QAPro_StepIn(lCounter)
                End Select

                If lReturn < 0 Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                lReturn = Recurse()

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Select Case m_lQASDatabaseID
                    Case 3 'if QAS Names installed
                        lReturn = N_QAPro_StepOut()
                    Case Else 'Default to QAS Pro
                        lReturn = QAPro_StepOut()
                End Select

                If lReturn < 0 Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                'Nothing to step into for this premises, just return the single address



                lReturn = GetAddressLines(v_lItem:=lCounter, r_sSubPremises:=sSubPremises, r_sBuildingNumber:=sPremNumber, r_sThoroughfare:=sThoroughfare, r_sPostTown:=sPostTown, r_sCounty:=sCounty, r_sPostCode:=sPostCode) 'DB 15/11/99    Postcode added to parameters

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '
                ' We need to make sure the address we have here is still
                ' the same building that was original asked for.
                ' (sPremNumber maybe a housename, hence the Ucase$)
                '

                'DB 15/11/99 Start

                'Check the given premise id against the sub-premises
                'or the premise number (which could be a name)

                'DB 16/11/99

                'Need to loosen the check to an instr
                '(i.e. given name or number is contained within the field)

                'CB 070200 Start - OIS Error #47
                'Removed following lines as may now search on no premise no. / name
                'If InStr(1, UCase$(sPremNumber), UCase$(m_sPremiseID), vbTextCompare) <> 0 _
                ''Or InStr(1, UCase$(sSubPremises), UCase$(m_sPremiseID), vbTextCompare) Then

                ' Add to return array

                ReDim Preserve m_vArray(5, m_iLineCount) 'DB 15/11/99 Increased to 5

                ' We need the sub premise & the premise number
                ' in case it has both. ('DB)

                m_vArray(0, m_iLineCount) = sSubPremises

                m_vArray(1, m_iLineCount) = sPremNumber

                m_vArray(2, m_iLineCount) = sThoroughfare ' 1 -> 2

                m_vArray(3, m_iLineCount) = sPostTown ' 2 -> 3

                m_vArray(4, m_iLineCount) = sCounty ' 3 -> 4

                ' Also add the postcode, because they get recoded

                m_vArray(5, m_iLineCount) = sPostCode
                m_iLineCount += 1

                'End If
                'CB 070200 End - OIS Error #47

                'DB 15/11/99 End

            End If

        Next lCounter

        Return result

    End Function

    Private Sub EndSearch()

        Select Case m_lQASDatabaseID
            Case 3 'if QAS Names installed
                N_QAPro_EndSearch()
            Case Else 'Default to QAS Pro
                QAPro_EndSearch()
        End Select

    End Sub

    Private Function QA_AddrLine(ByRef vl1 As Integer, ByRef vi2 As Integer, ByRef vs3 As String, ByRef rs4 As String, ByRef vi5 As Integer) As Integer

        Select Case m_lQASDatabaseID
            Case 3 'if QAS Names installed
                Return N_QAPro_AddrLine(vl1, vi2, vs3, rs4, vi5)
            Case Else 'Default to QAS Pro
                Return QAPro_AddrLine(vl1, vi2, vs3, rs4, vi5)
        End Select

    End Function

    Private Function QA_GetItemInfo(ByRef vl1 As Integer, ByRef vi2 As Integer, ByRef rl3 As Integer) As Integer

        Select Case m_lQASDatabaseID
            Case 3 'if QAS Names installed
                Return N_QAPro_GetItemInfo(vl1, vi2, rl3)
            Case Else 'Default to QAS Pro
                Return QAPro_GetItemInfo(vl1, vi2, rl3)
        End Select

    End Function
End Class

