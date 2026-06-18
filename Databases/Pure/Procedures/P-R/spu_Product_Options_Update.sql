SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Product_Options_Update'
GO

CREATE PROCEDURE spu_Product_Options_Update
    @option_number int,
    @branch_id int,
    @value varchar(20)
--****************************************************************************
-- Revision     Description of Modification         Date        Who 
-- --------     ---------------------------         ----------  --- 
-- 1.0          Created                             16/12/2004  AG
-- 1.1          Inserted the record if not exists   16/02/2005  MKR
--****************************************************************************

AS

BEGIN
    IF EXISTS (SELECT option_number FROM hidden_options WHERE option_number = @option_number AND branch_id = @branch_id)
        BEGIN
            UPDATE hidden_options 
            SET value = @value
            WHERE branch_id = @branch_id
            AND option_number = @option_number
        END
    ELSE
        BEGIN
            INSERT INTO hidden_options (branch_id, option_number, Value) VALUES (@branch_id, @option_number, @value)
        END
END
 
GO
