SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Get_MediaType_By_ID'
GO

--Start (Prakash C Varghese) - (Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc) - (6.2.5)
CREATE  PROCEDURE spu_SAM_Get_MediaType_By_ID 

@MediaType_ID int

AS
BEGIN
    SELECT *
    FROM   MediaType
    WHERE  MediaType_Id = @MediaType_ID
END
--End (Prakash C Varghese) - (Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc) - (6.2.5)
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO