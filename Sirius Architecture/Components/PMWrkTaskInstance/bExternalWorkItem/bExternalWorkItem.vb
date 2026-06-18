Option Explicit On
Imports System.Configuration
Imports System.Security.Principal
Imports bExternalWorkItem.IntegrationProxyLayer
Imports Dataract.e5.Domain.Entities
Imports Dataract.e5.Domain.Interfaces
Imports Dataract.FW.Communication
Imports Dataract.FW.Domain
Imports SSP.Shared

Namespace e5IntegrationProxyLayer
    Public NotInheritable Class Business

        Public Const ACApp As String = "bExternalWorkItem"
        Private Const ACClass As String = "Business"

        Private m_lReturn As Integer
        Private m_sUsername As String
        Private m_sPassword As String
        Private m_nUserID As Integer
        Private m_sCallingAppName As String
        Private m_nSourceID As Integer
        Private m_nLanguageID As Integer
        Private m_nCurrencyID As Integer
        Private m_sGisDataModelCode As String
        Private m_nLogLevel As Integer
        Private oE5ConfigurationSection As Object


        Private Shared ReadOnly Proxy As New ServiceProxy()
        Private Shared SiteCollectionId As New Guid("C1DB3EB6-DAAB-4767-8197-0CE6FD42C21E")
        Const g_sClient As String = "E5"





#Region "Intialise"
        Public Function Initialise( _
                       ByVal sUsername As String, _
                       ByVal sPassword As String, _
                       ByVal nUserID As Integer, _
                       ByVal nSourceID As Integer, _
                       ByVal nLanguageID As Integer, _
                       ByVal nCurrencyID As Integer, _
                       ByVal iLogLevel As Integer, _
                       ByVal sCallingAppName As String, _
                       Optional ByVal vDatabase As Object = Nothing) As Integer

            Try
                m_sUsername = sUsername
                m_sPassword = sPassword
                m_nUserID = nUserID
                m_nLanguageID = nLanguageID
                m_nSourceID = nSourceID
                m_nCurrencyID = nCurrencyID
                m_nLogLevel = iLogLevel
                m_sCallingAppName = sCallingAppName


                oE5ConfigurationSection = ConfigurationManager.GetSection("E5Config")


                If oE5ConfigurationSection IsNot Nothing Then
                    SiteCollectionId = New Guid(CType(oE5ConfigurationSection, Object)("SiteCollectionID").ToString())
                Else
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="InitialiseFailed",
                                       vApp:=ACApp,
                                       vClass:=ACClass,
                                       vMethod:="Initialise",
                                       vErrNo:=Informations.Err().Number,
                                       vErrDesc:="Unable to read E5Config")
                End If


            Catch ex As Exception
                Return ExternalWorkItemConstants.PMEReturnCode.PMFalse
            End Try

            Return ExternalWorkItemConstants.PMEReturnCode.PMTrue

        End Function
#End Region

#Region "CreateExternalWorkItem"
        Public Function CreateExternalWorkItem(ByVal sExternalCategoryCode As String,
                                               ByVal oKeyArray(,) As Object,
                                               ByRef r_uExternalWorkId As System.Guid,
                                               ByVal sGuidParentWorkId As String,
                                               ByVal nExternalTaskStatus As Integer) As Integer


            Dim nResult As Integer


            Try

                nResult = gPMConstants.PMEReturnCode.PMTrue
                Dim nCategoryId1 As Integer
                Dim nCategoryId2 As Integer
                Dim nCategoryId3 As Integer

                'Read the configuration
                ReadConfiguration(sExternalCategoryCode, nCategoryId1, nCategoryId2, nCategoryId3)


                Dim oWorkEntity As New WorkEntity()
                m_lReturn = CreateWork(nCategoryID1:=nCategoryId1,
                                            nCategoryID2:=nCategoryId2,
                                            nCategoryID3:=nCategoryId3,
                                            sGuidParentWorkId:=sGuidParentWorkId,
                                            r_oWorkEntity:=oWorkEntity,
                                            oKeyArray:=oKeyArray,
                                            nExternalTaskStatus:=nExternalTaskStatus)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'return the external work id 
                r_uExternalWorkId = oWorkEntity.WorkId

            Catch ex As Exception
                nResult = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateNewFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateNew", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

                Return nResult

            End Try

            Return nResult

        End Function
