SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropView 'MediaType_Receipt'
go

--Start (Prakash C Varghese) - (Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc) - (6.1.2)
CREATE VIEW MediaType_Receipt
AS

    SELECT *
    FROM    MediaType
    WHERE   Is_Receipt=1

--End (Prakash C Varghese) - (Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc) - (6.1.2)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

