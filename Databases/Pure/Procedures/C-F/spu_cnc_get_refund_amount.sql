SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_cnc_get_refund_amount'
GO
CREATE PROCEDURE spu_cnc_get_refund_amount
    @insurance_file_cnt int,
    @data_model_code varchar(30)
AS
BEGIN

EXECUTE DDLDropTable '#Refund'

/* CREATE TEMPORARY TABLE */
CREATE TABLE #Refund(
    nb_amount numeric(9, 4),
    refund_amount numeric(9, 4))

DECLARE     @gis_output_details    varchar(60),
            @gis_policy_binder     varchar(60),
            @sql                   varchar(500),
            @refund_amount         numeric (9,4),
            @nb_amount             numeric (9,4)

SELECT  @gis_output_details = @data_model_code + '_OUTPUT_DETAILS',
        @gis_policy_binder = @data_model_code + '_policy_binder_id'

SELECT  @sql =
'INSERT INTO #Refund ' +
'(nb_amount) ' +
'SELECT rate_amt ' +
'FROM Insurance_File I, ' +
'Gis_Policy_Link L, ' +
@gis_output_details + ' D ' +
'WHERE L.insurance_file_cnt = I.insurance_file_cnt ' +
'AND D.' + @gis_policy_binder + ' = L.gis_policy_link_id ' +
'AND I.insurance_file_cnt = ' + CONVERT(VARCHAR(10), @insurance_file_cnt) + ' ' +
'AND D.description = ''Annual Premium'''

EXECUTE (@sql)

SELECT @sql =
'UPDATE #Refund ' +
'SET refund_amount = ' +
'rate_amt ' +
'FROM Insurance_File I, ' +
'Gis_Policy_Link L, ' +
@gis_output_details + ' D ' +
'WHERE L.insurance_file_cnt = I.insurance_file_cnt ' +
'AND D.' + @gis_policy_binder + ' = L.gis_policy_link_id ' +
'AND I.insurance_file_cnt = ' + CONVERT(VARCHAR(10), @insurance_file_cnt) + ' ' +
'AND D.description = ''Pro Rata Refund'' '

EXECUTE (@sql)

SELECT  isnull(nb_amount,0) as nb_amount,
        isnull(refund_amount,0) as refund_amount
FROM    #Refund

DROP TABLE #Refund

END
GO
