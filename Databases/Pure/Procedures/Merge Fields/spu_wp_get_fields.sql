SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_get_fields'
GO

CREATE PROCEDURE spu_wp_get_fields
    @source_id INT,
    @product_family INT = NULL
AS

SELECT
    wpf.field_name,
    wpf.sql,
    wpf.column_name,
    wpf.column_type,
    wpf.main_group,
    wpf.sub_group,
    wpf.display_name,
    wpf.is_displayed,
    wpf.loop1,
    wpf.loop2,
    wpf.loop3,
    wpf.loop4,
    wpf.product_family,
    wpf.data_model,
    wpf.property_id,
    wpf.sub_group2,
    wpf.sub_group3,
    wpf.sub_group4,
    isnull(wpf.specials_type,0),
	wpf.Table_Name 
FROM wp_fields wpf
LEFT JOIN hidden_options ho
    ON ho.option_number = wpf.hidden_option_number
    AND ho.branch_id = @source_id
WHERE 
(
    (   
        ISNULL(@product_family, 0) = 0
        AND
        wpf.product_family <> 16 -- The Value for pmePFSwift CONSTANT in gPMConstants
    )
    OR
    (
        ISNULL(@product_family, 0) <> 0
        AND
        product_family = @product_family  -- filter by product_family
    )
)
AND 
(
    wpf.hidden_option_number IS NULL
    OR
    (
        ISNULL(wpf.hidden_option_number,0) <> 0
        AND 
        ISNULL(wpf.required_option_value,0) = ISNULL(ho.Value,0)
    )
)
ORDER BY
    main_group,
    sub_group,
    CAST(sub_group2 as VARBINARY(255)),
    sub_group3,
    sub_group4,
    display_name
        

GO
