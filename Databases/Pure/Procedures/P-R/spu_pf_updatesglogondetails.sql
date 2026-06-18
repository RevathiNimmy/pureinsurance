DDLDROPPROCEDURE 'spu_pf_updatesglogondetails'
GO
CREATE PROCEDURE spu_pf_updatesglogondetails(
		@pfscheme_id int,
 		@username varchar(255),
		@password varchar(255),
		@clbrokerid varchar(255))
AS
UPDATE pfscheme SET provider_username = @username, provider_password = @password, clbrokerid = @clbrokerid WHERE schemeno = @pfscheme_id
