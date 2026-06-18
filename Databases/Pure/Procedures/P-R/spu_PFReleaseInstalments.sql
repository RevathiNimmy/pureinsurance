SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_PFReleaseInstalments'
GO


CREATE PROCEDURE spu_PFReleaseInstalments
    @pfprem_finance_cnt INT,
    @pfprem_finance_Version INT
AS BEGIN
    UPDATE PFInstalments
        SET Status = 1,			-- New
		batch_id=NULL
    WHERE 
        pfprem_finance_cnt = @pfprem_finance_cnt
    AND pfprem_finance_Version = @pfprem_finance_Version
    AND status = 7				-- Hold
END
GO
