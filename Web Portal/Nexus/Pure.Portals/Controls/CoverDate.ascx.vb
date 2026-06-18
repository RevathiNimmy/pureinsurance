Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Controls_CoverDate
        Inherits System.Web.UI.UserControl

        Private bShowEndDate As Boolean = True, bAllowEditEndDate = True
        Private iAllowFutureFromDate As Integer = 0, iAllowFutureToDate = 0, iAllowBackFromDate = 0

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Session(CNMTAType) = MTAType.PERMANENT Then
                'Control should not be shown during MTA
                PnlCoverDate.Visible = False
            Else 'New Business
                If CType(Session.Item(CNMode), Mode) = Mode.View Then
                    'if user is in View Mode then controls will be disabled
                    START_DATE__RISKBASE.Enabled = False
                    END_DATE__RISKBASE.Enabled = False
                    rangevldStartDate.Enabled = False
                    rangevldEndDate.Enabled = False
                Else
                    'In all conditions, we need to fire an event called "onblur"
                    Dim oProductConfig As Config.Product = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork) _
                    .Portals.Portal(CMS.Library.Portal.GetPortalID()).Products.Product(CType(Session(CNQuote), NexusProvider.Quote).ProductCode)

                    Dim tsTimeScale As Config.TimeScale = oProductConfig.CoverDate.TimeScale
                    Dim iPeriod As Integer = oProductConfig.CoverDate.Period
                    Dim bTrueMonthlyPolicy As Boolean = oProductConfig.CoverDate.TrueMonthlyPolicy
                    Dim sMidnightRenewal As String = oProductConfig.CoverDate.MidnightRenewal

                    START_DATE__RISKBASE.Attributes.Add("onblur", "ChangeEndDate('" & tsTimeScale & "', '" & iPeriod & "', '" & bTrueMonthlyPolicy & "', '" & sMidnightRenewal & "')")
                    START_DATE__RISKBASE.Attributes.Add("onfocus", "ChangeEndDate('" & tsTimeScale & "', '" & iPeriod & "', '" & bTrueMonthlyPolicy & "', '" & sMidnightRenewal & "')")

                    'range from Cover Start From Date
                    rangevldStartDate.Type = ValidationDataType.Date
                    rangevldStartDate.MinimumValue = Date.Today.Date.AddMonths(-iAllowBackFromDate)
                    rangevldStartDate.MaximumValue = Date.Today.Date.AddMonths(iAllowFutureFromDate)

                    'range from Cover End For Date
                    rangevldEndDate.Type = ValidationDataType.Date
                    rangevldEndDate.MinimumValue = Date.Today.Date.AddMonths(-iAllowBackFromDate)
                    rangevldEndDate.MaximumValue = Date.Today.Date.AddMonths(iAllowFutureToDate)

                    If bShowEndDate Then
                        'if bShowEndDate=true then only End Date will be visible
                        END_DATE__RISKBASE.Style("Visibility") = "visible"
                        lbl_CoverEndDate.Visible = True
                        If bAllowEditEndDate = False Then
                            'if bAllowEditEndDate=true then only End Date will be editable
                            END_DATE__RISKBASE.ReadOnly = True
                            calEndDate.Enabled = False
                            rangevldEndDate.Enabled = False 'added
                        End If
                    Else
                        END_DATE__RISKBASE.Style("Visibility") = "hidden"
                        lbl_CoverEndDate.Visible = False
                        calEndDate.Visible = False
                    End If
                End If
            End If
        End Sub

        Public WriteOnly Property ShowEndDate() As Boolean
            Set(ByVal value As Boolean)
                bShowEndDate = value
            End Set
        End Property

        Public WriteOnly Property AllowEditEndDate() As Boolean
            Set(ByVal value As Boolean)
                bAllowEditEndDate = value
            End Set
        End Property

        Public WriteOnly Property AllowFutureFromDate() As Integer
            Set(ByVal value As Integer)
                iAllowFutureFromDate = value
            End Set
        End Property

        Public WriteOnly Property AllowFutureToDate() As Integer
            Set(ByVal value As Integer)
                iAllowFutureToDate = value
            End Set
        End Property

        Public WriteOnly Property AllowBackFromDate() As Integer
            Set(ByVal value As Integer)
                iAllowBackFromDate = value
            End Set
        End Property

        Public Property CoverStartDate() As String
            Get
                Return START_DATE__RISKBASE.Text
            End Get
            Set(ByVal value As String)
                START_DATE__RISKBASE.Text = value
            End Set
        End Property

        Public Property CoverEndDate() As String
            Get
                Return END_DATE__RISKBASE.Text
            End Get
            Set(ByVal value As String)
                END_DATE__RISKBASE.Text = value
            End Set
        End Property

    End Class

End Namespace