#End Region

#Region "UpdateExternalWorkItemStatus"
        Public Function UpdateExternalWorkItemStatus(ByVal uGuidWorkId As System.Guid,
                                                     ByVal nExternalTaskStatus As Integer, ByRef sErrorMessage As String) As Integer


            Dim nResult As Integer
            Dim sInnerErrMessage As String = String.Empty
            'nStatusInd = 0 , Active
            'nStatusInd = 6 , Complete

            Try

                nResult = gPMConstants.PMEReturnCode.PMTrue

                Dim oWorkEntity As New WorkEntity()
                'retrieve parent WorkEntity object
                If uGuidWorkId <> Guid.Empty Then
                    m_lReturn = Retrieve(uGuidWorkId, oWorkEntity, sInnerErrMessage)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sErrorMessage = sInnerErrMessage
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                Dim bCallE5 As Boolean = False
                'Do not update , return with true status
                If nExternalTaskStatus = oWorkEntity.Status.StatusId Then
                    bCallE5 = False
                ElseIf nExternalTaskStatus = 1 AndAlso (oWorkEntity.Status.StatusId = 2 OrElse oWorkEntity.Status.StatusId = 4 OrElse oWorkEntity.Status.StatusId = 5) Then
                    Dim newItem = New WorkResolvedPropertyEntity()
                    newItem.WorkPropertyId = "__RetainAssignedUser"
                    newItem.Value = True
                    oWorkEntity.Properties.Add(newItem)

                    bCallE5 = True
                ElseIf nExternalTaskStatus = 4 AndAlso oWorkEntity.Status.StatusId <> 4 AndAlso oWorkEntity.Status.StatusId <> 5 Then
                    bCallE5 = True
                End If


                If bCallE5 = False Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                Dim oWindowsIdentity__1 = WindowsIdentity.GetCurrent()
                oWorkEntity.EntityAction = EntityAction.Updated
                oWorkEntity.Status = New StatusEntity With {.StatusId = nExternalTaskStatus}

                If nExternalTaskStatus = 4 OrElse nExternalTaskStatus = 5 Then
                    oWorkEntity.CompletionUser = New Dataract.e5.Domain.Entities.UserEntity() _
                    With {.UserName = If(oWindowsIdentity__1 IsNot Nothing,
                                         oWindowsIdentity__1.Name, "_e5IntegrationConsole")}
                    oWorkEntity.CompletionDate = DateTime.Now
                ElseIf (oWorkEntity.CompletionUser IsNot Nothing) Then
                    oWorkEntity.CompletionUser = Nothing
                    oWorkEntity.CompletionDate = DateTime.MinValue
                End If

                Dim oRequest = New SaveRequest() With {.Value = oWorkEntity}
                Dim oResponse = Proxy.Save(Of IWorkService)(oRequest)
                Dim oWorkEntityResponse As WorkEntity

                'This statement will throw an exception if its not able to validate
                oWorkEntityResponse = Proxy.ValidateAndGetSingleResponse(Of WorkEntity)(oResponse)

                'If above statment validates and does not throw an error, 
                'then verify we get a guid value other then guid.empty
                If oWorkEntityResponse.WorkId = Guid.Empty Then
                    Throw New Exception("WorkId is Guid.Empty")
                End If

            Catch ex As Exception
                nResult = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateExternalWorkItemStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateExternalWorkItemStatus", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
                sErrorMessage = ex.Message
                Return nResult

            End Try

            Return nResult

        End Function
#End Region

