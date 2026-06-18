SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Risk_Selection_Status_Update'
GO


CREATE PROCEDURE spu_SIR_Risk_Selection_Status_Update

	@risk_cnt int,
	@is_risk_selected int

AS

BEGIN
	If ISNULL(@is_risk_selected,0) = -1 
		SELECT @is_risk_selected = is_risk_selected FROM risk WHERE risk_cnt = @risk_cnt

	DECLARE @risk_folder_cnt int
	DECLARE @risk_number int
        DECLARE @original_risk_cnt int
    DECLARE @insurance_file_cnt int
        Select @original_risk_cnt=ifrl.original_risk_cnt,@insurance_file_cnt=insurance_file_cnt from insurance_file_risk_link ifrl
        WHERE risk_cnt=@risk_cnt 
		
        IF ISNULL(@original_risk_cnt,0)=0
        BEGIN 
	-- if we are deselecting an item then do just that	
	IF ISNULL(@is_risk_selected,0) = 0 
		BEGIN
			UPDATE risk 
			SET is_risk_selected = NULL
			WHERE risk_cnt = @risk_cnt

		END 
	-- if we are selecting an item
	ELSE		
		BEGIN
			-- get the risk to be selected details
			SELECT @risk_folder_cnt = risk_folder_cnt,
			       @risk_number = risk_number
			FROM risk
			WHERE risk_cnt = @risk_cnt 

			-- deselect any variations of this risk
			UPDATE risk 
			SET is_risk_selected = NULL 
			FROM risk INNER JOIN insurance_file_risk_link ifrl 
			ON ifrl.risk_cnt=risk.risk_cnt

			WHERE risk_folder_cnt = @risk_folder_cnt
			AND risk_number = @risk_number			
   			AND risk.risk_cnt not in (Select ifrl.risk_cnt from insurance_file_risk_link ifrl 
						inner join risk on ifrl.risk_cnt=risk.risk_cnt
						where risk_folder_cnt = @risk_folder_cnt
						And ifrl.status_flag = 'D')
			AND risk.risk_cnt in (Select ifrl.risk_cnt from insurance_file_risk_link ifrl  
						inner join risk on ifrl.risk_cnt=risk.risk_cnt 
						where ifrl.insurance_file_cnt=@insurance_file_cnt)

			-- set the specified risk to selected
			UPDATE risk 
			SET is_risk_selected = @is_risk_selected
			WHERE risk_cnt = @risk_cnt
		END 
	END 
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
