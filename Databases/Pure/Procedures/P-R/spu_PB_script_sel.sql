SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PB_script_sel'
GO
/********************************************************************************************************/
/* Returns a specified known text field as a column named "xml_data" or else an empty record set        */
/********************************************************************************************************/
/* Revision Description of Modification Date        Who                                                 */
/* -------- --------------------------- ----        ---                                                 */
/* 1.0      Original                    22/02/2001  CLG                                                 */
/********************************************************************************************************/
CREATE PROCEDURE spu_PB_script_sel
    @key integer,
    @required_column varchar(50)
AS
    DECLARE @iDataSize integer

    /* need to check if the field is empty as GetTextField in dPMDAO is not very forgiving */
    IF @required_column = 'script_dynamic_logic'
        select @iDataSize = datalength(script_dynamic_logic) from gis_screen where GIS_screen_id = @key
    ELSE IF @required_column = 'script_defaults'
        select @iDataSize = datalength(script_defaults) from gis_screen where GIS_screen_id = @key
    ELSE IF @required_column = 'script_quote'
        select @iDataSize = datalength(script_quote) from risk_type_rule_set where risk_type_rule_set_id = @key
    ELSE IF @required_column = 'script_ual'
        select @iDataSize = datalength(script_ual) from rule_set where rule_set_id = @key
    ELSE
        select @iDataSize = 1

    /* if no data or unknown column then return an empty record set */
    IF @iDataSize < 1
        return

    IF @required_column = 'script_dynamic_logic'
        select xml_data = script_dynamic_logic from gis_screen where GIS_screen_id = @key
    ELSE IF @required_column = 'script_defaults'
        select xml_data = script_defaults from gis_screen where GIS_screen_id = @key
    ELSE IF @required_column = 'script_quote'
        select xml_data = script_quote from risk_type_rule_set where risk_type_rule_set_id = @key
    ELSE IF @required_column = 'script_ual'
        select xml_data = script_ual from rule_set where rule_set_id = @key

GO

