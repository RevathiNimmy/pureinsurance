SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Product_options_get'
GO

CREATE PROCEDURE spe_Product_options_get
    @find_option int,
    @branch_id int
AS
BEGIN

IF @find_option = 1 
    SELECT option_number,branch_id, value, UW_type, ' ' as name, ' ' as description 
    	FROM hidden_options WHERE option_number > 1 
	ORDER BY option_number, branch_id
else if @find_option = 2  
    SELECT option_number,branch_id, value, UW_type, ' ' as name, ' ' as description 
    	FROM hidden_options WHERE branch_id = @branch_id and option_number > 1
	ORDER BY option_number, branch_id
else
    SELECT option_number,branch_id, value, UW_type, ' ' as name, ' ' as description 
    	FROM hidden_options 
	ORDER BY option_number, branch_id
	


END

GO

