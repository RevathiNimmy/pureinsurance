<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="True" CodeFile="Dashboard.aspx.vb" Inherits="Nexus.Modal_Dashboard" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="server">
    <div id="Modal_Dashboard">
        <div class="card" style="background-color: #f5f5f5;">
            <div class="card-body clearfix">
                <div class="row">
                    <div class="col-xl-6">
                        <div class="card">
                            <div class="card-body">
                                <span class="float-start m-2 me-4">
                                    <img id="imgUserThumnail" style="height: 100px;" alt="" class="rounded-circle img-thumbnail" runat="server">
                                </span>
                                <div class="">
                                    <h2 class="font-weight-normal pt-2 mb-1">
                                        <asp:Label ID="lblName_view" runat="server"></asp:Label></h2>
                                    <p class="font-18">
                                        <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                    </p>
                                </div>
                                <div>
                                    <ul class="mb-1 list-inline">
                                        <li class="list-inline-item me-4">
                                            <h5 class="mb-1">
                                                <asp:Label ID="lblNoofPolicies" runat="server" /></h5>
                                            <p class="mb-0 font-15"><%= GetLocalResourceObject("lblNoofPolicies") %></p>
                                        </li>
                                        <li class="list-inline-item me-4">
                                            <h5 class="mb-1">
                                                <asp:Label ID="lblNoofOpenClaims" runat="server" /></h5>
                                            <p class="mb-0 font-15"><%= GetLocalResourceObject("lblNoofOpenClaims") %></p>
                                        </li>
                                        <li class="list-inline-item">
                                            <h5 class="mb-1">
                                                <asp:Label ID="lblNoofClosedClaims" runat="server" /></h5>
                                            <p class="mb-0 font-15"><%= GetLocalResourceObject("lblNoofClosedClaims") %></p>
                                        </li>
                                    </ul>
                                </div>
                                <!-- end div-->
                            </div>
                            <!-- end card-body-->
                        </div>
                    </div>
                    <!-- end col -->
                    <div class="col-xl-3">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="header-title mt-0 mb-2 fs-4"><%= GetLocalResourceObject("lblTotalValue") %></h4>
                                <div class="widget-chart-1">
                                    <div class="widget-chart-box-1 float-left" dir="ltr">
                                        <asp:Label ID="lblThisYearPremium" runat="server" Text="0"></asp:Label>
                                        <p class="text-muted mb-1"><%= GetLocalResourceObject("lblThisYear") %></p>
                                    </div>
                                    <div class="widget-detail-1 text-right">
                                        <h2 class="font-24 mb-1">
                                            <asp:Label ID="lblTotalPremium" runat="server" Text="0"></asp:Label></h2>
                                        <p class="text-muted mb-1"><%= GetLocalResourceObject("lblTotalPremium") %></p>
                                        <p class="mb-0 text-muted">
                                            <span id="spanPremium" runat="server">
                                                <i id="iPremiumPercent" runat="server"></i>
                                                <asp:Label ID="lblPremiumPercent" runat="server"></asp:Label></span>
                                            <span class="text-nowrap"><%= GetLocalResourceObject("lblLastYear") %></span>
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-3">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="header-title mt-0 mb-2 fs-4"><%= GetLocalResourceObject("lblClaimIncurred") %></h4>
                                <div class="widget-chart-1">
                                    <div class="widget-chart-box-1 float-left" dir="ltr">
                                        <asp:Label ID="lblClaimIncurred" runat="server" Text="0"></asp:Label>
                                        <p class="text-muted mb-1"><%= GetLocalResourceObject("lblThisYear") %></p>
                                    </div>
                                    <div class="widget-detail-1 text-right">
                                        <h2 class="font-24 mb-1">
                                            <asp:Label ID="lblTotalClaimIncurred" runat="server" Text="0"></asp:Label></h2>
                                        <p class="text-muted mb-1"><%= GetLocalResourceObject("lblTotalClaimIncurred") %></p>
                                        <p class="mb-0 text-muted">
                                            <span id="spanClaimIncurred" runat="server">
                                                <i id="iClaimIncurred" runat="server"></i>
                                                <asp:Label ID="lblClaimIncurredPercent" runat="server"></asp:Label></span>
                                            <span class="text-nowrap"><%= GetLocalResourceObject("lblLastYear") %></span>
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xl-4 col-md-6">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="header-title mt-0 mb-3 fs-4"><%= GetLocalResourceObject("lblClaimLastRatio") %></h4>
                                <h3 class="mt-3 mb-1">
                                    <asp:Label ID="lblClaimLossRatio" runat="server" Text="0%"></asp:Label></h3>
                                <p class="mb-0 text-muted">
                                    <span id="spanClaimLossRatio" runat="server">
                                        <i id="iClaimLossRatio" runat="server" ></i>
                                        <asp:Label ID="lblClmLossRatioPercent" runat="server"></asp:Label>

                                    </span>
                                    <span class="text-nowrap"><%= GetLocalResourceObject("lblLastYear") %></span>
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-4 col-md-6">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="header-title mt-0 mb-3 fs-4"><%= GetLocalResourceObject("lblPremiumDue") %></h4>
                                <h3 class="mt-3 mb-3">
                                    <asp:Label ID="lblPremiumDue" runat="server" Text="0"></asp:Label>
                                </h3>
                            </div>
                        </div>

                    </div>
                    <div class="col-xl-4 col-md-6">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="header-title mt-0 mb-3 fs-4"><%= GetLocalResourceObject("lblClaimOutstanding") %></h4>
                                <h3 class="mt-3 mb-3">
                                    <asp:Label ID="lblClaimOutstanding" runat="server" Text="0"></asp:Label>
                                </h3>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="row">
                    <div class="col-xl-4 col-md-6">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="header-title mt-0 mb-3 fs-4"><%= GetLocalResourceObject("lblPremiumRiskType") %></h4>
                                <div class="mt-3 mb-2" style="height: 320px;">
                                    <canvas id="donut-chart-example" data-colors="#727cf5,#fa5c7c,#0acf97,#ebeff2,#FF6384,#36A2EB"></canvas>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-8">
                        <div class="card">
                            <div class="card-body">
                                <h4 class="header-title mb-4 fs-4"><%= GetLocalResourceObject("lblWriitenPremiumv/sClaimIncurred") %></h4>

                                <div dir="ltr">
                                    <div class="mt-3 chartjs-chart" style="height: 320px;">
                                        <canvas id="bar-chart-example" data-colors="#fa5c7c,#727cf5"></canvas>
                                    </div>
                                </div>

                            </div>
                            <!-- end card body-->
                        </div>
                        <!-- end card -->
                    </div>
                    <!-- end col-->

                </div>
                <div class="row">
                    <div class="col-xl-6 col-lg-12">
                        <div class="card">
                            <div class="card-body">
                                <div class="d-flex justify-content-between align-items-center mb-2">
                                    <h4 class="header-title fs-4"><%= GetLocalResourceObject("lblRecentPolicies") %></h4>
                                </div>

                                <div class="table-responsive">
                                    <asp:GridView ID="grdvPolicies" runat="server" AutoGenerateColumns="False" EmptyDataRowStyle-CssClass="noData" EmptyDataText="No Quotes And Policy Found" OnRowDataBound="grdvPolicies_RowDataBound" CssClass="table table-striped table-sm table-nowrap table-centered mb-0">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Policy ID" ItemStyle-Width="50%">
                                                <ItemTemplate>
                                                    <h5 class="font-15 mb-1 fw-normal">
                                                        <asp:Literal ID="ltPolicyID" runat="server"></asp:Literal>
                                                    </h5>
                                                    <span class="text-muted font-13">
                                                        <asp:Literal ID="ltProduct" runat="server"></asp:Literal></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Renewal Date">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltRenewalDate" runat="server"></asp:Literal>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <h5><span id="spanStatus" runat="server"><i class="mdi mdi-bitcoin"></i>
                                                        <asp:Literal ID="ltStatus" runat="server"></asp:Literal></span></h5>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Premium">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltPremium" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                </div>
                                <!-- end table-responsive-->

                            </div>
                            <!-- end card-body-->
                        </div>
                        <!-- end card-->
                    </div>
                    <!-- end col-->

                    <div class="col-xl-6 col-lg-6">
                        <div class="card">
                            <div class="card-body">
                                <div class="d-flex justify-content-between align-items-center mb-2">
                                    <h4 class="header-title fs-4"><%= GetLocalResourceObject("lblRecentTransactions") %></h4>
                                </div>

                                <div class="table-responsive">
                                    <asp:GridView ID="grdvTransactions" runat="server" AutoGenerateColumns="False" EmptyDataRowStyle-CssClass="noData" EmptyDataText="No Transactions Found" OnRowDataBound="grdvTransactions_RowDataBound" CssClass="table table-striped table-sm table-nowrap table-centered mb-0">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Doc.Ref" ItemStyle-Width="50%">
                                                <ItemTemplate>
                                                    <h5 class="font-15 mb-1 fw-normal">
                                                        <asp:Literal ID="ltDocRef" runat="server"></asp:Literal>
                                                    </h5>
                                                    <span class="text-muted font-13">
                                                        <asp:Literal ID="ltDocRefDecs" runat="server"></asp:Literal>

                                                        <asp:Literal ID="ltMediaRef" runat="server"></asp:Literal></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transactions Date">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltTransactionsDate" runat="server"></asp:Literal>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount">
                                                <ItemTemplate>
                                                    <h5><span id="spanAmount" runat="server">
                                                        <asp:Literal ID="ltAmount" runat="server"></asp:Literal></span></h5>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>

                                </div>

                            </div>
                            <!-- end card-body -->
                        </div>
                        <!-- end card-->
                    </div>
                    <!-- end col -->

                </div>
            </div>
        </div>
    </div>
</asp:Content>


