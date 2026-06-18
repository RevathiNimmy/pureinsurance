SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
EXEC DDLDropProcedure 'spu_Get_Due_Date_For_Transactions'
GO

CREATE  PROCEDURE spu_Get_Due_Date_For_Transactions

	@insurance_file_cnt INTEGER,
	@account_id INTEGER
AS

BEGIN

DECLARE @due_date DATETIME
DECLARE @amount INT
DECLARE @period varchar(1)

SET @due_date = GETDATE()
SET @amount = 0

	SELECT @amount = PFF.amount,
		@period = PFF.period,
		@due_date = IFI.cover_start_date
	FROM INSURANCE_FILE IFI with (nolock)
	JOIN DOCUMENT DOC with (nolock) on DOC.insurance_file_cnt = IFI.insurance_file_cnt AND DOC.documenttype_id IN (4, 15, 17, 18, 52)
	JOIN ACCOUNT ACC with (nolock) ON ACC.account_id = @account_id
	JOIN PARTY PA with (nolock) ON PA.party_cnt = ACC.account_key
	JOIN PFFrequency PFF with (nolock) ON PA.payment_term_code = PFF.pffrequency_id	
	WHERE IFI.insurance_file_cnt = @insurance_file_cnt


	SET @due_date=CASE UPPER(@period) 
	WHEN 'D' THEN 
		DATEADD(DD, @amount, @due_date)		
	WHEN 'M' THEN
		DATEADD(MM, @amount, @due_date)
	WHEN 'W' THEN
		DATEADD(WW, @amount, @due_date)
	WHEN 'Y' THEN
		DATEADD(YY, @amount, @due_date)
	END
	
SELECT @due_date
		
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO