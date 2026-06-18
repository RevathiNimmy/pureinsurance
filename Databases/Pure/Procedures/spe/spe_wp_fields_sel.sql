SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_wp_fields_sel'
GO

CREATE PROCEDURE spe_wp_fields_sel
    @field_name VARCHAR(255)
AS

SELECT
    field_name,
    sql,
    column_name,
    column_type,
    main_group,
    sub_group,
    display_name,
    is_displayed,
    loop1,
    loop2,
    loop3,
    product_family,
    data_model,
    property_id,
    sub_group2,
    sub_group3,
    specials_type
FROM wp_fields
WHERE field_name = @field_name

GO

