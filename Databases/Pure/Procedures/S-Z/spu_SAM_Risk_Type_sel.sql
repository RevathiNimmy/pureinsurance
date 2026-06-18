SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_SAM_Risk_Type_sel'
GO
--Start (Sriram P)Tech Spec - UIIC - Product Risk Maintenance (6.2)
CREATE PROCEDURE spu_SAM_Risk_Type_sel(
    @risk_type_id int,
    @column_name varchar (255))
AS  
BEGIN
	DECLARE @sql varchar(255)

	If UPPER(LTRIM(RTRIM(@column_name))) = 'CODE' OR UPPER(LTRIM(RTRIM(@column_name))) = 'DESCRIPTION'
	BEGIN
		SET @column_name = 'rt.' + LTRIM(RTRIM(@column_name))  
	END

	SET @sql = 'SELECT ' + LTRIM(RTRIM(@column_name))  
	  
	SET @sql = @sql + ' FROM Risk_Type rt'
	SET @sql = @sql + ' LEFT JOIN document_template dt1 ON dt1.document_template_id = rt.header_clause_id'
	SET @sql = @sql + ' LEFT JOIN document_template dt2 ON dt2.document_template_id = rt.trailer_clause_id'
	SET @sql = @sql + ' WHERE risk_type_id = '  + CONVERT(varchar(5),@risk_type_id) 
	EXEC (@sql)
END
--End (Sriram P)Tech Spec - UIIC - Product Risk Maintenance (6.2)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
