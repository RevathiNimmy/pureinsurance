Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctRIModelControl_NET.uctRIModelControl")>
Partial Public Class uctRIModelControl
    Inherits System.Windows.Forms.UserControl

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "uctRIModelControl"

    ' ***************************************************************** '
    '                        PRIVATE PROPERTIES
    ' ***************************************************************** '
    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Internal business object references
    Private m_oRIModel As bSIRRIModel.Business

    Private m_oTreaty As bSIRTreaty.Business

    ' Root RI Model
    Private m_oRootModel As RIModelCache

    ' Collections for caching
    Private m_cRIModels As Collection
    Private m_cTreatyParties As Collection
    Private m_lRIArrangementID As Integer
    Private m_iFilterType As Integer

    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '
    <Browsable(False)>
    Public ReadOnly Property RIModelID() As Integer
        Get
            If m_oRootModel Is Nothing Then
                Return -1
            Else
                Return m_oRootModel.RIModelID
            End If
        End Get
    End Property
    Public Property RIArrangementID() As Integer
        Get
            Return m_lRIArrangementID
        End Get
        Set(value As Integer)
            m_lRIArrangementID = value
        End Set
    End Property
    Public Property FilterType() As Integer
        Get
            Return m_iFilterType
        End Get
        Set(value As Integer)
            m_iFilterType = value
        End Set
    End Property

    ' ***************************************************************** '
    '                         PUBLIC METHODS
    ' ***************************************************************** '
    Public Function Clear(Optional ByVal bClearCaches As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "Clear"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset root node
            m_oRootModel = Nothing

            ' Clear Treeview
            trvRIModel.Nodes.Clear()

            ' Clear all caches?
            If bClearCaches Then
                m_cRIModels = New Collection()
                m_cTreatyParties = New Collection()
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Initialise"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.Initialise", "Unable to initialise instance of ObjectManager")
            End If

            ' Get ri model business object
            Dim temp_m_oRIModel As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oRIModel, "bSIRRIModel.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oRIModel = temp_m_oRIModel
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of ri model business object")
            End If

            ' Get treaty business object
            Dim temp_m_oTreaty As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oTreaty, "bSIRTreaty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oTreaty = temp_m_oTreaty
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of treaty business object")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Public Function SetProperties(ByVal lRIModelID As Integer,
    Optional ByVal v_lXOLRIModelId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "SetProperties"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if we have an existing RI Model
            If lRIModelID > 0 Then
                ' Add whole model
                lReturn = CType(AddRIModel(lRIModelID, , v_lXOLRIModelId:=v_lXOLRIModelId), gPMConstants.PMEReturnCode)

                For Each trNode As TreeNode In trvRIModel.Nodes
                    If trNode.Level <> 0 Then
                        Continue For
                    End If
                    trNode.Expand()
                Next

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("AddRIModel(lRIModelID)", "Unable to add root RI Model")
                End If
            Else
                lReturn = CType(NewRIModel(), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("NewRIModel()", "Unable to add root RI Model")
                End If
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    Public Function UpdateRIModel(ByVal sDescription As String, ByVal dtEffectiveDate As Date, ByVal vExpiryDate As Object, ByVal iClaimsAllocation As Integer, ByVal sCurrencyDescription As String, ByVal lClaimXOLID As Integer, ByVal cClaimXOLLimit As Decimal, ByVal lCatXOLID As Integer, ByVal cCatXOLLimit As Decimal, ByVal iCatXOLReinstatements As Integer, ByVal vRILines(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "UpdateRIModel"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Essentially we take the root model, update it's cache object and refresh the entire control
            m_oRootModel.Description = sDescription
            m_oRootModel.EffectiveDate = dtEffectiveDate
            m_oRootModel.ExpiryDate = vExpiryDate
            m_oRootModel.ClaimsAllocation = iClaimsAllocation
            m_oRootModel.CurrencyDescription = sCurrencyDescription
            m_oRootModel.ClaimXOLID = lClaimXOLID
            m_oRootModel.ClaimXOLLimit = cClaimXOLLimit
            m_oRootModel.CatXOLID = lCatXOLID
            m_oRootModel.CatXOLLimit = cCatXOLLimit
            m_oRootModel.CatXOLReinstatements = iCatXOLReinstatements
            m_oRootModel.Lines = vRILines

            ' Refresh tree
            trvRIModel.Nodes.Clear()
            lReturn = CType(AddRIModel(m_oRootModel.RIModelID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("AddRIModel(m_oRootModel.RIModelID)", "Unable to refresh RI Model list")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '                         PRIVATE METHODS
    ' ***************************************************************** '
    Private Function AddPriorities(ByVal oRIModel As RIModelCache, ByVal oParentNode As TreeNode) As Integer

        Dim result As Integer = 0
        Dim oPNode, oTNode As TreeNode
        Dim vLines(,) As Object
        Dim lPriority As Integer
        Dim sCaption As String = ""

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "AddPriorities"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check for lines
            If Not Information.IsArray(oRIModel.Lines) Then
                Return result
            End If

            ' Copy array and sort on percent and priority
            vLines = VB6.CopyArray(oRIModel.Lines)

            lReturn = CType(gPMFunctions.ShellSort2DArray(vLines, MainModule.RIModelLineEnum.DBMLSharePercent, "DESCENDING"), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ShellSort2DArray", "Unable to sort ri model line array")
            End If

            lReturn = CType(gPMFunctions.ShellSort2DArray(vLines, MainModule.RIModelLineEnum.DBMLPriority), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ShellSort2DArray", "Unable to sort ri model line array")
            End If

            ' Process all lines

            For lCount As Integer = vLines.GetLowerBound(1) To vLines.GetUpperBound(1)
                ' Add by priority, so check for new one

                If lPriority <> CDbl(vLines(MainModule.RIModelLineEnum.DBMLPriority, lCount)) Then

                    lPriority = CInt(vLines(MainModule.RIModelLineEnum.DBMLPriority, lCount))

                    ' Build caption and add node
                    ' Surplus lines decimals - use reinsurance_type_id value directly (6=First Surplus, 7=Second Surplus, 8=Third Surplus)
                    ' DBMLRITypeID is at index 11 in the SP result, not defined in this project's RIModelLineEnum
                    Dim iRIType As Integer = gPMFunctions.ToSafeInteger(vLines(11, lCount))
                    Dim sLines As String
                    If iRIType = 6 OrElse iRIType = 7 OrElse iRIType = 8 Then
                        sLines = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDecimal, CDec(vLines(MainModule.RIModelLineEnum.DBMLNumberOfLines, lCount)), 2)
                    Else
                        sLines = ToSafeString(vLines(MainModule.RIModelLineEnum.DBMLNumberOfLines, lCount))
                    End If
                    sCaption = "Priority " & lPriority & " - " &
                               sLines & " line(s) of " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, ToSafeDouble(ToSafeDouble(vLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount)) - gPMFunctions.ToSafeCurrency(ToSafeDouble(vLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount)), 0)))
                    oPNode = trvRIModel.Nodes.Find(oParentNode.Name, True)(0).Nodes.Add(sCaption, sCaption, "Priority")
                End If
                ' Add the treaty node

                If CDbl(vLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)) > 0 Then ' i.e. not deleted


                    sCaption = "Treaty (" & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, CDbl(vLines(MainModule.RIModelLineEnum.DBMLSharePercent, lCount))) & ") - " & CStr(vLines(MainModule.RIModelLineEnum.DBMLDescription, lCount))

                    If Not (oPNode Is Nothing) Then
                        oTNode = trvRIModel.Nodes.Find(oPNode.Name, True)(0).Nodes.Add(sCaption, sCaption, "Treaty")
                    End If

                    ' Add treaty party details

                    If CDbl(vLines(MainModule.RIModelLineEnum.DBMLTreatyTypeID, lCount)) <> 2 Then

                        If Not (oTNode Is Nothing) Then

                            lReturn = CType(AddTreatyParties(CInt(vLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)), oTNode), gPMConstants.PMEReturnCode)
                        End If
                    Else





                        lReturn = CType(AddTreatyParties(lTreatyID:=CInt(vLines(MainModule.RIModelLineEnum.DBMLTreatyID, lCount)), oParentNode:=oTNode, lTreatyTypeId:=CInt(vLines(MainModule.RIModelLineEnum.DBMLTreatyTypeID, lCount)), dCedingRate:=CDbl(vLines(MainModule.RIModelLineEnum.DBMLCedingrate, lCount)), cLowerLimit:=CDec(vLines(MainModule.RIModelLineEnum.DBMLLowerLimit, lCount)), cUpperLimit:=CDec(vLines(MainModule.RIModelLineEnum.DBMLLineLimit, lCount))), gPMConstants.PMEReturnCode)
                    End If
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("AddTreatyParties(vLines(DBMLTreatyID, lCount), oTNode)", "Unable to add treaty breakdown")
                    End If
                End If
            Next lCount


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Private Function AddRIModel(ByVal lRIModelID As Integer, Optional ByRef oParentNode As TreeNode = Nothing, Optional ByVal sCode As String = "", Optional ByVal sPrefix As String = "", Optional ByVal cLimit As Decimal = 0, Optional ByVal lReinstatements As Integer = 0, Optional ByVal lLayer As Integer = 1,
    Optional ByVal v_lXOLRIModelId As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim vResults As Object
        Dim oNode, oNotes As TreeNode
        Dim sCaption As String = ""

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "AddRIModel"

        ' E007
        Dim oRIModelXOLRIModel As RIModelCache
        Dim bIsXOLRIModel As Boolean
        Dim oRIExtendedLimit(,) As Object
        Dim cExtendedLimitAmount As Decimal
        Dim bIsExtendedLimitApplied As Boolean
        ' Check if this model exists
        Dim oRIModel As RIModelCache = Nothing
        Try
            oRIModel = m_cRIModels.Item("M" & lRIModelID)
            oRIModelXOLRIModel = m_cRIModels("M" & v_lXOLRIModelId)
        Catch
        End Try

        result = gPMConstants.PMEReturnCode.PMTrue

        If v_lXOLRIModelId > 0 AndAlso lRIModelID > 0 AndAlso lRIModelID <> v_lXOLRIModelId Then
            bIsXOLRIModel = True
        End If
        ' If we don't have this ri model in the cache load it
        If oRIModel Is Nothing Then
            oRIModel = New RIModelCache()

            ' Get RI Model header details

            lReturn = m_oRIModel.GetRIModels(r_vRIModel:=vResults, v_lRIModelID:=lRIModelID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oRIModel.GetRIModels", "Unable to retrieve reinsurance model lines")
            End If

            ' Store data

            oRIModel.RIModelID = (vResults(MainModule.RIModelEnum.DBMRIModelID, 0))

            oRIModel.Description = (vResults(MainModule.RIModelEnum.DBMDescription, 0))

            oRIModel.EffectiveDate = gPMFunctions.ToSafeDate(vResults(MainModule.RIModelEnum.DBMEffectiveDate, 0)).ToString("dd MMM yyyy")

            oRIModel.ExpiryDate = gPMFunctions.ToSafeDate(vResults(MainModule.RIModelEnum.DBMExpiryDate, 0)).ToString("dd MMM yyyy")

            oRIModel.ClaimsAllocation = gPMFunctions.ToSafeInteger((vResults(MainModule.RIModelEnum.DBMClaimAllocationType, 0)))

            oRIModel.CurrencyDescription = (vResults(MainModule.RIModelEnum.DBMCurrencyDescription, 0))

            oRIModel.ClaimXOLID = gPMFunctions.ToSafeLong((vResults(MainModule.RIModelEnum.DBMXOLClmRIModelID, 0)))

            oRIModel.ClaimXOLLimit = gPMFunctions.ToSafeCurrency((vResults(MainModule.RIModelEnum.DBMXOLClmLimit, 0)))

            oRIModel.CatXOLID = gPMFunctions.ToSafeLong((vResults(MainModule.RIModelEnum.DBMXOLCatRIModelID, 0)))

            oRIModel.CatXOLLimit = gPMFunctions.ToSafeCurrency((vResults(MainModule.RIModelEnum.DBMXOLCatLimit, 0)))

            oRIModel.CatXOLReinstatements = gPMFunctions.ToSafeLong((vResults(MainModule.RIModelEnum.DBMXOLCatReinstatements, 0)))

            ' Get RI Model lines
            If bIsXOLRIModel = True Then
                lReturn = m_oRIModel.GetRIModelLines(
                    v_lRIModelID:=lRIModelID,
                    r_vRIModelLines:=vResults,
                    v_iFilterType:=m_iFilterType,
                    v_sTreatyTypeCode:="PROP",
                    v_lRIArrangementID:=m_lRIArrangementID)
            Else
                lReturn = m_oRIModel.GetRIModelLines(v_lRIModelID:=lRIModelID, v_iFilterType:=m_iFilterType,
                v_lRIArrangementID:=m_lRIArrangementID, r_vRIModelLines:=vResults)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oRIModel.GetRIModelLines", "Unable to retrieve reinsurance model lines")
                End If
            End If

            ' Store lines
            lReturn = m_oRIModel.GetRIExtendedLimitAmount(
            v_nRIArrangementID:=m_lRIArrangementID,
            v_nFilterType:=m_iFilterType,
            r_oRIExtendedLimit:=oRIExtendedLimit)
            If lReturn <> PMEReturnCode.PMTrue Then
                RaiseError("m_oRIModel.GetRIExtendedLimitAmount", "Unable to retrieve RI Extended Limit Amount")
            End If

            If oRIExtendedLimit IsNot Nothing AndAlso IsArray(oRIExtendedLimit) Then
                cExtendedLimitAmount = ToSafeCurrency(oRIExtendedLimit(0, 0))
                bIsExtendedLimitApplied = oRIExtendedLimit(1, 0)
                If bIsExtendedLimitApplied AndAlso vResults IsNot Nothing AndAlso IsArray(vResults) AndAlso CDec(cExtendedLimitAmount) > 0D Then
                    vResults(4, 0) = cExtendedLimitAmount
                    vResults(4, 1) = cExtendedLimitAmount
                End If
            End If

            oRIModel.Lines = vResults

            ' Add to cache collection
            m_cRIModels.Add(oRIModel, "M" & oRIModel.RIModelID)
        End If
        ' E007 Changes
        If bIsXOLRIModel = True Then
            If oRIModelXOLRIModel Is Nothing Then
                oRIModelXOLRIModel = New RIModelCache
                ' Get RI Model header details
                lReturn = m_oRIModel.GetRIModels(
                    r_vRIModel:=vResults,
                    v_lRIModelID:=v_lXOLRIModelId)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("m_oRIModel.GetRIModels", "Unable to retrieve reinsurance model lines")
                End If

                ' Store data
                oRIModelXOLRIModel.RIModelID = vResults(MainModule.RIModelEnum.DBMRIModelID, 0)
                oRIModelXOLRIModel.Description = vResults(MainModule.RIModelEnum.DBMDescription, 0)
                oRIModelXOLRIModel.EffectiveDate = ToSafeDate(vResults(MainModule.RIModelEnum.DBMEffectiveDate, 0))
                oRIModelXOLRIModel.ExpiryDate = vResults(MainModule.RIModelEnum.DBMExpiryDate, 0)
                oRIModelXOLRIModel.ClaimsAllocation = ToSafeInteger(vResults(MainModule.RIModelEnum.DBMClaimAllocationType, 0))
                oRIModelXOLRIModel.CurrencyDescription = vResults(MainModule.RIModelEnum.DBMCurrencyDescription, 0)
                oRIModelXOLRIModel.ClaimXOLID = ToSafeLong(vResults(MainModule.RIModelEnum.DBMXOLClmRIModelID, 0))
                oRIModelXOLRIModel.ClaimXOLLimit = ToSafeCurrency(vResults(MainModule.RIModelEnum.DBMXOLClmLimit, 0))
                oRIModelXOLRIModel.CatXOLID = ToSafeLong(vResults(MainModule.RIModelEnum.DBMXOLCatRIModelID, 0))
                oRIModelXOLRIModel.CatXOLLimit = ToSafeCurrency(vResults(MainModule.RIModelEnum.DBMXOLCatLimit, 0))
                oRIModelXOLRIModel.CatXOLReinstatements = ToSafeLong(vResults(MainModule.RIModelEnum.DBMXOLCatReinstatements, 0))

                ' Get RI Model lines
                If bIsXOLRIModel = True Then
                    lReturn = m_oRIModel.GetRIModelLines(
                        v_lRIModelID:=v_lXOLRIModelId,
                        r_vRIModelLines:=vResults,
                        v_iFilterType:=m_iFilterType,
                        v_lRIArrangementID:=m_lRIArrangementID,
                        v_sTreatyTypeCode:="XOL")
                Else
                    lReturn = m_oRIModel.GetRIModelLines(
                        v_lRIModelID:=v_lXOLRIModelId,
                        v_iFilterType:=m_iFilterType,
                        v_lRIArrangementID:=m_lRIArrangementID,
                        r_vRIModelLines:=vResults)
                End If
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("m_oRIModel.GetRIModelLines", "Unable to retrieve reinsurance model lines")
                End If

                ' Store lines
                oRIModelXOLRIModel.Lines = vResults

                ' Add to cache collection
                m_cRIModels.Add(oRIModelXOLRIModel, "M" & oRIModelXOLRIModel.RIModelID)

            End If
        End If

        ' Check where to add in treeview
        If oParentNode Is Nothing Then
            ' This is the main RI Model add as root node
            ' This is the main RI Model add as root node
            If bIsXOLRIModel = True Then
                m_oRootModel = oRIModelXOLRIModel
                m_oRootModel = oRIModel
                oNode = trvRIModel.Nodes.Add("M" & oRIModelXOLRIModel.RIModelID, sPrefix & oRIModelXOLRIModel.Description, "ClosedFolder")

                ' Add properties node
                trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add("RI Model Details", "RI Model Details", "Notes")

                ' Add properties
                trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add("Effective Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, oRIModelXOLRIModel.EffectiveDate.ToString()), "Effective Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, oRIModelXOLRIModel.EffectiveDate.ToString()), "Note")
                If IsDate(oRIModelXOLRIModel.ExpiryDate) Then
                    trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add(("Expiry Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMFunctions.ToSafeDate(oRIModelXOLRIModel.ExpiryDate).ToString())), "Expiry Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMFunctions.ToSafeDate(oRIModelXOLRIModel.ExpiryDate).ToString()), "Note")
                End If
                trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add("Claims Allocation: " & CStr(Interaction.Choose(oRIModelXOLRIModel.ClaimsAllocation + 1, "Proportional", "By Priority", "Non-Proportional")), "Claims Allocation: " & CStr(Interaction.Choose(oRIModelXOLRIModel.ClaimsAllocation + 1, "Proportional", "By Priority", "Non-Proportional")), "Note")
                trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add("RI Model Currency: " & oRIModelXOLRIModel.CurrencyDescription, "RI Model Currency: " & oRIModelXOLRIModel.CurrencyDescription, "Note")

                ' Add priority nodes
                lReturn = AddPriorities(oRIModelXOLRIModel, oNode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("AddPriorities(oRIModelXOLRIModel, oNode)", "Unable to add priority nodes")
                End If

                oNode = trvRIModel.Nodes.Add("M" & oRIModel.RIModelID, sPrefix & oRIModel.Description, "ClosedFolder")
                ' Add properties node
                trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add("RI Model Details", "RI Model Details", "Notes")

                ' Add properties
                trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add("Effective Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, oRIModel.EffectiveDate.ToString()), "Effective Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, oRIModel.EffectiveDate.ToString()), "Note")
                If IsDate(oRIModel.ExpiryDate) Then
                    trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add(("Expiry Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMFunctions.ToSafeDate(oRIModel.ExpiryDate).ToString())), "Expiry Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMFunctions.ToSafeDate(oRIModel.ExpiryDate).ToString()), "Note")
                End If
                trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add("Claims Allocation: " & CStr(Interaction.Choose(oRIModel.ClaimsAllocation + 1, "Proportional", "By Priority", "Non-Proportional")), "Claims Allocation: " & CStr(Interaction.Choose(oRIModel.ClaimsAllocation + 1, "Proportional", "By Priority", "Non-Proportional")), "Note")
                trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add("RI Model Currency: " & oRIModel.CurrencyDescription, "RI Model Currency: " & oRIModel.CurrencyDescription, "Note")
                If bIsExtendedLimitApplied = True Then
                    trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add("Extended Limit Amount: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cExtendedLimitAmount), "Extended Limit Amount: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cExtendedLimitAmount), "Note")
                End If
                ' Add priority nodes
                lReturn = AddPriorities(oRIModel, oNode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("AddPriorities(oRIModel, oNode)", "Unable to add priority nodes")
                End If
                ' Set remaining properties

                oNode.SelectedImageKey = "OpenFolder"
            Else
                m_oRootModel = oRIModel
                oNode = trvRIModel.Nodes.Add("M" & oRIModel.RIModelID, sPrefix & oRIModel.Description, "ClosedFolder")

            End If
        Else
            ' parent so this is XOL, we need to check for duplicates
            Try
                If (trvRIModel.Nodes(0).Nodes.ContainsKey(sCode & CStr(oRIModel.RIModelID)) = False) Then
                    oNode = trvRIModel.Nodes(0).Nodes.Add(sCode & CStr(oRIModel.RIModelID), sPrefix & oRIModel.Description, "ClosedFolder")
                Else
                    oNode = Nothing
                End If
            Catch
            End Try

            ' Check node
            If oNode Is Nothing Then
                ' If we couldn't add this node it is due to a cyclical link
                oNode = trvRIModel.Nodes.Find(oParentNode.Name, True)(0).Nodes.Add(sCode & "CYCLIC", sPrefix & oRIModel.Description & " [Cyclical Link]", "ClosedFolder")
                GoTo Finally_Renamed
            End If

            ' Add XOL notes (direct on XOL node)
            If sCode = "CLM" Then
                trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add("XOL Limit: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cLimit), "XOL Limit: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cLimit), "Note")
            Else
                trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add("XOL Total Limit: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cLimit), "XOL Total Limit: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cLimit), "Note")
                trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add("Auto Reinstatement: " & (IIf(lReinstatements > 0, "Yes (" & lReinstatements & ")", "No")), "Auto Reinstatement: " & (IIf(lReinstatements > 0, "Yes (" & lReinstatements & ")", "No")), "Note")
            End If
        End If

        If bIsXOLRIModel = False Then
            ' Add properties node
            oNotes = trvRIModel.Nodes.Find(oNode.Name, True)(0).Nodes.Add("RI Model Details", "RI Model Details", "Notes")

            ' Add properties
            oNotes.Nodes.Add(("Effective Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, oRIModel.EffectiveDate.ToString())), "Effective Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, oRIModel.EffectiveDate.ToString()), "Note")

            If Information.IsDate(oRIModel.ExpiryDate) Then
                oNotes.Nodes.Add(("Expiry Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMFunctions.ToSafeDate(oRIModel.ExpiryDate).ToString())), "Expiry Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMFunctions.ToSafeDate(oRIModel.ExpiryDate).ToString()), "Note")
            End If
            oNotes.Nodes.Add("Claims Allocation: " & CStr(Interaction.Choose(oRIModel.ClaimsAllocation + 1, "Proportional", "By Priority", "Non-Proportional")), "Claims Allocation: " & CStr(Interaction.Choose(oRIModel.ClaimsAllocation + 1, "Proportional", "By Priority", "Non-Proportional")), "Note")
            oNotes.Nodes.Add("RI Model Currency: " & oRIModel.CurrencyDescription, "RI Model Currency: " & oRIModel.CurrencyDescription, "Note")
            If bIsExtendedLimitApplied = True Then
                oNotes.Nodes.Add("Extended Limit Amount: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cExtendedLimitAmount), "Extended Limit Amount: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cExtendedLimitAmount), "Note")
            End If

            ' Add priority nodes
            lReturn = CType(AddPriorities(oRIModel, oNode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("AddPriorities(oRIModel, oNode)", "Unable to add priority nodes")
            End If
        End If

        ' Check for Per Claim XOL
        If oRIModel.ClaimXOLID > 0 Then
            ' Add appropriate XOL node
            If Not String.IsNullOrEmpty(sCode) Then
                sCode = sCode.Substring(0, 3)
            End If
            Select Case sCode
                Case ""
                    lReturn = CType(AddRIModel(oRIModel.ClaimXOLID, oNode, "CLM", "XOL Per Claim - ", oRIModel.ClaimXOLLimit), gPMConstants.PMEReturnCode)
                Case "CLM"
                    lReturn = CType(AddRIModel(oRIModel.ClaimXOLID, oNode, "CLM", "XOL Layer " & lLayer + 1 & " - ", oRIModel.ClaimXOLLimit, lLayer + 1), gPMConstants.PMEReturnCode)
                Case Else
                    lReturn = gPMConstants.PMEReturnCode.PMTrue
            End Select
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("AddRIModel", "Unable to add Per Claim XOL Model")
            End If
        End If

        ' Check for catastrophe XOL
        'developer guide no. 131
        If (oRIModel.CatXOLID > 0) Then
            If Not String.IsNullOrEmpty(sCode) Then
                If (sCode.Substring(0, 3) <> "CLM") Then
                    ' Add appropriate XOL node
                    Select Case sCode.Substring(0, 3)
                        'Case ""
                        '    lReturn = CType(AddRIModel(oRIModel.CatXOLID, oNode, "CAT", "XOL Catastrophe - ", oRIModel.CatXOLLimit, oRIModel.CatXOLReinstatements), gPMConstants.PMEReturnCode)
                        Case "CAT"
                            lReturn = CType(AddRIModel(oRIModel.CatXOLID, oNode, "CAT", "XOL Layer " & lLayer + 1 & " - ", oRIModel.CatXOLLimit, oRIModel.CatXOLReinstatements, lLayer + 1), gPMConstants.PMEReturnCode)
                        Case Else
                            lReturn = gPMConstants.PMEReturnCode.PMTrue
                    End Select
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("AddRIModel", "Unable to add Catastrophe XOL Model")
                    End If
                End If
            Else
                lReturn = CType(AddRIModel(oRIModel.CatXOLID, oNode, "CAT", "XOL Catastrophe - ", oRIModel.CatXOLLimit, oRIModel.CatXOLReinstatements), gPMConstants.PMEReturnCode)
            End If
        End If

        GoTo Finally_Renamed
Catch_Renamed:
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:
        Return result

    End Function

    Private Function AddTreatyParties(ByVal lTreatyID As Integer, Optional ByVal oParentNode As TreeNode = Nothing, Optional ByVal lTreatyTypeId As Integer = 0, Optional ByVal dCedingRate As Double = 0, Optional ByVal cLowerLimit As Decimal = 0, Optional ByVal cUpperLimit As Decimal = 0) As Integer

        Dim result As Integer = 0
        Dim sCaption As String = ""
        Dim oNode As TreeNode
        Dim vTreatyResults As Object

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "AddTreatyParties"

        ' Check if this model exists
        Dim vResults(,) As Object = Nothing
        Try

            vResults = m_cTreatyParties.Item("T" & lTreatyID)

        Catch
        End Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' If we don't have this treaty in the cache load it
        If Not Information.IsArray(vResults) Then
            ' Get RI Model header details

            lReturn = m_oTreaty.GetTreatyPartyList(v_lTreatyID:=lTreatyID, r_vTreatyParties:=vResults)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oTreaty.GetTreatyPartyList", "Unable to retrieve reinsurance model lines")
            End If

            ' Add to cache collection
            If Information.IsArray(vResults) Then
                m_cTreatyParties.Add(vResults, "T" & lTreatyID)
            End If
        End If

        ' Loop through parties
        If Information.IsArray(vResults) Then
            For lCount As Integer = vResults.GetLowerBound(1) To vResults.GetUpperBound(1)
                ' Build caption and add node


                sCaption = CStr(vResults(MainModule.TreatyPartyEnum.DBTPResolvedName, lCount)) & " [" & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, (vResults(MainModule.TreatyPartyEnum.DBTPSharePercent, lCount))) & "]"
                oNode = trvRIModel.Nodes.Find(oParentNode.Name, True)(0).Nodes.Add(sCaption, sCaption, "Party")
            Next
            lReturn = m_oTreaty.GetTreatyEffectivePeriod(lTreatyId:=lTreatyID, vResult:=vTreatyResults)

            If Information.IsArray(vTreatyResults) Then
                If Information.IsDate(vTreatyResults(0, 0)) Then

                    oNode = trvRIModel.Nodes.Find(oParentNode.Name, True)(0).Nodes.Add("Effective Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, (vTreatyResults(0, 0))), "Effective Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, (vTreatyResults(0, 0))), "Note")
                End If
                If Information.IsDate(vTreatyResults(1, 0)) Then

                    oNode = trvRIModel.Nodes.Find(oParentNode.Name, True)(0).Nodes.Add("Expiry Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, (vTreatyResults(1, 0))), "Expiry Date: " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateLong, (vTreatyResults(1, 0))), "Note")
                End If
            End If

            If lTreatyTypeId = 2 Then
                sCaption = "Ceding Rate:" & (dCedingRate / 100).ToString("P4")
                oNode = trvRIModel.Nodes.Find(oParentNode.Name, True)(0).Nodes.Add(sCaption, sCaption, "Note")
                sCaption = "Lower Limit:" & cLowerLimit.ToString()
                oNode = trvRIModel.Nodes.Find(oParentNode.Name, True)(0).Nodes.Add(sCaption, sCaption, "Note")
                sCaption = "Upper Limit:" & cUpperLimit.ToString()
                oNode = trvRIModel.Nodes.Find(oParentNode.Name, True)(0).Nodes.Add(sCaption, sCaption, "Note")
            End If
        End If

        GoTo Finally_Renamed
Catch_Renamed:
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:
        Return result
    End Function

    Private Function NewRIModel() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "NewRIModel"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create new model
            m_oRootModel = New RIModelCache()

            ' Populate default values
            m_oRootModel.RIModelID = 0
            m_oRootModel.Description = "New Model"
            m_oRootModel.EffectiveDate = DateTime.Today

            m_oRootModel.ExpiryDate = Nothing
            m_oRootModel.ClaimsAllocation = 0
            m_oRootModel.CurrencyDescription = ""
            m_oRootModel.ClaimXOLID = 0
            m_oRootModel.ClaimXOLLimit = 0
            m_oRootModel.CatXOLID = 0
            m_oRootModel.CatXOLLimit = 0
            m_oRootModel.CatXOLReinstatements = 0

            ' Add to cache
            m_cRIModels.Add(m_oRootModel, "M0")

            ' Populate tree
            lReturn = CType(AddRIModel(0), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("AddRIModel(0)", "Unable to add new ri model")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '                       USERCONTROL EVENTS
    ' ***************************************************************** '
    Private Sub UserControl_Initialize()
        ' Initialise the cache collections
        m_cRIModels = New Collection()
        m_cTreatyParties = New Collection()
    End Sub

    Private Sub uctRIModelControl_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        Try
            trvRIModel.SetBounds(0, 0, ClientRectangle.Width, ClientRectangle.Height)
        Catch
        End Try
    End Sub

    Private Sub UserControl_Terminate()
        Dim lReturn As Integer
        Const kMethodName As String = "UserControl_Terminate"

        Try

            ' Release business objects
            If Not (m_oRIModel Is Nothing) Then

                m_oRIModel.Dispose()
                m_oRIModel = Nothing
            End If

            If Not (m_oTreaty Is Nothing) Then

                m_oTreaty.Dispose()
                m_oTreaty = Nothing
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)
        Finally

        End Try
        Exit Sub
    End Sub




End Class
