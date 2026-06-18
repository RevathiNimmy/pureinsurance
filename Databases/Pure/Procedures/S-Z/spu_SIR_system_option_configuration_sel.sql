SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIR_system_option_configuration_sel'
GO

CREATE PROCEDURE spu_SIR_system_option_configuration_sel

AS

    SELECT
        option_number,
        system_option_configuration_group_id,
        control_type,
        control_top,
        control_height,
        control_left,
        control_width,
        control_caption,
        command,
        mandatory_or_optional,
        tab_index,
        command_parameters,
		parent_name,
		control_name
    FROM system_option_configuration
    ORDER BY system_option_configuration_group_id, tab_index

GO

