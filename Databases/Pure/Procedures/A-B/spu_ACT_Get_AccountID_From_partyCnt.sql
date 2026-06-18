SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Get_AccountID_From_partyCnt'
GO


CREATE PROCEDURE spu_ACT_Get_AccountID_From_partyCnt
    @companyid integer,
    @PartyCnt int,
    @AccountID int OUTPUT,
    @SubBranchID int OUTPUT
AS

-- PN6169 Get the Product Option for multi-branch core accounts
 
DECLARE @Value VARCHAR(20)
SELECT  @Value = Value
FROM    Hidden_options
WHERE   option_number = 16	 

IF ISNULL(@Value, 0) = 0
BEGIN
    -- Unique short_codes, select from all
	SELECT @AccountID = account_id,
       		@SubBranchID = sub_branch_id
	FROM   account
	WHERE  account_key = @PartyCnt
END
ELSE
BEGIN
	SELECT @AccountID = account_id,
       		@SubBranchID = sub_branch_id
	FROM   account
	WHERE  account_key = @PartyCnt
	AND company_id = @companyid

END
GO