#Region "CreateWork"
        Private Function CreateWork(ByVal nCategoryID1 As Integer,
                                           ByVal nCategoryID2 As Integer,
                                           ByVal nCategoryID3 As Integer,
                                           ByRef r_oWorkEntity As WorkEntity,
                                           ByVal sGuidParentWorkId As String,
                                           ByVal oKeyArray(,) As Object,
                                           ByVal nExternalTaskStatus As Integer) As Integer

            Dim nResult As Integer = 0
            Dim sErrorMessage As String = String.Empty
            Try
                nResult = gPMConstants.PMEReturnCode.PMTrue

                Dim guidParentWorkId As System.Guid
                If sGuidParentWorkId <> "" Then
                    guidParentWorkId = New Guid(sGuidParentWorkId)
                End If
                Dim oWork = New WorkEntity() ' create a work entity
                oWork.EntityAction = EntityAction.Created  ' signals creation attempt
                oWork.SiteCollectionId = SiteCollectionId  ' e5_Config.Enterprise.Id - the e5 instance id
                oWork.Status = New StatusEntity With {.StatusId = nExternalTaskStatus}   'signals Launch status

                oWork.Category = New CategoryEntity With {.CategoryId = nCategoryID3,
                                                          .Level = 3,
                                                          .ParentCategory =
                                                          New CategoryEntity() With {.CategoryId = nCategoryID2,
                                                                                     .Level = 2,
                                                                                     .ParentCategory =
                                                                                     New CategoryEntity() With {.CategoryId = nCategoryID1,
                                                                                                                .Level = 1}}}

                Dim oWindowsIdentity__1 = WindowsIdentity.GetCurrent()

                oWork.CreationUser = New Dataract.e5.Domain.Entities.UserEntity() _
                With {.UserName = If(oWindowsIdentity__1 IsNot Nothing,
                                     oWindowsIdentity__1.Name, "_e5IntegrationConsole")}

                Dim oParentWorkEntity As New WorkEntity()
                'If we are trying to create a child, so retrieve parent WorkEntity object
                If guidParentWorkId <> Guid.Empty Then
                    m_lReturn = Retrieve(guidParentWorkId, oParentWorkEntity, sErrorMessage)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    Else
                        'Retrieved the parent work entity correctly, we can set it now
                        oWork.Parent = oParentWorkEntity
                    End If
                End If

                'Add the property values

                Dim sPropertyName As String = String.Empty
                Dim oPropertyValue As Object

                If oKeyArray IsNot Nothing Then
                    Dim nLowerBound As Integer = oKeyArray.GetLowerBound(1)
                    Dim nUpperBound As Integer = oKeyArray.GetUpperBound(1)

                    For iCount As Integer = nLowerBound To nUpperBound
                        sPropertyName = oKeyArray(0, iCount).ToString()
                        oPropertyValue = oKeyArray(1, iCount)

                        If sPropertyName <> String.Empty Then
                            Dim oProperty = New WorkResolvedPropertyEntity()
                            oProperty.WorkPropertyId = sPropertyName
                            oProperty.Value = oPropertyValue
                            oProperty.SerialisedValue = oProperty.Value.ToString()

                            oWork.Properties.Add(oProperty)
                        End If

                    Next
                End If

                Dim oRequest = New SaveRequest() With {.Value = oWork}
                Dim oResponse = Proxy.Save(Of IWorkService)(oRequest)
                Dim bContinueProcessing As Boolean = False

                If oResponse.Notifications.Count > 0 Then
                    'Read all the ExceptionKeyNames configuration section
                    Dim sExceptionKeyNames() As String = CType(oE5ConfigurationSection, Object)("ExceptionKeyNames").ToString().Split(";")
                    'Loop through all the notifications
                    For nNotificationCount As Integer = 0 To oResponse.Notifications.Count - 1

                        If sExceptionKeyNames.GetLength(0) > 0 Then
                            'Loop through all the Exception configurations
                            For nExceptionCount As Integer = 0 To sExceptionKeyNames.GetLength(0) - 1
                                'Check for a match
                                If oResponse.Notifications(nNotificationCount).Description.ToUpper() = CType(oE5ConfigurationSection, Object)("Exception_" + sExceptionKeyNames(nExceptionCount)).ToString().ToUpper() Then
                                    bContinueProcessing = True
                                    Exit For
                                End If
                            Next
                        End If
                        If bContinueProcessing = True Then
                            Exit For
                        End If
                    Next
                End If

                'Skip this as it will throw an exception if we validate it. 
                If bContinueProcessing = False Then
                    'This statement will throw an exception if its not able to validate
                    r_oWorkEntity = Proxy.ValidateAndGetSingleResponse(Of WorkEntity)(oResponse)

                    'If above statment validates and does not throw an error, 
                    'then verify we get a guid value other then guid.empty
                    If r_oWorkEntity.WorkId = Guid.Empty Then
                        Throw New Exception("WorkId is Guid.Empty")
                    End If
                End If

                Return nResult
            Catch ex As Exception
                nResult = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="CreateWork Failed",
                                   vApp:=ACApp,
                                   vClass:=ACClass,
                                   vMethod:="CreateWork",
                                   vErrNo:=Informations.Err().Number,
                                   vErrDesc:=ex.Message, excep:=ex)

                Return nResult
            End Try
        End Function
