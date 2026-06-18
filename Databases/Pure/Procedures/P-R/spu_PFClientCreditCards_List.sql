SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_PFClientCreditCards_List'
GO


CREATE PROCEDURE spu_PFClientCreditCards_List
    @ClientID int
AS BEGIN
    SELECT DISTINCT BankAccountName,cc_number,cc_expiry_date,cc_start_date,cc_issue,cc_pin
    FROM pfPremiumFinance
    WHERE ClientID = @ClientID
    AND cc_number IS NOT NULL
END
GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
