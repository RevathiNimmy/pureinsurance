SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pm_get_eff_id_from_code'
GO


CREATE PROCEDURE spu_pm_get_eff_id_from_code
    @tablename varchar(255),
    @code char(10),
    @effective_date datetime,
    @id int OUTPUT
AS
/*******************************************************************************************************/
/* Selects the ID of the effective record, based on the code, tablename and effective date supplied.   */
/* Return values 0 OK                                                                                  */
/* -100 Error                                                                                          */
/*******************************************************************************************************/
/* Revision Description of Modification 							Date        Who                    */
/* -------- --------------------------- 							----        ---                    */
/* 1.0      Original                    							10/11/1997  RFC                    */
/* 2.0      Updated to use sp_executesql instead of using a table.  14/11/2006  RDT                    */
/*******************************************************************************************************/
BEGIN
    	DECLARE @columnname varchar(255)

   	SELECT @columnname = RTRIM(@tablename) + '_id'

	DECLARE @IntVariable INT
	DECLARE @SQLString NVARCHAR(500)
	DECLARE @ParmDefinition NVARCHAR(500)
	
	/* Build the SQL string once. */
	SET @SQLString = N'SET @LookupID = (SELECT TOP 1 ' + @columnname + ' ' +
	        'FROM ' + @tablename + ' WITH(NOLOCK) ' +
	        'WHERE code = ''' + RTRIM(@code) + ''' ' +
	        'AND effective_date <= ''' + RTRIM(CONVERT(varchar(50), @effective_date)) + ''' ' +
	        'ORDER BY effective_date DESC, ' + @columnname + ' DESC)'
	
	/* Specify the parameter format once. */
	SET @ParmDefinition = N'@LookupID int OUTPUT'
	
	/* Execute the string with the first parameter value. */
	EXECUTE sp_executesql @SQLString, @ParmDefinition,
	                      @LookupID = @id output
	
	IF @id IS NULL BEGIN
        	RETURN -100
    	END ELSE BEGIN
        	RETURN 0
    	END

END
GO



