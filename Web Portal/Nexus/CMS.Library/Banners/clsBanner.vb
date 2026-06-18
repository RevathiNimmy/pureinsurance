Imports System.Web.Security

Namespace Banner

#Region "Helpers"
    ''' <summary>
    ''' Public enum used to define the publication limits available 
    ''' to banners.
    ''' </summary>
    ''' <remarks>
    ''' NoLimit / Do not apply exposure limit
    ''' TimeLimit / Display from live date to expiry date, no count limit
    ''' CountLimit / Display until count limit is reached 
    ''' ExposuresPerDay / Display no more then count limit per day
    ''' ExposuresPerWeek / Display no more then count limit per week
    ''' ExposuresPerMonth / Display no more then count limit per month
    ''' </remarks>
    Public Enum BannerExposureLimit
        NoLimit = 0
        TimeLimit = 1
        CountLimit = 2
        ExposuresPerDay = 3
        ExposuresPerWeek = 4
        ExposuresPerMonth = 5
    End Enum


#End Region

#Region "Banner"

    ''' <summary>
    ''' Public class used to represent a banner that may be used for
    ''' publication in the CMS.
    ''' </summary>
    Public Class Banner
        Inherits SiteMap.ContentHistory

#Region "Locals"
        Private M_ID As Integer
        Private M_Category As BannerCategory
        Private M_Type As BannerType
        Private M_source As String
        Private M_Url As String
        Private M_Name As String
        Private M_ImagePath As String
        Private M_Priority As Integer
        Private M_CodeSnippet As String
        Private M_ThirdPartyUse As Boolean
        Private M_Exposure As BannerExposureLimit
        Private M_ExposureTarget As Integer
        Private M_ExposureTargetRemaining As Integer
        Private M_ExposureTotal As Integer
        Private M_ExposureLast As DateTime
        Private M_ExposureLiveDate As DateTime
        Private M_ExposureExpireDate As DateTime

        Private M_CategoryPath As String

        Private M_Published As Boolean
        Private M_Submitted As Boolean
#End Region

