SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropTrigger 'tD_PFRF'
GO
CREATE TRIGGER tD_PFRF ON PFRF FOR DELETE
AS
BEGIN
    DECLARE @RECORDS INTEGER

    SELECT @RECORDS = COUNT(PFPremiumFinance.pfprem_finance_cnt)
        FROM PFPremiumFinance, deleted
        WHERE PFPremiumFinance.CompanyNo = deleted.CompanyNo
        AND PFPremiumFinance.SchemeNo = deleted.SchemeNo
        AND PFPremiumFinance.SchemeVersion = deleted.SchemeVersion
        AND PFPremiumFinance.StartDate BETWEEN deleted.StartDate and deleted.EndDate

    IF @RECORDS > 0 BEGIN
        ROLLBACK WORK
        RAISERROR ('Cannot delete from PFRF. Related records in PFPremiumFinance.', 1, 1) WITH NOWAIT
    END
END
GO

