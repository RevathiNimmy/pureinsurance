
/*************************************************************************/  
/* ERWIN generated add record and generate ID column if required. */  
/*************************************************************************/  
/*************************************************************************/  
/* 1.0 06/08/1997 RFC Original (Based on SP Original) */  
/* ECK 18/05/00 company ID parameter */  
/* eck 150501 remove = NULL and introduce table locking*/  
/*************************************************************************/  
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spe_ACTNumber_add
GO

CREATE PROCEDURE spe_ACTNumber_add
    @actnumber_id int OUTPUT ,
    @actnumber_range_id int ,
    @user_id smallint,
    @company_id smallint
AS

INSERT INTO ACTNumber (
	    actnumber_id ,
		actnumber_range_id ,
		user_id ,
		company_id)

VALUES (1,
	    @actnumber_range_id,
		@user_id,
		@company_id)

SELECT @actnumber_id = @@IDENTITY


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
