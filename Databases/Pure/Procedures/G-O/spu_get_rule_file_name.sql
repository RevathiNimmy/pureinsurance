SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_rule_file_name'
GO

CREATE PROCEDURE spu_get_rule_file_name 

    @gis_policy_link_id INT,
    @effective_date DATETIME,
    @type VARCHAR(10) = NULL,
    @quote_type TINYINT = 0
AS

DECLARE @sSQL NVARCHAR(MAX)
DECLARE @oBY VARCHAR(255)

IF ISNULL(@type,'') = ''  

    SELECT @type = 'RT'  
  
SELECT @type = RTRIM(@type)  
 
IF @quote_type = 4 
	SET @oBY = 'dre_default DESC'
ELSE IF @quote_type = 2 
	SET @oBY = 'dre_validation DESC'
ELSE IF @quote_type = 1
	SET @oBY = 'dre_quote DESC'
ELSE 
	SET @oBY = 'rst.code DESC'
 
SET @sSQL = 'SELECT rs.file_name, rst.code, rs.dre_executor_url, rs.dre_default_token, rs.dre_default, ' +
   ' rs.dre_validation, rs.dre_quote, rs.post_dre_script, rs.pre_pre_rule,rs.pre_ruleset_effective_date,rs.pre_child_ruleset_effectivedate,rs.pre_version ' +
   ' FROM gis_policy_link gpl (nolock) ' + 
   ' INNER JOIN  risk r (nolock) ON gpl.risk_id = r.risk_cnt  ' + 
   ' INNER JOIN  risk_type_rule_set rs   ON R.risk_type_id = RS.risk_type_id ' + 
   ' INNER JOIN risk_type_rule_set_type rst ON rs.risk_type_rule_set_type_id = rst.risk_type_rule_set_type_id ' +  
 
   ' WHERE gpl.gis_policy_link_id = ' + CAST(@gis_policy_link_id AS VARCHAR) +  
   ' AND rs.is_deleted = 0  AND rs.effective_date <= ''' +  CAST(@effective_date AS VARCHAR) + ''' AND rs.live = 1  AND rs.type = ''' + CAST(@type AS VARCHAR) + ''' ORDER BY ' + @oBY + ' , rs.effective_date DESC'  
   
EXEC sp_executeSQL @sSQL

