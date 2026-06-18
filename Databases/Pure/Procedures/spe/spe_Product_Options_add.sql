SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Product_Options_add'
GO

CREATE PROCEDURE spe_Product_Options_add
    @option_number int,
    @branch_id int,
    @value varchar(20),
    @uw_type varchar(10)
AS
BEGIN
INSERT INTO hidden_options (
    option_number ,
    branch_id ,
    value ,
    uw_type )
VALUES (
    @option_number,
    @branch_id,
    @value,
    @uw_type)
END

GO

