SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Product_Options_del'
GO

CREATE PROCEDURE spe_Product_Options_del
    @option_number int,
    @branch_id int,
    @delete_Option int
AS
BEGIN

if @delete_option = 1
	DELETE FROM hidden_options WHERE option_number > 1
else if @delete_option = 2
	DELETE FROM hidden_options WHERE option_number = @option_number 
	and branch_id = branch_id
else if @delete_option = 3
	DELETE FROM hidden_options WHERE option_number = @option_number 

END



GO

