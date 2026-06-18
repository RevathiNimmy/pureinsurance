SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SAM_GetQuoteExpiryDate'
GO


CREATE PROCEDURE spu_SAM_GetQuoteExpiryDate
    @Insurance_File_Cnt integer
AS
	select 
		quote_expiry_date
	from 
		insurance_file
		join insurance_file_type on insurance_file.insurance_file_type_id = insurance_file_type.insurance_file_type_id
	where 
		insurance_file_type.code in ('MtaQteTemp','MtaQuote') and
		insurance_file_cnt = @insurance_file_cnt
