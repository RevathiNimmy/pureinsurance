SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropView 'MediaType_Payment'
go

--Start (Prakash C Varghese) - (Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc) - (6.1.1)
CREATE VIEW MediaType_Payment
AS

    SELECT *
    FROM    MediaType
    WHERE   Is_Payment=1

--End (Prakash C Varghese) - (Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc) - (6.1.1)

GO

