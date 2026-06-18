EXECUTE DDLDropProcedure 'spu_cnc_get_addons'
GO

CREATE PROCEDURE spu_cnc_get_addons
    @insurance_file_cnt INT,
    @gis_scheme_id INT,
    @premium numeric(19, 4),
    @out_addon_premium numeric(19, 4) OUTPUT
AS
BEGIN

DECLARE @addon_premium numeric(19, 4)
DECLARE @total_addon_premium numeric(19, 4)

DECLARE @gis_data_model_code varchar (10)
DECLARE @sql varchar(1250)
DECLARE @Fee_Amount numeric(19, 4)
DECLARE @Fee_Percentage numeric(19, 4)

DECLARE @gis_policy_link_id INT
DECLARE @Output_id INT

/* Default Test Data */
--SELECT @insurance_file_cnt = 904
--SELECT @gis_scheme_id = 550
--SELECT @premium = 68.25

/* Get the gis_policy_link_id and the datamodel code */
SELECT @gis_policy_link_id = l.gis_policy_link_id, @gis_data_model_code = d.code
    FROM gis_policy_link l,
         gis_data_model d
    WHERE insurance_file_cnt = @insurance_file_cnt
    AND l.gis_data_model_id = d.gis_data_model_id

/* Create a temporary table for the addons */
CREATE TABLE #TempAddon (
    Fee_Amount numeric(19, 4) NULL,
    Fee_Percentage numeric(19, 4) NULL)
--SELECT @gis_data_model_code

SELECT @gis_data_model_code =  RTRIM(@gis_data_model_code)
SELECT @sql = 'INSERT INTO #TempAddon SELECT fee_amount, fee_percentage FROM '
SELECT @sql = @sql + @gis_data_model_code + '_Output_Fees WHERE '
SELECT @sql = @sql + @gis_data_model_code + '_Output_id = (SELECT  MAX(' + @gis_data_model_code
SELECT @sql = @sql + '_Output_id) FROM ' + @gis_data_model_code + '_Output'
SELECT @sql = @sql + ' WHERE scheme_id = ' + CONVERT(varchar,@gis_scheme_id) + ' AND ' + @gis_data_model_code
SELECT @sql = @sql + '_Policy_binder_id = ' + CONVERT(varchar,@gis_policy_link_id) + ') AND '
SELECT @sql = @sql + @gis_data_model_code + '_Policy_binder_id = ' + CONVERT(varchar,@gis_policy_link_id)
--SELECT @sql

exec (@sql)
/*
INSERT INTO #TempAddon 
SELECT fee_amount, fee_percentage
 FROM CNBIKE_Output_Fees
 WHERE CNBIKE_Output_id = 
    (SELECT  MAX(CNBIKE_Output_id) 
    FROM CNBIKE_Output
    WHERE scheme_id = @gis_scheme_id
    AND CNBIKE_Policy_binder_id = @gis_policy_link_id)
 AND CNBIKE_Policy_binder_id = @gis_policy_link_id
*/
/* Loop around the addons to get a total addon value */
 SELECT @total_addon_premium = 0

DECLARE add_cursor CURSOR FAST_FORWARD FOR
    SELECT fee_amount, fee_percentage
    FROM #TempAddon

OPEN add_cursor
FETCH NEXT FROM add_cursor INTO @fee_amount, @fee_percentage

WHILE @@FETCH_STATUS = 0 BEGIN
    SELECT @fee_amount =  ISNULL(@fee_amount,0)
    SELECT @fee_percentage =  ISNULL(@fee_percentage,0)
    
    SELECT @addon_premium = @premium * (@fee_percentage/100)
    SELECT @total_addon_premium =  @total_addon_premium + @addon_premium
    
    SELECT @total_addon_premium =  @total_addon_premium + @fee_amount 
--    SELECT @fee_amount, @fee_percentage
    FETCH NEXT FROM add_cursor INTO @fee_amount, @fee_percentage
END

SELECT @out_addon_premium = @total_addon_premium

 
CLOSE add_cursor
DEALLOCATE add_cursor

DROP TABLE #TempAddon

END
GO
