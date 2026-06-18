
DDLDropProcedure 'spu_ACT_Update_TransDetailOutstanding'
GO

CREATE PROCEDURE spu_ACT_Update_TransDetailOutstanding
(
    	@transdetail_id INT,
    	@currency_amount_diff MONEY,
    	@base_amount_diff MONEY,
    	@account_amount_diff MONEY,
		@system_amount_diff MONEY,
		@transdetailex_id INT = 0
)

AS
BEGIN

	CREATE TABLE #transdetailex
	(
		ID INT IDENTITY(1,1),
		transdetailex_id INT
	)
	INSERT INTO #transdetailex
	(transdetailex_id)
	SELECT transdetailex_id  
	FROM TransDetailEx 
	WHERE transdetail_id = @transdetail_id
	AND outstanding_currency_amount <> 0
	ORDER BY portion_no ASC


	DECLARE @iStart INT = 1
	DECLARE @iCount INT

	SELECT @iCount = COUNT(*) from #transdetailex

    IF
 	(
    	SELECT outstanding_amount - Round(@base_amount_diff,2)
    	FROM TransDetail
    	WHERE transdetail_id = @transdetail_id
    ) = 0
    BEGIN

    	/*If base amount is fully matched then set all outstanding amounts to zero*/
    	UPDATE TransDetail
		SET	outstanding_amount = 0,
			outstanding_currency_amount = 0,
			outstanding_account_amount = 0,
			outstanding_system_amount = 0,
			amount_updated=GetDate()
		WHERE transdetail_id = @transdetail_id

		IF Exists(SELECT 1 FROM  TransDetailEx WHERE transdetail_id = @transdetail_id)
		BEGIN
    			UPDATE TransDetailEx
					SET outstanding_currency_amount = 0,
						outstanding_account_amount = 0,
						outstanding_system_amount = 0
				WHERE transdetail_id = @transdetail_id
		END

    END
    ELSE
    BEGIN

    	IF
		(
			SELECT amount - (outstanding_amount - @base_amount_diff)
			FROM TransDetail
			WHERE transdetail_id = @transdetail_id
		) = 0
		BEGIN

			/*If base amount is fully outstanding then set all outstanding amounts to their original values*/
			UPDATE TransDetail
			SET	outstanding_amount = amount,
				outstanding_currency_amount = currency_amount,
				outstanding_account_amount = account_amount,
				outstanding_system_amount = system_amount,
				amount_updated=GetDate()
			WHERE transdetail_id = @transdetail_id

			IF Exists(SELECT 1 FROM  TransDetailEx WHERE transdetail_id = @transdetail_id)
			BEGIN
			
    				UPDATE tdx
						SET tdx.outstanding_currency_amount = tdx.currency_amount,
							tdx.outstanding_account_amount = tdx.account_amount,
							tdx.outstanding_system_amount = tdx.system_amount
					FROM TransDetailEx tdx
					INNER JOIN TransDetailEx tdx1
					ON tdx.transdetailex_id = tdx1.transdetailex_id
					WHERE tdx.transdetail_id = @transdetail_id
			END
		END
		ELSE
		BEGIN

    		/*Otherwise, change the outstanding amounts by the passed in amounts*/
			UPDATE TransDetail
			SET outstanding_amount = outstanding_amount - @base_amount_diff,
				outstanding_currency_amount = outstanding_currency_amount - @currency_amount_diff,
				outstanding_account_amount = outstanding_account_amount - @account_amount_diff,
				outstanding_system_amount = outstanding_system_amount - @system_amount_diff,
				amount_updated=GetDate()
			WHERE transdetail_id = @transdetail_id 

			DECLARE @outstanding_currency_amount Money
			DECLARE @outstanding_account_amount Money
			DECLARE @outstanding_system_amount Money
			
			 WHILE(@iStart < = @iCount)
			 BEGIN 
				SELECT @transdetailex_id = transdetailex_id from #transdetailex WHERE ID = @iStart
				IF @transdetailex_id <> 0
				BEGIN
				SELECT @outstanding_currency_amount = outstanding_currency_amount, @outstanding_account_amount = outstanding_account_amount,
				 @outstanding_system_amount=outstanding_system_amount FROM TransDetailEx where transdetailex_id = @transdetailex_id

				 -- If @currency_amount_diff values is positive
				 IF @currency_amount_diff > 0
				 BEGIN
					IF (@outstanding_currency_amount - @currency_amount_diff) < 0
					BEGIN
						UPDATE TransDetailEx
						SET outstanding_currency_amount = outstanding_currency_amount - @outstanding_currency_amount,
							outstanding_account_amount = outstanding_account_amount - @outstanding_account_amount,
							outstanding_system_amount = outstanding_system_amount - @outstanding_system_amount
						WHERE transdetailex_id = @transdetailex_id

						SET @currency_amount_diff = @currency_amount_diff - @outstanding_currency_amount
						SET @account_amount_diff = @account_amount_diff - @outstanding_account_amount
						SET @system_amount_diff = @system_amount_diff - @outstanding_system_amount
					END
					ELSE
					BEGIN
						IF @currency_amount_diff <> 0
						BEGIN
						UPDATE TransDetailEx
							SET outstanding_currency_amount = outstanding_currency_amount - @currency_amount_diff,
								outstanding_account_amount = outstanding_account_amount - @account_amount_diff,
								outstanding_system_amount = outstanding_system_amount - @system_amount_diff
							WHERE transdetailex_id = @transdetailex_id
						END
						BREAK;
					END
				END
					 -- If @currency_amount_diff values is Negative
				IF @currency_amount_diff < 0 
				BEGIN
					IF (@outstanding_currency_amount - @currency_amount_diff) > 0
					BEGIN
						UPDATE TransDetailEx
						SET outstanding_currency_amount = outstanding_currency_amount - @outstanding_currency_amount,
							outstanding_account_amount = outstanding_account_amount - @outstanding_account_amount,
							outstanding_system_amount = outstanding_system_amount - @outstanding_system_amount
						WHERE transdetailex_id = @transdetailex_id

						SET @currency_amount_diff = @currency_amount_diff - @outstanding_currency_amount
						SET @account_amount_diff = @account_amount_diff - @outstanding_account_amount
						SET @system_amount_diff = @system_amount_diff - @outstanding_system_amount
					END
					ELSE
					BEGIN
						IF @currency_amount_diff <> 0
						BEGIN
						UPDATE TransDetailEx
							SET outstanding_currency_amount = outstanding_currency_amount - @currency_amount_diff,
								outstanding_account_amount = outstanding_account_amount - @account_amount_diff,
								outstanding_system_amount = outstanding_system_amount - @system_amount_diff
							WHERE transdetailex_id = @transdetailex_id
						END
						BREAK;
					END
				END
				END
				SET @iStart = @iStart + 1
			END

		END
    END
END
