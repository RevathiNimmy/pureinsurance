if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spu_PFChangeCommisionTransID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_PFChangeCommisionTransID]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROC spu_PFChangeCommisionTransID
    @PremiumFinanceCnt int,
    @PremiumFinanceVersion int,
    @CommissionAccountID int,
    @DocumentRef VARCHAR(20),
    @DocSeq SMALLINT
AS BEGIN
    DECLARE @CommissionTransDetailID int
    SELECT @CommissionTransDetailID = TransDetail_ID
    FROM TransDetail t, document d
    WHERE t.document_sequence = @DocSeq
    AND d.document_id = t.document_id
    AND d.document_ref = @DocumentRef
    AND t.Account_id = @CommissionAccountID
    UPDATE pfPremiumFinance
    SET commission_transdetail_id = @CommissionTransDetailID
    WHERE
        pfprem_finance_cnt = @PremiumFinanceCnt
    AND pfprem_finance_version = @PremiumFinanceVersion
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

