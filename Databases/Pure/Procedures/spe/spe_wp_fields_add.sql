SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_wp_fields_add'
GO

CREATE PROCEDURE spe_wp_fields_add
    @field_name VARCHAR(255),
    @sql VARCHAR(255),
    @column_name VARCHAR(255),
    @column_type INT,
    @main_group VARCHAR(255),
    @sub_group VARCHAR(255),
    @display_name VARCHAR(255),
    @is_displayed TINYINT,
    @loop1 VARCHAR(255),
    @loop2 VARCHAR(255),
    @loop3 VARCHAR(255),
    @loop4 VARCHAR(255) = NULL,
    @product_family INT,
    @data_model VARCHAR(20),
    @property_id INT,
    @sub_group2 VARCHAR(255),
    @sub_group3 VARCHAR(255),
    @sub_group4 VARCHAR(255) = NULL,
    @specials_type INT = NULL,
	@table_name VARCHAR(255)
AS


INSERT INTO wp_fields 
(
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
    loop4,
    product_family,
    data_model,
    property_id,
    sub_group2,
    sub_group3,
    sub_group4,
    specials_type,
	table_name
)
VALUES
(
    @field_name,
    @sql,
    @column_name,
    @column_type,
    @main_group,
    @sub_group,
    @display_name,
    @is_displayed,
    @loop1,
    @loop2,
    @loop3,
    @loop4,
    @product_family,
    @data_model,
    @property_id,
    @sub_group2,
    @sub_group3,
    @sub_group4,
    @specials_type,
	@table_name
)


GO


