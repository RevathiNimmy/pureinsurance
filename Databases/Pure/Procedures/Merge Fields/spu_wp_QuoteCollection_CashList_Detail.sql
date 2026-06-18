SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON  SET NOCOUNT ON
GO

EXECUTE DDLDropProcedure 'spu_wp_QuoteCollection_CashList_Detail'

GO

CREATE PROCEDURE spu_wp_QuoteCollection_CashList_Detail
    @InsuranceFileCnt INT

AS

    SELECT cl.CashList_Ref, cli.media_ref, cli.mediatype_id
    FROM cashlist cl
	Join cashlistitem cli ON cl.cashlist_id=cli.cashlist_id
	Join Insurance_File_Payment_Details ifpd ON ifpd.cashlistitem_id=cli.cashlistitem_id
    WHERE  ifpd.insurance_file_cnt = @InsuranceFileCnt
  
GO

SET NOCOUNT OFF

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
