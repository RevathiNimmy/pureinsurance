EXEC DDLDropProcedure 'spu_ACT_Update_PolicyCashListItem'
GO

CREATE PROCEDURE spu_ACT_Update_PolicyCashListItem
	@insurance_file_cnt INT,
	@cashlistitem_id	INT

AS

DECLARE @old_cashlistitem_id INT
DECLARE @old_cashlist_id INT
DECLARE @old_transdetail_id INT

SELECT @old_cashlistitem_id=NULL

SELECT
	@old_cashlistitem_id=CLI.cashlistitem_id,
	@old_cashlist_id=CLI.cashlist_id,
	@old_transdetail_id=CLI.transdetail_id
FROM
	CashListItem CLI
INNER JOIN
	Insurance_File IFF ON CLI.cashlistitem_id=IFF.cashlistitem_id
WHERE IFF.insurance_file_cnt=@insurance_file_cnt

IF NOT @old_cashlistitem_id IS NULL BEGIN
	IF @old_cashlistitem_id<>@cashlistitem_id AND ISNULL(@old_transdetail_id,0)=0 BEGIN	
		-- Remove unposted receipt/payment
		DELETE CashListItem 
		WHERE cashlistitem_id=@old_cashlistitem_id

		-- Remove the cashlist if there are no other related cashlistitem records
		DELETE CashList
		WHERE cashlist_id=@old_cashlist_id
		AND NOT EXISTS (SELECT cashlistitem_id FROM CashListItem WHERE cashlist_id=@old_cashlist_id)

		-- Reset information on Insurance_File
		IF @cashlistitem_id=0 BEGIN
			UPDATE Insurance_File
			SET cashlistitem_id=NULL,
				cashlistitem_valid=0
			WHERE insurance_file_cnt=@insurance_file_cnt
		END
	END
END

-- Point the Insurance File at the new one
IF @cashlistitem_id<>0 BEGIN
	UPDATE Insurance_File
	SET cashlistitem_id=@cashlistitem_id,
		cashlistitem_valid=1
	WHERE insurance_file_cnt=@insurance_file_cnt
END

GO