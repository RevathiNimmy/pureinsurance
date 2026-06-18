SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_Sum_Insured'
GO
/*******************************************************************************************************/
/* spu_SAM_Get_Sum_Insured - Generates the xml node for a Sums Insured PB property                     */
/*                                                                                                     */
/* RDT 14/12/2005                                                                                       */
/*******************************************************************************************************/
CREATE PROCEDURE spu_SAM_Get_Sum_Insured
    @gis_datamodel_code varchar(30) = NULL,
    @gis_policy_link_id int = NULL,
    @sum_insured_type_id int,
    @tagname varchar(255) = NULL

AS
BEGIN

DECLARE @SQL varchar(5000)

SELECT @gis_datamodel_code = RTRIM(@gis_datamodel_code)

SELECT @SQL = 'SELECT DISTINCT 0 US , ' + @tagname + '.rate RATE, ' + @tagname + '.premium PREMIUM, c.SUM_INSURED_TYPE, '
SELECT @SQL = @SQL + 'c.DESCRIPTION, c.REFERENCE, c.SUM_INSURED, c.DATE_ADDED, '
SELECT @SQL = @SQL + 'c.DATE_DELETED, c.IS_VALUATION_REQUIRED, c.VALUATION_DATE '
SELECT @SQL = @SQL + 'FROM ' + @gis_datamodel_code + '_sum_insured ' + @tagname + ' JOIN '
SELECT @SQL = @SQL + 	'(SELECT SUM_INSURED.' + @gis_datamodel_code + '_Policy_binder_id, sequence_id, '
SELECT @SQL = @SQL + 	'0 US , sum_insured_type_id SUM_INSURED_TYPE, description DESCRIPTION, '
SELECT @SQL = @SQL + 	'reference REFERENCE, sum_insured SUM_INSURED, date_added DATE_ADDED, '
SELECT @SQL = @SQL + 	'date_deleted DATE_DELETED, is_valuation_required IS_VALUATION_REQUIRED, '
SELECT @SQL = @SQL + 	'valuation_date VALUATION_DATE '
SELECT @SQL = @SQL + 	'FROM ' + @gis_datamodel_code + '_sum_insured SUM_INSURED '
SELECT @SQL = @SQL + 	'INNER JOIN ' + @gis_datamodel_code + '_Policy_binder pb '
SELECT @SQL = @SQL + 	'ON SUM_INSURED.' + @gis_datamodel_code + '_Policy_binder_id = pb.' + @gis_datamodel_code + '_Policy_binder_id '
SELECT @SQL = @SQL + 	'WHERE pb.gis_policy_link_id = ' + convert(varchar(10),@gis_policy_link_id) + ' AND sum_insured_type_id = ' + convert(varchar(10),@sum_insured_type_id) + ') c '
SELECT @SQL = @SQL + 	'ON ' + @tagname + '.' + @gis_datamodel_code + '_Policy_binder_id = c.' + @gis_datamodel_code + '_Policy_binder_id '
SELECT @SQL = @SQL + 	'WHERE sum_insured_type_id = ' + convert(varchar(10),@sum_insured_type_id) + '  AND ' + @tagname + '.rate IS NOT NULL FOR XML AUTO'

/* This will produce a statement of the following structure :-

SELECT DISTINCT 0 US , 
HIGHVAL.rate RATE, 
HIGHVAL.premium PREMIUM, 
c.SUM_INSURED_TYPE, 
c.DESCRIPTION, 
c.REFERENCE, 
c.SUM_INSURED, 
c.DATE_ADDED, 
c.DATE_DELETED, 
c.IS_VALUATION_REQUIRED, 
c.VALUATION_DATE 
FROM HOMEOWNERS_sum_insured HIGHVAL 
JOIN 
       (SELECT SUM_INSURED.HOMEOWNERS_Policy_binder_id, 
	sequence_id, 
	0 US, 
	sum_insured_type_id SUM_INSURED_TYPE, 
	description DESCRIPTION, 
	reference REFERENCE, 
	sum_insured SUM_INSURED, 
	date_added DATE_ADDED, 
	date_deleted DATE_DELETED, 
	is_valuation_required IS_VALUATION_REQUIRED, 
	valuation_date VALUATION_DATE 
	FROM HOMEOWNERS_sum_insured SUM_INSURED 
	INNER JOIN HOMEOWNERS_Policy_binder pb 
	ON SUM_INSURED.HOMEOWNERS_Policy_binder_id = pb.HOMEOWNERS_Policy_binder_id 
	WHERE pb.gis_policy_link_id = 1341 AND 
	sum_insured_type_id = 1) c 
ON HIGHVAL.HOMEOWNERS_Policy_binder_id = c.HOMEOWNERS_Policy_binder_id 
WHERE sum_insured_type_id = 1  AND 
HIGHVAL.rate IS NOT NULL 
FOR XML AUTO

*/

exec(@SQL)
--SELECT @SQL 

END