SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_add_stats_details_control'

GO

CREATE PROCEDURE spu_add_stats_details_control  
    @stats_folder_cnt int  ,  
    @only_ri INT =0  
AS
BEGIN
/******************************************************************************************/
/* spu_add_stats_details_control is the controlling procedure for creating the             */
/* Stats_Detail table.                                    */
/*                                                                                        */
/* 1 parameter is passed in - @stats_folder_cnt                       */
/*                                                                                        */
/* The stored procedures for all stats details methods are called in sequence.            */
/*                                                                                        */
/* A failure on any of the methods will result in the deletion of all stats details for   */
/* the given stats folder;  the stats folder records for the given policy will then be    */
/* deleted.                                                                               */
/******************************************************************************************/
/* Revision Description of Modification         Date        Who       */
/* --------     ---------------------------         ----        ---       */
/* 1.0      Original                    25/06/1997  TF    */
/*                                            */
/* 1.5      Database model adjusted                           */
/*      source_id removed from primary keys     15/07/1997  TF    */
/* 2.0          Only necessary SPs are called for RSA system    26/09/2000      SR    */
	/******************************************************************************************/
	DECLARE @return_status INT
	DECLARE @RI2007Enabled INT
	DECLARE @transtype INT
	DECLARE @FeeRecordExists INT = 0
	DECLARE @Insurance_File_CNT INT
        DECLARE @is_pt INT

	SELECT @Insurance_File_CNT = insurance_file_cnt
	FROM   stats_folder  with (nolock)
	WHERE  stats_folder_cnt = @stats_folder_cnt

	SELECT @RI2007Enabled = ISNULL(VALUE, 0)
	FROM   hidden_options  with (nolock)
	WHERE  option_number = 88 --RI2007Enabled
	--select @transtype= last_trans_type_id from Insurance_File_System where insurance_file_cnt = (select insurance_file_cnt from Stats_Folder where stats_folder_cnt=@stats_folder_cnt)
	SELECT @transtype = last_trans_type_id
	FROM   Insurance_File_System  with (nolock)
	WHERE  insurance_file_cnt = @Insurance_File_CNT

	IF EXISTS(SELECT 1
			  FROM   Policy_fee_u  with (nolock)
			  WHERE  insurance_file_cnt = @insurance_file_cnt
					 AND ISNULL(currency_amount, 0) <> 0)
	BEGIN
		SET @FeeRecordExists = 1
	END
	ELSE
	BEGIN
		IF EXISTS(SELECT 1
				  FROM   Tax_Calculation  with (nolock)
				  WHERE  insurance_file_cnt = @insurance_file_cnt
						 AND transtype = 'TTF')
			SET @FeeRecordExists = 1
	END

	SELECT @RI2007Enabled = ISNULL(VALUE, 0)
	FROM   hidden_options with (nolock)
	WHERE  option_number = 88 --RI2007Enabled
	--select @transtype= last_trans_type_id from Insurance_File_System where insurance_file_cnt = (select insurance_file_cnt from Stats_Folder where stats_folder_cnt=@stats_folder_cnt)
	SELECT @transtype = ISNULL(last_trans_type_id,0)
	FROM   Insurance_File_System with (nolock)
	WHERE  insurance_file_cnt = @Insurance_File_CNT

	IF EXISTS(SELECT 1
			  FROM   Policy_fee_u  with (nolock)
			  WHERE  insurance_file_cnt = @insurance_file_cnt
					 AND ISNULL(currency_amount, 0) <> 0)
	BEGIN
		SET @FeeRecordExists = 1
	END
	ELSE
	BEGIN
		IF EXISTS(SELECT 1
				  FROM   Tax_Calculation  with (nolock)
				  WHERE  insurance_file_cnt = @insurance_file_cnt
						 AND transtype = 'TTF')
			SET @FeeRecordExists = 1
	END

/* Create stats records for gross premium */
	--RWH (21/03/2001) This also handle coinsurer records.
	IF @only_ri=0 AND NOT (@TRANSTYPE IN ( 21 ) AND @RI2007Enabled=0)
	BEGIN
		EXECUTE @return_status = spu_add_stats_details_gross @stats_folder_cnt

		IF @return_status <> 0
			GOTO Err_Add_Stats_Details
	END

	IF @only_ri=1
		SET @is_pt =1
  	ELSE
		SET @is_pt = 0
		IF NOT (@TRANSTYPE IN ( 21 ) AND @RI2007Enabled=0)
	      BEGIN
			/* Create stats records for net retained */
		    EXECUTE @return_status = spu_add_stats_details_net @stats_folder_cnt, @is_pt
				IF @return_status <> 0
			GOTO Err_Add_Stats_Details
           END
	IF @only_ri=0 AND @FeeRecordExists = 1 AND NOT (@TRANSTYPE IN ( 21 ) AND @RI2007Enabled = 0)
	BEGIN
		/* Create stats records for Fees */
		EXECUTE @return_status = spu_add_stats_details_fee @stats_folder_cnt

		IF @return_status <> 0
			GOTO Err_Add_Stats_Details
	END

	IF @only_ri=0  AND NOT (@TRANSTYPE IN ( 21 ) AND @RI2007Enabled = 0)
	BEGIN
		/* Create stats records for sub-agent commission */
		EXECUTE @return_status = spu_add_stats_details_comm @stats_folder_cnt

		IF @return_status <> 0
			GOTO Err_Add_Stats_Details
	END

  IF @only_ri=1
	SET @is_pt =1
  ELSE
	SET @is_pt = 0
  EXECUTE
    @return_status = spu_add_stats_details_RI
                    @stats_folder_cnt ,@is_pt
	IF @return_status <> 0
		GOTO Err_Add_Stats_Details

IF @only_ri=0  AND NOT (@TRANSTYPE IN ( 21 ) AND @RI2007Enabled = 0)
	BEGIN
		/* Create stats records for tax amount */
		EXECUTE @return_status = spu_add_stats_details_tax @stats_folder_cnt

		IF @return_status <> 0
			GOTO Err_Add_Stats_Details
	END
IF @only_ri=0  AND NOT (@TRANSTYPE IN ( 21 ) AND @RI2007Enabled = 0)
	BEGIN
		/* Update the Earning Pattern Id for each stats detail entry based on the rating section */
		EXECUTE @return_status = spu_update_stats_earning_patterns @stats_folder_cnt

		IF @return_status <> 0
			GOTO Err_Add_Stats_Details
	END

	RETURN

	ERR_ADD_STATS_DETAILS:

	BEGIN
		/* Delete all stats details for this folder */
		DELETE FROM Stats_Detail
		WHERE  stats_folder_cnt = @stats_folder_cnt

		-- Delete the stats folder record
		DELETE FROM Stats_Folder
		WHERE  stats_folder_cnt = @stats_folder_cnt

		RETURN
	END
END
GO 
