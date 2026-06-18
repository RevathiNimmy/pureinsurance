
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure spu_SAM_User_Authorities_sel 
GO

CREATE PROCEDURE spu_SAM_User_Authorities_sel(
    @user_id int,
    @column_name nvarchar (255))
AS
BEGIN
 DECLARE @sql nvarchar(4000)
 SET @sql = 'DECLARE @temp_column_value varchar(5) '
 SET @sql = @sql + ' SELECT @temp_column_value = ' + LTRIM(RTRIM(@column_name))
 SET @sql = @sql + ' FROM User_Authorities  '
 SET @sql = @sql + ' WHERE user_id = '  + CONVERT(varchar(5),@user_id)

 /** For has_write_off_authority **/
 SET @sql = @sql + ' IF ' + '''' + LTRIM(RTRIM(@column_name)) + '''' + ' = ' + '''' + 'has_write_off_authority' + ''''
 SET @sql = @sql + ' BEGIN SELECT has_write_off_authority, write_off_currency_id, write_off_amount, C.Code FROM User_Authorities UA INNER JOIN Currency C ON UA.write_off_currency_id=C.currency_id WHERE user_id =  '  + CONVERT(varchar(5),@user_id) + ' END'


 /** For has_payments_authority **/
 SET @sql = @sql + ' ELSE IF ' + '''' + LTRIM(RTRIM(@column_name)) + '''' + ' = ' + '''' + 'has_payments_authority' + '''' + ' AND @temp_column_value = 1'
 SET @sql = @sql + ' BEGIN SELECT has_payments_authority, Claims_payments_currency_id payments_currency_id, payments_amount, C.Code  FROM User_Authorities UA INNER JOIN Currency C ON UA.payments_currency_id=C.currency_id WHERE user_id =  '  + CONVERT(varchar(5),@user_id) + ' END'

/** For has_ManualJournal_authority **/  
SET @sql = @sql + ' ELSE IF ' + '''' + LTRIM(RTRIM(@column_name)) + '''' + ' = ' + '''' + 'has_manualjournal_authority' + '''' + ' AND @temp_column_value = 1'  
SET @sql = @sql + ' BEGIN SELECT has_manualjournal_authority, ManualJournal_currency_id, ManualJournal_currency_amount, C.Code  FROM User_Authorities UA INNER JOIN Currency C ON UA.payments_currency_id=C.currency_id WHERE user_id =  '  + CONVERT(varchar(5),@user_id) + ' END'  

 /** For has_claim_Payments_authority **/
 SET @sql = @sql + ' ELSE IF ' + '''' + LTRIM(RTRIM(@column_name)) + '''' + ' = ' + '''' + 'has_claim_Payments_authority' + '''' + ' AND @temp_column_value = 1'
 SET @sql = @sql + ' BEGIN SELECT has_claim_Payments_authority, claims_payments_currency_id, claim_payments_amount, C.Code  FROM User_Authorities UA INNER JOIN Currency C ON UA.Claims_payments_currency_id=C.currency_id WHERE user_id =  '  + CONVERT(varchar(5),@user_id) + ' END'

 /** For has_paynow_write_off_authority **/
 SET @sql = @sql + ' ELSE IF ' + '''' + LTRIM(RTRIM(@column_name)) + '''' + ' = ' + '''' + 'has_paynow_write_off_authority' + '''' + ' AND @temp_column_value = 1'
 SET @sql = @sql + ' BEGIN SELECT has_paynow_write_off_authority, paynow_write_off_currency_id, paynow_write_off_amount, C.Code  FROM User_Authorities UA INNER JOIN Currency C ON UA.payments_currency_id=C.currency_id WHERE user_id =  '  + CONVERT(varchar(
5),@user_id) + ' END'

 /** For IsRecommender **/
 SET @sql = @sql + ' ELSE IF ' + '''' + LTRIM(RTRIM(@column_name)) + '''' + ' = ' + '''' + 'is_recommender' + '''' + ' AND @temp_column_value = 1'
 SET @sql = @sql + ' BEGIN SELECT is_recommender, recommender_currency_id, recommender_currency_amount, C.Code  FROM User_Authorities UA INNER JOIN Currency C ON UA.payments_currency_id=C.currency_id WHERE user_id =  '  + CONVERT(varchar(5),@user_id) + ' 
END'

 /** For allow_reverse_allocations **/
 SET @sql = @sql + ' ELSE IF ' + '''' + LTRIM(RTRIM(@column_name)) + '''' + ' = ' + '''' + 'allow_reverse_allocations' + '''' + ' AND @temp_column_value = 1'
 SET @sql = @sql + ' BEGIN SELECT allow_reverse_allocations, reverse_allocations_days FROM User_Authorities WHERE user_id =  '  + CONVERT(varchar(5),@user_id) + ' END'

  SET @sql = @sql + ' ELSE BEGIN SELECT ' + @column_name + ' FROM User_Authorities WHERE user_id =  '  + CONVERT(varchar(5),@user_id) + ' END'
 EXEC (@sql)
END