#End Region

#Region "RetrieveWork"
        ''' <summary>
        ''' Retrieves the work.
        ''' </summary>
        ''' <param name="uGuidParentWorkID">The work id.</param>
        ''' <returns></returns>
        Private Shared Function Retrieve(ByVal uGuidParentWorkID As Guid, ByRef r_oParentWorkEntity As WorkEntity, ByRef sInnerErrMessage As String) As Integer

            Dim nResult As Integer = 0
            Try


                nResult = gPMConstants.PMEReturnCode.PMTrue
                Dim oBuilder = Proxy.InitializeQueryBuilder(Of WorkEntity)(SiteCollectionId)
                oBuilder.EqualsTo(Function(t) t.WorkId, uGuidParentWorkID)

                Dim oRequest = New RetrieveRequest() With {.Query = oBuilder.Query}
                Dim oResponse = Proxy.Retrieve(Of IWorkService)(oRequest)
                r_oParentWorkEntity = Proxy.ValidateAndGetSingleResponse(Of WorkEntity)(oResponse)

                If r_oParentWorkEntity.WorkId = Guid.Empty Then
                    Throw New Exception("WorkId is Guid.Empty")
                End If

                Return nResult

            Catch ex As Exception
                nResult = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Retrieve Failed",
                                   vApp:=ACApp,
                                   vClass:=ACClass,
                                   vMethod:="Retrieve",
                                   vErrNo:=Informations.Err().Number,
                                   vErrDesc:=ex.Message, excep:=ex)
                sInnerErrMessage = ex.Message

                Return nResult
            End Try

        End Function
#End Region

#Region "UpdateProperty"
        ''' <summary>
        ''' Updates the work property.
        ''' </summary>
        ''' <param name="workId">The work id.</param>
        ''' <param name="sPropertyName">The property id.</param>
        ''' <param name="value">The value.</param>
        Private Shared Function UpdateProperty(ByVal workId As Guid, ByVal sPropertyName As String, ByVal value As Object) As Integer 'WorkResolvedPropertyEntity

            Dim nResult As Integer = 0

            Try


                nResult = gPMConstants.PMEReturnCode.PMTrue

                Dim [property] = New WorkResolvedPropertyEntity()
                [property].EntityAction = EntityAction.Updated
                [property].SiteCollectionId = SiteCollectionId
                [property].WorkId = workId
                [property].WorkPropertyId = sPropertyName
                [property].Value = value
                [property].SerialisedValue = [property].Value.ToString()

                ' submit to work service and return created work
                Dim oRequest = New SaveRequest() With {.Value = [property]}
                Dim oResponse = Proxy.Save(Of IWorkResolvedPropertyService)(oRequest)
                Proxy.ValidateAndGetSingleResponse(Of WorkResolvedPropertyEntity)(oResponse)

                Return nResult

            Catch ex As Exception
                nResult = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="UpdateProperty failed",
                                   vApp:=ACApp,
                                   vClass:=ACClass,
                                   vMethod:="UpdateProperty",
                                   vErrNo:=Informations.Err().Number,
                                   vErrDesc:=ex.Message, excep:=ex)

                Return nResult
            End Try

        End Function

#End Region

#Region "QueryWorkProperties"
        ''' <summary>
        ''' Queries the work properties.
        ''' </summary>
        ''' <param name="workId">The work id.</param>
        ''' <param name="propertyIdsToRetrieve">The property ids to retrieve.</param>
        ''' <returns></returns>
        Private Shared Function QueryProperties(ByVal workId As Guid,
                                                    ByVal propertyIdsToRetrieve As IEnumerable(Of String)) As IEnumerable(Of WorkResolvedPropertyEntity)

            Dim oIdsToRetrieve As IEnumerable(Of String) = If(propertyIdsToRetrieve IsNot Nothing, propertyIdsToRetrieve.ToArray(), New String(-1) {})

            Dim oResolvedProperties = New List(Of WorkResolvedPropertyEntity)()

            If Not oIdsToRetrieve.Any() Then
                ' return all work properties
                Dim builder = Proxy.InitializeQueryBuilder(Of WorkResolvedPropertyEntity)(SiteCollectionId)
                builder.EqualsTo(Function(t) t.WorkId, workId)

                Dim oRequest = New RetrieveRequest() With {.Query = builder.Query}
                Dim oResponse = Proxy.Retrieve(Of IWorkResolvedPropertyService)(oRequest)
                oResolvedProperties.AddRange(Proxy.ValidateAndGetManyResponse(Of WorkResolvedPropertyEntity)(oResponse))
            Else
                For Each propertyId As String In oIdsToRetrieve
                    Dim oBuilder = Proxy.InitializeQueryBuilder(Of WorkResolvedPropertyEntity)(SiteCollectionId)
                    oBuilder.EqualsTo(Function(t) t.WorkId, workId)
                    oBuilder.EqualsTo(Function(t) t.WorkPropertyId, propertyId)

                    Dim oRequest = New RetrieveRequest() With {.Query = oBuilder.Query}
                    Dim oResponse = Proxy.Retrieve(Of IWorkResolvedPropertyService)(oRequest)
                    oResolvedProperties.Add(Proxy.ValidateAndGetSingleResponse(Of WorkResolvedPropertyEntity)(oResponse))
                Next
            End If

            Return oResolvedProperties
        End Function
#End Region

#Region "ReadConfiguration"
        Private Function ReadConfiguration(ByVal sType As String,
                                           ByRef rCategoryId1 As Integer,
                                           ByRef rCategoryId2 As Integer,
                                           ByRef rCategoryId3 As Integer) As Integer

            Dim nResult As Integer = 0

            Try


                nResult = gPMConstants.PMEReturnCode.PMTrue


                If oE5ConfigurationSection IsNot Nothing Then
                    rCategoryId1 = CType(oE5ConfigurationSection, Object)(sType + "_CategoryID_1").ToString()
                    rCategoryId2 = CType(oE5ConfigurationSection, Object)(sType + "_CategoryID_2").ToString()
                    rCategoryId3 = CType(oE5ConfigurationSection, Object)(sType + "_CategoryID_3").ToString()

                Else
                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="ReadConfigurationFailed",
                                       vApp:=ACApp,
                                       vClass:=ACClass,
                                       vMethod:="ReadConfiguration",
                                       vErrNo:=Informations.Err().Number,
                                       vErrDesc:="Unable to read E5Config")
                End If


                Return nResult

            Catch ex As Exception
                nResult = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                   sMsg:="ReadConfiguration Failed",
                   vApp:=ACApp,
                   vClass:=ACClass,
                   vMethod:="ReadConfiguration",
                   vErrNo:=Informations.Err().Number,
                   vErrDesc:=ex.Message)
                Return nResult
            End Try


        End Function
#End Region



    End Class

End Namespace