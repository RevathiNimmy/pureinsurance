/* IPT */
EXECUTE DDLDropProcedure 'spu_cnc_get_ipt'
GO

CREATE PROCEDURE spu_cnc_get_ipt
    @insurance_file_cnt INT
AS
BEGIN


	DECLARE @risk_code_id INT
	DECLARE @effective_date DATETIME

	SELECT @effective_date = cover_start_date, @risk_code_id = risk_code_id
	    FROM insurance_file 
	    WHERE insurance_file_cnt = @insurance_file_cnt

	SELECT rate
	FROM ipt 
	WHERE risk_code_id = @risk_code_id
	--AND effective_date <=@effective_date
	AND effective_date = 
	    (SELECT MAX(effective_date) 
	     FROM ipt
	     WHERE risk_code_id = @risk_code_id
	     AND effective_date <=@effective_date) 

END
GO