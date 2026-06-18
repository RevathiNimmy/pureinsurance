<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TransactionConfirmation.ascx.vb"
    Inherits="Nexus.SummaryCoverCntrl" %>
<%@ Register Src="~/Controls/Document.ascx" TagName="Document" TagPrefix="uc4" %>

<p>
    You have added the following valuable items to your cover:
</p>
<p>
    <asp:Literal ID="lblDocumentsText" runat="server" Text="You may view and print the following documents by clicking on the links below:"
        Visible="false" />
</p>
<p>
    <asp:Literal ID="Literal1" runat="server" Text="You may view and print the following documents by clicking on the links below:"
        Visible="false" />
</p>

<uc4:Document ID="liScheduledocument" runat="server" DocumentName="Schedule" PreGenerate="false"
            Visible="false" Text="Schedule" />

<uc4:Document ID="liMTAScheduledocument" runat="server" DocumentName="MTASchedule"
            PreGenerate="false" Visible="false" Text="MTASchedule" />

<uc4:Document ID="liMTCScheduledocument" runat="server" DocumentName="MTCSchedule"
            PreGenerate="false" Visible="false" Text="MTCSchedule" />

<uc4:Document ID="liReceiptdocument" runat="server" DocumentName="Receipt" PreGenerate="false"
            Visible="false" Text="Produce Receipt" />

<uc4:Document ID="liCertificate" runat="server" DocumentName="Certificate"
            PreGenerate="false" Visible="false" Text="Certificate" />

<uc4:Document ID="liDebitNote" runat="server" DocumentName="DebitNote"
            PreGenerate="false" Visible="false" Text="DebitNote" />


<uc4:Document ID="liAdviceNote" runat="server" DocumentName="AdviceNote"
            PreGenerate="false" Visible="false" Text="AdviceNote" />
