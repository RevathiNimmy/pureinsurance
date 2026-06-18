SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_SAM_Update_party_Account_Address'
GO

CREATE PROCEDURE spu_SAM_Update_party_Account_Address
	@account_key int,
	@address1 varchar(40),
	@address2 varchar(40),
	@address3 varchar(40),
	@address4 varchar(40),
	@postal_code varchar(20),
	@address_country varchar(20)

AS
DECLARE @Country_id INT
SELECT @Country_id = Country_id FROM country WHERE CODE = @address_country
UPDATE Account
SET

address1=@address1,
address2=@address2,
address3=@address3,
address4=@address4,
postal_code=@postal_code,
address_country=@Country_id

WHERE account_key = @account_key

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