#Region "Constructors"

        ''' <summary>
        ''' Banner constructor
        ''' </summary>
        ''' <param name="pBannerID">Banner Identity, Integer, Required</param>
        ''' <param name="pBannerTypeID">Banner Type Identity, Integer, Required</param>
        ''' <param name="pBannerCategoryID">Banner Category Identity, Integer, Required</param>
        ''' <param name="pBannerName">Banner Name, String, Required</param>
        ''' <param name="pBannerUrl">Banner URL, String, Required (at least string.empty)</param>
        ''' <param name="pBannerImagePath">Banner Image Path, String, Required (at least string.empty)</param>
        ''' <param name="pBannerPriority">Banner Priority, Integer (1-5), Required</param>
        ''' <param name="pBannerCodeSnippet">Banner Code Snippet, String, Required (at least string.empty)</param>
        ''' <param name="pExposureTarget">Banner Exposure Target Impressions, Integer, Required</param>
        ''' <param name="pExposureTargetRemaining">Banner Exposure Target Impressions Remaining, Integer, Required</param>
        ''' <param name="pExposureLimit">Banner Exposure Limit By, BannerExposureLimit, Required</param>
        ''' <param name="pBannerCategoryPath">Banner Category Path, String, Required</param>
        ''' <param name="pExposureTotal">Banner Exposure Total, Integer, Required</param>
        ''' <param name="pExposureLast">Banner Exposure Last Impression Date, Datetime, Required</param>
        ''' <param name="pExposureLiveDate">Banner Exposure Go Live Date, Datetime, Required</param>
        ''' <param name="pExposureExpireDate">Banner Exposure Expire Date, Datetime, Required</param>
        ''' <param name="pThirdPartyUse">Banner Exposure Third Party Use, Boolean, Required</param>
        ''' <param name="pAdminGuid">Admin GUID Used By Parent Class, Guid, Required</param>
        ''' <param name="pAction">Action Used By Parent Class, String, Required</param>
        ''' <param name="pPublished">Banner Published, Boolean, Required</param>
        ''' <param name="pSubmitted">Banner Submitted, Boolean, Required</param>
        ''' <param name="pAdminDate">Admin Date Used By Parent Class, Datetime, Required</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal pBannerID As Integer, ByVal pBannerTypeID As Integer, ByVal pBannerCategoryID As Integer, ByVal pBannerName As String, _
                    ByVal pBannerUrl As String, ByVal pBannerSource As String, ByVal pBannerImagePath As String, ByVal pBannerPriority As Integer, ByVal pBannerCodeSnippet As String, _
                    ByVal pExposureTarget As Integer, ByVal pExposureTargetRemaining As Integer, ByVal pExposureLimit As BannerExposureLimit, _
                    ByVal pBannerCategoryPath As String, ByVal pExposureTotal As Integer, ByVal pExposureLast As DateTime, ByVal pExposureLiveDate As DateTime, _
                    ByVal pExposureExpireDate As DateTime, ByVal pThirdPartyUse As Boolean, ByVal pAdminGuid As Guid, ByVal pAction As String, _
                    ByVal pPublished As Boolean, ByVal pSubmitted As Boolean, ByVal pAdminDate As Date)

            MyBase.New(0, pAdminDate, Membership.GetUser(pAdminGuid).UserName, pAction)

            M_ID = pBannerID
            M_Name = pBannerName
            M_Url = pBannerUrl
            M_Type = Functions.GetBannerType(pBannerTypeID)
            M_Category = Functions.GetBannerCategory(pBannerCategoryID)
            M_source = pBannerSource
            M_ImagePath = pBannerImagePath
            M_CategoryPath = pBannerCategoryPath
            M_Priority = pBannerPriority
            M_CodeSnippet = pBannerCodeSnippet
            M_ThirdPartyUse = pThirdPartyUse
            M_Exposure = pExposureLimit
            M_ExposureTarget = pExposureTarget
            M_ExposureTargetRemaining = pExposureTargetRemaining
            M_ExposureTotal = pExposureTotal
            If Not pExposureLast = Nothing Then
                M_ExposureLast = pExposureLast
            End If
            M_ExposureLiveDate = pExposureLiveDate
            M_ExposureExpireDate = pExposureExpireDate

            M_Published = pPublished
            M_Submitted = pSubmitted

        End Sub
#End Region

#Region "Properties"


        Public ReadOnly Property ID() As Integer
            Get
                Return M_ID
            End Get
        End Property
        Public ReadOnly Property Url() As String
            Get
                Return M_Url
            End Get
        End Property
        Public ReadOnly Property Name() As String
            Get
                Return M_Name
            End Get
        End Property
        Public ReadOnly Property ImagePath() As String
            Get
                Return M_ImagePath
            End Get
        End Property
        Public ReadOnly Property Priority() As Integer
            Get
                Return M_Priority
            End Get
        End Property
        Public ReadOnly Property PriorityLevel() As String
            Get
                Select Case Priority
                    Case 0
                        Return "Lowest"
                    Case 1
                        Return "Lower"
                    Case 3
                        Return "Normal"
                    Case 4
                        Return "Higher"
                    Case 5
                        Return "Highest"
                    Case Else
                        Return "Not Set"
                End Select
            End Get
        End Property
        Public ReadOnly Property CodeSnippet() As String
            Get
                Return M_CodeSnippet
            End Get
        End Property
        Public ReadOnly Property Source() As String
            Get
                Select Case M_source
                    Case "I"
                        Return "Internal"
                    Case "E"
                        Return "External"
                    Case Else
                        Return "Unknow source"
                End Select
            End Get
        End Property
        Public ReadOnly Property ThirdPartyUse() As Boolean
            Get
                Return M_ThirdPartyUse
            End Get
        End Property
        Public ReadOnly Property Published() As Boolean
            Get
                Return M_Published
            End Get
        End Property
        Public ReadOnly Property Submitted() As Boolean
            Get
                Return M_Submitted
            End Get
        End Property

        Public ReadOnly Property TypeID() As Integer
            Get
                Return Type.ID
            End Get
        End Property
        Public ReadOnly Property TypeName() As String
            Get
                Return Type.Name
            End Get
        End Property
        Public ReadOnly Property TypeDetails() As String
            Get
                Return Type.Details
            End Get
        End Property
        Public ReadOnly Property Type() As BannerType
            Get
                Return M_Type
            End Get
        End Property

        Public ReadOnly Property Category() As BannerCategory
            Get
                Return M_Category
            End Get
        End Property
        Public ReadOnly Property CategoryID()
            Get
                Return Category.ID
            End Get
        End Property
        Public ReadOnly Property CategoryPath() As String
            Get
                Return M_CategoryPath
            End Get
        End Property
        Public ReadOnly Property CategoryName() As String
            Get
                Return Category.Name
            End Get
        End Property

        Public ReadOnly Property ExposureLimit() As BannerExposureLimit
            Get
                Return M_Exposure
            End Get
        End Property
        Public ReadOnly Property ExposureLimitTarget() As Integer
            Get
                Return M_ExposureTarget
            End Get
        End Property
        Public ReadOnly Property ExposureLimitTargetRemaining()
            Get
                Return M_ExposureTargetRemaining
            End Get
        End Property
        Public ReadOnly Property ExposureTotal() As Integer
            Get
                Return M_ExposureTotal
            End Get
        End Property
        Public ReadOnly Property ExposureLast() As DateTime
            Get
                Return M_ExposureLast
            End Get
        End Property
        Public ReadOnly Property ExposureLiveDate() As DateTime
            Get
                Return M_ExposureLiveDate
            End Get
        End Property
        Public ReadOnly Property ExposureExpireDate() As DateTime
            Get
                Return M_ExposureExpireDate
            End Get
        End Property

        Public ReadOnly Property Exposure()
            Get
                Return GetExposureDetails()
            End Get
        End Property

        Private Function GetExposureDetails() As String
            Dim strExposureText As String = String.Empty
            Select Case ExposureLimit
                Case BannerExposureLimit.NoLimit
                    strExposureText = "None Applied"
                Case BannerExposureLimit.TimeLimit
                    strExposureText = "Only displayed from <b>{0}</b> to <b>{1}</b>"
                    strExposureText = String.Format(strExposureText, ExposureLiveDate.ToShortDateString, ExposureExpireDate.ToShortDateString)
                Case BannerExposureLimit.CountLimit
                    strExposureText = "Limited to <b>{0}</b> exposures with <b>{1}</b> remaining."
                    strExposureText = String.Format(strExposureText, ExposureLimitTarget, ExposureLimitTargetRemaining)
                Case BannerExposureLimit.ExposuresPerDay
                    strExposureText = "Limited to <b>{0}</b> exposures per day with <b>{1}</b> remaining for today."
                    strExposureText = String.Format(strExposureText, ExposureLimitTarget, ExposureLimitTargetRemaining)
                Case BannerExposureLimit.ExposuresPerWeek
                    strExposureText = "Limited to <b>{0}</b> exposures per week with <b>{1}</b> remaining this week."
                    strExposureText = String.Format(strExposureText, ExposureLimitTarget, ExposureLimitTargetRemaining)
                Case BannerExposureLimit.ExposuresPerMonth
                    strExposureText = "Limited to <b>{0}</b> exposures per month with <b>{1}<b> remaining this month."
                    strExposureText = String.Format(strExposureText, ExposureLimitTarget, ExposureLimitTargetRemaining)
                Case Else
                    strExposureText = "Unknown or invalid details"
            End Select
            Return strExposureText
        End Function
#End Region



    End Class

#End Region


    Public Class BannerType

        Private M_ID As Integer
        Private M_Name As String
        Private M_Width As Integer
        Private M_Height As Integer
        Private M_Details As String

        Public Sub New(ByVal pBannerTypeID As Integer, ByVal pName As String, _
                        ByVal pWidth As Integer, ByVal pHeight As Integer, ByVal pDetails As String)

            M_ID = pBannerTypeID
            M_Name = pName
            M_Width = pWidth
            M_Height = pHeight
            M_Details = pDetails
        End Sub

        Public ReadOnly Property ID() As Integer
            Get
                Return M_ID
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return M_Name
            End Get
        End Property

        Public ReadOnly Property Details() As String
            Get
                Return M_Details
            End Get
        End Property

        Public ReadOnly Property Width() As Integer
            Get
                Return M_Width
            End Get
        End Property

        Public ReadOnly Property Height() As Integer
            Get
                Return M_Height
            End Get
        End Property
    End Class

    Public Class BannerCategory

        Private M_ID As Integer
        Private M_name As String
        Private M_portal_id As Integer
        Private M_parent_id As Integer
        Private M_depth As Integer
        Private M_position As Integer
        Private M_no_banners As Integer
        Private M_no_punlished_banners As Integer


        Public ReadOnly Property ID() As Integer
            Get
                Return M_ID
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return M_name
            End Get
        End Property

        Public ReadOnly Property PortalID() As Integer
            Get
                Return M_portal_id
            End Get
        End Property

        Public ReadOnly Property ParentID() As Integer
            Get
                Return M_parent_id
            End Get
        End Property

        Public ReadOnly Property Depth() As Integer
            Get
                Return M_depth
            End Get
        End Property

        Public ReadOnly Property Position() As Integer
            Get
                Return M_position
            End Get
        End Property

        Public ReadOnly Property ParentCategory() As BannerCategory
            Get
                Return Functions.GetBannerCategory(ParentID)
            End Get
        End Property

        Public ReadOnly Property NoBanners() As Integer
            Get
                Return M_no_banners
            End Get
        End Property

        Public ReadOnly Property NoBannersPublished() As Integer
            Get
                Return M_no_punlished_banners
            End Get
        End Property

        Public Sub New(ByVal pID As Integer, ByVal pName As String, ByVal pPortalID As String, ByVal pParentID As Integer, ByVal pDepth As Integer, ByVal pPosition As Integer, ByVal pTotalCount As Integer, ByVal pPublishedCount As Integer)
            M_ID = pID
            M_name = pName
            M_portal_id = pPortalID
            M_parent_id = pParentID
            M_depth = pDepth
            M_position = pPosition
            M_no_banners = pTotalCount
            M_no_punlished_banners = pPublishedCount
        End Sub

    End Class
End Namespace
