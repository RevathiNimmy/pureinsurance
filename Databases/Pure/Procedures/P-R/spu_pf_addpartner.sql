DDLDROPPROCEDURE 'spu_pf_addpartner'
GO
CREATE PROCEDURE spu_pf_addpartner(
		@pf_prem_finance_cnt int,
 		@fullname varchar(255),
		@address1 varchar(255),
		@address2 varchar(255),
		@address3 varchar(255),
		@address4 varchar(255),
		@postcode varchar(10))
AS
DECLARE @tmpid INT

SET @tmpid=0

IF NOT EXISTS(SELECT pfprem_finance_cnt FROM  pfpartners WHERE fullname = @fullname and address1 = @address1 and address2 = @address2 and address3 = @address3 and address4 = @address4 and postcode = @postcode)
BEGIN
	INSERT INTO pfpartners (pfprem_finance_cnt, pfpartner_id, fullname, address1, address2, address3, address4, postcode) VALUES (@pf_prem_finance_cnt, @tmpid , @fullname, @address1, @address2, @address3, @address4, @postcode)
END
