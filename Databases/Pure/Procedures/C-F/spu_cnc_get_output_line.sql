EXECUTE DDLDropProcedure 'spu_cnc_get_output_line'
GO

CREATE PROCEDURE spu_cnc_get_output_line
    @insurance_file_cnt integer,
    @data_model_code varchar(30),
    @description varchar(50)
AS
BEGIN

DECLARE @gis_output_details varchar(60),
        @gis_policy_binder  varchar(60),
        @sql                varchar(500),
        @fap_amount         numeric(19, 4)

/* CREATE TEMPORARY TABLE */
CREATE TABLE #FAP(
    rate_amt numeric(19, 4)
)

SELECT  @gis_output_details = @data_model_code + '_OUTPUT_DETAILS',
        @gis_policy_binder = @data_model_code + '_policy_binder_id'

SELECT  @sql =
'INSERT INTO #FAP ' +
'(rate_amt) ' +
'SELECT rate_amt ' +
'FROM Insurance_File I, ' +
'Gis_Policy_Link L, ' +
@gis_output_details + ' D ' +
'WHERE L.insurance_file_cnt = I.insurance_file_cnt ' +
'AND D.' + @gis_policy_binder + ' = L.gis_policy_link_id ' +
'AND I.insurance_file_cnt = ' + CONVERT(VARCHAR(10), @insurance_file_cnt) + ' ' +
'AND D.description = ''' + @description + ''''
EXECUTE (@sql)
--SELECT @sql
SELECT  rate_amt
FROM    #FAP

DROP TABLE #FAP

END

GO