SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_cnc_get_fap_amount'
GO
CREATE PROCEDURE spu_cnc_get_fap_amount
    @insurance_file_cnt integer,
    @data_model_code varchar(30)
AS
BEGIN

DECLARE @gis_output_details varchar(60),
        @gis_policy_binder  varchar(60),
        @sql                varchar(500),
        @fap_amount         numeric(19, 4)

/* CREATE TEMPORARY TABLE */
CREATE TABLE #FAP(
    fap_amount numeric(19, 4)
)

SELECT  @gis_output_details = @data_model_code + '_OUTPUT_DETAILS',
        @gis_policy_binder = @data_model_code + '_policy_binder_id'

SELECT  @sql =
'INSERT INTO #FAP ' +
'(fap_amount) ' +
'SELECT rate_amt ' +
'FROM Insurance_File I, ' +
'Gis_Policy_Link L, ' +
@gis_output_details + ' D ' +
'WHERE L.insurance_file_cnt = I.insurance_file_cnt ' +
'AND D.' + @gis_policy_binder + ' = L.gis_policy_link_id ' +
'AND I.insurance_file_cnt = ' + CONVERT(VARCHAR(10), @insurance_file_cnt) + ' ' +
'AND D.description = ''annual premium'''
EXECUTE (@sql)

SELECT  fap_amount
FROM    #FAP

DROP TABLE #FAP

END
GO

