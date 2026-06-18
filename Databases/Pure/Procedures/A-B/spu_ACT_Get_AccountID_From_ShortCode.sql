SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_AccountID_From_ShortCode'
GO


CREATE PROCEDURE spu_ACT_Get_AccountID_From_ShortCode
    @company_id integer,
    @sub_branch_id integer=NULL,
    @ShortCode varchar(20),
    @AccountID integer OUTPUT,
	@delete_at_purge TINYINT=0 OUTPUT
AS

-- PWF 04/10/2002
-- Get the Product Option for multi-branch core accounts
-- PN6169 Product Option is 16 not 20
DECLARE @Value VARCHAR(20)
SELECT  @Value = Value
FROM    Hidden_options
WHERE   option_number = 16	--PN6169


IF ISNULL(@Value, 0) = 0
BEGIN
    -- Unique short_codes, select from all
    SELECT  @AccountID = account_id,
			@delete_at_purge = delete_at_purge
    FROM    Account
    WHERE   short_code = @ShortCode
END ELSE BEGIN
    -- Multi-branch core accounts, select from company
    SELECT  @AccountID = account_id,
			@delete_at_purge = delete_at_purge
    FROM    Account
    WHERE   short_code = @ShortCode
    AND     company_id=@company_id
END

GO


