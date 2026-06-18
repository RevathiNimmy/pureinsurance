SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_policy_shared_premiums_add'
GO
 
CREATE PROCEDURE spe_policy_shared_premiums_add
		@insurance_file_cnt 	int,
		@party_cnt 		int,
		@split_value 		numeric(19,4),
		@split_percentage 	numeric(7,4)

AS
BEGIN
	INSERT INTO policy_shared_premiums
	      (
	      insurance_file_cnt,
	      party_cnt,
	      split_value,
	      split_percentage
	      ) 
	 VALUES
	      (
	      @insurance_file_cnt,
	      @party_cnt,
	      @split_value,
	      @split_percentage	
	      )
END
GO
