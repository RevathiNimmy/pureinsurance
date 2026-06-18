SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_GIS_Scheme_QuoteExpiryDays'
GO

CREATE PROCEDURE spu_Get_GIS_Scheme_QuoteExpiryDays
    @insurance_file_cnt int,
    @gis_scheme_id      int,
    @quote_expiry_days  int OUTPUT
AS
Declare @QuoteExpiryDays as int
Declare @DaysAllowed1 as int		
Declare @DaysAllowed2 as int

IF @gis_scheme_id > 0 
	-- we have a gis_scheme_id, so get it on that basis
	SELECT  @QuoteExpiryDays = quote_expiry_days
	FROM 	GIS_Scheme
	WHERE   gis_scheme_id = @gis_scheme_id
ELSE
   BEGIN		
	-- MAX       - will provide you the maxium number of days allowed 
	-- MIN(ABS(  - will look for the minimun positive value (ignoring the -1)

	SELECT @DaysAllowed1 = MAX(GS.quote_expiry_days), 
		@DaysAllowed2 = MIN(ABS(GS.quote_expiry_days)) 
	FROM	gis_scheme GS,
		gis_QEM_Usage GQU,
		risk_group RG,
		risk_code  RC,
		insurance_file INSF
	WHERE   INSF.insurance_file_cnt = @insurance_file_cnt
	AND	INSF.risk_code_id = RC.risk_code_id
	AND	RC.risk_group_id = RG.risk_group_id
	AND	RG.risk_group_id = GQU.risk_group_id
	AND	GS.gis_scheme_id = GQU.gis_scheme_id

	if @DaysAllowed2 = 0 
		select @QuoteExpiryDays = @DaysAllowed2
	else
		select @QuoteExpiryDays = @DaysAllowed1
   END
	
-- return the value	back
SELECT @quote_expiry_days = @QuoteExpiryDays    

GO